using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order

        public ActionResult Index(string searchText, int? page, int? statusFilter)
        {
            int pageSize = 5; 
            int pageIndex = page ?? 1; 
           
            var items = db.orders.OrderByDescending(x => x.OrderID).AsQueryable();

            // Tìm kiếm theo từ khóa (mã đơn hàng hoặc tên khách hàng)
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Code.Contains(searchText) || x.CustomerName.Contains(searchText));
            }

            // Lọc theo trạng thái
            if (statusFilter.HasValue)
            {
                items = items.Where(x => x.Status == statusFilter.Value);
            }

            // Phân trang
            var pagedItems = items.ToPagedList(pageIndex, pageSize);

            
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageIndex;
            ViewBag.SearchText = searchText;
            ViewBag.StatusFilter = statusFilter;

            return View(pagedItems); 
        }

        public ActionResult Details(int id)
        {
            var order = db.orders.Include("OrderDetails").FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại!");
            }
            return View(order);
        }

        [HttpPost]
        public JsonResult UpdateStatus(int id, int status)
        {
            try
            {
                var order = db.orders.FirstOrDefault(o => o.OrderID == id);
                if (order == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại!" });
                }

                // Cập nhật trạng thái thanh toán
                order.Status = status;
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        [HttpPost]
        public JsonResult UpdatePaymentMethod(int id, int typePayment)
        {
            try
            {
                var order = db.orders.FirstOrDefault(o => o.OrderID == id);
                if (order == null)
                {
                    return Json(new { success = false, message = "Đơn hàng không tồn tại!" });
                }

                // Cập nhật phương thức thanh toán
                order.TypePayment = typePayment;
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {

            var item = db.orders.Find(id);
            if (item != null)
            {
                db.orders.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }


    }
}