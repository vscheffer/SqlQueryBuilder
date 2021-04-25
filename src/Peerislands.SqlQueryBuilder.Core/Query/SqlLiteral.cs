using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Query
{
    public class SqlLiteral
    {
        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public SqlLiteral(string value)
        {
            _value = value;
        }
    }
}
