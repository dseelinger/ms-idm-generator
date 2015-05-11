using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using IdmNet;
using IdmNet.Models;
using IdmNet.SoapModels;

namespace IdmGenerateModels
{
    class Program
    {
        private static IdmNetClient _client;

        static void Main()
        {
            MainAsync().Wait();

            Console.WriteLine("Completeion: Schema Update Complete.  Hit enter to close.");
            Console.ReadLine();
        }

        private static async Task MainAsync()
        {
            _client = IdmNetClientFactory.BuildClient();

            var objectTypeResource = await GetObjectTypes();

            var allSchemaObjects = await GetSchema(objectTypeResource);

            GenerateCode(allSchemaObjects);
        }

        
        private static async Task<IEnumerable<IdmResource>> GetObjectTypes()
        {
            Console.WriteLine("Querying Object Types.");
            var objectTypeResource =
                await
                    _client.SearchAsync(new SearchCriteria("/ObjectTypeDescription[Name='BindingDescription']")
                    {
                        Selection = new List<string> { "Name" }
                    });
            return objectTypeResource;
        }


        private static async Task<List<ObjectTypeDescription>> GetSchema(IEnumerable<IdmResource> objectTypeResource)
        {
            var allSchemaObjects = new List<ObjectTypeDescription>();
            foreach (var idmResource in objectTypeResource)
            {
                var objTypeName = idmResource.GetAttrValue("Name");
                Console.WriteLine("Getting Schema for ObjectTypeDescription: [{0}]", objTypeName);
                var objectType = await _client.GetSchemaAsync(objTypeName);
                allSchemaObjects.Add(objectType);
            }
            return allSchemaObjects;
        }


        private static void GenerateCode(List<ObjectTypeDescription> allSchemaObjects)
        {
            ClearOutputDirectory();

            foreach (var objectTypeDescription in allSchemaObjects)
            {
                Console.WriteLine("Generating model and tests for [{0}]", objectTypeDescription.Name);
                GenerateModelAndTests(objectTypeDescription);
            }
        }

        private static void ClearOutputDirectory()
        {
            var targetDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\output\";
            if (Directory.Exists(targetDirectoryPath))
            {
                Directory.Delete(targetDirectoryPath, true);
            }
            Directory.CreateDirectory(targetDirectoryPath);
        }


        private static void GenerateModelAndTests(ObjectTypeDescription objectTypeDescription)
        {
            IdmCodeGenerator generator = new IdmCodeGenerator(objectTypeDescription);
            generator.Generate();
        }
    }
}
