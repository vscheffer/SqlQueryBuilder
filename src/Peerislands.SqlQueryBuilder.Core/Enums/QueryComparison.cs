using System;
using System.Collections.Generic;
using System.Text;

namespace Peerislands.SqlQueryBuilder.Core.Enums
{
    public enum QueryComparison
    {
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        In
    }
}
