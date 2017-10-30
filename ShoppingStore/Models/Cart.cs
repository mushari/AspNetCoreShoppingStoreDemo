using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Cart
    {
        private List<CartLine> lineList = new List<CartLine>();

        public virtual void AddItem(Product product, ApplicationUser user, int quantity)
        {
            // Check list already have product
            CartLine line = lineList
                .Where(l => l.Product.ProductId == product.ProductId
                && l.User.Id == user.Id)
                .SingleOrDefault();

            if (line == null)
            {
                lineList.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                    User = user
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveItem(Product product, int quantity)
        {
            var item = lineList.Where(l => l.Product.ProductId == product.ProductId).SingleOrDefault();
            if (item != null && item.Quantity > 0)
            {
                item.Quantity -= quantity;
            }
            if (item.Quantity <= 0)
            {
                RemoveLine(product);
            }
        }

        public virtual void RemoveLine(Product product)
        {
            lineList.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }


        public virtual decimal ComputeTotalValue(string culture)
        {
            return lineList.Where(
                l => l.Product.ProductId.EndsWith("_" + culture)).Sum(e =>
                e.Product.Price * e.Quantity);
        }

        public virtual void Clear()
        {
            lineList.Clear();
        }

        public virtual IEnumerable<CartLine> GetCartLines => lineList;


    }
}
