using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using IdmNet;
using IdmNet.SoapModels;
using Xunit;

namespace IdmGenerateModels.Tests.Spikes
{
    public class ModelSpikes
    {
        //[Fact]
        public async Task TestSingleObjectType()
        {
            var client = IdmNetClientFactory.BuildClient();

            var objectType = await client.GetSchemaAsync("Group");
            var attr = objectType.BindingDescriptions[23];

        }

        //[Fact]
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

        //[Fact]
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
