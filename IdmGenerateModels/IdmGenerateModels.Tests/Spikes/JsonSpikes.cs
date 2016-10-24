using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace IdmGenerateModels.Tests.Spikes
{

    public class JsonSpikes
    {
        [Fact]
        public void This_is_what_a_serialized_List_looks_like_in_JSON()
        {
            var list = new List<ReferenceToObjectTypeMap>
            {
                new ReferenceToObjectTypeMap { AttrName = "foo", ObjType = "bar" }
            };

            string json = JsonConvert.SerializeObject(list);
        }
    }
}
