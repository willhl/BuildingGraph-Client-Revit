
# BuildingGraph-Client-Revit

BuildingGraph-Client-Revit is a C# client for Revit and Dynamo to publish Revit models to a Neo4j database. Integrations included in this repository:

 - Revit > Neo4j (bolt)
 - Dynamo <> Building Graph Server (GraphQL)
 - WIP: Revit <> Building Graph Server (GraphQL)

Also included in other repositories:
[BuildingGraph-Integration-RhinoGrasshopper](https://github.com/willhl/BuildingGraph-Integration-RhinoGrasshopper)
 - Rhino Grasshopper <> Neo4j (bolt)
 - Rhino Grasshopper <> Building Graph Server (GraphQL)

All of these repositories include dependencies to the core client libraries for Neo4j and GraphQL: 
[BuildingGraph-Client](https://github.com/willhl/BuildingGraph-Client)


## Firstly, you'll need a Neo4j Database

 There are various options for this, for local development a docker image works well. If you're using docker use these command to quickly get up and running:
   
    docker pull neo4j:4.0
    docker run -p 7474:7474 -p 7687:7687 -p 7473:7473  neo4j:4.0

This will start the latest community version of Neo4j in a new container, note that once this container is deleted the database will be lost as well. To persist data over container instances refer to the full documentation on running Neo4j in docker: https://neo4j.com/developer/docker/.

If you're not using docker have a look at https://neo4j.com/product/ for other installation options.

Once up and running point your browser at the IP or host name used by docker and the configured http/https port (usually 7474 or 7473, e.g. https://localhost:7473), here you can access the Neo4j Browser to configure the database username and password.

# Using the Revit <> Neo4j (bolt) Integration

BuildingGraph.Integration.RevitUI a C# .NET addin for Revit that extracts Spaces, Mechanical, Electrical and Plumbing systems from a Revit model and publishes them to a Graph Database. The current implementation supports writing to a Neo4j graph database using the bolt protocol.

  - Publishes all ducts, pipes, cables trays and electrical circuits and their connections.
  - Includes all Revit element parameter values on their respective Nodes and Edges.
  - Includes relationships to levels, spaces, systems and element types.
  - Simplified space bounding surface geometry feature extraction (currently only area and facing vector).


## Building and Installation

Clone this repository to your local system (including submodules): 

    $ git clone --recurse-submodules https://github.com/willhl/BuildingGraph-Client-Revit.git

Or, if you have already cloned this repository, you can grab the submodules with this command:

    $ git submodule update --init --recursive

Then open BuildingGraph.Integration.Revit.sln in Visual Studio. Just set BuildingGraph.Integration.RevitUI as the startup project and set the solution configuration to Debug201x and x64, and hit Run. This will build the projects, copy the .addin file to the Revit plugins directory and open Revit.

It assumes you have Revit installed, and the Revit API assemblies are in "C:\Program Files\Autodesk\Revit 20xx", if this is not the case on your system you may need to edit BuildConfigurations\Imports.targets with the location of your Revit API assemblies.

The post build events will copy the .addin manifest and built assemblies to your Revit addins folder, again if this is not "C:\ProgramData\Autodesk\Revit\Addins\20xx" you can edit BuildConfigurations\LocalDebugAddin.targets with the correct addins location for your system.

### Publishing Revit models to Neo4j

Following the above steps there should be a "HL Apps" panel in the Revit Add-ins ribbon tab. Select HLApps >  "Publish to graph", here you can enter the host name, port and credentials for your Neo4j database. Select the parse options you need then hit "publish" to push your model to the graph database, then you can begin you exploration into the graph.


## Example Graph Data Queries
Here are some example Cypher queries you can run against your graph data once successfully published from Revit. Some of these examples use a filter for the space number, you'll need to change these values to actual spaces in your model.

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

## Cypher Query to Power BI
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

This sample is not a complete implementation and there are many opportunities for improvement. 

### MEP
 - Use MEPSection data for more accurate lengths, and inclusion of other calculation data such as reynolds numbers.
 - Store parameter data in the graph with the same units as the Revit model settings.

### Geometry
 - Remove duplicate surfaces, currently surfaces are created for both directions between spaces.
 - Optimise the geometry extraction algorithm (large Revit models can take a very long time to process)
 - Include more geometric data, such as overall width, height, maybe even a polygon of the outline shape.
 - Make use of native geometric data types of the graph database

### Graph
 - Read data back from the graph database and update Revit model parameters.
 - Allow for versioning when publishing multiple versions of the same model, currently each publish creates a new graph (no merge yet).
 - Optimise the graph write stage, currently every single node and relationship change is a single transaction.

### General
 - Add progress bars to inform status of long running processes
 - Persist user settings
 - Error handling and logging
 - Set window owner to Revit window handle


# Using the Dynamo <> Building Graph Server (GraphQL) Integration

BuildingGraph.Integration.Dynamo C# .NET ZeroTouch  package for Dynamo Sandbox and Dynamo Revit. It provides the following nodes:

 - Building Graph Client - Use this to provide the url to your GraphQL API
 - Queries.BGGraphQLQuery - Use this to supply queries to the Buidling Graph Client
 - BGNode.FromNameAndId - Use this to initially create a node with a given Name and/or Id
 - Mutations.CreateNode - Use this to push your node to the database together with it parameters
 - Mutations.UpdateNode - Use this to update the parameters on a node
 - Mutations.RelateNode- Use this to relate two nodes together

For example Dynamo graphs using these node see the [ examples repository.](https://github.com/willhl/BuildingGraph-Client-Examples)

## Building and Installation

Follow the steps in the above "Using the Revit <> Neo4j (bolt) Integration" section to clone the repository and open the solution file.  

  1. Build the BuildingGraph.Integration.Dynamo project
  2. In Dynamo (Sandbox or Revit), goto File > Import Library
  3. Find the project build directory (usually BuildingGraph-Client-Revit\BuildingGraph.Integration.Dynamo\bin\Debug) and select BuildingGraph.Integration.Dynamo.dll


# Licence
This code is licensed under the terms of the MIT License. Please see the LICENSE file for full details.
