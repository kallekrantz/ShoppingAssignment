using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using QliroShopper.Services;
using System.Data;
using QliroShopper.Tests.Utils;

namespace QliroShopper.Tests
{
    public class TestOrderGetting
    {
        private string connection_string = "Data Source=:memory:";
        [Fact]
        public void TestGettingOneOrder()
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    new OrderService(context).AddOrder("{}");
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new OrderService(context);
                    var orders = orderService.GetAllOrders();
                    Assert.NotEmpty(orders);
                    var order = orderService.FindOrder(1);
                    Assert.Equal(0, order.TotalPrice);
                }

            }
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGettingNotExistingOrder(int dataId)
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    Assert.Null(new OrderService(context).FindOrder(dataId));
                }
            }

        }
    }
}
