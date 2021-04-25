using Peerislands.SqlQueryBuilder.Core.Entities.Clauses;
using Peerislands.SqlQueryBuilder.Core.Enums;
using Peerislands.SqlQueryBuilder.Core.Query;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Entities
{
    public class SelectQueryBuilder
    {
        protected bool _distinct = false;
        protected TopClause _topClause = new TopClause(0);
        protected List<string> _selectedColumns = new List<string>();
        protected string _selectedTable = string.Empty;
        protected List<JoinClause> _joins = new List<JoinClause>();
        protected WhereStatement _whereStatement = new WhereStatement();
        protected List<OrderByClause> _orderByStatement = new List<OrderByClause>();
        protected List<string> _groupByColumns = new List<string>();
        protected WhereStatement _havingStatement = new WhereStatement();

        internal WhereStatement WhereStatement
        {
            get { return _whereStatement; }
            set { _whereStatement = value; }
        }

        public SelectQueryBuilder() { }
        public SelectQueryBuilder(DbProviderFactory factory)
        {
            _dbProviderFactory = factory;
        }

        private DbProviderFactory _dbProviderFactory;
        public void SetDbProviderFactory(DbProviderFactory factory)
        {
            _dbProviderFactory = factory;
        }

        public bool Distinct
        {
            get { return _distinct; }
            set { _distinct = value; }
        }

        public int TopRecords
        {
            get { return _topClause.Quantity; }
            set
            {
                _topClause.Quantity = value;
            }
        }
        public TopClause TopClause
        {
            get { return _topClause; }
            set { _topClause = value; }
        }

        public string[] SelectedColumns
        {
            get
            {
                if (_selectedColumns.Count > 0)
                    return _selectedColumns.ToArray();
                else
                    return new string[1] { "*" };
            }
        }

        public SelectQueryBuilder SelectAllColumns()
        {
            _selectedColumns.Clear();
            return this;
        }
        public void SelectCount()
        {
            SelectColumn("count(1)");
        }
        public void SelectColumn(string column)
        {
            _selectedColumns.Clear();
            _selectedColumns.Add(column);
        }
        public SelectQueryBuilder SelectColumns(params string[] columns)
        {
            _selectedColumns.Clear();
            foreach (string column in columns)
            {
                _selectedColumns.Add(column);
            }
            return this;
        }
        public SelectQueryBuilder SelectFromTable(string table)
        {
            _selectedTable = table;
            return this;
        }

        public SelectQueryBuilder AddTop(int top)
        {
            TopClause = new TopClause(top);
            return this;
        }

        public SelectQueryBuilder AddJoin(JoinClause newJoin)
        {
            _joins.Add(newJoin);
            return this;
        }
        public SelectQueryBuilder AddJoin(JoinType join, string toTableName, string toColumnName, QueryComparison @operator, string fromTableName, string fromColumnName)
        {
            JoinClause NewJoin = new JoinClause(join, toTableName, toColumnName, @operator, fromTableName, fromColumnName);
            _joins.Add(NewJoin);
            return this;
        }

        public WhereStatement Where
        {
            get { return _whereStatement; }
            set { _whereStatement = value; }
        }

        public SelectQueryBuilder AddWhere(WhereClause clause)
        {
            AddWhere(clause, 1);
            return this;
        }
        public SelectQueryBuilder AddWhere(WhereClause clause, int level)
        {
            _whereStatement.Add(clause, level);
            return this;
        }
        public WhereClause AddWhere(string field, QueryComparison @operator, object compareValue) { return AddWhere(field, @operator, compareValue, 1); }
        public WhereClause AddWhere(Enum field, QueryComparison @operator, object compareValue) { return AddWhere(field.ToString(), @operator, compareValue, 1); }
        public WhereClause AddWhere(string field, QueryComparison @operator, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
            _whereStatement.Add(NewWhereClause, level);
            return NewWhereClause;
        }

        public SelectQueryBuilder AddOrderBy(OrderByClause clause)
        {
            _orderByStatement.Add(clause);
            return this;
        }
        public SelectQueryBuilder AddOrderBy(Enum field, Sorting order)
        {
            this.AddOrderBy(field.ToString(), order);
            return this;
        }
        public SelectQueryBuilder AddOrderBy(string field, Sorting order)
        {
            OrderByClause NewOrderByClause = new OrderByClause(field, order);
            _orderByStatement.Add(NewOrderByClause);
            return this;
        }

        public SelectQueryBuilder GroupBy(params string[] columns)
        {
            foreach (string Column in columns)
            {
                _groupByColumns.Add(Column);
            }
            return this;
        }

        public WhereStatement Having
        {
            get { return _havingStatement; }
            set { _havingStatement = value; }
        }

        public void AddHaving(WhereClause clause) { AddHaving(clause, 1); }
        public void AddHaving(WhereClause clause, int level)
        {
            _havingStatement.Add(clause, level);
        }
        public WhereClause AddHaving(string field, QueryComparison @operator, object compareValue) { return AddHaving(field, @operator, compareValue, 1); }
        public WhereClause AddHaving(Enum field, QueryComparison @operator, object compareValue) { return AddHaving(field.ToString(), @operator, compareValue, 1); }
        public WhereClause AddHaving(string field, QueryComparison @operator, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
            _havingStatement.Add(NewWhereClause, level);
            return NewWhereClause;
        }

        public string BuildQuery()
        {

            StringBuilder query = new StringBuilder();

            query.Append("SELECT ");

            if (_distinct)
            {
                query.Append("DISTINCT ");
            }

            if (_topClause.Quantity > 0)
            {
                query.Append($"TOP {_topClause.Quantity} ");
            }

            if (_selectedColumns.Count == 0)
            {
                query.Append($"{_selectedTable}.*");
            }
            else
            {
                foreach (string ColumnName in _selectedColumns)
                {
                    query.Append($"{ColumnName}, ");
                }
                query.Length--;
                query.Length--;
                query.Append(" ");
            }

            query.Append($" FROM {_selectedTable} ");

            if (_joins.Count > 0)
            {
                foreach (JoinClause Clause in _joins)
                {
                    string JoinString = "";
                    switch (Clause.JoinType)
                    {
                        case JoinType.InnerJoin: JoinString = "INNER JOIN"; break;
                        case JoinType.OuterJoin: JoinString = "OUTER JOIN"; break;
                        case JoinType.LeftJoin: JoinString = "LEFT JOIN"; break;
                        case JoinType.RightJoin: JoinString = "RIGHT JOIN"; break;
                    }
                    JoinString += " " + Clause.ToTable + " ON ";
                    JoinString += WhereStatement.CreateQueryComparisonClause(Clause.FromTable + '.' + Clause.FromColumn, Clause.ComparisonOperator, new SqlLiteral(Clause.ToTable + '.' + Clause.ToColumn));
                    query.Append($"{JoinString} ");
                }
            }

            if (_whereStatement.ClauseLevels > 0)
            {
                query.Append($" WHERE { _whereStatement.BuildWhereStatement()} ");
            }

            if (_groupByColumns.Count > 0)
            {
                query.Append(" GROUP BY ");
                foreach (string Column in _groupByColumns)
                {
                    query.Append($"{Column} ,");
                }
                query.Length--;
                query.Length--;
                query.Append(" ");
            }

            if (_havingStatement.ClauseLevels > 0)
            {
                if (_groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }
                query.Append($" HAVING {_havingStatement.BuildWhereStatement()} ");
            }

            if (_orderByStatement.Count > 0)
            {
                query.Append(" ORDER BY ");
                foreach (OrderByClause Clause in _orderByStatement)
                {
                    string OrderByClause = "";
                    switch (Clause.SortOrder)
                    {
                        case Sorting.Ascending:
                            OrderByClause = Clause.FieldName + " ASC"; break;
                        case Sorting.Descending:
                            OrderByClause = Clause.FieldName + " DESC"; break;
                    }
                    query.Append(OrderByClause + ',');
                }
                query.Length--;
                query.Length--;
                query.Append(" ");
            }


            // Return the built query
            return query.ToString();
        }
    }

}
