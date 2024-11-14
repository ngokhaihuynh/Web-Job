using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebJob.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
        }

        public void AddToCart(ShoppingCartItem item, int quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.SaveProductId == item.SaveProductId);
            if (checkExits != null)
            {
                checkExits.Quantity += quantity;
                checkExits.Total = checkExits.Price * checkExits.Quantity;

                checkExits.SaveProductName = item.SaveProductName;
                checkExits.SaveProductCate = item.SaveProductCate;
                checkExits.SaveProductImg = item.SaveProductImg;
                checkExits.SaveProductSalaryMin = item.SaveProductSalaryMin;
                checkExits.SaveProductSalaryMax = item.SaveProductSalaryMax;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x => x.SaveProductId == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }

        }
        public decimal GetTotal()
        {
            return Items.Sum(x => x.Total);
        }

        public decimal GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }

       

    }

    public class ShoppingCartItem
    {
        public int SaveProductId { get; set; }
        public string SaveProductName { get; set; }
        public string Alias { get; set; }
        public string SaveProductCate { get; set; }
        public string SaveProductImg { get; set; }
        public decimal? SaveProductSalaryMin { get; set; }
        public decimal SaveProductSalaryMax { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

    }
}