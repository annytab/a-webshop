using System;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of countries
    /// </summary>
    [ValidateInput(false)]
    public class admin_countriesController : Controller
    {
        #region View methods

        // Get the list of countries
        // GET: /admin_countries/
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
        // POST: /admin_countries/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_countries?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_countries/edit/1?returnUrl=?kw=df&so=ASC
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
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.Country = Country.GetOneById(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;

            // Create new empty county if the country does not exist
            if (ViewBag.Country == null)
            {
                // Add data to the view
                ViewBag.Country = new Country();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_countries/translate/1?returnUrl=?kw=df&so=ASC
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
            ViewBag.StandardCountry = Country.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedCountry = Country.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardCountry != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_countries" + returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the country
        // POST: /admin_countries/edit
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
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string name = collection["txtName"];
            string countryCode = collection["txtCountryCode"];
            byte vatCode = Convert.ToByte(collection["selectVatCode"]);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the country
            Country country = Country.GetOneById(id, adminLanguageId);

            // Check if the country exists (haha)
            if (country == null)
            {
                // Create a empty country
                country = new Country();
            }
            
            // Update values
            country.name = name;
            country.country_code = countryCode;
            country.vat_code = vatCode;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the country
            if (country.name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }
            if (country.country_code.Length > 2)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("country_code"), "2") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the country
                if (country.id == 0)
                {
                    // Add the country
                    Int64 insertId = Country.AddMasterPost(country);
                    country.id = Convert.ToInt32(insertId);
                    Country.AddLanguagePost(country, adminLanguageId);
                }
                else
                {
                    // Update the country
                    Country.UpdateMasterPost(country);
                    Country.UpdateLanguagePost(country, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_countries" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Country = country;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the country
        // POST: /admin_countries/translate
        [HttpPost]
        public ActionResult translate(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get the return url
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
            Int32 id = Convert.ToInt32(collection["hiddenCountryId"]);
            string name = collection["txtTranslatedName"];

            // Create the translated Country
            Country translatedCountry = new Country();
            translatedCountry.id = id;
            translatedCountry.name = name;

            // Create a error message
            string errorMessage = string.Empty;

            // Check the translated country name
            if (translatedCountry.name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved country
                Country country = Country.GetOneById(id, translationLanguageId);

                if (country == null)
                {
                    // Add a new translated country
                    Country.AddLanguagePost(translatedCountry, translationLanguageId);
                }
                else
                {
                    // Update the translated country
                    country.name = translatedCountry.name;
                    Country.UpdateLanguagePost(country, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_countries" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardCountry = Country.GetOneById(id, adminLanguageId);
                ViewBag.TranslatedCountry = translatedCountry;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the country
        // POST: /admin_countries/delete/1?returnUrl=?kw=df&so=ASC
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
                // Delete the country and all the connected posts (CASCADE)
                errorCode = Country.DeleteOnId(id);
            }
            else
            {
                // Delete the country post
                errorCode = Country.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_countries" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace