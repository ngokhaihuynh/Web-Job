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


        /* public ActionResult Index(string SearchText, int? page, int? statusFilter)
         {
             var pageSize = 10; // Số lượng đơn hàng trên mỗi trang
             if (page == null)
             {
                 page = 1;
             }

             // Lấy danh sách đơn hàng, sắp xếp theo ID giảm dần
             IEnumerable<Order> items = db.orders.OrderByDescending(x => x.OrderID);

             // Tìm kiếm theo từ khóa (Code hoặc CustomerName)
             if (!string.IsNullOrEmpty(SearchText))
             {
                 items = items.Where(x => x.Code.Contains(SearchText) || x.CustomerName.Contains(SearchText));
             }

             // Lọc theo trạng thái
             if (statusFilter.HasValue)
             {
                 if (statusFilter == 1) // Chưa thanh toán
                 {
                     items = items.Where(x => x.Status == 1);
                 }
                 else if (statusFilter == 2) // Đã thanh toán
                 {
                     items = items.Where(x => x.Status == 2);
                 }
             }


             // Phân trang
             var pageIndex = page ?? 1;
             items = items.ToPagedList(pageIndex, pageSize);


             ViewBag.PageSize = pageSize;
             ViewBag.Page = page;
             ViewBag.SearchText = SearchText; // Giữ nội dung tìm kiếm
             ViewBag.StatusFilter = statusFilter; // Giữ trạng thái lọc

             return View(items);
         }

 */

        public ActionResult Index(string searchText, int? page, int? statusFilter)
        {
            int pageSize = 10; // Số lượng đơn hàng trên mỗi trang
            int pageIndex = page ?? 1; // Trang hiện tại (mặc định là trang 1)

            // Lấy danh sách đơn hàng, sắp xếp giảm dần theo OrderID
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

            // Truyền dữ liệu vào ViewBag để giữ lại thông tin sau khi lọc
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageIndex;
            ViewBag.SearchText = searchText;
            ViewBag.StatusFilter = statusFilter;

            return View(pagedItems); // Trả về danh sách phân trang
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

      

      
    }
}