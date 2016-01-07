using System;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of administrators
    /// </summary>
    [ValidateInput(false)] 
    public class admin_administratorsController : Controller
    {
        #region View methods

        // Get the list of administrators
        // GET: /admin_administrators/
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
        // POST: /admin_administrators/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_administrators?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_administrators/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the view
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.Administrator = Administrator.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty administrator post if the administrator does not exist
            if (ViewBag.Administrator == null)
            {
                // Add data to the view
                ViewBag.Administrator = new Administrator();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        #endregion

        #region Post methods

        // Update the administrator
        // POST: /admin_administrators/edit
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

            // Get all the form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string userName = collection["txtUserName"];
            string password = collection["txtPassword"];
            string role = collection["selectAdminRole"];

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the administrator
            Administrator administrator = Administrator.GetOneById(id);
            bool postExists = true;

            // Check if the administrator exists
            if (administrator == null)
            {
                // Create an empty administrator
                administrator = new Administrator();
                postExists = false;
            }

            // Update values
            administrator.admin_user_name = userName;
            administrator.admin_role = role;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a administrator on user name
            Administrator adminOnUserName = Administrator.GetOneByUserName(userName);

            // Check for errors in the administrator
            if (adminOnUserName != null && administrator.id != adminOnUserName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_unique"), tt.Get("user_name")) + "<br/>";
            }
            if (administrator.admin_user_name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("user_name"), "50") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the administrator
                if (postExists == false)
                {
                    // Add the administrator
                    Int32 insertId = (Int32)Administrator.Add(administrator);
                    Administrator.UpdatePassword(insertId, PasswordHash.CreateHash(password));
                }
                else
                {
                    // Update the administrator
                    Administrator.Update(administrator);

                    // Only update the password if it has changed
                    if (password != "")
                    {
                        Administrator.UpdatePassword(administrator.id, PasswordHash.CreateHash(password));
                    }
                }

                // Redirect the user to the list
                return Redirect("/admin_administrators" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Administrator = administrator;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the administrator
        // POST: /admin_administrators/delete/1?returnUrl=?kw=sok&qp=2
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

            // Delete the administrator post and all the connected posts (CASCADE)
            errorCode = Administrator.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_administrators" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace