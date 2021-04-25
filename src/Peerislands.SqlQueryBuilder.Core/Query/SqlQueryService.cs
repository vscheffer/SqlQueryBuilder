using Peerislands.SqlQueryBuilder.Core.Entities;
using Peerislands.SqlQueryBuilder.Core.Enums;
using Peerislands.SqlQueryBuilder.Core.Interfaces;
using Peerislands.SqlQueryBuilder.Core.Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Services
{
    public class SqlQueryService: ISqlQueryService
    {
        private readonly ISqlQueryRepository _sqlQueryRepository;
        public SqlQueryService(ISqlQueryRepository sqlQueryRepository)
        {
            _sqlQueryRepository = sqlQueryRepository;
        }

        public void Insert(SqlQuery sqlQuery)
        {
            _sqlQueryRepository.Insert(sqlQuery);
        }

        public void Update(SqlQuery sqlQuery)
        {
            _sqlQueryRepository.Update(sqlQuery);
        }
        public void Delete(SqlQuery sqlQuery)
        {
            _sqlQueryRepository.Delete(sqlQuery);
        }
        public SqlQuery Get(string id)
        {
            return _sqlQueryRepository.Get(id);
        }
        public List<SqlQuery> GetAll()
        {
            return _sqlQueryRepository.GetAll();
        }

        public void ProcessQuery(SqlQuery parameter)
        {

            var query = new SelectQueryBuilder()
               .SelectFromTable(parameter.table)
               .AddTop(parameter.top)
               .SelectColumns(parameter.columns.Select(a => a.column).ToArray());


            foreach (var join in parameter.join)
            {
                query.AddJoin(JoinType.InnerJoin, join.table, join.fieldTojoin, QueryComparison.Equals, parameter.table, join.fieldFromjoin);
            }

            foreach (var where in parameter.where)
            {
                var clause = query.AddWhere(where.fieldName, where.@operator, where.fieldValue);
                foreach (var andOr in where?.andOrClause)
                {
                    clause.AddClause(andOr.logicalOperator, andOr.@operator, andOr.fieldValue);
                }
            }

            parameter.SqlQueryOutput = query.BuildQuery();
        }

        public void ProcessAllQueries()
        {
            var allImputs = GetAll();

            foreach (var query in allImputs)
            {
                ProcessQuery(query);
                Update(query);
            }
        }
    }
}
