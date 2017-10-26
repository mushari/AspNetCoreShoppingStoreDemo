using ShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Data.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; set; }
        void SaveOrder(Order order);
    }
}
