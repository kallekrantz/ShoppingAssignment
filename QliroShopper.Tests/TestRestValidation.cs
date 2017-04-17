using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using QliroShopper.Controllers;
using QliroShopper.Models;
using QliroShopper.Tests.Utils;
using Xunit;

namespace QliroShopper.Tests
{
    public class TestRestValidation
    {
        [Fact]
        public void TestOrderAddWithValidationError()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string)){
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    controller.ModelState.AddModelError("FakeError", "FakeError");
                    var response = controller.AddOrder(new Order());
                    Assert.IsType<BadRequestObjectResult>(response);
                }
            }
        }

        [Fact]
        public void TestEditOrderWithValidationError()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string)){
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    controller.ModelState.AddModelError("FakeError", "FakeError");
                    var response = controller.EditOrder(1, new Order());
                    Assert.IsType<BadRequestObjectResult>(response);
                }
            }
        }
        [Fact]
        public void TestAddItemWithValidationError()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string)){
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    controller.ModelState.AddModelError("FakeError", "FakeError");
                    var response = controller.AddItem(1, new Item());
                    Assert.IsType<BadRequestObjectResult>(response);
                }
            }
        }
        [Fact]
        public void TestEditItemWithValidationError()
        {
            using (var connection = new TestSqliteSetup(TestDatabaseService.connection_string)){
                using (var context = new OrderContext(connection.Options))
                {
                    var controller = new OrderController(context);
                    controller.ModelState.AddModelError("FakeError", "FakeError");
                    var response = controller.EditItem(1, 1, new Item());
                    Assert.IsType<BadRequestObjectResult>(response);
                }
            }
        }
    }
}