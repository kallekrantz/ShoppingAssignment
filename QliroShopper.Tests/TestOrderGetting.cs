using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using QliroShopper.Services;
using System.Data;
using QliroShopper.Tests.Utils;
using QliroShopper.Models;
using System.Collections.Generic;

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
                    var order =  new Order{
                        OrderItems = new List<Item>{
                            new Item{
                                Quantity = 0,
                                Price = 2,
                                Description = "Waaaave",
                            }
                        }
                    };
                    new DatabaseService(context).AddOrder(order);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var order = orderService.FindOrder(1);
                    Assert.NotNull(order);
                    Assert.Equal(0, order.TotalPrice);
                    Assert.Equal(order.OrderItems.Count, 1);
                    Assert.Equal(order.OrderItems[0].Order, order);
                }

            }
        }

        [Fact]
        public void TestCalculatingTotalPrice()
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    var order =  new Order{
                        OrderItems = new List<Item>{
                            new Item{
                                Quantity = 1,
                                Price = 5,
                                Description = "Potato",
                            },
                            new Item{
                                Quantity = 2,
                                Price = 25,
                                Description = "Watermelon",
                            },
                            new Item{
                                Quantity = 3,
                                Price = 50,
                                Description = "Baked Beans",
                            },
                            new Item{
                                Quantity = 4,
                                Price = 100,
                                Description = "Meat",
                            }
                        }
                    };
                    new DatabaseService(context).AddOrder(order);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var orders = orderService.GetAllOrders();
                    Assert.NotEmpty(orders);
                    var order = orderService.FindOrder(1);
                    Assert.Equal(605, order.TotalPrice);
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
                    Assert.Null(new DatabaseService(context).FindOrder(dataId));
                }
            }

        }
    }
}
