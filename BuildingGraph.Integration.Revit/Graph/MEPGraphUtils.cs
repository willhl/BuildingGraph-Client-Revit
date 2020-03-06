using System.Collections.Generic;
using System;
using System.Linq;
using Autodesk.Revit.DB;

using BuildingGraph.Client;
using BuildingGraph.Client.Model;
using BuildingGraph.Client.Introspection;
using BuildingGraph.Integration.Revit.Geometry;
using HLApps.Revit.Parameters;

namespace BuildingGraph.Integrations.Revit
{
    public static class MEPGraphUtils
    {
        public static object RevitToGraphValue(HLRevitElementData elmData)
        {
            var val = elmData.Value;
            if (val is ElementId) return (val as ElementId).IntegerValue;

            if (val is Element) return (val as Element).Name;

            return val;
        }

        public static Dictionary<string, object> GetNodePropsWithElementProps(Node node, Element elm)
        {
            return GetNodePropsWithElementProps(node, elm, null, null, false);
        }

            /// <summary>
            /// Get a dictionary of paramters and values which match those required by the node type
            /// defined in the schema
            /// </summary>
            /// <param name="node"></param>
            /// <param name="elm"></param>
            /// <param name="clientMapping"></param>
            /// <returns></returns>
            public static Dictionary<string, object> GetNodePropsWithElementProps(Node node, Element elm, 
            IBuildingGraphSchema schema,
            BuildingGraphMapping clientMapping,
            bool includeOnlyMappedFields)
        {
            var elmParms = node.GetAllProperties();

            
            if (elm != null && elm.Location is LocationPoint)
            {
                var lpt = (elm.Location as LocationPoint);
                var insPt = lpt.Point;
                if (!elmParms.ContainsKey("Location")) elmParms.Add("Location", insPt.ToBuildingGraph());
                //if (!elmParms.ContainsKey("LocationRotation")) elmParms.Add("LocationRotation", lpt.Rotation);
            }
            else if (elm != null && elm.Location is LocationCurve)
            {
                //just start and end points for now
                var asCurve = (elm.Location as LocationCurve).Curve;
                var insPt = asCurve.GetEndPoint(0);
                if (!elmParms.ContainsKey("Location")) elmParms.Add("Location", insPt.ToBuildingGraph());

                var endPt = asCurve.GetEndPoint(1);
                if (!elmParms.ContainsKey("LocationEnd")) elmParms.Add("LocationEnd", endPt.ToBuildingGraph());

                var length = asCurve.Length;
                if (!elmParms.ContainsKey("length")) elmParms.Add("Length", length);

                var slope = Math.Abs(endPt.Z - insPt.Z) / length;
                if (!elmParms.ContainsKey("slope")) elmParms.Add("slope", length);
            }

            IBuildingGraphType bqType = schema != null ? schema.GetBuildingGraphType(node.Label) : null;
            BuildingGraphMappedType clType = clientMapping != null ? clientMapping.Types.FirstOrDefault(ct => ct.Name == node.Label) : null;

            foreach (var param in elm.Parameters.OfType<Parameter>())
            {
                var hp = new HLRevitParameter(param);
                var paramName = Utils.GetGraphQLCompatibleFieldName(param.Definition.Name);
                var val = RevitToGraphValue(hp);

                if (bqType != null && clientMapping != null)
                {
                    //resolve mapped field name if present
                    if (clType != null)
                    {
                        var mappedFielName = clType.ValueMap.FirstOrDefault(vm => vm.Value == paramName);
                        if (mappedFielName.Value == paramName) paramName = mappedFielName.Key;
                    }

                    var paramField = bqType.Fields.FirstOrDefault(fb => fb.Name == paramName);
                    if (includeOnlyMappedFields && paramField == null) continue;

                    //attempt to convert units
                    if (val is double)
                    {
                        var fieldUnit = paramField.Args.FirstOrDefault(arg => arg.Name == "unit");
                        if (fieldUnit == null) continue;

                        //var fieldUnitType = schema.GetBuildingGraphType(fieldUnit.TypeName);

                        var unitMapping = clientMapping.Types.FirstOrDefault(tp => tp.Name == fieldUnit.TypeName);

                        var defaultValue = fieldUnit.DefaultValue != null ? fieldUnit.DefaultValue.ToString() : string.Empty;
                        if (unitMapping != null && unitMapping.ValueMap.ContainsKey(fieldUnit.DefaultValue.ToString()))
                        {
                            var revitValue = unitMapping.ValueMap[defaultValue];
                            DisplayUnitType revitUnitTypeEnum;
                            if (Enum.TryParse(revitValue, out revitUnitTypeEnum))
                            {
                                //var type = Type.GetType(unitMapping.NativeType);// "Namespace.MyClass, MyAssembly");
                                val = UnitUtils.ConvertFromInternalUnits((double)val, revitUnitTypeEnum);
                            }
                        }
                    }
                }

                if (!elmParms.ContainsKey(paramName))
                {
                    elmParms.Add(paramName, val);
                }
            }

            return elmParms;
        }
   
        public static Dictionary<string, object> GetEdgeProps(Element elm)
        {
            var eprops = new Dictionary<string, object>();
            if (elm != null)
            {
                eprops.Add("UniqueId", elm.UniqueId);
                eprops.Add("ModelId", elm.Id.IntegerValue);
            }

            return eprops;

        }
    }
}
