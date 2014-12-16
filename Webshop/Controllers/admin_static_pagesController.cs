using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of static pages
    /// </summary>
    [ValidateInput(false)] 
    public class admin_static_pagesController : Controller
    {
        #region View methods

        // Get the list of static pages
        // GET: /admin_static_pages/
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
        // POST: /admin_static_pages/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_static_pages?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_static_pages/edit
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
            ViewBag.StaticPage = StaticPage.GetOneById(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;

            // Create new empty page if the page does not exist
            if (ViewBag.StaticPage == null)
            {
                // Add data to the view
                ViewBag.StaticPage = new StaticPage();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_static_pages/translate
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
            ViewBag.StandardStaticPage = StaticPage.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedStaticPage = StaticPage.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardStaticPage != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_static_pages" + returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the static page
        // POST: /admin_static_pages/edit
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
            string title = collection["txtTitle"];
            string linkname = collection["txtLinkname"];
            string description = collection["txtDescription"];
            string metaDescription = collection["txtMetaDescription"];
            string metaKeywords = collection["txtMetaKeywords"];
            string pageName = collection["txtPageName"];
            string metaRobots = collection["selectMetaRobots"];
            byte connectionId = Convert.ToByte(collection["selectConnectionId"]);
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the static page
            StaticPage staticPage = StaticPage.GetOneById(id, adminLanguageId);

            // Check if the static page exists
            if (staticPage == null)
            {
                // Create an empty static page
                staticPage = new StaticPage();
            }

            // Update values
            staticPage.title = title;
            staticPage.link_name = linkname;
            staticPage.main_content = description;
            staticPage.meta_description = metaDescription;
            staticPage.meta_keywords = metaKeywords;
            staticPage.page_name = pageName;
            staticPage.meta_robots = metaRobots;
            staticPage.connected_to_page = connectionId;
            staticPage.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a static page on page name
            StaticPage pageOnPageName = StaticPage.GetOneByPageName(staticPage.page_name, adminLanguageId);

            // Check for errors
            if (pageOnPageName != null && staticPage.id != pageOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (staticPage.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(staticPage.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (staticPage.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (staticPage.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (staticPage.link_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("link_name"), "100") + "<br/>";
            }
            if (staticPage.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (staticPage.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the static page
                if (staticPage.id == 0)
                {
                    // Add the static page
                    Int64 insertId = StaticPage.AddMasterPost(staticPage);
                    staticPage.id = Convert.ToInt32(insertId);
                    StaticPage.AddLanguagePost(staticPage, adminLanguageId);
                }
                else
                {
                    // Update the static page
                    StaticPage.UpdateMasterPost(staticPage);
                    StaticPage.UpdateLanguagePost(staticPage, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_static_pages" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.StaticPage = staticPage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the static page
        // POST: /admin_static_pages/translate
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
            Int32 id = Convert.ToInt32(collection["hiddenStaticPageId"]);
            string title = collection["txtTranslatedTitle"];
            string linkname = collection["txtTranslatedLinkname"];
            string description = collection["txtTranslatedDescription"];
            string metadescription = collection["txtTranslatedMetadescription"];
            string metakeywords = collection["txtTranslatedMetakeywords"];
            string pagename = collection["txtTranslatedPagename"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Create the translated static page
            StaticPage translatedStaticPage = new StaticPage();
            translatedStaticPage.id = id;
            translatedStaticPage.title = title;
            translatedStaticPage.link_name = linkname;
            translatedStaticPage.main_content = description;
            translatedStaticPage.meta_description = metadescription;
            translatedStaticPage.meta_keywords = metakeywords;
            translatedStaticPage.page_name = pagename;
            translatedStaticPage.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a static page on page name
            StaticPage pageOnPageName = StaticPage.GetOneByPageName(translatedStaticPage.page_name, adminLanguageId);

            // Check the page name
            if (pageOnPageName != null && translatedStaticPage.id != pageOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedStaticPage.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(translatedStaticPage.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedStaticPage.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (translatedStaticPage.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (translatedStaticPage.link_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("link_name"), "100") + "<br/>";
            }
            if (translatedStaticPage.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (translatedStaticPage.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved static page
                StaticPage staticPage = StaticPage.GetOneById(id, translationLanguageId);

                if (staticPage == null)
                {
                    // Add a new translated static page
                    StaticPage.AddLanguagePost(translatedStaticPage, translationLanguageId);
                }
                else
                {
                    // Update values for the saved static page
                    staticPage.title = translatedStaticPage.title;
                    staticPage.link_name = translatedStaticPage.link_name;
                    staticPage.main_content = translatedStaticPage.main_content;
                    staticPage.meta_description = translatedStaticPage.meta_description;
                    staticPage.meta_keywords = translatedStaticPage.meta_keywords;
                    staticPage.page_name = translatedStaticPage.page_name;
                    staticPage.inactive = translatedStaticPage.inactive;

                    // Update the static page translation
                    StaticPage.UpdateLanguagePost(staticPage, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_static_pages" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardStaticPage = StaticPage.GetOneById(id, adminLanguageId);
                ViewBag.TranslatedStaticPage = translatedStaticPage;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the static page
        // POST: /admin_static_pages/delete
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
                // Delete the static page and all the connected posts (CASCADE)
                errorCode = StaticPage.DeleteOnId(id);
            }
            else
            {
                // Delete the translated static page post
                errorCode = StaticPage.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_static_pages" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace