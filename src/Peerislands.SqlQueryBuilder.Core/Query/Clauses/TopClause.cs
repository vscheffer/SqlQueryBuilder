using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Entities.Clauses
{
    public struct TopClause
    {
        public int Quantity;
        public TopClause(int nr)
        {
            Quantity = nr;
        }
    }
}
