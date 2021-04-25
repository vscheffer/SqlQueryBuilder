using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Peerislands.SqlQueryBuilder.Core.Entities;
using Peerislands.SqlQueryBuilder.Core.Interfaces;
using MongoDB.Bson.Serialization.Conventions;
using Peerislands.SqlQueryBuilder.Core.Query.Models;
using MongoDB.Bson;

namespace Peerislands.SqlQueryBuilder.Data.Repositories
{

    public class SqlQueryRepository : ISqlQueryRepository
    {

        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<SqlQuery> _sqlQueryCollection;

        public SqlQueryRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _sqlQueryCollection = _mongoClient.GetDatabase("SqlQueryBuilder").GetCollection<SqlQuery>("SqlQuery");
        }

        public void Delete(SqlQuery sqlQuery)
        {
            _sqlQueryCollection.DeleteOne(Builders<SqlQuery>.Filter.Eq("Id", sqlQuery.Id));
        }

        public SqlQuery Get(string id)
        {
            return _sqlQueryCollection.Find(Builders<SqlQuery>.Filter.Eq("Id", id)).FirstOrDefault();
        }

        public List<SqlQuery> GetAll()
        {
            return _sqlQueryCollection.Find(Builders<SqlQuery>.Filter.Empty).ToList();
        }

        public void Insert(SqlQuery sqlQuery)
        {
            _sqlQueryCollection.InsertOne(sqlQuery);
        }

        public void Update(SqlQuery sqlQuery)
        {
            _sqlQueryCollection.ReplaceOne(Builders<SqlQuery>.Filter.Eq("Id", sqlQuery.Id), sqlQuery);
        }
    }
}
