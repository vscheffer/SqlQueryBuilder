using Peerislands.SqlQueryBuilder.Core.Query.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Interfaces
{
    public interface ISqlQueryService
    {
        void Insert(SqlQuery sqlQuery);
        void Update(SqlQuery sqlQuery);
        void Delete(SqlQuery sqlQuery);
        SqlQuery Get(string id);
        void ProcessQuery(SqlQuery parameter);
        void ProcessAllQueries();
        List<SqlQuery> GetAll();
    }
}
