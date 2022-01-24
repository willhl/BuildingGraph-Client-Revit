using Autodesk.Revit.DB;
using System.Threading.Tasks;

namespace BuildingGraph.Integration.Revit
{
    public interface IMEPGraphWriter
    {
        void Write(MEPRevitGraph mepGraph, Document rootDoc);
    }
}