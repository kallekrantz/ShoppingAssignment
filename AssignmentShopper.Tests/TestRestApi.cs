using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AssignmentShopper.Controllers;
using AssignmentShopper.Models;
using AssignmentShopper.Tests.Utils;
using Xunit;

namespace AssignmentShopper.Tests
{
    public class TestRestApi
    {
        [Fact]
        public void TestGetNoOrders()
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
        public void TestGetMultipleOrders()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var responses = new List<IActionResult>{
                        controller.AddOrder(new Order()),
                        controller.AddOrder(new Order()),
                        controller.AddOrder(new Order()),
                        controller.AddOrder(new Order()),
                        controller.AddOrder(new Order()),
                    };
                    Assert.All(responses, r => Assert.IsType<OkResult>(r));
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetAllOrders();
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    var orders = (List<Order>) response.Value;
                    Assert.Equal(5, orders.Count);
                }
            }
        }
        [Fact]
        public void TestGetNotExistingOrder()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetOrder(1);
                    Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }

        [Fact]
        public void TestCreateOrderAndGetIt()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order());
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetOrder(1);
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                }
            }
        }
        [Fact]
        public void TestCreateOrderAndRemoveIt()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order());
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetOrder(1);
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    var remove_response = controller.RemoveOrder(1);
                    Assert.Equal((int)HttpStatusCode.OK, remove_response.StatusCode);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetOrder(1);
                    Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }
        [Fact]
        public void TestRemoveNotExistingOrder()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var remove_response = controller.RemoveOrder(1);
                    Assert.Equal((int)HttpStatusCode.NotFound, remove_response.StatusCode);
                }
            }
        }
        [Fact]
        public void TestCreateOrderAndModifyIt()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                // Add the order
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order());
                    Assert.IsType<OkResult>(response);
                }
                // Modify the order
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetOrder(1);
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    var order = (Order)response.Value;
                    Assert.Empty(order.OrderItems);
                    order.OrderItems.Add(new Item{
                        Quantity = 1,
                        Price = 2,
                        Description = "Apple"
                    });
                    var modify_response = controller.EditOrder(1, order);
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                }
                // Verify the order
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetOrder(1);
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    var order = (Order)response.Value;
                    Assert.Equal(1, order.OrderItems.Count);
                    Assert.Equal(1, order.OrderItems[0].Quantity);
                    Assert.Equal("Apple", order.OrderItems[0].Description);
                    Assert.Equal(2, order.OrderItems[0].Price);
                    Assert.Equal(order.TotalPrice, 2);
                }
            }
        }
        [Fact]
        public void TestEditNotExistingOrder()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var order = new Order();
                    order.OrderItems = new List<Item>();
                    order.OrderItems.Add(new Item{
                        Quantity = 1,
                        Price = 2,
                        Description = "Apple"
                    });
                    var response = controller.EditOrder(1, order);
                    Assert.IsType<NotFoundResult>(response);
                }
            }
        }
        [Fact]
        public void TestAddItem()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order());
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddItem(1, new Item{
                        Quantity = 1,
                        Price = 4,
                        Description = "Pineapple"
                    });
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetItems(1);
                    var items = (List<Item>) response.Value;
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(1, items.Count);
                    var item = items[0];
                    Assert.Equal(1, item.Quantity);
                    Assert.Equal(4, item.Price);
                    Assert.Equal("Pineapple", item.Description);
                }
            }
        }
        [Fact]
        public void TestGetItem()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order{
                        OrderItems = new List<Item>{
                            new Item{
                                Quantity = 1,
                                Price = 4,
                                Description = "Pineapple"
                            }
                        }
                    });
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetItem(1, 1);
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    var item = (Item) response.Value;
                    Assert.Equal(1, item.Quantity);
                    Assert.Equal(4, item.Price);
                    Assert.Equal("Pineapple", item.Description);
                }
            }
        }
        [Fact]
        public void TestGetItemNotExistingOrder()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetItem(1, 1);
                    Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }
        [Fact]
        public void TestGetItemNotExistingItem()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order());
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetItem(1, 1);
                    Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }
        [Fact]
        public void TestAddItemToNotExistingOrder()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddItem(1, new Item{
                        Quantity = 1,
                        Price = 4,
                        Description = "Pineapple"
                    });
                    Assert.IsType<NotFoundResult>(response);
                }
            }
        }
        [Fact]
        public void TestEditItem()
        {
           using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order{
                        OrderItems = new List<Item>{
                            new Item{
                                Quantity = 1,
                                Price = 4,
                                Description = "Pineapple"
                            }
                        }
                    });
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.EditItem(1, 1, new Item{
                                Quantity = 3,
                                Price = 2,
                                Description = "Apple"
                            });
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetItems(1);
                    var items = (List<Item>) response.Value;
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(1, items.Count);
                    var item = items[0];
                    Assert.Equal(3, item.Quantity);
                    Assert.Equal(2, item.Price);
                    Assert.Equal("Apple", item.Description);
                }
            }
        }
        [Fact]
        public void TestEditItemNotExistingOrder()
        {
           using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.EditItem(1, 1, new Item{
                                Quantity = 3,
                                Price = 2,
                                Description = "Apple"
                            });
                    Assert.IsType<NotFoundResult>(response);
                }
            }
        }
        [Fact]
        public void TestEditItemNotExistingItem()
        {
           using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order{});
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.EditItem(1, 1, new Item{
                                Quantity = 3,
                                Price = 2,
                                Description = "Apple"
                            });
                    Assert.IsType<NotFoundResult>(response);
                }
            }
        }
        [Fact]
        public void TestRemoveItem()
        {
           using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order{
                        OrderItems = new List<Item>{
                            new Item{
                                Quantity = 1,
                                Price = 4,
                                Description = "Pineapple"
                            }
                        }
                    });
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.RemoveItem(1, 1);
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.GetItems(1);
                    var items = (List<Item>) response.Value;
                    Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
                    Assert.Empty(items);
                }
            }
        }
        [Fact]
        public void TestRemoveItemNotExistingOrder()
        {
           using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.RemoveItem(1, 1);
                    Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }
        [Fact]
        public void TestRemoveItemNotExistingItem()
        {
           using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string))
            {
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.AddOrder(new Order{
                        OrderItems = new List<Item>{}
                    });
                    Assert.IsType<OkResult>(response);
                }
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    var response = controller.RemoveItem(1, 1);
                    Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }
    }
}