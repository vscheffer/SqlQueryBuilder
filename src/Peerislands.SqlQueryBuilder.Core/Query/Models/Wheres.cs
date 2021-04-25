using Peerislands.SqlQueryBuilder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Query.Models
{
    public class Wheres
    {
        public Wheres()
        {
            andOrClause = new List<AndOrClause>();
        }

        public QueryComparison @operator { get; set; }
        public string fieldName { get; set; }
        public string fieldValue { get; set; }

        public List<AndOrClause> andOrClause { get; set; }
    }
}
