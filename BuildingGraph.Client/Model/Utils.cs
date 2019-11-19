using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Client
{
    public sealed class Utils
    {


        public static string GetGraphQLCompatibleFieldName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;

            var qlName = string.Empty;
            for (int i = 0; i < name.Length; i++)
            {
                var ch = name[i];
                if (ch == '#')
                {
                    qlName += "No";
                }
                else if (ch == '%')
                {
                    qlName += "Pc";
                }
                else if (char.IsLetterOrDigit(ch) || ch == '_')
                {
                    if (i == 0 && char.IsDigit(ch)) qlName += DigitToText[int.Parse(ch.ToString())];
                    qlName += ch;
                }
                else
                {
                    qlName += '_';
                }
            }

            return qlName;
        }

        static string[] DigitToText = {"a","b","c","d","e","f","g","h","i","j"};
    }
}
