using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using BuildingGraph.Client.Kafka;

namespace BuildingGraph.Integration.Revit.Streams
{
    public class RevitStreamConsumer
    {

        Client.BuildingGraphClient _client;
        

        public ICollection<ElementStreamHandler> StreamHandlers = new List<ElementStreamHandler>();

        public RevitStreamConsumer()
        {

            kafkaConsumer = new KafkaConsumer("localhost:9092");

            kafkaConsumer.NewMessageArrived += KafkaConsumer_NewMessageArrived;  

            _client = new Client.BuildingGraphClient(@"http://localhost:4002/graphql", null);

        }


        private void KafkaConsumer_NewMessageArrived(StreamMessage message)
        {
            
            foreach (var handler in StreamHandlers)
            {
                handler.processRemote(message);
            }
        }

        public KafkaConsumer kafkaConsumer { get; private set; }


        public void Start()
        {
            kafkaConsumer.StartAsync("lc-ng", new string[] { "ext-service-abstractElements" });
        }

        //cache of DBIDs to Revit IDs

        public void RefreshCacheFromGraph(Document doc)
        {
            //pull all elements

            var schme = _client.GetSchema();

            var docIdent = HLApps.Revit.Utils.DocUtils.GetDocumentIdent(doc);
            var docVars = new Dictionary<string, object>();
            docVars.Add("modelIdent", docIdent);
            var modelElementsQuery = @"query($modelIdent:String){
  Model (Identity:$modelIdent){
    Identity
    ModelElements {
      UniqueId
      AbstractElement {
        __typename
        Id
        Name
      }
    }
  }
}";

            var modelElementQResult = _client.ExecuteQuery(modelElementsQuery, docVars);
            


        }

    }



}
