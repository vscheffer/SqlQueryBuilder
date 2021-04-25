using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Query.Models
{
    public class SqlQuery
    {
        private string _id;

        public SqlQuery()
        {
            columns = new List<Columns>();
            where = new List<Wheres>();
            join = new List<Joins>();
        }

        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }


        public string table { get; set; }
        public int top { get; set; }
        public List<Columns> columns { get; set; }
        public List<Wheres> where { get; set; }
        public List<Joins> join { get; set; }
        public string SqlQueryOutput { get; set; }
    }
}
