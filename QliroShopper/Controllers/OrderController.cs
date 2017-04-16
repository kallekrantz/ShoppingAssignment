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
        public IEnumerable<Order> Get()
        {
            var orderService = new DatabaseService(context);
            return orderService.GetAllOrders();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) 
            {
                return new NotFoundResult();
            }
            else
            {
                return Ok(order);                
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Order order)
        {
            var orderService = new DatabaseService(context);
            orderService.AddOrder(order);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = context.Orders.Find(id);
            if (order == null) 
            {
                return new NotFoundResult();
            }
            context.Remove(order);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}/item")]
        public IActionResult GetItems(int id)
        {
            return Ok();
        }
        
        [HttpPost("{id}/item")]
        public IActionResult AddItem(int id, [FromBody]Item item)
        {
            var orderService = new DatabaseService(context);
            var order = orderService.FindOrder(id);
            if (order == null) return NotFound();
            orderService.AddItem(order, item);
            return Ok();
        }
        
        [HttpGet("{id}/item/{itemId}")]
        public IActionResult GetItem(int id, int itemId)
        {
            return Ok();
        }

        [HttpPut("{id}/item/{itemId}")]
        public IActionResult EditItem(int id, int itemId, [FromBody]string val)
        {
            return Ok();
        }

        [HttpDelete("{id}/item/{itemId}")]
        public IActionResult RemoveItem(int id, int itemId)
        {
            return Ok();
        }
    }
}
