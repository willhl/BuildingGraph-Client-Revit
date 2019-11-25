using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace BuildingGraph.Client.RhinoGrasshopper
{
    public class BuildingGraphQueryComponent : GH_Component
    {
        public override Guid ComponentGuid => new Guid("{8976525E-FD1F-4E11-A436-38BA6964399D}");
        private BuildingGraphClient _client;

        public BuildingGraphQueryComponent() : base("BG Query", "BGQuery", "Building graph API Query", "BGraph", "Query")
        {

        }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Endpoint", "E", "URL of Building Graph API", GH_ParamAccess.item);
            pManager.AddTextParameter("Query", "Q", "GraphQL query", GH_ParamAccess.item);
            pManager.AddTextParameter("Variable Names", "Vn", "Variable Names", GH_ParamAccess.list);
            pManager.AddGenericParameter("Variable Values", "Vv", "Variable Values", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "R", "Query result as json", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            this.Phase = GH_SolutionPhase.Collecting;

            var Query = string.Empty;
            DA.GetData("Query", ref Query);

            var variableNames = new List<string>();
            DA.GetDataList("Variable Names", variableNames);

            var variableValues = new List<object>();
            DA.GetDataList("Variable Values", variableValues);

            var inputVariables = Utils.GHInputsToDictionary(variableNames, variableValues);

            var endpoint = string.Empty;
            DA.GetData("Endpoint", ref endpoint);

            if (_client == null) _client = new BuildingGraphClient(endpoint);
            var result = _client.ExecuteQuery(Query, inputVariables);

            DA.SetData("Result", result);
            DA.IncrementIteration();

            this.Phase = GH_SolutionPhase.Computed;

        }

        protected override void AfterSolveInstance()
        {
            base.AfterSolveInstance();
        }
    }


}
