using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of customers
    /// </summary>
    [ValidateInput(false)]
    public class admin_customersController : Controller
    {
        #region View methods

        // Get the list of customers
        // GET: /admin_customers/
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
        // POST: /admin_customers/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_customers?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_customers/edit/1?returnUrl=?kw=df&so=ASC
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
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.Countries = Country.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.Customer = Customer.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;
    
            // Create a new empty customer post if the customer does not exist
            if (ViewBag.Customer == null)
            {
                // Add data to the view
                ViewBag.Customer = new Customer();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Log in as the customer
        // GET: /admin_customers/log_in_as/5?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult log_in_as(Int32 id = 0, string returnUrl = "")
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

            // Get webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();
            string redirectHttps = webshopSettings.Get("REDIRECT-HTTPS");

            // Create the customer cookie
            HttpCookie customerCookie = new HttpCookie("CustomerCookie");
            customerCookie.Value = Tools.ProtectCookieValue(id.ToString(), "CustomerLogin");
            customerCookie.Expires = DateTime.UtcNow.AddDays(1);
            customerCookie.HttpOnly = true;
            customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
            Response.Cookies.Add(customerCookie);

            // Redirect the user to the start page
            return RedirectToAction("index", "home");

        } // End of the log_in_as method

        // Get the customer files form
        // GET: /admin_customers/files/5?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult files(Int32 id = 0, string returnUrl = "")
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
            ViewBag.Customer = Customer.GetOneById(id);
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Make sure that the customer not is null
            if (ViewBag.Customer == null)
            {
                return Redirect("/admin_customers" + returnUrl);
            }

            // Return the view
            return View("files");

        } // End of the files form

        #endregion

        #region Post methods

        // Update the customer
        // POST: /admin_customers/edit
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
            Int32 language_id = Convert.ToInt32(collection["selectLanguage"]);
            string email = collection["txtEmail"];
            string password = collection["txtPassword"];
            byte customerType = Convert.ToByte(collection["customerType"]);
            string orgNumber = collection["txtOrgNumber"];
            string vatNumber = collection["txtVatNumber"];
            string contactName = collection["txtContactName"];
            string phoneNumber = collection["txtPhoneNumber"];
            string mobilePhoneNumber = collection["txtMobilePhoneNumber"];
            string invoiceName = collection["txtInvoiceName"];
            string invoiceAddress1 = collection["txtInvoiceAddress1"];
            string invoiceAddress2 = collection["txtInvoiceAddress2"];
            string invoicePostCode = collection["txtInvoicePostCode"];
            string invoiceCity = collection["txtInvoiceCity"];
            Int32 invoiceCountry = Convert.ToInt32(collection["selectInvoiceCountry"]);
            string deliveryName = collection["txtDeliveryName"];
            string deliveryAddress1 = collection["txtDeliveryAddress1"];
            string deliveryAddress2 = collection["txtDeliveryAddress2"];
            string deliveryPostCode = collection["txtDeliveryPostCode"];
            string deliveryCity = collection["txtDeliveryCity"];
            Int32 deliveryCountry = Convert.ToInt32(collection["selectDeliveryCountry"]);
            bool wantNewsletter = Convert.ToBoolean(collection["cbNewsletter"]);
            string facebook_user_id = collection["txtFacebookUserId"];
            string google_user_id = collection["txtGoogleUserId"];

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the customer
            Customer customer = Customer.GetOneById(id);

            // Check if the customer exists
            if (customer == null)
            {
                // Create an empty customer
                customer = new Customer();
            }

            // Update values
            customer.email = email.Length > 100 ? email.Substring(0, 100) : email;
            customer.language_id = language_id;
            customer.org_number = orgNumber.Length > 20 ? orgNumber.Substring(0, 20) : orgNumber;
            customer.customer_type = customerType;
            customer.vat_number = vatNumber.Length > 20 ? vatNumber.Substring(0, 20) : vatNumber;
            customer.contact_name = contactName.Length > 100 ? contactName.Substring(0, 100) : contactName;
            customer.phone_number = phoneNumber.Length > 100 ? phoneNumber.Substring(0, 100) : phoneNumber;
            customer.mobile_phone_number = mobilePhoneNumber.Length > 100 ? mobilePhoneNumber.Substring(0, 100) : mobilePhoneNumber;
            customer.invoice_name = invoiceName.Length > 100 ? invoiceName.Substring(0, 100) : invoiceName;
            customer.invoice_address_1 = invoiceAddress1.Length > 100 ? invoiceAddress1.Substring(0, 100) : invoiceAddress1;
            customer.invoice_address_2 = invoiceAddress2.Length > 100 ? invoiceAddress2.Substring(0, 100) : invoiceAddress2;
            customer.invoice_post_code = invoicePostCode.Length > 100 ? invoicePostCode.Substring(0, 100) : invoicePostCode;
            customer.invoice_city = invoiceCity.Length > 100 ? invoiceCity.Substring(0, 100) : invoiceCity;
            customer.invoice_country = invoiceCountry;
            customer.delivery_name = deliveryName.Length > 100 ? deliveryName.Substring(0, 100) : deliveryName;
            customer.delivery_address_1 = deliveryAddress1.Length > 100 ? deliveryAddress1.Substring(0, 100) : deliveryAddress1;
            customer.delivery_address_2 = deliveryAddress2.Length > 100 ? deliveryAddress2.Substring(0, 100) : deliveryAddress2;
            customer.delivery_post_code = deliveryPostCode.Length > 100 ? deliveryPostCode.Substring(0, 100) : deliveryPostCode;
            customer.delivery_city = deliveryCity.Length > 100 ? deliveryCity.Substring(0, 100) : deliveryCity;
            customer.delivery_country = deliveryCountry;
            customer.newsletter = wantNewsletter;
            customer.facebook_user_id = facebook_user_id;
            customer.google_user_id = google_user_id;

            // Create a error message
            string errorMessage = string.Empty;

            // Get the customer on email
            Customer customerOnEmail = Customer.GetOneByEmail(customer.email);

            // Check for errors
            if(customer.language_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("language").ToLower()) + "<br/>";
            }
            if (customer.invoice_country == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("invoice_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
            }
            if (customer.delivery_country == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("delivery_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
            }
            if (customerOnEmail != null && customer.id != customerOnEmail.id)
            {
                errorMessage += "&#149; " + tt.Get("error_email_unique") + "<br/>";
            }
            if (AnnytabDataValidation.IsEmailAddressValid(customer.email) == null)
            {
                errorMessage += "&#149; " + tt.Get("error_email_valid") + "<br/>";
            }
            if (customer.facebook_user_id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Facebook user id", "50") + "<br/>";
            }
            if (customer.google_user_id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Google user id", "50") + "<br/>";
            }
           
            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the customer
                if (customer.id == 0)
                {
                    // Add the customer
                    Int32 insertId = (Int32)Customer.Add(customer);
                    Customer.UpdatePassword(insertId, PasswordHash.CreateHash(password));        
                }
                else
                {
                    // Update the customer
                    Customer.Update(customer);

                    // Only update the password if it has changed
                    if (password != "")
                    {
                        Customer.UpdatePassword(customer.id, PasswordHash.CreateHash(password));
                    }
                }

                // Redirect the user to the list
                return Redirect("/admin_customers" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Customer = customer;
                ViewBag.TranslatedTexts = tt;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.Countries = Country.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Export email addresses to a text file
        // POST: /admin_customers/export_email_addresses
        [HttpPost]
        public ActionResult export_email_addresses(FormCollection collection)
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
            Int32 languageId = Convert.ToInt32(collection["selectLanguage"]);

            // Create a stream writer
            MemoryStream outputStream = null;

            // Create the file stream result
            FileStreamResult fileStreamResult = null;

            try
            {
                // Get email addresses
                string emailAddresses = Customer.GetEmailAddresses(languageId, false);

                // Get the stream
                byte[] streamData = System.Text.Encoding.UTF8.GetBytes(emailAddresses);

                // Write to the stream
                outputStream = new MemoryStream(streamData);

                // Go to the beginning of the output stream
                outputStream.Seek(0, SeekOrigin.Begin);

                // Create the file stream result
                fileStreamResult = new FileStreamResult(outputStream, "text/plain") { FileDownloadName = "email_addresses.txt" };

            }
            catch (Exception exception)
            {
                string exceptionMessage = "";
                exceptionMessage = exception.Message;
            }

            // Return the text file
            return fileStreamResult;

        } // End of the export_email_addresses method

        // Delete a customer file
        // POST: /admin_customers/delete_file?customerId=5&productId=4&returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult delete_file(Int32 customerId = 0, Int32 productId = 0, string returnUrl = "")
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

            // Delete the customer file and all the connected posts (CASCADE)
            errorCode = CustomerFile.DeleteOnId(customerId, productId);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_customers" + returnUrl);

        } // End of the delete_file method

        // Delete the customer
        // POST: /admin_customers/delete/5?returnUrl=?kw=df&so=ASC
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

            // Delete the customer post and all the connected posts (CASCADE)
            errorCode = Customer.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_customers" + returnUrl);

        } // End of the delete method

        #endregion

	} // End of the class

} // End of the namespace