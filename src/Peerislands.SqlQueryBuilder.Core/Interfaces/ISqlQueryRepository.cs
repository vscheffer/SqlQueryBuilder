using Peerislands.SqlQueryBuilder.Core.Query.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Interfaces
{
    public interface ISqlQueryRepository
    {
        void Insert(SqlQuery sqlQuery);
        void Update(SqlQuery sqlQuery);
        void Delete(SqlQuery sqlQuery);
        SqlQuery Get(string id);
        List<SqlQuery> GetAll();
    }

}
