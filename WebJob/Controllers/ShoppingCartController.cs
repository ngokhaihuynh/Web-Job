using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;
using WebJob.Models.payment;

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

        public ActionResult MyOrders(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = User.Identity.GetUserId(); // Lấy UserId của người dùng hiện tại
            }

            // Lấy danh sách các đơn hàng
            var orders = db.orders
                           .Where(o => o.UserId == userId)
                           .OrderByDescending(o => o.CreatedDate)
                           .ToList();

            return View(orders);
        }
    


    public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }


        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }

        // thanh toán
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            // Kiểm tra nếu người dùng chưa đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    success = false,
                    Code = -1,
                    Url = Url.Action("Login", "Account") // Redirect đến trang đăng nhập
                });
            }

            System.Diagnostics.Debug.WriteLine($"TypePaymentVN: {req.TypePaymentVN}");
            var code = new { success = false, Code = -1, Url = "" };
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
                    order.Status = 1; // chưa thanh toán
                    order.UserId = User.Identity.GetUserId();
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductID = x.SaveProductId,
                        Quantity = x.Quantity,
                        Price = x.Price,

                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.ModifiedBy = req.CustomerName;
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = req.Phone;
                    order.ModifiedDate = DateTime.Now;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    db.orders.Add(order);
                    db.SaveChanges();
                    cart.ClearCart();
                    code = new { success = true, Code = req.TypePayment, Url = "" };
                    //var url = "";
                    if (req.TypePayment == 2)
                    {
                        var url = UrlPayment(req.TypePaymentVN, order.Code);
                        code = new { success = true, Code = req.TypePayment, Url = url };
                    }
                }
            }
            return Json(code);
        }


        public ActionResult OrderDetails(int id)
        {
            var order = db.orders.Include("OrderDetails")
                                 .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
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
                if (cart == null)
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

        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.orders.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.Status = 2;//đã thanh toán
                            db.orders.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }

        /* public string UrlPayment(int TypePaymentVN, string orderCode)
         {

             var urlPayment = "";
             var order = db.orders.FirstOrDefault(x => x.Code == orderCode);

             //Get Config Info
             string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
             string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
             string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
             string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

             VnPayLibrary vnpay = new VnPayLibrary();//Build URL for VNPAY
             var Price = (long)order.TotalAmount * 100;

             vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
             vnpay.AddRequestData("vnp_Command", "pay");
             vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
             vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
             if (TypePaymentVN == 1)
             {
                 vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
             }
             else if (TypePaymentVN == 2)
             {
                 vnpay.AddRequestData("vnp_BankCode", "VNBANK");
             }
             else if (TypePaymentVN == 3)
             {
                 vnpay.AddRequestData("vnp_BankCode", "INTCARD");
             }

             vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
             vnpay.AddRequestData("vnp_CurrCode", "VND");
             vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
             vnpay.AddRequestData("vnp_Locale", "vn");
             vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderID);
             vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

             vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
             vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là du

             urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
             return urlPayment;
         }
 */

        #region Thanh toán vnpay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = db.orders.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        #endregion

    }
}