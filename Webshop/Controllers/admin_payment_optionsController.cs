using System;
using System.Globalization;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of payment options
    /// </summary>
    [ValidateInput(false)]
    public class admin_payment_optionsController : Controller
    {
        #region View methods

        // Get the list of payment options
        // GET: /admin_payment_options/
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
        // POST: /admin_payment_options/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_payment_options?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_payment_options/edit
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
            ViewBag.PaymentOption = PaymentOption.GetOneById(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;

            // Create new empty payment option if the payment option does not exist
            if (ViewBag.PaymentOption == null)
            {
                // Create a new payment option
                ViewBag.PaymentOption = new PaymentOption();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_payment_options/translate
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
            ViewBag.StandardPaymentOption = PaymentOption.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedPaymentOption = PaymentOption.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardPaymentOption != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_payment_options" + returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the payment option
        // POST: /admin_payment_options/edit
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
            string payment_term_code = collection["txtPaymentTermCode"];
            decimal fee = 0;
            decimal.TryParse(collection["txtFee"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out fee);
            Int32 unit_id = Convert.ToInt32(collection["selectUnit"]);   
            Int32 connection = Convert.ToInt32(collection["selectConnection"]);
            Int32 value_added_tax_id = Convert.ToInt32(collection["selectValueAddedTax"]);
            string account_code = collection["txtAccountCode"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the payment option
            PaymentOption paymentOption = PaymentOption.GetOneById(id, adminLanguageId);

            // Check if the payment option exists
            if (paymentOption == null)
            {
                // Create a empty payment option
                paymentOption = new PaymentOption();
            }

            // Update values
            paymentOption.product_code = product_code;
            paymentOption.name = name;
            paymentOption.payment_term_code = payment_term_code;
            paymentOption.fee = fee;
            paymentOption.unit_id = unit_id;
            paymentOption.connection = connection;
            paymentOption.value_added_tax_id = value_added_tax_id;
            paymentOption.account_code = account_code;
            paymentOption.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the payment option
            if (paymentOption.product_code.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("product_code"), "50") + "<br/>";
            }
            if (paymentOption.name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "100") + "<br/>";
            }
            if (paymentOption.payment_term_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("payment_term_code"), "10") + "<br/>";
            }
            if (paymentOption.fee < 0 || paymentOption.fee > 9999999999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("fee"), "9 999 999 999.99") + "<br/>";
            }
            if (paymentOption.account_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("account_code"), "10") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == "")
            {
                // Check if we should add or update the payment option
                if (paymentOption.id == 0)
                {
                    // Add the payment option
                    Int64 insertId = PaymentOption.AddMasterPost(paymentOption);
                    paymentOption.id = Convert.ToInt32(insertId);
                    PaymentOption.AddLanguagePost(paymentOption, adminLanguageId);
                }
                else
                {
                    // Update the payment option
                    PaymentOption.UpdateMasterPost(paymentOption);
                    PaymentOption.UpdateLanguagePost(paymentOption, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_payment_options" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Units = Unit.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.PaymentOption = paymentOption;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the payment option
        // POST: /admin_payment_options/translate
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
            Int32 id = Convert.ToInt32(collection["hiddenPaymentOptionId"]);
            string name = collection["txtTranslatedName"];
            Int32 value_added_tax_id = Convert.ToInt32(collection["selectValueAddedTax"]);
            string account_code = collection["txtAccountCode"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Create the translated payment option
            PaymentOption translatedPaymentOption = new PaymentOption();
            translatedPaymentOption.id = id;
            translatedPaymentOption.name = name;
            translatedPaymentOption.value_added_tax_id = value_added_tax_id;
            translatedPaymentOption.account_code = account_code;
            translatedPaymentOption.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Check the translated country name
            if (translatedPaymentOption.name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "100") + "<br/>";
            }
            if (translatedPaymentOption.account_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("account_code"), "10") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved payment option
                PaymentOption paymentOption = PaymentOption.GetOneById(id, translationLanguageId);

                if (paymentOption == null)
                {
                    // Add a new translated payment option
                    PaymentOption.AddLanguagePost(translatedPaymentOption, translationLanguageId);
                }
                else
                {
                    // Update the translated payment option
                    paymentOption.name = translatedPaymentOption.name;
                    paymentOption.value_added_tax_id = translatedPaymentOption.value_added_tax_id;
                    paymentOption.account_code = translatedPaymentOption.account_code;
                    paymentOption.inactive = translatedPaymentOption.inactive;
                    PaymentOption.UpdateLanguagePost(paymentOption, translationLanguageId);

                }

                // Redirect the user to the list
                return Redirect("/admin_payment_options" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "id", "ASC");
                ViewBag.StandardPaymentOption = PaymentOption.GetOneById(id, adminLanguageId);
                ViewBag.TranslatedPaymentOption = translatedPaymentOption;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the payment option
        // POST: /admin_payment_options/delete
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
                // Delete the payment option and all the connected posts (CASCADE)
                errorCode = PaymentOption.DeleteOnId(id);
            }
            else
            {
                // Delete the payment option language post
                errorCode = PaymentOption.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_payment_options" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace