# GraphData-MEP

GraphData-MEP is a C# .NET addin for Revit.

It extracts Spaces, Mechanical, Electrical and Plumbing systems from a Revit model and publishes them to a Graph Database. The current implementation supports writing to a Neo4j graph database.


  - Publishes all ducts, pipes, cables trays and electrical circuits and their connections.
  - Includes all Revit element parameter values on their respective Nodes and Edges.
  - Includes relationships to levels, spaces, systems and element types.
  - Simplified space bounding surface geometry feature extraction (currently only area and facing vector).

## Graph Database
---
You'll need access to a Neo4j database. There are various options for this, for local development a docker image works well. If you're using docker use these command to quickly get up and running:
   
    docker pull neo4j:latest
    docker run -p 7474:7474 -p 7687:7687 -p 7473:7473  neo4j:latest

This will start the latest community version of Neo4j in a new container, note that once this container is deleted the database will be lost as well. To persist data over container instances refer to the full documentation on running Neo4j in docker: https://neo4j.com/developer/docker/.

If you're not using docker have a look at https://neo4j.com/product/ for other installation options.

Once up and running point your browser at the IP or host name used by docker and the configured http/https port (usually 7474 or 7473, e.g. https://localhost:7473), here you can access the Neo4j Browser to configure the database username and password.

## Building and Installation
---

Fork then clone this repository to your local system, then open RevitToGraphDB.sln in Visual Studio.

There should be no installation steps required as the Visual Studio solution is pre-configured to build and debug for Revit 2018 or 2019. Just set HLApps.Revit.Graph.UIAddin as the startup project and set the solution configuration to either Debug2018 (for Revit 2018) or Debug2019 (for Revit 2019), and hit Run.

It assumes you have Revit 2018 and/or 2019 installed, and the Revit API assemblies are in "C:\Program Files\Autodesk\Revit 201x", if this is not the case on your system you may need to edit HLApps.RevitBuildConfigurations\Imports.targets with the location of your Revit API assemblies.

The post build events will copy the .addin manifest and built assemblies to your Revit addins folder, again if this is not "C:\ProgramData\Autodesk\Revit\Addins\201x" you can edit HLApps.RevitBuildConfigurations\LocalDebugAddin.targets with the correct addins location for your system.

## Example Graph Data Queries
---
Here are some example cypher queries you can run against your graph data once succesfully published from Revit. Some of these examples use a filter for the space number, you'll need to change these values to actual spaces in your model.

### Electrical
Find all DB Panels and Circuits:

    MATCH (n:DBPanel)-[r:ELECTRICAL_FLOW_TO*]->(s:Circuit)-[z:ELECTRICAL_FLOW_TO]->(b) RETURN n,s,b LIMIT 300
Find all Cable Tray routes between two spaces:

    MATCH a=(rv:RevitModel {`Project Number`:'Project Number'})-[:IS_IN]-()-[:REALIZED_BY]-(s:CableTray )-[:IS_IN_SPACE]->(sp:Space {Number:"01-12"}) 
    MATCH p=(s)-[:CABLETRAY_FLOW_TO*]-(i:CableTray)-[:IS_IN_SPACE]->(sc:Space {Number:"01-27"}) RETURN a,p
    
### Mechanical
Find all ducts between two spaces:

    MATCH p=(n:Space {Number:"01-01"})<-[:FLOWS_TO_SPACE]-(:Terminal)-[:AIR_FLOW_TO*1..20]-(:Terminal)-[:FLOWS_TO_SPACE]->(:Space {Number:"01-02"}) RETURN p LIMIT 30
Find duct routes back from a space to the base equipment:

    MATCH (sp:Space {Number:'01-01'}) MATCH pai=(sp)<-[:FLOWS_TO_SPACE]-(ain:Terminal)<-[ai:AIR_FLOW_TO*]-(e:Equipment)
    OPTIONAL MATCH pao=(sp)-[:FLOWS_TO_SPACE]->(aout:Terminal)-[ao:AIR_FLOW_TO*]->(e:Equipment)
    RETURN pai,pao


Find all air path flow rates and lengths from terminals and sum by space name:

    MATCH (r:RevitModel)<-[:IS_IN]-(ModelElement)<-[:REALIZED_BY]-(sp:Space) 
	MATCH pai=(sp)<-[:FLOWS_TO_SPACE]-(ain:Terminal)<-[ai:AIR_FLOW_TO*]-(e:Equipment) 
	OPTIONAL MATCH pao=(sp)-[:FLOWS_TO_SPACE]->(aout:Terminal)-[ao:AIR_FLOW_TO*]->(e:Equipment) 
	UNWIND ai as air
	UNWIND ao as aor
	RETURN sp.Name, sum(ain.Flow) as AirIn, sum(aout.Flow) as AirOut,sum(air.Length),sum(aor.Length), e.Name


### Spaces

Find all spaces:

    MATCH (n:Space) RETURN n
Find all space names and numbers, order by number:

    MATCH (n:Space) RETURN n.Number as Number, n.Name as Name ORDER BY Number
Boundaries between two spaces:

    MATCH (n:Space {Number:"01-08"})-[:BOUNDED_BY]->(s:Section)-[:BOUNDED_BY]->(p:Space {Number:"01-10"})
    MATCH (s)-[:IS_ON]-(m)-[:IS_OF]-(t)
    RETURN n,p,s,m,t LIMIT 30

Boundaries between a space and the outside:

    MATCH (n:Space {Number:"01-11"})-[:BOUNDED_BY]-(s:Section)-[:BOUNDED_BY]-(p:Environment)
    MATCH (s)-[:IS_ON]-(m)
    RETURN n,p,s,m LIMIT 30


To calculate fabric heat loss through all surfaces run the following scripts in order.
1 - Set outside and inside temperatures:

    MATCH (e:Environment {Name:'OutsideBoundary'}) SET e.DesignTemp = 18
    MATCH (s:Space) SET s.DesignTemp=21

2 - Set temperature delta across surface:

    MATCH (m)<-[:BOUNDED_BY]-(s:Section)<-[:BOUNDED_BY]-(e:Space) WHERE EXISTS (m.DesignTemp) and EXISTS (e.DesignTemp)
    set s.DesignTempDelta = e.DesignTemp - m.DesignTemp return s
  
3 - Set fabric loss through surface:

    MATCH (s:Section)<-[r:BOUNDED_BY]-(sp:Space) WHERE EXISTS (s.DesignTempDelta) MATCH (s)-[:IS_ON]->()-[:IS_OF]->(t)
    SET s.FabricLoss = (s.DesignTempDelta * r.Area * t.`Heat Transfer Coefficient (U)`) RETURN s.FabricLoss

4 - Find total fabric loss per space:

    MATCH (s:Section)<-[r:BOUNDED_BY]-(sp:Space) where EXISTS (s.DesignTempDelta) RETURN sp.Name as Name, sum(s.FabricLoss) ORDER BY Name

## Cypher to Power BI
---
Open CypherToPowerBI.pbix, or, use the following Power Query (paste into the Advanced Editor window) to create a table in Power BI from a cypher query:

    let
        Source = 
            Json.Document(
                Web.Contents(neo4jHost & ":" & Number.ToText(neo4jPort) & "/db/data/transaction/commit",
                [
                    Headers=[Authorization="Basic " & neo4jAuth],
                    Content=Text.ToBinary("{""statements"" : [ {
                            ""statement"" : """ & CypherQuery & """} ]
                            }")
                ])),
        results = Source[results],
        results1 = results{0},
        data = results1[data],
        columns = results1[columns],
    
        #"Converted to Table" = Table.FromList(data, Splitter.SplitByNothing(), null, null, ExtraValues.Error),
        #"Expanded Column1" = Table.ExpandRecordColumn(#"Converted to Table", "Column1", {"row"}, {"Column1.row"}),
        #"Column1 row" = #"Expanded Column1"[Column1.row],
    
        ExpandAllQuandl = Table.FromRows(#"Column1 row", columns)
    in
        ExpandAllQuandl

These are the parameters for the power query:
 - neo4jHost = host name with http protocol (e.g. http:\\\localhost)
 - neo4jPort = Neo4j http port (7474)
 - neo4jAuth = Base64 encoded username:password, (e.g. bmVvNGo6cGFzc3dvcmQ=)
 - CypherQuery = The cypher query (must return fields only, and not nodes)

## Todo
---
This sample is not a complete implementation and there are many opportunities for improvement. 

### MEP
 - Use MEPSection data for more accurate lengths, and inclusion of other calculation data such as reynolds numbers.
 - Store parameter data in the graph with the same untis as the Revit model settings.

### Geometry
 - Remove duplicate surfaces, currently surfaces are created for both directions between spaces.
 - Optimise the geometry extraction algorithm (large Revit models can take a very long time to process)
 - Include more geometric data, such as overall width, hight, maybe even a polygon of the outline shape.
 - Make use of native geometric data types of the graph database

### Graph
 - Read data back from the graph database and update Revit model parameters.
 - Allow for versioning when publishing multiple versions of the same model, currently each publish creates a new graph (no merge yet).
 - Optimise the graph write stage, currently every single node and relatonship change is a single transaction.

### General
 - Add progress bars to inform status of long running processes
 - Persist user settings
 - Error handling and logging
 - Set window owner to Revit window handle

# Licence
___
This sample is licensed under the terms of the MIT License. Please see the LICENSE file for full details.
