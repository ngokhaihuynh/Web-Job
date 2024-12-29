using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        // Phương thức Index để hiển thị danh sách sản phẩm
        public ActionResult Index(int? id, int page = 1)
        {
            var products = db.products.AsQueryable();

            // Lọc sản phẩm theo danh mục nếu có id
            if (id != null)
            {
                products = products.Where(x => x.CateProId == id);
            }

            // Chuyển đổi thành IPagedList với số lượng sản phẩm trên mỗi trang (3 sản phẩm/trang)
            var pagedProducts = products.OrderBy(x => x.ProductID).ToPagedList(page, 3);

            // Truyền dữ liệu vào View
            ViewBag.CategoryId = id;  // Để giữ lại giá trị danh mục đã chọn khi phân trang
            return View(pagedProducts);
        }


        public ActionResult DetailProduct(string alias, int id)
        {

            var item = db.products.Find(id);
            var defaultImages = item.ProductImages.Where(x => x.IsDefault).ToList();
            ViewBag.DefaultImages = defaultImages;
            return View(item);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.CateProId == id).ToList();
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult View_itemsByCatePdId(int page = 1, int pageSize = 5)
        {
            var items = db.products.Where(x => x.IsActive).OrderBy(x => x.ProductID).ToPagedList(page, pageSize);
            return PartialView(items);
        }


    }
}