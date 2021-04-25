using Peerislands.SqlQueryBuilder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Query.Models
{
    public class Joins
    {
        public string fieldFromjoin { get; set; }
        public string table { get; set; }
        public string fieldTojoin { get; set; }
        public QueryComparison @operator { get; set; }
        public JoinType joinType { get; set; }
    }
}
