using Peerislands.SqlQueryBuilder.Core.Enums;
using Peerislands.SqlQueryBuilder.Core.Query;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Entities.Clauses
{
    public class WhereStatement : List<List<WhereClause>>
    {
        public int ClauseLevels
        {
            get { return this.Count; }
        }

        private void AssertLevelExistance(int level)
        {
            if (this.Count < (level - 1))
            {
                throw new Exception("Level " + level + " not allowed because level " + (level - 1) + " does not exist.");
            }

            else if (this.Count < level)
            {
                this.Add(new List<WhereClause>());
            }
        }

        public void Add(WhereClause clause) { this.Add(clause, 1); }
        public void Add(WhereClause clause, int level)
        {
            this.AddWhereClauseToLevel(clause, level);
        }
        public WhereClause Add(string field, QueryComparison @operator, object compareValue) { return this.Add(field, @operator, compareValue, 1); }
        public WhereClause Add(Enum field, QueryComparison @operator, object compareValue) { return this.Add(field.ToString(), @operator, compareValue, 1); }
        public WhereClause Add(string field, QueryComparison @operator, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
            this.AddWhereClauseToLevel(NewWhereClause, level);
            return NewWhereClause;
        }

        private void AddWhereClause(WhereClause clause)
        {
            AddWhereClauseToLevel(clause, 1);
        }

        private void AddWhereClauseToLevel(WhereClause clause, int level)
        {
            AssertLevelExistance(level);
            this[level - 1].Add(clause);
        }

        public string BuildWhereStatement()
        {
            string Result = "";
            foreach (List<WhereClause> WhereStatement in this)
            {
                string LevelWhere = "";
                foreach (WhereClause Clause in WhereStatement)
                {
                    string WhereClause = "";

                    WhereClause = CreateQueryComparisonClause(Clause.FieldName, Clause.ComparisonOperator, Clause.Value);


                    foreach (WhereClause.SubClause SubWhereClause in Clause.SubClauses)
                    {
                        switch (SubWhereClause.LogicOperator)
                        {
                            case LogicOperator.And:
                                WhereClause += " AND "; break;
                            case LogicOperator.Or:
                                WhereClause += " OR "; break;
                        }
                        WhereClause += CreateQueryComparisonClause(Clause.FieldName, SubWhereClause.ComparisonOperator, SubWhereClause.Value);

                    }
                    LevelWhere += "(" + WhereClause + ") AND ";
                }
                LevelWhere = LevelWhere.Substring(0, LevelWhere.Length - 5);
                if (WhereStatement.Count > 1)
                {
                    Result += " (" + LevelWhere + ") ";
                }
                else
                {
                    Result += " " + LevelWhere + " ";
                }
                Result += " OR";
            }
            Result = Result.Substring(0, Result.Length - 2);

            return Result;
        }

        internal static string CreateQueryComparisonClause(string fieldName, QueryComparison QueryComparisonOperator, object value)
        {
            string Output = "";
            if (value != null && value != System.DBNull.Value)
            {
                switch (QueryComparisonOperator)
                {
                    case QueryComparison.Equals:
                        Output = fieldName + " = " + FormatSQLValue(value); break;
                    case QueryComparison.NotEquals:
                        Output = fieldName + " <> " + FormatSQLValue(value); break;
                    case QueryComparison.GreaterThan:
                        Output = fieldName + " > " + FormatSQLValue(value); break;
                    case QueryComparison.GreaterOrEquals:
                        Output = fieldName + " >= " + FormatSQLValue(value); break;
                    case QueryComparison.LessThan:
                        Output = fieldName + " < " + FormatSQLValue(value); break;
                    case QueryComparison.LessOrEquals:
                        Output = fieldName + " <= " + FormatSQLValue(value); break;
                    case QueryComparison.Like:
                        Output = fieldName + " LIKE " + FormatSQLValue(value); break;
                    case QueryComparison.NotLike:
                        Output = "NOT " + fieldName + " LIKE " + FormatSQLValue(value); break;
                    case QueryComparison.In:
                        Output = fieldName + " IN (" + FormatSQLValue(value) + ")"; break;
                }
            }
            else
            {
                if ((QueryComparisonOperator != QueryComparison.Equals) && (QueryComparisonOperator != QueryComparison.NotEquals))
                {
                    throw new Exception("Cannot use QueryComparison operator " + QueryComparisonOperator.ToString() + " for NULL values.");
                }
                else
                {
                    switch (QueryComparisonOperator)
                    {
                        case QueryComparison.Equals:
                            Output = fieldName + " IS NULL"; break;
                        case QueryComparison.NotEquals:
                            Output = "NOT " + fieldName + " IS NULL"; break;
                    }
                }
            }
            return Output;
        }

        internal static string FormatSQLValue(object someValue)
        {
            string FormattedValue = "";
            if (someValue == null)
            {
                FormattedValue = "NULL";
            }
            else
            {
                switch (someValue.GetType().Name)
                {
                    case "String": FormattedValue = "'" + ((string)someValue).Replace("'", "''") + "'"; break;
                    case "DateTime": FormattedValue = "'" + ((DateTime)someValue).ToString("yyyy/MM/dd hh:mm:ss") + "'"; break;
                    case "DBNull": FormattedValue = "NULL"; break;
                    case "Boolean": FormattedValue = (bool)someValue ? "1" : "0"; break;
                    case "SqlLiteral": FormattedValue = ((SqlLiteral)someValue).Value; break;
                    default: FormattedValue = someValue.ToString(); break;
                }
            }
            return FormattedValue;
        }

    }
}
