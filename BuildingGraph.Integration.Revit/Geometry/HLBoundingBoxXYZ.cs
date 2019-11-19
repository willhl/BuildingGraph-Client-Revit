using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace HLApps.Revit.Geometry
{
    public class HLBoundingBoxXYZ // : Tuple<XYZ, XYZ>
    {
        public static HLBoundingBoxXYZ FromXYZPoints(IList<XYZ> points)
        {
            HLBoundingBoxXYZ bbox = new HLBoundingBoxXYZ();

            foreach (XYZ point in points)
            {
                bbox.ExpandToContain(point);
            }

            return bbox;
        }


        /// <summary>
        /// Minimum and maximum X, Y and Z values.
        /// </summary>
        double xmin, ymin, zmin, xmax, ymax, zmax;

        /// <summary>
        /// Initialise to infinite values.
        /// </summary>
        public HLBoundingBoxXYZ()
        {
            xmin = ymin = zmin = double.MaxValue;
            xmax = ymax = zmax = double.MinValue;
        }

        public HLBoundingBoxXYZ(XYZ min, XYZ max, bool wibble)
        {
            xmin = ymin = zmin = double.MaxValue;
            xmax = ymax = zmax = double.MinValue;

            ExpandToContain(min);
            ExpandToContain(max);
        }

        /// <summary>
        /// Initialise to finiate values.
        /// </summary>
        public HLBoundingBoxXYZ(BoundingBoxXYZ rvBox)
        {
            xmin = rvBox.Min.X;
            xmax = rvBox.Max.X;

            ymin = rvBox.Min.Y;
            ymax = rvBox.Max.Y;

            zmin = rvBox.Min.Z;
            zmax = rvBox.Max.Z;
        }

        /// <summary>
        /// Initialise to finiate values.
        /// </summary>
        public HLBoundingBoxXYZ(XYZ center, XYZ size)
        {

            xmin = center.X - (size.X * 0.5);
            ymin = center.Y - (size.Y * 0.5);
            zmin = center.Z - (size.Z * 0.5);

            xmax = center.X + (size.X * 0.5);
            ymax = center.Y + (size.Y * 0.5);
            zmax = center.Z + (size.Z * 0.5);
        }

        /// <summary>
        /// Return current lower left corner.
        /// </summary>
        public XYZ Min
        {
            get { return new XYZ(xmin, ymin, zmin); }
        }

        /// <summary>
        /// Return current upper right corner.
        /// </summary>
        public XYZ Max
        {
            get { return new XYZ(xmax, ymax, zmax); }
        }

        public XYZ MidPoint
        {
            get { return 0.5 * (Min + Max); }
        }

        /// <summary>
        /// Expand bounding box to contain 
        /// the given new point.
        /// </summary>
        public void ExpandToContain(XYZ p)
        {
            if (p.X < xmin) { xmin = p.X; }
            if (p.Y < ymin) { ymin = p.Y; }
            if (p.Z < zmin) { zmin = p.Z; }
            if (p.X > xmax) { xmax = p.X; }
            if (p.Y > ymax) { ymax = p.Y; }
            if (p.Z > zmax) { zmax = p.Z; }
        }

        public bool Intersects(HLBoundingBoxXYZ r2)
        {
            var r1 = this;

            return r1.Min.X <= r2.Max.X && r1.Max.X >= r2.Min.X && r1.Min.Y <= r2.Max.Y && r1.Max.Y >= r2.Min.Y && r1.Min.Z <= r2.Max.Z && r1.Max.Z >= r2.Min.Z;

        }

        public bool Contains(XYZ point)
        {
            return point.X <= xmax
                && point.Y <= ymax
                && point.Z <= zmax
                && point.X >= xmin
                && point.Y >= ymin
                && point.Z >= zmin;
        }

        public bool IntersectRay(Line Ray)
        {

            var bmin = this.Min;
            var bmax = this.Max;

            XYZ itx = XYZ.Zero;
            if (Ray.IsBound && CheckLineBoX(bmin, bmax, Ray.GetEndPoint(0), Ray.GetEndPoint(1), ref itx)) return true;


            //if (Ray.IsBound)
            {
                var mpointItx = Ray.Project(this.MidPoint);
                var mpx = getInteresct(Ray, this.MidPoint);
                if (this.Contains(mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmin.X, bmin.Y, bmin.Z));
                mpx = getInteresct(Ray, new XYZ(bmin.X, bmin.Y, bmin.Z));
                if (this.Contains(mpx)) return true;// mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmin.X, bmax.Y, bmin.Z));
                mpx = getInteresct(Ray, new XYZ(bmin.X, bmax.Y, bmin.Z));
                if (this.Contains(mpx)) return true;//mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmax.X, bmax.Y, bmin.Z));
                mpx = getInteresct(Ray, new XYZ(bmax.X, bmax.Y, bmin.Z));
                if (this.Contains(mpx)) return true;//mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmax.X, bmin.Y, bmin.Z));
                mpx = getInteresct(Ray, new XYZ(bmax.X, bmin.Y, bmin.Z));
                if (this.Contains(mpx)) return true;//mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmin.X, bmin.Y, bmax.Z));
                mpx = getInteresct(Ray, new XYZ(bmin.X, bmin.Y, bmax.Z));
                if (this.Contains(mpx)) return true;//mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmin.X, bmax.Y, bmax.Z));
                mpx = getInteresct(Ray, new XYZ(bmin.X, bmax.Y, bmax.Z));
                if (this.Contains(mpx)) return true;//mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmax.X, bmax.Y, bmax.Z));
                mpx = getInteresct(Ray, new XYZ(bmax.X, bmax.Y, bmax.Z));
                if (this.Contains(mpx)) return true;//mpointItx.XYZPoint)) return true;

                //mpointItx = Ray.Project(new XYZ(bmax.X, bmin.Y, bmax.Z));
                mpx = getInteresct(Ray, new XYZ(bmax.X, bmin.Y, bmax.Z));
                if (this.Contains(mpx)) return true;//mpointItx.XYZPoint)) return true;
            }


            var rx0 = Ray.Origin;
            var rn_inv = Ray.Direction.Negate();

            double tx1 = (bmin.X - rx0.X) * rn_inv.X;
            double tx2 = (bmax.X - rx0.X) * rn_inv.X;

            var tmin = Math.Min(tx1, tx2);
            var tmax = Math.Max(tx1, tx2);

            double ty1 = (bmin.Y - rx0.Y) * rn_inv.Y;
            double ty2 = (bmax.Y - rx0.Y) * rn_inv.Y;

            tmin = Math.Max(tmin, Math.Min(ty1, ty2));
            tmax = Math.Min(tmax, Math.Max(ty1, ty2));

            double tz1 = (bmin.Z - rx0.Z) * rn_inv.Z;
            double tz2 = (bmax.Z - rx0.Z) * rn_inv.Z;

            tmin = Math.Max(tmin, Math.Min(tz1, tz2));
            tmax = Math.Min(tmax, Math.Max(tz1, tz2));

            return tmax >= tmin;
        }

        bool CheckLineBoX(XYZ B1, XYZ B2, XYZ L1, XYZ L2, ref XYZ Hit)
        {
            if (L2.X < B1.X && L1.X < B1.X) return false;
            if (L2.X > B2.X && L1.X > B2.X) return false;
            if (L2.Y < B1.Y && L1.Y < B1.Y) return false;
            if (L2.Y > B2.Y && L1.Y > B2.Y) return false;
            if (L2.Z < B1.Z && L1.Z < B1.Z) return false;
            if (L2.Z > B2.Z && L1.Z > B2.Z) return false;
            if (L1.X > B1.X && L1.X < B2.X &&
                L1.Y > B1.Y && L1.Y < B2.Y &&
                L1.Z > B1.Z && L1.Z < B2.Z)
            {
                Hit = L1;
                return true;
            }
            if ((GetIntersection(L1.X - B1.X, L2.X - B1.X, L1, L2, ref Hit) && InBoX(Hit, B1, B2, 1))
              || (GetIntersection(L1.Y - B1.Y, L2.Y - B1.Y, L1, L2, ref Hit) && InBoX(Hit, B1, B2, 2))
              || (GetIntersection(L1.Z - B1.Z, L2.Z - B1.Z, L1, L2, ref Hit) && InBoX(Hit, B1, B2, 3))
              || (GetIntersection(L1.X - B2.X, L2.X - B2.X, L1, L2, ref Hit) && InBoX(Hit, B1, B2, 1))
              || (GetIntersection(L1.Y - B2.Y, L2.Y - B2.Y, L1, L2, ref Hit) && InBoX(Hit, B1, B2, 2))
              || (GetIntersection(L1.Z - B2.Z, L2.Z - B2.Z, L1, L2, ref Hit) && InBoX(Hit, B1, B2, 3)))
                return true;

            return false;
        }

        XYZ getInteresct(Line ln, XYZ P)
        {
            //A + dot(AP,AB) / dot(AB,AB) * AB

            var A = ln.GetEndPoint(0);
            var B = ln.GetEndPoint(1);
            var AP = P - A;
            var AB = B - A;

            var prjPmpointItxA = A + AP.DotProduct(AB) / AB.DotProduct(AB) * AB;

            return prjPmpointItxA;
        }

        bool GetIntersection(double fDst1, double fDst2, XYZ P1, XYZ P2, ref XYZ Hit)
        {
            if ((fDst1 * fDst2) >= 0.0f) return false;
            if (fDst1 == fDst2) return false;
            Hit = P1 + (P2 - P1) * (-fDst1 / (fDst2 - fDst1));
            return true;
        }

        bool InBoX(XYZ Hit, XYZ B1, XYZ B2, int AXis)
        {
            if (AXis == 1 && Hit.Z > B1.Z && Hit.Z < B2.Z && Hit.Y > B1.Y && Hit.Y < B2.Y) return true;
            if (AXis == 2 && Hit.Z > B1.Z && Hit.Z < B2.Z && Hit.X > B1.X && Hit.X < B2.X) return true;
            if (AXis == 3 && Hit.X > B1.X && Hit.X < B2.X && Hit.Y > B1.Y && Hit.Y < B2.Y) return true;
            return false;
        }

        public bool IsInvalid
        {
            get
            {
                return double.IsInfinity(xmax) || double.IsInfinity(xmin) || double.IsInfinity(ymax) || double.IsInfinity(ymin) || double.IsInfinity(zmax) || double.IsInfinity(zmin) || double.IsNaN(xmax) || double.IsNaN(xmin) || double.IsNaN(ymax) || double.IsNaN(ymin) || double.IsNaN(zmax) || double.IsNaN(zmin);
            }
        }

        /// <summary>
        /// Set size of bbox from current center point
        /// </summary>
        public XYZ Size
        {
            get
            {
                if (double.IsInfinity(xmax) || double.IsInfinity(xmin) || double.IsInfinity(ymax) || double.IsInfinity(ymin) || double.IsInfinity(zmax) || double.IsInfinity(zmin)) return XYZ.Zero;
                if (double.IsNaN(xmax) || double.IsNaN(xmin) || double.IsNaN(ymax) || double.IsNaN(ymin) || double.IsNaN(zmax) || double.IsNaN(zmin)) return XYZ.Zero;
                return new XYZ(Math.Abs(xmax - xmin), Math.Abs(ymax - ymin), Math.Abs(zmax - zmin));
            }
            set
            {
                var center = this.MidPoint;
                var size = value;

                xmin = center.X - (size.X * 0.5);
                ymin = center.Y - (size.Y * 0.5);
                zmin = center.Z - (size.Z * 0.5);

                xmax = center.X + (size.X * 0.5);
                ymax = center.Y + (size.Y * 0.5);
                zmax = center.Z + (size.Z * 0.5);
            }
        }
    }


    public class HLBoundingBoxUV // : Tuple<XYZ, XYZ>
    {
        public static HLBoundingBoxUV FromXYZPoints(IList<UV> points)
        {
            HLBoundingBoxUV bbox = new HLBoundingBoxUV();

            foreach (UV point in points)
            {
                bbox.ExpandToContain(point);
            }

            return bbox;
        }


        /// <summary>
        /// Minimum and maximum X, Y and Z values.
        /// </summary>
        double umin, vmin, umax, vmax;

        /// <summary>
        /// Initialise to infinite values.
        /// </summary>
        public HLBoundingBoxUV()
        {
            umin = vmin = double.MaxValue;
            umax = vmax = double.MinValue;
        }

        /// <summary>
        /// Return current lower left corner.
        /// </summary>
        public UV Min
        {
            get { return new UV(umin, vmin); }
        }

        /// <summary>
        /// Return current upper right corner.
        /// </summary>
        public UV Max
        {
            get { return new UV(umax, vmax); }
        }

        public UV MidPoint
        {
            get { return 0.5 * (Min + Max); }
        }

        public double Left
        {
            get
            {
                return umin;
            }
        }

        public double Right
        {
            get
            {
                return umax;
            }
        }

        public double Top
        {
            get
            {
                return vmax;
            }
        }

        public double Bottom
        {
            get
            {
                return vmin;
            }
        }

        /// <summary>
        /// Expand bounding box to contain 
        /// the given new point.
        /// </summary>
        public void ExpandToContain(UV p)
        {
            if (p.U < umin) { umin = p.U; }
            if (p.V < vmin) { vmin = p.V; }
            if (p.U > umax) { umax = p.U; }
            if (p.V > vmax) { vmax = p.V; }
        }
    }
}
