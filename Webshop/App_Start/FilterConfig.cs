using System.Web.Mvc;

namespace Annytab.Webshop
{
    /// <summary>
    /// This class handles the configuration of filters
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register global filters for the entire application
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AnnytabRequireHttpsAttribute());

        } // End of the RegisterGlobalFilters method

    } // End of the class

} // End of the namespace