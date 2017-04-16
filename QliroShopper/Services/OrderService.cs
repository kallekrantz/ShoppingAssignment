using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using QliroShopper.Models;

namespace QliroShopper.Services
{
    public class OrderService
    {
        private OrderContext _context;

        public OrderService(OrderContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders(){
            return _context.Orders
                           .Include(b => b.OrderItems) 
                           .ToList();
        }
        public Order FindOrder(int orderId)
        {
            return _context.Orders
                           .Include(b => b.OrderItems)
                           .Where(b => b.Id == orderId)
                           .FirstOrDefault();
        }

        public void AddOrder(string v)
        {
            _context.Orders.Add(new Order{});
            _context.SaveChanges();
        }

        public void AddItem(Order order, Item item){
            order.OrderItems.Add(item);
            _context.Add(item);
            _context.Update(order);
            _context.SaveChanges();
        }
    }
}