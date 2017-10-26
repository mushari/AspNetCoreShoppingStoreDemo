using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Cart
    {
        private List<CartLine> lineList = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            // Check list already have product
            CartLine line = lineList
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (line == null)
            {
                lineList.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Product product) =>
            lineList.RemoveAll(l => l.Product.ProductId == product.ProductId);


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

        public virtual IEnumerable<CartLine> GetCartLines =>
            lineList;
    }
}
