using System;
using Autodesk.Revit.DB;

namespace HLApps.Revit.Parameters
{
    public class HLRevitElementName : HLRevitElementData
    {
        Element _elm;

        public HLRevitElementName(Element elm)
        {
            _elm = elm;
        }

        public override string Name
        {
            get { return "Name"; }
        }

        public override object Value
        {
            get
            {
                return _elm.Name;
            }
            set
            {
                if (value != null)
                {
                    _elm.Name = value.ToString();
                }
            }
        }

        public override Type ExpectedType
        {
            get { return typeof(string); }
        }

        public override string StringValue
        {
            get
            {
                return _elm.Name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value)) _elm.Name = value;
            }
        }

        public override StorageType StorageType
        {
            get { return Autodesk.Revit.DB.StorageType.String; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override int IntElementId
        {
            get
            {
                return _elm.Id.IntegerValue;
            }
        }

        protected override void OnDisposing()
        {

        }
    }

    /*
     *Name property on element has no Get?? this soultion didn't work
    public static class ObjectExtensions
    {
        public static Object GetPropertyValue(this Object obj, String propertyName)
        {
            if (obj == null) throw new ArgumentNullException("obj", "`obj` cannot be null");

            var fields = from f in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                         where f.Name.Contains(String.Format("<{0}>", propertyName))
                         select f;

            if (fields.Any())
            {
                return fields.First().GetValue(obj);
            }

            return null;
        }
    } */


}
