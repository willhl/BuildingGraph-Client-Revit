using System.Collections.Generic;
using System.Linq;
using HLApps.Revit.Utils;
using BuildingGraph.Client;
using BuildingGraph.Client.Introspection;
using Model = BuildingGraph.Client.Model;

namespace BuildingGraph.Integrations.Revit
{
    public class MEPGraphWriterGraphQL //: IMEPGraphWriter
    {
        BuildingGraphClient _gdbClient;
        public MEPGraphWriterGraphQL(BuildingGraphClient gdbClient )
        {
            _gdbClient = gdbClient;
        }


        public void Write(MEPRevitGraph mepGraph, BuildingGraphMapping clientMapping, Autodesk.Revit.DB.Document rootDoc)
        {

            var track = new Dictionary<MEPRevitNode, PendingNode>();
            var models = new Dictionary<string, PendingNode>();
            var types = new Dictionary<string, PendingNode>();
            var levels = new Dictionary<string, PendingNode>();

            var rootModelNode = new Model.Model();
            var rootModelIdent = DocUtils.GetDocumentIdent(rootDoc);
            rootModelNode.Name = rootDoc.PathName;
            rootModelNode.ExtendedProperties.Add("Identity", rootModelIdent);
            rootModelNode.ExtendedProperties.Add("DateTimeStamp", System.DateTime.Now.ToShortDateString());

            var schema = _gdbClient.GetSchema();

            //gather data from the graph about nodes which already exist
            string dGather = @"query($modelIdent:String){
  Model (Identity:$modelIdent){
    Identity
    ModelElements {
      UniqueId
      AbstractElement {
        NodeType: __typename
        Name
        ... on Space{
          Number
          Name
          Area
          Id
        }
        ... on Level{
          Name
          Elevation
        }
      }
    }
  }
}";


            var vars = new Dictionary<string, object>();
            vars.Add("modelIdent", rootModelIdent);
            var res = _gdbClient.ExecuteQuery(dGather, vars);

            var dbModels = res.Result.Model;
            dynamic dbRootModel = null; 
            Dictionary<string, dynamic> modElmCache = new Dictionary<string, dynamic>();
            if (models != null)
            {
                foreach (var model in models)
                {
                    dbRootModel = model;
                    foreach (var modelElement in dbModels.ModelElements)
                    {
                        var meID = modelElement.UniqueId.Value;
                        if (meID == null) continue;
                        if (!modElmCache.ContainsKey(meID))
                        {
                            modElmCache.Add(modelElement.UniqueId.Value, modelElement);
                        }
                        else
                        {
                            //there shouldn't be multiple
                            modElmCache[meID] = modelElement;
                        }
                    }
                }
            }


        


        var rootparams = MEPGraphUtils.GetNodePropsWithElementProps(rootModelNode, rootDoc.ProjectInformation, schema, clientMapping, true);
            PendingNode seid = null;

            if (dbRootModel == null)
            {
                seid = _gdbClient.Push(rootModelNode.Label, rootparams);
            }
            else
            {
                seid = _gdbClient.Push(rootModelNode.Label, rootparams);
            }

            models.Add(rootModelIdent, seid);

            var exprops = new Dictionary<string, object>();
            exprops.Add("test", 1);


            //add the nodes
            foreach (var mepNode in mepGraph.Nodes)
            {

                var npNode = mepNode.AsAbstractNode;
                var elmAbParams = npNode.GetAllProperties();


                if (!string.IsNullOrEmpty(mepNode.OrginDocIdent))
                {
                    var elmNode = mepNode.AsElementNode;
                    var elmParms = elmNode.GetAllProperties();

                    var elmdoc = DocUtils.GetDocument(mepNode.OrginDocIdent, rootDoc.Application);
                    var elm = elmdoc.GetElement(new Autodesk.Revit.DB.ElementId(mepNode.OriginId));
                    if (elm != null)
                    {

                        elmParms = MEPGraphUtils.GetNodePropsWithElementProps(elmNode, elm, schema, clientMapping, true);
                        elmAbParams = MEPGraphUtils.GetNodePropsWithElementProps(npNode, elm, schema, clientMapping, true);
                    }

                    var atid = _gdbClient.Push(npNode.Label, elmAbParams);
                    track.Add(mepNode, atid);

                    var elmid = _gdbClient.Push(elmNode.Label, elmParms);

                    //relate the element node to the abstracted model node
                    _gdbClient.Relate(atid, elmid, Model.MEPEdgeTypes.REALIZED_BY, null);


                    PendingNode modelId = null;
                    //create up model nodes
                    if (models.ContainsKey(mepNode.OrginDocIdent))
                    {
                        modelId = models[mepNode.OrginDocIdent];
                    }
                    else
                    {
                        var modelNode = new Model.Model();
                        modelNode.ExtendedProperties.Add("Identity", mepNode.OrginDocIdent);
                        var mparams = modelNode.GetAllProperties();

                        var ldoc = DocUtils.GetDocument(mepNode.OrginDocIdent, rootDoc.Application);
                        if (ldoc != null)
                        {
                            mparams = MEPGraphUtils.GetNodePropsWithElementProps(modelNode, ldoc.ProjectInformation, schema, clientMapping, true);
                        }

                        modelId = _gdbClient.Push(modelNode.Label, mparams);
                        models.Add(mepNode.OrginDocIdent, modelId);
                    }

                    var elmedgeProps = MEPGraphUtils.GetEdgeProps(elm);
                    //connect up model node to element node
                    _gdbClient.Relate(elmid, modelId, Model.MEPEdgeTypes.IS_IN, elmedgeProps);

                    Autodesk.Revit.DB.Element typeElm = null;


                    if (elm is Autodesk.Revit.DB.FamilyInstance)
                    {
                        typeElm = (elm as Autodesk.Revit.DB.FamilyInstance).Symbol;

                    }
                    else
                    {
                        var mpType = elm.GetTypeId();
                        typeElm = elmdoc.GetElement(mpType);
                    }



                    //create type nodes
                    if (typeElm != null)
                    {
                        PendingNode tsId = null;
                        if (!types.ContainsKey(typeElm.UniqueId))
                        {
                            var edgeProps = MEPGraphUtils.GetEdgeProps(typeElm);
                            var tsNode = new Model.ElementType();
                            tsNode.Name = typeElm.Name;
                            var tsprops1 = MEPGraphUtils.GetNodePropsWithElementProps(tsNode, typeElm, schema, clientMapping, true);

                            tsId = _gdbClient.Push(tsNode.Label, tsprops1);
                            types.Add(typeElm.UniqueId, tsId);

                            var tselmNode = new Model.ModelElement();
                            var tsprops2 = MEPGraphUtils.GetNodePropsWithElementProps(tsNode, typeElm, schema, clientMapping, true);
                            var tselmId = _gdbClient.Push(tselmNode.Label, tsprops2);

                            _gdbClient.Relate(tsId, tselmId, Model.MEPEdgeTypes.REALIZED_BY, exprops);
                            _gdbClient.Relate(tselmId, modelId, Model.MEPEdgeTypes.IS_IN, edgeProps);
                        }
                        else
                        {
                            tsId = types[typeElm.UniqueId];
                        }
                        _gdbClient.Relate(atid, tsId, Model.MEPEdgeTypes.IS_OF, exprops);

                    }

                    //create level nodes
                    var lvl = elmdoc.GetElement(new Autodesk.Revit.DB.ElementId(mepNode.LevelId));
                    if (lvl != null)
                    {
                        var edgeProps = MEPGraphUtils.GetEdgeProps(lvl);
                        PendingNode lvlId = null;
                        if (!levels.ContainsKey(lvl.UniqueId))
                        {
                            var lvlNode = new Model.Level();
                            lvlNode.Name = lvl.Name;
                            var lvlprops = MEPGraphUtils.GetNodePropsWithElementProps(lvlNode, lvl, schema, clientMapping, true);
                            lvlId = _gdbClient.Push(lvlNode.Label, lvlprops);
                            levels.Add(lvl.UniqueId, lvlId);
                            _gdbClient.Relate(lvlId, modelId, Model.MEPEdgeTypes.IS_IN, edgeProps);
                        }
                        else
                        {
                            lvlId = levels[lvl.UniqueId];
                        }

                        _gdbClient.Relate(atid, lvlId, Model.MEPEdgeTypes.IS_ON, exprops);

                    }

                }
                else
                {
                    var modelId = _gdbClient.Push(npNode.Label, elmAbParams);
                    track.Add(mepNode, modelId);
                }


            }

            //now add the adjacencies 
            foreach (var mepEdge in mepGraph.Edges)
            {
                if (!track.ContainsKey(mepEdge.ThisNode))
                {
                    continue;
                }

                if (!track.ContainsKey(mepEdge.NextNode))
                {
                    continue;
                }

                var nid1 = track[mepEdge.ThisNode];
                var nid2 = track[mepEdge.NextNode];

                var edPArams = new Dictionary<string, object>();
                foreach (var wkvp in mepEdge.Weights)
                {
                    edPArams.Add(wkvp.Key, wkvp.Value);
                }


                _gdbClient.Relate(nid1, nid2, mepEdge.AsNodeEdge.EdgeType, edPArams);
            }


            //add systems and connections
            foreach (var system in mepGraph.Systems)
            {

                var sysNode = new Model.System();

                var syselm = rootDoc.GetElement(new Autodesk.Revit.DB.ElementId(system));
                var srops = MEPGraphUtils.GetNodePropsWithElementProps(sysNode, syselm, schema, clientMapping, true);
                var sysNodeId = _gdbClient.Push(sysNode.Label, srops);

                var tselmNode = new Model.ModelElement();
                tselmNode.ExtendedProperties.Add("UniqueId", syselm.UniqueId);

                var emprops = MEPGraphUtils.GetNodePropsWithElementProps(tselmNode, syselm, schema, clientMapping, true);
                var tselmId = _gdbClient.Push(tselmNode.Label, emprops);
                _gdbClient.Relate(sysNodeId, tselmId, Model.MEPEdgeTypes.REALIZED_BY, null);
                var edgeProps = MEPGraphUtils.GetEdgeProps(syselm);
                _gdbClient.Relate(tselmId, seid, Model.MEPEdgeTypes.IS_IN, edgeProps);


                var stypeId = syselm.GetTypeId();
                var typeElm = rootDoc.GetElement(stypeId);
                if (typeElm != null)
                {
                    PendingNode tsId = null;
                    if (!types.ContainsKey(typeElm.UniqueId))
                    {
                        var stypeedgeProps = MEPGraphUtils.GetEdgeProps(typeElm);
                        var tsNode = new Model.ElementType();
                        tsNode.Name = typeElm.Name;
                        var tsprops1 = MEPGraphUtils.GetNodePropsWithElementProps(tsNode, typeElm, schema, clientMapping, true);

                        tsId = _gdbClient.Push(tsNode.Label, tsprops1);
                        types.Add(typeElm.UniqueId, tsId);

                        var sysTypeelmNode = new Model.ModelElement();
                        var tsprops2 = MEPGraphUtils.GetNodePropsWithElementProps(tsNode, typeElm, schema, clientMapping, true);
                        var sysTypeelmId = _gdbClient.Push(sysTypeelmNode.Label, tsprops2);

                        _gdbClient.Relate(tsId, sysTypeelmId, Model.MEPEdgeTypes.REALIZED_BY, null);
                        _gdbClient.Relate(tselmId, seid, Model.MEPEdgeTypes.IS_IN, stypeedgeProps);
                    }
                    else
                    {
                        tsId = types[typeElm.UniqueId];
                    }

                    _gdbClient.Relate(sysNodeId, tsId, Model.MEPEdgeTypes.IS_OF, null);

                }

                var snodes = mepGraph.GetAllNodesForSystem(system);
                foreach (var snd in snodes)
                {
                    if (!track.ContainsKey(snd))
                    {
                        continue;
                    }

                    var rid = track[snd];
                    _gdbClient.Relate(rid, sysNodeId, Model.MEPEdgeTypes.ABSTRACTED_BY, null);
                }

            }

            _gdbClient.Commit();


        }



    }
}
