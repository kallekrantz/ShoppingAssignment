using System;
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
       public void UpdateOrder(Order old_order, Order updated_order)
        {
            _context.Entry(old_order).CurrentValues.SetValues(updated_order);
            _context.SaveChanges();
        }
        public void AddItem(Order order, Item item)
        {
            order.OrderItems.Add(item);
            _context.Add(item);
            _context.Update(order);
            _context.SaveChanges();
        }

        public IList<Item> GetAllItemsForOrder(Order order)
        {
            return _context.Items
                           .Where(b => b.Order.Id == order.Id).ToList();
        }

        public Item FindItem(Order order, int itemId)
        {
            return _context.Items
                           .Where(b => b.Order.Id == order.Id && b.Id == itemId)
                           .FirstOrDefault();
        }

        public void UpdateItem(Item item, Item updated_item)
        {
            _context.Entry(item).CurrentValues.SetValues(updated_item);
            _context.SaveChanges();        
        }
    }
}