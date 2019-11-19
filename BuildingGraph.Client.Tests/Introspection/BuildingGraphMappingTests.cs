using Microsoft.VisualStudio.TestTools.UnitTesting;
using HLApps.Cloud.BuildingGraph.Introspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HLApps.Cloud.BuildingGraph.Introspection.Tests
{
    [TestClass()]
    public class BuildingGraphMappingTests
    {
        [TestMethod()]
        public void BuildingGraphMappingTest()
        {
            var json = System.IO.File.ReadAllText(@"C:\src\HLApps\GraphData-MEP\HLApps.Revit.Graph\RevitToGraphQLMappings.json");
            
            //var bgmap = new BuildingGraphMapping(json);
          


        }
    }
}