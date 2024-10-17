using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class SaveJobController : Controller
    {
        // GET: SaveJob
        public ActionResult Index()
        {
            SaveJob save = (SaveJob)Session["Save"];
            if(save != null)
            {
                return View(save.Items);
            }
            return View();
        }

        public ActionResult ShowCount()
        {
            SaveJob save = (SaveJob)Session["Save"];
            if(save != null)
            {
                return Json(new { Count = save.Items.Count }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { Count = 0}, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult AddSaveJob(int id)
        {
            var code = new { success = false, msg="", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkJob = db.Jobs.FirstOrDefault(x => x.JobID == id);
            if(checkJob != null)
            {
                SaveJob save = (SaveJob)Session["Save"];
                if(save == null)
                {
                    save = new SaveJob();
                }
                SaveJobItem item = new SaveJobItem
                {
                    SaveJobId = checkJob.JobID,
                    SaveJobName = checkJob.JobTitle,
                    SaveJobCate = checkJob.JobCategory.CategoryName,
                    Alias = checkJob.Alias,
                    SaveJobSalaryMin = checkJob.Salary.SalaryMin,
                    SaveJobSalaryMax = checkJob.Salary.SalaryMax

                };
                if (checkJob.CompanyImages.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.SaveJobImg = checkJob.CompanyImages.FirstOrDefault(x => x.IsDefault).ImageURL;
                }

                save.AddSaveJob(item);
                Session["Save"] = save;
                code = new { success = true, msg = "Lưu thành công", code = 1, Count = save.Items.Count };

            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { success = false, msg = "", code = -1, Count = 0 };
            SaveJob save = (SaveJob)Session["Save"];
            if (save != null)
            {
                var checkJob = save.Items.FirstOrDefault(x => x.SaveJobId == id);
                if(checkJob != null)
                {
                    save.Remove(id);
                    code = new { success = true, msg = "", code = 1, Count = save.Items.Count };
                }
            }
            return Json(code);
        }


        public ActionResult GetSaveJobItems()
        {
            SaveJob save = (SaveJob)Session["Save"];
            if (save != null)
            {
                return PartialView("_SaveJobView", save.Items);
            }
            return PartialView("_SaveJobView", new List<SaveJobItem>()); // trả về danh sách rỗng nếu không có dữ liệu
        }

    }
}