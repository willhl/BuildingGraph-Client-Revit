using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit.Parsers
{
    public interface IRevitGraphParser
    {
        bool CanParse(Element elm);
        ElementFilter GetFilter();

        void ParseFrom(Element elm, MEPRevitGraphWriter writer);
        void ParseFrom(ICollection<Element> elm, MEPRevitGraphWriter writer);

        void FinalizeGraph(MEPRevitGraphWriter writer);
        void InitializeGraph(MEPRevitGraphWriter writer);
    }

}
