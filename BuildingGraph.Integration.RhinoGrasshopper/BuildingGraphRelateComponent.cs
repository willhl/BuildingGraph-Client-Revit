using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace BuildingGraph.Client.RhinoGrasshopper
{
    public class BuildingGraphRelateComponent : GH_Component
    {
        public override Guid ComponentGuid => new Guid("{3B04B260-7795-4060-B380-8AB798BD85F1}");
        private BuildingGraphClient _client;

        public BuildingGraphRelateComponent() : base("BG Relate", "BGRelate", "Building graph API relate nodes", "BGraph", "Mutate")
        {

        }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Endpoint", "E", "URL of Building Graph API", GH_ParamAccess.item);
            pManager.AddTextParameter("From Node", "frmNode", "Node name of from Element", GH_ParamAccess.item);
            pManager.AddTextParameter("From Id", "frmId", "From Element ID", GH_ParamAccess.item);
            pManager.AddTextParameter("To Node", "toNode", "Node name of to Element", GH_ParamAccess.item);
            pManager.AddTextParameter("To Id", "toId", "To Element ID", GH_ParamAccess.item);
            pManager.AddTextParameter("Relationship Name", "rn", "Name of relationship to create/merge", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("From Node ID", "F", "From Node ID", GH_ParamAccess.item);
            pManager.AddTextParameter("To Node ID", "T", "To Node ID", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            this.Phase = GH_SolutionPhase.Collecting;


            var fromElmName = string.Empty;
            DA.GetData("From Node", ref fromElmName);

            var fromElmId = string.Empty;
            DA.GetData("From Id", ref fromElmId);

            var toElmName = string.Empty;
            DA.GetData("To Node", ref toElmName);

            var toElmId = string.Empty;
            DA.GetData("To Id", ref toElmId);

            var relName = string.Empty;
            DA.GetData("Relationship Name", ref relName);

            var endpoint = string.Empty;
            DA.GetData("Endpoint", ref endpoint);

            if (_client == null) _client = new BuildingGraphClient(endpoint);

            var fromPn = new PendingNode(fromElmName, fromElmId);
            var toPn = new PendingNode(toElmName, toElmId);

            //variables in relationships not supported yet
            _client.Relate(fromPn, toPn, relName, null);
            _client.Commit();

            DA.SetData("From Node ID", fromElmId);
            DA.SetData("To Node ID", toElmId);
            DA.IncrementIteration();
            
            this.Phase = GH_SolutionPhase.Computed;

        }

        protected override void AfterSolveInstance()
        {
            base.AfterSolveInstance();
        }
    }


}
