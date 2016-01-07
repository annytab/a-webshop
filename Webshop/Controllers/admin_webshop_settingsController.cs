using System;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls settings for the web shop
    /// </summary>
    [ValidateInput(false)]
    public class admin_webshop_settingsController : Controller
    {
        #region View methods

        // Get the webshop settings
        // GET: /admin_webshop_settings/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(Request);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("~/Views/admin_default/index.cshtml");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the view
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.WebshopSettings = WebshopSetting.GetAllFromCache();

            // Return the view
            return View();

        } // End of the index method

        #endregion

        #region Post methods

        // Update webshop settings
        // POST: /admin_webshop_settings/index
        [HttpPost]
        public ActionResult index(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(Request);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("~/Views/admin_default/index.cshtml");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Update all the webshop settings
            foreach(string key in collection.Keys)
            {
                // Get the value
                string value = collection[key];
                value = value.Length > 100 ? value.Substring(0, 100) : value;

                // Update the value for the key
                WebshopSetting.Update(key, collection[key]);
            }

            // Return the default view
            return RedirectToAction("index", "admin_default");

        } // End of the index method

        #endregion

    } // End of the class

} // End of the namespace