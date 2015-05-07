using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;

/// <summary>
/// This class the payex payment solution
/// </summary>
public static class PayExManager
{
    #region Create order methods

    /// <summary>
    /// Create a order
    /// </summary>
    public static Dictionary<string, string> CreateOrder(Order order, List<OrderRow> orderRows, Domain domain, 
        KeyStringList tt, string paymentType)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if(payExTest == true)
        {
            responseData = CreateOrderTest(order, orderRows, domain, webshopSettings, tt, paymentType);
        }
        else
        {
            responseData = CreateOrderProduction(order, orderRows, domain, webshopSettings, tt, paymentType);
        }

        // Return the response data
        return responseData;

    } // End of the CreateOrder method

    /// <summary>
    /// Create a test order
    /// </summary>
    private static Dictionary<string, string> CreateOrderTest(Order order, List<OrderRow> orderRows, Domain domain, 
        KeyStringList settings, KeyStringList tt, string paymentType)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the currency
        Currency orderCurrency = Currency.GetOneById(order.currency_code);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, orderCurrency.decimals);

        // Add the data
        long accountNumber = 0;
        long.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        string purchaseOperation = "SALE";
        if (paymentType == "INVOICE")
        {
            purchaseOperation = "AUTHORIZATION";
        }
        Int64 price = (Int64)Math.Round((order.total_sum - order.gift_cards_amount) * 100, MidpointRounding.AwayFromZero);
        string priceArgList = "";
        string currency = order.currency_code;
        Int32 vat = 0;
        string orderID = order.id.ToString();
        string productNumber = tt.Get("order").ToUpper() + "# " + order.id;
        string description = domain.webshop_name;
        string clientIPAddress = Tools.GetUserIP();
        string clientIdentifier = "";
        string externalID = "";
        string returnUrl = domain.web_address + "/order/payex_confirmation/" + order.id.ToString();
        string view = paymentType;
        string agreementRef = "";
        string cancelUrl = domain.web_address + "/order/confirmation/" + order.id.ToString();
        string clientLanguage = order.language_code.ToLower() + "-" + order.country_code.ToUpper();

        // Set additional values
        string additionalValues = "USECSS=RESPONSIVEDESIGN";

        // Change to PayEx Payment Gateway 2.0 if the payment type is CREDITCARD
        if(paymentType == "CREDITCARD")
        {
            additionalValues = "RESPONSIVE=1";
        }

        // Create the md5 hash
        string hash = GetMD5Hash(accountNumber.ToString() + purchaseOperation + price.ToString() + priceArgList + currency + vat 
            + orderID.ToString() + productNumber + description + clientIPAddress + clientIdentifier + additionalValues + externalID
            + returnUrl + view + agreementRef + cancelUrl + clientLanguage + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Initialize the order
        string xmlResponse = pxOrder.Initialize8(accountNumber, purchaseOperation, price, priceArgList, currency, vat, orderID, 
            productNumber, description, clientIPAddress, clientIdentifier, additionalValues, externalID, returnUrl, 
            view, agreementRef, cancelUrl, clientLanguage, hash);

        // Create the xml document
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Add data from the xml
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("order_ref", ParseXmlNode(doc, "/payex/orderRef"));
            responseData.Add("redirect_url", ParseXmlNode(doc, "/payex/redirectUrl"));

        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Check if the response data contains the order reference
        if(responseData.ContainsKey("order_ref") == false)
        {
            return responseData;
        }

        // Sum of order rows
        decimal payexSum = 0;

        // Add order rows
        for (int i = 0; i < orderRows.Count; i++)
        {
            // Calculate values
            Int32 quantity = (Int32)orderRows[i].quantity;
            decimal priceValue = Math.Round(orderRows[i].unit_price * quantity * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
            decimal vatValue = priceValue * orderRows[i].vat_percent;
            decimal orderValue = Math.Round((priceValue + vatValue) * 100, MidpointRounding.AwayFromZero) / 100;

            Int64 amount = (Int64)Math.Round((priceValue + vatValue) * 100, MidpointRounding.AwayFromZero);
            Int32 vatPrice = (Int32)Math.Round(vatValue * 100, MidpointRounding.AwayFromZero);
            Int32 vatPercent = (Int32)Math.Round(orderRows[i].vat_percent * 100, MidpointRounding.AwayFromZero);
            hash = GetMD5Hash(accountNumber.ToString() + responseData["order_ref"] + orderRows[i].product_code + orderRows[i].product_name + quantity.ToString()
                + amount.ToString() + vatPrice.ToString() + vatPercent.ToString() + settings.Get("PAYEX-ENCRYPTION-KEY"));

            // Add the order row
            string error = pxOrder.AddSingleOrderLine2(accountNumber, responseData["order_ref"], orderRows[i].product_code, orderRows[i].product_name, "", "", "", "",
                quantity, amount, vatPrice, vatPercent, hash);

            // Add to the payex sum
            payexSum += orderValue;
        }

        // Calculate the rounding amount
        decimal rounding = Math.Round((order.total_sum - payexSum) * 100, MidpointRounding.AwayFromZero) / 100;

        // Add the rounding
        if (rounding != 0)
        {
            Int64 roundingAmount = (Int64)Math.Round(rounding * 100, MidpointRounding.AwayFromZero);
            hash = GetMD5Hash(accountNumber.ToString() + responseData["order_ref"] + "rd" + tt.Get("rounding") + "1"
                    + roundingAmount.ToString() + "0" + "0" + settings.Get("PAYEX-ENCRYPTION-KEY"));
            string errorMessage = pxOrder.AddSingleOrderLine2(accountNumber, responseData["order_ref"], "rd", tt.Get("rounding"), "", "", "", "",
                    1, roundingAmount, 0, 0, hash);
        }

        // Add the gift cards amount
        if(order.gift_cards_amount > 0)
        {
            decimal amount = order.gift_cards_amount > payexSum ? payexSum : order.gift_cards_amount;
            Int64 giftCardsAmount = (Int64)Math.Round(amount * -1 * 100, MidpointRounding.AwayFromZero);
            hash = GetMD5Hash(accountNumber.ToString() + responseData["order_ref"] + "gc" + tt.Get("gift_cards") + "1"
                    + giftCardsAmount.ToString() + "0" + "0" + settings.Get("PAYEX-ENCRYPTION-KEY"));
            string errorMessage = pxOrder.AddSingleOrderLine2(accountNumber, responseData["order_ref"], "gc", tt.Get("gift_cards"), "", "", "", "",
                    1, giftCardsAmount, 0, 0, hash);
        }

        // Check if we should create an invoice
        if (paymentType == "INVOICE")
        {
            if(order.customer_type == 0)
            {
                responseData = CreateInvoiceCorporate(responseData["order_ref"], order, domain, settings, tt);
            }
            else if (order.customer_type == 1)
            {
                responseData = CreateInvoicePrivate(responseData["order_ref"], order, domain, settings, tt);
            }
        }

        // Return the response data
        return responseData;

    } // End of the CreateOrderTest method

    /// <summary>
    /// Create a production order
    /// </summary>
    private static Dictionary<string, string> CreateOrderProduction(Order order, List<OrderRow> orderRows, Domain domain, 
        KeyStringList settings, KeyStringList tt, string paymentType)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the currency
        Currency orderCurrency = Currency.GetOneById(order.currency_code);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, orderCurrency.decimals);

        // Add the data
        long accountNumber = 0;
        long.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        string purchaseOperation = "SALE";
        if (paymentType == "INVOICE")
        {
            purchaseOperation = "AUTHORIZATION";
        }
        Int64 price = (Int64)Math.Round((order.total_sum - order.gift_cards_amount) * 100, MidpointRounding.AwayFromZero);
        string priceArgList = "";
        string currency = order.currency_code;
        Int32 vat = 0;
        string orderID = order.id.ToString();
        string productNumber = tt.Get("order").ToUpper() + "# " + order.id;
        string description = domain.webshop_name;
        string clientIPAddress = Tools.GetUserIP();
        string clientIdentifier = "";
        string externalID = "";
        string returnUrl = domain.web_address + "/order/payex_confirmation/" + order.id.ToString();
        string view = paymentType;
        string agreementRef = "";
        string cancelUrl = domain.web_address + "/order/confirmation/" + order.id.ToString();
        string clientLanguage = order.language_code.ToLower() + "-" + order.country_code.ToUpper();

        // Set additional values
        string additionalValues = "USECSS=RESPONSIVEDESIGN";

        // Change to PayEx Payment Gateway 2.0 if the payment type is CREDITCARD
        if (paymentType == "CREDITCARD")
        {
            additionalValues = "RESPONSIVE=1";
        }

        // Create the md5 hash
        string hash = GetMD5Hash(accountNumber.ToString() + purchaseOperation + price.ToString() + priceArgList + currency + vat
            + orderID.ToString() + productNumber + description + clientIPAddress + clientIdentifier + additionalValues + externalID
            + returnUrl + view + agreementRef + cancelUrl + clientLanguage + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Initialize the order
        string xmlResponse = pxOrder.Initialize8(accountNumber, purchaseOperation, price, priceArgList, currency, vat, orderID,
            productNumber, description, clientIPAddress, clientIdentifier, additionalValues, externalID, returnUrl,
            view, agreementRef, cancelUrl, clientLanguage, hash);

        // Create the xml document
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Add data from the xml
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("order_ref", ParseXmlNode(doc, "/payex/orderRef"));
            responseData.Add("redirect_url", ParseXmlNode(doc, "/payex/redirectUrl"));

        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Check if the response data contains the order reference
        if (responseData.ContainsKey("order_ref") == false)
        {
            return responseData;
        }

        // Sum of order rows
        decimal payexSum = 0;

        // Add order rows
        for (int i = 0; i < orderRows.Count; i++)
        {
            // Calculate values
            Int32 quantity = (Int32)orderRows[i].quantity;
            decimal priceValue = Math.Round(orderRows[i].unit_price * quantity * 100, MidpointRounding.AwayFromZero) / 100;
            decimal vatValue = priceValue * orderRows[i].vat_percent;
            decimal orderValue = Math.Round((priceValue + vatValue) * 100, MidpointRounding.AwayFromZero) / 100;

            Int64 amount = (Int64)Math.Round((priceValue + vatValue) * 100, MidpointRounding.AwayFromZero);
            Int32 vatPrice = (Int32)Math.Round(vatValue * 100, MidpointRounding.AwayFromZero);
            Int32 vatPercent = (Int32)Math.Round(orderRows[i].vat_percent * 100, MidpointRounding.AwayFromZero);
            hash = GetMD5Hash(accountNumber.ToString() + responseData["order_ref"] + orderRows[i].product_code + orderRows[i].product_name + quantity.ToString()
                + amount.ToString() + vatPrice.ToString() + vatPercent.ToString() + settings.Get("PAYEX-ENCRYPTION-KEY"));

            // Add the order row
            string error = pxOrder.AddSingleOrderLine2(accountNumber, responseData["order_ref"], orderRows[i].product_code, orderRows[i].product_name, "", "", "", "",
                quantity, amount, vatPrice, vatPercent, hash);

            // Add to the payex sum
            payexSum += orderValue;
        }

        // Calculate the rounding amount
        decimal rounding = Math.Round((order.total_sum - payexSum) * 100, MidpointRounding.AwayFromZero) / 100;

        // Add the rounding
        if (rounding != 0)
        {
            Int64 roundingAmount = (Int64)Math.Round(rounding * 100, MidpointRounding.AwayFromZero);
            hash = GetMD5Hash(accountNumber.ToString() + responseData["order_ref"] + "rd" + tt.Get("rounding") + "1"
                    + roundingAmount.ToString() + "0" + "0" + settings.Get("PAYEX-ENCRYPTION-KEY"));
            string errorMessage = pxOrder.AddSingleOrderLine2(accountNumber, responseData["order_ref"], "rd", tt.Get("rounding"), "", "", "", "",
                    1, roundingAmount, 0, 0, hash);
        }

        // Add the gift cards amount
        if (order.gift_cards_amount > 0)
        {
            decimal amount = order.gift_cards_amount > payexSum ? payexSum : order.gift_cards_amount;
            Int64 giftCardsAmount = (Int64)Math.Round(amount * -1 * 100, MidpointRounding.AwayFromZero);
            hash = GetMD5Hash(accountNumber.ToString() + responseData["order_ref"] + "gc" + tt.Get("gift_cards") + "1"
                    + giftCardsAmount.ToString() + "0" + "0" + settings.Get("PAYEX-ENCRYPTION-KEY"));
            string errorMessage = pxOrder.AddSingleOrderLine2(accountNumber, responseData["order_ref"], "gc", tt.Get("gift_cards"), "", "", "", "",
                    1, giftCardsAmount, 0, 0, hash);
        }

        // Check if we should create an invoice
        if (paymentType == "INVOICE")
        {
            if (order.customer_type == 0)
            {
                responseData = CreateInvoiceCorporate(responseData["order_ref"], order, domain, settings, tt);
            }
            else if (order.customer_type == 1)
            {
                responseData = CreateInvoicePrivate(responseData["order_ref"], order, domain, settings, tt);
            }
        }

        // Return the response data
        return responseData;

    } // End of the CreateOrderProduction method

    #endregion

    #region Completion methods

    /// <summary>
    /// Complete a order
    /// </summary>
    public static Dictionary<string, string> CompleteOrder(string orderReference)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if (payExTest == true)
        {
            responseData = CompleteOrderTest(orderReference, webshopSettings);
        }
        else
        {
            responseData = CompleteOrderProduction(orderReference, webshopSettings);
        }

        // Return the response data
        return responseData;

    } // End of the CompleteOrder method

    /// <summary>
    /// Complete a test order
    /// </summary>
    private static Dictionary<string, string> CompleteOrderTest(string orderReference, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the account number
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + orderReference + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Complete(accountNumber, orderReference, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
            responseData.Add("already_completed", ParseXmlNode(doc, "/payex/alreadyCompleted"));
            responseData.Add("order_id", ParseXmlNode(doc, "/payex/orderId"));
            responseData.Add("payment_method", ParseXmlNode(doc, "/payex/paymentMethod"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CompleteOrderTest method

    /// <summary>
    /// Complete a production order
    /// </summary>
    private static Dictionary<string, string> CompleteOrderProduction(string orderReference, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the account number
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + orderReference + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Complete(accountNumber, orderReference, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
            responseData.Add("already_completed", ParseXmlNode(doc, "/payex/alreadyCompleted"));
            responseData.Add("order_id", ParseXmlNode(doc, "/payex/orderId"));
            responseData.Add("payment_method", ParseXmlNode(doc, "/payex/paymentMethod"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CompleteOrderProduction method

    #endregion

    #region Capture methods

    /// <summary>
    /// Capture an authorized transaction
    /// </summary>
    public static Dictionary<string, string> CaptureTransaction(Order order)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if (payExTest == true)
        {
            responseData = CaptureTransactionTest(order, webshopSettings);
        }
        else
        {
            responseData = CaptureTransactionProduction(order, webshopSettings);
        }

        // Return the response data
        return responseData;

    } // End of the CaptureTransaction method

    /// <summary>
    /// Capture an authorized test transaction
    /// </summary>
    private static Dictionary<string, string> CaptureTransactionTest(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the currency
        Currency orderCurrency = Currency.GetOneById(order.currency_code);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, orderCurrency.decimals);

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);
        Int64 amount = (Int64)Math.Round(order.total_sum * 100, MidpointRounding.AwayFromZero);
        Int32 vatAmount = (Int32)Math.Round(order.vat_sum * 100, MidpointRounding.AwayFromZero);
        string additionalValues = "";

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + amount.ToString() + order.id.ToString() 
            + vatAmount.ToString() + additionalValues + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Capture5(accountNumber, transactionNumber, amount, order.id.ToString(), vatAmount, additionalValues, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
            responseData.Add("transaction_number_original", ParseXmlNode(doc, "/payex/originalTransactionNumber"));  
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CaptureTransactionTest method

    /// <summary>
    /// Capture an authorized production transaction
    /// </summary>
    private static Dictionary<string, string> CaptureTransactionProduction(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the currency
        Currency orderCurrency = Currency.GetOneById(order.currency_code);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, orderCurrency.decimals);

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);
        Int64 amount = (Int64)Math.Round(order.total_sum * 100, MidpointRounding.AwayFromZero);
        Int32 vatAmount = (Int32)Math.Round(order.vat_sum * 100, MidpointRounding.AwayFromZero);
        string additionalValues = "";

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + amount.ToString() + order.id.ToString()
            + vatAmount.ToString() + additionalValues + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Capture5(accountNumber, transactionNumber, amount, order.id.ToString(), vatAmount, additionalValues, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
            responseData.Add("transaction_number_original", ParseXmlNode(doc, "/payex/originalTransactionNumber"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CaptureTransactionProduction method

    #endregion

    #region Check methods

    /// <summary>
    /// Check a transaction
    /// </summary>
    public static Dictionary<string, string> CheckTransaction(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if (payExTest == true)
        {
            responseData = CheckTransactionTest(order, webshopSettings);
        }
        else
        {
            responseData = CheckTransactionProduction(order, webshopSettings);
        }

        // Return the response data
        return responseData;

    } // End of the CheckTransaction method

    /// <summary>
    /// Check a test transaction
    /// </summary>
    public static Dictionary<string, string> CheckTransactionTest(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);
        
        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Check2(accountNumber, transactionNumber, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CheckTransactionTest method

    /// <summary>
    /// Check a production transaction
    /// </summary>
    public static Dictionary<string, string> CheckTransactionProduction(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Check2(accountNumber, transactionNumber, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CheckTransactionProduction method

    #endregion

    #region Cancel methods

    /// <summary>
    /// Cancel an authorized transaction
    /// </summary>
    public static Dictionary<string, string> CancelTransaction(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if (payExTest == true)
        {
            responseData = CancelTransactionTest(order, webshopSettings);
        }
        else
        {
            responseData = CancelTransactionProduction(order, webshopSettings);
        }

        // Return the response data
        return responseData;

    } // End of the CancelTransaction method

    /// <summary>
    /// Cancel an authorized test transaction
    /// </summary>
    public static Dictionary<string, string> CancelTransactionTest(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Cancel2(accountNumber, transactionNumber, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response paramName
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CancelTransactionTest method

    /// <summary>
    /// Cancel an authorized production transaction
    /// </summary>
    public static Dictionary<string, string> CancelTransactionProduction(Order order, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Cancel2(accountNumber, transactionNumber, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response paramName
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CancelTransactionProduction method

    #endregion

    #region Invoice methods

    /// <summary>
    /// Create a invoice to a person
    /// </summary>
    public static Dictionary<string, string> CreateInvoicePrivate(string orderReference, Order order, Domain domain, KeyStringList settings, KeyStringList tt)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if (payExTest == true)
        {
            responseData = CreateInvoicePrivateTest(orderReference, order, domain, webshopSettings, tt);
        }
        else
        {
            responseData = CreateInvoicePrivateProduction(orderReference, order, domain, webshopSettings, tt);
        }

        // Return the response data
        return responseData;

    } // End of the CreateInvoicePrivate method

    /// <summary>
    /// Create a test invoice for a person
    /// </summary>
    private static Dictionary<string, string> CreateInvoicePrivateTest(string orderReference, Order order, Domain domain, KeyStringList settings, KeyStringList tt)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the data that we need for a credit check
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        string country = order.country_code;
        string socialSecurityNumber = order.customer_org_number;
        string orderRef = orderReference;
        string customerRef = order.customer_id.ToString();
        string customerName = order.invoice_name;
        string streetAddress = order.invoice_address_1;
        string coAddress = "";
        string postalCode = order.invoice_post_code.Replace(" ", "");
        string city = order.invoice_city;
        string phoneNumber = order.customer_mobile_phone;
        string email = order.customer_email;
        string productCode = "";
        string creditCheckRef = "";
        Int32 mediaDistribution = 11;
        string invoiceDate = "";
        Int16 invoiceDueDays = 0;
        Int32 invoiceNumber = 0;
        string invoiceLayout = "";
        string invoiceText = tt.Get("order") + " " + order.id.ToString() + " - " + domain.webshop_name;

        // Create the md5 hash
        string hash = GetMD5Hash(accountNumber.ToString() + orderRef + customerRef + customerName + streetAddress + coAddress
            + postalCode + city + country + socialSecurityNumber + phoneNumber + email + productCode + creditCheckRef + mediaDistribution.ToString()
            + invoiceText + invoiceDate + invoiceDueDays.ToString() + invoiceNumber + invoiceLayout + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Create the invoice
        string xmlResponse = pxOrder.PurchaseInvoicePrivate(accountNumber, orderRef, customerRef, customerName, streetAddress, coAddress,
            postalCode, city, country, socialSecurityNumber, phoneNumber, email, productCode, creditCheckRef, mediaDistribution, invoiceText,
            invoiceDate, invoiceDueDays, invoiceNumber, invoiceLayout, hash);

        // Create the xml document
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Add data from the xml
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));

        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CreateInvoicePrivateTest method

    /// <summary>
    /// Create a production invoice for a person
    /// </summary>
    private static Dictionary<string, string> CreateInvoicePrivateProduction(string orderReference, Order order, Domain domain, KeyStringList settings, KeyStringList tt)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the data that we need for a credit check
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        string country = order.country_code;
        string socialSecurityNumber = order.customer_org_number;
        string orderRef = orderReference;
        string customerRef = order.customer_id.ToString();
        string customerName = order.invoice_name;
        string streetAddress = order.invoice_address_1;
        string coAddress = "";
        string postalCode = order.invoice_post_code.Replace(" ", "");
        string city = order.invoice_city;
        string phoneNumber = order.customer_mobile_phone;
        string email = order.customer_email;
        string productCode = "";
        string creditCheckRef = "";
        Int32 mediaDistribution = 11;
        string invoiceDate = "";
        Int16 invoiceDueDays = 0;
        Int32 invoiceNumber = 0;
        string invoiceLayout = "";
        string invoiceText = tt.Get("order") + " " + order.id.ToString() + " - " + domain.webshop_name;

        // Create the md5 hash
        string hash = GetMD5Hash(accountNumber.ToString() + orderRef + customerRef + customerName + streetAddress + coAddress
            + postalCode + city + country + socialSecurityNumber + phoneNumber + email + productCode + creditCheckRef + mediaDistribution.ToString()
            + invoiceText + invoiceDate + invoiceDueDays.ToString() + invoiceNumber + invoiceLayout + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Create the invoice
        string xmlResponse = pxOrder.PurchaseInvoicePrivate(accountNumber, orderRef, customerRef, customerName, streetAddress, coAddress,
            postalCode, city, country, socialSecurityNumber, phoneNumber, email, productCode, creditCheckRef, mediaDistribution, invoiceText,
            invoiceDate, invoiceDueDays, invoiceNumber, invoiceLayout, hash);

        // Create the xml document
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Add data from the xml
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));

        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CreateInvoicePrivateProduction method

    /// <summary>
    /// Create a invoice to a company
    /// </summary>
    public static Dictionary<string, string> CreateInvoiceCorporate(string orderReference, Order order, Domain domain, KeyStringList settings, KeyStringList tt)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if (payExTest == true)
        {
            responseData = CreateInvoiceCorporateTest(orderReference, order, domain, webshopSettings, tt);
        }
        else
        {
            responseData = CreateInvoiceCorporateProduction(orderReference, order, domain, webshopSettings, tt);
        }

        // Return the response data
        return responseData;

    } // End of the CreateInvoiceCorporate method

    /// <summary>
    /// Create a test invoice for a company
    /// </summary>
    private static Dictionary<string, string> CreateInvoiceCorporateTest(string orderReference, Order order, Domain domain, KeyStringList settings, KeyStringList tt)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the data that we need
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        string orderRef = orderReference;

        string companyRef = order.customer_id.ToString();
        string companyName = order.invoice_name;
        string streetAddress = order.invoice_address_1;
        string coAddress = "";
        string postalCode = order.invoice_post_code.Replace(" ", "");
        string city = order.invoice_city;
        string country = order.country_code;
        string organizationNumber = order.customer_org_number;
        string phoneNumber = order.customer_mobile_phone;
        string email = order.customer_email;
        string productCode = "";
        string creditCheckRef = "";
        Int32 mediaDistribution = 11;
        string invoiceDate = "";
        Int16 invoiceDueDays = 0;
        Int32 invoiceNumber = 0;
        string invoiceLayout = "";
        string invoiceText = tt.Get("order") + " " + order.id.ToString() + " - " + domain.webshop_name;

        // Create the md5 hash
        string hash = GetMD5Hash(accountNumber.ToString() + orderRef + companyRef + companyName + streetAddress + coAddress
            + postalCode + city + country + organizationNumber + phoneNumber + email + productCode + creditCheckRef + mediaDistribution.ToString()
            + invoiceText + invoiceDate + invoiceDueDays.ToString() + invoiceNumber + invoiceLayout + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Create the invoice
        string xmlResponse = pxOrder.PurchaseInvoiceCorporate(accountNumber, orderRef, companyRef, companyName, streetAddress, coAddress,
            postalCode, city, country, organizationNumber, phoneNumber, email, productCode, creditCheckRef, mediaDistribution, invoiceText,
            invoiceDate, invoiceDueDays, invoiceNumber, invoiceLayout, hash);

        // Create the xml document
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Add data from the xml
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));

        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CreateInvoiceCorporateTest method

    /// <summary>
    /// Create a production invoice for a company
    /// </summary>
    private static Dictionary<string, string> CreateInvoiceCorporateProduction(string orderReference, Order order, Domain domain, KeyStringList settings, KeyStringList tt)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the data that we need
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        string orderRef = orderReference;

        string companyRef = order.customer_id.ToString();
        string companyName = order.invoice_name;
        string streetAddress = order.invoice_address_1;
        string coAddress = "";
        string postalCode = order.invoice_post_code.Replace(" ", "");
        string city = order.invoice_city;
        string country = order.country_code;
        string organizationNumber = order.customer_org_number;
        string phoneNumber = order.customer_mobile_phone;
        string email = order.customer_email;
        string productCode = "";
        string creditCheckRef = "";
        Int32 mediaDistribution = 11;
        string invoiceDate = "";
        Int16 invoiceDueDays = 0;
        Int32 invoiceNumber = 0;
        string invoiceLayout = "";
        string invoiceText = tt.Get("order") + " " + order.id.ToString() + " - " + domain.webshop_name;

        // Create the md5 hash
        string hash = GetMD5Hash(accountNumber.ToString() + orderRef + companyRef + companyName + streetAddress + coAddress
            + postalCode + city + country + organizationNumber + phoneNumber + email + productCode + creditCheckRef + mediaDistribution.ToString()
            + invoiceText + invoiceDate + invoiceDueDays.ToString() + invoiceNumber + invoiceLayout + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Create the invoice
        string xmlResponse = pxOrder.PurchaseInvoiceCorporate(accountNumber, orderRef, companyRef, companyName, streetAddress, coAddress,
            postalCode, city, country, organizationNumber, phoneNumber, email, productCode, creditCheckRef, mediaDistribution, invoiceText,
            invoiceDate, invoiceDueDays, invoiceNumber, invoiceLayout, hash);

        // Create the xml document
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Add data from the xml
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/transactionNumber"));

        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CreateInvoiceCorporateProduction method

    #endregion

    #region Credit methods

    /// <summary>
    /// Credit a transaction
    /// </summary>
    public static Dictionary<string, string> CreditTransaction(Order order, List<OrderRow> orderRows, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test variable
        bool payExTest = false;
        bool.TryParse(webshopSettings.Get("PAYEX-TEST"), out payExTest);

        // Check if it is test or production
        if (payExTest == true)
        {
            responseData = CreditTransactionTest(order, orderRows, webshopSettings);
        }
        else
        {
            responseData = CreditTransactionProduction(order, orderRows, webshopSettings);
        }

        // Return the response data
        return responseData;

    } // End of the CreditTransaction method

    /// <summary>
    /// Credit a test transaction
    /// </summary>
    public static Dictionary<string, string> CreditTransactionTest(Order order, List<OrderRow> orderRows, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Test.PxOrder pxOrder = new Annytab.Webshop.Payex.Test.PxOrder();

        // Get the currency
        Currency orderCurrency = Currency.GetOneById(order.currency_code);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, orderCurrency.decimals);

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);
        Int64 amount = (Int64)Math.Round(order.total_sum * 100, MidpointRounding.AwayFromZero);
        Int32 vatAmount = (Int32)Math.Round(order.vat_sum * 100, MidpointRounding.AwayFromZero);
        string additionalValues = "";

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + amount.ToString() + order.id.ToString()
            + vatAmount.ToString() + additionalValues + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Credit5(accountNumber, transactionNumber, amount, order.id.ToString(), vatAmount, additionalValues, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/originalTransactionNumber"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CreditTransactionTest method

    /// <summary>
    /// Credit a production transaction
    /// </summary>
    public static Dictionary<string, string> CreditTransactionProduction(Order order, List<OrderRow> orderRows, KeyStringList settings)
    {
        // Create the dictionary to return
        Dictionary<string, string> responseData = new Dictionary<string, string>(10);

        // Create the PayEx order
        Annytab.Webshop.Payex.Production.PxOrder pxOrder = new Annytab.Webshop.Payex.Production.PxOrder();

        // Get the currency
        Currency orderCurrency = Currency.GetOneById(order.currency_code);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, orderCurrency.decimals);

        // Get the data
        Int64 accountNumber = 0;
        Int64.TryParse(settings.Get("PAYEX-ACCOUNT-NUMBER"), out accountNumber);
        Int32 transactionNumber = 0;
        Int32.TryParse(order.payment_token, out transactionNumber);
        Int64 amount = (Int64)Math.Round(order.total_sum * 100, MidpointRounding.AwayFromZero);
        Int32 vatAmount = (Int32)Math.Round(order.vat_sum * 100, MidpointRounding.AwayFromZero);
        string additionalValues = "";

        // Create the hash
        string hash = GetMD5Hash(accountNumber.ToString() + transactionNumber.ToString() + amount.ToString() + order.id.ToString()
            + vatAmount.ToString() + additionalValues + settings.Get("PAYEX-ENCRYPTION-KEY"));

        // Get the transaction information
        string xmlResponse = pxOrder.Credit5(accountNumber, transactionNumber, amount, order.id.ToString(), vatAmount, additionalValues, hash);

        // Parse the xml response
        XmlDocument doc = new XmlDocument();

        try
        {
            // Load the xml document
            doc.LoadXml(xmlResponse);

            // Get data from the xml response
            responseData.Add("error_code", ParseXmlNode(doc, "/payex/status/errorCode"));
            responseData.Add("description", ParseXmlNode(doc, "/payex/status/description"));
            responseData.Add("parameter_name", ParseXmlNode(doc, "/payex/paramName"));
            responseData.Add("transaction_status", ParseXmlNode(doc, "/payex/transactionStatus"));
            responseData.Add("transaction_number", ParseXmlNode(doc, "/payex/originalTransactionNumber"));
        }
        catch (Exception ex)
        {
            responseData.Add("exception", ex.Message);
        }

        // Return the response data
        return responseData;

    } // End of the CreditTransactionProduction method

    #endregion

    #region Helper methods

    /// <summary>
    /// Get a MD5 hash from a string
    /// </summary>
    /// <param name="stringToHash">The string to hash</param>
    /// <returns>A MD5 hashed string</returns>
    public static string GetMD5Hash(string stringToHash)
    {
        // Create the string to return
        string hashedString = "";

        // Create a hash reference
        MD5 hash = new MD5CryptoServiceProvider();

        // Get the byte array
        byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

        // Loop the byte array
        for (int i = 0; i < data.Length; i++)
        {
            hashedString += data[i].ToString("x2");
        }

        // Return the hashed string
        return hashedString;

    } // End of the GetMD5Hash method

    /// <summary>
    /// Parse an xml node to a string
    /// </summary>
    /// <param name="doc">A reference to a xml document</param>
    /// <param name="xpath">The xpath for the node</param>
    /// <returns>A string</returns>
    private static string ParseXmlNode(XmlDocument doc, string xpath)
    {
        // Create the string to return
        string value = "";

        // Get the node
        XmlNode node = doc.SelectSingleNode(xpath);

        // Get the value
        if(node != null)
        {
            value = node.InnerText;
        }

        // Return the string
        return value;

    } // End of the ParseXmlNode method

    #endregion

} // End of the class