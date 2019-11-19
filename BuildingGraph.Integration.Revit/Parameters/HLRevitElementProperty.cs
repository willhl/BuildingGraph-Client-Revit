using System;
using System.Linq;
using Autodesk.Revit.DB;

namespace HLApps.Revit.Parameters
{
    public class HLRevitElementProperty : HLRevitElementData
    {
        string _name;
        object _value;
        Type _ExpectedType;
        StorageType _storageType;
        object _baseObj = null;
        bool _isReadOnly = true;
        int _elementdId = -1;

        public HLRevitElementProperty(int elmId, string name, object value, StorageType storageTyp)
        {
            _name = name;
            _value = value;
            _ExpectedType = value.GetType();
            _storageType = storageTyp;
            _isValid = true;
            _elementdId = elmId;
        }

        public HLRevitElementProperty(int elmId, string name, object value, StorageType storageTyp, object baseObj)
        {
            _name = name;
            _value = value;
            _ExpectedType = value.GetType();
            _storageType = storageTyp;
            _baseObj = baseObj;
            _isValid = true;
            _elementdId = elmId;
        }

        public HLRevitElementProperty(string name, object baseObj)
        {
            _name = name;
            _baseObj = baseObj;

            var prop = _baseObj.GetType().GetProperties().FirstOrDefault(pr => pr.Name == _name);

            var bt = _baseObj.GetType().BaseType;
            var propbt = bt.GetProperties().FirstOrDefault(pr => pr.Name == _name);

            var allProps = baseObj.GetType().GetProperties().Select(p => p.Name).ToList();
            if (prop != null)
            {
                _ExpectedType = prop.PropertyType;
                _storageType = Autodesk.Revit.DB.StorageType.None;
                if (prop.GetGetMethod() != null)
                {
                    _value = prop.GetValue(baseObj, null);
                    _isValid = true;
                }
                else
                {
                    // _value = baseObj.GetPropertyValue(name);
                }

                _isReadOnly = !prop.CanWrite;
            }
        }

        public override string Name
        {
            get
            {
                return _name;
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
                if (_baseObj != null)
                {
                    var prop = _baseObj.GetType().GetProperties().FirstOrDefault(pr => pr.Name == _name);

                    if (prop != null)
                    {
                        prop.SetValue(_baseObj, value, null);
                    }
                }
            }
        }

        public override Type ExpectedType
        {
            get
            {
                return _ExpectedType;
            }
        }

        public override string StringValue
        {
            get
            {
                if (_value is Category)
                {
                    return (_value as Category).Name;
                }
                else if (_value is Element)
                {
                    return (_value as Element).Name;
                }

                return _value.ToString();
            }
            set
            {

                Value = value;
            }

        }

        public override StorageType StorageType
        {
            get
            {
                return _storageType;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
        }

        bool _isValid = false;
        public bool IsValid
        {
            get { return _isValid; }
        }

        public override int IntElementId
        {
            get
            {
                return _elementdId;
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
