using System.Web.Mvc;
using System.Web.Routing;

namespace Annytab.Webshop
{
    /// <summary>
    /// This class handles the routes for the application, user friendly urls
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register routes
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(css|js|cshtml)(/.*)?" });

            // The default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional }
            );

        } // End of the RegisterRoutes method

    } // End of the class

} // End of the namespace