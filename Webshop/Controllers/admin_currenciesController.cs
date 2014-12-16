using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of currencies
    /// </summary>
    [ValidateInput(false)]
    public class admin_currenciesController : Controller
    {
        #region View methods

        // Get the list of currencies
        // GET: /admin_currencies/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(Request);

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

        // Search in the list
        // POST: /admin_currencies/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_currencies?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_currencies/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(string id = "", string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

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
                return View("index");
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
            ViewBag.Currency = Currency.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty currency post if the currency post does not exist
            if (ViewBag.Currency == null)
            {
                // Add data to the view
                ViewBag.Currency = new Currency();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        #endregion

        #region Post methods

        // Update the currency
        // POST: /admin_currencies/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get the return url
            string returnUrl = collection["returnUrl"];
            ViewBag.QueryParams = new QueryParams(returnUrl);

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
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get all the form values
            string currency_code = collection["txtCurrencyCode"];
            decimal conversion_rate = 0;
            decimal.TryParse(collection["txtConversionRate"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out conversion_rate);
            Int16 currency_base = 0;
            Int16.TryParse(collection["txtCurrencyBase"].Replace(",", "."), out currency_base);
            byte decimals = 0;
            byte.TryParse(collection["txtDecimals"].Replace(",", "."), out decimals);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the currency
            Currency currency = Currency.GetOneById(currency_code);
            bool postExists = true;

            // Check if the static text exists
            if (currency == null)
            {
                // Create an empty currency
                currency = new Currency();
                postExists = false;
            }

            // Update values
            currency.currency_code = currency_code;
            currency.conversion_rate = conversion_rate;
            currency.currency_base = currency_base;
            currency.decimals = decimals;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the currency
            if (currency.currency_code.Length != 3)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_certain_length"), tt.Get("currency_code"), "3", "3") + "<br/>";
            }
            if (currency.conversion_rate < 0 || currency.conversion_rate > 9999.999999M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("conversion_rate"), "9 999.999999") + "<br/>";
            }
            if (currency.decimals < 0 || currency.decimals > 2)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("number_of_decimals"), "3") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the static text
                if (postExists == false)
                {
                    // Add the currency
                    Currency.Add(currency);
                }
                else
                {
                    // Update the currency
                    Currency.Update(currency);
                }

                // Redirect the user to the list
                return Redirect("/admin_currencies" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Currency = currency;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the currency
        // POST: /admin_currencies/delete/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult delete(string id = "", string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the currency post and all the connected posts (CASCADE)
            errorCode = Currency.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_currencies" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace