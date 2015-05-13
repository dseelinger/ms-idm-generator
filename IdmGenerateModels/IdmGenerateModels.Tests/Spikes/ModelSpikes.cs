using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using IdmNet;
using IdmNet.SoapModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdmGenerateModels.Tests.Spikes
{
    [TestClass]
    [Ignore]
    public class ModelSpikes
    {
        [TestMethod]
        [TestCategory("Integration")]
        public async Task AllObjectTypeNames()
        {
            var client = IdmNetClientFactory.BuildClient();

            var objectTypesAsResources =
                await
                    client.SearchAsync(new SearchCriteria("/ObjectTypeDescription")
                    {
                        Selection = new List<string> { "Name" }
                    });
            foreach (var resource in objectTypesAsResources)
            {
                Debug.WriteLine(resource.GetAttrValue("Name"));
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task AllReferenceAttributes()
        {
            var client = IdmNetClientFactory.BuildClient();

            var resources =
                await
                    client.SearchAsync(new SearchCriteria("/AttributeTypeDescription[DataType='Reference']")
                    {
                        Selection = new List<string> { "Name" }
                    });
            foreach (var resource in resources)
            {
                Debug.WriteLine(resource.GetAttrValue("Name"));
            }
        }
    }
}
