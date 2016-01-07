using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of custom design templates
    /// </summary>
    [ValidateInput(false)]
    public class admin_custom_designController : Controller
    {
        #region View methods

        // Get the index page
        // GET: /admin_custom_design/
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
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Search in the list
        // POST: /admin_custom_design/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_custom_design?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit theme page
        // GET: /admin_custom_design/edit_theme/2
        [HttpGet]
        public ActionResult edit_theme(Int32 id = 0, string returnUrl = "")
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
            ViewBag.CustomTheme = CustomTheme.GetOneById(id);
            ViewBag.CustomThemeTemplates = CustomThemeTemplate.GetAllPostsByCustomThemeId(id, "user_file_name", "ASC");
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Create a new theme if the theme does not exist
            if(ViewBag.CustomTheme == null)
            {
                ViewBag.CustomTheme = new CustomTheme();
            }

            // Return the view
            return View();

        } // End of the edit_theme method

        // Get the edit template page
        // GET: /admin_custom_design/edit_template/5?userFileName=layout.cshtml
        [HttpGet]
        public ActionResult edit_template(Int32 id = 0, string userFileName = "", string returnUrl = "")
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

            // Get the administrator default language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Check if the theme exists
            if(CustomTheme.MasterPostExists(id) == false)
            {
                return RedirectToAction("index");
            }

            // Get the custom them template
            CustomThemeTemplate template = CustomThemeTemplate.GetOneById(id, userFileName);

            // Create a new template if the template does not exist
            if (template == null)
            {
                // Redirect the user to the index page
                template = new CustomThemeTemplate();
                template.custom_theme_id = id;
            }

            // Set form data
            ViewBag.CustomTemplate = template;
            ViewBag.MasterDesign = CustomThemeTemplate.GetMasterFileContent(template.master_file_url);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the edit template view
            return View("edit_template");

        } // End of the edit_template method

        // Get the page with user images
        // GET: /admin_custom_design/images
        [HttpGet]
        public ActionResult images(string returnUrl = "")
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
            ViewBag.CustomImages = GetCustomImageUrls();
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            return View();

        } // End of the images method

        // Get the page with user files
        // GET: /admin_custom_design/user_files
        [HttpGet]
        public ActionResult user_files(string returnUrl = "")
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

            // Get the administrator default language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the form
            ViewBag.UserFiles = GetUserFileUrls();
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            return View("user_files");

        } // End of the user_files method

        #endregion

        #region Post methods

        // Update the theme
        // POST: /admin_custom_design/edit_theme
        [HttpPost]
        public ActionResult edit_theme(FormCollection collection)
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

            // Get form data
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string name = collection["txtName"];

            // Get the theme
            CustomTheme theme = CustomTheme.GetOneById(id);

            // Check if the theme exists
            if(theme == null)
            {
                theme = new CustomTheme();
            }

            // Update values
            theme.name = AnnytabDataValidation.TruncateString(name, 100);

            // Add or update the theme
            if(id == 0)
            {
                // Add the theme
                theme.id = (Int32)CustomTheme.Add(theme);
                CustomTheme.AddCustomThemeTemplates(theme.id);
            }
            else
            {
                // Update the theme
                CustomTheme.Update(theme);
            }

            // Redirect the user to edit theme page
            return RedirectToAction("edit_theme", new { id = theme.id, returnUrl = returnUrl });

        } // End of the edit_theme method

        // Update the custom theme template
        // POST: /admin_custom_design/edit_template
        [HttpPost]
        public ActionResult edit_template(FormCollection collection)
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

            // Get form data
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string user_file_name = collection["txtUserFileName"];
            string file_content = collection["txtContent"];
            string comment = collection["txtComment"];

            // Get the template
            CustomThemeTemplate template = CustomThemeTemplate.GetOneById(id, user_file_name);

            // Update the template if is different from null
            if(template != null)
            {
                // Set the file content
                template.user_file_content = file_content;
                template.comment = comment;

                // Update the template
                CustomTheme.UpdateTemplate(template);
            }
            else
            {
                // Create a new template
                template = new CustomThemeTemplate();
                template.custom_theme_id = id;
                template.user_file_name = user_file_name;
                template.user_file_content = file_content;
                template.comment = comment;

                // Add the template
                CustomThemeTemplate.Add(template);
            }

            // Redirect the user to the edit theme page
            return RedirectToAction("edit_theme", new { id = template.custom_theme_id, returnUrl = returnUrl });

        } // End of the edit_template method

        // Restore all the templates to the default master design
        // GET: /admin_custom_design/restore_templates_to_default/5
        [HttpGet]
        public ActionResult restore_templates_to_default(Int32 id = 0, string returnUrl = "")
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

            // Delete the existing templates
            CustomTheme.DeleteTemplatesOnId(id);

            // Add all the custom templates
            CustomTheme.AddCustomThemeTemplates(id);

            // Redirect the user to the edit theme page
            return RedirectToAction("edit_theme", new { id = id, returnUrl = returnUrl });

        } // End of the restore_templates_to_default method

        // Export all master templates to a zip file
        // GET: /admin_custom_design/export_master_templates_to_zip/5
        [HttpGet]
        public ActionResult export_master_templates_to_zip(Int32 id = 0, string returnUrl = "")
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

            // Get all the template files
            List<CustomThemeTemplate> templates = CustomThemeTemplate.GetAllPostsByCustomThemeId(id, "user_file_name", "ASC");

            // Create the zip file variable
            Ionic.Zip.ZipFile zipFile = null;

            // Create a output stream
            MemoryStream outputStream = new MemoryStream();

            // Create the file stream result
            FileStreamResult fileStreamResult = null;

            try
            {
                // Create a new zip file
                zipFile = new Ionic.Zip.ZipFile(System.Text.Encoding.UTF8);

                // Loop all the templates
                for (int i = 0; i < templates.Count; i++)
                {
                    // Get the master file content
                    string masterContent = CustomThemeTemplate.GetMasterFileContent(templates[i].master_file_url);

                    // Get the stream
                    byte[] streamData = System.Text.Encoding.UTF8.GetBytes(masterContent);

                    // Add the zip file
                    zipFile.AddEntry(templates[i].user_file_name, streamData);
                }

                // Save the zip file
                zipFile.Save(outputStream);

                // Go to the beginning of the output stream
                outputStream.Seek(0, SeekOrigin.Begin);

                // Create the file stream result
                fileStreamResult = new FileStreamResult(outputStream, "application/zip") { FileDownloadName = "master-templates.zip" };

            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }
            finally
            {
                // Close streams
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }

            // Return the zip file
            return fileStreamResult;

        } // End of the export_master_templates_to_zip method

        // Export all templates to a zip file
        // GET: /admin_custom_design/export_templates_to_zip/5
        [HttpGet]
        public ActionResult export_templates_to_zip(Int32 id = 0, string returnUrl = "")
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

            // Get all the custom theme templates
            List<CustomThemeTemplate> templates = CustomThemeTemplate.GetAllPostsByCustomThemeId(id, "user_file_name", "ASC");

            // Create the zip file variable
            Ionic.Zip.ZipFile zipFile = null;

            // Create a output stream
            MemoryStream outputStream = new MemoryStream();

            // Create the file stream result
            FileStreamResult fileStreamResult = null;

            try
            {
                // Create a new zip file
                zipFile = new Ionic.Zip.ZipFile(System.Text.Encoding.UTF8);

                // Loop all the templates
                for (int i = 0; i < templates.Count; i++)
                {
                    // Get the stream
                    byte[] streamData = System.Text.Encoding.UTF8.GetBytes(templates[i].user_file_content);

                    // Add the zip file
                    zipFile.AddEntry(templates[i].user_file_name, streamData);
                }

                // Save the zip file
                zipFile.Save(outputStream);

                // Go to the beginning of the output stream
                outputStream.Seek(0, SeekOrigin.Begin);

                // Create the file stream result
                fileStreamResult = new FileStreamResult(outputStream, "application/zip") { FileDownloadName = "custom-templates.zip" };

            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }
            finally
            {
                // Close streams
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }

            // Return the zip file
            return fileStreamResult;

        } // End of the export_templates_to_zip method

        // Import template files from a zip file
        // POST: /admin_custom_design/import_templates_from_zip
        [HttpPost]
        public ActionResult import_templates_from_zip(FormCollection collection)
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

            // Get the form data
            Int32 custom_theme_id = Convert.ToInt32(collection["hiddenId"]);

            // Delete the existing templates
            CustomTheme.DeleteTemplatesOnId(custom_theme_id);

            // Add master templates
            CustomTheme.AddCustomThemeTemplates(custom_theme_id);

            // Get the zip file
            HttpPostedFileBase uploadedFile = Request.Files["importFilesFromZip"];

            // Create the zip file variable
            Ionic.Zip.ZipFile zipFile = null;

            try
            {
                // Read the zip file
                zipFile = Ionic.Zip.ZipFile.Read(uploadedFile.InputStream);

                // Loop the zip file
                for(int i = 0; i < zipFile.Count; i++)
                {
                    // Get the file name
                    string fileName = zipFile[i].FileName;

                    // Get the file content
                    string fileContent = "";

                    // Get the stream from the zip entry
                    using (Stream stream = zipFile[i].OpenReader())
                    {
                        // Create a byte buffer
                        byte[] buffer = new byte[stream.Length];

                        // Create a byte counter
                        int byteCounter = 0;

                        // Read the bytes
                        while ((byteCounter = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            // Get the content of the zip-file
                            fileContent = System.Text.Encoding.UTF8.GetString(buffer);
                        }
                    }

                    // Try to find the template
                    CustomThemeTemplate template = CustomThemeTemplate.GetOneById(custom_theme_id, fileName);

                    // Check if the template is null or not
                    if(template == null)
                    {
                        template = new CustomThemeTemplate();
                        template.custom_theme_id = custom_theme_id;
                        template.user_file_name = fileName;
                        template.user_file_content = fileContent;
                        template.comment = "This template does not have a master file.";

                        // Add the template
                        CustomTheme.AddTemplate(template);
                    }
                    else
                    {
                        // Change the content of the file
                        template.user_file_content = fileContent;

                        // Update the template
                        CustomTheme.UpdateTemplate(template);
                    }
                }
            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }
            finally
            {
                // Close the zip file if it is different from null
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }

            // Redirect the user to the edit theme page
            return RedirectToAction("edit_theme", new { id = custom_theme_id, returnUrl = returnUrl });

        } // End of the import_templates_from_zip method

        // Delete the theme
        // GET: /admin_custom_design/delete_theme
        [HttpGet]
        public ActionResult delete_theme(Int32 id = 0, string returnUrl = "")
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

            // Delete the theme and all the connected posts (CASCADE)
            errorCode = CustomTheme.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_custom_design" + returnUrl);

        } // End of the delete_theme method

        // Delete the template
        // GET: /admin_custom_design/delete_template
        [HttpGet]
        public ActionResult delete_template(Int32 id = 0, string userFileName = "", string returnUrl = "")
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

            // Delete the template
            CustomTheme.DeleteTemplateOnId(id, userFileName);

            // Redirect the user to the list
            return RedirectToAction("edit_theme", "admin_custom_design", new { id = id, returnUrl = returnUrl });

        } // End of the delete_template method

        // Update custom images
        // POST: /admin_custom_design/images
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

            // Get images
            string[] imageUrls = collection.GetValues("otherImageUrl");
            List<HttpPostedFileBase> customImages = new List<HttpPostedFileBase>(10);

            HttpFileCollectionBase images = Request.Files;
            string[] imageKeys = images.AllKeys;
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].ContentLength == 0)
                    continue;

                customImages.Add(images[i]);
            }

            // Update images
            UpdateImages(customImages, imageUrls);

            // Redirect the user to the same page
            return Redirect("/admin_custom_design" + returnUrl);

        } // End of the images method

        // Export all user images to a zip file
        // GET: /admin_custom_design/export_images_to_zip/
        [HttpGet]
        public ActionResult export_images_to_zip(string returnUrl = "")
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

            // Create the zip file variable
            Ionic.Zip.ZipFile zipFile = null;

            // Create the directory string
            string imagesDirectory = "/Content/images/user_design/";

            // Create a output stream
            MemoryStream outputStream = new MemoryStream();

            // Create the file stream result
            FileStreamResult fileStreamResult = null;

            try
            {
                // Create a new zip file
                zipFile = new Ionic.Zip.ZipFile(System.Text.Encoding.UTF8);

                // Get all the file names
                string[] imageUrlList = System.IO.Directory.GetFiles(Server.MapPath(imagesDirectory));

                // Add all the images to the zip file
                for (int i = 0; i < imageUrlList.Length; i++)
                {
                    zipFile.AddFile(imageUrlList[i], "");
                }

                // Save the zip file
                zipFile.Save(outputStream);

                // Go to the beginning of the output stream
                outputStream.Seek(0, SeekOrigin.Begin);

                // Create the file stream result
                fileStreamResult = new FileStreamResult(outputStream, "application/zip") { FileDownloadName = "user-images.zip" };

            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }
            finally
            {
                // Close streams
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }

            // Return the zip file
            return fileStreamResult;

        } // End of the export_images_to_zip method

        // Import images from a zip file
        // POST: /admin_custom_design/import_images_from_zip/
        [HttpPost]
        public ActionResult import_images_from_zip(FormCollection collection)
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

            // Get the zip file
            HttpPostedFileBase uploadedFile = Request.Files["importImagesFromZip"];

            // Create the directory string
            string imagesDirectory = Server.MapPath("/Content/images/user_design/");

            // Create the zip file variable
            Ionic.Zip.ZipFile zipFile = null;

            try
            {
                // Create the directory if it does not exist
                if (System.IO.Directory.Exists(imagesDirectory) == false)
                {
                    System.IO.Directory.CreateDirectory(imagesDirectory);
                }

                // Read the zip file
                zipFile = Ionic.Zip.ZipFile.Read(uploadedFile.InputStream);

                // Loop all the images in the zip file
                foreach (Ionic.Zip.ZipEntry image in zipFile)
                {
                    // Extract the image
                    image.Extract(imagesDirectory, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }

            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }
            finally
            {
                // Close the zip file if it is different from null
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }

            // Redirect the user to the images page
            return RedirectToAction("images", new { returnUrl = returnUrl });

        } // End of the import_images_from_zip method

        // Delete all user images
        // GET: /admin_custom_design/delete_all_user_images
        [HttpGet]
        public ActionResult delete_all_user_images(string returnUrl = "")
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

            // Create the directory string
            string imagesDirectory = "/Content/images/user_design/";

            try
            {
                // Get all the file names
                string[] imageUrlList = System.IO.Directory.GetFiles(Server.MapPath(imagesDirectory));

                // Delete all images in the directory
                for (int i = 0; i < imageUrlList.Length; i++)
                {
                    System.IO.File.Delete(imageUrlList[i]);
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Redirect the user to the index page
            return RedirectToAction("images", new { returnUrl = returnUrl });

        } // End of the delete_all_user_images method

        // Add user files
        // POST: /admin_custom_design/add_user_files
        [HttpPost]
        public ActionResult add_user_files(FormCollection collection)
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

            // Create the directory string
            string userFileDirectory = Server.MapPath("/Content/user_files/");

            // Get all the files
            HttpFileCollectionBase userFiles = Request.Files;

            try
            {
                // Create the directory if it does not exist
                if (System.IO.Directory.Exists(userFileDirectory) == false)
                {
                    System.IO.Directory.CreateDirectory(userFileDirectory);
                }

                // Loop the files and save them
                if (userFiles != null)
                {
                    for (int i = 0; i < userFiles.Count; i++)
                    {
                        userFiles[i].SaveAs(userFileDirectory + System.IO.Path.GetFileName(userFiles[i].FileName));
                    }
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Redirect the user to the user files page
            return RedirectToAction("user_files", new { returnUrl = returnUrl });

        } // End of the add_user_files method

        // Export all user files to a zip file
        // GET: /admin_custom_design/export_user_files_to_zip/
        [HttpGet]
        public ActionResult export_user_files_to_zip(string returnUrl = "")
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

            // Get file urls
            List<string> userFileUrls = GetUserFileUrls();

            // Create the zip file variable
            Ionic.Zip.ZipFile zipFile = null;

            // Create a output stream
            MemoryStream outputStream = new MemoryStream();

            // Create the file stream result
            FileStreamResult fileStreamResult = null;

            try
            {
                // Create a new zip file
                zipFile = new Ionic.Zip.ZipFile(System.Text.Encoding.UTF8);

                // Add all the user files to the zip file
                for (int i = 0; i < userFileUrls.Count; i++)
                {
                    zipFile.AddFile(Server.MapPath(userFileUrls[i]), "");
                }

                // Save the zip file
                zipFile.Save(outputStream);

                // Go to the beginning of the output stream
                outputStream.Seek(0, SeekOrigin.Begin);

                // Create the file stream result
                fileStreamResult = new FileStreamResult(outputStream, "application/zip") { FileDownloadName = "user-files.zip" };

            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }
            finally
            {
                // Close streams
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }

            // Return the zip file
            return fileStreamResult;

        } // End of the export_user_files_to_zip method

        // Import user files from a zip file
        // POST: /admin_custom_design/import_user_files_from_zip
        [HttpPost]
        public ActionResult import_user_files_from_zip(FormCollection collection)
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

            // Get the zip file
            HttpPostedFileBase uploadedFile = Request.Files["importFilesFromZip"];

            // Create the directory string
            string userFileDirectory = Server.MapPath("/Content/user_files/");

            // Create the zip file variable
            Ionic.Zip.ZipFile zipFile = null;

            try
            {
                // Create the directory if it does not exist
                if (System.IO.Directory.Exists(userFileDirectory) == false)
                {
                    System.IO.Directory.CreateDirectory(userFileDirectory);
                }

                // Read the zip file
                zipFile = Ionic.Zip.ZipFile.Read(uploadedFile.InputStream);

                // Loop all the files in the zip file
                foreach (Ionic.Zip.ZipEntry file in zipFile)
                {
                    // Extract the file
                    file.Extract(userFileDirectory, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }

            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }
            finally
            {
                // Close the zip file if it is different from null
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }

            // Redirect the user to the user files page
            return RedirectToAction("user_files", new { returnUrl = returnUrl });

        } // End of the import_user_files_from_zip method

        // Delete the user file
        // GET: /admin_custom_design/delete_user_file
        [HttpGet]
        public ActionResult delete_user_file(string file_name = "", string returnUrl = "")
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

            // Create the directory string
            string userFilePath = Server.MapPath(file_name);

            try
            {
                // Delete the file if it exists
                if (System.IO.File.Exists(userFilePath))
                {
                    System.IO.File.Delete(userFilePath);
                }
            }
            catch(Exception ex)
            {
                string exMessage = ex.Message;
            }
            
            // Redirect the user to the user files page
            return RedirectToAction("user_files", new { returnUrl = returnUrl });
            
        } // End of the delete_user_file method

        #endregion

        #region Helper methods

        /// <summary>
        /// Get a list of urls to custom images
        /// </summary>
        /// <returns>A list with image urls to custom images</returns>
        private List<string> GetCustomImageUrls()
        {
            // Create the list to return
            List<string> imageUrls = new List<string>(10);

            // Create the directory string
            string imagesDirectory = "/Content/images/user_design/";

            try
            {
                // Get all the file names
                string[] imageUrlList = System.IO.Directory.GetFiles(Server.MapPath(imagesDirectory));

                // Add them to the list
                for (int i = 0; i < imageUrlList.Length; i++)
                {
                    imageUrls.Add(imagesDirectory + System.IO.Path.GetFileName(imageUrlList[i]));
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Return the list
            return imageUrls;

        } // End of the GetCustomImageUrls method

        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="images">A list of posted files for images</param>
        /// <param name="imageUrls">An array of urls to images</param>
        private void UpdateImages(List<HttpPostedFileBase> images, string[] imageUrls)
        {

            // Create directory strings
            string imageDirectory = "/Content/images/user_design/";

            // Create an array for urls of saved images
            string[] savedOtherImageUrls = null;

            try
            {
                // Check if the directory exists
                if (System.IO.Directory.Exists(Server.MapPath(imageDirectory)) == false)
                {
                    // Create the directory
                    System.IO.Directory.CreateDirectory(Server.MapPath(imageDirectory));
                }

                // Get saved images
                savedOtherImageUrls = System.IO.Directory.GetFiles(Server.MapPath(imageDirectory));
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
                    if (imageUrls != null)
                    {
                        for (int j = 0; j < imageUrls.Length; j++)
                        {
                            // Get the file name of the other image url
                            string otherImageUrlFileName = System.IO.Path.GetFileName(imageUrls[j]);

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
                        System.IO.File.Delete(Server.MapPath(imageDirectory + savedImageFileName));
                    }
                }
            }

            // Save images
            if (images != null)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].SaveAs(Server.MapPath(imageDirectory + System.IO.Path.GetFileName(images[i].FileName)));
                }
            }

        } // End of the UpdateImages method

        /// <summary>
        /// Get a list of urls to user files
        /// </summary>
        /// <returns>A list with file urls to user files</returns>
        private List<string> GetUserFileUrls()
        {
            // Create the list to return
            List<string> fileUrls = new List<string>(10);

            // Create the directory string
            string filesDirectory = "/Content/user_files/";

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

        } // End of the GetUserFileUrls method

        #endregion

    } // End of the class

} // End of the namespace