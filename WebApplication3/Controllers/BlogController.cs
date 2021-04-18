using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class BlogController : Controller
    {
        dbQLSanPhamDataContext db = new dbQLSanPhamDataContext();
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PostFeed(string type)
        {
            NHASANXUAT nsx = db.NHASANXUATs.Where(s => s.Meta.Contains(type)).FirstOrDefault();
            //check null
            if (nsx == null)
            {
                return HttpNotFound();
            }
            IEnumerable<SANPHAM> posts = (from s in db.SANPHAMs where s.NHASANXUAT.Meta.Contains(type) select s).ToList();
            var feed=new SyndicationFeed(nsx.TenNSX, "RSS Feed",
                new Uri("https://nthphong.somee.com/RSS"),
                Guid.NewGuid().ToString(),
                DateTime.Now);
            var items = new List<SyndicationItem>();
            foreach(SANPHAM sp in posts)
            {
                string postUrl = String.Format("http://nthphong.somee.com/" + sp.Meta + "-{0}", sp.MaSP);
                var item = new SyndicationItem(Helper.Helper.RemoveIllegalCharacters(sp.TenSanPham),
                    Helper.Helper.RemoveIllegalCharacters(sp.Mota),
                    new Uri(postUrl),
                    sp.MaSP.ToString(),
                    sp.Ngaycapnhat.Value);
                items.Add(item);
            }
            feed.Items = items;
            return new RSSActionResult { Feed = feed };
        }

        public ActionResult ReadRSS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReadRSS(string url)
        {
            WebClient wclient = new WebClient();
            wclient.Encoding = ASCIIEncoding.UTF8;
            string RSSData = wclient.DownloadString(url);

            XDocument xml = XDocument.Parse(RSSData, LoadOptions.PreserveWhitespace);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new Rss20FeedFormatter
                               {
                                   //TenSanPham = ((string)x.Element("tensanpham")),
                                   //Link = ((string)x.Element("link")),
                                   //Mota = ((string)x.Element("mota")),
                                   //Ngaycapnhat = ((string)x.Element("ngaycapnhat"))
                               });
            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.URL = url;
            return View();
        }
    }
}