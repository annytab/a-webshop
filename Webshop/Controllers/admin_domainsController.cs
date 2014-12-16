using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of domains
    /// </summary>
    [ValidateInput(false)]
    public class admin_domainsController : Controller
    {

        #region View methods

        // Get the list of domains
        // GET: /admin_domains/
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
        // POST: /admin_domains/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_domains?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_domains/edit
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
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.Countries = Country.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.Companies = Company.GetAll("id", "ASC");
            ViewBag.Domain = Domain.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty domain post if the domain post does not exist
            if (ViewBag.Domain == null)
            {
                // Add data to the view
                ViewBag.Domain = new Domain();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the images form
        // GET: /admin_domains/images
        [HttpGet]
        public ActionResult images(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the form
            ViewBag.Domain = Domain.GetOneById(id);
            ViewBag.ImageUrls = GetDomainImageUrls(id);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.Domain != null)
            {
                return View("images");
            }
            else
            {
                return Redirect("/admin_domains" + returnUrl);
            }

        } // End of the images method

        // Get the sitemap form
        // GET: /admin_domains/sitemap
        [HttpGet]
        public ActionResult sitemap(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the form
            ViewBag.Domain = Domain.GetOneById(id);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Redirect the user to index page if the domain is null
            if (ViewBag.Domain == null)
            {
                return Redirect("/admin_domains" + returnUrl);
            }

            // Return the view
            return View("sitemap");

        } // End of the sitemap method

        // Get the marketing form
        // GET: /admin_domains/marketing
        [HttpGet]
        public ActionResult marketing(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the form
            ViewBag.Domain = Domain.GetOneById(id);
            ViewBag.MarketingFiles = GetMarketingFiles(id);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Redirect the user to index page if the domain is null
            if(ViewBag.Domain == null)
            {
                return Redirect("/admin_domains" + returnUrl);
            }

            // Return the view
            return View("marketing");

        } // End of the marketing method

        #endregion

        #region Post methods

        // Update the domain
        // POST: /admin_domains/edit
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
            string webshop_name = collection["txtWebshopName"];
            string domainName = collection["txtDomainName"];
            string webAddress = collection["txtWebAddress"];
            Int32 country_id = Convert.ToInt32(collection["selectCountry"]);
            Int32 frontEndLanguageId = Convert.ToInt32(collection["selectFrontEndLanguage"]);
            Int32 backEndLanguageId = Convert.ToInt32(collection["selectBackEndLanguage"]);
            string currencyCode = collection["selectCurrency"];
            Int32 company_id = Convert.ToInt32(collection["selectCompany"]);
            byte default_display_view = Convert.ToByte(collection["selectDefaultDisplayView"]);
            byte mobile_display_view = Convert.ToByte(collection["selectMobileDisplayView"]);
            Int32 custom_theme_id = Convert.ToByte(collection["selectTemplate"]);
            bool pricesIncludesVat = Convert.ToBoolean(collection["selectPricesIncludesVat"]);
            string analytics_tracking_id = collection["txtAnalyticsTrackingId"];
            string facebook_app_id = collection["txtFacebookAppId"];
            string facebook_app_secret = collection["txtFacebookAppSecret"];
            string google_app_id = collection["txtGoogleAppId"];
            string google_app_secret = collection["txtGoogleAppSecret"];

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the domain
            Domain domain = Domain.GetOneById(id);
            bool postExists = true;

            // Check if the domain exists
            if (domain == null)
            {
                // Create an empty domain
                domain = new Domain();
                postExists = false;
            }

            // Check if we should remove the theme cache
            if(custom_theme_id != domain.custom_theme_id)
            {
                CustomTheme.RemoveThemeCache(domain.custom_theme_id);
            }

            // Update values
            domain.webshop_name = webshop_name;
            domain.domain_name = domainName.TrimEnd('/');
            domain.web_address = webAddress.TrimEnd('/');
            domain.country_id = country_id;
            domain.front_end_language = frontEndLanguageId;
            domain.back_end_language = backEndLanguageId;
            domain.currency = currencyCode;
            domain.company_id = company_id;
            domain.default_display_view = default_display_view;
            domain.mobile_display_view = mobile_display_view;
            domain.custom_theme_id = custom_theme_id;
            domain.prices_includes_vat = pricesIncludesVat;
            domain.analytics_tracking_id = analytics_tracking_id;
            domain.facebook_app_id = facebook_app_id;
            domain.facebook_app_secret = facebook_app_secret;
            domain.google_app_id = google_app_id;
            domain.google_app_secret = google_app_secret;

            // Get the domain on domain name
            Domain domainOnName = Domain.GetOneByDomainName(domain.domain_name);

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the domain
            if (domain.domain_name.ToLower().Contains("http://") || domain.domain_name.ToLower().Contains("https://") 
                || domain.domain_name.ToLower().Contains("www") || domain.domain_name.Length > 100)
            {
                errorMessage += "&#149; " + tt.Get("error_domain_name") + "<br/>";
            }
            if (domainOnName != null && domain.id != domainOnName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_unique"), tt.Get("domain_name")) + "<br/>";
            }
            if (domain.webshop_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "100") + "<br/>";
            }
            if(domain.web_address.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("web_address"), "100") + "<br/>";
            }
            if(domain.front_end_language == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("front_end_language").ToLower()) + "<br/>";
            }
            if (domain.back_end_language == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("back_end_language").ToLower()) + "<br/>";
            }
            if (domain.country_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("country").ToLower()) + "<br/>";
            }
            if (domain.company_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("company").ToLower()) + "<br/>";
            }
            if (domain.currency == "")
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("currency").ToLower()) + "<br/>";
            }
            if (domain.analytics_tracking_id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("analytics_tracking_id"), "50") + "<br/>";
            }
            if (domain.facebook_app_id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Facebook app id", "50") + "<br/>";
            }
            if (domain.facebook_app_secret.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Facebook app secret", "50") + "<br/>";
            }
            if (domain.google_app_id.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Google app id", "100") + "<br/>";
            }
            if (domain.google_app_secret.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Google app secret", "50") + "<br/>";
            }
            
            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the static text
                if (postExists == false)
                {
                    // Add the domain
                    Domain.Add(domain);
                }
                else
                {
                    // Update the domain
                    Domain.Update(domain);
                }

                // Redirect the user to the list
                return Redirect("/admin_domains" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.Countries = Country.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.Countries = Country.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.Companies = Company.GetAll("id", "ASC");
                ViewBag.Domain = domain;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Update the images for the domain
        // POST: /admin_domains/images
        [HttpPost]
        public ActionResult images(FormCollection collection)
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

            // Get form values 
            Int32 domainId = Convert.ToInt32(collection["hiddenDomainId"]);

            // Create the directory path
            string directoryPath = "/Content/domains/" + domainId.ToString() + "/images/";

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(directoryPath)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(directoryPath));
            }

            // Get all the files
            HttpFileCollectionBase imageFiles = Request.Files;

            // Get all the keys
            string[] imageKeys = imageFiles.AllKeys;

            // Loop all the images and save them
            for (int i = 0; i < imageFiles.Count; i++)
            {
                // Just continue if the file is empty
                if (imageFiles[i].ContentLength == 0)
                    continue;

                // Save the file
                imageFiles[i].SaveAs(Server.MapPath(directoryPath + imageKeys[i] + ".jpg"));
            }

            // Redirect the user to the list
            return Redirect("/admin_domains" + returnUrl);

        } // End of the images method

        // Update the domain
        // POST: /admin_domains/sitemap
        [HttpPost]
        public ActionResult sitemap(FormCollection collection)
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

            // Get form values 
            Int32 domainId = Convert.ToInt32(collection["hiddenDomainId"]);
            string priorityCategories = collection["selectPriorityCategories"];
            string priorityProducts = collection["selectPriorityProducts"];
            string changeFrequency = collection["selectChangeFrequency"];

            // Get the domain
            Domain domain = Domain.GetOneById(domainId);

            // Update the sitemap
            SitemapManager.CreateSitemap(domain, priorityCategories, priorityProducts, changeFrequency);

            // Redirect the user to the list
            return Redirect("/admin_domains" + returnUrl);

        } // End of the sitemap method

        // Delete the domain
        // GET: /admin_domains/delete
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

            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the domain post and all the connected posts (CASCADE)
            errorCode = Domain.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Delete all the files for the domain
            DeleteAllFiles(id);

            // Redirect the user to the list
            return Redirect("/admin_domains" + returnUrl);

        } // End of the delete method

        // Add or update a marketing file
        // POST: /admin_domains/add_marketing_file
        [HttpPost]
        public ActionResult add_marketing_file(FormCollection collection)
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

            // Get form values 
            Int32 domainId = Convert.ToInt32(collection["hiddenDomainId"]);
            string type = collection["selectType"];
            string title = collection["txtTitle"];
            string description = collection["txtDescription"];

            // Get the domain
            Domain domain = Domain.GetOneById(domainId);

            // Excecute the marketing file export
            if(type == "google_shopping")
            {
                GoogleShopping.Create(domain, title, description);
            }

            // Redirect the user the marketing form
            return RedirectToAction("marketing", new { id = domainId, returnUrl = returnUrl });

        } // End of the add_marketing_file method

        // Download the marketing file
        // GET: /admin_domains/download_marketing_file
        [HttpGet]
        public ActionResult download_marketing_file(Int32 id, string file_name = "", string returnUrl = "")
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

            // Create the file path string
            string filePath = Server.MapPath(file_name);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, System.IO.Path.GetFileName(filePath));
            }

            // Redirect the user to the marketing files page
            return RedirectToAction("marketing", new { id = id, returnUrl = returnUrl });

        } // End of the download_marketing_file method

        // Delete the marketing file
        // GET: /admin_domains/delete_marketing_file
        [HttpGet]
        public ActionResult delete_marketing_file(Int32 id, string file_name = "", string returnUrl = "")
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

            // Create the file path string
            string filePath = Server.MapPath(file_name);

            try
            {
                // Delete the file if it exists
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Redirect the user to the marketing files page
            return RedirectToAction("marketing", new { id = id, returnUrl = returnUrl });

        } // End of the delete_marketing_file method

        #endregion

        #region Helper methods

        /// <summary>
        /// Get image urls for the domain
        /// </summary>
        /// <param name="domainId">The id for the domain</param>
        /// <returns>A key string list with urls</returns>
        private KeyStringList GetDomainImageUrls(Int32 domainId)
        {
            // Create the list to return
            KeyStringList domainImageUrls = new KeyStringList(5);

            // Create paths
            string noImagePath = "/Content/images/annytab_design/no_image_wide.jpg";
            string directoryPath = "/Content/domains/" + domainId.ToString() + "/images/";

            // Add images to the key string list
            domainImageUrls.Add("background_image", directoryPath + "background_image.jpg");
            domainImageUrls.Add("default_logotype", directoryPath + "default_logotype.jpg");
            domainImageUrls.Add("mobile_logotype", directoryPath + "mobile_logotype.jpg");
            domainImageUrls.Add("big_icon", directoryPath + "big_icon.jpg");
            domainImageUrls.Add("small_icon", directoryPath + "small_icon.jpg");

            // Get all the keys in the dictionary
            List<string> keys = domainImageUrls.dictionary.Keys.ToList<string>();

            // Loop all the keys
            for (int i = 0; i < keys.Count; i++)
            {
                // Get the url
                string url = domainImageUrls.Get(keys[i]);

                // Check if the file exists
                if (System.IO.File.Exists(Server.MapPath(url)) == false)
                {
                    domainImageUrls.Update(keys[i], noImagePath);
                }
            }

            // Return the list
            return domainImageUrls;

        } // End of the GetDomainImageUrls method

        /// <summary>
        /// Delete all the files for the domain
        /// </summary>
        /// <param name="domainId">The domain id</param>
        private void DeleteAllFiles(Int32 domainId)
        {
            // Define the directory url
            string directory = Server.MapPath("/Content/domains/" + domainId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.Delete(directory, true);
            }

        } // End of the DeleteAllFiles method


        /// <summary>
        /// Get a list of urls to marketing files
        /// </summary>
        /// <returns>A list with urls to marketing files</returns>
        private List<string> GetMarketingFiles(Int32 domainId)
        {
            // Create the list to return
            List<string> fileUrls = new List<string>(10);

            // Create the directory string
            string filesDirectory = "/Content/domains/" + domainId.ToString() + "/marketing/";

            try
            {
                // Get all the file names
                string[] fileUrlList = System.IO.Directory.GetFiles(Server.MapPath(filesDirectory));

                // Add them to the list
                for (int i = 0; i < fileUrlList.Length; i++)
                {
                    fileUrls.Add(filesDirectory + System.IO.Path.GetFileName(fileUrlList[i]));
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Return the list
            return fileUrls;

        } // End of the GetMarketingFiles method

        #endregion

    } // End of the class

} // End of the namespace