using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;

namespace WebApplication3.Models
{
    public class SitemapGenerator : ISitemapGenerator
    {
        private static readonly XNamespace xmlns = "http://www.sitemap.org/schemas/sitemap/0.9";
        private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        public XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items)
        {
            var sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(xmlns + "urlset",
                        new XAttribute("xmlns", xmlns),
                        new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                        new XAttribute(xsi + "schemaLocation", "http://www.sitemap.org/schemas/sitemap/0.9 http://wwwsitemap.org/schemas/sitemap/0.9/sitemap.xsd"),
                        from item in items
                        select CreateItemElement(item)
                        ));
            return sitemap;
        }
        public XElement CreateItemElement(ISitemapItem item)
        {
        var itemElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", item.Url.ToLowerInvariant()));
        if (item.LastModified.HasValue)
            itemElement.Add(new XElement(xmlns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));
        if (item.ChangeFrequency.HasValue)
            itemElement.Add(new XElement(xmlns + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));
        if (item.Priority.HasValue)
            itemElement.Add(new XElement(xmlns + "priority", item.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
        return itemElement;
    }
    }
}