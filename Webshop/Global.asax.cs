using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;

namespace Annytab.Webshop
{
    /// <summary>
    /// This class handles the entire application
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// This method is called when the application starts
        /// </summary>
        protected void Application_Start()
        {
            // Upgrade the database to the latest version
            DatabaseManager.UpgradeDatabase();

            // Get all virtual paths and create a virtual path provider
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new AnnytabPathProvider());
            
            // Register configurations
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Start the background worker to delete expired sessions
            Thread oThread = new Thread(new ThreadStart(SessionAgent.Run));
            oThread.Start();

        } // End of the Application_Start method

        /// <summary>
        /// This method is called when the session starts
        /// </summary>
        protected void Session_Start()
        {
            // Create a new cookie
            HttpCookie aCookie = new HttpCookie("LayoutType");

            // Make sure that the layout type is null
            if (Request.Cookies["LayoutType"] == null)
            {
                // Get the user agent and convert the letters to lower case letters
                string userAgent = Request.UserAgent;
                userAgent = userAgent.ToLower();

                // Redirect the user to the mobile website if one of the conditions below is on
                if (userAgent.Contains("android") || userAgent.Contains("iphone") || userAgent.Contains("windows ce") ||
                    userAgent.Contains("opera mini") || userAgent.Contains("windows phone"))
                {
                    aCookie.Value = "mobile"; // Mobile device
                }
                else
                {
                    aCookie.Value = "standard"; // Standard device
                }

                // Set the expiration and add the cookie
                aCookie.Expires = DateTime.Now.AddDays(1);
                aCookie.HttpOnly = true;
                Response.Cookies.Add(aCookie);
            }

        } // End of the Session_Start method

        /// <summary>
        /// This method handles application errors
        /// </summary>
        protected void Application_Error() 
        {
            // Just return if debugging is enabled
            if (HttpContext.Current.IsDebuggingEnabled == true)
                return;
            
            // Get the last exception
            Exception error = Server.GetLastError();

            // Get the error code
            Int32 code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;

            // Redirect the user based on the error
            if(error is HttpRequestValidationException)
            {
                // Invalid input html or scripts
                Response.Redirect("/home/error/invalid_input");
            }
            else if(code == 404)
            {
                try
                {
                    // Page not found 404
                    Response.StatusCode = 404;
                    Response.Status = "404 Not Found";
                    Response.Write(Tools.GetHttpNotFoundPage());
                }
                catch(Exception ex)
                {
                    string exMessage = ex.Message;
                    Response.Redirect("/home/error/general");
                }   
            }
            else
            {
                // A general error
                Response.Redirect("/home/error/general");
            }

            // Clear all the errors on the server
            Server.ClearError();
             
        } // End of the Application_Error method

    } // End of the class

} // End of the namespace