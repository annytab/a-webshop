using System;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of units
    /// </summary>
    [ValidateInput(false)]
    public class admin_unitsController : Controller
    {
        #region View methods

        // Get the list of units
        // GET: /admin_units/
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
        // POST: /admin_units/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_units?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_units/edit
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
            ViewBag.Unit = Unit.GetOneById(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;
            
            // Create new empty unit if the unit does not exist
            if (ViewBag.Unit == null)
            {
                // Add data to the view
                ViewBag.Unit = new Unit();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_units/translate
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
            ViewBag.StandardUnit = Unit.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedUnit = Unit.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardUnit != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_units" + returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the unit
        // POST: /admin_units/edit
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
            string unit_code = collection["txtUnitCode"];
            string name = collection["txtName"];
            string unit_code_si = collection["txtUnitCodeSi"];
            string erp_code = collection["txtErpCode"];

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the unit
            Unit unit = Unit.GetOneById(id, adminLanguageId);

            // Check if the unit exists
            if (unit == null)
            {
                // Create a empty unit
                unit = new Unit();
            }

            // Update values
            unit.unit_code_si = unit_code_si;
            unit.unit_code = unit_code;
            unit.name = name;
            unit.erp_code = erp_code;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the unit
            if (unit.unit_code_si.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("unit_code_si"), "10") + "<br/>";
            }
            if (unit.unit_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("unit_code"), "10") + "<br/>";
            }
            if (unit.name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }
            if (unit.erp_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("erp_code"), "10") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the unit
                if (unit.id == 0)
                {
                    // Add the unit
                    Int64 insertId = Unit.AddMasterPost(unit);
                    unit.id = Convert.ToInt32(insertId);
                    Unit.AddLanguagePost(unit, adminLanguageId);
                }
                else
                {
                    // Update the unit
                    Unit.UpdateMasterPost(unit);
                    Unit.UpdateLanguagePost(unit, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_units" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Unit = unit;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the unit
        // POST: /admin_units/translate
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
            Int32 id = Convert.ToInt32(collection["hiddenUnitId"]);
            string unit_code = collection["txtTranslatedUnitCode"];
            string name = collection["txtTranslatedName"];

            // Create the translated unit
            Unit translatedUnit = new Unit();
            translatedUnit.id = id;
            translatedUnit.unit_code = unit_code;
            translatedUnit.name = name;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors
            if (translatedUnit.unit_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("unit_code"), "10") + "<br/>";
            }
            if (translatedUnit.name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }
            
            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved unit
                Unit unit = Unit.GetOneById(id, translationLanguageId);

                if (unit == null)
                {
                    // Add a new translated unit
                    Unit.AddLanguagePost(translatedUnit, translationLanguageId);
                }
                else
                {
                    // Update the translated unit
                    unit.unit_code = translatedUnit.unit_code;
                    unit.name = translatedUnit.name;
                    Unit.UpdateLanguagePost(unit, translationLanguageId);

                }

                // Redirect the user to the list
                return Redirect("/admin_units" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardUnit = Unit.GetOneById(id, adminLanguageId);
                ViewBag.TranslatedUnit = translatedUnit;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the unit
        // POST: /admin_units/delete
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
                // Delete the unit and all the connected posts (CASCADE)
                errorCode = Unit.DeleteOnId(id);
            }
            else
            {
                // Delete the unit post
                errorCode = Unit.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if(errorCode != 0)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_units" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace