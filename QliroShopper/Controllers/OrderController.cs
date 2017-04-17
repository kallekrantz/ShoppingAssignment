using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QliroShopper.Models;
using QliroShopper.Services;

namespace QliroShopper.Controllers
{
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly OrderContext context;

        public OrderController(OrderContext context){
            this.context = context;
        }

        // GET api/values
        [HttpGet]
        public ObjectResult GetAllOrders()
        {
            var orderService = new DatabaseService(context);
            return Ok(orderService.GetAllOrders());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ObjectResult GetOrder(int id)
        {
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return NotFound("Could not find order");
            return Ok(order);
        }

        // POST api/values
        [HttpPost]
        public IActionResult AddOrder([FromBody]Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var orderService = new DatabaseService(context);
            orderService.AddOrder(order);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult EditOrder(int id, [FromBody]Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var service = new DatabaseService(context);
            var stored_order = service.FindOrder(id);
            if (stored_order == null) return NotFound();
            service.UpdateOrder(stored_order, order);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public StatusCodeResult RemoveOrder(int id)
        {
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return new NotFoundResult();
            context.Remove(order);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}/item")]
        public ObjectResult GetItems(int id)
        {
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return NotFound("Order not found");
            return Ok(orderService.GetAllItemsForOrder(order));
        }

        [HttpPost("{id}/item")]
        public IActionResult AddItem(int id, [FromBody]Item item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return NotFound();
            orderService.AddItem(order, item);
            return Ok();
        }

        [HttpGet("{id}/item/{itemId}")]
        public ObjectResult GetItem(int id, int itemId)
        {
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return NotFound("Order not found");
            var item = orderService.FindItem(order, itemId);
            if (item == null) return NotFound("Item not found");
            return Ok(item);
        }

        [HttpPut("{id}/item/{itemId}")]
        public IActionResult EditItem(int id, int itemId, [FromBody]Item updated_item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return NotFound();
            var item = orderService.FindItem(order, itemId);
            if (item == null) return NotFound();
            orderService.UpdateItem(item, updated_item);
            return Ok();
        }

        [HttpDelete("{id}/item/{itemId}")]
        public StatusCodeResult RemoveItem(int id, int itemId)
        {
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return NotFound();
            var item = orderService.FindItem(order, itemId);
            if (item == null) return NotFound();
            context.Remove(item);
            context.SaveChanges();
            return Ok();
        }
    }
}
