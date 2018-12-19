using System;

/// <summary>
/// This class handles the configuration of svea payments
/// </summary>
public class SveaSettings : Webpay.Integration.CSharp.Config.IConfigurationProvider
{
    /// <summary>
    /// Get the username for svea web pay
    /// </summary>
    /// <param name="type"> eg. HOSTED, INVOICE or PAYMENTPLAN</param>
    /// <param name="country">country code</param>
    /// <returns>The user name as a string</returns>
    public string GetUsername(Webpay.Integration.CSharp.Util.Constant.PaymentType type, Webpay.Integration.CSharp.Util.Constant.CountryCode country)
    {
        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.INVOICE || type == Webpay.Integration.CSharp.Util.Constant.PaymentType.PAYMENTPLAN)
        {
            switch (country)
            {
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.SE:
                    return webshopSettings.Get("SVEA-SE-USER-NAME");
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.NO:
                    return webshopSettings.Get("SVEA-NO-USER-NAME");
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.FI:
                    return webshopSettings.Get("SVEA-FI-USER-NAME");
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.DK:
                    return webshopSettings.Get("SVEA-DK-USER-NAME");
            }
        }
        return "";

    } // End of the GetUsername method

    /// <summary>
    /// Get the password for svea web pay
    /// </summary>
    /// <param name="type"> eg. HOSTED, INVOICE or PAYMENTPLAN</param>
    /// <param name="country">country code</param>
    /// <returns>The password as a string</returns>
    public string GetPassword(Webpay.Integration.CSharp.Util.Constant.PaymentType type, Webpay.Integration.CSharp.Util.Constant.CountryCode country)
    {
        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.INVOICE || type == Webpay.Integration.CSharp.Util.Constant.PaymentType.PAYMENTPLAN)
        {
            switch (country)
            {
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.SE:
                    return webshopSettings.Get("SVEA-SE-PASSWORD");
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.NO:
                    return webshopSettings.Get("SVEA-NO-PASSWORD");
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.FI:
                    return webshopSettings.Get("SVEA-FI-PASSWORD");
                case Webpay.Integration.CSharp.Util.Constant.CountryCode.DK:
                    return webshopSettings.Get("SVEA-DK-PASSWORD");
            }
        }
        return "";

    } // End of the GetPassword method

    /// <summary>
    /// Get the client number for svea web pay
    /// </summary>
    /// <param name="type"> eg. HOSTED, INVOICE or PAYMENTPLAN</param>
    /// <param name="country">country code</param>
    /// <returns>The client number as an int</returns>
    public int GetClientNumber(Webpay.Integration.CSharp.Util.Constant.PaymentType type, Webpay.Integration.CSharp.Util.Constant.CountryCode country)
    {
        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        switch (country)
        {
            case Webpay.Integration.CSharp.Util.Constant.CountryCode.SE:
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.INVOICE)
                {
                    return Convert.ToInt32(webshopSettings.Get("SVEA-SE-INVOICE-CLIENT-NUMBER"));
                }
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.PAYMENTPLAN)
                {
                    return 59999;
                }
                break;
            case Webpay.Integration.CSharp.Util.Constant.CountryCode.NO:
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.INVOICE)
                {
                    return Convert.ToInt32(webshopSettings.Get("SVEA-NO-INVOICE-CLIENT-NUMBER"));
                }
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.PAYMENTPLAN)
                {
                    return 32503;
                }
                break;
            case Webpay.Integration.CSharp.Util.Constant.CountryCode.FI:
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.INVOICE)
                {
                    return Convert.ToInt32(webshopSettings.Get("SVEA-FI-INVOICE-CLIENT-NUMBER"));
                }
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.PAYMENTPLAN)
                {
                    return 27136;
                }
                break;
            case Webpay.Integration.CSharp.Util.Constant.CountryCode.DK:
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.INVOICE)
                {
                    return Convert.ToInt32(webshopSettings.Get("SVEA-DK-INVOICE-CLIENT-NUMBER"));
                }
                if (type == Webpay.Integration.CSharp.Util.Constant.PaymentType.PAYMENTPLAN)
                {
                    return 64008;
                }
                break;
        }
        return 0;

    } // End of the GetClientNumber method

    /// <summary>
    /// Get the merchant id for svea web pay
    /// </summary>
    /// <param name="type"> eg. HOSTED, INVOICE or PAYMENTPLAN</param>
    /// <param name="country">country code</param>
    /// <returns>The merchant id as a string</returns>
    public string GetMerchantId(Webpay.Integration.CSharp.Util.Constant.PaymentType type, Webpay.Integration.CSharp.Util.Constant.CountryCode country)
    {
        // Get the company settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Return the merchant id
        return type == Webpay.Integration.CSharp.Util.Constant.PaymentType.HOSTED ? webshopSettings.Get("SVEA-MERCHANT-ID") : "";

    } // End of the GetMerchantId method

    /// <summary>
    /// Get the secret word for svea web pay
    /// </summary>
    /// <param name="type"> eg. HOSTED, INVOICE or PAYMENTPLAN</param>
    /// <param name="country">country code</param>
    /// <returns>The secret word as a string</returns>
    public string GetSecretWord(Webpay.Integration.CSharp.Util.Constant.PaymentType type, Webpay.Integration.CSharp.Util.Constant.CountryCode country)
    {
        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Return the secret word
        return type == Webpay.Integration.CSharp.Util.Constant.PaymentType.HOSTED ? webshopSettings.Get("SVEA-SECRET-WORD") : "";

    } // End of the GetSecretWord method

    /// <summary>
    /// Get the end point for svea web pay
    /// </summary>
    /// <param name="type"> eg. HOSTED, INVOICE or PAYMENTPLAN</param>
    /// <returns>The end point url as a string</returns>
    public string GetEndPoint(Webpay.Integration.CSharp.Util.Constant.PaymentType type)
    {
        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Get the test value
        string sveaTest = webshopSettings.Get("SVEA-TEST");

        // Check if we are in the test mode or the production mode
        if (sveaTest.ToLower() == "true")
        {
            return type == Webpay.Integration.CSharp.Util.Constant.PaymentType.HOSTED ? Webpay.Integration.CSharp.Config.SveaConfig.GetTestPayPageUrl() : Webpay.Integration.CSharp.Config.SveaConfig.GetTestWebserviceUrl();
        }
        else
        {
            return type == Webpay.Integration.CSharp.Util.Constant.PaymentType.HOSTED ? Webpay.Integration.CSharp.Config.SveaConfig.GetProdPayPageUrl() : Webpay.Integration.CSharp.Config.SveaConfig.GetProdWebserviceUrl();
        }
        
    } // End of the GetEndPoint method


    /// <summary>
    /// Get a svea country code
    /// </summary>
    /// <param name="countryCodeString">A country code in two letters</param>
    /// <returns>A svea country code</returns>
    public static Webpay.Integration.CSharp.Util.Constant.CountryCode GetSveaCountryCode(string countryCodeString)
    {
        // Create the country code to return (SE is standard)
        Webpay.Integration.CSharp.Util.Constant.CountryCode countryCode = Webpay.Integration.CSharp.Util.Constant.CountryCode.SE;

        // Check if we should set another country code
        if (countryCodeString.ToLower() == "dk")
        {
            countryCode = Webpay.Integration.CSharp.Util.Constant.CountryCode.DK;
        }
        else if (countryCodeString.ToLower() == "fi")
        {
            countryCode = Webpay.Integration.CSharp.Util.Constant.CountryCode.FI;
        }
        else if (countryCodeString.ToLower() == "no")
        {
            countryCode = Webpay.Integration.CSharp.Util.Constant.CountryCode.NO;
        }
        
        // Return the country code 
        return countryCode;

    } // End of the GetSveaCountryCode method

    /// <summary>
    /// Get the svea currency code
    /// </summary>
    /// <param name="currencyCodeString">The currency code as a 3-letter string</param>
    /// <returns>A svea currency code</returns>
    public static Webpay.Integration.CSharp.Util.Constant.Currency GetSveaCurrency(string currencyCodeString)
    {
        // Create the currency to return
        Webpay.Integration.CSharp.Util.Constant.Currency currency = Webpay.Integration.CSharp.Util.Constant.Currency.SEK;

        // Check if we should set another currency code
        if (currencyCodeString.ToUpper() == "DKK")
        {
            currency = Webpay.Integration.CSharp.Util.Constant.Currency.DKK;
        }
        else if (currencyCodeString.ToUpper() == "EUR")
        {
            currency = Webpay.Integration.CSharp.Util.Constant.Currency.EUR;
        }
        else if (currencyCodeString.ToUpper() == "NOK")
        {
            currency = Webpay.Integration.CSharp.Util.Constant.Currency.NOK;
        }

        // Return the currency
        return currency;

    } // End of the GetSveaCurrency method

    /// <summary>
    /// Get the svea language code
    /// </summary>
    /// <param name="languageCodeString">A two letter language code string</param>
    /// <returns>A svea language code</returns>
    public static Webpay.Integration.CSharp.Util.Constant.LanguageCode GetSveaLanguageCode(string languageCodeString)
    {
        // Create the currency to return
        Webpay.Integration.CSharp.Util.Constant.LanguageCode languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.en;

        // Check if we should set another language code
        if (languageCodeString.ToLower() == "da")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.da;
        }
        else if (languageCodeString.ToLower() == "de")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.de;
        }
        else if (languageCodeString.ToLower() == "es")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.es;
        }
        else if (languageCodeString.ToLower() == "fi")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.fi;
        }
        else if (languageCodeString.ToLower() == "fr")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.fr;
        }
        else if (languageCodeString.ToLower() == "it")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.it;
        }
        else if (languageCodeString.ToLower() == "no")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.no;
        }
        else if (languageCodeString.ToLower() == "sv")
        {
            languageCode = Webpay.Integration.CSharp.Util.Constant.LanguageCode.sv;
        }

        // Return the language code
        return languageCode;

    } // End of the GetSveaLanguageCode method

} // End of the class
