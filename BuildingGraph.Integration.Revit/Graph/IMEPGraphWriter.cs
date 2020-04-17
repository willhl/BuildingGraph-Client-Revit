using Autodesk.Revit.DB;
using System.Threading.Tasks;

namespace BuildingGraph.Integrations.Revit
{
    public interface IMEPGraphWriter
    {
        void Write(MEPRevitGraph mepGraph, Document rootDoc);
    }
}