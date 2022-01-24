using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Integration.Revit
{
    public enum MEPPathDirection
    {
        Indeterminate,
        In,
        Out,

    }


    public enum MEPPathConnectionType
    {
        Phyiscal,
        Proximity,
        Analytical,
        ProximityNoConnector,
        SectionOf,
    }


    public enum MEPPathSectionType
    {
        Section,
        Equipment,
        Terminal,
        Transition,
        Accessory,
        Space,
        Wall,
        Window,
        Door,
        Floor,
        Roof,
        Lighting,
        Electrical,
        Data,
        Security,
        Safety,
        
    }
}
