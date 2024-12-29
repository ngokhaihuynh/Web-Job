using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebJob
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // CHO CÁC AREA
            // Route tùy chỉnh để truy cập vào View trong Area
            // Route tùy chỉnh cho Area "Employer"
            routes.MapRoute(
                name: "EmployerHome",
                url: "Employer/home",
                defaults: new { area = "Employer", controller = "HomeEM", action = "Index" }
            ).DataTokens["area"] = "Employer"; // Xác định Area là "Employer"

            routes.MapRoute(
            name: "DangTin",
            url: "them-tin-tuyen-dung",
            defaults: new { area = "Employer", controller = "Jobs", action = "Add" },
             namespaces: new[] { "WebJob.Areas.Employer.Controllers" }
            ).DataTokens["area"] = "Employer";

            routes.MapRoute(
           name: "CVUNGTUYEN",
           url: "cv-ung-tuyen",
           defaults: new { area = "Employer", controller = "Applicant", action = "Index" },
            namespaces: new[] { "WebJob.Areas.Employer.Controllers" }
           ).DataTokens["area"] = "Employer";

            routes.MapRoute(
       name: "LienHeEmloyer",
       url: "lien-he-ho-tro",
       defaults: new { area = "Employer", controller = "HomeEM", action = "Contact" },
        namespaces: new[] { "WebJob.Areas.Employer.Controllers" }
       ).DataTokens["area"] = "Employer";


            routes.MapRoute(
       name: "JobActive",
       url: "viec-lam-da-dang",
       defaults: new { area = "Employer", controller = "Jobs", action = "Index" },
        namespaces: new[] { "WebJob.Areas.Employer.Controllers" }
       ).DataTokens["area"] = "Employer";

            // cac trang menu lon

            routes.MapRoute(
            name: "DoanhThu",
            url: "doanh-thu",
            defaults: new { area = "Admin", controller = "Order", action = "RevenueStatistics" },
            namespaces: new[] { "WebJob.Areas.Admin.Controllers" }
            ).DataTokens["area"] = "Admin";




            // Route cho tìm kiếm
            routes.MapRoute(
                name: "Search",
                url: "search",
                defaults: new { controller = "Search", action = "Search" },
                namespaces: new[] { "WebJob.Controllers" }
            );


            // trang chu
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );

           

            routes.MapRoute(
             name: "vnpay_return",
             url: "vnpay_return",
             defaults: new { controller = "ShoppingCart", action = "VnpayReturn", alias = UrlParameter.Optional },
             namespaces: new[] { "WebJob.Controllers" }
         );
            routes.MapRoute(
             name: "GoiDaMua",
             url: "goi-da-mua",
             defaults: new { controller = "ShoppingCart", action = "MyOrders", alias = UrlParameter.Optional },
             namespaces: new[] { "WebJob.Controllers" }
         );


            // tin túc
            routes.MapRoute(
           name: "News",
           url: "tin-tuc",
           defaults: new { controller = "News", action = "Index", alias = UrlParameter.Optional },
           namespaces: new[] { "WebJob.Controllers" }
       );
            // bài viết
            routes.MapRoute(
           name: "BaiViet",
           url: "post/{alias}",
           defaults: new { controller = "Articel", action = "Index", alias = UrlParameter.Optional },
           namespaces: new[] { "WebJob.Controllers" }
       );


            routes.MapRoute(
         name: "DetailNews",
         url: "{alias}-n{id}",
         defaults: new { controller = "News", action = "Detail" },
         namespaces: new[] { "WebJob.Controllers" }
     );


            // Lien he
            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Contact", alias = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );

            // cac viec lam
            routes.MapRoute(
                name: "CategoryJob",
                url: "viec-lam",
                defaults: new { controller = "Jobs", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );

            // luu cong viec
            routes.MapRoute(
             name: "SaveJob",
             url: "cong-viec-da-luu",
             defaults: new { controller = "SaveJob", action = "Index", alias = UrlParameter.Optional },
             namespaces: new[] { "WebJob.Controllers" }
         );

            // Ứng tuyển
            routes.MapRoute(
            name: "AppllyJob",
            url: "ung-tuyen",
            defaults: new { controller = "SaveJob", action = "AppllyJob", alias = UrlParameter.Optional },
            namespaces: new[] { "WebJob.Controllers" }
        );
            // Luu san pham
            routes.MapRoute(
            name: "SaveProduct",
            url: "gio-hang",
            defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
            namespaces: new[] { "WebJob.Controllers" }
        );

            // thanh toan
            routes.MapRoute(
            name: "Checkout",
            url: "thanh-toan",
            defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
            namespaces: new[] { "WebJob.Controllers" }
        );


            // cac trang menu nho
            routes.MapRoute(
               name: "CategoryJobleft",
               url: "{alias}-{id}",
               defaults: new { controller = "Jobs", action = "JobCategory", id = UrlParameter.Optional },
               namespaces: new[] { "WebJob.Controllers" }
           );
            routes.MapRoute(
               name: "CategoryJobHome",
               url: "{alias}-{id}",
               defaults: new { controller = "Jobs", action = "View_itemsByCateId", id = UrlParameter.Optional },
               namespaces: new[] { "WebJob.Controllers" }
           );
            routes.MapRoute(
               name: "AllJob",
               url: "viec-lam",
               defaults: new { controller = "Jobs", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebJob.Controllers" }
            );


            routes.MapRoute(
              name: "DetailJob",
              url: "chi-tiet/{alias}-p{id}",
              defaults: new { controller = "Jobs", action = "Detail", alias = UrlParameter.Optional },
              namespaces: new[] { "WebJob.Controllers" }
          );


            // PHAN NHÀ TUYỂN DỤNG
            //  routes.MapRoute(
            //    name: "HomeEmployer",
            //    url: "nha-tuyen-dung",
            //    defaults: new { controller = "HomeEM", action = "Index", alias = UrlParameter.Optional },
            //    namespaces: new[] { "WebJob.Controllers" }
            //);
            // Phan product

            // san pham
            routes.MapRoute(
              name: "CategoryProducts",
              url: "danh-muc-san-pham/{alias}-{id}",
              defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
              namespaces: new[] { "WebJob.Controllers" }
          );

            routes.MapRoute(
           name: "Product",
           url: "goi",
           defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
           namespaces: new[] { "WebJob.Controllers" }
       );

            routes.MapRoute(
            name: "detailProduct",
            url: "chi-tiet-goi/{alias}-p{id}",
            defaults: new { controller = "Products", action = "DetailProduct", alias = UrlParameter.Optional },
            namespaces: new[] { "WebJob.Controllers" }
        );



            // end product
            //
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );
        }
    }
}
