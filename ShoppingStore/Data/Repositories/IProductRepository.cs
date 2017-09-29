﻿using ShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Data.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
       
    }
}
