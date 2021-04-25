using Peerislands.SqlQueryBuilder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Query.Models
{
    public class AndOrClause
    {
        public LogicOperator logicalOperator { get; set; }
        public QueryComparison @operator { get; set; }
        public string fieldValue { get; set; }
    }
}
