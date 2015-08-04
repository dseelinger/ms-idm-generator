using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static IEnumerable<IdmResource> _objectTypes;
        private static string _json;

        public static void Main()
        {
            MainAsync().Wait();

            Console.WriteLine("Completeion: Code generation.  Hit enter to close.");
            Console.ReadLine();
        }

        public static async Task MainAsync()
        {
            _json = File.ReadAllText("AttributeToObjectMapping.json");
            Client = IdmNetClientFactory.BuildClient();

            _objectTypes = await GetObjectTypes();

            await GenerateCode();
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


        public static async Task<int> GenerateCode()
        {
            ClearOutputDirectory();

            foreach (var idmResource in _objectTypes)
            {
                var objectType = await GetSchema(idmResource);

                Console.WriteLine("Generating model and tests for [{0}]", objectType.Name);
                GenerateModelAndTests(objectType);
            }
            return 1;
        }

        public static async Task<Schema> GetSchema(IdmResource idmResource)
        {
            var objTypeName = idmResource.GetAttrValue("Name");
            Console.WriteLine("Getting Schema for ObjectTypeDescription: [{0}]", objTypeName);
            var objectType = await Client.GetSchemaAsync(objTypeName);
            return objectType;
        }


        public static void ClearOutputDirectory()
        {
            Console.WriteLine("Cleaning output directory.");
            TargetDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\output\";
            if (Directory.Exists(TargetDirectoryPath))
            {
                Directory.Delete(TargetDirectoryPath, true);
            }
            Directory.CreateDirectory(TargetDirectoryPath);
            Directory.CreateDirectory(TargetDirectoryPath + @"tests\");
        }


        public static void GenerateModelAndTests(Schema objectTypeDescription)
        {
            var objTypeNames = (from o in _objectTypes select o.GetAttrValue("Name")).ToList();
            IdmCodeGenerator generator = new IdmCodeGenerator(objectTypeDescription, objTypeNames, _json);

            Tuple<string, string> classAndTests = generator.Generate();
            var classFile = $@"{TargetDirectoryPath}{objectTypeDescription.Name}.cs";
            File.WriteAllText(classFile, classAndTests.Item1);

            var testsFile = $@"{TargetDirectoryPath}Tests\{objectTypeDescription.Name}Tests.cs";
            File.WriteAllText(testsFile, classAndTests.Item2);
        }
    }
}
