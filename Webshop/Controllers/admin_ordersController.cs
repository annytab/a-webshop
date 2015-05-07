using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of orders
    /// </summary>
    [ValidateInput(false)]
    public class admin_ordersController : Controller
    {
        #region View methods

        // Get the list of orders
        // GET: /admin_orders/
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
        // POST: /admin_orders/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_orders?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_orders/edit/1?returnUrl=?kw=df&so=ASC
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
            ViewBag.Order = Order.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Redirect the user to the index page if the order does not exist
            if (ViewBag.Order == null)
            {
                // Redirect the user to the index page
                return Redirect("/admin_orders" + returnUrl);
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the order to print
        // GET: /admin_orders/print
        [HttpGet]
        public ActionResult print(Int32 id = 0)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

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

            // Get the order
            Order order = Order.GetOneById(id);

            // Check if the order is null
            if (order == null)
            {
                return RedirectToAction("index");
            }

            // Set form data
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.Order = order;
            ViewBag.OrderRows = OrderRow.GetByOrderId(order.id);
            ViewBag.InvoiceCountry = Country.GetOneById(order.invoice_country_id, adminLanguageId);
            ViewBag.DeliveryCountry = Country.GetOneById(order.delivery_country_id, adminLanguageId);
            ViewBag.PaymentOption = PaymentOption.GetOneById(order.payment_option, adminLanguageId);
            ViewBag.Company = Company.GetOneById(order.company_id);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.CultureInfo = Tools.GetCultureInfo(Language.GetOneById(currentDomain.back_end_language));

            // Return the view
            return View();

        } // End of the print method

        // Get gift cards for the order
        // GET: /admin_orders/gift_cards/5?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult gift_cards(Int32 id = 0, string returnUrl = "")
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

            // Get the order
            Order order = Order.GetOneById(id);

            // Check if the order is null
            if (order == null)
            {
                return RedirectToAction("index");
            }

            // Set form data
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.Order = order;
            ViewBag.OrderGiftCards = OrderGiftCard.GetByOrderId(order.id, "amount", "ASC");
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.CultureInfo = Tools.GetCultureInfo(Language.GetOneById(currentDomain.back_end_language));
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            return View();

        } // End of the gift_cards method

        #endregion

        #region Post methods

        // Update the order
        // POST: /admin_orders/edit
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
            string customer_email = collection["txtEmail"];
            string customer_org_number = collection["txtOrgNumber"];
            string customer_vat_number = collection["txtVatNumber"];
            string customer_phone = collection["txtPhoneNumber"];
            string customer_mobile_phone = collection["txtMobilePhoneNumber"];
            string customer_name = collection["txtCustomerName"];
            string invoice_name = collection["txtInvoiceName"];
            string invoice_address_1 = collection["txtInvoiceAddress1"];
            string invoice_address_2 = collection["txtInvoiceAddress2"];
            string invoice_post_code = collection["txtInvoicePostCode"];
            string invoice_city = collection["txtInvoiceCity"];
            string invoice_country_id = collection["selectInvoiceCountry"];
            string delivery_name = collection["txtDeliveryName"];
            string delivery_address_1 = collection["txtDeliveryAddress1"];
            string delivery_address_2 = collection["txtDeliveryAddress2"];
            string delivery_post_code = collection["txtDeliveryPostCode"];
            string delivery_city = collection["txtDeliveryCity"];
            string delivery_country_id = collection["selectDeliveryCountry"];
            string payment_status = collection["selectPaymentStatus"];
            string order_status = collection["selectOrderStatus"];
            bool exported_to_erp = Convert.ToBoolean(collection["cbExportedToErp"]);
            DateTime desired_date_of_delivery = DateTime.MinValue;
            DateTime.TryParse(collection["txtDesiredDateOfDelivery"], out desired_date_of_delivery);
            string discount_code = collection["txtDiscountCode"];

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Get the saved order
            Order order = Order.GetOneById(id);

            // Get the payment option
            PaymentOption paymentOption = PaymentOption.GetOneById(order.payment_option, currentDomain.back_end_language);

            // Create the error message string
            string error_message = "";

            // Check for errors
            if (invoice_country_id == "0")
            {
                error_message += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("invoice_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
            }
            else
            {
                order.invoice_country_id = Convert.ToInt32(invoice_country_id);
            }
            if (delivery_country_id == "0")
            {
                error_message += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("delivery_address") + ":" + tt.Get("country").ToLower()) + "<br/>";
            }
            else
            {
                order.delivery_country_id = Convert.ToInt32(delivery_country_id);
            }

            // Check if the payment status has been updated on the order
            if(order.document_type == 1 && order.payment_status != payment_status)
            {
                // Respond to the updated payment status
                error_message += UpdatePaymentStatus(order, payment_status);
            }

            // Check if the order status has been updated on the order
            if (order.document_type == 1 && order.order_status != order_status)
            {
                // Respond to the updated order status
                error_message += UpdateOrderStatus(order, paymentOption, order_status);
            }

            // Update values
            order.customer_email = AnnytabDataValidation.TruncateString(customer_email, 100);
            order.customer_org_number = AnnytabDataValidation.TruncateString(customer_org_number, 20);
            order.customer_vat_number = AnnytabDataValidation.TruncateString(customer_vat_number, 20);
            order.customer_phone = AnnytabDataValidation.TruncateString(customer_phone, 100);
            order.customer_mobile_phone = AnnytabDataValidation.TruncateString(customer_mobile_phone, 100);
            order.customer_name = AnnytabDataValidation.TruncateString(customer_name, 100);
            order.invoice_name = AnnytabDataValidation.TruncateString(invoice_name, 100);
            order.invoice_address_1 = AnnytabDataValidation.TruncateString(invoice_address_1, 100);
            order.invoice_address_2 = AnnytabDataValidation.TruncateString(invoice_address_2, 100);
            order.invoice_post_code = AnnytabDataValidation.TruncateString(invoice_post_code, 100);
            order.invoice_city = AnnytabDataValidation.TruncateString(invoice_city, 100);
            order.delivery_name = AnnytabDataValidation.TruncateString(delivery_name, 100);
            order.delivery_address_1 = AnnytabDataValidation.TruncateString(delivery_address_1, 100);
            order.delivery_address_2 = AnnytabDataValidation.TruncateString(delivery_address_2, 100);
            order.delivery_post_code = AnnytabDataValidation.TruncateString(delivery_post_code, 100);
            order.delivery_city = AnnytabDataValidation.TruncateString(delivery_city, 100);
            order.payment_status = payment_status;
            order.order_status = order_status;
            order.exported_to_erp = exported_to_erp;
            order.desired_date_of_delivery = AnnytabDataValidation.TruncateDateTime(desired_date_of_delivery);
            order.discount_code = AnnytabDataValidation.TruncateString(discount_code, 50);

            // Check if there is any errors
            if(error_message == "")
            {
                // Update the order
                Order.Update(order);

                // Redirect the user to the list
                return Redirect("/admin_orders" + returnUrl);
            }
            else
            {
                // Update the order
                Order.Update(order);

                // Add data to the view
                ViewBag.ErrorMessage = error_message;
                ViewBag.TranslatedTexts = tt;
                ViewBag.Order = order;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }
            
        } // End of the edit method

        // Delete the order
        // POST: /admin_orders/delete/1?returnUrl=?kw=df&so=ASC
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

            // Delete the order post and all the connected posts (CASCADE)
            errorCode = Order.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_orders" + returnUrl);

        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Update the payment status
        /// </summary>
        /// <param name="order">A reference to the order</param>
        /// <param name="paymentStatus">The payment status as a string</param>
        /// <returns>An error message as a string</returns>
        private string UpdatePaymentStatus(Order order, string paymentStatus)
        {
            // Create the string to return
            string error_message = "";

            // Check the status change
            if (paymentStatus == "payment_status_invoice_approved" || paymentStatus == "payment_status_paid")
            {
                CustomerFile.AddCustomerFiles(order);
            }

            // Return the error message
            return error_message;

        } // End of the UpdatePaymentStatus method

        /// <summary>
        /// Respond to an updated order status
        /// </summary>
        /// <param name="order"></param>
        /// <param name="paymentOption"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        private string UpdateOrderStatus(Order order, PaymentOption paymentOption, string orderStatus)
        {
            // Create the string to return
            string error_message = "";

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

            // Check the order status
            if (orderStatus == "order_status_delivered")
            {
                if(paymentOption.connection == 102) // Payson invoice
                {
                    // Get credentials
                    string paysonEmail = webshopSettings.Get("PAYSON-EMAIL");
                    string userId = webshopSettings.Get("PAYSON-AGENT-ID");
                    string md5Key = webshopSettings.Get("PAYSON-MD5-KEY");
                    bool paysonTest = false;
                    bool.TryParse(webshopSettings.Get("PAYSON-TEST"), out paysonTest);

                    // Create the api
                    PaysonIntegration.PaysonApi paysonApi = new PaysonIntegration.PaysonApi(userId, md5Key, null, paysonTest);

                    // Update the order
                    PaysonIntegration.Data.PaymentUpdateData paymentUpdateData = new PaysonIntegration.Data.PaymentUpdateData(order.payment_token, PaysonIntegration.Utils.PaymentUpdateAction.ShipOrder);
                    PaysonIntegration.Response.PaymentUpdateResponse paymentUpdateResponse = paysonApi.MakePaymentUpdateRequest(paymentUpdateData);

                    // Check if the response is successful
                    if (paymentUpdateResponse != null && paymentUpdateResponse.Success == false)
                    {
                        // Set error messages
                        foreach (string key in paymentUpdateResponse.ErrorMessages)
                        {
                            error_message += "&#149; " + "Payson: " + paymentUpdateResponse.ErrorMessages[key] + "<br/>";
                        }
                    }
                }
                else if(paymentOption.connection == 301) // Svea invoice
                {
                    // Get the order rows
                    List<OrderRow> orderRows = OrderRow.GetByOrderId(order.id);

                    // Create the payment configuration
                    SveaSettings sveaConfiguration = new SveaSettings();

                    // Create the order builder
                    Webpay.Integration.CSharp.Order.Handle.DeliverOrderBuilder inoviceBuilder = Webpay.Integration.CSharp.WebpayConnection.DeliverOrder(sveaConfiguration);

                    // Add order rows
                    for (int i = 0; i < orderRows.Count; i++)
                    {
                        // Get the unit
                        Unit unit = Unit.GetOneById(orderRows[i].unit_id, domain.back_end_language);

                        // Create an order item
                        Webpay.Integration.CSharp.Order.Row.OrderRowBuilder orderItem = new Webpay.Integration.CSharp.Order.Row.OrderRowBuilder();
                        orderItem.SetArticleNumber(orderRows[i].product_code);
                        orderItem.SetName(orderRows[i].product_name);
                        orderItem.SetQuantity(orderRows[i].quantity);
                        orderItem.SetUnit(unit.unit_code);
                        orderItem.SetAmountExVat(orderRows[i].unit_price);
                        orderItem.SetVatPercent(orderRows[i].vat_percent * 100);

                        // Add the order item
                        inoviceBuilder.AddOrderRow(orderItem);
                    }

                    // Get the order id
                    Int64 sveaOrderId = 0;
                    Int64.TryParse(order.payment_token, out sveaOrderId);

                    // Set invoice values
                    inoviceBuilder.SetOrderId(sveaOrderId);
                    inoviceBuilder.SetNumberOfCreditDays(15);
                    inoviceBuilder.SetInvoiceDistributionType(Webpay.Integration.CSharp.Util.Constant.InvoiceDistributionType.POST);
                    inoviceBuilder.SetCountryCode(SveaSettings.GetSveaCountryCode(order.country_code));

                    // Make the request to send the invoice
                    Webpay.Integration.CSharp.WebpayWS.DeliverOrderEuResponse deliverOrderResponse = inoviceBuilder.DeliverInvoiceOrder().DoRequest();
                    
                    // Check if the response is successful
                    if (deliverOrderResponse.Accepted == false)
                    {
                        // Set error messages
                        error_message += "&#149; " + "Svea code: " + deliverOrderResponse.ResultCode.ToString() + "<br/>";
                        error_message += "&#149; " + "Svea message: " + deliverOrderResponse.ErrorMessage + "<br/>";
                    }
                }
                else if (paymentOption.connection >= 400 && paymentOption.connection <= 499) // Payex
                {
                    // Check the transaction
                    Dictionary<string, string> payexResponse = PayExManager.CheckTransaction(order, webshopSettings);

                    // Get response variables
                    string error_code = payexResponse.ContainsKey("error_code") == true ? payexResponse["error_code"] : "";
                    string description = payexResponse.ContainsKey("description") == true ? payexResponse["description"] : "";
                    string parameter_name = payexResponse.ContainsKey("parameter_name") == true ? payexResponse["parameter_name"] : "";
                    string transaction_status = payexResponse.ContainsKey("transaction_status") == true ? payexResponse["transaction_status"] : "";
                    string transaction_number = payexResponse.ContainsKey("transaction_number") == true ? payexResponse["transaction_number"] : "";

                    // Check if the response was successful
                    if (error_code.ToUpper() == "OK")
                    {
                        if(transaction_status == "3") // Authorize
                        {
                            // Capture the transaction
                            payexResponse = PayExManager.CaptureTransaction(order);

                            // Get response variables
                            error_code = payexResponse.ContainsKey("error_code") == true ? payexResponse["error_code"] : "";
                            description = payexResponse.ContainsKey("description") == true ? payexResponse["description"] : "";
                            parameter_name = payexResponse.ContainsKey("parameter_name") == true ? payexResponse["parameter_name"] : "";
                            transaction_status = payexResponse.ContainsKey("transaction_status") == true ? payexResponse["transaction_status"] : "";
                            transaction_number = payexResponse.ContainsKey("transaction_number") == true ? payexResponse["transaction_number"] : "";
                            string transaction_number_original = payexResponse.ContainsKey("transaction_number_original") == true ? payexResponse["transaction_number_original"] : "";

                            if(error_code.ToUpper() != "OK" || transaction_status != "6")
                            {
                                // Set error messages
                                error_message += "&#149; " + "Payex code: " + error_code + "<br/>";
                                error_message += "&#149; " + "Payex message: " + description + "<br/>";
                                error_message += "&#149; " + "Payex parameter: " + parameter_name + "<br/>";
                                error_message += "&#149; " + "Payex status: " + transaction_status + "<br/>";
                                error_message += "&#149; " + "Payex number (original): " + transaction_number + "<br/>";
                            }
                            else
                            {
                                // Update the transaction number for the order
                                Order.SetPaymentToken(order.id, transaction_number);
                            }
                        }
                    }
                    else
                    {
                        // Set error messages
                        error_message += "&#149; " + "Payex code: " + error_code + "<br/>";
                        error_message += "&#149; " + "Payex message: " + description + "<br/>";
                        error_message += "&#149; " + "Payex parameter: " + parameter_name + "<br/>";
                        error_message += "&#149; " + "Payex status: " + transaction_status + "<br/>";
                        error_message += "&#149; " + "Payex number: " + transaction_number + "<br/>";
                    }
                }
            }
            else if (orderStatus == "order_status_cancelled")
            {
                if(paymentOption.connection >= 100 && paymentOption.connection <= 199) // Payson
                {
                    // Get credentials
                    string paysonEmail = webshopSettings.Get("PAYSON-EMAIL");
                    string userId = webshopSettings.Get("PAYSON-AGENT-ID");
                    string md5Key = webshopSettings.Get("PAYSON-MD5-KEY");
                    bool paysonTest = false;
                    bool.TryParse(webshopSettings.Get("PAYSON-TEST"), out paysonTest);

                    // Create the api
                    PaysonIntegration.PaysonApi paysonApi = new PaysonIntegration.PaysonApi(userId, md5Key, null, paysonTest);

                    // Get details about the payment status
                    PaysonIntegration.Response.PaymentDetailsResponse paysonResponse = paysonApi.MakePaymentDetailsRequest(new PaysonIntegration.Data.PaymentDetailsData(order.payment_token));

                    // Get the type and status of the payment
                    PaysonIntegration.Utils.PaymentType? paymentType = paysonResponse.PaymentDetails.PaymentType;
                    PaysonIntegration.Utils.PaymentStatus? paymentStatus = paysonResponse.PaymentDetails.PaymentStatus;
                    PaysonIntegration.Utils.InvoiceStatus? invoiceStatus = paysonResponse.PaymentDetails.InvoiceStatus;

                    // Payment update
                    PaysonIntegration.Data.PaymentUpdateData paymentUpdateData = null;
                    PaysonIntegration.Response.PaymentUpdateResponse paymentUpdateResponse = null;

                    if (paymentType == PaysonIntegration.Utils.PaymentType.Direct && paymentStatus == PaysonIntegration.Utils.PaymentStatus.Completed)
                    {
                        // Refund the payment
                        paymentUpdateData = new PaysonIntegration.Data.PaymentUpdateData(order.payment_token, PaysonIntegration.Utils.PaymentUpdateAction.Refund);
                        paymentUpdateResponse = paysonApi.MakePaymentUpdateRequest(paymentUpdateData);
                    }
                    else if (paymentType == PaysonIntegration.Utils.PaymentType.Invoice && invoiceStatus == PaysonIntegration.Utils.InvoiceStatus.OrderCreated)
                    {
                        // Cancel the order
                        paymentUpdateData = new PaysonIntegration.Data.PaymentUpdateData(order.payment_token, PaysonIntegration.Utils.PaymentUpdateAction.CancelOrder);
                        paymentUpdateResponse = paysonApi.MakePaymentUpdateRequest(paymentUpdateData);
                    }
                    else if (paymentType == PaysonIntegration.Utils.PaymentType.Invoice && (invoiceStatus == PaysonIntegration.Utils.InvoiceStatus.Shipped 
                        || invoiceStatus == PaysonIntegration.Utils.InvoiceStatus.Done))
                    {
                        // Credit the order
                        paymentUpdateData = new PaysonIntegration.Data.PaymentUpdateData(order.payment_token, PaysonIntegration.Utils.PaymentUpdateAction.CreditOrder);
                        paymentUpdateResponse = paysonApi.MakePaymentUpdateRequest(paymentUpdateData);
                    }

                    // Check if there was any errors
                    if (paymentUpdateResponse != null && paymentUpdateResponse.Success == false)
                    {
                        // Set error messages
                        foreach (string key in paymentUpdateResponse.ErrorMessages)
                        {
                            error_message += "&#149; " + "Payson: " + paymentUpdateResponse.ErrorMessages[key] + "<br/>";
                        }
                    }
                }
                else if(paymentOption.connection == 201) // PayPal
                {
                    // Get credentials
                    string paypalClientId = webshopSettings.Get("PAYPAL-CLIENT-ID");
                    string paypalClientSecret = webshopSettings.Get("PAYPAL-CLIENT-SECRET");
                    string paypalMode = webshopSettings.Get("PAYPAL-MODE");
                    Dictionary<string, string> config = new Dictionary<string, string> { { "mode", paypalMode } };

                    try
                    {
                        // Create the credential token
                        PayPal.OAuthTokenCredential tokenCredential = new PayPal.OAuthTokenCredential(paypalClientId, paypalClientSecret, config);

                        // Create the api context
                        PayPal.APIContext paypalContext = new PayPal.APIContext(tokenCredential.GetAccessToken());
                        paypalContext.Config = config;

                        // Look up the sale
                        PayPal.Api.Payments.Sale sale = PayPal.Api.Payments.Sale.Get(paypalContext, order.payment_token);

                        if (sale.state == "completed")
                        {
                            // Refund the payment
                            paypalContext.HTTPHeaders = null;
                            PayPal.Api.Payments.Refund refund = sale.Refund(paypalContext, new PayPal.Api.Payments.Refund());

                            if(refund.state != "completed")
                            {
                                error_message += "&#149; " + "PayPal: " + refund.state;
                            }
                        }
                        else
                        {
                            error_message += "&#149; " + "PayPal: " + sale.state;
                        }
                    }
                    catch (Exception ex)
                    {
                        error_message += "&#149; PayPal: " + ex.Message;
                    }
                }
                else if(paymentOption.connection == 301) // Svea invoice
                {
                    // Create the payment configuration
                    SveaSettings sveaConfiguration = new SveaSettings();

                    // Get the order id
                    Int64 sveaOrderId = 0;
                    Int64.TryParse(order.payment_token, out sveaOrderId);

                    // Cancel the order
                    Webpay.Integration.CSharp.Order.Handle.CloseOrderBuilder closeOrder = Webpay.Integration.CSharp.WebpayConnection.CloseOrder(sveaConfiguration);
                    closeOrder.SetOrderId(sveaOrderId);
                    closeOrder.SetCountryCode(SveaSettings.GetSveaCountryCode(order.country_code));
                    Webpay.Integration.CSharp.WebpayWS.CloseOrderEuResponse closeOrderResponse = closeOrder.CloseInvoiceOrder().DoRequest();

                    // Check if the response is successful
                    if (closeOrderResponse.Accepted == false)
                    {
                        // Set error messages
                        error_message += "&#149; " + "Svea code: " + closeOrderResponse.ResultCode.ToString() + "<br/>";
                        error_message += "&#149; " + "Svea message: " + closeOrderResponse.ErrorMessage + "<br/>";
                    }
                }
                else if(paymentOption.connection >= 400 && paymentOption.connection <= 499) // Payex
                {
                    // Check the transaction
                    Dictionary<string, string> payexResponse = PayExManager.CheckTransaction(order, webshopSettings);

                    // Get response variables
                    string error_code = payexResponse.ContainsKey("error_code") == true ? payexResponse["error_code"] : "";
                    string description = payexResponse.ContainsKey("description") == true ? payexResponse["description"] : "";
                    string parameter_name = payexResponse.ContainsKey("parameter_name") == true ? payexResponse["parameter_name"] : "";
                    string transaction_status = payexResponse.ContainsKey("transaction_status") == true ? payexResponse["transaction_status"] : "";
                    string transaction_number = payexResponse.ContainsKey("transaction_number") == true ? payexResponse["transaction_number"] : "";

                    // Check if the response was successful
                    if(error_code.ToUpper() == "OK")
                    {
                        // Check if we should cancel or credit the order
                        if(transaction_status == "3") // Authorize
                        {
                            // Cancel the transaction
                            payexResponse = PayExManager.CancelTransaction(order, webshopSettings);

                            // Get response variables
                            error_code = payexResponse.ContainsKey("error_code") == true ? payexResponse["error_code"] : "";
                            description = payexResponse.ContainsKey("description") == true ? payexResponse["description"] : "";
                            parameter_name = payexResponse.ContainsKey("parameter_name") == true ? payexResponse["parameter_name"] : "";
                            transaction_status = payexResponse.ContainsKey("transaction_status") == true ? payexResponse["transaction_status"] : "";
                            transaction_number = payexResponse.ContainsKey("transaction_number") == true ? payexResponse["transaction_number"] : "";

                            if(error_code.ToUpper() != "OK" || transaction_status != "4")
                            {
                                // Set error messages
                                error_message += "&#149; " + "Payex code: " + error_code + "<br/>";
                                error_message += "&#149; " + "Payex message: " + description + "<br/>";
                                error_message += "&#149; " + "Payex parameter: " + parameter_name + "<br/>";
                                error_message += "&#149; " + "Payex status: " + transaction_status + "<br/>";
                                error_message += "&#149; " + "Payex number: " + transaction_number + "<br/>";
                            }
                        }
                        else if(transaction_status == "0" || transaction_status == "6") // Sale or capture
                        {
                            // Get the order rows
                            List<OrderRow> orderRows = OrderRow.GetByOrderId(order.id);

                            // Credit the transaction
                            payexResponse = PayExManager.CreditTransaction(order, orderRows, webshopSettings);

                            // Get response variables
                            error_code = payexResponse.ContainsKey("error_code") == true ? payexResponse["error_code"] : "";
                            description = payexResponse.ContainsKey("description") == true ? payexResponse["description"] : "";
                            parameter_name = payexResponse.ContainsKey("parameter_name") == true ? payexResponse["parameter_name"] : "";
                            transaction_status = payexResponse.ContainsKey("transaction_status") == true ? payexResponse["transaction_status"] : "";
                            transaction_number = payexResponse.ContainsKey("transaction_number") == true ? payexResponse["transaction_number"] : "";

                            if (error_code.ToUpper() != "OK" || transaction_status != "2")
                            {
                                // Set error messages
                                error_message += "&#149; " + "Payex code: " + error_code + "<br/>";
                                error_message += "&#149; " + "Payex message: " + description + "<br/>";
                                error_message += "&#149; " + "Payex parameter: " + parameter_name + "<br/>";
                                error_message += "&#149; " + "Payex status: " + transaction_status + "<br/>";
                                error_message += "&#149; " + "Payex number: " + transaction_number + "<br/>";
                            }
                        }
                    }
                    else
                    {
                        // Set error messages
                        error_message += "&#149; " + "Payex code: " + error_code + "<br/>";
                        error_message += "&#149; " + "Payex message: " + description + "<br/>";
                        error_message += "&#149; " + "Payex parameter: " + parameter_name + "<br/>";
                        error_message += "&#149; " + "Payex status: " + transaction_status + "<br/>";
                        error_message += "&#149; " + "Payex number: " + transaction_number + "<br/>";
                    }
                }
            }

            // Return the error message
            return error_message;

        } // End of the UpdateOrderStatus method

        #endregion

    } // End of the class

} // End of the namespace