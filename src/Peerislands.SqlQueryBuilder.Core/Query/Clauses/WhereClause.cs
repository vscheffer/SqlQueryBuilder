using Peerislands.SqlQueryBuilder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Entities.Clauses
{
    public struct WhereClause
    {
        private string m_FieldName;
        private QueryComparison m_ComparisonOperator;
        private object m_Value;
        internal struct SubClause
        {
            public LogicOperator LogicOperator;
            public QueryComparison ComparisonOperator;
            public object Value;
            public SubClause(LogicOperator logic, QueryComparison compareOperator, object compareValue)
            {
                LogicOperator = logic;
                ComparisonOperator = compareOperator;
                Value = compareValue;
            }
        }
        internal List<SubClause> SubClauses;

        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }

        public QueryComparison ComparisonOperator
        {
            get { return m_ComparisonOperator; }
            set { m_ComparisonOperator = value; }
        }

        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public WhereClause(string field, QueryComparison firstCompareOperator, object firstCompareValue)
        {
            m_FieldName = field;
            m_ComparisonOperator = firstCompareOperator;
            m_Value = firstCompareValue;
            SubClauses = new List<SubClause>();
        }
        public void AddClause(LogicOperator logic, QueryComparison compareOperator, object compareValue)
        {
            SubClause NewSubClause = new SubClause(logic, compareOperator, compareValue);
            SubClauses.Add(NewSubClause);
        }
    }
}
