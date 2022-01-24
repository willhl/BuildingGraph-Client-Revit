using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

namespace BuildingGraph.Integration.Revit
{
    public static class HLMEPSystemExtenstions
    {

        public static ICollection<ElectricalSystem> HLGetElectricalSystems(this MEPModel mepModel)
        {
#if REVIT2022
            return mepModel.GetElectricalSystems();
#else
            return mepModel.ElectricalSystems != null ? mepModel.ElectricalSystems.OfType<ElectricalSystem>().ToList() : null;
#endif

        }


    }
}
