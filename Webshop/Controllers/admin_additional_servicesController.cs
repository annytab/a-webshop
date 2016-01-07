using System;
using System.Globalization;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of additional services
    /// </summary>
    [ValidateInput(false)]
    public class admin_additional_servicesController : Controller
    {
        #region View methods

        // Get the list of additional services
        // GET: /admin_additional_services/
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
        // POST: /admin_additional_services/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_additional_services?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_additional_services/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "")
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
            ViewBag.Units = Unit.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.AdditionalService = AdditionalService.GetOneById(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty additional service if the service does not exist
            if (ViewBag.AdditionalService == null)
            {
                // Add data to the view
                ViewBag.AdditionalService = new AdditionalService();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_additional_services/translate/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult translate(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Translator" }) == true)
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.StandardAdditionalService = AdditionalService.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedAdditionalService = AdditionalService.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardAdditionalService != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_additional_services" + returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the additional service
        // POST: /admin_additional_services/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string product_code = collection["txtProductCode"];
            string name = collection["txtName"];
            decimal fee = 0;
            decimal.TryParse(collection["txtFee"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out fee);
            Int32 unit_id = Convert.ToInt32(collection["selectUnit"]);
            bool price_based_on_mount_time = Convert.ToBoolean(collection["cbPriceBasedOnMountTime"]);
            Int32 value_added_tax_id = Convert.ToInt32(collection["selectValueAddedTax"]);
            string account_code = collection["txtAccountCode"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the additional service
            AdditionalService additionalService = AdditionalService.GetOneById(id, adminLanguageId);

            // Check if the additional service exists
            if (additionalService == null)
            {
                // Create a empty additional service
                additionalService = new AdditionalService();
            }

            // Update values
            additionalService.product_code = product_code;
            additionalService.name = name;
            additionalService.fee = fee;
            additionalService.unit_id = unit_id;
            additionalService.price_based_on_mount_time = price_based_on_mount_time;
            additionalService.value_added_tax_id = value_added_tax_id;
            additionalService.account_code = account_code;
            additionalService.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the additional service
            if (additionalService.product_code.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("product_code"), "50") + "<br/>";
            }
            if (additionalService.name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "100") + "<br/>";
            }
            if (additionalService.fee < 0 || additionalService.fee > 9999999999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("fee"), "9 999 999 999.99") + "<br/>";
            }
            if (additionalService.account_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("account_code"), "10") + "<br/>";
            }
            if (additionalService.unit_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("unit").ToLower()) + "<br/>";
            }
            if (additionalService.value_added_tax_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("value_added_tax").ToLower()) + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == "")
            {
                // Check if we should add or update the additional service
                if (additionalService.id == 0)
                {
                    // Add the additional service
                    Int64 insertId = AdditionalService.AddMasterPost(additionalService);
                    additionalService.id = Convert.ToInt32(insertId);
                    AdditionalService.AddLanguagePost(additionalService, adminLanguageId);
                }
                else
                {
                    // Update the additional service
                    AdditionalService.UpdateMasterPost(additionalService);
                    AdditionalService.UpdateLanguagePost(additionalService, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_additional_services" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Units = Unit.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.AdditionalService = additionalService;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the additional service
        // POST: /admin_additional_services/translate
        [HttpPost]
        public ActionResult translate(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            string returnUrl = collection["returnUrl"];
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Translator" }) == true)
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

            // Get the admin default language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get all the form values
            Int32 translationLanguageId = Convert.ToInt32(collection["selectLanguage"]);
            Int32 id = Convert.ToInt32(collection["hiddenAdditionalServiceId"]);
            string name = collection["txtTranslatedName"];
            Int32 value_added_tax_id = Convert.ToInt32(collection["selectValueAddedTax"]);
            string account_code = collection["txtAccountCode"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Create the translated additional service
            AdditionalService translatedAdditionalService = new AdditionalService();
            translatedAdditionalService.id = id;
            translatedAdditionalService.name = name;
            translatedAdditionalService.value_added_tax_id = value_added_tax_id;
            translatedAdditionalService.account_code = account_code;
            translatedAdditionalService.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors
            if (translatedAdditionalService.name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "100") + "<br/>";
            }
            if (translatedAdditionalService.value_added_tax_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("value_added_tax").ToLower()) + "<br/>";
            }
            if (translatedAdditionalService.account_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("account_code"), "10") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved additional service
                AdditionalService additionalService = AdditionalService.GetOneById(id, translationLanguageId);

                if (additionalService == null)
                {
                    // Add a new translated additional service
                    AdditionalService.AddLanguagePost(translatedAdditionalService, translationLanguageId);
                }
                else
                {
                    // Update the translated additional service
                    additionalService.name = translatedAdditionalService.name;
                    additionalService.value_added_tax_id = translatedAdditionalService.value_added_tax_id;
                    additionalService.account_code = translatedAdditionalService.account_code;
                    additionalService.inactive = translatedAdditionalService.inactive;
                    AdditionalService.UpdateLanguagePost(additionalService, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_additional_services" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardAdditionalService = AdditionalService.GetOneById(id, adminLanguageId);
                ViewBag.TranslatedAdditionalService = translatedAdditionalService;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the additional service
        // POST: /admin_additional_services/delete/1?returnUrl=?kw=sok&qp=2
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "")
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

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the full post or just the translation
            if (languageId == 0 || languageId == currentDomain.back_end_language)
            {
                // Delete the additional service and all the connected posts (CASCADE)
                errorCode = AdditionalService.DeleteOnId(id);
            }
            else
            {
                // Delete the additional service language post
                errorCode = AdditionalService.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_additional_services" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace