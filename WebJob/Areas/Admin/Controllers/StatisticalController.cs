using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }

        /* [HttpGet]
         public ActionResult GetStatiscal(string fromDate, string toDate)
         {
             var query = from o in db.orders
                         join od in db.orderDetails
                         on o.OrderID equals od.OrderID
                         join p in db.products
                         on od.ProductID equals p.ProductID
                         select new
                         {
                             Createdate = o.CreatedDate,
                             Quantity = od.Quantity,
                             Price = od.Price,
                             OriginPrice = p.OriginalPrice
                         };
             if(!string.IsNullOrEmpty(fromDate) )
             {
                 DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                 query = query.Where(x => x.Createdate >= startDate);
             }
             if (!string.IsNullOrEmpty(toDate))
             {
                 DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                 query = query.Where(x => x.Createdate < endDate);
             }

             var result = query.GroupBy(x => DbFunctions.TruncateTime(x.Createdate)).Select(x => new
             {
                 Date = x.Key.Value,
                 TotalBuy = x.Sum(y => y.Quantity * y.OriginPrice),
                 TotalSell = x.Sum(y => y.Quantity * y.Price)
             }).Select(x => new
             {
                 Date = x.Date,
                 DoanhThu = x.TotalSell,
                 LoiNhuan = x.TotalSell - x.TotalBuy
             });


             return Json(new { Data = result },JsonRequestBehavior.AllowGet );
         }

     }*/

        [HttpGet]
        public ActionResult GetStatiscal(string fromDate, string toDate)
        {
            var query = from o in db.orders
                        join od in db.orderDetails on o.OrderID equals od.OrderID
                        join p in db.products on od.ProductID equals p.ProductID
                        select new
                        {
                            Createdate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginPrice = p.OriginalPrice
                        };

            // Kiểm tra và parse ngày bắt đầu
            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParseExact(fromDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.Createdate) >= startDate.Date);
                }
            }

            // Kiểm tra và parse ngày kết thúc
            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.Createdate) <= endDate.Date);
                }
            }

            // Xử lý nhóm và tính toán
            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.Createdate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price)
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
    }
    }