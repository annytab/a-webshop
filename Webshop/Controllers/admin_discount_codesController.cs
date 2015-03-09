using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of discount codes
    /// </summary>
    [ValidateInput(false)] 
    public class admin_discount_codesController : Controller
    {
        #region View methods

        // Get a list of discount codes
        // GET: /admin_discount_codes/
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
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                ViewBag.AdminSession = true;
                return View();
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Search in the list
        // POST: /admin_discount_codes/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_discount_codes?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_discount_codes/edit/SXXXSWEEE?returnUrl=?kw=df&so=ASC
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

            // Add data to the view
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.DiscountCode = DiscountCode.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty discount code post if the discount code does not exist
            if (ViewBag.DiscountCode == null)
            {
                // Add data to the view
                ViewBag.DiscountCode = new DiscountCode();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        #endregion

        #region Post methods

        // Update the discount code
        // POST: /admin_discount_codes/edit
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
            string id = collection["txtId"];
            Int32 language_id = Convert.ToInt32(collection["selectLanguage"]);
            string currency_code = collection["selectCurrency"];
            decimal discount_value = 0;
            decimal.TryParse(collection["txtDiscountValue"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out discount_value);
            bool free_freight = Convert.ToBoolean(collection["cbFreeFreight"]);
            bool once_per_customer = Convert.ToBoolean(collection["cbOncePerCustomer"]);
            bool exclude_products_on_sale = Convert.ToBoolean(collection["cbExcludeProductsOnSale"]);
            DateTime end_date = Convert.ToDateTime(collection["txtEndDate"]);
            decimal minimum_order_value = 0;
            decimal.TryParse(collection["txtMinimumOrderValue"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out minimum_order_value);

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Get the discount code
            DiscountCode discountCode = DiscountCode.GetOneById(id);
            bool postExists = true;

            // Check if the discount code exists
            if (discountCode == null)
            {
                // Create an empty discount code
                discountCode = new DiscountCode();
                postExists = false;
            }

            // Update values
            discountCode.id = id;
            discountCode.language_id = language_id;
            discountCode.currency_code = currency_code;
            discountCode.discount_value = discount_value;
            discountCode.free_freight = free_freight;
            discountCode.once_per_customer = once_per_customer;
            discountCode.exclude_products_on_sale = exclude_products_on_sale;
            discountCode.end_date = AnnytabDataValidation.TruncateDateTime(end_date);
            discountCode.minimum_order_value = minimum_order_value;

            // Create a error message
            string errorMessage = string.Empty;

            if (discountCode.language_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("language").ToLower()) + "<br/>";
            }
            if (discountCode.currency_code == "")
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("currency").ToLower()) + "<br/>";
            }
            if (discountCode.id.Length == 0 || discountCode.id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_certain_length"), tt.Get("id"), "1", "50") + "<br/>";
            }
            if (discountCode.discount_value < 0 || discountCode.discount_value > 9.999M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("discount"), "9.999") + "<br/>";
            }
            if (discountCode.minimum_order_value < 0 || discountCode.minimum_order_value > 999999999999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("minimum_order_value"), "999 999 999 999.99") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the discount code
                if (postExists == false)
                {
                    // Add the discount code
                    DiscountCode.Add(discountCode);
                }
                else
                {
                    // Update the discount code
                    DiscountCode.Update(discountCode);
                }

                // Redirect the user to the list
                return Redirect("/admin_discount_codes" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "id", "ASC");
                ViewBag.DiscountCode = discountCode;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the discount code
        // POST: /admin_discount_codes/delete/XXXSSSWWSS?returnUrl=?kw=sok&qp=2
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

            // Delete the discount code post and all the connected posts (CASCADE)
            errorCode = DiscountCode.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_discount_codes" + returnUrl);

        } // End of the delete method

        #endregion

    } // End of the class

} // End of the namespace