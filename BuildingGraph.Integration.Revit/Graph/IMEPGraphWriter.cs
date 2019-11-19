using Autodesk.Revit.DB;

namespace BuildingGraph.Integrations.Revit
{
    public interface IMEPGraphWriter
    {
        void Write(MEPRevitGraph mepGraph, Document rootDoc);
    }
}