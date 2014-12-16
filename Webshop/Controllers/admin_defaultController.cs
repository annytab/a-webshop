using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class represent the default page for the admin page
    /// </summary>
    [ValidateInput(false)]
    public class admin_defaultController : Controller
    {
        #region View methods

        // Get the default page
        // GET: /admin_default/
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View();
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Get the order sale data page
        // GET: /admin_default/order_sale_data
        public ActionResult order_sale_data()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Set form values
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Return the view
            return View();

        } // End of the order_sale_data method

        // Get the product sale data page
        // GET: /admin_default/product_sale_data
        public ActionResult product_sale_data()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;          
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Set form values
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Return the view
            return View();

        } // End of the product_sale_data method

        #endregion

        #region Post methods

        // Filter the order statistics report
        // POST: /admin_default/filter_report
        [HttpPost]
        public ActionResult filter_report(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get all the form values
            string countryCode = collection["selectCountry"];
            Int32 groupBy = Convert.ToInt32(collection["selectGroupBy"]);
            Int32 pageSize = Convert.ToInt32(collection["selectPageSize"]);

            // Redirect the user to the index page
            return RedirectToAction("index", new { cc = countryCode, gb = groupBy, pz = pageSize });

        } // End of the edit method

        #endregion

    } // End of the class

} // End of the namespace
