using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace HLApps.Revit.Utils
{
    static class RevitElementExtensions
    {


        public static Parameter HLGetParameter(this Element elm, string name)
        {

            var foundPramams = elm.GetParameters(name);

            if (foundPramams.Count == 0)
            {
                foundPramams = elm.Parameters.OfType<Parameter>().Where(p => string.Equals(p.Definition.Name, name, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }


            var firstValidParam = foundPramams.FirstOrDefault(p => p.HasValue);

            return firstValidParam != null ? firstValidParam : foundPramams.FirstOrDefault();

        }

        public static Parameter HLGetParameter(this Element elm, BuiltInParameter bip)
        {

            return elm.get_Parameter(bip);

        }


        public static Parameter HLGetParameter(this Element elm, Guid Id)
        {

            return elm.get_Parameter(Id);

        }


        public static Parameter HLGetParameter(this Element elm, ElementId parameterId)
        {
            if (parameterId.IntegerValue == -1) return null;

            Parameter param = null;
            if (parameterId.IntegerValue < -1)
            {
                param = elm.get_Parameter((BuiltInParameter)parameterId.IntegerValue);
            }
            else
            {
                param = elm.Parameters.OfType<Parameter>().FirstOrDefault(pr => GetParameterId(pr).IntegerValue == parameterId.IntegerValue);
            }

            return param;
        }

        public static ElementId GetParameterId(Parameter param)
        {
            return param.Id;
        }
    }
}
