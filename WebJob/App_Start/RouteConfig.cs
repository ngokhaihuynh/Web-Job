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

            // cac trang menu lon

            // trang chu
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );
            
            // Lien he
            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
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
