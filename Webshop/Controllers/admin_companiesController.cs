using System;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of companies
    /// </summary>
    [ValidateInput(false)]
    public class admin_companiesController : Controller
    {
        #region View methods

        // Get the list of companies
        // GET: /admin_companies/
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
        // POST: /admin_companies/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_companies?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_companies/edit/1?returnUrl=?kw=df&so=ASC
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
            ViewBag.Company = Company.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty company post if the company does not exist
            if (ViewBag.Company == null)
            {
                // Add data to the view
                ViewBag.Company = new Company();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        #endregion

        #region Post methods

        // Update the company
        // POST: /admin_companies/edit
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
            string registered_office = collection["txtRegisteredOffice"];
            string org_number = collection["txtOrgNumber"];
            string vat_number = collection["txtVatNumber"];
            string phone_number = collection["txtPhoneNumber"];
            string mobile_phone_number = collection["txtMobilePhoneNumber"];
            string email = collection["txtEmail"];
            string post_address_1 = collection["txtPostAddress1"];
            string post_address_2 = collection["txtPostAddress2"];
            string post_code = collection["txtPostCode"];
            string post_city = collection["txtPostCity"];
            string post_country = collection["txtPostCountry"];

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList translatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the company
            Company company = Company.GetOneById(id);

            // Check if the company exists
            if (company == null)
            {
                // Create an empty company
                company = new Company();
            }

            // Update values
            company.name = name.Length > 100 ? name.Substring(0, 100) : name;
            company.registered_office = registered_office.Length > 100 ? registered_office.Substring(0, 100) : registered_office;
            company.org_number = org_number.Length > 100 ? org_number.Substring(0, 100) : org_number;
            company.vat_number = vat_number.Length > 100 ? vat_number.Substring(0, 100) : vat_number;
            company.phone_number = phone_number.Length > 100 ? phone_number.Substring(0, 100) : phone_number;
            company.mobile_phone_number = mobile_phone_number.Length > 100 ? mobile_phone_number.Substring(0, 100) : mobile_phone_number;
            company.email = email.Length > 100 ? email.Substring(0, 100) : email;
            company.post_address_1 = post_address_1.Length > 100 ? post_address_1.Substring(0, 100) : post_address_1;
            company.post_address_2 = post_address_2.Length > 100 ? post_address_2.Substring(0, 100) : post_address_2;
            company.post_code = post_code.Length > 100 ? post_code.Substring(0, 100) : post_code;
            company.post_city = post_city.Length > 100 ? post_city.Substring(0, 100) : post_city;
            company.post_country = post_country.Length > 100 ? post_country.Substring(0, 100) : post_country;

            // Create a error message
            string errorMessage = string.Empty;

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the company
                if (company.id == 0)
                {
                    // Add the company
                    Company.Add(company);
                }
                else
                {
                    // Update the company
                    Company.Update(company);
                }

                // Redirect the user to the list
                return Redirect("/admin_companies" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Company = company;
                ViewBag.TranslatedTexts = translatedTexts;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the company
        // POST: /admin_companies/delete/1?returnUrl=?kw=df&so=ASC
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

            // Delete the company post and all the connected posts (CASCADE)
            errorCode = Company.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_companies" + returnUrl);

        } // End of the delete method

        #endregion

    } // End of the class

} // End of the namespace