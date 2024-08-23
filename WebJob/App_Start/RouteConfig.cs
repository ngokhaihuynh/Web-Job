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

            // cac tranh menu lon
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );
            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );

            routes.MapRoute(
                name: "CategoryJob",
                url: "viec-lam",
                defaults: new { controller = "Jobs", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebJob.Controllers" }
            );
            routes.MapRoute(
             name: "SaveJob",
             url: "cong-viec-da-luu",
             defaults: new { controller = "SaveJob", action = "Index", alias = UrlParameter.Optional },
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
