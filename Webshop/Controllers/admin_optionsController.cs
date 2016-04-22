using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class handles administrator methods for products
    /// </summary>
    [ValidateInput(false)] 
    public class admin_optionsController : Controller
    {

        #region View methods

        // Get the list of options
        // GET: /admin_options/
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
        // POST: /admin_options/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];
  
            // Return the url with search parameters
            return Redirect("~/admin_options?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_options/edit
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[]{"Administrator", "Editor"}) == true)
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
            ViewBag.OptionType = OptionType.GetOneById(id, adminLanguageId);
            ViewBag.Options = Option.GetByOptionTypeId(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;

            // Create new empty options if the option type does not exist
            if (ViewBag.OptionType == null)
            {
                // Add data to the view
                ViewBag.OptionType = new OptionType();
                ViewBag.Options = new List<Option>();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_options/translate
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
            ViewBag.StandardOptionType = OptionType.GetOneById(id, adminLanguageId);
            ViewBag.StandardOptions = Option.GetByOptionTypeId(id, adminLanguageId);
            ViewBag.TranslatedOptionType = OptionType.GetOneById(id, languageId);
            ViewBag.TranslatedOptions = Option.GetByOptionTypeId(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardOptionType != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_options" + returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the option
        // POST: /admin_options/edit
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
            Int32 optionTypeId = Convert.ToInt32(collection["txtId"]);
            string optionTypeTitle = collection["txtTitle"];
            string google_name = collection["selectGoogleName"];
            string[] optionIds = collection.GetValues("optionId");
            string[] optionTitles = collection.GetValues("optionTitle");
            string[] optionSuffix = collection.GetValues("optionSuffix");

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the option type
            OptionType optionType = OptionType.GetOneById(optionTypeId, adminLanguageId);

            // Check if the option type exists
            if (optionType == null)
            {
                // Create the option type
                optionType = new OptionType();
            }

            // Update values
            optionType.title = optionTypeTitle;
            optionType.google_name = google_name;

            // Count the options
            Int32 optionCount = optionIds != null ? optionIds.Length : 0;

            // Create the list of options
            List<Option> options = new List<Option>(optionCount);

            // Add all options to the list
            for (int i = 0; i < optionCount; i++)
            {
                if (optionTitles[i] != string.Empty)
                {
                    // Create a option
                    Option option = new Option();
                    option.id = Convert.ToInt32(optionIds[i]);
                    option.title = optionTitles[i];
                    option.product_code_suffix = optionSuffix[i];
                    option.sort_order = Convert.ToInt16(i);
                    option.option_type_id = optionType.id;

                    // Add the option to the list
                    options.Add(option);
                }
            }

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the option type
            if (optionType.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "100") + "<br/>";
            }
            if (optionType.google_name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("google_name"), "50") + "<br/>";
            }

            // Check for errors in options
            foreach (Option option in options)
            {
                if (option.title.Length > 50)
                {
                    errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), option.title, "50") + "<br/>";
                }
                if (option.product_code_suffix.Length > 10)
                {
                    errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), option.product_code_suffix, "10") + "<br/>";
                }
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the option
                if (optionType.id == 0)
                {
                    // Add the option
                    AddOption(optionType, options, adminLanguageId);
                }
                else
                {
                    // Update the option
                    UpdateOption(optionType, options, adminLanguageId);
                }

                // Update the option count
                OptionType.UpdateCount(optionType.id);

                // Redirect the user to the list
                return Redirect("/admin_options" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.OptionType = optionType;
                ViewBag.Options = options;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the option
        // POST: /admin_options/translate
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
            Int32 optionTypeId = Convert.ToInt32(collection["hiddenOptionTypeId"]);
            string optionTypeTitle = collection["txtTranslatedTitle"];
            string[] optionIds = collection.GetValues("optionId");
            string[] optionTitles = collection.GetValues("optionTranslatedTitle");

            // Create the translated option type
            OptionType translatedOptionType = new OptionType();
            translatedOptionType.id = optionTypeId;
            translatedOptionType.title = optionTypeTitle;

            // Create a list of translated options
            Int32 optionCount = optionIds != null ? optionIds.Length : 0;
            List<Option> translatedOptions = new List<Option>(optionCount);

            for (int i = 0; i < optionCount; i++)
            {
                // Create a new option
                Option translatedOption = new Option();
                translatedOption.id = Convert.ToInt32(optionIds[i]);
                translatedOption.title = optionTitles[i];
                translatedOption.option_type_id = optionTypeId;

                // Add the translated option
                translatedOptions.Add(translatedOption);

            }

            // Create a error message
            string errorMessage = string.Empty;

            // Check the option type title
            if (optionTypeTitle.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }

            // Check for errors in options
            for (int i = 0; i < optionCount; i++)
            {
                if (optionTitles[i].Length > 50)
                {
                    errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), optionTitles[i], "50") + "<br/>";
                }
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved option type
                OptionType optionType = OptionType.GetOneById(optionTypeId, translationLanguageId);

                if (optionType == null)
                {
                    // Add a new translated option type
                    OptionType.AddLanguagePost(translatedOptionType, translationLanguageId);
                }
                else
                {
                    // Update the translated option type
                    optionType.title = translatedOptionType.title;
                    OptionType.UpdateLanguagePost(optionType, translationLanguageId);
               
                }

                // Translate options
                for (int i = 0; i < translatedOptions.Count; i++)
                {

                    // Get the option
                    Option option = Option.GetOneById(translatedOptions[i].id, translationLanguageId);

                    if (option == null)
                    {
                        // Add the translated option
                        Option.AddLanguagePost(translatedOptions[i], translationLanguageId);
                    }
                    else
                    {
                        // Update the option
                        option.title = translatedOptions[i].title;
                        Option.UpdateLanguagePost(option, translationLanguageId);
                    }
                }

                // Redirect the user to the list
                return Redirect("/admin_options" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardOptionType = OptionType.GetOneById(optionTypeId, adminLanguageId);
                ViewBag.StandardOptions = Option.GetByOptionTypeId(optionTypeId, adminLanguageId);
                ViewBag.TranslatedOptionType = translatedOptionType;
                ViewBag.TranslatedOptions = translatedOptions;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the option
        // POST: /admin_options/delete
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
                // Delete options by option type id
                List<Option> options = Option.GetByOptionTypeId(id);
                for(int i = 0; i < options.Count; i++)
                {
                    ProductOption.DeleteOnOptionId(options[i].id);
                    Option.DeleteOnId(options[i].id);   
                }

                // Delete product option types
                List<ProductOptionType> productOptionTypes = ProductOptionType.GetByOptionTypeId(id);
                for (int i = 0; i < productOptionTypes.Count; i++)
                {
                    ProductOptionType.DeleteOnId(productOptionTypes[i].id);
                }

                // Delete the option type and all the connected posts (CASCADE)
                errorCode = OptionType.DeleteOnId(id);
            }
            else
            {
                // Get all the translated options for the option type
                List<Option> options = Option.GetByOptionTypeId(id, languageId);

                // Delete translated options
                for (int i = 0; i < options.Count; i++)
                {
                    Option.DeleteLanguagePostOnId(options[i].id, languageId);
                }

                // Delete the option type post
                errorCode = OptionType.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_options" + returnUrl);
            
        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Add the option to the database
        /// </summary>
        /// <param name="optionType">A reference to a option type</param>
        /// <param name="options">A list of options</param>
        /// <param name="languageId">A language id</param>
        private void AddOption(OptionType optionType, List<Option> options, Int32 languageId)
        {
            // Save the option type
            long insertId = OptionType.AddMasterPost(optionType);
            optionType.id = Convert.ToInt32(insertId);
            OptionType.AddLanguagePost(optionType, languageId);

            // Save all the options
            foreach (Option option in options)
            {
                option.option_type_id = optionType.id;
                insertId = Option.AddMasterPost(option);
                option.id = Convert.ToInt32(insertId);
                Option.AddLanguagePost(option, languageId);
            }

        } // End of the AddOption method

        /// <summary>
        /// Update the option in the database
        /// </summary>
        /// <param name="optionType">A reference to a option type</param>
        /// <param name="options">A list of options</param>
        /// <param name="languageId">A language id</param>
        private void UpdateOption(OptionType optionType, List<Option> options, Int32 languageId)
        {

            // Update the option type
            OptionType.UpdateMasterPost(optionType);
            OptionType.UpdateLanguagePost(optionType, languageId);

            // Get all the saved options
            List<Option> savedOptions = Option.GetByOptionTypeId(optionType.id, languageId);

            // Update or add options
            foreach (Option option in options)
            {
                // Get the saved option
                Option savedOption = Option.GetOneById(option.id, languageId);

                if (savedOption != null)
                {
                    Option.UpdateMasterPost(option);
                    Option.UpdateLanguagePost(option, languageId);
                }
                else
                {
                    long insertId = Option.AddMasterPost(option);
                    option.id = Convert.ToInt32(insertId);
                    Option.AddLanguagePost(option, languageId);
                }

            }

            // Delete options
            foreach (Option savedOption in savedOptions)
            {
                // A boolean to indicate if the id is found
                bool idFound = false;

                // Loop all the input options
                foreach (Option option in options)
                {
                    // Id has been found
                    if (savedOption.id == option.id)
                    {
                        idFound = true;
                    }
                }

                // Delete the id if has not been found
                if (idFound == false)
                {
                    Option.DeleteOnId(savedOption.id);
                }
            }

        } // End of the Update Option method

        #endregion

    } // End of the class

} // End of the namespace
