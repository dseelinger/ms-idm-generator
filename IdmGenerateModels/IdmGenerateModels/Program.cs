﻿using System;
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

        public static void Main()
        {
            MainAsync().Wait();

            Console.WriteLine("Completeion: Schema Update Complete.  Hit enter to close.");
            Console.ReadLine();
        }

        public static async Task MainAsync()
        {
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
            var objTypeNames = from o in _objectTypes select o.GetAttrValue("Name");
            IdmCodeGenerator generator = new IdmCodeGenerator(objectTypeDescription, objTypeNames);
            var classString = generator.Generate();

            var classFile = string.Format(@"{0}{1}.cs", TargetDirectoryPath, objectTypeDescription.Name);
            File.WriteAllText(classFile, classString);
        }
    }
}
