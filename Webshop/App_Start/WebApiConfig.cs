using System.Web.Http;

namespace Annytab.Webshop
{
    /// <summary>
    /// This class handles the routes for the web api
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register routes for the web api
        /// </summary>
        public static void Register(HttpConfiguration config)
        {
            // The default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        } // End of the Register method

    } // End of the class

} // End of the namespace
