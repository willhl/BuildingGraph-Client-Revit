using System;
using System.Linq;
using Autodesk.Revit.DB;

using HLApps.Revit.Utils;

namespace HLApps.Revit.Parameters
{

    public abstract class HLRevitElementData : IHLPeramiter, IDisposable
    {

        public abstract string Name
        {
            get;
        }

        public abstract object Value
        {
            get;
            set;
        }

        public abstract int IntElementId
        {
            get;
        }

        public abstract Type ExpectedType
        {
            get;
        }

        public abstract string StringValue
        {
            get;
            set;
        }

        public abstract StorageType StorageType
        {
            get;
        }

        public abstract bool IsReadOnly
        {
            get;
        }



        public bool PreserveIds { get; set; }


        ~HLRevitElementData()
        {
            Dispose(false);
        }

        bool _disposed = false;
        public bool IsDisposed
        {
            get
            {
                return _disposed;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
                OnDisposing();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        protected abstract void OnDisposing();

        //some parameters refer to themselves but are also valid parameters
        static string[] _bultPrameterExclusions = new string[] { "Family", "Family Name", "Type", "Type Name", "System Type" };
        public static HLRevitElementData GetElementData(Element elm, string paramterName, ElementId parameterId, FieldTypes fieldType)
        {
            if (parameterId.IntegerValue == ElementId.InvalidElementId.IntegerValue) return null;

            Parameter param = null;
            if (parameterId.IntegerValue < -1)
            {
                param = elm.get_Parameter((BuiltInParameter)parameterId.IntegerValue);
            }
            else
            {
                param = elm.Parameters.OfType<Parameter>().FirstOrDefault(pr => GetParameterId(pr).IntegerValue == parameterId.IntegerValue);
            }

            if (param != null)
            {
                //var isType = elm is FamilySymbol || elm is Family || elm is MEPSystemType || elm is ElementType;

                //if (isType && _bultPrameterExclusions.Contains(param.Definition.Name))
                //{
                return GetElementData(elm, param.Definition.Name, parameterId, Guid.Empty, fieldType);
                //}
                //else
                //{
                //    return new HLRevitParameter(param);
                //}
            }
            else
            {
                return GetElementData(elm, paramterName, parameterId, Guid.Empty, fieldType);
            }

        }

        public static ElementId GetParameterId(Parameter param)
        {
            return param.Id;
        }


        public static HLRevitElementData GetElementData(Element elm, string ParameterName, FieldTypes FieldType)
        {
            return GetElementData(elm, ParameterName, ElementId.InvalidElementId, Guid.Empty, FieldTypes.InstanceParameter, false);
        }

        public static HLRevitElementData GetElementData(Element elm, string ParameterName, ElementId paramterId, Guid paramGuid, FieldTypes FieldType)
        {
            return GetElementData(elm, ParameterName, paramterId, paramGuid, FieldType, false);
        }


        public static HLRevitElementData GetElementData(Element elm, string ParameterName, ElementId paramterId, Guid paramGuid, FieldTypes FieldType, bool redirectType)
        {
            //TODO: clean up this mess!
            if (elm == null) return null;

            HLRevitElementData elementData = null;
            Element workingElm = elm;

            if (paramterId.IntegerValue < -1)
            {

                if (FieldType == FieldTypes.TypeParameter)
                {
                    if (elm is FamilySymbol)
                    {
                        var fs = elm as FamilySymbol;
                        workingElm = fs;
                    }
                    else if (elm is Family)
                    {
                        var fam = elm as Family;
                        workingElm = fam;
                    }
                    else if (elm is FamilyInstance)
                    {
                        var fi = elm as FamilyInstance;
                        workingElm = fi.Symbol;
                    }
                    else if (elm is ElementType)
                    {
                        var et = elm as ElementType;
                        workingElm = et;
                    }
                    else if (elm is MEPCurve)
                    {
                        MEPCurve famIns = elm as MEPCurve;
                        var symbId = famIns.GetTypeId();
                        workingElm = elm.Document.GetElement(symbId);
                    }
                    else if (elm is MEPCurveType)
                    {
                        var et = elm as MEPCurveType;
                        workingElm = et;
                    }
                }

                var param = workingElm.get_Parameter((BuiltInParameter)paramterId.IntegerValue);
                if (param != null) return new HLRevitParameter(param);

                workingElm = elm;

            }


            if (ParameterName == "Family Name" || ParameterName == "Family")
            {
                if (workingElm is FamilySymbol)
                {
                    var fs = workingElm as FamilySymbol;
                    string fsfName = fs.Family != null ? fs.Family.Name : fs.Name;
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, fsfName, StorageType.String, workingElm);
                    return elementData;
                }
                else if (workingElm is Family)
                {
                    var fam = workingElm as Family;
                    string fName = fam.Name;
                    if (fam.Document.IsFamilyDocument && string.IsNullOrEmpty(fName))
                    {
                        fName = System.IO.Path.GetFileNameWithoutExtension(fam.Document.PathName);
                    }
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, fName, StorageType.String, workingElm);
                    return elementData;
                }
                else if (workingElm is FamilyInstance)
                {
                    var fi = workingElm as FamilyInstance;
                    var sym = fi.Symbol;
                    var fifname = sym != null ? sym.Family != null ? sym.Family.Name : sym.Name : fi.Name;
                    elementData = new HLRevitElementName(sym.Family);
                    return elementData;
                    //elementData = new HLRevitElementProperty(elm.Id.IntegerValue, ParameterName, fifname, StorageType.String, elm);
                }

                else if (workingElm is ElementType)
                {
                    var et = workingElm as ElementType;
#if REVIT2015
                    var famName = et.FamilyName;
#else
                    var famName = et.Name;
#endif
                    //elementData = new HLRevitElementName(et);
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, famName, StorageType.String, workingElm);
                    return elementData;
                }
                else if (workingElm is MEPCurveType)
                {
                    var et = workingElm as MEPCurveType;
#if REVIT2015
                    var famName = et.FamilyName;
#else
                    var famName = et.Name;
#endif
                    //elementData = new HLRevitElementName(et);
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, famName, StorageType.String, workingElm);
                    return elementData;
                }

            }

            if (ParameterName == "Type Name" || (redirectType && ParameterName == "Type")) //was removed as type is a valid parameter
            {
                if (workingElm is FamilySymbol)
                {
                    elementData = new HLRevitElementName(workingElm);
                    return elementData;
                }
                else if (workingElm is Family)
                {
                    var fam = workingElm as Family;
                    elementData = new HLRevitElementName(fam);
                    return elementData;
                }
                else if (workingElm is FamilyInstance)
                {
                    var fi = workingElm as FamilyInstance;
                    var sym = fi.Symbol;
                    elementData = new HLRevitElementName(sym);
                    return elementData;
                }

                else if (workingElm is ElementType)
                {
                    var et = workingElm as ElementType;
#if REVIT2015
                    var famName = et.FamilyName;
#else
                    var famName = et.Name;
#endif
                    //elementData = new HLRevitElementName(et);
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, famName, StorageType.String, workingElm);
                    return elementData;
                }
                else if (workingElm is MEPCurveType)
                {
                    var et = workingElm as MEPCurveType;
#if REVIT2015
                    var famName = et.FamilyName;
#else
                    var famName = et.Name;
#endif
                    //elementData = new HLRevitElementName(et);
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, famName, StorageType.String, workingElm);
                    return elementData;
                }

            }
            else if ((ParameterName == "Name") && !(workingElm is SpatialElement))
            {
                elementData = new HLRevitElementName(workingElm);
                return elementData;
            }


            //first redirect element to working element
            if (FieldType == FieldTypes.TypeParameter)
            {
                //redirect to type
                if (elm is FamilySymbol)
                {
                    var fs = elm as FamilySymbol;
                    workingElm = fs;
                }
                else if (elm is Family)
                {
                    var fam = elm as Family;
                    workingElm = fam;
                }
                else if (elm is FamilyInstance)
                {
                    var fi = elm as FamilyInstance;
                    workingElm = fi.Symbol;
                }
                else if (elm is ElementType)
                {
                    var et = elm as ElementType;
                    workingElm = et;
                }
                else if (elm is MEPCurve)
                {
                    MEPCurve famIns = elm as MEPCurve;
                    var symbId = famIns.GetTypeId();
                    workingElm = elm.Document.GetElement(symbId);
                }
                else if (elm is MEPCurveType)
                {
                    var et = elm as MEPCurveType;
                    workingElm = et;
                }
            }

            if (FieldType == FieldTypes.Property)
            {
                //refatoring removed this... not sure what this was supposed to be...
                //try poperties applicable to MEPcurves

            }

            if (workingElm == null) return null;

            //look for this parameter name directly
            Parameter fieldParam = null;
            if (paramterId.IntegerValue != -1)
            {
                fieldParam = workingElm.HLGetParameter(paramterId);
            }

            if (fieldParam == null && paramGuid != Guid.Empty)
            {
                fieldParam = workingElm.HLGetParameter(paramGuid);
            }

            if (fieldParam == null && !string.IsNullOrEmpty(ParameterName))
            {
                fieldParam = workingElm.HLGetParameter(ParameterName);
            }

            if (fieldParam != null) return new HLRevitParameter(fieldParam);

            //explore other parameter name redirects for special cases dependent on element type:

            if (workingElm is MEPCurve)
            {
                var elmMepCurve = workingElm as MEPCurve;
                switch (ParameterName)
                {
                    case "MEPSystemId":
                        if (elmMepCurve.MEPSystem != null)
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, elmMepCurve.MEPSystem.Id, StorageType.ElementId);
                        }
                        else
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, ElementId.InvalidElementId, StorageType.ElementId);
                        }
                        return elementData;

                }
            }

            if (workingElm is FamilyInstance)
            {
                FamilyInstance elmFamInst = workingElm as FamilyInstance;
                switch (ParameterName)
                {
                    case "Family And Type":
                        string fnName = elmFamInst.Symbol.Family != null ? elmFamInst.Symbol.Family.Name + " : " : string.Empty;
                        elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, fnName + workingElm.Name, StorageType.String, workingElm);
                        return elementData;


                    case "Space":
                    case "space":

                        var elmSpaceId = elmFamInst.Space != null ? elmFamInst.Space.Id : ElementId.InvalidElementId;
                        elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, elmSpaceId.IntegerValue, StorageType.ElementId, workingElm);
                        return elementData;


                    case "Room":
                    case "room":

                        var elmRoomId = elmFamInst.Room != null ? elmFamInst.Room.Id : ElementId.InvalidElementId;
                        elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, elmRoomId.IntegerValue, StorageType.ElementId, workingElm);
                        return elementData;


                    case "SpaceId":
                        if (elmFamInst.Space != null)
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, elmFamInst.Space.Id, StorageType.ElementId);
                        }
                        else
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, ElementId.InvalidElementId, StorageType.ElementId);
                        }
                        return elementData;


                    case "RoomId":
                        if (elmFamInst.Room != null)
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, elmFamInst.Room.Id, StorageType.ElementId);
                        }
                        else
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, ElementId.InvalidElementId, StorageType.ElementId);
                        }
                        return elementData;


                    case "FamilySymbolId":
                        if (elmFamInst.Symbol != null)
                        {
                            //elementData = new HLRevitElementName(elmFamInst.Symbol);
                            elementData = new HLRevitElementProperty(elm.Id.IntegerValue, ParameterName, elmFamInst.Symbol.Id, StorageType.ElementId);
                        }
                        else
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, ElementId.InvalidElementId, StorageType.ElementId);
                        }
                        return elementData;


                    case "FamilyId":
                        if ((elmFamInst.Symbol != null) && (elmFamInst.Symbol.Family != null))
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, elmFamInst.Symbol.Family.Id, StorageType.ElementId);
                        }
                        else
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, ElementId.InvalidElementId, StorageType.ElementId);
                        }
                        return elementData;


                }
            }

            if (workingElm is FamilySymbol)
            {
                FamilySymbol fs = workingElm as FamilySymbol;

                switch (ParameterName)
                {
                    case "FamilySymbolId":

                        //elementData = new HLRevitElementName(fs);
                        elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, fs.Id, StorageType.ElementId);
                        return elementData;


                    case "FamilyId":
                        if (fs.Family != null)
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, fs.Family.Id, StorageType.ElementId);
                        }
                        else
                        {
                            elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, ElementId.InvalidElementId, StorageType.ElementId);
                        }
                        return elementData;

                }
            }

            if (workingElm is ElementType || workingElm is MEPCurveType)
            {
                switch (ParameterName)
                {
                    case "FamilySymbolId":
                        //elementData = new HLRevitElementName(workingElm);
                        elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, workingElm.Id, StorageType.ElementId);
                        return elementData;

                }
            }


            switch (ParameterName)
            {
                case "LevelId":
#if REVIT2015
                    if (workingElm.LevelId != null)
                    {
                        elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, workingElm.LevelId, StorageType.ElementId);
#else
                            if (workingElm.Level != null)
                            {
                                elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, workingElm.Level.Id, StorageType.ElementId);
#endif
                    }
                    else
                    {
                        elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, ElementId.InvalidElementId, StorageType.ElementId);
                    }
                    return elementData;


                case "Type":
                case "FamilySymbolId":
                    //elementData = new HLRevitElementName(workingElm);
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, workingElm.Id, StorageType.ElementId);
                    return elementData;


            }


  
            //finally try refection to get property of the API object
            try
            {
                var prop = workingElm.GetType().GetProperties().FirstOrDefault(pr => pr.Name == ParameterName);
                if (prop != null)
                {
                    object elmVal = prop.GetValue(workingElm, null);
                    elementData = new HLRevitElementProperty(workingElm.Id.IntegerValue, ParameterName, elmVal, StorageType.String, workingElm);
                    return elementData;
                }
            }
            catch
            {
                //log exception (or not)
            }


            return elementData;
        }
    }


    public enum FieldTypes
    {
        InstanceParameter,
        TypeParameter,
        Property,
        Calculated,
        Referenced,
    }

}
