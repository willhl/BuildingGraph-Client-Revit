using System;
using System.Linq;
using Autodesk.Revit.DB;

namespace HLApps.Revit.Parameters
{
    public class HLRevitParameter : HLRevitElementData
    {
        private Parameter _revitPerameter;
        private Guid definedGuid = new Guid();

        public HLRevitParameter(Parameter revitParam)
        {
            _revitPerameter = revitParam;
        }

        public Guid GUID
        {
            get
            {
                if (_revitPerameter.IsShared)
                {
                    return _revitPerameter.GUID;
                }
                else
                {
                    return definedGuid;
                }
            }
        }

        public ElementId Id
        {
            get
            {
                return _revitPerameter.Id;
            }
        }

        public override int IntElementId
        {
            get
            {
                return Id.IntegerValue;
            }
        }


        public override string Name
        {
            get
            {

                return _revitPerameter.Definition.Name; ;
            }
        }

        public override object Value
        {
            get
            {
                object retVal = string.Empty;
                if (_revitPerameter != null)
                {
                    var doc = _revitPerameter.Element.Document;

                    switch (_revitPerameter.StorageType)
                    {

                        case Autodesk.Revit.DB.StorageType.Double:
                            // append the type and value
                            retVal = _revitPerameter.AsDouble();
                            break;
                        case Autodesk.Revit.DB.StorageType.ElementId:
                            if (Name == "Category")
                            {
                                retVal = _revitPerameter.AsValueString();
                                return retVal;
                            }

                            var elid = _revitPerameter.AsElementId();
                            if (PreserveIds) return elid;

                            if (Name.ToLower().EndsWith("id"))
                            {
                                try
                                {
                                    retVal = _revitPerameter.AsValueString();
                                }
                                catch { retVal = doc.GetElement(elid); }//.IntegerValue; }
                            }
                            else
                            {
                                var vael = _revitPerameter.AsValueString();
                                var vaels = _revitPerameter.AsString();
                                retVal = doc.GetElement(elid);//.IntegerValue;
                                if (retVal == null) retVal = string.IsNullOrEmpty(vaels) ? vael : vaels;
                                return retVal;
                            }

                            break;
                        case Autodesk.Revit.DB.StorageType.Integer:
                            // append the type and value
                            int retValAsInt = _revitPerameter.AsInteger();

                            if (_revitPerameter.Definition.ParameterType == ParameterType.YesNo)
                            {
                                retVal = (retValAsInt == 1);
                            }
                            else
                            {
                                retVal = retValAsInt;
                            }

                            return retVal;

                        case Autodesk.Revit.DB.StorageType.String:
                            // append the type and value
                            retVal = _revitPerameter.AsString();
                            return retVal;

                        case Autodesk.Revit.DB.StorageType.None:
                            // append the type and value
                            retVal = _revitPerameter.AsValueString();
                            return retVal;

                        default:
                            break;
                    }
                }


                return retVal;
            }
            set
            {
                string valueAsString = value != null ? value.ToString() : string.Empty;
                bool wasSet = false;

                switch (_revitPerameter.StorageType)
                {
                    case Autodesk.Revit.DB.StorageType.Double:
                        // append the type and value
                        double dval = 0;
                        bool wasParsed = false;
                        
                        if (value is double)
                        {
                            dval = (double)value;
                            wasParsed = true;
                        }
                        else
                        {
                            wasParsed = double.TryParse(valueAsString, out dval);

                            if (!wasParsed && !_revitPerameter.SetValueString(valueAsString) && valueAsString.Contains(' '))
                            {
                                var stl = valueAsString.Split(' ');
                                wasParsed = double.TryParse(stl[0], out dval);
                            }

                        }

                        if (wasParsed && _revitPerameter.DisplayUnitType == DisplayUnitType.DUT_PERCENTAGE && dval > 1)
                        {
                            dval = dval / 100;
                            _revitPerameter.Set(dval);
                            wasSet = true;
                        }
                        else if (wasParsed)
                        {
                            _revitPerameter.Set(dval);
                            wasSet = true;
                        }

                        if (!wasSet || !wasParsed)
                        {
                            throw new Exception("Data type mismatch, could not parse \"" + valueAsString + "\" to a number");
                        }

                        break;
                    case Autodesk.Revit.DB.StorageType.ElementId:

                        int eval;

                        if (value is ElementId)
                        {
                            _revitPerameter.Set((ElementId)value);
                        }
                        else if (value is int)
                        {
                            _revitPerameter.Set(new ElementId((int)value));
                        }
                        else
                        {
                            if (int.TryParse(valueAsString, out eval))
                            {
                                _revitPerameter.Set(new ElementId(eval));
                            }
                            else
                            {
                                //invalid value, raise error                    
                                _revitPerameter.SetValueString(valueAsString);
                            }
                        }

                        break;
                    case Autodesk.Revit.DB.StorageType.Integer:
                        // append the type and value

                        if (_revitPerameter.Definition.ParameterType == ParameterType.YesNo)
                        {
                            bool valueAsBool = false;

                            //use string as common ground, if value is bool it'll be reparsed.

                            valueAsString = valueAsString.ToLower().Trim();
                            if (valueAsString == "yes" || valueAsString == "y")
                            {
                                valueAsBool = true;
                            }
                            else if (valueAsString == "no" || valueAsString == "n")
                            {
                                valueAsBool = false;
                            }
                            else if (!bool.TryParse(valueAsString, out valueAsBool))
                            {
                                //perhaps value is an int or string expressed as 1
                                valueAsBool = valueAsString == "1";
                            }

                            _revitPerameter.Set(valueAsBool ? 1 : 0);
                            wasSet = true;

                        }
                        else if (value is int)
                        {
                            _revitPerameter.Set((int)value);
                            wasSet = true;
                        }
                        else
                        {
                            int ival;

                            if (int.TryParse(valueAsString, out ival))
                            {
                                _revitPerameter.Set(ival);
                                wasSet = true;
                            }
                            else
                            {
                                if (!_revitPerameter.SetValueString(valueAsString) && valueAsString.Contains(" "))
                                {
                                    var stl = valueAsString.Split(' ');
                                    if (int.TryParse(stl[0], out ival))
                                    {
                                        _revitPerameter.Set(ival);
                                        wasSet = true;
                                    }
                                }
                            }
                        }

                        if (!wasSet)
                        {
                            throw new Exception("Data type mismatch, could not parse \"" + valueAsString + "\" to an integer");
                        }


                        break;
                    case Autodesk.Revit.DB.StorageType.String:
                        // append the type and value

                        _revitPerameter.Set(valueAsString);

                        break;
                    case Autodesk.Revit.DB.StorageType.None:
                        // append the type and value
                        _revitPerameter.SetValueString(valueAsString);

                        break;
                    default:
                        break;
                }

            }
        }

        public bool ReturnUnsetValuesAsEmptyString { get; set; }

        public override string StringValue
        {
            get
            {
                if (!_revitPerameter.HasValue && ReturnUnsetValuesAsEmptyString) return string.Empty;

                if (_revitPerameter.StorageType == StorageType.Double)
                {
                    if (!_revitPerameter.HasValue) return "0";

                    return _revitPerameter != null ? _revitPerameter.AsValueString() : string.Empty;
                }
                else if (_revitPerameter.Definition.ParameterType == ParameterType.YesNo)
                {
                    var bval = this.Value;
                    if (bval != null && bval is bool)
                    {
                        return (bool)bval ? "Yes" : "No";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                if (!_revitPerameter.HasValue) return string.Empty;

                var val = this.Value;

                if (val is Category)
                {
                    return (val as Category).Name;

                }

                if (val is Element)
                {
                    return (val as Element).Name;
                }

                return val != null ? val.ToString() : string.Empty;
            }
            set
            {
                if (_revitPerameter.StorageType == StorageType.Double)
                {
                    string oringalValue = _revitPerameter.AsValueString();
                    if (oringalValue != value)
                    {
                        _revitPerameter.SetValueString(value);
                    }
                }
                else
                {
                    var thisStringVal = StringValue;
                    if (string.IsNullOrEmpty(thisStringVal) && string.IsNullOrEmpty(value)) return;

                    if (thisStringVal != value)
                    {
                        this.Value = value;
                    }
                }
            }
        }

        public override Type ExpectedType
        {
            get
            {
                return Value != null ? Value.GetType() : typeof(string);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return (_revitPerameter.IsReadOnly || Name.ToLower().Contains("specified"));
            }
        }

        public override StorageType StorageType
        {
            get
            {
                return _revitPerameter.StorageType;
            }
        }

        public Parameter Parameter
        {
            get
            {
                return _revitPerameter;
            }
        }

        public bool IsShared
        {
            get
            {
                return _revitPerameter != null ? _revitPerameter.IsShared : false;
            }
        }

        protected override void OnDisposing()
        {
            _revitPerameter.Dispose();
            _revitPerameter = null;
        }

    }




}
