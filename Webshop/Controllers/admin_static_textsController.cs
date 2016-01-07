using System;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of static texts
    /// </summary>
    [ValidateInput(false)]
    public class admin_static_textsController : Controller
    {
        #region View methods

        // Get the list of static texts
        // GET: /admin_static_texts/
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
        // POST: /admin_static_texts/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_static_texts?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_static_texts/edit/1?returnUrl=?kw=sok&qp=2
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
            ViewBag.StaticText = StaticText.GetOneById(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;

            // Create new empty static text if the static text does not exist
            if (ViewBag.StaticText == null)
            {
                // Add data to the view
                ViewBag.StaticText = new StaticText();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_static_texts/translate/1?returnUrl=?kw=sok&qp=2
        [HttpGet]
        public ActionResult translate(string id = "", string returnUrl = "")
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
            ViewBag.StandardStaticText = StaticText.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedStaticText = StaticText.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardStaticText != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_static_texts" + returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the static text
        // POST: /admin_static_texts/edit
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get all the form values
            string staticTextId = collection["txtId"];
            string staticTextText = collection["txtText"];

            // Get the static text
            StaticText staticText = StaticText.GetOneById(staticTextId, adminLanguageId);
            bool postExists = true;

            // Check if the static text exists
            if (staticText == null)
            {
                // Create an empty static text
                staticText = new StaticText();
                staticText.id = staticTextId;
                postExists = false;
            }

            // Update values
            staticText.value = staticTextText;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the static text
            if (AnnytabDataValidation.CheckPageNameCharacters(staticText.id) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("id")) + "<br/>";
            }
            if (staticText.id.Length == 0 || staticText.id.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_certain_length"), tt.Get("id"), "1", "100") + "<br/>";
            }
            if (staticText.value.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("text"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the static text
                if (postExists == false)
                {
                    // Add the static text
                    StaticText.Add(staticText, adminLanguageId);
                }
                else
                {
                    // Update the static text
                    StaticText.Update(staticText, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_static_texts" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.StaticText = staticText;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the static text
        // POST: /admin_static_texts/translate
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
            string staticTextId = collection["hiddenStaticTextId"];
            string staticTextText = collection["txtTranslatedText"];

            // Create the translated static text
            StaticText translatedStaticText = new StaticText();
            translatedStaticText.id = staticTextId;
            translatedStaticText.value = staticTextText;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the static text
            if (translatedStaticText.value.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("text"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the static text
                StaticText staticText = StaticText.GetOneById(staticTextId, translationLanguageId);

                if (staticText == null)
                {
                    // Add a new translated static text
                    StaticText.Add(translatedStaticText, translationLanguageId);
                }
                else
                {
                    // Update the translated static text
                    staticText.value = translatedStaticText.value;
                    StaticText.Update(staticText, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_static_texts" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardStaticText = StaticText.GetOneById(staticTextId, adminLanguageId);
                ViewBag.TranslatedStaticText = translatedStaticText;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the static text
        // POST: /admin_static_texts/delete/1?returnUrl=?kw=sok&qp=2
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
                // Delete the static text and all the connected posts (CASCADE)
                errorCode = StaticText.DeleteOnId(id);
            }
            else
            {
                // Delete the translated post
                errorCode = StaticText.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_static_texts" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace