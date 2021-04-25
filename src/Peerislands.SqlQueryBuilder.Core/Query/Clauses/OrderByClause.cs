using Peerislands.SqlQueryBuilder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Entities.Clauses
{
    public struct OrderByClause
    {
        public string FieldName;
        public Sorting SortOrder;
        public OrderByClause(string field)
        {
            FieldName = field;
            SortOrder = Sorting.Ascending;
        }
        public OrderByClause(string field, Sorting order)
        {
            FieldName = field;
            SortOrder = order;
        }
    }
}
