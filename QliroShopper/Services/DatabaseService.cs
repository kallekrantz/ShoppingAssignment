using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using QliroShopper.Models;

namespace QliroShopper.Services
{
    public class DatabaseService
    {
        private OrderContext _context;

        public DatabaseService(OrderContext context)
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

        public void AddOrder(Order o)
        {
            _context.Orders.Add(o);
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