using System.Collections.Generic;
using System.Net;
using QliroShopper.Controllers;
using QliroShopper.Models;
using QliroShopper.Tests.Utils;
using Xunit;

namespace QliroShopper.Tests
{
    public class TestRestApi
    {
        [Fact]
        public void TestGetEmptyList()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    var controller = new OrderController(context);
                    var response = controller.GetAllOrders();
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    var orders = (List<Order>) response.Value;
                    Assert.Empty(orders);
                }
            }
        }
        [Fact]
        public void TestGetOrder()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    var controller = new OrderController(context);
                    var response = controller.Get(1);
                    Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }
    }
}