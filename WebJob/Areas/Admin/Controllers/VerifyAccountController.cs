using PagedList;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class VerifyAccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/VerifyAccount
        public ActionResult Index(int? page, string filter = "all")
        {
            // Lấy danh sách từ cơ sở dữ liệu
            var query = db.employerVerifications.AsQueryable();

            if (filter == "verified")
            {
                query = query.Where(x => x.IsVerified);
            }
            else if (filter == "unverified")
            {
                query = query.Where(x => !x.IsVerified);
            }

            // Số lượng phần tử mỗi trang
            int pageSize = 10;
            // Số trang hiện tại
            int pageNumber = page ?? 1;

            // Chuyển danh sách thành PagedList
            var model = query.OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);

            // Truyền filter vào ViewBag để hiển thị trạng thái nút lọc
            ViewBag.Filter = filter;

            return View(model);
        }

        // GET: Admin/VerifyAccount/Details/5
        public ActionResult Details(int id)
        {
            var account = db.employerVerifications.FirstOrDefault(e => e.Id == id);

            if (account == null)
            {
                return HttpNotFound("Không tìm thấy tài khoản.");
            }

            // Kiểm tra và đảm bảo đường dẫn tài liệu có thể truy cập từ trình duyệt (URL tương đối)
            if (!string.IsNullOrEmpty(account.VerificationDocumentPath))
            {
                account.VerificationDocumentPath = Url.Content("~/Uploads/VerificationDocuments/" + Path.GetFileName(account.VerificationDocumentPath));
            }

            return View(account);
        }



        // POST: Admin/VerifyAccount/Verify/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(int id)
        {
            // Lấy tài khoản từ cơ sở dữ liệu
            var account = db.employerVerifications.FirstOrDefault(e => e.Id == id);
            if (account == null)
            {
                return HttpNotFound("Không tìm thấy tài khoản.");
            }

            // Cập nhật trạng thái xác thực
            account.IsVerified = true;

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Quay lại trang chi tiết hoặc danh sách
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(int id)
        {
            var account = db.employerVerifications.FirstOrDefault(e => e.Id == id);
            if (account == null)
            {
                return HttpNotFound("Không tìm thấy tài khoản.");
            }

            // Thay đổi trạng thái xác thực (đảo ngược trạng thái)
            account.IsVerified = !account.IsVerified;

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Quay lại trang chi tiết tài khoản
            return RedirectToAction("Details", new { id = id });
        }
    }
}
