using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the customer order process
    /// </summary>
    public class orderController : Controller
    {
        #region View methods

        // Get the order page
        // GET: /order/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the currency
            Currency currency = Currency.GetOneById(currentDomain.currency);
            currency = currency != null ? currency : new Currency();

            // Create the customer
            Customer customer = new Customer();
            HttpCookie customerCookie = Request.Cookies.Get("CustomerCookie");

            // Get the customer
            if (customerCookie != null)
            {
                Int32 customerId = 0;
                Int32.TryParse(Tools.UnprotectCookieValue(customerCookie.Value, "CustomerLogin"), out customerId);
                customer = Customer.GetOneById(customerId);
                customer = customer != null ? customer : new Customer();
            }

            // Get the vat code and the freight multiplier
            Country invoiceCountry = Country.GetOneById(customer.invoice_country, currentDomain.front_end_language);
            Country deliveryCountry = Country.GetOneById(customer.delivery_country, currentDomain.front_end_language);
            byte vatCode = Tools.GetVatCode(customer.customer_type, invoiceCountry, deliveryCountry);

            // Calculate the decimal multiplier
            Int32 decimalMultiplier = (Int32)Math.Pow(10, currency.decimals);

            // Get the cart items
            List<CartItem> cartItems = CartItem.GetCartItems(currentDomain.front_end_language);
            Dictionary<string, decimal> cartAmounts = CartItem.GetCartAmounts(cartItems, currentDomain.front_end_language, vatCode, decimalMultiplier);

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("check_out"), "/order"));

            // Get the discount code, gift cards and errors
            string discountCodeId = Session["DiscountCodeId"] != null ? Session["DiscountCodeId"].ToString() : "";
            List<GiftCard> giftCards = Session["GiftCards"] != null ? (List<GiftCard>)Session["GiftCards"] : new List<GiftCard>(0);
            string codeError = Session["CodeError"] != null ? Session["CodeError"].ToString() : "";

            // Create a error message
            string errorMessage = "";

            if (codeError == "empty_shopping_cart")
            {
                errorMessage += "&#149; " + tt.Get("error_cart_empty") + "<br/>";
            }
            if (codeError == "customer_not_signed_in")
            {
                errorMessage += "&#149; " + tt.Get("log_in") + "<br/>";
            }
            if (codeError == "invalid_discount_code")
            {
                errorMessage += "&#149; " + tt.Get("invalid_discount_code") + "<br/>";
            }
            if (codeError == "invalid_gift_card")
            {
                errorMessage += "&#149; " + tt.Get("invalid_gift_card") + "<br/>";
            }

            // Remove the code error
            Session.Remove("CodeError");

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.CurrentCategory = new Category();
            ViewBag.PaymentOptionId = 0;
            ViewBag.Currency = currency;
            ViewBag.Customer = customer;
            ViewBag.CartItems = cartItems;
            ViewBag.DecimalMultiplier = decimalMultiplier;
            ViewBag.VatCode = vatCode;
            ViewBag.CartAmounts = cartAmounts;
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);
            ViewBag.DesiredDateOfDelivery = DateTime.UtcNow;
            ViewBag.DiscountCodeId = discountCodeId;
            ViewBag.GiftCards = giftCards;

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/checkout.cshtml");

        } // End of the index method

        // Get the order confirmation page
        // GET: /order/confirmation
        [HttpGet]
        public ActionResult confirmation(int id = 0)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the order
            Order order = Order.GetOneById(id);

            // Make sure that the order not is null
            if(order == null)
            {
                Response.StatusCode = 404;
                Response.Status = "404 Not Found";
                Response.Write(Tools.GetHttpNotFoundPage());
                return new EmptyResult();
            }

            // Get the signed in customer
            Customer currentCustomer = null;
            HttpCookie customerCookie = Request.Cookies.Get("CustomerCookie");

            // Check if the customer cookie exists
            if (customerCookie != null)
            {
                // Get the customer from the cookie
                Int32 customerId = 0;
                Int32.TryParse(Tools.UnprotectCookieValue(customerCookie.Value, "CustomerLogin"), out customerId);
                currentCustomer = Customer.GetOneById(customerId);
            }

            // Make sure that a customer is signed in and that the order 
            // is issued to that customer
            if(currentCustomer == null || order.customer_id != currentCustomer.id)
            {
                return RedirectToAction("index", "home");
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/customer/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("order_confirmation"), "/order/confirmation/" + order.id.ToString()));

            // Set values form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.CurrentCategory = new Category();
            ViewBag.Order = order;
            ViewBag.OrderRows = OrderRow.GetByOrderId(order.id);
            ViewBag.InvoiceCountry = Country.GetOneById(order.invoice_country_id, currentDomain.front_end_language);
            ViewBag.DeliveryCountry = Country.GetOneById(order.delivery_country_id, currentDomain.front_end_language);
            ViewBag.PaymentOption = PaymentOption.GetOneById(order.payment_option, currentDomain.front_end_language);
            ViewBag.Company = Company.GetOneById(order.company_id);
            ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view confirmation.cshtml
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/order_confirmation.cshtml");

        } // End of the confirmation method

        #endregion

        #region Post methods

        // Create the order and checkout the customer
        // POST: /order/index
        [HttpPost]
        public ActionResult index(FormCollection collection)
        {
            // Check if the customer is signed in
            if (Request.Cookies.Get("CustomerCookie") == null)
            {
                return RedirectToAction("login", "customer");
            }

            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the language
            Language currentLanguage = Language.GetOneById(currentDomain.front_end_language);

            // Get the currency 
            Currency currency = Currency.GetOneById(currentDomain.currency);
            currency = currency != null ? currency : new Currency();

            // Get the document type
            byte document_type = 1;
            if(collection["btnRequest"] != null)
            {
                document_type = 0;
            }

            // Get all the form values
            Int32 customerId = Convert.ToInt32(collection["hiddenCustomerId"]);
            byte customerType = Convert.ToByte(collection["hiddenCustomerType"]);
            byte vatCode = Convert.ToByte(collection["hiddenVatCode"]);
            string[] additionalServiceIds = collection.GetValues("cbService");
            Int32 paymentOptionId = Convert.ToInt32(collection["radioPaymentOption"]);
            string email = collection["txtEmail"];
            string orgNumber = collection["txtOrgNumber"];
            string vatNumber = "";
            string contactName = collection["txtContactName"];
            string phoneNumber = collection["txtPhoneNumber"];
            string mobilePhoneNumber = collection["txtMobilePhoneNumber"];
            string invoiceName = collection["txtInvoiceName"];
            string invoiceAddress1 = collection["txtInvoiceAddress1"];
            string invoiceAddress2 = collection["txtInvoiceAddress2"];
            string invoicePostCode = collection["txtInvoicePostCode"];
            string invoiceCity = collection["txtInvoiceCity"];
            Int32 invoiceCountryId = Convert.ToInt32(collection["selectInvoiceCountry"]);
            string deliveryName = collection["txtDeliveryName"];
            string deliveryAddress1 = collection["txtDeliveryAddress1"];
            string deliveryAddress2 = collection["txtDeliveryAddress2"];
            string deliveryPostCode = collection["txtDeliveryPostCode"];
            string deliveryCity = collection["txtDeliveryCity"];
            Int32 deliveryCountryId = Convert.ToInt32(collection["selectDeliveryCountry"]);
            Country deliveryCountry = Country.GetOneById(deliveryCountryId, currentDomain.front_end_language);
            DateTime desired_date_of_delivery = DateTime.MinValue;
            DateTime.TryParse(collection["txtDesiredDateOfDelivery"], out desired_date_of_delivery);
            string discount_code_id = collection["hiddenDiscountCodeId"];

            // Set values depending of the customer type, 0: Company
            if(customerType == 0)
            {
                vatNumber = collection["txtVatNumber"];    
            }

            // Create the customer
            Customer customer = new Customer();
            customer.id = customerId;
            customer.email = email.Length > 100 ? email.Substring(0, 100) : email;
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
            customer.invoice_country = invoiceCountryId;
            customer.delivery_name = deliveryName.Length > 100 ? deliveryName.Substring(0, 100) : deliveryName;
            customer.delivery_address_1 = deliveryAddress1.Length > 100 ? deliveryAddress1.Substring(0, 100) : deliveryAddress1;
            customer.delivery_address_2 = deliveryAddress2.Length > 100 ? deliveryAddress2.Substring(0, 100) : deliveryAddress2;
            customer.delivery_post_code = deliveryPostCode.Length > 100 ? deliveryPostCode.Substring(0, 100) : deliveryPostCode;
            customer.delivery_city = deliveryCity.Length > 100 ? deliveryCity.Substring(0, 100) : deliveryCity;
            customer.delivery_country = deliveryCountryId;

            // Create the order
            Order order = new Order();
            order.document_type = document_type;
            order.customer_id = customer.id;
            order.order_date = DateTime.UtcNow;
            order.company_id = currentDomain.company_id;
            order.country_code = currentLanguage.country_code;
            order.language_code = currentLanguage.language_code;
            order.currency_code = currency.currency_code;
            order.conversion_rate = (currency.conversion_rate / currency.currency_base);
            order.customer_type = customer.customer_type;
            order.customer_org_number = customer.org_number;
            order.customer_vat_number = customer.vat_number;
            order.customer_name = customer.contact_name;
            order.customer_email = customer.email;
            order.customer_phone = customer.phone_number;
            order.customer_mobile_phone = customer.mobile_phone_number;
            order.invoice_name = customer.invoice_name;
            order.invoice_address_1 = customer.invoice_address_1;
            order.invoice_address_2 = customer.invoice_address_2;
            order.invoice_post_code = customer.invoice_post_code;
            order.invoice_city = customer.invoice_city;
            order.invoice_country_id = invoiceCountryId;
            order.delivery_name = customer.delivery_name;
            order.delivery_address_1 = customer.delivery_address_1;
            order.delivery_address_2 = customer.delivery_address_2;
            order.delivery_post_code = customer.delivery_post_code;
            order.delivery_city = customer.delivery_city;
            order.delivery_country_id = deliveryCountryId;
            order.vat_code = vatCode;
            order.payment_option = paymentOptionId;
            order.payment_status = "payment_status_pending";
            order.exported_to_erp = false;
            order.order_status = "order_status_pending";
            order.desired_date_of_delivery = AnnytabDataValidation.TruncateDateTime(desired_date_of_delivery);
            order.discount_code = discount_code_id;

            // Calculate the decimal multiplier
            Int32 decimalMultiplier = (Int32)Math.Pow(10, currency.decimals);

            // Get the cart items
            List<CartItem> cartItems = CartItem.GetCartItems(currentDomain.front_end_language);
            Dictionary<string, decimal> cartAmounts = CartItem.GetCartAmounts(cartItems, currentDomain.front_end_language, vatCode, decimalMultiplier);

            // Create order rows
            List<OrderRow> orderRows = CartItem.GetOrderRows(cartItems, order.vat_code, currentDomain.front_end_language, decimalMultiplier);

            // Get additional services
            List<AdditionalService> additionalServices = AdditionalService.GetAllActive(currentDomain.front_end_language, "id", "ASC");
            Int32 additionalServiceIdsCount = additionalServiceIds != null ? additionalServiceIds.Length : 0;

            // Loop additional services
            for(int i = 0; i < additionalServices.Count; i++)
            {
                // Check if the service is selected
                for (int j = 0; j < additionalServiceIdsCount; j++)
                {
                    // Convert the id
                    Int32 serviceId = Convert.ToInt32(additionalServiceIds[j]);

                    // The service is selected
                    if(additionalServices[i].id == serviceId)
                    {
                        // Set the service as selected
                        additionalServices[i].selected = true;

                        // Calculate the fee
                        decimal fee = additionalServices[i].price_based_on_mount_time == true ? additionalServices[i].fee * cartAmounts["total_mount_time"] : additionalServices[i].fee;
                        fee *= (currency.currency_base / currency.conversion_rate);
                        fee = Math.Round(fee * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

                        // Get the value added tax percent
                        ValueAddedTax vat = ValueAddedTax.GetOneById(additionalServices[i].value_added_tax_id);

                        // Create a order row
                        OrderRow orderRow = new OrderRow();
                        orderRow.product_code = additionalServices[i].product_code;
                        orderRow.manufacturer_code = "";
                        orderRow.product_id = 0;
                        orderRow.product_name = additionalServices[i].name;
                        orderRow.vat_percent = order.vat_code != 0 ? 0 : vat.value;
                        orderRow.quantity = 1;
                        orderRow.unit_id = additionalServices[i].unit_id;
                        orderRow.unit_price = fee;
                        orderRow.account_code = additionalServices[i].account_code;
                        orderRow.supplier_erp_id = "";
                        orderRow.sort_order = (Int16)orderRows.Count();

                        // Add the order row
                        orderRows.Add(orderRow);
                    }
                }
            }

            // Get the payment fee
            PaymentOption paymentOption = PaymentOption.GetOneById(order.payment_option, currentDomain.front_end_language);
            paymentOption = paymentOption != null ? paymentOption : new PaymentOption();

            if (paymentOption.fee > 0)
            {
                // Get the value added tax percent
                ValueAddedTax vat = ValueAddedTax.GetOneById(paymentOption.value_added_tax_id);

                // Calculate the fee
                decimal fee = paymentOption.fee * (currency.currency_base / currency.conversion_rate);
                fee = Math.Round(fee * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

                // Create a order row
                OrderRow orderRow = new OrderRow();
                orderRow.product_code = paymentOption.product_code;
                orderRow.manufacturer_code = "";
                orderRow.product_id = 0;
                orderRow.product_name = paymentOption.name;
                orderRow.vat_percent = order.vat_code != 0 ? 0 : vat.value;
                orderRow.quantity = 1;
                orderRow.unit_id = paymentOption.unit_id;
                orderRow.unit_price = fee;
                orderRow.account_code = paymentOption.account_code;
                orderRow.supplier_erp_id = "";
                orderRow.sort_order = (Int16)orderRows.Count();

                // Add the order row
                orderRows.Add(orderRow);
            }

            // Get translated texts
            KeyStringList translatedTexts = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors
            if (AnnytabDataValidation.IsEmailAddressValid(customer.email) == null)
            {
                errorMessage += "&#149; " + translatedTexts.Get("error_email_valid") + "<br/>";
            }
            if(cartItems.Count <= 0)
            {
                errorMessage += "&#149; " + translatedTexts.Get("error_cart_empty") + "<br/>";
            }
            if (order.payment_option == 0)
            {
                errorMessage += "&#149; " + translatedTexts.Get("error_no_payment_option") + "<br/>";
            }
            if(order.invoice_country_id == 0)
            {
                errorMessage += "&#149; " + String.Format(translatedTexts.Get("error_select_value"), translatedTexts.Get("invoice_address") + ":" + translatedTexts.Get("country").ToLower()) + "<br/>";
            }
            if (order.delivery_country_id == 0)
            {
                errorMessage += "&#149; " + String.Format(translatedTexts.Get("error_select_value"), translatedTexts.Get("delivery_address") + ":" + translatedTexts.Get("country").ToLower()) + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == "")
            {
                // Get the order sums and add them to the order
                Dictionary<string, decimal> orderAmounts = Order.GetOrderAmounts(orderRows, order.vat_code, decimalMultiplier);

                // Add the sums to the order
                order.net_sum = orderAmounts["net_amount"];
                order.vat_sum = orderAmounts["vat_amount"];
                order.rounding_sum = orderAmounts["rounding_amount"];
                order.total_sum = orderAmounts["total_amount"];

                // Add the order
                Int64 insertId = Order.Add(order);
                order.id = Convert.ToInt32(insertId);

                // Update the gift cards amount for the order
                if(order.document_type != 0)
                {
                    order.gift_cards_amount = ProcessGiftCards(order.id, order.total_sum);
                    Order.UpdateGiftCardsAmount(order.id, order.gift_cards_amount);
                }
                
                // Add the order rows
                for (int i = 0; i < orderRows.Count; i++)
                {
                    orderRows[i].order_id = order.id;
                    OrderRow.Add(orderRows[i]);
                }

                // Delete the shopping cart
                CartItem.ClearShoppingCart();

                // Get the confirmation message
                string message = RenderOrderConfirmationView(order.id, this.ControllerContext);

                // Create a session to indicate that ecommerce data should be sent to google
                Session["SendToGoogleEcommerce"] = true;

                // Check if the user wants to send a request
                if (order.document_type == 0)
                {
                    // Send the request email
                    Tools.SendOrderConfirmation(customer.email, translatedTexts.Get("request") + " " + order.id.ToString() + " - " + currentDomain.webshop_name, message);
                    return RedirectToAction("confirmation", "order", new { id = order.id });
                }

                // Send the order confirmation email
                Tools.SendOrderConfirmation(customer.email, translatedTexts.Get("order_confirmation") + " " + order.id.ToString() + " - " + currentDomain.webshop_name, message);

                // Check if the order has been paid by gift cards
                if (order.total_sum <= order.gift_cards_amount)
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(order.id, "payment_status_paid");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);

                    // Return the order confirmation
                    return RedirectToAction("confirmation", "order", new { id = order.id });
                }

                // Check the selected payment option
                if(paymentOption.connection == 101)
                {
                    return CreatePaysonPayment(order, orderRows, currentDomain, translatedTexts, "DIRECT");
                }
                if (paymentOption.connection == 102)
                {
                    return CreatePaysonPayment(order, orderRows, currentDomain, translatedTexts, "INVOICE");
                }
                else if (paymentOption.connection == 201)
                {
                    return CreatePayPalPayment(order, orderRows, currentDomain, translatedTexts);
                }
                else if (paymentOption.connection == 301)
                {
                    return CreateSveaPayment(order, orderRows, currentDomain, translatedTexts, "INVOICE");
                }
                else if (paymentOption.connection == 302)
                {
                    return CreateSveaPayment(order, orderRows, currentDomain, translatedTexts, "CREDITCARD");
                }
                else if (paymentOption.connection == 401)
                {
                    return CreatePayexPayment(order, orderRows, currentDomain, translatedTexts, "CREDITCARD");
                }
                else if (paymentOption.connection == 402)
                {
                    return CreatePayexPayment(order, orderRows, currentDomain, translatedTexts, "DIRECTDEBIT");
                }
                else if (paymentOption.connection == 403)
                {
                    return CreatePayexPayment(order, orderRows, currentDomain, translatedTexts, "INVOICE");
                }
                else if (paymentOption.connection == 404)
                {
                    return CreatePayexPayment(order, orderRows, currentDomain, translatedTexts, "MICROACCOUNT");
                }
                else
                {
                    return RedirectToAction("confirmation", "order", new { id = order.id });
                }
            }
            else
            {

                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("check_out"), "/order/"));

                // Set form values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.CurrentDomain = currentDomain;
                ViewBag.TranslatedTexts = translatedTexts;
                ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
                ViewBag.CurrentCategory = new Category();
                ViewBag.Customer = customer;
                ViewBag.Currency = currency;
                ViewBag.CartItems = cartItems;
                ViewBag.DecimalMultiplier = decimalMultiplier;
                ViewBag.PaymentOptionId = paymentOptionId;
                ViewBag.VatCode = vatCode;
                ViewBag.CartAmounts = cartAmounts;
                ViewBag.PricesIncludesVat = Session["PricesIncludesVat"] != null ? Convert.ToBoolean(Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);
                ViewBag.DesiredDateOfDelivery = order.desired_date_of_delivery;
                ViewBag.DiscountCodeId = order.discount_code;
                ViewBag.GiftCards = Session["GiftCards"] != null ? (List<GiftCard>)Session["GiftCards"] : new List<GiftCard>(0);

                // Return the index view
                return currentDomain.custom_theme_id == 0 ? View("index") : View("/Views/theme/checkout.cshtml");
            }

        } // End of the index method

        #endregion

        #region Cart methods

        // Add quantity to a product in the shopping cart
        // GET: /order/add_quantity
        [HttpGet]
        public ActionResult add_quantity(string id = "")
        {
            // Add one unit to the cart item in the shopping cart
            CartItem.UpdateCartQuantity(id, 1);

            // Redirect the user to the start page
            return RedirectToAction("index", "order", new { cu = "true" });

        } // End of the add_quantity method

        // Remove quantity from a product in the shopping cart
        // GET: /order/remove_quantity
        [HttpGet]
        public ActionResult remove_quantity(string id = "")
        {
            // Remove one unit from the cart item in the shopping cart
            CartItem.UpdateCartQuantity(id, -1);

            // Redirect the user to the start page
            return RedirectToAction("index", "order", new { cu = "true" });

        } // End of the remove_quantity method

        // Remove a order row from the shopping cart
        // GET: /order/delete_row
        [HttpGet]
        public ActionResult delete_row(string id = "")
        {
            // Delete the cart item from the shopping cart
            CartItem.DeleteCartItem(id);

            // Redirect the user to the start page
            return RedirectToAction("index", "order", new { cu = "true" });

        } // End of the delete_row method

        // Clear the shopping cart
        // GET: /order/clear
        [HttpGet]
        public ActionResult clear()
        {
            // Remove the shopping cart
            CartItem.ClearShoppingCart();

            // Redirect the user to the start page
            return RedirectToAction("index", "home", new { cu = "true" });

        } // End of the clear method

        // Set a discount code for the order
        // POST: /order/set_discount_code
        [HttpPost]
        public ActionResult set_discount_code(FormCollection collection)
        {
            // Get all the form values
            string discount_code = collection["txtDiscountCode"];

            // Set the discount code and modify the shopping cart
            CartItem.SetDiscountCode(discount_code);

            // Redirect the user to the start page
            return RedirectToAction("index", "order", new { cu = "true" });

        } // End of the set_discount_code method

        // Add a gift card to the order
        // POST: /order/add_gift_card
        [HttpPost]
        public ActionResult add_gift_card(FormCollection collection)
        {
            // Get all the form values
            string gift_card_code = collection["txtGiftCardCode"];

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the gift card
            GiftCard giftCard = GiftCard.GetOneById(gift_card_code);

            // Get the currency
            Currency currency = Currency.GetOneById(domain.currency);

            // Get the list of gift cards
            List<GiftCard> giftCards = (List<GiftCard>)Session["GiftCards"];

            // Make sure that the list not is null and check if the gift card has been added already
            bool duplicateGiftCard = false;
            if (giftCards != null)
            {
                // Loop the list
                for (int i = 0; i < giftCards.Count; i++)
                {
                    // Check if the gift card already has been added
                    if (giftCards[i].id == gift_card_code)
                    {
                        duplicateGiftCard = true;
                        break;
                    }
                }
            }

            // Check if there is errors with the gift card
            if (giftCard == null)
            {
                // The gift card does not exist
                Session["CodeError"] = "invalid_gift_card";
            }
            else if(duplicateGiftCard == true)
            {
                // The gift card has already been added
                Session["CodeError"] = "invalid_gift_card";
            }
            else if (DateTime.UtcNow.AddDays(-1) > giftCard.end_date)
            {
                // The gift card is not valid anymore
                Session["CodeError"] = "invalid_gift_card";
            }
            else if (giftCard.language_id != domain.front_end_language)
            {
                // The gift card is not valid for the language
                Session["CodeError"] = "invalid_gift_card";
            }
            else if (giftCard.currency_code != currency.currency_code)
            {
                // The gift card is not valid for the currency
                Session["CodeError"] = "invalid_gift_card";
            }
            else
            {
                // Make sure that the list not is null
                if(giftCards == null)
                {
                    giftCards = new List<GiftCard>(10);
                }

                // Add the gift card to the list
                giftCards.Add(giftCard);

                // Recreate the session variable
                Session["GiftCards"] = giftCards;
                Session.Remove("CodeError");
            }

            // Redirect the user to the check out
            return RedirectToAction("index", "order");

        } // End of the add_gift_card method

        // Delete a gift card
        // GET: /order/delete_gift_card
        [HttpGet]
        public ActionResult delete_gift_card(string id = "")
        {
            // Get the list of gift cards
            List<GiftCard> giftCards = (List<GiftCard>)Session["GiftCards"];

            // Make sure that the list not is null
            if(giftCards != null)
            {
                // Loop the list
                for(int i = 0; i < giftCards.Count; i++)
                {
                    // Delete the gift card if it can be found
                    if(giftCards[i].id == id)
                    {
                        giftCards.RemoveAt(i);
                        break;
                    }
                }

                // Recreate the session variable
                Session["GiftCards"] = giftCards;
                Session.Remove("CodeError");
            }

            // Redirect the user to the check out
            return RedirectToAction("index", "order");

        } // End of the clear method

        #endregion

        #region Payex payment

        /// <summary>
        /// Create a payex payment
        /// </summary>
        public ActionResult CreatePayexPayment(Order order, List<OrderRow> orderRows, Domain domain, KeyStringList tt, string paymentType)
        {
            // Create the payex payment
            Dictionary<string, string> response = PayExManager.CreateOrder(order, orderRows, domain, tt, paymentType);

            // Get the error code
            string error_code = response.ContainsKey("error_code") == true ? response["error_code"] : "";

            // Check if the response is successful
            if (error_code == "OK" && paymentType == "INVOICE")
            {
                // Get response variables
                string transaction_status = response.ContainsKey("transaction_status") == true ? response["transaction_status"] : "";
                string transaction_number = response.ContainsKey("transaction_number") == true ? response["transaction_number"] : "";

                // Save the transaction number
                Order.SetPaymentToken(order.id, transaction_number);

                if(transaction_status == "3")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(order.id, "payment_status_invoice_approved");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if (transaction_status == "5")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(order.id, "payment_status_invoice_not_approved");
                }

                // Redirect the user to the order confirmation page
                return RedirectToAction("confirmation", "order", new { id = order.id });

            }
            else if (error_code == "OK")
            {
                // Redirect the user to payex
                return Redirect(response["redirect_url"]);
            }
            else
            {
                // Redirect the user to the order confirmation page
                return RedirectToAction("confirmation", "order", new { id = order.id });
            }

        } // End of the CreatePayexPayment method

        // Get the payex confirmation page
        // GET: /order/payex_confirmation
        [HttpGet]
        public ActionResult payex_confirmation(Int32 id = 0)
        {
            // Get the order reference
            string orderReference = "";
            if (Request.Params["orderRef"] != null)
            {
                orderReference = Server.UrlDecode(Request.Params["orderRef"]);
            }

            // Complete the order
            Dictionary<string, string> response = PayExManager.CompleteOrder(orderReference);

            // Get response variables
            string error_code = response.ContainsKey("error_code") == true ? response["error_code"] : "";
            string transaction_status = response.ContainsKey("transaction_status") == true ? response["transaction_status"] : "";
            string transaction_number = response.ContainsKey("transaction_number") == true ? response["transaction_number"] : "";
            string payment_method = response.ContainsKey("payment_method") == true ? response["payment_method"] : "";

            // Get the current domain 
            Domain domain = Tools.GetCurrentDomain();

            // Get the order
            Order order = Order.GetOneById(id);

            // Make sure that the order not is null
            if (order == null)
            {
                // Redirect the user to the order confirmation page
                return RedirectToAction("index", "home");
            }

            // Make sure that callback is accepted
            if (error_code == "OK")
            {
                // Save the transaction number
                Order.SetPaymentToken(id, transaction_number);

                // Get the payment option
                PaymentOption paymentOption = PaymentOption.GetOneById(order.payment_option, domain.back_end_language);

                if (paymentOption.connection == 403 && transaction_status == "3")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_invoice_approved");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if (paymentOption.connection == 402 && transaction_status == "0")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_paid");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if ((paymentOption.connection == 401 || paymentOption.connection == 404) && transaction_status == "0")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_paid");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if(paymentOption.connection == 403 && transaction_status != "5")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_invoice_not_approved");
                }
            }

            // Redirect the user to the order confirmation page
            return RedirectToAction("confirmation", "order", new { id = id });

        } // End of the payex_confirmation method

        #endregion

        #region PayPal payment

        /// <summary>
        /// Create a paypal payment
        /// </summary>
        public ActionResult CreatePayPalPayment(Order order, List<OrderRow> orderRows, Domain domain, KeyStringList tt)
        {
            // Create the string to return
            string error_message = "";

            // Get the currency
            Currency currency = Currency.GetOneById(order.currency_code);

            // Get the webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

            // Get credentials
            string paypalClientId = webshopSettings.Get("PAYPAL-CLIENT-ID");
            string paypalClientSecret = webshopSettings.Get("PAYPAL-CLIENT-SECRET");
            string paypalMode = webshopSettings.Get("PAYPAL-MODE");
            Dictionary<string, string> config = new Dictionary<string, string> { { "mode", paypalMode } };

            // Create a payment variable
            PayPal.Api.Payments.Payment createdPayment = null;

            try
            {
                // Create the credential token
                PayPal.OAuthTokenCredential tokenCredential = new PayPal.OAuthTokenCredential(paypalClientId, paypalClientSecret, config);

                // Create the api context
                PayPal.APIContext paypalContext = new PayPal.APIContext(tokenCredential.GetAccessToken());
                paypalContext.Config = config;

                // Create the amount details
                decimal subTotal = order.net_sum + order.rounding_sum - order.gift_cards_amount;
                PayPal.Api.Payments.Details amountDetails = new PayPal.Api.Payments.Details();
                amountDetails.subtotal = subTotal.ToString("F2", CultureInfo.InvariantCulture);
                amountDetails.tax = order.vat_sum.ToString("F2", CultureInfo.InvariantCulture);

                // Create the amount
                decimal totalAmount = order.total_sum - order.gift_cards_amount;
                PayPal.Api.Payments.Amount amount = new PayPal.Api.Payments.Amount();
                amount.total = totalAmount.ToString("F2", CultureInfo.InvariantCulture);
                amount.currency = order.currency_code;
                amount.details = amountDetails;

                // Create a transaction
                PayPal.Api.Payments.Transaction transaction = new PayPal.Api.Payments.Transaction();
                transaction.item_list = new PayPal.Api.Payments.ItemList();
                transaction.item_list.items = new List<PayPal.Api.Payments.Item>(10);

                // Add order rows to the transaction
                for (int i = 0; i < orderRows.Count; i++)
                {
                    // Create a new item
                    PayPal.Api.Payments.Item item = new PayPal.Api.Payments.Item();
                    item.sku = orderRows[i].product_code.Length > 50 ? orderRows[i].product_code.Substring(0, 50) : orderRows[i].product_code;
                    item.name = orderRows[i].product_name.Length > 100 ? orderRows[i].product_name.Substring(0, 50) : orderRows[i].product_name;
                    item.price = orderRows[i].unit_price.ToString("F2", CultureInfo.InvariantCulture);
                    item.quantity = Convert.ToInt32(orderRows[i].quantity).ToString();
                    item.currency = order.currency_code;

                    // Add the item to the list
                    transaction.item_list.items.Add(item);
                }

                // Add the rounding
                if(order.rounding_sum != 0)
                {
                    PayPal.Api.Payments.Item roundingItem = new PayPal.Api.Payments.Item();
                    roundingItem.sku = "rd";
                    roundingItem.name = tt.Get("rounding");
                    roundingItem.price = order.rounding_sum.ToString("F2", CultureInfo.InvariantCulture);
                    roundingItem.quantity = "1";
                    roundingItem.currency = order.currency_code;
                    transaction.item_list.items.Add(roundingItem);
                }

                // Add the gift cards amount
                if (order.gift_cards_amount > 0)
                {
                    decimal giftCardAmount = order.gift_cards_amount * -1;
                    PayPal.Api.Payments.Item giftCardsItem = new PayPal.Api.Payments.Item();
                    giftCardsItem.sku = "gc";
                    giftCardsItem.name = tt.Get("gift_cards");
                    giftCardsItem.price = giftCardAmount.ToString("F2", CultureInfo.InvariantCulture);
                    giftCardsItem.quantity = "1";
                    giftCardsItem.currency = order.currency_code;
                    transaction.item_list.items.Add(giftCardsItem);
                }
                
                // Set the transaction amount
                transaction.amount = amount;
                List<PayPal.Api.Payments.Transaction> transactions = new List<PayPal.Api.Payments.Transaction>();
                transactions.Add(transaction);

                // Create the payer
                PayPal.Api.Payments.Payer payer = new PayPal.Api.Payments.Payer();
                payer.payment_method = "paypal";

                // Create redirect urls
                string hostUrl = Request.Url.Host;
                PayPal.Api.Payments.RedirectUrls redirectUrls = new PayPal.Api.Payments.RedirectUrls();
                redirectUrls.return_url = domain.web_address + "/order/paypal_confirmation/" + order.id;
                redirectUrls.cancel_url = domain.web_address + "/order/confirmation/" + order.id;

                // Create the payment
                PayPal.Api.Payments.Payment payment = new PayPal.Api.Payments.Payment();
                payment.intent = "sale";
                payment.payer = payer;
                payment.redirect_urls = redirectUrls;
                payment.transactions = transactions;

                // Create the payment
                createdPayment = payment.Create(paypalContext);

            }
            catch (Exception ex)
            {
                error_message = ex.Message;
            }

            // Check if there is any errors in the payment
            if (createdPayment != null)
            {
                // Save the paypal payment id
                Order.SetPaymentToken(order.id, createdPayment.id);

                // Get the link
                string link = "";
                foreach(PayPal.Api.Payments.Links url in createdPayment.links)
                {
                    if (url.rel == "approval_url")
                    {
                        link = url.href;
                        break;
                    }
                }

                // Redirect the user to the paypal page
                return Redirect(link);
            }
            else
            {
                // Redirect the user to the order confirmation page
                return RedirectToAction("confirmation", "order", new { id = order.id });
            }

        } // End of the CreatePayPalPayment method

        // Get the paypal confirmation page
        // GET: /order/paypal_confirmation
        [HttpGet]
        public ActionResult paypal_confirmation(Int32 id = 0)
        {
            // Get the payer id
            string payerId = "";
            if (Request.Params["PayerID"] != null)
            {
                payerId = Server.UrlDecode(Request.Params["PayerID"]);
            }

            // Get the order
            Order order = Order.GetOneById(id);

            // Make sure that the order not is null
            if(order == null)
            {
                // Redirect the user to the order confirmation page
                return RedirectToAction("index", "home");
            }

            // Get the webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

            // Get credentials
            string paypalClientId = webshopSettings.Get("PAYPAL-CLIENT-ID");
            string paypalClientSecret = webshopSettings.Get("PAYPAL-CLIENT-SECRET");
            string paypalMode = webshopSettings.Get("PAYPAL-MODE");
            Dictionary<string, string> config = new Dictionary<string, string> { { "mode", paypalMode } };

            // Create a error message
            string error_message = "";

            // Create a payment variable
            PayPal.Api.Payments.Payment createdPayment = null;

            try
            {
                // Create the credential token
                PayPal.OAuthTokenCredential tokenCredential = new PayPal.OAuthTokenCredential(paypalClientId, paypalClientSecret, config);

                // Create the api context
                PayPal.APIContext paypalContext = new PayPal.APIContext(tokenCredential.GetAccessToken());
                paypalContext.Config = config;

                // Get the payment
                PayPal.Api.Payments.Payment payment = PayPal.Api.Payments.Payment.Get(paypalContext, order.payment_token);

                // Create the payment excecution
                PayPal.Api.Payments.PaymentExecution paymentExecution = new PayPal.Api.Payments.PaymentExecution();
                paymentExecution.payer_id = payerId;
                paypalContext.HTTPHeaders = null;

                // Excecute the payment
                createdPayment = payment.Execute(paypalContext, paymentExecution);

            }
            catch (Exception ex)
            {
                error_message = ex.Message;
            }

            // Check if the created payment is different from null
            if (createdPayment != null && createdPayment.state == "approved")
            {
                // Get the sale id
                List<PayPal.Api.Payments.RelatedResources> resources = createdPayment.transactions[0].related_resources;

                // Save the paypal sale id
                Order.SetPaymentToken(order.id, resources[0].sale.id);

                // Update the order status
                Order.UpdatePaymentStatus(order.id, "payment_status_paid");

                // Add customer files
                CustomerFile.AddCustomerFiles(order);
            }

            // Redirect the user to the order confirmation page
            return RedirectToAction("confirmation", "order", new { id = id });

        } // End of the paypal_confirmation method

        #endregion

        #region Payson payment

        /// <summary>
        /// Create a payson payment
        /// </summary>
        /// <param name="order">A reference to a order</param>
        /// <param name="orderRows">A reference to a list of order rows</param>
        /// <param name="domain">A reference to the domain</param>
        /// <param name="tt">A reference to translated texts</param>
        /// <param name="paymentType">The payment type, INVOICE, DIRECT</param>
        /// <returns>A redirect to payson</returns>
        public ActionResult CreatePaysonPayment(Order order, List<OrderRow> orderRows, Domain domain, 
            KeyStringList tt, string paymentType)
        {
            // Get the webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

            // Get the language
            Language language = Language.GetOneById(domain.front_end_language);

            // Get credentials
            string paysonEmail = webshopSettings.Get("PAYSON-EMAIL");
            string userId = webshopSettings.Get("PAYSON-AGENT-ID");
            string md5Key = webshopSettings.Get("PAYSON-MD5-KEY");
            bool paysonTest = false;
            bool.TryParse(webshopSettings.Get("PAYSON-TEST"), out paysonTest);

            // Set urls
            string returnUrl = domain.web_address + "/order/payson_confirmation/" + order.id.ToString();
            string notificationUrl = domain.web_address + "/api/payments/payson_notification/" + order.id.ToString();
            string cancelUrl = domain.web_address + "/order/confirmation/" + order.id.ToString();

            // Create the sender
            PaysonIntegration.Utils.Sender sender = new PaysonIntegration.Utils.Sender(order.customer_email);
            sender.FirstName = "@";
            sender.LastName = "@";

            // Get the total amount
            decimal totalAmount = order.total_sum - order.gift_cards_amount;

            // Create the receiver
            List<PaysonIntegration.Utils.Receiver> receivers = new List<PaysonIntegration.Utils.Receiver>(1);
            PaysonIntegration.Utils.Receiver receiver = new PaysonIntegration.Utils.Receiver(paysonEmail, totalAmount);
            receiver.SetPrimaryReceiver(true);
            receivers.Add(receiver);

            // Create a list of order items
            List<PaysonIntegration.Utils.OrderItem> orderItems = new List<PaysonIntegration.Utils.OrderItem>(10);

            // Add order items
            for (int i = 0; i < orderRows.Count; i++)
            {
                // Create a order item
                PaysonIntegration.Utils.OrderItem orderItem = new PaysonIntegration.Utils.OrderItem(orderRows[i].product_name);
                orderItem.SetOptionalParameters(orderRows[i].product_code, orderRows[i].quantity, orderRows[i].unit_price, orderRows[i].vat_percent);

                // Add the order item to the list
                orderItems.Add(orderItem);
            }

            // Add the rounding
            if(order.rounding_sum != 0)
            {
                PaysonIntegration.Utils.OrderItem roundingItem = new PaysonIntegration.Utils.OrderItem(tt.Get("rounding"));
                roundingItem.SetOptionalParameters("rd", 1, order.rounding_sum, 0);
                orderItems.Add(roundingItem);
            }

            // Add the gift cards amount
            if (order.gift_cards_amount > 0)
            {
                PaysonIntegration.Utils.OrderItem giftCardItem = new PaysonIntegration.Utils.OrderItem(tt.Get("gift_cards"));
                giftCardItem.SetOptionalParameters("gc", 1, order.gift_cards_amount * -1, 0);
                orderItems.Add(giftCardItem);
            }

            // Create funding constraints depending on the payment method
            List<PaysonIntegration.Utils.FundingConstraint> fundingConstraints = new List<PaysonIntegration.Utils.FundingConstraint>();
            if (paymentType == "INVOICE")
            {
                fundingConstraints.Add(PaysonIntegration.Utils.FundingConstraint.Invoice);
            }
            else if(paymentType == "DIRECT")
            {
                fundingConstraints.Add(PaysonIntegration.Utils.FundingConstraint.Bank);
                fundingConstraints.Add(PaysonIntegration.Utils.FundingConstraint.CreditCard);
            }

            // Create the order memo
            string orderMemo = tt.Get("order").ToUpper() + "# " + order.id.ToString();

            // Create the pay data
            PaysonIntegration.Data.PayData payData = new PaysonIntegration.Data.PayData(returnUrl, cancelUrl, orderMemo, sender, receivers);
            payData.SetFundingConstraints(fundingConstraints);
            payData.GuaranteeOffered = PaysonIntegration.Utils.GuaranteeOffered.No;
            payData.SetLocaleCode(AnnytabDataValidation.GetPaysonLocaleCode(language));
            payData.SetCurrencyCode(order.currency_code);
            payData.SetOrderItems(orderItems);
            payData.ShowReceiptPage = false;
            payData.SetIpnNotificationUrl(notificationUrl);
            payData.SetTrackingId(order.id.ToString());

            // Create the api
            PaysonIntegration.PaysonApi paysonApi = new PaysonIntegration.PaysonApi(userId, md5Key, null, paysonTest);

            // Create a respone
            PaysonIntegration.Response.PayResponse response = null;

            try
            {
                // Create the payment
                response = paysonApi.MakePayRequest(payData);
            }
            catch(Exception ex)
            {
                string exMessage = ex.Message;
            }
            
            // Check if the response is successful
            string forwardUrl = "";
            if (response != null && response.Success == true)
            {
                // Update the order with the payment token
                Order.SetPaymentToken(order.id, response.Token);

                // Get the forward url
                forwardUrl = paysonApi.GetForwardPayUrl(response.Token);
 
                // Redirect the user to the forward url
                return Redirect(forwardUrl);
            }
            else
            {
                // Redirect the user to the order confirmation page
                return RedirectToAction("confirmation", "order", new { id = order.id });
            }

        } // End of the CreatePaysonPayment

        // Get the payson confirmation
        // GET: /order/payson_confirmation
        [HttpGet]
        public ActionResult payson_confirmation(Int32 id = 0)
        {
            // Get the webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

            // Get the order
            Order order = Order.GetOneById(id);

            // Make sure that the order not is null
            if (order == null)
            {
                // Redirect the user to the order confirmation page
                return RedirectToAction("index", "home");
            }

            // Get the payment token
            string token = order.payment_token;

            // Get credentials
            string userId = webshopSettings.Get("PAYSON-AGENT-ID");
            string md5Key = webshopSettings.Get("PAYSON-MD5-KEY");
            bool paysonTest = false;
            bool.TryParse(webshopSettings.Get("PAYSON-TEST"), out paysonTest);

            // Create the payson api
            PaysonIntegration.PaysonApi paysonApi = new PaysonIntegration.PaysonApi(userId, md5Key, null, paysonTest);

            // Create the response
            PaysonIntegration.Response.PaymentDetailsResponse response = null;

            try
            {
                // Make the payment details request
                response = paysonApi.MakePaymentDetailsRequest(new PaysonIntegration.Data.PaymentDetailsData(token));
            }
            catch(Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Check if the response is successful or not
            if (response != null && response.Success == true)
            {
                // Get the type and status of the payment
                PaysonIntegration.Utils.PaymentType? paymentType = response.PaymentDetails.PaymentType;
                PaysonIntegration.Utils.PaymentStatus? paymentStatus = response.PaymentDetails.PaymentStatus;
                PaysonIntegration.Utils.InvoiceStatus? invoiceStatus = response.PaymentDetails.InvoiceStatus;

                // Check if the paymentstatus is completed or ordercreated
                if (paymentType == PaysonIntegration.Utils.PaymentType.Direct && paymentStatus == PaysonIntegration.Utils.PaymentStatus.Completed)
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_paid");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if (paymentType == PaysonIntegration.Utils.PaymentType.Invoice && invoiceStatus == PaysonIntegration.Utils.InvoiceStatus.OrderCreated)
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_invoice_approved");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
            }

            // Redirect the user to the order confirmation page
            return RedirectToAction("confirmation", "order", new { id = id });

        } // End of the payson_confirmation method

        #endregion

        #region Svea Payment

        /// <summary>
        /// Create a new svea payment
        /// </summary>
        /// <param name="order">A reference to a order</param>
        /// <param name="orderRows">A reference to order rows</param>
        /// <param name="domain">A reference to the current domain</param>
        /// <param name="tt">A reference to translated texts</param>
        /// <param name="paymentType">The payment type, INVOICE</param>
        [HttpGet]
        public ActionResult CreateSveaPayment(Order order, List<OrderRow> orderRows, Domain domain, KeyStringList tt, string paymentType)
        {
            // Get webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

            // Get the language
            Language language = Language.GetOneById(domain.front_end_language);

            // Create the payment configuration
            SveaSettings sveaConfiguration = new SveaSettings();

            // Create the order builder
            Webpay.Integration.CSharp.Order.Create.CreateOrderBuilder orderBuilder = Webpay.Integration.CSharp.WebpayConnection.CreateOrder(sveaConfiguration);

            // Add order rows
            for (int i = 0; i < orderRows.Count; i++)
            {
                // Get the unit
                Unit unit = Unit.GetOneById(orderRows[i].unit_id, domain.front_end_language);
                unit = unit != null ? unit : new Unit();

                // Create an order item
                Webpay.Integration.CSharp.Order.Row.OrderRowBuilder orderItem = new Webpay.Integration.CSharp.Order.Row.OrderRowBuilder();
                orderItem.SetArticleNumber(orderRows[i].product_code);
                orderItem.SetName(orderRows[i].product_name);
                orderItem.SetQuantity(orderRows[i].quantity);
                orderItem.SetUnit(unit.unit_code);
                orderItem.SetAmountExVat(orderRows[i].unit_price);
                orderItem.SetVatPercent(orderRows[i].vat_percent * 100);

                // Add the order item
                orderBuilder.AddOrderRow(orderItem);
            }

            // Add the gift cards amount
            if (order.gift_cards_amount > 0)
            {
                Webpay.Integration.CSharp.Order.Row.OrderRowBuilder orderItem = new Webpay.Integration.CSharp.Order.Row.OrderRowBuilder();
                orderItem.SetArticleNumber("gc");
                orderItem.SetName(tt.Get("gift_cards"));
                orderItem.SetQuantity(1);
                orderItem.SetUnit("");
                orderItem.SetAmountExVat(order.gift_cards_amount * -1);
                orderItem.SetVatPercent(0);

                // Add the order item
                orderBuilder.AddOrderRow(orderItem);
            }

            // Add the customer
            if(order.customer_type == 0) // Company
            {
                // Set values for the customer
                Webpay.Integration.CSharp.Order.Identity.CompanyCustomer company = new Webpay.Integration.CSharp.Order.Identity.CompanyCustomer();
                company.SetNationalIdNumber(order.customer_org_number);
                company.SetVatNumber(order.customer_vat_number);
                company.SetCompanyName(order.invoice_name);
                company.SetEmail(order.customer_email);
                company.SetPhoneNumber(order.customer_phone);
                company.SetIpAddress(Tools.GetUserIP());

                // Get addresses
                //Webpay.Integration.CSharp.WebpayWS.GetCustomerAddressesResponse response = Webpay.Integration.CSharp.WebpayConnection.GetAddresses(sveaConfiguration)
                //.SetCountryCode(SveaSettings.GetSveaCountryCode(order.country_code)).SetCompany(order.customer_org_number).SetZipCode(order.invoice_post_code).SetOrderTypeInvoice().DoRequest();

                // Add customer details
                orderBuilder.AddCustomerDetails(company);

            }
            else if(order.customer_type == 1) // Person
            {
                // Set values for the customer
                Webpay.Integration.CSharp.Order.Identity.IndividualCustomer person = new Webpay.Integration.CSharp.Order.Identity.IndividualCustomer();
                person.SetNationalIdNumber(order.customer_org_number);
                person.SetEmail(order.customer_email);
                person.SetPhoneNumber(order.customer_phone);
                person.SetIpAddress(Request.UserHostAddress);

                // Get addresses
                //Webpay.Integration.CSharp.WebpayWS.GetCustomerAddressesResponse response = Webpay.Integration.CSharp.WebpayConnection.GetAddresses(sveaConfiguration)
                //.SetCountryCode(SveaSettings.GetSveaCountryCode(order.country_code)).SetIndividual(order.customer_org_number).SetZipCode(order.invoice_post_code).SetOrderTypeInvoice().DoRequest();

                // Add customer details
                orderBuilder.AddCustomerDetails(person);
            }

            // Get the test variable
            string testOrderString = "";
            if (webshopSettings.Get("SVEA-TEST").ToLower() == "true")
            {
                testOrderString = ":" + Tools.GeneratePassword();
            }

            // Set order values
            orderBuilder.SetCountryCode(SveaSettings.GetSveaCountryCode(order.country_code));
            orderBuilder.SetOrderDate(order.order_date);
            orderBuilder.SetClientOrderNumber(order.id.ToString() + testOrderString);
            orderBuilder.SetCurrency(SveaSettings.GetSveaCurrency(order.currency_code));

            // Create the payment
            if(paymentType == "INVOICE") // Invoice
            {
                // Create the invoice payment
                Webpay.Integration.CSharp.WebpayWS.CreateOrderEuResponse orderResponse = orderBuilder.UseInvoicePayment().DoRequest();

                // Check if the order is accepted
                if(orderResponse.Accepted == true)
                {
                    // Update the order with the payment token
                    Order.SetPaymentToken(order.id, orderResponse.CreateOrderResult.SveaOrderId.ToString());

                    // Update the payment status
                    Order.UpdatePaymentStatus(order.id, "payment_status_invoice_approved");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else
                {
                    // Update the payment status
                    Order.UpdatePaymentStatus(order.id, "payment_status_invoice_not_approved");
                }
            }
            else if(paymentType == "CREDITCARD")
            {
                // Add the rounding
                if(order.rounding_sum != 0)
                {
                    Webpay.Integration.CSharp.Order.Row.OrderRowBuilder orderItem = new Webpay.Integration.CSharp.Order.Row.OrderRowBuilder();
                    orderItem.SetArticleNumber("rd");
                    orderItem.SetName(tt.Get("rounding"));
                    orderItem.SetQuantity(1);
                    orderItem.SetUnit("");
                    orderItem.SetAmountExVat(order.rounding_sum);
                    orderItem.SetVatPercent(0);

                    // Add the order item
                    orderBuilder.AddOrderRow(orderItem);
                }

                // Set the payment option
                Webpay.Integration.CSharp.Hosted.Payment.PaymentMethodPayment payment = orderBuilder.UsePaymentMethod(Webpay.Integration.CSharp.Util.Constant.PaymentMethod.KORTCERT);
                payment.SetReturnUrl(domain.web_address + "/order/svea_confirmation/" + order.id.ToString());
                payment.SetCancelUrl(domain.web_address + "/order/confirmation/" + order.id.ToString());
                payment.SetPayPageLanguageCode(SveaSettings.GetSveaLanguageCode(language.language_code));

                // Get the payment form
                Webpay.Integration.CSharp.Hosted.Helper.PaymentForm paymentForm = payment.GetPaymentForm();
                ViewBag.PaymentForm = paymentForm.GetCompleteForm();
                
                // Return the svea webpay view
                return View("svea_webpay");
            }

            // Redirect the user to the order confirmation page
            return RedirectToAction("confirmation", "order", new { id = order.id });

        } // End of the CreateSveaPayment method

        /// <summary>
        /// Get the svea confirmation response
        /// </summary>
        /// <param name="collection">A collection of form elements</param>
        /// <param name="id">The order id</param>
        [HttpPost]
        public ActionResult svea_confirmation(FormCollection collection, Int32 id = 0)
        {
            // Get the form data
            string response = collection["response"];
            string merchantId = collection["merchantid"];
            string macId = collection["mac"];

            // Get the svea response
            Webpay.Integration.CSharp.Response.Hosted.SveaResponse sveaResponse = new Webpay.Integration.CSharp.Response.Hosted.SveaResponse(response);

            // Get the order
            Order order = Order.GetOneById(id);

            // Make sure that the order not is null
            if (order == null)
            {
                // Redirect the user to the order confirmation page
                return RedirectToAction("index", "home");
            }

            // Set the order as payed
            if(sveaResponse.OrderAccepted == true)
            {
                // Update the payment status
                Order.UpdatePaymentStatus(id, "payment_status_paid");

                // Add customer files
                CustomerFile.AddCustomerFiles(order);
            }

            // Redirect the user to the order confirmation page
            return RedirectToAction("confirmation", "order", new { id = id });

        } // End of the svea_confirmation method

        #endregion

        #region Helper methods

        /// <summary>
        /// Render the order confirmation to a string
        /// </summary>
        /// <param name="orderId">The order id</param>
        /// <param name="context">The current controller context</param>
        /// <returns>A string with html code for the order confirmation</returns>
        public static string RenderOrderConfirmationView(Int32 orderId, ControllerContext context)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the order
            Order order = Order.GetOneById(orderId);

            // Set form values
            context.Controller.ViewBag.CurrentDomain = currentDomain;
            context.Controller.ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");
            context.Controller.ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            context.Controller.ViewBag.CurrentCategory = new Category();
            context.Controller.ViewBag.Order = order;
            context.Controller.ViewBag.OrderRows = OrderRow.GetByOrderId(order.id);
            context.Controller.ViewBag.InvoiceCountry = Country.GetOneById(order.invoice_country_id, currentDomain.front_end_language);
            context.Controller.ViewBag.DeliveryCountry = Country.GetOneById(order.delivery_country_id, currentDomain.front_end_language);
            context.Controller.ViewBag.PaymentOption = PaymentOption.GetOneById(order.payment_option, currentDomain.front_end_language);
            context.Controller.ViewBag.Company = Company.GetOneById(order.company_id);
            context.Controller.ViewBag.CultureInfo = Tools.GetCultureInfo(context.Controller.ViewBag.CurrentLanguage);

            // Find the ViewEngine for this view
            string viewName = currentDomain.custom_theme_id == 0 ? "/Views/order/_confirmation_body.cshtml" : "/Views/theme/order_confirmation_body.cshtml";
            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewName);

            // Get the view
            IView view = viewEngineResult.View;

            // Set the start tag for the html document
            string htmlString = "<html><body>";

            using (StringWriter stringWriter = new StringWriter())
            {

                ViewContext viewContext = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, stringWriter);
                view.Render(viewContext, stringWriter);

                // Add the body
                htmlString += stringWriter.ToString();
            }

            // Add the end tag for the html document
            htmlString += "</body></html>";

            // Return the html string
            return htmlString;

        } // End of the RenderOrderConfirmationView method

        /// <summary>
        /// Process gift cards and return the total amount of processed gift cards
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <param name="orderTotalSum">The total amount for the order</param>
        /// <returns>The total amount of processed gift cards</returns>
        private decimal ProcessGiftCards(Int32 orderId, decimal orderTotalSum)
        {
            // Create the decimal to return
            decimal gift_cards_amount = 0;

            // Get all of the gift cards
            List<GiftCard> giftCards = (List<GiftCard>)Session["GiftCards"];

            // Make sure that the list not is null
            if (giftCards != null)
            {
                // Loop the list of gift cards
                for (int i = 0; i < giftCards.Count; i++)
                {
                    // Create an order gift card post
                    OrderGiftCard orderGiftCard = new OrderGiftCard();
                    orderGiftCard.order_id = orderId;
                    orderGiftCard.gift_card_id = giftCards[i].id;

                    // Calculate the remaining difference
                    decimal diff = orderTotalSum - gift_cards_amount;

                    // Check if the gift card amount is greater than the order sum or not
                    if (giftCards[i].amount <= diff)
                    {
                        // Update the gift card
                        gift_cards_amount += giftCards[i].amount;
                        orderGiftCard.amount = giftCards[i].amount;
                        giftCards[i].amount = 0;
                        GiftCard.Update(giftCards[i]);

                        // Add the order gift card
                        OrderGiftCard.Add(orderGiftCard);
                    }
                    else
                    {
                        // Update the gift card
                        giftCards[i].amount -= diff;
                        GiftCard.Update(giftCards[i]);
                        gift_cards_amount += diff;

                        // Add the order gift card
                        orderGiftCard.amount = diff;
                        OrderGiftCard.Add(orderGiftCard);

                        // Break out from the loop
                        break;
                    }
                }
            }

            // Return the amount for gift cards
            return gift_cards_amount;

        } // End of the ProcessGiftCards method

        #endregion

    } // End of the class

} // End of the namespace