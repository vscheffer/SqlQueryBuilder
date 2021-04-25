using Newtonsoft.Json;
using Peerislands.SqlQueryBuilder.Core.Interfaces;
using Peerislands.SqlQueryBuilder.Core.Query.Models;
using System;
using Xunit;

namespace Peerislands.SqlQueryBuilder.Test
{
    public class TestQueryBuilder
    {
        private readonly ISqlQueryService _sqlQueryService;
        public TestQueryBuilder(ISqlQueryService sqlQueryService)
        {
            _sqlQueryService = sqlQueryService;
        }

        [Fact]
        public void SimpleQuey()
        {
            var queryData = "{\"table\":\"orders\",\"top\":\"0\",\"columns\":[],\"where\":[{\"operator\":\"Equals\",\"fieldName\":\"orders.store_id\",\"fieldValue\":\"1\"}],\"join\":[]}";

            var result = JsonConvert.DeserializeObject<SqlQuery>(queryData);
            _sqlQueryService.ProcessQuery(result);

            Assert.Equal("SELECT orders.* FROM orders  WHERE  (orders.store_id = '1')", result.SqlQueryOutput.Trim());
        }

        [Fact]
        public void LikeOperation()
        {
            var queryData = "{\"table\":\"customers\",\"top\":\"0\",\"columns\":[],\"where\":[{\"operator\":\"Like\",\"fieldName\":\"customers.first_name\",\"fieldValue\":\"Deb%\"}],\"join\":[]}";

            var result = JsonConvert.DeserializeObject<SqlQuery>(queryData);
            _sqlQueryService.ProcessQuery(result);

            Assert.Equal("SELECT customers.* FROM customers  WHERE  (customers.first_name LIKE 'Deb%')", result.SqlQueryOutput.Trim());
        }


        [Fact]
        public void BetweenOperation()
        {
            var queryData = "{\"table\":\"orders\",\"top\":\"0\",\"columns\":[],\"where\":[{\"operator\":\"GreaterOrEquals\",\"fieldName\":\"orders.order_date\",\"fieldValue\":\"2019-01-01\",\"andOrClause\":[{\"logicalOperator\":\"and\",\"fieldValue\":\"2020-01-01\",\"operator\":\"LessThan\"}]}],\"join\":[]}";

            var result = JsonConvert.DeserializeObject<SqlQuery>(queryData);
            _sqlQueryService.ProcessQuery(result);

            Assert.Equal("SELECT orders.* FROM orders  WHERE  (orders.order_date >= '2019-01-01' AND orders.order_date < '2020-01-01')", result.SqlQueryOutput.Trim());
        }


        [Fact]
        public void JoinOperation()
        {
            var queryData = "{\"table\":\"orders\",\"top\":\"0\",\"columns\":[{\"column\":\"order_id\"},{\"column\":\"order_status\"},{\"column\":\"customers.first_name\"},{\"column\":\"stores.store_name\"}],\"where\":[{\"operator\":\"Equals\",\"fieldName\":\"orders.store_id\",\"fieldValue\":\"1\",\"andOrClause\":[{\"logicalOperator\":\"or\",\"fieldValue\":\"2\",\"operator\":\"Equals\"}]}],\"join\":[{\"fieldFromjoin\":\"customer_id\",\"table\":\"customers\",\"fieldTojoin\":\"customer_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"},{\"fieldFromjoin\":\"store_id\",\"table\":\"stores\",\"fieldTojoin\":\"store_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"}]}";

            var result = JsonConvert.DeserializeObject<SqlQuery>(queryData);
            _sqlQueryService.ProcessQuery(result);

            Assert.Equal("SELECT order_id, order_status, customers.first_name, stores.store_name  FROM orders INNER JOIN customers ON orders.customer_id = customers.customer_id INNER JOIN stores ON orders.store_id = stores.store_id  WHERE  (orders.store_id = '1' OR orders.store_id = '2')", result.SqlQueryOutput.Trim());
        }

        [Fact]
        public void TopOperation()
        {
            var queryData = "{\"table\":\"orders\",\"top\":\"15\",\"columns\":[{\"column\":\"order_id\"},{\"column\":\"order_status\"},{\"column\":\"customers.first_name\"},{\"column\":\"stores.store_name\"}],\"where\":[{\"operator\":\"Equals\",\"fieldName\":\"orders.store_id\",\"fieldValue\":\"1\"}],\"join\":[{\"fieldFromjoin\":\"customer_id\",\"table\":\"customers\",\"fieldTojoin\":\"customer_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"},{\"fieldFromjoin\":\"store_id\",\"table\":\"stores\",\"fieldTojoin\":\"store_id\",\"operator\":\"Equals\",\"joinType\":\"InnerJoin\"}]}";

            var result = JsonConvert.DeserializeObject<SqlQuery>(queryData);
            _sqlQueryService.ProcessQuery(result);

            Assert.Equal("SELECT TOP 15 order_id, order_status, customers.first_name, stores.store_name  FROM orders INNER JOIN customers ON orders.customer_id = customers.customer_id INNER JOIN stores ON orders.store_id = stores.store_id  WHERE  (orders.store_id = '1')", result.SqlQueryOutput.Trim());
        }
    }
}
