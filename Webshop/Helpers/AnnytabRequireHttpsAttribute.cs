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

    } // End of the OnAuthorization method

    /// <summary>
    /// Handle a non https request
    /// </summary>
    /// <param name="filterContext"></param>
    protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
    {
        // Modify the url
        UriBuilder uriBuilder = new UriBuilder(filterContext.HttpContext.Request.Url);
        uriBuilder.Scheme = "https";
        uriBuilder.Port = 443;

        // Redirect to the https add
        filterContext.HttpContext.Response.StatusCode = 301;
        filterContext.HttpContext.Response.Redirect(uriBuilder.Uri.AbsoluteUri);

    } // End of the HandleNonHttpsRequest method

} // End of the class