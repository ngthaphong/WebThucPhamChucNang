using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "PostFeed",
                "Feed/{type}",
                new { controller = "Blog", action = "PostFeed", type="blog.rss" }
            );

            routes.MapRoute(
                "Sitemap",
                "sitemap.xml",
                new { controller = "Sitemap", action = "Index" }
            );

            routes.MapRoute(
                name: "NSX",
                url: "{meta}-{id}",
                defaults: new { controller = "LHTStore", action = "SPTheoNSX", id = UrlParameter.Optional }
                //new {id = @"\d+" }
            );

            routes.MapRoute(
                name: "Loai",
                url: "{meta}-{id}",
                defaults: new { controller = "LHTStore", action = "SPTheoLoai", id = UrlParameter.Optional }
                //new {id = @"\d+" }
            );

            routes.MapRoute(
                name: "ChiTiet",
                url: "chi-tiet-san-pham/{meta}-{id}",
                defaults: new { controller = "LHTStore", action = "Details", id = UrlParameter.Optional }
                //new {id = @"\d+" }
            );

            routes.MapRoute(
                name: "TrangChu",
                url: "{controller}",
                defaults: new { controller = "LHTStore", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
