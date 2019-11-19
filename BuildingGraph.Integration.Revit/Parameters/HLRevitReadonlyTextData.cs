using System;
using Autodesk.Revit.DB;

namespace HLApps.Revit.Parameters
{
    public class HLRevitReadonlyTextData : HLRevitElementData
    {
        int _elmId;
        string _value;
        string _Name;

        public HLRevitReadonlyTextData(ElementId elementId, string value, string name)
        {
            _elmId = elementId.IntegerValue;
            _value = value;
            _Name = name;
        }

        public override Type ExpectedType
        {
            get
            {
                return typeof(string);
            }
        }

        public override int IntElementId
        {
            get
            {
                return _elmId;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return _Name;
            }
        }

        public override StorageType StorageType
        {
            get
            {
                return StorageType.String;
            }
        }

        public override string StringValue
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public override object Value
        {
            get
            {
                return StringValue;
            }
            set
            {
                StringValue = value != null ? value.ToString() : string.Empty;
            }
        }

        protected override void OnDisposing()
        {

        }
    }


    public class HLRevitReadonlyData : HLRevitElementData
    {
        int _elmId;
        object _value;
        string _Name;

        public HLRevitReadonlyData(ElementId elementId, object value, string name)
        {
            _elmId = elementId.IntegerValue;
            _value = value;
            _Name = name;

            _storageType = StorageType.None;
            if (value is int)
            {
                _storageType = StorageType.Integer;
            }
            else if (value is double)
            {
                _storageType = StorageType.Double;
            }
            else if (value is string)
            {
                _storageType = StorageType.String;
            }
            else if (value is ElementId)
            {
                _storageType = StorageType.ElementId;
            }
        }


        public override Type ExpectedType
        {
            get
            {
                return typeof(string);
            }
        }

        public override int IntElementId
        {
            get
            {
                return _elmId;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return _Name;
            }
        }

        StorageType _storageType;
        public override StorageType StorageType
        {
            get
            {
                return _storageType;
            }
        }

        public override string StringValue
        {
            get
            {
                return _value != null ? _value.ToString() : string.Empty;
            }
            set
            {
                throw new Exception("Value is read only");
            }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
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
