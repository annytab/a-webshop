using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls customer interaction
    /// </summary>
    public class customerController : Controller
    {
        #region View methods

        // Get the customer start page (my pages)
        // GET: /customer/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            Customer currentCustomer = Customer.GetSignedInCustomer();

            // Check if the customer is signed in
            if (currentCustomer == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.Customer = currentCustomer;
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/customer_start_page.cshtml");

        } // End of the index method

        // Get the customer login page
        // GET: /customer/login
        [HttpGet]
        public ActionResult login()
        {
            // Get the current domain and
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("log_in"), "/customer/login"));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.Customer = new Customer();
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/customer_login.cshtml");

        } // End of the login method

        // Redirect the customer to the facebook login
        // GET: /customer/facebook_login
        [HttpGet]
        public ActionResult facebook_login()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Create a random state
            string state = Tools.GeneratePassword();
            Session["FacebookState"] = state;

            // Create the url
            string url = "https://www.facebook.com/dialog/oauth?client_id=" + domain.facebook_app_id + "&state=" + state
                + "&response_type=code&redirect_uri=" + Server.UrlEncode(domain.web_address + "/customer/facebook_login_callback");

            // Redirect the customer
            return Redirect(url);

        } // End of the facebook_login method

        // Login the customer with facebook
        // GET: /customer/facebook_login_callback
        [HttpGet]
        public async Task<ActionResult> facebook_login_callback()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the state
            string state = "";
            if (Request.Params["state"] != null)
            {
                state = Server.UrlDecode(Request.Params["state"]);
            }

            // Get the state stored in the session
            string sessionState = "";
            if (Session["FacebookState"] != null)
            {
                sessionState = Session["FacebookState"].ToString();
            }

            // Get the code
            string code = "";
            if (Request.Params["code"] != null)
            {
                code = Server.UrlDecode(Request.Params["code"]);
            }

            // Make sure that the callback is valid
            if (state != sessionState || code == "")
            {
                return Redirect("/");
            }

            // Get the access token
            string access_token = await AnnytabExternalLogin.GetFacebookAccessToken(domain, code);

            // Get the facebook user
            Dictionary<string, object> facebookUser = await AnnytabExternalLogin.GetFacebookUser(domain, access_token);
                
            // Get the facebook data
            string facebookId = facebookUser.ContainsKey("id") == true ? facebookUser["id"].ToString() : "";
            string facebookName = facebookUser.ContainsKey("name") == true ? facebookUser["name"].ToString() : "";
            string facebookEmail = facebookId + "_facebook";

            // Get webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();
            string redirectHttps = webshopSettings.Get("REDIRECT-HTTPS");

            // Get the signed in customer
            Customer customer = Customer.GetSignedInCustomer();

            // Check if the customer exists or not
            if (facebookId != "" && customer != null)
            {
                // Update the customer
                customer.facebook_user_id = facebookId;
                Customer.Update(customer);

                // Redirect the customer to his start page
                return RedirectToAction("index", "customer");
            }
            else if (facebookId != "" && customer == null)
            {
                // Check if we can find a customer with the facebook id
                customer = Customer.GetOneByFacebookUserId(facebookId);

                // Check if the customer exists
                if (customer == null)
                {
                    // Create a new customer
                    customer = new Customer();
                    customer.email = facebookEmail;
                    customer.customer_type = 0;
                    customer.language_id = domain.front_end_language;
                    customer.contact_name = AnnytabDataValidation.TruncateString(facebookName, 100);
                    customer.customer_password = PasswordHash.CreateHash(Tools.GeneratePassword());
                    customer.invoice_name = AnnytabDataValidation.TruncateString(facebookName, 100);
                    customer.invoice_country = domain.country_id;
                    customer.delivery_name = AnnytabDataValidation.TruncateString(facebookName, 100);
                    customer.delivery_country = domain.country_id;
                    customer.facebook_user_id = facebookId;

                    // Add the new customer
                    Int64 insertId = Customer.Add(customer);
                    Customer.UpdatePassword((Int32)insertId, PasswordHash.CreateHash(customer.customer_password));

                    // Create the customer cookie
                    HttpCookie customerCookie = new HttpCookie("CustomerCookie");
                    customerCookie.Value = Tools.ProtectCookieValue(insertId.ToString(), "CustomerLogin");
                    customerCookie.Expires = DateTime.UtcNow.AddDays(1);
                    customerCookie.HttpOnly = true;
                    customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(customerCookie);

                    // Redirect the user to the edit customer data method
                    return Redirect("/customer/edit_company");
                }
                else
                {
                    // Create the customer cookie
                    HttpCookie customerCookie = new HttpCookie("CustomerCookie");
                    customerCookie.Value = Tools.ProtectCookieValue(customer.id.ToString(), "CustomerLogin");
                    customerCookie.Expires = DateTime.UtcNow.AddDays(1);
                    customerCookie.HttpOnly = true;
                    customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(customerCookie);

                    // Redirect the user to the check out
                    return Redirect("/order");
                }
            }
            else
            {
                // Redirect the user to the login
                return RedirectToAction("login", "customer");
            }

        } // End of the facebook_login_callback method

        // Redirect the customer to the google login
        // GET: /customer/google_login
        [HttpGet]
        public ActionResult google_login()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Create a random state
            string state = Tools.GeneratePassword();
            Session["GoogleState"] = state;

            // Create the url
            string url = "https://accounts.google.com/o/oauth2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fplus.me&state=" + state
                + "&redirect_uri=" + Server.UrlEncode(domain.web_address + "/customer/google_login_callback") + "&response_type=code&client_id=" + domain.google_app_id
                + "&access_type=offline";

            // Redirect the customer
            return Redirect(url);

        } // End of the google_login method

        // Handle the google login callback
        // GET: /customer/google_login_callback
        [HttpGet]
        public async Task<ActionResult> google_login_callback()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the state
            string state = "";
            if (Request.Params["state"] != null)
            {
                state = Server.UrlDecode(Request.Params["state"]);
            }

            // Get the state stored in the session
            string sessionState = "";
            if(Session["GoogleState"] != null)
            {
                sessionState = Session["GoogleState"].ToString();
            }

            // Get the code
            string code = "";
            if (Request.Params["code"] != null)
            {
                code = Server.UrlDecode(Request.Params["code"]);
            }

             // Check if this is a valid callback
            if (state != sessionState || code == "")
            {
                // Redirect the customer
                return Redirect("/");
            }

            // Get the access token
            string access_token = await AnnytabExternalLogin.GetGoogleAccessToken(domain, code);

            // Get the google user
            Dictionary<string, object> googleUser = await AnnytabExternalLogin.GetGoogleUser(domain, access_token);

            // Get the google data
            string googleId = googleUser.ContainsKey("id") == true ? googleUser["id"].ToString() : "";
            string googleName = googleUser.ContainsKey("displayName") == true ? googleUser["displayName"].ToString() : "";

            // Get webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();
            string redirectHttps = webshopSettings.Get("REDIRECT-HTTPS");

            // Get the signed in customer
            Customer customer = Customer.GetSignedInCustomer();

            // Check if the customer exists or not
            if (googleId != "" && customer != null )
            {
                // Update the customer
                customer.google_user_id = googleId;
                Customer.Update(customer);

                // Redirect the customer to his start page
                return RedirectToAction("index", "customer");
            }
            else if (googleId != "" && customer == null)
            {
                // Check if we can find a customer with the google id
                customer = Customer.GetOneByGoogleUserId(googleId);

                // Check if the customer exists
                if (customer == null)
                {
                    // Create a new customer
                    customer = new Customer();
                    customer.email = googleId + "_google";
                    customer.customer_type = 0;
                    customer.language_id = domain.front_end_language;
                    customer.contact_name = AnnytabDataValidation.TruncateString(googleName, 100);
                    customer.customer_password = PasswordHash.CreateHash(Tools.GeneratePassword());
                    customer.invoice_name = AnnytabDataValidation.TruncateString(googleName, 100);
                    customer.invoice_country = domain.country_id;
                    customer.delivery_name = AnnytabDataValidation.TruncateString(googleName, 100);
                    customer.delivery_country = domain.country_id;
                    customer.google_user_id = googleId;

                    // Add the new customer
                    Int64 insertId = Customer.Add(customer);
                    Customer.UpdatePassword((Int32)insertId, PasswordHash.CreateHash(customer.customer_password));

                    // Create the customer cookie
                    HttpCookie customerCookie = new HttpCookie("CustomerCookie");
                    customerCookie.Value = Tools.ProtectCookieValue(insertId.ToString(), "CustomerLogin");
                    customerCookie.Expires = DateTime.UtcNow.AddDays(1);
                    customerCookie.HttpOnly = true;
                    customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(customerCookie);

                    // Redirect the user to the edit company page
                    return Redirect("/customer/edit_company");
                }
                else
                {
                    // Create the customer cookie
                    HttpCookie customerCookie = new HttpCookie("CustomerCookie");
                    customerCookie.Value = Tools.ProtectCookieValue(customer.id.ToString(), "CustomerLogin");
                    customerCookie.Expires = DateTime.UtcNow.AddDays(1);
                    customerCookie.HttpOnly = true;
                    customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(customerCookie);

                    // Redirect the user to the check out
                    return Redirect("/order");
                }
            }
            else
            {
                // Redirect the user to the login
                return RedirectToAction("login", "customer");
            }

        } // End of the google_login_callback method

        // Get the customer forgot email password page
        // GET: /customer/forgot_email_password
        [HttpGet]
        public ActionResult forgot_email_password()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("forgot") + " " + tt.Get("email") + "/" + tt.Get("password"), "/customer/forgot_email_password"));

            // Set values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.Customer = new Customer();
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/forgot_email_password.cshtml");

        } // End of the forgot_email_password method

        // Get the edit form
        // GET: /customer/edit_company
        [HttpGet]
        public ActionResult edit_company()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Create the customer
            Customer currentCustomer = Customer.GetSignedInCustomer();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("address_information").ToLower(), "/customer/edit_company"));

            // Add data to the view
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.TranslatedTexts = tt;
            ViewBag.Customer = currentCustomer;
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Create a new empty customer post if the customer does not exist
            if (ViewBag.Customer == null)
            {
                // Add data to the view
                Customer customer = new Customer();
                customer.customer_type = 0;
                ViewBag.Customer = customer;
            }

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/edit_company.cshtml");

        } // End of the edit_company method

        // Get the edit form for a private person
        // GET: /customer/edit_person
        [HttpGet]
        public ActionResult edit_person()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Create the customer
            Customer currentCustomer = Customer.GetSignedInCustomer();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("address_information").ToLower(), "/customer/edit_person"));

            // Add data to the view
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.TranslatedTexts = tt;
            ViewBag.Customer = currentCustomer;
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Create a new empty customer post if the customer does not exist
            if (ViewBag.Customer == null)
            {
                // Add data to the view
                Customer customer = new Customer();
                customer.customer_type = 1;
                ViewBag.Customer = customer;
            }

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/edit_person.cshtml");

        } // End of the edit_person method

        // Get the edit reviews form
        // GET: /customer/edit_reviews
        [HttpGet]
        public ActionResult edit_reviews()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the signed in customer
            Customer currentCustomer = Customer.GetSignedInCustomer();

            // Check if there is a signed in customer
            if (currentCustomer == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("reviews").ToLower(), "/customer/edit_reviews"));

            // Add data to the view
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.TranslatedTexts = tt;
            ViewBag.Customer = currentCustomer;
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/edit_customer_reviews.cshtml");

        } // End of the edit_reviews form

        // Get the download files form
        // GET: /customer/download_files
        [HttpGet]
        public ActionResult download_files()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the signed in customer
            Customer currentCustomer = Customer.GetSignedInCustomer();

            // Check if there is a signed in customer
            if (currentCustomer == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("files"), "/customer/download_files"));

            // Add data to the view
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.TranslatedTexts = tt;
            ViewBag.Customer = currentCustomer;
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/customer_download_files.cshtml");

        } // End of the download_files method

        #endregion

        #region Post methods

        // Update the company customer
        // POST: /customer/edit_company
        [HttpPost]
        public ActionResult edit_company(FormCollection collection)
        {
            // Get all the form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            byte customerType = Convert.ToByte(collection["customerType"]);
            string email = collection["txtEmail"];
            string password = collection["txtPassword"];
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
            bool wantsNewsletter = Convert.ToBoolean(collection["cbNewsletter"]);

            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

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
            customer.language_id = currentDomain.front_end_language;
            customer.customer_type = customerType;
            customer.org_number = orgNumber.Length > 20 ? orgNumber.Substring(0, 20) : orgNumber;
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
            customer.newsletter = wantsNewsletter;

            // Create a error message
            string errorMessage = string.Empty;

            // Get the customer on email
            Customer customerOnEmail = Customer.GetOneByEmail(customer.email);

            // Check for errors
            if (customerOnEmail != null && customer.id != customerOnEmail.id)
            {
                errorMessage += "&#149; " + tt.Get("error_email_unique") + "<br/>";
            }
            if (AnnytabDataValidation.IsEmailAddressValid(customer.email) == null)
            {
                errorMessage += "&#149; " + tt.Get("error_email_valid") + "<br/>";
            }
            if (customer.invoice_country == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("invoice_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
            }
            if (customer.delivery_country == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("delivery_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == "")
            {
                // Check if we should add or update the customer
                if (customer.id == 0)
                {
                    // Add the customer
                    Int32 insertId = (Int32)Customer.Add(customer);
                    Customer.UpdatePassword(insertId, PasswordHash.CreateHash(password));

                    // Get webshop settings
                    KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();
                    string redirectHttps = webshopSettings.Get("REDIRECT-HTTPS");

                    // Create the customer cookie
                    HttpCookie customerCookie = new HttpCookie("CustomerCookie");
                    customerCookie.Value = Tools.ProtectCookieValue(customer.id.ToString(), "CustomerLogin");
                    customerCookie.Expires = DateTime.UtcNow.AddDays(1);
                    customerCookie.HttpOnly = true;
                    customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(customerCookie);
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

                // Redirect the customer to his pages
                return RedirectToAction("index");
            }
            else
            {

                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
                breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("address_information").ToLower(), "/customer/edit_company"));

                // Set form values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.CurrentCategory = new Category();
                ViewBag.CurrentDomain = currentDomain;
                ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
                ViewBag.TranslatedTexts = tt;
                ViewBag.Customer = customer;
                ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

                // Return the edit view
                return currentDomain.custom_theme_id == 0 ? View("edit_company") : View("/Views/theme/edit_company.cshtml");
            }

        } // End of the edit_company method

        // Update a person customer
        // POST: /customer/edit_person
        [HttpPost]
        public ActionResult edit_person(FormCollection collection)
        {

            // Get all the form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            byte customerType = Convert.ToByte(collection["customerType"]);
            string email = collection["txtEmail"];
            string password = collection["txtPassword"];
            string personNumber = collection["txtPersonNumber"];
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
            bool wantsNewsletter = Convert.ToBoolean(collection["cbNewsletter"]);

            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

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
            customer.language_id = currentDomain.front_end_language;
            customer.customer_type = customerType;
            customer.org_number = personNumber.Length > 20 ? personNumber.Substring(0, 20) : personNumber;
            customer.vat_number = "";
            customer.phone_number = phoneNumber.Length > 100 ? phoneNumber.Substring(0, 100) : phoneNumber;
            customer.mobile_phone_number = mobilePhoneNumber.Length > 100 ? mobilePhoneNumber.Substring(0, 100) : mobilePhoneNumber;
            customer.invoice_name = invoiceName.Length > 100 ? invoiceName.Substring(0, 100) : invoiceName;
            customer.contact_name = contactName.Length > 100 ? contactName.Substring(0, 100) : contactName;
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
            customer.newsletter = wantsNewsletter;

            // Create a error message
            string errorMessage = string.Empty;

            // Get the customer on email
            Customer customerOnEmail = Customer.GetOneByEmail(customer.email);

            // Check for errors
            if (customerOnEmail != null && customer.id != customerOnEmail.id)
            {
                errorMessage += "&#149; " + tt.Get("error_email_unique") + "<br/>";
            }
            if (AnnytabDataValidation.IsEmailAddressValid(customer.email) == null)
            {
                errorMessage += "&#149; " + tt.Get("error_email_valid") + "<br/>";
            }
            if (customer.invoice_country == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("invoice_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
            }
            if (customer.delivery_country == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("delivery_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
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

                    // Get webshop settings
                    KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();
                    string redirectHttps = webshopSettings.Get("REDIRECT-HTTPS");

                    // Create the customer cookie
                    HttpCookie customerCookie = new HttpCookie("CustomerCookie");
                    customerCookie.Value = Tools.ProtectCookieValue(customer.id.ToString(), "CustomerLogin");
                    customerCookie.Expires = DateTime.UtcNow.AddDays(1);
                    customerCookie.HttpOnly = true;
                    customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(customerCookie);
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

                // Redirect the customer to his pages
                return RedirectToAction("index");
            }
            else
            {
                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
                breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("address_information").ToLower(), "/customer/edit_person"));

                // Set form values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.CurrentCategory = new Category();
                ViewBag.CurrentDomain = currentDomain;
                ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
                ViewBag.TranslatedTexts = tt;
                ViewBag.Customer = customer;
                ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

                // Return the edit view
                return currentDomain.custom_theme_id == 0 ? View("edit_person") : View("/Views/theme/edit_person.cshtml");
            }

        } // End of the edit_person method

        // Add a review
        // POST: /customer/add_review
        [HttpPost]
        public ActionResult add_review(FormCollection collection)
        {
            // Make sure that the customer is signed in
            Customer customer = Customer.GetSignedInCustomer();

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(domain.front_end_language, "id", "ASC");

            // Check if the post request is valid
            if (customer == null || collection == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get the form data
            Int32 productId = Convert.ToInt32(collection["hiddenProductId"]);
            decimal userVote = 0;
            decimal.TryParse(collection["userVote"], NumberStyles.Any, CultureInfo.InvariantCulture, out userVote);
            string reviewText = collection["txtReviewText"];

            // Modify the review text
            reviewText = reviewText.Replace(Environment.NewLine, "<br />");

            // Get the product
            Product product = Product.GetOneById(productId, domain.front_end_language);

            // Create a new product review
            ProductReview review = new ProductReview();
            review.product_id = productId;
            review.customer_id = customer.id;
            review.language_id = domain.front_end_language;
            review.review_date = DateTime.UtcNow;
            review.rating = AnnytabDataValidation.TruncateDecimal(userVote, 0, 999999.99M);
            review.review_text = reviewText;

            // Add the product review
            Int64 insertId = ProductReview.Add(review);

            // Send a email to the administrator of the website
            string subject = tt.Get("review") + " - " + domain.webshop_name;
            string message = tt.Get("id") + ": " + insertId.ToString() + "<br />"
                + tt.Get("product") + ": " + review.product_id.ToString() + "<br />"
                + tt.Get("customer") + ": " + customer.invoice_name + "<br />" 
                + tt.Get("rating") + ": " + review.rating.ToString() + "<br /><br />" 
                + review.review_text;
            Tools.SendEmailToHost("", subject, message);

            // Update the product rating
            Product.UpdateRating(product.id, domain.front_end_language);

            // Redirect the user to the same product page
            return RedirectToAction("product", "home", new { id = product.page_name });

        } // End of the add_review method

        // Edit a review
        // POST: /customer/edit_review
        [HttpPost]
        public ActionResult edit_review(FormCollection collection)
        {
            // Get the signed in customer
            Customer customer = Customer.GetSignedInCustomer();

            // Check if the post request is valid
            if (customer == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(domain.front_end_language, "id", "ASC");

            // Get form values
            Int32 reviewId = Convert.ToInt32(collection["hiddenId"]);
            decimal userVote = 0;
            decimal.TryParse(collection["userVote"], NumberStyles.Any, CultureInfo.InvariantCulture, out userVote);
            string reviewText = collection["txtReviewText"];

            // Modify the review text
            reviewText = reviewText.Replace(Environment.NewLine, "<br />");

            // Get the review
            ProductReview review = ProductReview.GetOneById(reviewId);

            // Update the review
            if(review != null && review.customer_id == customer.id)
            {
                // Update values
                review.review_date = DateTime.UtcNow;
                review.review_text = reviewText;
                review.rating = AnnytabDataValidation.TruncateDecimal(userVote, 0, 999999.99M);

                // Update the review
                ProductReview.Update(review);

                // Send a email to the administrator of the website
                string subject = tt.Get("review") + " - " + domain.webshop_name;
                string message = tt.Get("id") + ": " + review.id.ToString() + "<br />"
                    + tt.Get("product") + ": " + review.product_id.ToString() + "<br />"
                    + tt.Get("customer") + ": " + customer.invoice_name + "<br />"
                    + tt.Get("rating") + ": " + review.rating.ToString() + "<br /><br />"
                    + review.review_text;
                Tools.SendEmailToHost("", subject, message);

                // Update the product rating
                Product.UpdateRating(review.product_id, review.language_id);
            }

            // Return the edit reviews view
            return RedirectToAction("edit_reviews");

        } // End of the edit_review method

        // Delete a review
        // POST: /customer/delete_review/1
        [HttpGet]
        public ActionResult delete_review(Int32 id = 0)
        {
            // Get the signed in customer
            Customer customer = Customer.GetSignedInCustomer();

            // Check if the post request is valid
            if (customer == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the review
            ProductReview review = ProductReview.GetOneById(id);

            // Delete the review
            if (review != null && review.customer_id == customer.id)
            {
                // Delete the review
                ProductReview.DeleteOnId(review.id);

                // Update the product rating
                Product.UpdateRating(review.product_id, review.language_id);
            }

            // Return the edit reviews view
            return RedirectToAction("edit_reviews");

        } // End of the delete_review method

        // Download the file
        // POST: /customer/download_files
        [HttpPost]
        public ActionResult download_files(FormCollection collection)
        {
            // Get the signed in customer
            Customer customer = Customer.GetSignedInCustomer();

            // Check if the post request is valid
            if (customer == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get form values
            Int32 productId = Convert.ToInt32(collection["hiddenProductId"]);
            Int32 languageId = Convert.ToInt32(collection["hiddenLanguageId"]);
            string fileName = collection["hiddenFileName"];

            // Get the customer file and the product
            CustomerFile customerFile = CustomerFile.GetOneById(customer.id, productId, languageId);
            Product product = Product.GetOneById(productId, languageId);

            // Make sure that the customer file or the product not is null
            if(customerFile == null || product == null)
            {
                return RedirectToAction("index", "home");
            }

            // Check if we should change the language
            languageId = product.use_local_files == true ? languageId : 0;

            // Create the file path
            string filePath = Server.MapPath("/Content/images/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/dc_files/" + fileName);

            // Check if the file exists
            if(System.IO.File.Exists(filePath))
            {
                return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            // Return the user to the file download page
            return RedirectToAction("download_files");

        } // End of the download_files method

        // Sign in the customer
        // POST: /customer/login
        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            // Get the data from the form
            string returnUrl = collection["hiddenReturnUrl"];
            string email = collection["txtEmail"];
            string password = collection["txtPassword"];

            // Get the customer
            Customer customer = Customer.GetOneByEmail(email);

            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get translated texts
            KeyStringList translatedTexts = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Check if the customer exists and if the password is correct
            if (customer != null && Customer.ValidatePassword(email, password) == true)
            {
                // Get webshop settings
                KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();
                string redirectHttps = webshopSettings.Get("REDIRECT-HTTPS");

                // Create the customer cookie
                HttpCookie customerCookie = new HttpCookie("CustomerCookie");
                customerCookie.Value = Tools.ProtectCookieValue(customer.id.ToString(), "CustomerLogin");
                customerCookie.Expires = DateTime.UtcNow.AddDays(1);
                customerCookie.HttpOnly = true;
                customerCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                Response.Cookies.Add(customerCookie);

                // Redirect the user to the checkout page
                return Redirect(returnUrl);
            }
            else
            {
                // Create a new customer
                customer = new Customer();
                customer.email = email;
                string error_message = "&#149; " + translatedTexts.Get("error_login");

                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("my_pages"), "/customer"));
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("log_in"), "/customer/login"));

                // Set values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.CurrentCategory = new Category();
                ViewBag.TranslatedTexts = translatedTexts;
                ViewBag.CurrentDomain = currentDomain;
                ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
                ViewBag.Customer = customer;
                ViewBag.ErrorMessage = error_message;
                ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

                // Return the login view
                return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/customer_login.cshtml");
            }

        } // End of the login method

        // Send an email to the customer with a random password
        // GET: /customer/forgot_email_password
        [HttpPost]
        public ActionResult forgot_email_password(FormCollection collection)
        {
            // Get form data
            string email = collection["txtEmail"];
            string orgPersonNumber = collection["txtOrgPersonNumber"];

            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get translated texts
            KeyStringList translatedTexts = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Get the customer
            Customer customer = Customer.GetOneByEmail(email);

            // Create a random password
            string password = Tools.GeneratePassword();
            
            // Create a error message
            string error_message = string.Empty;

            // Check if the customer exists
            if(customer != null)
            {
                // Create the mail message
                string subject = translatedTexts.Get("forgot") + " " + translatedTexts.Get("email") + "/" + translatedTexts.Get("password");
                string message = translatedTexts.Get("email") + ": " + customer.email + "<br />" + translatedTexts.Get("password") + ": " + password + "<br />";

                // Try to send the email message
                if(Tools.SendEmailToCustomer(customer.email, subject, message) == false)
                {
                    error_message += "&#149; " + translatedTexts.Get("error_send_email");
                }
            }
            else
            {
                // Try to get the customer on organization number
                customer = Customer.GetOneByOrgNumber(orgPersonNumber);

                if(customer != null)
                {
                    // Create the mail message
                    string subject = translatedTexts.Get("forgot") + " " + translatedTexts.Get("email") + "/" + translatedTexts.Get("password");
                    string message = translatedTexts.Get("email") + ": " + customer.email + "<br />" + translatedTexts.Get("password") + ": " + password + "<br />";

                    // Try to send the email message
                    if (Tools.SendEmailToCustomer(customer.email, subject, message) == false)
                    {
                        error_message += "&#149; " + translatedTexts.Get("error_send_email");
                    }
                }
                else
                {
                    error_message += "&#149; " + translatedTexts.Get("error_customer_exists");
                } 
            }

            // Check if there is a error message
            if (error_message == string.Empty)
            {
                // Update the password
                Customer.UpdatePassword(customer.id, PasswordHash.CreateHash(password));

                // Redirect the user to the login page
                return RedirectToAction("login");
            }
            else
            {
                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("my_pages"), "/customer"));
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("forgot") + " " + translatedTexts.Get("email") + "/" + translatedTexts.Get("password"), "/customer/forgot_email_password"));

                // Set values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.CurrentCategory = new Category();
                ViewBag.TranslatedTexts = translatedTexts;
                ViewBag.CurrentDomain = currentDomain;
                ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
                ViewBag.Customer = new Customer();
                ViewBag.ErrorMessage = error_message;
                ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

                // Return the view
                return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/forgot_email_password.cshtml");
            }

        } // End of the forgot_email_password method

        // Sign out the customer
        // GET: /customer/logout
        [HttpGet]
        public ActionResult logout()
        {
            // Delete the customer cookie
            HttpCookie customerCookie = new HttpCookie("CustomerCookie");
            customerCookie.Value = "";
            customerCookie.Expires = DateTime.UtcNow.AddDays(-1);
            Response.Cookies.Add(customerCookie);

            // Redirect the user to the login page
            return RedirectToAction("index", "home");

        } // End of the logout method

        #endregion

    } // End of the class

} // End of the namespace