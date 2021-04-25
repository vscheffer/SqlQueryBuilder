using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Peerislands.SqlQueryBuilder.Core.Interfaces;
using Peerislands.SqlQueryBuilder.Core.Services;
using Peerislands.SqlQueryBuilder.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Test
{
 public   class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {


            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddLogging()
              .AddSingleton<IConfiguration>(configuration)
              .AddSingleton<ISqlQueryService, SqlQueryService>()
              .AddSingleton<ISqlQueryRepository, SqlQueryRepository>()
              .AddSingleton<IMongoClient, MongoClient>(s =>
              {
                  var uri = s.GetRequiredService<IConfiguration>()["MongoUri"];
                  return new MongoClient(uri);
              })
              .BuildServiceProvider();

        }
    }
}
