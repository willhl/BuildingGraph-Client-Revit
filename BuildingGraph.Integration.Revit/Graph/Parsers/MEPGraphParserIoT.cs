using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using HLApps.Revit.Utils;
using HLApps.Revit.Geometry;
using Model = BuildingGraph.Client.Model;

namespace BuildingGraph.Integration.Revit.Parsers
{
    public class MEPGraphParserIoT : MEPGraphParserConnectors
    {
        public override bool CanParse(Element elm)
        {
            var fi = elm as FamilyInstance;
            return fi != null && fi.Symbol != null && fi.Symbol.FamilyName != null && fi.Symbol.FamilyName.ToLower().StartsWith("hl_iot");
        }

 
        public override ElementFilter GetFilter()
        {
            var dattFilter = new ElementCategoryFilter(BuiltInCategory.OST_DataDevices);
            return dattFilter;
        }

        public override void ParseFrom(ICollection<Element> elms, MEPRevitGraphWriter writer)
        {
            foreach (var elm in elms)
            {
                ParseFrom(elm, writer);
            }
        }


        public override void ParseFrom(Element elm, MEPRevitGraphWriter writer)
        {
            var fi = elm as FamilyInstance;
            if (fi == null) return;

            var elmHost = fi.Host;

            Transform tsx = Transform.Identity;
            if (elmHost is RevitLinkInstance)
            {
                RevitLinkInstance rl = elmHost as RevitLinkInstance;
                var ldoc = rl.GetLinkDocument();
                var sRef = fi.HostFace.ConvertToStableRepresentation(elm.Document);
                //dcdb8503-82b6-4a11-a408-754c3e8e7289-0026baca:0:RVTLINK:3791865:0:INSTANCE:3769400:94:SURFACE
                string[] refTokens = sRef.Split(':');
                var extIdst = refTokens[3]; //6ths index contains the elementID in host document!
                var extId = new ElementId(int.Parse(extIdst));

                var lelmHost = ldoc.GetElement(extId);
                if (lelmHost != null)
                {
                    writer.Graph.AddConnection(elm, lelmHost, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.SENSING);
                }

                tsx = rl.GetTotalTransform().Inverse;
                elmHost = lelmHost;
            }
            else if (elmHost  != null && !(elmHost is ReferencePlane))
            {
                writer.Graph.AddConnection(elm, elmHost, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.SENSING);
            }
            else if (fi.Space != null)
            {
                writer.Graph.AddConnection(elm, fi.Space, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.SENSING);
            }

            //find space, level, etc of host element.
            if (elmHost != null && !(elmHost is ReferencePlane))
            {
                base.ScanSpacesFromElement(elmHost, writer.Graph, writer.Cache.ParsedElements, tsx, elm.Document);
            }
        }


    }
}
