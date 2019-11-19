using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Dynamic;

//using System.Net.Http;
using GraphQL.Client.Http;
using GraphQL.Client;
using GraphQL.Common.Request;
using Nito.AsyncEx;
using BuildingGraph.Client.Introspection;
using BuildingGraph.Client.Model;

namespace BuildingGraph.Client
{
    public class BuildingGraphClient //: IGraphDBClient
    {
        string _endPointUrl;
        GraphQLHttpClient _client;
        BuildingGraphMapping _clientMapping;
        Queue<MutationRequest> _mutationPushQueue;
        Queue<MutationRequest> _mutationRelateQueue;
        IBuildingGraphSchema _schemaCache;
        //HttpClient _client;
        public BuildingGraphClient(string endPointUrl) : this(endPointUrl, null)
        {

        }

        public BuildingGraphClient(string endPointUrl, BuildingGraphMapping clientMapping)
        {
            _endPointUrl = endPointUrl;
            _clientMapping = clientMapping;
            _mutationPushQueue = new Queue<MutationRequest>();
            _mutationRelateQueue = new Queue<MutationRequest>();
        }


        public async Task<dynamic> ExecuteQueryAsync(string query, Dictionary<string, object> args)
        {

            try
            {

                if (_client == null) _client = new GraphQLHttpClient(_endPointUrl);

                dynamic eo = null;
                if (args != null && args.Count > 0)
                {
                    eo = args.Aggregate(new ExpandoObject() as IDictionary<string, Object>, (a, p) => { a.Add(p.Key, p.Value); return a; });
                }

                var assentRequest = new GraphQLRequest()
                {
                    Query = query,
                    Variables = eo
                };


                var graphQLResponse = await _client.SendQueryAsync(assentRequest);// SendQueryAsync(assentRequest);

                return graphQLResponse.Data;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public async Task<dynamic> ExecuteMutationAsync(string query, Dictionary<string, object> args)
        {

            try
            {

                if (_client == null) _client = new GraphQLHttpClient(_endPointUrl);

                //turn args into anonymous object
                dynamic eo = args.Aggregate(new ExpandoObject() as IDictionary<string, Object>, (a, p) => { a.Add(p.Key, p.Value); return a; });

                var assentRequest = new GraphQLRequest()
                {
                    Query = query,
                    Variables = eo
                };


                //var graphQLString = JsonConvert.SerializeObject(assentRequest, _client.Options.JsonSerializerSettings);

                var graphQLResponse = await _client.SendMutationAsync(assentRequest);// SendQueryAsync(assentRequest);

                return graphQLResponse.Data;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }

        public dynamic ExecuteQuery(string query, Dictionary<string, object> variables)
        {
            var task = Task.Run(() => AsyncContext.Run(() => ExecuteQueryAsync(query, variables)));
            task.Wait();
            return task.Result;
        }

        public dynamic ExecuteMutation(string query, Dictionary<string, object> args)
        {
            //var task = Task.Run(() => AsyncContext.Run(() => ExecuteMutationAsync(query, args)));
            var task = ExecuteMutationAsync(query, args);
            task.RunSynchronously();
            return task.Result;
        }


        public bool IsSupportedMutation(string nodeName, NodeMutationType mutation)
        {

            string mutationPrefix = string.Empty;

            switch (mutation)
            {
                case NodeMutationType.Create:
                    mutationPrefix = "Create";
                    break;

                case NodeMutationType.Update:
                    mutationPrefix = "Update";
                    break;

                case NodeMutationType.Delete:
                    mutationPrefix = "Delete";
                    break;
            }
            var mutationName = mutationPrefix + nodeName;

            if (_schemaCache == null) _schemaCache = GetSchema();
            var mutationField = _schemaCache.GetMutations(nodeName).FirstOrDefault(m => m.Name == mutationName);
            return mutationField != null;

        }

        public IBuildingGraphSchema GetSchema()
        {

            var schema = ExecuteQuery(@"
{
__schema {
    types {
      interfaces {
        name
      }
      kind
      name
      description
      fields {
        name
        description
        type {
          name
          kind
          ofType{
            name
          }
        }
        args{
          name
          defaultValue
          type{
            name
            kind
            ofType{
            name
          }
          }
        }
      }
      enumValues{
        name
        description
      }
    }
  }
}", null);
            if (schema is Exception) throw new Exception("There was an error qurying the API", (schema as Exception));

            IBuildingGraphSchema bdSchema = null;
            try
            {
                if (schema == null || schema.__schema == null)
                {
                    throw new Exception("Error querying schema " + (schema is string ? schema : string.Empty) );
                }
                bdSchema = new BuildingGraphSchema(schema.__schema);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error qurying the API", ex);
            }

            return bdSchema;
        }



        public void Dispose()
        {

        }


        public PendingNode Push(string nodeName, Dictionary<string, object> variables)
        {
            return Push(nodeName, variables, null);
        }

        /// <summary>
        /// Adds or Updates a node in the graph
        /// </summary>
        /// <param name="node">The node to add/update</param>
        /// <param name="variables">Variables to add/update for this node</param>
        /// <param name="mergeOn">To update a node, add Ids or other identifiing variables to match the existing node. Leave null to create a new node.</param>
        /// <returns>The pending node which can be used to relate it to other pending nodes</returns>
        public PendingNode Push(string nodeName, Dictionary<string, object> variables, Dictionary<string, object> mergeOn)
        {

            //find create mutation
            //find parameters
            //translate values?


            //map node name to GrapgQL type
            BuildingGraphMappedType mappedType = null;
            if (_clientMapping != null)
            {
                mappedType = _clientMapping.Types.FirstOrDefault(t => t.NativeType == nodeName);
            }

            var IsUpdate = (mergeOn != null);
            var mutationPrefix = IsUpdate ? "Update" : "Create";
            var mutationName = mutationPrefix + nodeName;

            if (_schemaCache == null) _schemaCache = GetSchema();
            var mutationField = _schemaCache.GetMutations(nodeName).FirstOrDefault(m => m.Name == mutationName);
            if (mutationField == null) return new PendingNode(nodeName);// throw new Exception("Unknown mutation type: " + mutationName);

            //map node variable name to GraphQL variable name
            Dictionary<string, object> mutationArgs = null;
            if (mappedType == null)
            {
                mutationArgs = new Dictionary<string, object>(variables);
            }
            else
            {
                mutationArgs = new Dictionary<string, object>(variables);
            }

            if (IsUpdate)
            {
                //find matching id arguments for merge operation
                var mergeArgs = mutationField.Args.Where(arg => arg.TypeName == "ID!" && mergeOn.ContainsKey(arg.Name));
                foreach (var marg in mergeArgs)
                {
                    if (!mutationArgs.ContainsKey(marg.Name)) mutationArgs.Add(marg.Name, mergeOn[marg.Name]);
                }
            }

            var pn = new PendingNode(nodeName);
            MutationRequest mr = new MutationRequest(mutationField, pn, mutationArgs) { ReturnFields = "Id" };

            _mutationPushQueue.Enqueue(mr);
            return pn;

        }

        public void Detach(PendingNode fromNodeId, PendingNode toNodeId, MEPEdgeTypes relType)
        {
            MutateRelationship(fromNodeId, toNodeId, relType.ToString(), null, "Remove");
        }

        public void Detach(PendingNode fromNodeId, PendingNode toNodeId, string relType)
        {
            MutateRelationship(fromNodeId, toNodeId, relType, null, "Remove");

        }
        public void Relate(PendingNode fromNodeId, PendingNode toNodeId, MEPEdgeTypes relType, Dictionary<string, object> variables)
        {
            MutateRelationship(fromNodeId, toNodeId, relType.ToString(), variables, "Add");
        }

        public void Relate(PendingNode fromNodeId, PendingNode toNodeId, string relType, Dictionary<string, object> variables)
        {
            MutateRelationship(fromNodeId, toNodeId, relType, variables, "Add");
        }


        private void MutateRelationship(PendingNode fromNodeId, PendingNode toNodeId, string relTypeName, Dictionary<string, object> variables, string rMutType)
        {

            if (_schemaCache == null) _schemaCache = GetSchema();
            //basic name pattern match, nothing fancy... could use mapping in JSON file but keep it simple for now
            var mutations = _schemaCache.GetMutations();// toNodeId.NodeName);
            //var relTypeName = relType.ToString();

            var mutationName = $"{rMutType}_{fromNodeId.NodeName}_{relTypeName}_{toNodeId.NodeName}";
            var mutationField = mutations.FirstOrDefault(m => m.Name == mutationName);
            //find matching interfaces? just support for AbstractElement for now

            var toNodeType = _schemaCache.GetBuildingGraphType(toNodeId.NodeName);
            var fromNodeType = _schemaCache.GetBuildingGraphType(fromNodeId.NodeName);

            if (mutationField == null)
            {
                foreach (var toNodInf in toNodeType.Interfaces)
                {
                    //var toNodInf = toNodeType.Interfaces.FirstOrDefault();
                    foreach (var fromNodInf in fromNodeType.Interfaces)
                    {
                        //var fromNodInf = fromNodeType.Interfaces.FirstOrDefault();

                        if (mutationField == null && !string.IsNullOrEmpty(fromNodInf))
                        {
                            mutationName = $"{rMutType}_{fromNodInf}_{relTypeName}_{toNodeId.NodeName}";
                            mutationField = mutations.FirstOrDefault(m => m.Name == mutationName);
                        }

                        if (mutationField == null && !string.IsNullOrEmpty(toNodInf))
                        {
                            mutationName = $"{rMutType}_{fromNodeId.NodeName}_{relTypeName}_{toNodInf}";
                            mutationField = mutations.FirstOrDefault(m => m.Name == mutationName);
                        }

                        if (mutationField == null && !string.IsNullOrEmpty(fromNodInf) && !string.IsNullOrEmpty(toNodInf))
                        {
                            mutationName = $"{rMutType}_{fromNodInf}_{relTypeName}_{toNodInf}";
                            mutationField = mutations.FirstOrDefault(m => m.Name == mutationName);
                        }
                        if (mutationField != null) break;
                    }
                    if (mutationField != null) break;
                }
            }

            var mutationArgs = variables != null ? new Dictionary<string, object>(variables) : new Dictionary<string, object>();
            mutationArgs.Add("fromId", fromNodeId);
            mutationArgs.Add("toId", toNodeId);
            //mutationArgs.Add("MutationDateTime", "");
            //mutationArgs.Add("MutationUser", "");

            if (mutationField != null)
            {
                MutationRequest mr = new MutationRequest(mutationField, toNodeId, mutationArgs);
                _mutationRelateQueue.Enqueue(mr);
            }
            else
            {
                throw new Exception("No supported mutations for " + relTypeName);
            }

        }


        public ICollection<PendingNode> Commit()
        {
            //var res = CommitAsync();

            var task = Task.Run(() => AsyncContext.Run(() => CommitAsync()));

            //res.Wait();
            task.Wait();

            return task.Result;

        }

        public async Task<ICollection<PendingNode>> CommitAsync()
        {
            var resPush = await CommitAsync(_mutationPushQueue);
            var resRelate = await CommitAsync(_mutationRelateQueue);

            return resPush;
        }

        private async Task<ICollection<PendingNode>> CommitAsync(Queue<MutationRequest> _mutationQueue)
        {
            if (_mutationQueue.Count == 0) return null;
            //the variables of the current mutation request
            var inputVariables = new Dictionary<string, MutationArg>();
            var mutationQueries = new List<MutationRequest>();

            while (_mutationQueue.Count > 0)
            {
                var pendingMutation = _mutationQueue.Dequeue();

                //find matching variables to mutation field args
                var matchingArgs = pendingMutation.Mutation.Args.Where(arg => pendingMutation.Variables.Keys.Any(var => var == arg.Name));

                var argBuilder = new List<string>();
                foreach (var arg in matchingArgs)
                {
                    string inputVarName = "input" + arg.Name;

                    var inputValue = pendingMutation.Variables[arg.Name];
                    if (inputValue is PendingNode) inputValue = (inputValue as PendingNode).TempId;

                    var inputValueStr = inputValue != null ? inputValue.ToString() : null;

                    var confilictCount = 0;
                    var sharedInputName = inputVarName;
                    while (inputVariables.ContainsKey(sharedInputName)
                        && (inputVariables[sharedInputName].TypeName != arg.TypeName
                        || (inputVariables[sharedInputName].Value != null ? inputVariables[sharedInputName].Value.ToString() : null)
                        != inputValueStr))
                    {
                        sharedInputName = inputVarName + ++confilictCount;
                    }

                    argBuilder.Add($"{arg.Name}:$" + sharedInputName);


                    if (!inputVariables.ContainsKey(sharedInputName))
                    {
                        if (arg.TypeName == "String" && inputValue != null && !(inputValue is string))
                        {
                            inputValue = inputValue.ToString();
                        }

                        inputVariables.Add(sharedInputName, new MutationArg(arg.TypeName, sharedInputName, inputValue));
                    }
                }

                var qstring = string.Empty;
                pendingMutation.MutationAlias = $"m{mutationQueries.Count}";
                var returnFields = string.IsNullOrEmpty(pendingMutation.ReturnFields) ? "" : "{" + pendingMutation.ReturnFields + "}";
                if (argBuilder.Count > 0)
                {
                    //all nodes should implement BaseElement so we can be sure they have an Id property.
                    //we're going to need this value later so we can relate nodes
                    qstring = $"{pendingMutation.MutationAlias} : {pendingMutation.Mutation.Name}({string.Join(" ", argBuilder)}){returnFields}";
                }
                else
                {
                    //all nodes should implement BaseElement so we can be sure they have an Id property.
                    //we're going to need this value later so we can relate nodes
                    qstring = $"{pendingMutation.MutationAlias} : {pendingMutation.Mutation.Name}{returnFields}";
                }
                pendingMutation.Query = qstring;
                mutationQueries.Add(pendingMutation);
            }

            var queryFunction = "mutation";

            if (inputVariables.Count > 0)
            {
                var functionInputs = string.Join(" ", inputVariables.Select(kvp => kvp.Value.FullArgName));
                queryFunction = $"{queryFunction} ({functionInputs}) {"{"}";
            }


            var query = $"{queryFunction}\n{string.Join("\n", mutationQueries.Select(pn => pn.Query))} {"\n}"}";

            var queryVariables = new Dictionary<string, object>();
            foreach (var marg in inputVariables)
            {
                queryVariables.Add(marg.Value.Name, marg.Value.Value);
            }

            var result = await ExecuteMutationAsync(query, queryVariables);

            if (result is Exception)
            {
                throw new Exception("Error with mutation query: " + query, result as Exception);
            }

            foreach (var mutation in mutationQueries)
            {
                var mres = result[mutation.MutationAlias];
                var dbuuid = mres != null && mres.Id != null ? mres.Id.Value : string.Empty;
                mutation.DB_UUID = dbuuid;
                mutation.Nodes.ForEach(n => n.SetCommited(dbuuid));
            }

            return mutationQueries.SelectMany(mu => mu.Nodes).ToList();
        }


    }

    public class MutationArg
    {
        public MutationArg(string typeName, string argName, object value)
        {
            TypeName = typeName;
            Name = argName;
            Value = value;
        }

        public string TypeName;
        public string Name;
        public object Value;
        public string FullArgName
        {
            get
            {
                return $"${Name}:{TypeName}";
            }
        }
    }

    internal class MutationRequest
    {
        public MutationRequest(IBuildingGraphField mutation, PendingNode node, Dictionary<string, object> variables)
        {
            Variables = variables;
            Mutation = mutation;
            Nodes = new List<PendingNode>();
            Nodes.Add(node);
        }

        public Dictionary<string, object> Variables { get; }
        public List<PendingNode> Nodes { get; }
        public IBuildingGraphField Mutation { get; }
        public string ReturnFields { get; set; }
        public string MutationAlias { get; set; }
        public string DB_UUID
        {
            get; set;
        }
        public string Query { get; set; }
        public bool IsComplete { get; set; }
    }

    public class AbstractEntityType
    {
        string Name { get; }
    }

    public class ModelElementType
    {

    }

    public enum NodeMutationType
    {
        Update,
        Create,
        Delete
    }

}
