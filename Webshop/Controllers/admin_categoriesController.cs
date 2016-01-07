using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of categories
    /// </summary>
    [ValidateInput(false)]
    public class admin_categoriesController : Controller
    {
        #region View methods

        // Get the list of categories
        // GET: /admin_categories/
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
        // POST: /admin_categories/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_categories?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_categories/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
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

            // Get the administrator default language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the view
            ViewBag.Category = Category.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Add data to the view
            if (ViewBag.Category == null)
            {
                // Add data to the view
                ViewBag.Category = new Category();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the images form
        // GET: /admin_categories/images/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult images(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
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

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the view
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.Category = Category.GetOneById(id, adminLanguageId);
            ViewBag.MainImageUrl = GetMainImageUrl(id, languageId);
            ViewBag.EnvironmentImages = Tools.GetEnvironmentImageUrls(id, languageId, true);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.Category != null)
            {
                return View("images");
            }
            else
            {
                return Redirect("/admin_categories" + returnUrl);
            }

        } // End of the images method

        // Get the translate form
        // GET: /admin_categories/translate/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult translate(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
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

            // Get the default language id
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
            ViewBag.StandardCategory = Category.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedCategory = Category.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardCategory != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_categories" + returnUrl);
            }

        } // End of the translate method

        // Reset statistics for all categories or a specific category, set the id to 0 if you want to reset statistics for all categories
        // GET: /admin_categories/reset_statistics/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult reset_statistics(Int32 id = 0, string returnUrl = "")
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

            // Reset statistics for all categories or just one category
            if (id == 0)
            {
                Category.ResetStatistics();
            }
            else
            {
                Category.UpdatePageviews(id, 0);
            }

            // Return the index view
            return Redirect("/admin_categories" + returnUrl);

        } // End of the reset_statistics method

        #endregion

        #region Post methods

        // Update the category
        // POST: /admin_categories/edit
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

            // Get form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            Int32 parentCategoryId = Convert.ToInt32(collection["selectCategory"]);
            string title = collection["txtTitle"];
            string description = collection["txtDescription"];
            string metaDescription = collection["txtMetaDescription"];
            string metaKeywords = collection["txtMetaKeywords"];
            string pageName = collection["txtPageName"];
            string metaRobots = collection["selectMetaRobots"];
            bool useLocalImages = Convert.ToBoolean(collection["cbLocalImages"]);
            DateTime date_added = DateTime.MinValue;
            DateTime.TryParse(collection["txtDateAdded"], out date_added);
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the category
            Category category = Category.GetOneById(id, adminLanguageId);

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Check if the category exists
            if (category == null)
            {
                // Create a new category
                category = new Category();
            }

            // Set values for the category
            category.parent_category_id = parentCategoryId;
            category.title = title;
            category.main_content = description;
            category.meta_description = metaDescription;
            category.meta_keywords = metaKeywords;
            category.page_name = pageName;
            category.meta_robots = metaRobots;
            category.use_local_images = useLocalImages;
            category.date_added = AnnytabDataValidation.TruncateDateTime(date_added);
            category.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a category on page name
            Category categoryOnPageName = Category.GetOneByPageName(category.page_name, adminLanguageId);

            // Check the page name
            if (categoryOnPageName != null && category.id != categoryOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (category.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(category.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (category.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (category.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (category.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (category.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the category
                if (category.id != 0)
                {
                    // Update the category
                    Category.UpdateMasterPost(category);
                    Category.UpdateLanguagePost(category, adminLanguageId);
                }
                else
                {
                    // Add the category
                    Int64 insertId = Category.AddMasterPost(category);
                    category.id = Convert.ToInt32(insertId);
                    Category.AddLanguagePost(category, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_categories" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Category = category;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Update images for the category
        // POST: /admin_categories/images
        [HttpPost]
        public ActionResult images(FormCollection collection)
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

            // Get form values
            Int32 categoryId = Convert.ToInt32(collection["txtId"]);
            Int32 languageId = Convert.ToInt32(collection["selectLanguage"]);

            // Get images
            string[] environmentImageUrls = collection.GetValues("otherImageUrl");
            HttpPostedFileBase mainImage = null;
            List<HttpPostedFileBase> environmentImages = new List<HttpPostedFileBase>(10);

            HttpFileCollectionBase images = Request.Files;
            string[] imageKeys = images.AllKeys;
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].ContentLength == 0)
                    continue;

                if (imageKeys[i] == "uploadMainImage")
                    mainImage = images[i];
                else
                    environmentImages.Add(images[i]);
            }

            // Update images
            UpdateImages(categoryId, languageId, mainImage, environmentImages, environmentImageUrls);

            // Redirect the user to the list
            return Redirect("/admin_categories" + returnUrl);

        } // End of the images method

        // Translate the category
        // POST: /admin_categories/translate
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

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get all the form values
            Int32 translationLanguageId = Convert.ToInt32(collection["selectLanguage"]);
            Int32 categoryId = Convert.ToInt32(collection["hiddenCategoryId"]);
            string title = collection["txtTranslatedTitle"];
            string description = collection["txtTranslatedDescription"];
            string metadescription = collection["txtTranslatedMetadescription"];
            string metakeywords = collection["txtTranslatedMetakeywords"];
            string pagename = collection["txtTranslatedPagename"];
            bool use_local_images = Convert.ToBoolean(collection["cbLocalImages"]);
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Create the translated category
            Category translatedCategory = new Category();
            translatedCategory.id = categoryId;
            translatedCategory.title = title;
            translatedCategory.main_content = description;
            translatedCategory.meta_description = metadescription;
            translatedCategory.meta_keywords = metakeywords;
            translatedCategory.page_name = pagename;
            translatedCategory.use_local_images = use_local_images;
            translatedCategory.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a category on page name
            Category categoryOnPageName = Category.GetOneByPageName(translatedCategory.page_name, translationLanguageId);

            // Check for errors
            if (categoryOnPageName != null && translatedCategory.id != categoryOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedCategory.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(translatedCategory.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedCategory.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (translatedCategory.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (translatedCategory.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (translatedCategory.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved category
                Category category = Category.GetOneById(categoryId, translationLanguageId);

                if (category == null)
                {
                    // Add a new translated category
                    Category.AddLanguagePost(translatedCategory, translationLanguageId);
                }
                else
                {
                    // Update values for the saved category
                    category.title = translatedCategory.title;
                    category.main_content = translatedCategory.main_content;
                    category.meta_description = translatedCategory.meta_description;
                    category.meta_keywords = translatedCategory.meta_keywords;
                    category.page_name = translatedCategory.page_name;
                    category.use_local_images = translatedCategory.use_local_images;
                    category.inactive = translatedCategory.inactive;

                    // Update the category translation
                    Category.UpdateLanguagePost(category, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_categories" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardCategory = Category.GetOneById(categoryId, adminLanguageId);
                ViewBag.TranslatedCategory = translatedCategory;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the category
        // POST: /admin_categories/delete/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
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
                // Delete the category and all the posts connected to this category (CASCADE)
                errorCode = Category.DeleteOnId(id);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Delete all files
                DeleteAllFiles(id);

            }
            else
            {
                // Delete the translated post
                errorCode = Category.DeleteLanguagePostOnId(id, languageId);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Delete all the language specific files
                DeleteLanguageFiles(id, languageId);
            }

            // Redirect the user to the list
            return Redirect("/admin_categories" + returnUrl);

        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Get the main image url for a category
        /// </summary>
        /// <param name="categoryId">The category id</param>
        /// <param name="languageId">The language id</param>
        /// <returns>An image url as a string</returns>
        public string GetMainImageUrl(Int32 categoryId, Int32 languageId)
        {
            // Create the string to return
            string imageUrl = "/Content/images/annytab_design/no_image_square.jpg";

            // Create the main image url
            string categoryMainImageUrl = "/Content/categories/" + (categoryId / 100).ToString() + "/" + categoryId.ToString() + "/" + languageId.ToString() + "/main_image.jpg";

            // Check if the main image exists
            if (System.IO.File.Exists(Server.MapPath(categoryMainImageUrl)))
            {
                imageUrl = categoryMainImageUrl;
            }

            // Return the image url
            return imageUrl;

        } // End of the GetMainImageUrl

        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="categoryId">The category id</param>
        /// <param name="languageId">The language id</param>
        /// <param name="mainImage">The posted main image file</param>
        /// <param name="environmentImages">A list of posted files for environment images</param>
        /// <param name="environmentImageUrls">An array of urls to environment images</param>
        private void UpdateImages(Int32 categoryId, Int32 languageId, HttpPostedFileBase mainImage, List<HttpPostedFileBase> environmentImages, string[] environmentImageUrls)
        {

            // Create directory strings
            string environmentImagesDirectory = "/Content/categories/" + (categoryId / 100).ToString() + "/" + categoryId.ToString() + "/" + languageId.ToString() + "/environment_images/";
            string mainImageUrl = "/Content/categories/" + (categoryId / 100).ToString() + "/" + categoryId.ToString() + "/" + languageId.ToString() + "/main_image.jpg";

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(environmentImagesDirectory)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(environmentImagesDirectory));
            }

            // Create an array for urls of saved images
            string[] savedOtherImageUrls = null;

            // Get saved images
            try
            {
                savedOtherImageUrls = System.IO.Directory.GetFiles(Server.MapPath(environmentImagesDirectory));
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Check for images to delete
            if (savedOtherImageUrls != null)
            {
                // Loop the urls of saved images
                for (int i = 0; i < savedOtherImageUrls.Length; i++)
                {
                    // Create a boolean to indicate if the image should be deleted
                    bool deleteImage = true;

                    // Get the filename of the saved file
                    string savedImageFileName = System.IO.Path.GetFileName(savedOtherImageUrls[i]);

                    // Loop the names of images that exists
                    if (environmentImageUrls != null)
                    {
                        for (int j = 0; j < environmentImageUrls.Length; j++)
                        {
                            // Get the file name of the other image url
                            string otherImageUrlFileName = System.IO.Path.GetFileName(environmentImageUrls[j]);

                            // Check if the file names are equal
                            if (otherImageUrlFileName == savedImageFileName)
                            {
                                deleteImage = false;
                                break;
                            }
                        }
                    }

                    if (deleteImage == true)
                    {
                        // Delete the image
                        System.IO.File.Delete(Server.MapPath(environmentImagesDirectory + savedImageFileName));
                    }
                }
            }

            // Save the main image
            if (mainImage != null)
            {
                mainImage.SaveAs(Server.MapPath(mainImageUrl));
            }

            // Save other images
            if (environmentImages != null)
            {
                for (int i = 0; i < environmentImages.Count; i++)
                {
                    environmentImages[i].SaveAs(Server.MapPath(environmentImagesDirectory + System.IO.Path.GetFileName(environmentImages[i].FileName)));
                }
            }

        } // End of the UpdateImages method

        /// <summary>
        /// Delete all the files for the category
        /// </summary>
        /// <param name="categoryId">The category id</param>
        private void DeleteAllFiles(Int32 categoryId)
        {
            // Define the directory url
            string categoryDirectory = Server.MapPath("/Content/categories/" + (categoryId / 100).ToString() + "/" + categoryId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(categoryDirectory))
            {
                System.IO.Directory.Delete(categoryDirectory, true);
            }

        } // End of the DeleteAllFiles method

        /// <summary>
        /// Delete all the files for a specific language and category
        /// </summary>
        /// <param name="categoryId">The category id</param>
        /// <param name="languageId">The language id</param>
        private void DeleteLanguageFiles(Int32 categoryId, Int32 languageId)
        {
            // Define the directory url
            string categoryDirectory = Server.MapPath("/Content/categories/" + (categoryId / 100).ToString() + "/" + categoryId.ToString() + "/" + languageId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(categoryDirectory))
            {
                System.IO.Directory.Delete(categoryDirectory, true);
            }

        } // End of the DeleteLanguageFiles method

        #endregion

	} // End of the class

} // End of the namespace