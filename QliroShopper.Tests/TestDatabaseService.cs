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
        private readonly Order one_item_order = new Order{
                        OrderItems = new List<Item>{
                            new Item{
                                Quantity = 0,
                                Price = 2,
                                Description = "Waaaave",
                            }
                        }
                    };
        private readonly Order four_item_order = new Order{
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
        [Fact]
        public void TestGettingOneOrder()
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    new DatabaseService(context).AddOrder(one_item_order);
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
                    new DatabaseService(context).AddOrder(four_item_order);
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

        [Fact]
        public void TestModifyingOrder()
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    new DatabaseService(context).AddOrder(one_item_order);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var old_order = orderService.FindOrder(1);                    
                    var order = orderService.FindOrder(1);
                    Assert.Equal(0, order.TotalPrice);
                    order.OrderItems[0].Quantity = 1;
                    orderService.UpdateOrder(old_order, order);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var order = orderService.FindOrder(1);                    
                    Assert.Equal(order.TotalPrice, 2);
                }

            }
        }

        [Fact]
        public void TestGetItems()
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    new DatabaseService(context).AddOrder(four_item_order);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var order = orderService.FindOrder(1);
                    Assert.Equal(4, orderService.GetAllItemsForOrder(order).Count);
                }
            }
        }

        [Fact]
        public void TestGetSingleItem()
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    new DatabaseService(context).AddOrder(four_item_order);
                }
                // Test Happy Path
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var order = orderService.FindOrder(1);
                    var item = order.OrderItems[0];
                    Assert.NotNull(orderService.FindItem(order, item.Id));
                }
                // Verify that invalid IDs for both order and item would return null.
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var order = orderService.FindOrder(1);
                    var item = order.OrderItems[0];
                    var oldId = order.Id;
                    order.Id = -1;
                    // Invalid order, Valid item 
                    Assert.Null(orderService.FindItem(order, item.Id));
                    order.Id = oldId;
                    // Valid order, Invalid item
                    Assert.Null(orderService.FindItem(order, 123));
                }
            }
        }
        [Fact]
        public void TestModifyItem()
        {
            using (var connection = new TestSqliteSetup(connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {   
                    new DatabaseService(context).AddOrder(four_item_order);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var order = orderService.FindOrder(1);
                    var item = order.OrderItems[0];
                    item.Quantity = 10;
                    // Get a copy of the item.
                    var stored_item = orderService.FindItem(order, item.Id);
                    orderService.UpdateItem(item, stored_item);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var orderService = new DatabaseService(context);
                    var order = orderService.FindOrder(1);
                    var item = order.OrderItems[0];
                    Assert.Equal(10, item.Quantity);
                }
            }
        }
    }
}
