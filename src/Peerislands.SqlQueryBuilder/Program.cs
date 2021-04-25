using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using Peerislands.SqlQueryBuilder.Core.Entities;
using Peerislands.SqlQueryBuilder.Core.Entities.Clauses;
using Peerislands.SqlQueryBuilder.Core.Enums;
using Peerislands.SqlQueryBuilder.Core.Interfaces;
using Peerislands.SqlQueryBuilder.Core.Query.Models;
using Peerislands.SqlQueryBuilder.Core.Services;
using Peerislands.SqlQueryBuilder.Data.Repositories;


namespace Peerislands.SqlQueryBuilder
{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceCollection = ConfigureServices();


            var menu = new ConsoleMenu(args, level: 0)
            .Add("Load Json file", () => LoadJsonFile(serviceCollection))
            .Add("Seed sample Json", () => LoadJsonSeed(serviceCollection))
            .Add("Process all database content", () => ProcessQueries(serviceCollection))
            .Add("Delete all database content", () => DeleteData(serviceCollection))
            .Add("Export all Json files stored in the database", () => ExportData(serviceCollection))
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Selector = "--> ";
                config.EnableFilter = true;
                config.Title = "Peerislands SqlQueryBuilder";
                config.EnableWriteTitle = true;
                config.EnableBreadcrumb = false;
            });

            menu.Show();

        }

        private static void ProcessQueries(ServiceProvider serviceCollection)
        {
            var sqlQueryservice = serviceCollection.GetService<ISqlQueryService>();
            sqlQueryservice.ProcessAllQueries();
        }

        private static void ExportData(ServiceProvider serviceCollection)
        {
            var sqlQueryservice = serviceCollection.GetService<ISqlQueryService>();
            Console.WriteLine("Save path:");
            var jsonPath = Console.ReadLine();

            var data = sqlQueryservice.GetAll();

            foreach (var item in data)
            {
                var result = JsonConvert.SerializeObject(item);
                File.WriteAllText($"{jsonPath}\\{item.Id}.json", result);
            }

        }

        private static void LoadJsonFile(ServiceProvider serviceCollection)
        {
            var sqlQueryservice = serviceCollection.GetService<ISqlQueryService>();

            try
            {
                Console.WriteLine("Json path:");
                var jsonPath = Console.ReadLine();

                if (!jsonPath.ToLower().EndsWith(".json"))
                    throw new ArgumentException("there must be selected a .json file!");

                using (var sr = new StreamReader(jsonPath))
                {
                    var result = JsonConvert.DeserializeObject<SqlQuery>(sr.ReadToEnd());
                    sqlQueryservice.Insert(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void DeleteData(ServiceProvider serviceCollection)
        {
            var sqlQueryservice = serviceCollection.GetService<ISqlQueryService>();

            var data = sqlQueryservice.GetAll();
            data.ForEach(sqlQueryservice.Delete);
        }

        private static void LoadJsonSeed(ServiceProvider serviceCollection)
        {
            var sqlQueryservice = serviceCollection.GetService<ISqlQueryService>();

            var seedList = new List<string>()
            {
                "{\"table\":\"orders\",\"top\":\"0\",\"columns\":[],\"where\":[{\"operator\":\"Equals\",\"fieldName\":\"orders.store_id\",\"fieldValue\":\"1\"}],\"join\":[]}",
                "{\"table\":\"customers\",\"top\":\"0\",\"columns\":[],\"where\":[{\"operator\":\"Like\",\"fieldName\":\"customers.first_name\",\"fieldValue\":\"Deb\"}],\"join\":[]}",
                "{\"table\":\"orders\",\"top\":\"0\",\"columns\":[],\"where\":[{\"operator\":\"GreaterOrEquals\",\"fieldName\":\"orders.order_date\",\"fieldValue\":\"2019-01-01\",\"andOrClause\":[{\"logicalOperator\":\"and\",\"fieldValue\":\"2020-01-01\",\"operator\":\"LessThan\"}]}],\"join\":[]}",
                "{\"table\":\"orders\",\"top\":\"0\",\"columns\":[{\"column\":\"order_id\"},{\"column\":\"order_status\"},{\"column\":\"customers.first_name\"},{\"column\":\"stores.store_name\"}],\"where\":[{\"operator\":\"Equals\",\"fieldName\":\"orders.store_id\",\"fieldValue\":\"1\",\"andOrClause\":[{\"logicalOperator\":\"or\",\"fieldValue\":\"2\",\"operator\":\"Equals\"}]}],\"join\":[{\"fieldFromjoin\":\"customer_id\",\"table\":\"customers\",\"fieldTojoin\":\"customer_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"},{\"fieldFromjoin\":\"store_id\",\"table\":\"stores\",\"fieldTojoin\":\"store_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"}]}",
                "{\"table\":\"orders\",\"top\":\"15\",\"columns\":[{\"column\":\"order_id\"},{\"column\":\"order_status\"},{\"column\":\"customers.first_name\"},{\"column\":\"stores.store_name\"}],\"where\":[{\"operator\":\"Equals\",\"fieldName\":\"orders.store_id\",\"fieldValue\":\"1\"}],\"join\":[{\"fieldFromjoin\":\"customer_id\",\"table\":\"customers\",\"fieldTojoin\":\"customer_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"},{\"fieldFromjoin\":\"store_id\",\"table\":\"stores\",\"fieldTojoin\":\"store_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"}]}",
            };

            try
            {
                foreach (var item in seedList)
                {

                    var result = JsonConvert.DeserializeObject<SqlQuery>(item);
                    sqlQueryservice.Insert(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static ServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceProvider = new ServiceCollection()
              .AddLogging()
              .AddSingleton<IConfiguration>(configuration)
              .AddSingleton<ISqlQueryService, SqlQueryService>()
              .AddSingleton<ISqlQueryRepository, SqlQueryRepository>()
              .AddSingleton<IMongoClient, MongoClient>(s =>
              {
                  var uri = s.GetRequiredService<IConfiguration>()["MongoUri"];
                  return new MongoClient(uri);
              })
              .BuildServiceProvider();

            return serviceProvider;
        }

    }
}
