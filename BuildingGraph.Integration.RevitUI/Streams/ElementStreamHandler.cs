
using Autodesk.Revit.DB;
using BuildingGraph.Client.Kafka;
using BuildingGraph.Integration.Revit.UIAddin;
using BuildingGraph.Integration.Revit;
using BuildingGraph.Client;
using HLApps.Revit.Parameters;
using System.Linq;
using System.Collections.Generic;
using System;

namespace BuildingGraph.Integration.Revit.Streams
{
    /// <summary>
    /// Local logic to handle crud operations on elements from the message stream.
    /// inserts new families
    /// Adds parameters to families
    /// Updates values
    /// </summary>
    public class ElementStreamHandler //TODO: make interface
    {
        BuildingGraph.Client.BuildingGraphClient _client;
        string Label;

        /// <summary>
        /// The topics this handler should respond to
        /// </summary>
        string Topic = "ext-service-abstractElements";
        RevitEventDispatcher _dispatcher;

        public ElementStreamHandler(RevitEventDispatcher dispatcher, BuildingGraphClient client)
        {
            _dispatcher = dispatcher;
            _client = client;
        }

        Autodesk.Revit.DB.ElementFilter filter;

        public void processRemote(StreamMessage message) 
        {


            if (message.Operation == MessageOperation.Updated)
            {
                UpdateLocal(message);
            }

            if (message.Operation == MessageOperation.Created)
            {
                CreateLocal(message);
            }

        }



        public void processLocal(Element element) { }

        public void UpdateLocal(StreamMessage message) {

            //add parameters
            //get unit and types from schema?

            _dispatcher.QueueAction((uiapp) =>
            {
                var doc = uiapp.ActiveUIDocument.Document;

                var node = message.After.AsNode();
                var nodeElement = doc.GetElement(node.Id);
                if (nodeElement == null) return;

                using (var updatetx = new Transaction(doc, "Update from stream"))
                {
                    updatetx.Start("Update from stream");


                    var nodeparamers = node.ExtendedProperties;
                    foreach (var param in nodeElement.Parameters.OfType<Parameter>())
                    {
                        if (param.IsReadOnly) continue;

                        var hp = new HLRevitParameter(param);
                        var paramName = Utils.GetGraphQLCompatibleFieldName(param.Definition.Name);
                       
                        if (nodeparamers.ContainsKey(paramName))
                        {
                            var incommingValue = nodeparamers[paramName];
                            var currentValue = MEPGraphUtils.RevitToGraphValue(hp);

                            if (incommingValue != currentValue)
                            {
                                hp.Value = incommingValue;
                            }
                        }
                    }


                    updatetx.Commit();

                }

            });



        }


        /// <summary>
        /// Creates the element in the model
        /// </summary>
        /// <param name="doc"></param>
        public void CreateLocal(StreamMessage message) 
        {
            //insert at position?
            //find space it's in?
            //find family symbol/type?
            _dispatcher.QueueAction((uiapp) =>
            {
                var doc = uiapp.ActiveUIDocument.Document;

                var node = message.After.AsNode();
                var nodeElement = doc.GetElement(node.Id);

                using (var updatetx = new Transaction(doc, "Update from stream"))
                {
                    updatetx.Start("Update from stream");

                    if (node.Labels.Contains("FanCoilUnit"))
                    {
                        if (nodeElement == null)
                        {
                            //create node
                            //
                            //get the space it's in

                            var query = @"
query($fcuid:ID!){
 FanCoilUnit (Id:$fcuid){
   Id
   ModelElements{
      Id
      UniqueId
     }
   Space{
        Id
     }
    ConnectedTo{
      Id
      Apparent_Load
      ModelElements{
       Id
       UniqueId
     }
    }
   }
}";
                            var vars = new Dictionary<string, object>();
                            vars.Add("fcuid", node.Id);

                            var res = _client.ExecuteQuery(query, vars);

                            if (res.FanCoilUnit != null)
                            {
                                var fcu = res.FanCoilUnit[0];
                                Element modelElm = null;
                                foreach (var modelelemet in fcu.ModelElements)
                                {
                                    var id = modelelemet.UniqueId.Value;
                                    modelElm = doc.GetElement((string)id);
                                    if (modelElm != null) break;
                                }

                                if (fcu.Space != null)
                                {
                                    var spaceId = fcu.Space.Id.Value;

                                    //find space
                                    var spaceElm = doc.GetElement((string)spaceId) as SpatialElement;
                                    if (spaceElm != null)
                                    {
                                        //find center
                                        MEPRevitNode rv = new MEPRevitNode(spaceElm);

                                        var sbopt = new SpatialElementBoundaryOptions();
                                        sbopt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;
                                        sbopt.StoreFreeBoundaryFaces = true;
                                        SpatialElementGeometryCalculator sg = new SpatialElementGeometryCalculator(doc, sbopt);
                                        var spgets = sg.CalculateSpatialElementGeometry(spaceElm);

                                        var spgeo = spgets.GetGeometry();
                                        var center = spgeo.ComputeCentroid();
                                        var bbox = spgeo.GetBoundingBox();
                                        var heightOfUnit = 1;
                                        var randomOffset = (bbox.Max.X - bbox.Min.X) / 2 * (new Random().NextDouble() - 0.5) ;
                                        var inspoint = new XYZ(center.X + randomOffset, center.Y, bbox.Max.Z - heightOfUnit);

                                      
                                        FilteredElementCollector famFilter = new FilteredElementCollector(doc);

                                        //this is static for this example. We will need to discover the type and pull the family
                                        //from a family library service.
                                        string familyName = "HL_FanCoilUnit_HotandChilledWaterSoffit3HorizontalRoundConnections";

                                        Family foundFam = famFilter.OfClass(typeof(Family)).ToElements().OfType<Family>().FirstOrDefault(fm => fm.Name == familyName);
                                        var fcuSymbol = doc.GetElement(foundFam.GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;

                                        var newElm = doc.Create.NewFamilyInstance(inspoint, fcuSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);


                                        //var props = MEPGraphUtils.GetNodePropsWithElementProps(new Client.Model.ModelElement(), newElm);
                                        //var newnode = _client.Push("ModelElement", props);
                                        //_client.Relate(new PendingNode(node), newnode, Client.Model.MEPEdgeTypes.REALIZED_BY, null);



                                        var nodeparamers = node.ExtendedProperties;
                                        foreach (var param in newElm.Parameters.OfType<Parameter>())
                                        {
                                            if (param.IsReadOnly) continue;

                                            var hp = new HLRevitParameter(param);
                                            var paramName = Utils.GetGraphQLCompatibleFieldName(param.Definition.Name);

                                            if (nodeparamers.ContainsKey(paramName))
                                            {
                                                var incommingValue = nodeparamers[paramName];
                                                var currentValue = MEPGraphUtils.RevitToGraphValue(hp);

                                                if (incommingValue != currentValue)
                                                {
                                                    hp.Value = incommingValue;
                                                }
                                            }
                                        }

                                        if (fcu.ConnectedTo != null)
                                        {
                                            string fcuName = "FCU";

                                            Family foundfcuFam = famFilter.OfClass(typeof(Family)).ToElements().OfType<Family>().FirstOrDefault(fm => fm.Name == fcuName);
                                            var fcueSymbol = doc.GetElement(foundFam.GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
                                            var fcuEinspt = inspoint + new XYZ(1, 0, 0);
                                            var newFuceElm = doc.Create.NewFamilyInstance(fcuEinspt, fcuSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                                   
                                            foreach (var param in newFuceElm.Parameters.OfType<Parameter>())
                                            {
                                                if (param.IsReadOnly) continue;

                                                var hp = new HLRevitParameter(param);
                                                var paramName = Utils.GetGraphQLCompatibleFieldName(param.Definition.Name);

                                                if (fcu.ConnectedTo[paramName] != null)
                                                { 
                                                    var incommingValue = fcu.ConnectedTo[paramName].Value;
                                                    var currentValue = MEPGraphUtils.RevitToGraphValue(hp);

                                                    if (incommingValue != currentValue)
                                                    {
                                                        hp.Value = incommingValue;
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }

             
                            }

                        }
                    }

                    updatetx.Commit();

                }

            });

        }

        public void DeleteLocal(Document doc) { }

        public void UpdateRemote(Document doc) { 
            //GraphQL update op

        }
        public void CreateRemote(Document doc) {
            //GraphQL create op for Element
            //relate to abstract

        }
        public void DeleteRemote(Document doc) { }
    }



}
