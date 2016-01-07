using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Text;
using System.IO;
using System.IO.Compression;

/// <summary>
/// This class is used to handle sitemaps
/// </summary>
public static class SitemapManager
{

    /// <summary>
    /// Create the sitemap for a domain
    /// </summary>
    /// <param name="domain">A reference to the domain</param>
    /// <param name="priorityCategories">The priority for categories</param>
    /// <param name="priorityProducts">The priority for products</param>
    /// <param name="changeFrequency">The change frequency</param>
    public static void CreateSitemap(Domain domain, string priorityCategories, string priorityProducts, string changeFrequency)
    {

        // Create the directory path
        string directoryPath = HttpContext.Current.Server.MapPath("/Content/domains/" + domain.id.ToString() + "/sitemaps/");

        // Check if the directory exists
        if (System.IO.Directory.Exists(directoryPath) == false)
        {
            // Create the directory
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        // Create the file
        string filepath = directoryPath + "Sitemap.xml.gz";

        // Get categories, products and static pages
        List<Category> categories = Category.GetAll(domain.front_end_language, "title", "ASC");
        List<Product> products = Product.GetAll(domain.front_end_language, "title", "ASC");

        // Create variables
        GZipStream gzipStream = null;
        XmlTextWriter xmlTextWriter = null;

        try
        {
            // Create a gzip stream
            gzipStream = new GZipStream(new FileStream(filepath, FileMode.Create), CompressionMode.Compress);

            // Create a xml text writer
            xmlTextWriter = new XmlTextWriter(gzipStream, new UTF8Encoding(true));

            // Write the start of the document
            xmlTextWriter.WriteStartDocument();

            // Write the url set for the xml document <urlset>
            xmlTextWriter.WriteStartElement("urlset");
            xmlTextWriter.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlTextWriter.WriteAttributeString("xmlns:image", "http://www.google.com/schemas/sitemap-image/1.1");
            xmlTextWriter.WriteAttributeString("xmlns:video", "http://www.google.com/schemas/sitemap-video/1.1");

            // Create the start string
            string baseUrl = domain.web_address;

            // Add the baseurl
            CreateUrlPost(xmlTextWriter, baseUrl, "1.0", changeFrequency, DateTime.UtcNow);

            // Loop categories
            for (int i = 0; i < categories.Count; i++)
            {
                // Create the url post
                CreateUrlPost(xmlTextWriter, baseUrl + "/home/category/" + categories[i].page_name, priorityCategories, changeFrequency, DateTime.UtcNow);
            }

            // Loop products
            for (int i = 0; i < products.Count; i++)
            {
                // Create the url post
                CreateUrlPost(xmlTextWriter, baseUrl + "/home/product/" + products[i].page_name, priorityProducts, changeFrequency, DateTime.UtcNow);
            }

            // Write the end tag for the xml document </urlset>
            xmlTextWriter.WriteEndDocument();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            // Close streams
            if (xmlTextWriter != null)
            {
                // Close the XmlTextWriter
                xmlTextWriter.Close();
            }
            if (gzipStream != null)
            {
                // Close the gzip stream
                gzipStream.Close();
            }
        }

    } // End of the CreateSitemap method

    /// <summary>
    /// Create a url post
    /// </summary>
    /// <param name="xmlTextWriter">A reference to a XmlTextWriter</param>
    /// <param name="url">The url as a string</param>
    /// <param name="priority">The priority as a string</param>
    /// <param name="changeFrequency">The change frequency</param>
    /// <param name="lastModifiedDate">The date for the last modification</param>
    private static void CreateUrlPost(XmlTextWriter xmlTextWriter, string url, string priority, string changeFrequency, DateTime lastModifiedDate)
    {
        xmlTextWriter.WriteStartElement("url");
        xmlTextWriter.WriteStartElement("loc");
        xmlTextWriter.WriteString(url);
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("lastmod");
        xmlTextWriter.WriteString(string.Format("{0:yyyy-MM-dd}", lastModifiedDate));
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("changefreq");
        xmlTextWriter.WriteString(changeFrequency);
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("priority");
        xmlTextWriter.WriteString(priority);
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();

    } // End of the CreateUrlPost method

} // End of the class