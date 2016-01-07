using System;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of inspiration image maps
    /// </summary>
    [ValidateInput(false)] 
    public class admin_inspirationController : Controller
    {
        #region View methods

        // Get the list of image maps
        // GET: /admin_inspiration/
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
        // POST: /admin_inspiration/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_inspiration?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_inspiration/edit/1?returnUrl=?kw=df&so=ASC
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

            // Add data to the view
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.InspirationImageMap = InspirationImageMap.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty image map post if the image map does not exist
            if (ViewBag.InspirationImageMap == null)
            {
                // Add data to the view
                ViewBag.InspirationImageMap = new InspirationImageMap();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the image map form
        // GET: /admin_inspiration/image_map/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult image_map(Int32 id = 0, string returnUrl = "")
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

            // Add data to the view
            ViewBag.Keywords = "";
            ViewBag.CurrentPage = 1;
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.InspirationImageMap = InspirationImageMap.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.InspirationImageMap != null)
            {
                // Return the image view
                return View("image_map");
            }
            else
            {
                return Redirect("/admin_inspiration" + returnUrl);
            }

        } // End of the image_map method

        #endregion

        #region Post methods

        // Update the image map
        // POST: /admin_inspiration/edit
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
            Int32 language_id = Convert.ToInt32(collection["selectLanguage"]);
            string name = collection["txtName"];
            Int32 category_id = Convert.ToInt32(collection["selectCategory"]);
            HttpPostedFileBase inspirationImage = Request.Files["uploadInspirationImage"];

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Get the image map
            InspirationImageMap imageMap = InspirationImageMap.GetOneById(id);
            bool postExists = true;

            // Check if the image map exists
            if (imageMap == null)
            {
                // Create an empty image map
                imageMap = new InspirationImageMap();
                postExists = false;
            }

            // Update values
            imageMap.language_id = language_id;
            imageMap.name = name;
            imageMap.category_id = category_id;

            // Set the image name
            if (inspirationImage.ContentLength > 0)
            {
                imageMap.image_name = System.IO.Path.GetFileName(inspirationImage.FileName);  
            }

            // Create a error message
            string errorMessage = "";

            // Check for errors in the image map
            if (imageMap.name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }
            if (imageMap.image_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("file_name"), "100") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == "")
            {
                // Check if we should add or update the image map
                if (postExists == false)
                {
                    // Add the image map
                    Int64 insertId = InspirationImageMap.Add(imageMap);
                    imageMap.id = Convert.ToInt32(insertId);
                }
                else
                {
                    // Update the image map
                    InspirationImageMap.Update(imageMap);
                }

                // Update the image
                if (inspirationImage.ContentLength > 0)
                {
                    UpdateInspirationImage(imageMap, inspirationImage);
                }

                // Redirect the user to the list
                return Redirect("/admin_inspiration" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "id", "ASC");
                ViewBag.InspirationImageMap = imageMap;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Update the image map
        // POST: /admin_inspiration/image_map
        [HttpPost]
        public ActionResult image_map(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get all the form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string image_map_points = collection["hiddenImageMapPoints"];
            string keywords = collection["txtSearch"];
            Int32 currentPage = Convert.ToInt32(collection["hiddenPage"]);
            string returnUrl = collection["returnUrl"];

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

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Get the image map
            InspirationImageMap imageMap = InspirationImageMap.GetOneById(id);

            // Update values
            imageMap.image_map_points = image_map_points;

            // Check if the user wants to do a search
            if (collection["btnSearch"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = 1;
                ViewBag.InspirationImageMap = imageMap;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("image_map");
            }

            // Check if the user wants to do a search
            if (collection["btnPreviousPage"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage - 1;
                ViewBag.InspirationImageMap = imageMap;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("image_map");
            }

            // Check if the user wants to do a search
            if (collection["btnNextPage"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage + 1;
                ViewBag.InspirationImageMap = imageMap;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("image_map");
            }

            // Check if there is errors
            if (collection["btnSearchProducts"] == null)
            {
                // Update the image map
                InspirationImageMap.Update(imageMap);

                // Redirect the user to the list
                return Redirect("/admin_inspiration" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = 1;
                ViewBag.TranslatedTexts = tt;
                ViewBag.InspirationImageMap = imageMap;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("image_map");
            }

        } // End of the image_map method

        // Delete the image map
        // POST: /admin_inspiration/delete/1?returnUrl=?kw=sok&qp=2
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

            // Get the image map
            InspirationImageMap imageMap = InspirationImageMap.GetOneById(id);

            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the image map post and all the connected posts (CASCADE)
            errorCode = InspirationImageMap.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Delete the image
            if (imageMap != null)
            {
                DeleteInspirationImage(imageMap.id);
            }

            // Redirect the user to the list
            return Redirect("/admin_inspiration" + returnUrl);

        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Update the image map image
        /// </summary>
        /// <param name="imageMap">A reference to the image map</param>
        /// <param name="inspirationImage">The uploaded image</param>
        private void UpdateInspirationImage(InspirationImageMap imageMap, HttpPostedFileBase inspirationImage)
        {
            // Create the directory string
            string inspirationImageDirectory = Tools.GetInspirationImageDirectoryUrl(imageMap.id);

            // Delete the old image
            DeleteInspirationImage(imageMap.id);

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(inspirationImageDirectory)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(inspirationImageDirectory));
            }

            // Save the new image
            inspirationImage.SaveAs(Server.MapPath(inspirationImageDirectory + imageMap.image_name));

        } // End of the UpdateInspirationImage method

        /// <summary>
        /// Delete the inspiration image directory
        /// </summary>
        /// <param name="directoryUrl">The directory url</param>
        private void DeleteInspirationImage(Int32 imageMapId)
        {
            // Create the server file path
            string serverFilePath = Server.MapPath("/Content/inspiration/" + (imageMapId / 100).ToString() + "/" + imageMapId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(serverFilePath))
            {
                System.IO.Directory.Delete(serverFilePath, true);
            }

        } // End of the DeleteInspirationImage method

        #endregion

    } // End of the class

} // End of the namespace