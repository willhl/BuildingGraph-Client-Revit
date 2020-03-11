using System.Collections.Generic;
using System.Collections;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

using HLApps.Revit.Utils;
using HLApps.Revit.Geometry;
using gmdl = BuildingGraph.Client.Model;



namespace BuildingGraph.Integrations.Revit
{

    public class MEPRevitNode
    {
      
        public MEPRevitNode(string name)
        {
            _name = name;
            Connections = new HashSet<MEPRevitEdge>();
            OrginTransform = Transform.Identity;
        }

        public MEPRevitNode(string name, string category, string sectionType, gmdl.Node analyticalNode) : this(name)
        {
            _name = name;
            _node = analyticalNode;
            OrginCategoryName = category;
            AsElementNode.Name = _name;
            AsAbstractNode.Name = sectionType;
            AsAbstractNode.ExtendedProperties.Add("Category", this.OrginCategoryName);
        }


        public MEPRevitNode(string name, int orgId) : this(name)
        {
            _ordId = orgId;
            Connections = new HashSet<MEPRevitEdge>();

        }
        
        public MEPRevitNode(Element elm) : this(string.Empty)
        {
            if (elm == null) return;

            _ordId = elm.Id.IntegerValue;
            OrginIsInLinked = elm.Document.IsLinked;
            OrginDocIdent = DocUtils.GetDocumentIdent(elm.Document);

            _name = elm.Name;

            if (elm is FamilyInstance)
            {
                var fs = (elm as FamilyInstance);

                if (fs.MEPModel is MechanicalFitting)
                {
                    _name = (fs.MEPModel as MechanicalFitting).PartType.ToString();
                }
                else if (fs.Symbol != null)
                {
#if REVIT2015
                    if (_name != fs.Symbol.FamilyName)
                    {
                        _name = fs.Symbol.FamilyName + " : " + _name;

                    }
#else
                    _name = fs.Symbol.Name;
#endif
                }
            }
            else if (elm is Space)
            {
                var sp = (elm as Space);
                _name = sp.Number + " : " + sp.Name;
            }


            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_MechanicalEquipment) _node = new gmdl.Equipment();//_sectionType = MEPPathSectionType.Equipment;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_MEPSpaces) _node = new gmdl.Space(); //_sectionType = MEPPathSectionType.Space;

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctFitting) _node = new gmdl.DuctTransition(); //_sectionType = MEPPathSectionType.Transition;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_PipeFitting) _node = new gmdl.PipeTransition(); //_sectionType = MEPPathSectionType.Transition;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_CableTrayFitting) _node = new gmdl.CableTrayTransition(); ;// _sectionType = MEPPathSectionType.Transition;

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctAccessory) _node = new gmdl.DuctAccessory(); //_sectionType = MEPPathSectionType.Accessory;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_PipeAccessory) _node = new gmdl.PipeAccessory(); //_sectionType = MEPPathSectionType.Accessory;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_PlumbingFixtures) _node = new gmdl.Fixture();

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctCurves) _node = new gmdl.Duct(); //_sectionType = MEPPathSectionType.Section;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_FlexDuctCurves) _node = new gmdl.Duct(); // _sectionType = MEPPathSectionType.Section;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_PipeCurves) _node = new gmdl.Pipe(); // _sectionType = MEPPathSectionType.Section;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_FlexPipeCurves) _node = new gmdl.Pipe(); // _sectionType = MEPPathSectionType.Section;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_CableTray) _node = new gmdl.CableTray(); //_sectionType = MEPPathSectionType.Section;

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctTerminal) _node = new gmdl.Terminal(); // _sectionType = MEPPathSectionType.Terminal;

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalEquipment) _node = new gmdl.DBPanel();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalFixtures) _node = new gmdl.ElectricalLoad();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_LightingDevices) _node = new gmdl.Lighting();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_LightingFixtures) _node = new gmdl.Lighting();

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DataDevices)
            {
                var fi = elm as FamilyInstance;
                if (fi != null && fi.Symbol.FamilyName.StartsWith("HL_IoT_Sensor"))
                {
                    _node = new gmdl.Sensor();
                }
                else if (fi != null && fi.Symbol.FamilyName.StartsWith("HL_IoT_Control"))
                {
                    _node = new gmdl.Control();
                }
                else
                {
                    _node = new gmdl.Data();
                }
                
            }


            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_SecurityDevices) _node = new gmdl.Security();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_NurseCallDevices) _node = new gmdl.Safety();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Sprinklers) _node = new gmdl.Sprinkler();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_FireAlarmDevices) _node = new gmdl.FireAlarm();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_ElectricalCircuit) _node = new gmdl.Circuit();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_CommunicationDevices) _node = new gmdl.Communications();

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_CurtainWallPanels) _node = new gmdl.Wall();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Walls) _node = new gmdl.Wall();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Ceilings) _node = new gmdl.Ceiling();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors) _node = new gmdl.Door();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Floors) _node = new gmdl.Floor();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows) _node = new gmdl.Window();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Roofs) _node = new gmdl.Roof();
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Columns) _node = new gmdl.Column();

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Levels) _node = new gmdl.Level();

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Furniture) _node = new gmdl.Furniture();

            if (elm.Category != null)
            {
                OrginCategoryId = elm.Category.Id.IntegerValue;
                OrginCategoryName = elm.Category.Name;
            }

            Level flevel = null;
            if (elm is FamilyInstance)
            {
                flevel = elm.Document.GetElement(elm.LevelId) as Level;
            }
            else if (elm is MEPCurve)
            {
                var mepc = elm as MEPCurve;
                flevel = mepc.ReferenceLevel;
                /*
                 var mplc = mepc.Location as LocationCurve;
                 var midPoint = (mplc.Curve.GetEndPoint(0) + mplc.Curve.GetEndPoint(1)) / 2;
                 var levels = HLRevitUtilities.GetElements<Level>(elm.Document).OrderBy(lv => lv.ProjectElevation);

                 flevel = levels.LastOrDefault(lv => lv.ProjectElevation < midPoint.Z);*/
            }

            if (flevel != null)
            {
                this.LevelId = flevel.Id.IntegerValue;
                this.LevelName = flevel.Name;

                //var levels = HLRevitUtilities.GetElements<Level>(elm.Document).OrderBy(lv => lv.ProjectElevation).ToList();
                //var lev = levels.First(l => l.Id.IntegerValue == flevel.Id.IntegerValue);
                //LevelIndex = levels.IndexOf(lev);
            }


            AsElementNode.ExtendedProperties.Add("UniqueId", elm.UniqueId);
            AsElementNode.ExtendedProperties.Add("Name", Name);
            AsElementNode.Name = this.Name;
            AsAbstractNode.Name = this.Name;
            AsAbstractNode.ExtendedProperties.Add("Category", this.OrginCategoryName);

        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /*MEPPathSectionType _sectionType = MEPPathSectionType.Section;
        public MEPPathSectionType SectionType
        {
            get { return _sectionType; }
        }*/


        int _ordId = -1;
        public int OriginId
        {
            get { return _ordId; }
        }


        public bool OrginIsInLinked { get; private set; }
        public string OrginDocIdent { get; private set; }
        public Transform OrginTransform { get; set; }
        public int OrginCategoryId { get; set; }
        public string OrginCategoryName { get; set; }

        public string LevelName { get; set; }
        public int LevelId { get; set; }
        public int LevelIndex { get; set; }


        public ICollection<MEPRevitNode> In
        {
            get
            {
                return Connections.Where(cn => cn.Direction == MEPPathDirection.In).Select(n => n.NextNode).ToList();
            }
        }

        public ICollection<MEPRevitNode> Out
        {
            get
            {
                return Connections.Where(cn => cn.Direction == MEPPathDirection.Out).Select(n => n.NextNode).ToList();
            }
        }


        public HashSet<MEPRevitEdge> Connections { get; }

        public override string ToString()
        {
            return Name;
        }

        public Dictionary<string, object> Properties = new Dictionary<string, object>();

        public object GetProperty(string name)
        {
            if (Properties.ContainsKey(name))
            {
                return Properties[name];
            }
            else
            {
                throw new System.Exception("Node does not contain property: " + name);
            }
        }

        public void SetProperty(string name, object value)
        {
            if (Properties.ContainsKey(name))
            {
                Properties[name] = value;
            }
            else
            {
                Properties.Add(name, value);
            }
        }


        gmdl.ModelElement _modelNode;
        public virtual gmdl.ModelElement AsElementNode
        {
            get
            {
                if (_modelNode == null) _modelNode = new gmdl.ModelElement();

                return _modelNode;

            }
        }


        gmdl.Node _node;
        public virtual gmdl.Node AsAbstractNode
        {
            get
            {
                if (_node != null) return _node;
                _node = new gmdl.Section();
                return _node;
            }
        }

        public HLBoundingBoxXYZ BoundingBox { get; set; }
   
    }
}
