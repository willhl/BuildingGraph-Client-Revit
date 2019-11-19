using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using HLApps.Revit.Geometry;

namespace HLApps.Revit.Utils
{
    class GeoUtils
    {
        public static IEnumerable<Solid> GetAllSolidsInGeometry(GeometryObject geo)
        {
            if (geo != null)
            {
                if (geo is GeometryElement)
                {
                    var geoElm = geo as GeometryElement;
                    foreach (var geoObj in geoElm.OfType<GeometryObject>())
                    {
                        foreach (var recursiveCat in GetAllSolidsInGeometry(geoObj))
                        {
                            yield return recursiveCat;
                        }
                    }
                }

                if (geo is GeometryInstance)
                {
                    var geoInst = geo as GeometryInstance;
                    GeometryElement geoElm = null;
                    try
                    {
                        if (geoInst.Transform.IsConformal)
                        {
                            geoElm = geoInst.GetInstanceGeometry();
                        }
                    }
                    catch
                    {
                        //sometimes Revit throws an error unexpectedly, (e.g. "Trf is not conformal")
                        //so we'll swallow it for now
                    }

                    if (geoElm != null)
                    {
                        foreach (var recursiveCat in GetAllSolidsInGeometry(geoElm))
                        {
                            yield return recursiveCat;
                        }
                    }
                }

                if (geo is Solid)
                {
                    yield return geo as Solid;
                }
            }

        }


        /// <summary>
        /// Returns an axis aligned box which for extents of only visible geometry
        /// Solid.GetBoundingBox method includes planes and non-visible geometry
        /// </summary>
        /// <param name="geoObj"></param>
        /// <param name="parentTransform"></param>
        /// <returns></returns>
        public static HLBoundingBoxXYZ GetGeoBoundingBox(Solid geoObj, Transform parentTransform)
        {
            HLBoundingBoxXYZ box = null;

            var sld = geoObj as Solid;
            foreach (var edge in sld.Edges.OfType<Edge>())
            {
                foreach (var pt in edge.Tessellate())
                {
                    var ptx = parentTransform.OfPoint(pt);
                    if (box == null)
                    {
                        box = new HLBoundingBoxXYZ(ptx, new XYZ(0.01, 0.01, 0.01));
                    }
                    else
                    {
                        box.ExpandToContain(ptx);
                    }
                }
            }

            return box;
        }

        public static bool DoBoxesIntersect(HLBoundingBoxXYZ r1, HLBoundingBoxXYZ r2, double tolerance)
        {
            return r1.Min.X <= r2.Max.X && r1.Max.X >= r2.Min.X && r1.Min.Y <= r2.Max.Y && r1.Max.Y >= r2.Min.Y && r1.Min.Z <= r2.Max.Z && r1.Max.Z >= r2.Min.Z;
        }

        //Rotate the point (x,y,z) around the vector (u,v,w)
        public static XYZ RotateAboutVector(XYZ point, XYZ Vector, double angle)
        {

            double u = Vector.X;
            double v = Vector.Y;
            double w = Vector.Z;
            double x = point.X;
            double y = point.Y;
            double z = point.Z;

            double ux = Vector.X * point.X;
            double uy = Vector.X * point.Y;
            double uz = Vector.X * point.Z;
            double vx = Vector.Y * point.X;
            double vy = Vector.Y * point.Y;
            double vz = Vector.Y * point.Z;
            double wx = Vector.Z * point.X;
            double wy = Vector.Z * point.Y;
            double wz = Vector.Z * point.Z;
            double sa = Math.Sin(angle);
            double ca = Math.Cos(angle);
            x = u * (ux + vy + wz) + (x * (v * v + w * w) - u * (vy + wz)) * ca + (-wy + vz) * sa;
            y = v * (ux + vy + wz) + (y * (u * u + w * w) - v * (ux + wz)) * ca + (wx - uz) * sa;
            z = w * (ux + vy + wz) + (z * (u * u + v * v) - w * (ux + vy)) * ca + (-vx + uy) * sa;

            return new XYZ(x, y, z);
        }


    }
}
