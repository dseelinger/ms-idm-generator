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
        public static IdmNetClient Client;
        public static string TargetDirectoryPath;

        public static void Main()
        {
            MainAsync().Wait();

            Console.WriteLine("Completeion: Schema Update Complete.  Hit enter to close.");
            Console.ReadLine();
        }

        public static async Task MainAsync()
        {
            Client = IdmNetClientFactory.BuildClient();

            var objectTypeResource = await GetObjectTypes();

            await GenerateCode(objectTypeResource);
        }

        
        public static async Task<IEnumerable<IdmResource>> GetObjectTypes()
        {
            Console.WriteLine("Querying Object Types.");
            var objectTypeResource =
                await
                    Client.SearchAsync(new SearchCriteria("/ObjectTypeDescription")
                    {
                        Selection = new List<string> { "Name" }
                    });
            return objectTypeResource;
        }


        public static async Task<int> GenerateCode(IEnumerable<IdmResource> objectTypeResource)
        {
            ClearOutputDirectory();

            foreach (var idmResource in objectTypeResource)
            {
                var objectType = await GetSchema(idmResource);

                Console.WriteLine("Generating model and tests for [{0}]", objectType.Name);
                GenerateModelAndTests(objectType);
            }
            return 1;
        }

        public static async Task<ObjectTypeDescription> GetSchema(IdmResource idmResource)
        {
            var objTypeName = idmResource.GetAttrValue("Name");
            Console.WriteLine("Getting Schema for ObjectTypeDescription: [{0}]", objTypeName);
            var objectType = await Client.GetSchemaAsync(objTypeName);
            return objectType;
        }


        public static void ClearOutputDirectory()
        {
            TargetDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\output\";
            if (Directory.Exists(TargetDirectoryPath))
            {
                Directory.Delete(TargetDirectoryPath, true);
            }
            Directory.CreateDirectory(TargetDirectoryPath);
        }


        public static void GenerateModelAndTests(ObjectTypeDescription objectTypeDescription)
        {
            IdmCodeGenerator generator = new IdmCodeGenerator(objectTypeDescription);
            var classString = generator.Generate();

            var classFile = string.Format(@"{0}{1}.cs", TargetDirectoryPath, objectTypeDescription.Name);
            File.WriteAllText(classFile, classString);
        }
    }
}
