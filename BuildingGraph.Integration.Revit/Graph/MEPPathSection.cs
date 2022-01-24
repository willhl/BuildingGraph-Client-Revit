using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;


namespace BuildingGraph.Integration.Revit
{




     public class MEPPathSection
     {

        public MEPPathSection()
        {

        }

        public MEPPathSection(int orgId)
        {

        }

        public MEPPathSection(int orgId, double pathlength, MEPPathSection parent)
        {
            _ordId = orgId;
            _In = parent;
            _pathLength = pathlength;
        }

        public MEPPathSection(Element elm, double pathlength, MEPPathSection parent) : this(elm, pathlength)
        {
            _In = parent;
        }


        public MEPPathSection(Element elm, double pathlength)
        {
            _ordId = elm.Id.IntegerValue;
            _pathLength = pathlength;
            _name = elm.Name;

            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctTerminal) _sectionType = MEPPathSectionType.Terminal;
            if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_MechanicalEquipment) _sectionType = MEPPathSectionType.Equipment;

        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        MEPPathSectionType _sectionType = MEPPathSectionType.Section;
        public MEPPathSectionType SectionType
        {
            get { return _sectionType; }
        }

        double _pathLength = 0;
        public double PathLength
        {
            get { return _pathLength; }
        }

        public double TotalPathLeghth
        {
            get
            {
                return PathLength + (_In != null ? _In.TotalPathLeghth : 0);
            }
        }

        int _ordId = -1;
        int OriginId
        {
            get { return _ordId; }
        }

        MEPPathSection _In;
        public MEPPathSection In
        {
            get { return _In; }
        }

        List<MEPPathSection> _out = new List<MEPPathSection>();
        public IEnumerable<MEPPathSection> Out { get { return _out; } }

        public void Connect(MEPPathSection section, MEPPathDirection direction)
        {
            if (direction == MEPPathDirection.Out) _out.Add(section);
            if (direction == MEPPathDirection.In) _In = section;
        }

        public double GetLengthTo(string search)
        {
            if (_In != null && !_In.Name.Contains(search))
            {
                return _In.PathLength + _In.GetLengthTo(search);
            }

            return 0;
        }

    }



}
