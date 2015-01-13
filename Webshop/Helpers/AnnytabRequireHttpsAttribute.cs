using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

/// <summary>
/// This class will make sure that a request is from https
/// </summary>
public class AnnytabRequireHttpsAttribute : RequireHttpsAttribute
{
    /// <summary>
    /// Check if the request is a http request
    /// </summary>
    /// <param name="filterContext">A reference to the context</param>
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        // Check if the request is secure
        if (filterContext.HttpContext.Request.IsSecureConnection == false)
        {
            HandleNonHttpsRequest(filterContext);
        }
        else
        {
            HandleHttpsRequest(filterContext);
        }

    } // End of the OnAuthorization method

    /// <summary>
    /// Handle a non https request
    /// </summary>
    /// <param name="filterContext"></param>
    protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
    {
        // Get the current domain
        Domain domain = Tools.GetCurrentDomain();

        // Get the host
        string host = filterContext.HttpContext.Request.Url.Host;

        // Get website settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();
        string redirectHttps = webshopSettings.Get("REDIRECT-HTTPS");

        if (redirectHttps.ToLower() == "true")
        {
            // Modify the url
            UriBuilder uriBuilder = new UriBuilder(filterContext.HttpContext.Request.Url);
            uriBuilder.Scheme = "https";
            uriBuilder.Host = domain.web_address.Contains("www.") == true && uriBuilder.Host.Contains("www.") == false ? "www." + uriBuilder.Host : uriBuilder.Host;
            uriBuilder.Port = 443;

            // Redirect to the https add
            filterContext.HttpContext.Response.StatusCode = 301;
            filterContext.HttpContext.Response.Redirect(uriBuilder.Uri.AbsoluteUri);
        }
        else if (domain.web_address.Contains("www.") == true && host.Contains("www.") == false)
        {
            // Modify the url
            UriBuilder uriBuilder = new UriBuilder(filterContext.HttpContext.Request.Url);
            uriBuilder.Host = domain.web_address.Contains("www.") == true && uriBuilder.Host.Contains("www.") == false ? "www." + uriBuilder.Host : uriBuilder.Host;

            // Redirect to the https add
            filterContext.HttpContext.Response.StatusCode = 301;
            filterContext.HttpContext.Response.Redirect(uriBuilder.Uri.AbsoluteUri);
        }

    } // End of the HandleNonHttpsRequest method

    /// <summary>
    /// Handle a https request
    /// </summary>
    /// <param name="filterContext">A reference to the context</param>
    protected void HandleHttpsRequest(AuthorizationContext filterContext)
    {
        // Get the current domain
        Domain domain = Tools.GetCurrentDomain();

        // Get the host
        string host = filterContext.HttpContext.Request.Url.Host;

        // Check if we should do a www redirect
        if (domain.web_address.Contains("www.") == true && host.Contains("www.") == false)
        {
            // Modify the url
            UriBuilder uriBuilder = new UriBuilder(filterContext.HttpContext.Request.Url);
            uriBuilder.Host = domain.web_address.Contains("www.") == true && uriBuilder.Host.Contains("www.") == false ? "www." + uriBuilder.Host : uriBuilder.Host;

            // Redirect to the https add
            filterContext.HttpContext.Response.StatusCode = 301;
            filterContext.HttpContext.Response.Redirect(uriBuilder.Uri.AbsoluteUri);
        }

    } // End of the HandleHttpsRequest method

} // End of the class