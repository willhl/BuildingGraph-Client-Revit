using System.Linq;

using Autodesk.Revit.DB;
using Model = BuildingGraph.Client.Model;

namespace BuildingGraph.Integrations.Revit.Parsers
{
    public class MEPGraphParserElectrical : MEPGraphParserConnectors
    {
        public override bool CanParse(Element elm)
        {
            var cats = new BuiltInCategory[] {BuiltInCategory.OST_ElectricalEquipment, BuiltInCategory.OST_ElectricalFixtures, BuiltInCategory.OST_LightingDevices, BuiltInCategory.OST_LightingFixtures, BuiltInCategory.OST_SecurityDevices, BuiltInCategory.OST_DataDevices, BuiltInCategory.OST_FireAlarmDevices, BuiltInCategory.OST_CommunicationDevices, BuiltInCategory.OST_NurseCallDevices, BuiltInCategory.OST_TelephoneDevices };
            return elm.Category != null ? cats.Contains((BuiltInCategory)elm.Category.Id.IntegerValue) : false;
        }

        public override ElementFilter GetFilter()
        {
            var cats = new BuiltInCategory[] {BuiltInCategory.OST_ElectricalEquipment, BuiltInCategory.OST_ElectricalFixtures, BuiltInCategory.OST_LightingDevices, BuiltInCategory.OST_LightingFixtures, BuiltInCategory.OST_SecurityDevices, BuiltInCategory.OST_DataDevices, BuiltInCategory.OST_FireAlarmDevices, BuiltInCategory.OST_CommunicationDevices, BuiltInCategory.OST_NurseCallDevices, BuiltInCategory.OST_TelephoneDevices };
            var emcf = new ElementMulticategoryFilter(cats);
            return emcf;
        }

        public override void ParseFrom(Element elm, MEPRevitGraphWriter writer)
        {

            //connect db panels to their circuits
            if (elm is FamilyInstance)
            {
                var fi = elm as FamilyInstance;
                var sysElm = fi.MEPModel;
                if (sysElm != null && sysElm.ElectricalSystems != null)
                {
                    var panleSystems = sysElm.ElectricalSystems.OfType<Autodesk.Revit.DB.Electrical.ElectricalSystem>().Where(sy => sy.BaseEquipment != null && sy.BaseEquipment.Id == elm.Id);

                    foreach (var sys in panleSystems)
                    {
                        writer.Graph.AddConnection(elm, sys, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO);

                        try
                        {
                            var elmsINSys = sys.Elements.OfType<FamilyInstance>();

                            foreach (var emms in elmsINSys)
                            {
                                writer.Graph.AddConnection(sys, emms, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO);

                                ParseFrom(emms, writer);
                            }
                        }
                        catch
                        {
                            //log exception
                        }
                    }
                }
            }

            base.ParseFrom(elm, writer);

        }

    }

}
