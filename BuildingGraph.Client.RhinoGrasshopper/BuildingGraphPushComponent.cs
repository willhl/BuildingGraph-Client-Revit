using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using BuildingGraph.Client;

namespace BuildingGraph.Client.RhinoGrasshopper
{
    public class BuildingGraphPushComponent : GH_Component
    {
        public override Guid ComponentGuid => new Guid("{91989ECC-6E8F-4CF4-BC9A-EE5904FCFA86}");
        private BuildingGraphClient _client;

        public BuildingGraphPushComponent() : base("BG Push", "BGPush", "Building graph API push node", "BGraph", "Mutate")
        {

        }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Endpoint", "E", "URL of Building Graph API", GH_ParamAccess.item);
            pManager.AddTextParameter("Element Name", "N", "Element name", GH_ParamAccess.item);
            pManager.AddTextParameter("Id", "Id", "Element ID", GH_ParamAccess.item);
            pManager.AddTextParameter("Variable Names", "Vn", "Variable Names", GH_ParamAccess.list, string.Empty);
            pManager.AddGenericParameter("Variable Values", "Vv", "Variable Values", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Result Node ID", "R", "Result Node ID", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            this.Phase = GH_SolutionPhase.Collecting;

            var elmName = string.Empty;
            DA.GetData("Element Name", ref elmName);

            var elmId = string.Empty;
            DA.GetData("Id", ref elmId);

            var variableNames = new List<string>();
            DA.GetDataList("Variable Names", variableNames);

            var variableValues = new List<object>();
            DA.GetDataList("Variable Values", variableValues);

            var inputVariables = Utils.GHInputsToDictionary(variableNames, variableValues);

            var endpoint = string.Empty;
            DA.GetData("Endpoint", ref endpoint);

            if (_client == null) _client = new BuildingGraphClient(endpoint);

            Dictionary<string, object> mergeOn = null;
            if (!string.IsNullOrEmpty(elmId))
            {
                mergeOn = new Dictionary<string, object>();
                mergeOn.Add("Id", elmId);

            }

            var result = _client.Push(elmName, inputVariables, mergeOn);


            _client.Commit();

            DA.SetData("Result Node ID", result.TempId);
            DA.IncrementIteration();
            
            this.Phase = GH_SolutionPhase.Computed;

        }

        protected override void AfterSolveInstance()
        {
            base.AfterSolveInstance();
        }
    }


}
