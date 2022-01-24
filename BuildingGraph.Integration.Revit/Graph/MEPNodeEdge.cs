//using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using BuildingGraph.Client.Model;

namespace BuildingGraph.Integration.Revit
{

    public class MEPRevitEdge
    {

        public MEPRevitEdge(int index, MEPRevitNode owner, MEPRevitNode next, MEPPathDirection direction) : this()
        {
            ThisConnectorIndex = index;
            NextNode = next;
            ThisNode = owner;
            Direction = direction;
        }

        public MEPRevitEdge(int index, int systemId, MEPRevitNode owner, MEPRevitNode next, MEPPathDirection direction) : this(index, owner, next, direction)
        {
            SystemId = SystemId;
        }

        public MEPRevitEdge()
        {
            Length = 0;
        }

        public int SystemId { get; set; }
        public int ThisConnectorIndex { get; set; }
        public int NextConnectorIndex { get; set; }

        string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                SetWeight("Description", value);
            }

        }

        public MEPRevitNode NextNode { get; set; }

        public MEPRevitEdge Reverse { get; set; }

        public Autodesk.Revit.DB.XYZ ThisOrigin { get; set; }
        public Autodesk.Revit.DB.XYZ NextOrigin { get; set; }

        public MEPRevitNode ThisNode { get; set; }

        MEPPathConnectionType _ConnectionType = MEPPathConnectionType.Analytical;
        public MEPPathConnectionType ConnectionType
        {
            get
            {
                return _ConnectionType;
            }
            set
            {
                _ConnectionType = value;
                SetWeight("ConnectionType", value.ToString());
            }

        }

        MEPPathDirection _Direction;
        public MEPPathDirection Direction {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;
                SetWeight("Direction", value.ToString());
            }
        }


        Dictionary<string, object> _weights = new Dictionary<string, object>();
        public Dictionary<string, object> Weights
        {
            get
            {
                return _weights;
            }

        }


        public object GetWeight(string name)
        {
            if (Weights.ContainsKey(name))
            {
                return Weights[name];
            }
            else
            {
                throw new System.Exception("Node does not contain weight: " + name);
            }
        }

        public void SetWeight(string name, object value)
        {
            if (Weights.ContainsKey(name))
            {
                Weights[name] = value;
            }
            else
            {
                Weights.Add(name, value);
            }
        }

        public double Flow
        {
            get
            {
                return Weights.ContainsKey("Flow") ? (double)GetWeight("Flow") : 0;
            }
            set
            {
                SetWeight("Flow", value);
            }
        }

        public double Length
        {
            get
            {
                return Weights.ContainsKey("Length") ? (double)GetWeight("Length") : 0;
            }
            set
            {
                SetWeight("Length", value);
            }
        }


        Edge _edge;
        public Edge AsNodeEdge
        {
            get
            {
                if (_edge ==null) _edge = new Edge();

                return _edge;
            }
        }


    }

    public class MEPConnectorNodeEdge : MEPRevitEdge
    {

        public MEPConnectorNodeEdge(int index, MEPRevitNode owner, MEPRevitNode next, MEPPathDirection direction) : base(index, owner, next, direction)
        {
        }

        public MEPConnectorNodeEdge(int index, int systemId, MEPRevitNode owner, MEPRevitNode next, MEPPathDirection direction) : base(index, systemId, owner, next, direction)
        {
        }

    

    }

}
