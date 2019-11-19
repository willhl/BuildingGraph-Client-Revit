using Autodesk.Revit.DB;

namespace HLApps.Revit.Utils
{
    class MEPUtils
    {
        public static ConnectorManager GetConnectionManager(Element elm)
        {
            if (elm is FamilyInstance)
            {
                return (elm as FamilyInstance).MEPModel.ConnectorManager;
            }
            else if (elm is MEPCurve)
            {
                return (elm as MEPCurve).ConnectorManager;
            }
            else
            {
                return null;
            }
        }

    }
}
