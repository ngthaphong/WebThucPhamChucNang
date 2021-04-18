using System;
using System.Collections;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Xml;

namespace WebApplication3.Controllers
{
    public class SEOController : Controller
    {
        [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public class Sitemap
        {
            private ArrayList _map;

            public Sitemap()
            {
                _map = new ArrayList();
            }

            [XmlElement("url")]
            public Location[] Locations
            {
                get
                {
                    Location[] items = new Location[_map.Count];
                    _map.CopyTo(items);
                    return items;
                }
                set
                {
                    if (value == null)
                        return;
                    var items = (Location[])value;
                    _map.Clear();
                    foreach (Location item in items)
                        _map.Add(item);
                }
            }

            public int Add(Location item)
            {
                return _map.Add(item);
            }
        }

        public class Location
        {
            public enum EChangeFrequency
            {
                Always,
                Hourly,
                Daily,
                Weekly,
                Monthly,
                Yearly,
                Never
            }

            [XmlElement("loc")]
            public string Url { get; set; }

            [XmlElement("changefreq")]
            public EChangeFrequency? ChangeFrequency { get; set; }
            public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

            [XmlElement("lastmod")]
            public DateTime? LastModified { get; set; }
            public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

            [XmlElement("priority")]
            public double? Priority { get; set; }
            public bool ShouldSerializePriority() { return Priority.HasValue; }
        }

        public class XmlResult : ActionResult
        {
            private readonly object _objectToSerialize;

            public XmlResult(object objectToSerialize)
            {
                _objectToSerialize = objectToSerialize;
            }

            public object ObjectToSerialize
            {
                get { return _objectToSerialize; }
            }

            public override void ExecuteResult(ControllerContext context)
            {
                if (_objectToSerialize != null)
                {
                    context.HttpContext.Response.Clear();
                    var xs = new XmlSerializer(_objectToSerialize.GetType());
                    context.HttpContext.Response.ContentType = "text/xml";
                    xs.Serialize(context.HttpContext.Response.Output, _objectToSerialize);
                }
            }
        }

        //action
        public ActionResult Sitemap1()
        {
            var sm = new Sitemap();
            sm.Add(new Location()
            {
                Url = string.Format("http://www.TechnoDesign.ir/Articles/{0}/{1}", 1, "SEO-in-ASP.NET-MVC"),
                LastModified = DateTime.UtcNow,
                Priority = 0.5D
            });
            return new XmlResult(sm);
        }

        
    }
}
