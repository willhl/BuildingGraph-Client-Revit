using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using BuildingGraph.Client.Model.Types;

namespace BuildingGraph.Integration.Revit.Geometry
{
    public static partial class Convert
    {
        public static IPoint3D ToBuildingGraph(this XYZ XYZ)
        {       
            return new Point3D(XYZ.X, XYZ.Y, XYZ.Z);
        }

    }

}
