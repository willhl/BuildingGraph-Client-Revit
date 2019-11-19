using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace HLApps.Revit.Utils
{
    class DocUtils
    {
        public static string GetDocumentIdent(Document doc)
        {
            if (doc.IsFamilyDocument)
            {
                return doc.PathName;
            }
            else
            {
                return string.Format("{0}:{1}", doc.ProjectInformation != null ? doc.ProjectInformation.UniqueId.ToString() : "", doc.PathName);
            }
        }

        public static Document GetDocument(string docIdent, Autodesk.Revit.ApplicationServices.Application revitApp)
        {
            Document doc = null;
            if (!string.IsNullOrEmpty(docIdent))
            {
                doc = revitApp.Documents.OfType<Document>()
                    .FirstOrDefault(dc => GetDocumentIdent(dc) == docIdent);

            }

            return doc;
        }

        public static IEnumerable<RevitLinkInstance> GetLinkInstances(Document _document)
        {
            var liColl = new FilteredElementCollector(_document);
            return liColl.OfClass(typeof(RevitLinkInstance)).ToElements().OfType<RevitLinkInstance>();

        }
    }
}
