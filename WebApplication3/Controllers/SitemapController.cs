using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Helper;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class SitemapController : Controller
    {
        // GET: Sitemap
        public ActionResult Index()
        {
            var sitemapItems = new List<SitemapItem> {
            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority),"/cellucor"), changeFrequency:
            SitemapChangeFrequency.Hourly, priority: 0.9),

            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority),"/chi-tiet-san-pham/cellucor-cor-4-2-lb"), 
            changeFrequency:SitemapChangeFrequency.Daily, priority:0.9),

            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority),"/chi-tiet-san-pham/s-a-n-mass-effect-revolution-7-lb"),
            changeFrequency:SitemapChangeFrequency.Daily, priority:0.9),

            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority),"/chi-tiet-san-pham/dymatize-iso-100-isolate-5-lb"),
            changeFrequency:SitemapChangeFrequency.Daily, priority:0.9),

            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority),"/chi-tiet-san-pham/s-a-n-titanium-whey-isolate-supreme"),
            changeFrequency:SitemapChangeFrequency.Daily, priority:0.9),

            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority),"/chi-tiet-san-pham/c4-mass"),
            changeFrequency:SitemapChangeFrequency.Daily, priority:0.9)
            };

            return new SitemapResult(sitemapItems);
        }
    }
}