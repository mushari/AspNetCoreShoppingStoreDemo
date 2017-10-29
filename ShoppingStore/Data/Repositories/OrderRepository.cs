using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ShoppingStore.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;
        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Order> Orders
        {
            get
            {
                return context.Orders.Include(o => o.Lines)
                    .ThenInclude(l => l.Product);
            }
        }


        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderId == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
