using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        
        public ActionResult Index()
        {
            //ShoppingCart cart = (ShoppingCart)Session["Cart"];
            //if (cart != null)
            //{
            //    return View(cart.Items);
            //}
            return View();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                ViewBag.CheckCart=cart;
            }
            return View();
        }


        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }


        // thanh toán
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { success = false, code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    order.Email = req.Email;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductID = x.SaveProductId,
                        Quantity = x.Quantity,
                        Price = x.Price,

                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = req.Phone;
                    order.ModifiedDate = DateTime.Now;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9)+ rd.Next(0, 9);
                    db.orders.Add(order);
                    db.SaveChanges();
                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

        public ActionResult CheckOutSuccess()
        {
          
            return View();
        }
        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return View(cart.Items);
            }

            return PartialView();
        }


        public ActionResult Partial_Iteam_Cart_Page()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return View(cart.Items);
            }

            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                // Tổng số lượng của tất cả sản phẩm trong giỏ hàng
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkJob = cart.Items.FirstOrDefault(x => x.SaveProductId == id);
                if (checkJob != null)
                {
                    cart.Remove(id);
                    code = new { success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { success = false, msg = "", code = -1, count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.products.FirstOrDefault(x => x.ProductID == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if(cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    SaveProductId = checkProduct.ProductID,
                    SaveProductName = checkProduct.Title,
                    SaveProductCate = checkProduct.CategoryProduct.Title,
                    Alias = checkProduct.Alias,
                    SaveProductSalaryMin = checkProduct.PriceSale,
                    SaveProductSalaryMax = checkProduct.Price,
                    Quantity = quantity


                };
                if (checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.SaveProductImg = checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.Total = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { success = true, msg = "Thêm thành công", code = 1, count = cart.Items.Count };
            }
            return Json(code);
        }

        // mua ngay
        [HttpPost]
        public ActionResult BuyNow(int id)
        {
            var db = new ApplicationDbContext();
            var product = db.products.FirstOrDefault(x => x.ProductID == id);

            if (product != null)
            {
                // Tạo một giỏ hàng tạm thời cho mục "Mua Ngay"
                ShoppingCart cart = new ShoppingCart();

                ShoppingCartItem item = new ShoppingCartItem
                {
                    SaveProductId = product.ProductID,
                    SaveProductName = product.Title,
                    SaveProductCate = product.CategoryProduct.Title,
                    Alias = product.Alias,
                    SaveProductSalaryMin = product.PriceSale,
                    SaveProductSalaryMax = product.Price,
                    Quantity = 1,
                    Price = product.PriceSale > 0 ? (decimal)product.PriceSale : product.Price,
                    Total = (product.PriceSale > 0 ? (decimal)product.PriceSale : product.Price)
                };

                if (product.ProductImages.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.SaveProductImg = product.ProductImages.FirstOrDefault(x => x.IsDefault).Image;
                }

                cart.AddToCart(item, 1);
                Session["Cart"] = cart;  // Lưu giỏ hàng tạm thời vào session

                return RedirectToAction("CheckOut");
            }
            return RedirectToAction("Index"); // Nếu sản phẩm không tồn tại, trở về trang chính
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            try
            {
                // Giả sử bạn có hàm `ClearCart` trong session hoặc database
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    cart.ClearCart();
                    Session["Cart"] = cart;
                }
                return Json(new { success = true, message = "Xóa toàn bộ giỏ hàng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }


    }
}