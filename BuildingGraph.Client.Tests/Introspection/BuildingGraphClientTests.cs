using Microsoft.VisualStudio.TestTools.UnitTesting;
using HLApps.Cloud.BuildingGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using BuildingGraph.Client;

namespace BuildingGraph.Client.Tests
{
    [TestClass()]
    public class BuildingGraphClientTests
    {
    

        [TestMethod()]
        public void ElecticalService_AddSocketsToSpaces()
        {
            var client = new BuildingGraphClient(@"http://localhost:4001/graphql");
            var project = "Project Graph";
            var building = "Dynamo Tower";

            var eleQuery = @"query ($projectName:String, $buidingName: String, $rootDbName: String) {
  Space(filter: {BaseLevel: {Building: {Name: $buidingName, Projects_some: {Name: $projectName}}}}) {
    Id
    Name    
    Number
    Number_of_230V_Single_Sockets_Non_Essential
    Number_of_230V_Twin_Sockets_Non_Essential
    ElectricalOutlets{
      Name
      Is_Essential
      Number_Of_Outlets
    }
    BaseLevel{
      Id
      Abbreviation
    }
    DBPanelElements{
      Name
      Id
      OutgoingCircuits{
        Name
      	Id
      }
    }
  }
  rootPanel : DBPanel (Name:$rootDbName){
    Id
    OutgoingCircuits
    {
      Id
      Name
      Number
      levelPanels : Panels{
        Id
        Name
        Level{
          Id
          Name
          Abbreviation
        }
        OutgoingCircuits{
          Id
          Name
          Number
          spacePanels : Panels{
            Id
            Name
            Number_of_Ways
            OutgoingCircuits{
              Id
              Name
              Number
            }
            Space{
              Id
              Name
              Number
            }
          }
        }
      }
    }
  }
}

";

            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("projectName", project);
            vars.Add("buidingName", building);
            vars.Add("rootDbName", "DB-CORE1");
            var result = client.ExecuteQuery(eleQuery, vars);

            //if (result is Exception) return result;
            //if (result == null) return "Error querying API";
            //if (result.Space == null) return "No spaces found";

            //find root db and build connection tree
            var rootPanelId = string.Empty;
            var levelPanels = new Dictionary<string, PendingNode>();

            if (result.rootPanel != null && result.rootPanel.Count > 0)
            {
                var rootPanel = result.rootPanel[0];
                rootPanelId = rootPanel.Id.Value;
                foreach (var levelCircuit in rootPanel.OutgoingCircuits)
                {
                    //assum each circuit from root to level goes to only one level
                    var levelPanel = levelCircuit.levelPanels[0];
                    string levelId = levelPanel.Level.Id.Value;
                    levelPanels.Add(levelId, new PendingNode("Level", levelId));
                }
            }

            PendingNode rootDbNode = null;
            if (string.IsNullOrEmpty(rootPanelId)) {
                var rootDbVars = new Dictionary<string, object>();
                rootDbVars.Add("Name", "DB-CORE1");
                rootDbNode = client.Push("DBPanel", rootDbVars);
                client.Commit();
            }
            else
            {
                rootDbNode = new PendingNode("DBPanel", rootPanelId);
            }

            foreach (var space in result.Space)
            {
                var no_230SingleNonEssReq = space.Number_of_230V_Single_Sockets_Non_Essential.Value;
                var no_230TwinNonEssReg = space.Number_of_230V_Twin_Sockets_Non_Essential.Value;
                var no_230SingleNonEssCount = 0;
                var no_230TwinNonEssCount = 0;


                if (space.ElectricalOutlets != null)
                {
                    foreach (var elecLoad in space.ElectricalOutlets)
                    {
                        if (elecLoad.Is_Essential == null || !elecLoad.Is_Essential.Value)
                        {
                            if (elecLoad.Number_Of_Outlets != null && elecLoad.Number_Of_Outlets.Value == 1)
                            {
                                no_230SingleNonEssCount = ++no_230SingleNonEssCount;
                            }
                            else if (elecLoad.Number_Of_Outlets != null && elecLoad.Number_Of_Outlets.Value == 2)
                            {
                                no_230TwinNonEssCount = ++no_230TwinNonEssCount;
                            }
                        }
                    }
                }
                var no_230SingleNonEssDelta = no_230SingleNonEssReq - no_230SingleNonEssCount;
                var no_230TwinNonEssDelta = no_230TwinNonEssReg - no_230TwinNonEssCount;

                var newNodesCirc1 = new List<PendingNode>();
                var newNodesCirc2 = new List<PendingNode>();
                if (no_230SingleNonEssDelta > 0)
                {
                    for (int skc = 0; skc < no_230SingleNonEssDelta; skc++)
                    {
                        var sckTwinVars = new Dictionary<string, object>();
                        sckTwinVars.Add("Name", "Single Non-Essential Socket");
                        sckTwinVars.Add("Is_Essential", false);
                        sckTwinVars.Add("Number_Of_Outlets", 1);
                        sckTwinVars.Add("Apparent_Load", 5);
                        sckTwinVars.Add("Diversity", 0.1);
                        newNodesCirc1.Add(client.Push("ElectricalOutlet", sckTwinVars));
                    }

                    for (int skc = 0; skc < no_230TwinNonEssReg; skc++)
                    {
                        var sckTwinVars = new Dictionary<string, object>();
                        sckTwinVars.Add("Name", "Twin Non-Essential Socket");
                        sckTwinVars.Add("Is_Essential", false);
                        sckTwinVars.Add("Number_Of_Outlets", 2);
                        sckTwinVars.Add("Apparent_Load", 10);
                        sckTwinVars.Add("Diversity", 0.1);
                        newNodesCirc2.Add(client.Push("ElectricalOutlet", sckTwinVars));
                    }
                }

                client.Commit();


                if (newNodesCirc2.Count <= 0 && newNodesCirc2.Count <= 0) return;

                var spaceNode = new PendingNode("Space", space.Id.Value);
                PendingNode spacePanelNode = null;
                PendingNode twinCircNode = null;
                PendingNode singleCircNode = null;
                if (space.DBPanelElements != null && space.DBPanelElements.Count > 1)
                {
                    var spacePanel = space.DBPanelElements[0];
                    spacePanelNode = new PendingNode("DBPanel", spacePanel.Id.Value);

                    foreach (var circ in spacePanel.OutgoingCircuits)
                    {
                        if (circ.Name.Value == "Twin") twinCircNode = new PendingNode("Circuit", circ.Id.Value);
                        if (circ.Name.Value == "Single") singleCircNode = new PendingNode("Circuit", circ.Id.Value);
                    }
                }
                else
                {
                    var spaceDbVars = new Dictionary<string, object>();
                    spaceDbVars.Add("Name", "DB-" + space.Number.Value);
                    spacePanelNode = client.Push("DBPanel", spaceDbVars);
                    client.Commit();
                    client.Relate(spacePanelNode, spaceNode, Model.MEPEdgeTypes.IS_IN_SPACE, null);
                    client.Commit();
                }

                if (twinCircNode == null)
                {
                    var circVars = new Dictionary<string, object>();
                    circVars.Add("Name", "Twin");
                    circVars.Add("Voltage", 230);
                    twinCircNode = client.Push("Circuit", circVars);
                    client.Commit();
                    client.Relate(spacePanelNode, twinCircNode, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                    client.Commit();
                }

                if (singleCircNode == null)
                {
                    var circVars = new Dictionary<string, object>();
                    circVars.Add("Name", "Single");
                    circVars.Add("Voltage", 230);
                    singleCircNode = client.Push("Circuit", circVars);
                    client.Commit();
                    client.Relate(spacePanelNode, singleCircNode, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                    client.Commit();
                }


  

                foreach (var outletNode in newNodesCirc1)
                {
                    client.Relate(outletNode, spaceNode, Model.MEPEdgeTypes.IS_IN_SPACE, null);
                    client.Relate(singleCircNode, outletNode, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);                   
                }


                foreach (var outletNode in newNodesCirc2)
                {
                    client.Relate(outletNode, spaceNode, Model.MEPEdgeTypes.IS_IN_SPACE, null);
                    client.Relate(twinCircNode, outletNode, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                }

                client.Commit();
                //create db panel for level
                var levelId = space.BaseLevel.Id.Value;
                PendingNode levelDbNode = null;
                if (levelPanels.ContainsKey(levelId))
                {
                    levelDbNode = levelPanels[levelId];

              
                    var spaceCircVars = new Dictionary<string, object>();
                    spaceCircVars.Add("Name", space.Number);
                    var spaceCirc = client.Push("Circuit", spaceCircVars);
                    client.Relate(spaceCirc, spacePanelNode, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                    client.Relate(levelDbNode, spaceCirc, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                    client.Commit();
                }
                else
                {
                    var levelDbVars = new Dictionary<string, object>();
                    levelDbVars.Add("Name", "DB-" + space.BaseLevel.Abbreviation.Value);
                    levelDbNode = client.Push("DBPanel", levelDbVars);
                    client.Relate(levelDbNode, new PendingNode("Level", levelId), Model.MEPEdgeTypes.IS_ON, null);


                    var levelCircVars = new Dictionary<string, object>();
                    levelCircVars.Add("Name", space.BaseLevel.Abbreviation.Value);
                    levelCircVars.Add("Voltage", 230);
                    var levelCirc = client.Push("Circuit", levelCircVars);
                    client.Relate(levelCirc, levelDbNode, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                    client.Relate(rootDbNode, levelCirc, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);

                    levelPanels.Add(levelId, levelDbNode);
                    client.Commit();

                    var spaceCircVars = new Dictionary<string, object>();
                    spaceCircVars.Add("Name", space.Number);
                    spaceCircVars.Add("Voltage", 230);
                    var spaceCirc = client.Push("Circuit", spaceCircVars);
                    client.Relate(spaceCirc, spacePanelNode, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                    client.Relate(levelDbNode, spaceCirc, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO, null);
                    client.Commit();
                }

            }

            client.Commit();
        }



        [TestMethod()]
        public void getSchema()
        {
            var bc = new BuildingGraphClient(@"http://localhost:4001/graphql");
            var cs = bc.GetSchema();
            var ojs = cs.Types.Where(ts => ts.Kind == "OBJECT").ToList();

            var mutations = cs.GetMutations("Space").ToList();
            var spaceType = cs.GetBuildingGraphType("Space");
            var spaceArea = spaceType.Fields.FirstOrDefault(fl => fl.Name == "Area");
            var spaceAreaUnit = spaceArea.Args.FirstOrDefault(ar => ar.Name == "unit");
            var spaceAreaUnitType = cs.GetBuildingGraphType(spaceAreaUnit.TypeName);

            var dbPanelType = cs.GetBuildingGraphType("DBPanel");
            var areaUnitsType = cs.GetBuildingGraphType("AreaUnits");
            var currentUnitsType = cs.GetBuildingGraphType("CurrentUnits");
            var squareMetersType = cs.GetBuildingGraphType("SquareMeters");

        }


        [TestMethod()]
        public void PushNode()
        {
            var nodeName = "Space";
            var bc = new BuildingGraphClient(@"http://localhost:4001/graphql", null);

            var vars = new Dictionary<string, object>();
            vars.Add("Name", "Test Space");
            vars.Add("Number", "TS-01");
            vars.Add("Area", 100);
            bc.Push("Space", vars);

            var vars2 = new Dictionary<string, object>();
            vars2.Add("Name", "Test Space");
            vars2.Add("Number", "TS-02");
            vars2.Add("Area", 100);
            bc.Push("Space", vars2);

            var eqname = vars["Name"] == vars2["Name"];
            var eqnumber = vars["Number"] == vars2["Number"];
            var area = vars["Area"].ToString() == vars2["Area"].ToString();
            
            Assert.AreEqual(vars["Area"], vars2["Area"]);

            bc.Commit();
            //bc.ExecuteQuery()
        }

        //71ecc562-00a3-4956-aabb-e66b08830b1b
        [TestMethod()]
        public void UpdateNode()
        {
            
            var bc = new BuildingGraphClient(@"http://localhost:4001/graphql", null);

            var nodeName = "Space";
            var vars = new Dictionary<string, object>();
            vars.Add("Name", "Test Space");
            vars.Add("Number", "TS-01");
            vars.Add("Area", 100);

            var IDs = new Dictionary<string, object>();
            IDs.Add("Id", "71ecc562-00a3-4956-aabb-e66b08830b1b");

            bc.Push(nodeName, vars, IDs);

            bc.Commit();
            //bc.ExecuteQuery()
        }

        [TestMethod()]
        public void QureyModelElements()
        {
            var bc = new BuildingGraphClient(@"http://localhost:4001/graphql", null);

            var query = @"query($modelIdent:String){
  Model (Identity:$modelIdent){
    Identity
    ModelElements {
      UniqueId
      AbstractElement {
        __typename
        Name
      }
    }
  }
}";
            var vars = new Dictionary<string, object>();
            vars.Add("modelIdent", @"8764c510-57b7-44c3-bddf-266d86c26380-0000c160:C:\Users\reynoldsw\Documents\HL-ZZ-ZZ-MEP-0001 WTS Living Lab_detached_reynoldsw.rvt");

     
            var res = bc.ExecuteQuery(query, vars);
            
            var models = res.Model;

            Dictionary<string, dynamic> modElmCache = new Dictionary<string, dynamic>();
            if (models != null)
            {
                foreach (var model in models)
                {
                    foreach (var modelElement in model.ModelElements)
                    {
                        var meID = modelElement.UniqueId.Value;
                        if (meID == null) continue;
                        if (!modElmCache.ContainsKey(meID))
                        {
                            modElmCache.Add(modelElement.UniqueId.Value, modelElement);
                        }
                        else
                        {
                            //there shouln't be multiple
                            modElmCache[meID] = modelElement;
                        }
                    }
                }
            }


        }

        /*
        [TestMethod()]
        public void ElecticalSerivce_CalcLoads()
        {
            //find panels without outgoing
            //calc loads on all outgoing circuits
            //builds tree
        }*/
    }
}