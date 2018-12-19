using System;
using System.Globalization;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Azure;
using Annytab.Stemmer;

/// <summary>
/// This static class includes handy methods
/// </summary>
public static class Tools
{
    /// <summary>
    /// Get the database connection string
    /// </summary>
    /// <returns>A connection string</returns>
    public static string GetConnectionString()
    {
        // Return the connection string
        return CloudConfigurationManager.GetSetting("ConnectionString", false);

    } // End of the GetConnectionString method

    /// <summary>
    /// Get the connection string to a Azure Storage Account
    /// </summary>
    /// <returns>A connection string</returns>
    public static string GetAzureStorageAccount()
    {
        // Return the connection string
        return CloudConfigurationManager.GetSetting("AzureStorageAccount", false);

    } // End of the GetAzureStorageAccount method

    /// <summary>
    /// Get the current domain
    /// </summary>
    /// <returns>The current domain, a empty domain if it is null</returns>
    public static Domain GetCurrentDomain()
    {
        // Get the domain name
        string domainName = HttpContext.Current.Request.Url.Host;

        // Replace www.
        domainName = domainName.Replace("www.", "");

        // Get the domain post
        Domain domain = Domain.GetOneByDomainName(domainName);

        // Make sure that the domain not is null
        if(domain == null)
        {
            domain = new Domain();
            domain.id = 0;
            domain.webshop_name = "";
            domain.domain_name = "localhost";
            domain.web_address = "https://localhost:443";
            domain.country_id = 1;
            domain.front_end_language = 2;
            domain.back_end_language = 2;
            domain.currency = "SEK";
            domain.company_id = 1;
            domain.default_display_view = 0;
            domain.custom_theme_id = 0;
            domain.prices_includes_vat = false;
            domain.analytics_tracking_id = "";
            domain.facebook_app_id = "";
            domain.facebook_app_secret = "";
            domain.google_app_id = "";
            domain.google_app_secret = "";
            domain.noindex = true;
        }

        // Return the current front end language id
        return domain;

    } // End of the GetCurrentDomain method

    /// <summary>
    /// Get the stemmer based on the language
    /// </summary>
    /// <param name="language">A reference to the language</param>
    /// <returns>A reference to a Stemmer</returns>
    public static Stemmer GetStemmer(Language language)
    {
        // Create a default stemmer
        Stemmer stemmer = new DefaultStemmer();

        // Get the language code in lower case
        string language_code = language.language_code.ToLower();

        // Get a stemmer depending on the language
        if (language_code == "da")
        {
            stemmer = new DanishStemmer();
        }
        else if (language_code == "nl")
        {
            stemmer = new DutchStemmer();
        }
        else if (language_code == "en")
        {
            stemmer = new EnglishStemmer();
        }
        else if (language_code == "fi")
        {
            stemmer = new FinnishStemmer();
        }
        else if (language_code == "fr")
        {
            stemmer = new FrenchStemmer();
        }
        else if (language_code == "de")
        {
            stemmer = new GermanStemmer();
        }
        else if (language_code == "it")
        {
            stemmer = new ItalianStemmer();
        }
        else if (language_code == "no")
        {
            stemmer = new NorwegianStemmer();
        }
        else if (language_code == "pt")
        {
            stemmer = new PortugueseStemmer();
        }
        else if (language_code == "ro")
        {
            stemmer = new RomanianStemmer();
        }
        else if (language_code == "es")
        {
            stemmer = new SpanishStemmer();
        }
        else if (language_code == "sv")
        {
            stemmer = new SwedishStemmer();
        }

        // Return the stemmer
        return stemmer;

    } // End of the GetStemmer method

    /// <summary>
    /// Get the culture info
    /// </summary>
    /// <param name="language">A reference to a language</param>
    /// <returns>A reference to a culture info object</returns>
    public static CultureInfo GetCultureInfo(Language language)
    {
        // Create the culture info to return
        CultureInfo cultureInfo = new CultureInfo("en-US");

        // Create the culture info to return
        try
        {
            cultureInfo = new CultureInfo(language.language_code.ToLower() + "-" + language.country_code.ToUpper());
        }
        catch(Exception ex)
        {
            string exMessage = ex.Message;
            cultureInfo = new CultureInfo("en-US");
        }          

        // Return the culture info
        return cultureInfo;

    } // End of the GetCultureInfo method

    /// <summary>
    /// This method is used to generate a random password
    /// </summary>
    /// <returns>A password</returns>
    public static string GeneratePassword()
    {
        // Create the string to return
        string passwordString = "";

        // Create variables for the password generation
        Char[] possibleCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        Random randGenerator = new Random();

        // Create the password
        for (int i = 0; i < 10; i++)
        {
            int randomNumber = randGenerator.Next(possibleCharacters.Length);
            passwordString += possibleCharacters[randomNumber];
        }

        // Return the password string
        return passwordString;

    } // End of the GeneratePassword method

    /// <summary>
    /// Send an email to the webmaster
    /// </summary>
    /// <param name="customerAddress">The customer address</param>
    /// <param name="subject">The subject for the mail message</param>
    /// <param name="message">The message</param>
    public static bool SendEmailToHost(string customerAddress, string subject, string message)
    {
        // Create the boolean to return
        bool successful = true;

        // Get the company settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Create variables
        string host = webshopSettings.Get("SEND-EMAIL-HOST");
        Int32 port = 0;
        Int32.TryParse(webshopSettings.Get("SEND-EMAIL-PORT"), out port);
        string emailAddress = webshopSettings.Get("SEND-EMAIL-ADDRESS");
        string password = webshopSettings.Get("SEND-EMAIL-PASSWORD");
        string toAddress = webshopSettings.Get("CONTACT-US-EMAIL");
        string useSSL = webshopSettings.Get("SEND-EMAIL-USE-SSL");

        // Get the customer email
        MailAddress copyAddress = AnnytabDataValidation.IsEmailAddressValid(customerAddress);

        // Create the SmtpClient instance
        SmtpClient smtp = new SmtpClient(host, port);
        smtp.Credentials = new NetworkCredential(emailAddress, password);

        // Check if we should enable SSL
        if(useSSL.ToLower() == "true")
        {
            smtp.EnableSsl = true;
        }

        // Try to send the mail message
        try
        {
            // Create the mail message instance
            MailMessage mailMessage = new MailMessage(emailAddress, toAddress);

            // Add a carbon copy to the customer
            if(copyAddress != null)
            {
                mailMessage.CC.Add(copyAddress);
            }

            // Create the mail message
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            // Send the mail message
            smtp.Send(mailMessage);

        }
        catch (Exception ex)
        {
            string exceptionMessage = ex.Message;
            successful = false;
        }

        // Return the boolean
        return successful;

    } // End of the SendEmailToHost method

    /// <summary>
    /// Send an email to the customer service
    /// </summary>
    /// <param name="customerAddress">The customer address</param>
    /// <param name="subject">The subject for the mail message</param>
    /// <param name="message">The message</param>
    public static bool SendEmailToCustomerService(string customerAddress, string subject, string message)
    {
        // Create the boolean to return
        bool successful = true;

        // Get the company settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Create variables
        string host = webshopSettings.Get("SEND-EMAIL-HOST");
        Int32 port = 0;
        Int32.TryParse(webshopSettings.Get("SEND-EMAIL-PORT"), out port);
        string emailAddress = webshopSettings.Get("SEND-EMAIL-ADDRESS");
        string password = webshopSettings.Get("SEND-EMAIL-PASSWORD");
        string toAddress = webshopSettings.Get("CUSTOMER-SERVICE-EMAIL");
        string useSSL = webshopSettings.Get("SEND-EMAIL-USE-SSL");

        // Get the customer email
        MailAddress copyAddress = AnnytabDataValidation.IsEmailAddressValid(customerAddress);

        // Create the SmtpClient instance
        SmtpClient smtp = new SmtpClient(host, port);
        smtp.Credentials = new NetworkCredential(emailAddress, password);

        // Check if we should enable SSL
        if (useSSL.ToLower() == "true")
        {
            smtp.EnableSsl = true;
        }

        // Try to send the mail message
        try
        {
            // Create the mail message instance
            MailMessage mailMessage = new MailMessage(emailAddress, toAddress);

            // Add a carbon copy to the customer
            if (copyAddress != null)
            {
                mailMessage.CC.Add(copyAddress);
            }

            // Create the mail message
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            // Send the mail message
            smtp.Send(mailMessage);

        }
        catch (Exception ex)
        {
            string exceptionMessage = ex.Message;
            successful = false;
        }

        // Return the boolean
        return successful;

    } // End of the SendEmailToCustomerService method

    /// <summary>
    /// Send an email to a customer
    /// </summary>
    /// <param name="toAddress">The address to send the email to</param>
    /// <param name="subject">The subject for the mail message</param>
    /// <param name="message">The mail message</param>
    public static bool SendEmailToCustomer(string toAddress, string subject, string message)
    {
        // Create the boolean to return
        bool successful = true;

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Create variables
        string host = webshopSettings.Get("SEND-EMAIL-HOST");
        Int32 port = 0;
        Int32.TryParse(webshopSettings.Get("SEND-EMAIL-PORT"), out port);
        string emailAddress = webshopSettings.Get("SEND-EMAIL-ADDRESS");
        string password = webshopSettings.Get("SEND-EMAIL-PASSWORD");
        string useSSL = webshopSettings.Get("SEND-EMAIL-USE-SSL");

        // Create the SmtpClient instance
        SmtpClient smtp = new SmtpClient(host, port);
        smtp.Credentials = new NetworkCredential(emailAddress, password);

        // Check if we should enable SSL
        if (useSSL.ToLower() == "true")
        {
            smtp.EnableSsl = true;
        }

        // Try to send the mail message
        try
        {

            // Create the mail message instance
            MailMessage mailMessage = new MailMessage(emailAddress, toAddress);

            // Create the mail message
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            // Send the mail message
            smtp.Send(mailMessage);

        }
        catch (Exception ex)
        {
            string exceptionMessage = ex.Message;
            successful = false;
        }

        // Return the boolean
        return successful;

    } // End of the SendEmailToCustomer method

    /// <summary>
    /// Send a newsletter to all the customers that want one
    /// </summary>
    /// <param name="toAddresses">The addresses to send the email to</param>
    /// <param name="subject">The subject for the mail message</param>
    /// <param name="message">The mail message</param>
    public static bool SendNewsletter(string toAddresses, string subject, string message)
    {
        // Create the boolean to return
        bool successful = true;

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Create variables
        string host = webshopSettings.Get("SEND-EMAIL-HOST");
        Int32 port = 0;
        Int32.TryParse(webshopSettings.Get("SEND-EMAIL-PORT"), out port);
        string emailAddress = webshopSettings.Get("SEND-EMAIL-ADDRESS");
        string password = webshopSettings.Get("SEND-EMAIL-PASSWORD");
        string contactUsEmail = webshopSettings.Get("CONTACT-US-EMAIL");
        string useSSL = webshopSettings.Get("SEND-EMAIL-USE-SSL");

        // Replace semicolon with comma
        toAddresses = toAddresses.Replace(";", ",");

        // Create the SmtpClient instance
        SmtpClient smtp = new SmtpClient(host, port);
        smtp.Credentials = new NetworkCredential(emailAddress, password);

        // Check if we should enable SSL
        if (useSSL.ToLower() == "true")
        {
            smtp.EnableSsl = true;
        }

        // Try to send the mail message
        try
        {
            // Create the mail message instance
            MailMessage mailMessage = new MailMessage(emailAddress, contactUsEmail);

            // Create the mail message
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.Bcc.Add(toAddresses);

            // Send the mail message
            smtp.Send(mailMessage);

        }
        catch (Exception ex)
        {
            string exceptionMessage = ex.Message;
            successful = false;
        }

        // Return the boolean
        return successful;

    } // End of the SendNewsletter method

    /// <summary>
    /// Send an order confirmation
    /// </summary>
    /// <param name="toAddress">The address to send the email to</param>
    /// <param name="subject">The subject for the mail message</param>
    /// <param name="message">The mail message</param>
    public static bool SendOrderConfirmation(string toAddress, string subject, string message)
    {
        // Create the boolean to return
        bool successful = true;

        // Get the webshop settings
        KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

        // Create variables
        string host = webshopSettings.Get("SEND-EMAIL-HOST");
        Int32 port = 0;
        Int32.TryParse(webshopSettings.Get("SEND-EMAIL-PORT"), out port);
        string emailAddress = webshopSettings.Get("SEND-EMAIL-ADDRESS");
        string password = webshopSettings.Get("SEND-EMAIL-PASSWORD");
        string useSSL = webshopSettings.Get("SEND-EMAIL-USE-SSL");

        // Order email
        string orderEmail = webshopSettings.Get("ORDER-EMAIL");

        // Create the SmtpClient instance
        SmtpClient smtp = new SmtpClient(host, port);
        smtp.Credentials = new NetworkCredential(emailAddress, password);

        // Check if we should enable SSL
        if (useSSL.ToLower() == "true")
        {
            smtp.EnableSsl = true;
        }

        // Try to send the mail message
        try
        {
            // Get the mail address
            MailAddress mailToAddress = new MailAddress(toAddress);

            // Create the mail message instance
            MailMessage mailMessage = new MailMessage(emailAddress, toAddress);

            // Create the mail message
            mailMessage.CC.Add(new MailAddress(orderEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            // Send the mail message
            smtp.Send(mailMessage);

        }
        catch (Exception ex)
        {
            string exceptionMessage = ex.Message;
            successful = false;
        }

        // Return the boolean
        return successful;

    } // End of the SendEmailToCustomer method

    /// <summary>
    /// Get image urls for the domain
    /// </summary>
    /// <param name="domainId">The id for the domain</param>
    /// <returns>A key string list with urls</returns>
    public static KeyStringList GetDomainImageUrls(Int32 domainId)
    {
        // Create the list to return
        KeyStringList domainImageUrls = new KeyStringList(5);

        // Create paths
        string directoryPath = "/Content/domains/" + domainId.ToString() + "/images/";

        // Add images to the key string list
        domainImageUrls.Add("background_image", directoryPath + "background_image.jpg");
        domainImageUrls.Add("default_logotype", directoryPath + "default_logotype.jpg");
        domainImageUrls.Add("mobile_logotype", directoryPath + "mobile_logotype.jpg");
        domainImageUrls.Add("big_icon", directoryPath + "big_icon.jpg");
        domainImageUrls.Add("small_icon", directoryPath + "small_icon.jpg");

        // Return the list
        return domainImageUrls;

    } // End of the GetDomainImageUrls method

    /// <summary>
    /// Get the campaign image url
    /// </summary>
    /// <param name="string">The filename of the image</param>
    /// <returns>A string with a relative url to the campaign image</returns>
    public static string GetCampaignImageUrl(string imageFileName)
    {
        // Create the string to return
        string url = "/Content/campaigns/images/" + imageFileName;

        // Return the url
        return url;

    } // End of the GetCampaignImageUrl method

    /// <summary>
    /// Get the main image url for a category
    /// </summary>
    /// <param name="categoryId">The category id</param>
    /// <param name="languageId">The language id</param>
    /// <param name="useLocalImages">A boolean that indicates if local images should be used</param>
    /// <returns>An image url as a string</returns>
    public static string GetCategoryMainImageUrl(Int32 categoryId, Int32 languageId, bool useLocalImages)
    {
        // Set the language id
        languageId = useLocalImages == true ? languageId : 0;

        // Create the main image url
        string categoryMainImageUrl = "/Content/categories/" + (categoryId / 100).ToString() + "/" + categoryId.ToString() + "/" + languageId.ToString() + "/main_image.jpg";

        // Return the image url
        return categoryMainImageUrl;

    } // End of the GetCategoryMainImageUrl

    /// <summary>
    /// Get the main image url for the product
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    /// <param name="variantImageUrl">The variant image url</param>
    /// <param name="useLocalImages">A boolean that indicates if local images should be used</param>
    /// <returns>An image url as a string</returns>
    public static string GetProductMainImageUrl(Int32 productId, Int32 languageId, string variantImageUrl, bool useLocalImages)
    {
        // Set the language id
        languageId = useLocalImages == true ? languageId : 0;

        // Create the main image url
        string productMainImageUrl = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/main_image.jpg";

        if(variantImageUrl != "")
        {
            productMainImageUrl = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/other_images/" + variantImageUrl;
        }

        // Return the image url
        return productMainImageUrl;

    } // End of the GetProductMainImageUrl method

    /// <summary>
    /// Get a list of urls to other product images
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    /// <param name="useLocalImages">A boolean that indicates if local images should be used</param>
    /// <returns>A list with image urls to other product images</returns>
    public static List<string> GetOtherProductImageUrls(Int32 productId, Int32 languageId, bool useLocalImages)
    {
        // Set the language id
        languageId = useLocalImages == true ? languageId : 0;

        // Create the list to return
        List<string> otherImages = new List<string>(10);

        // Create the directory string
        string otherImagesDirectory = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/other_images/";

        try
        {
            // Get all the file names
            string[] imageUrlList = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(otherImagesDirectory));

            // Add them to the list
            for (int i = 0; i < imageUrlList.Length; i++)
            {
                otherImages.Add(otherImagesDirectory + System.IO.Path.GetFileName(imageUrlList[i]));
            }
        }
        catch (Exception ex)
        {
            string exMessage = ex.Message;
        }

        // Return the list
        return otherImages;

    } // End of the GetOtherProductImageUrls method

    /// <summary>
    /// Get a list of urls to environment images for categories
    /// </summary>
    /// <param name="categoryId">The category id</param>
    /// <param name="languageId">The language id</param>
    /// <param name="useLocalImages">A boolean that indicates if local images should be used</param>
    /// <returns>A list with image urls to environment images</returns>
    public static List<string> GetEnvironmentImageUrls(Int32 categoryId, Int32 languageId, bool useLocalImages)
    {

        // Set the language id
        languageId = useLocalImages == true ? languageId : 0;

        // Create the list to return
        List<string> environmentImages = new List<string>(10);

        // Create the directory string
        string environmentImagesDirectory = "/Content/categories/" + (categoryId / 100).ToString() + "/" + categoryId.ToString() + "/" + languageId.ToString() + "/environment_images/";

        try
        {
            // Get all the file names
            string[] imageUrlList = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(environmentImagesDirectory));

            // Add them to the list
            for (int i = 0; i < imageUrlList.Length; i++)
            {
                environmentImages.Add(environmentImagesDirectory + System.IO.Path.GetFileName(imageUrlList[i]));
            }
        }
        catch (Exception ex)
        {
            string exMessage = ex.Message;
        }

        // Return the list
        return environmentImages;

    } // End of the GetEnvironmentImageUrls method

    /// <summary>
    /// Get the directory url for spin images
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    /// <param name="useLocalImages">A boolean that indicates if local images should be used</param>
    /// <returns>The directory url for spin images</returns>
    public static string GetSpinImageDirectoryUrl(Int32 productId, Int32 languageId, bool useLocalImages)
    {
        // Set the language id
        languageId = useLocalImages == true ? languageId : 0;

        // Create string to return
        string spinImageDirectory = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/spin_images/";

        // Return the string
        return spinImageDirectory;

    } // End of the GetSpinImageDirectoryUrl method

    /// <summary>
    /// Get a string of image names separated by | in a spin image directory
    /// </summary>
    /// <param name="spinImageDirectoryUrl">The url to the directory for spin images</param>
    /// <returns>A string with image names separated by |</returns>
    public static string GetSpinImageNames(string spinImageDirectoryUrl)
    {
        // Get the server file path to the directory
        string serverFilePath = HttpContext.Current.Server.MapPath(spinImageDirectoryUrl);

        // Create the string to return
        string imageNameString = "";

        // Get all the files
        if (System.IO.Directory.Exists(serverFilePath) == true)
        {
            string[] files = Directory.GetFiles(serverFilePath);

            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    imageNameString += Path.GetFileName(files[i]);

                    if (i < (files.Length - 1))
                    {
                        imageNameString += "|";
                    }
                }
            }
        }

        // Return the string
        return imageNameString;

    } // End of the GetSpinImageNames method

    /// <summary>
    /// Get the directory url for inspiration image maps
    /// </summary>
    /// <param name="imageMapId">The id of the image map</param>
    /// <returns>The directory url for inspiration images</returns>
    public static string GetInspirationImageDirectoryUrl(Int32 imageMapId)
    {
        // Create string to return
        string inspirationImageDirectory = "/Content/inspiration/" + (imageMapId / 100).ToString() + "/" + imageMapId.ToString() + "/";

        // Return the string
        return inspirationImageDirectory;

    } // End of the GetInspirationImageDirectoryUrl method

    /// <summary>
    /// Get the url for a inspiration image map
    /// </summary>
    /// <param name="imageMapId">The id of the image map</param>
    /// <param name="imageName">The image name of the inspiration image</param>
    /// <returns>The directory url for inspiration images</returns>
    public static string GetInspirationImageUrl(Int32 imageMapId, string imageName)
    {
        // Create string to return
        string inspirationImageUrl = "/Content/inspiration/" + (imageMapId / 100).ToString() + "/" + imageMapId.ToString() + "/" + imageName;

        // Return the string
        return inspirationImageUrl;

    } // End of the GetInspirationImageUrl method

    /// <summary>
    /// Get the file path to downloadable files
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    /// <param name="useLocalFiles">A boolean that indicates if local files should be used</param>
    /// <returns>A list with urls to downloadable files</returns>
    public static string GetDownloadableFilesDirectory(Int32 productId, Int32 languageId, bool useLocalFiles)
    {
        // Set the language id
        languageId = useLocalFiles == true ? languageId : 0;

        // Create the directory string
        string filesDirectory = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/dc_files/";

        // Return the files directory
        return filesDirectory;

    } // End of the GetDownloadableFilesDirectory method

    /// <summary>
    /// Get a list of urls to downloadable files for a product and a language
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    /// <param name="useLocalFiles">A boolean that indicates if local files should be used</param>
    /// <returns>A list with urls to downloadable files</returns>
    public static List<string> GetDownloadableFiles(Int32 productId, Int32 languageId, bool useLocalFiles)
    {
        // Set the language id
        languageId = useLocalFiles == true ? languageId : 0;

        // Create the list to return
        List<string> downloadableFiles = new List<string>(10);

        // Create the directory string
        string filesDirectory = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/dc_files/";

        try
        {
            // Get all the file names
            string[] fileUrlList = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(filesDirectory));

            // Add them to the list
            for (int i = 0; i < fileUrlList.Length; i++)
            {
                downloadableFiles.Add(filesDirectory + System.IO.Path.GetFileName(fileUrlList[i]));
            }
        }
        catch (Exception ex)
        {
            string exMessage = ex.Message;
        }

        // Return the list
        return downloadableFiles;

    } // End of the GetDownloadableFiles method

    /// <summary>
    /// Get the vat code for the customer
    /// </summary>
    /// <param name="customerType">The customer type as a byte</param>
    /// <param name="invoiceCountry">A reference to the invoice country</param>
    /// <param name="deliveryCountry">A reference to the delivery country</param>
    /// <returns>The vat code as a byte</returns>
    public static byte GetVatCode(byte customerType, Country invoiceCountry, Country deliveryCountry)
    {
        // Create the byte to return
        byte vatCode = 0;

        // Make sure that countries not is null
        if(invoiceCountry == null || deliveryCountry == null)
        {
            return vatCode;
        }

        // Find the vat code (0:Domestic,1:EU, 2:Export)
        if (customerType == 0 && invoiceCountry.vat_code == 0 && deliveryCountry.vat_code == 2)
        {
            // Export
            vatCode = 2;
        }
        else if (customerType == 0 && invoiceCountry.vat_code == 1 && deliveryCountry.vat_code == 1)
        {
            // EU-trade
            vatCode = 1;
        }
        else if (customerType == 0 && invoiceCountry.vat_code == 1 && deliveryCountry.vat_code == 2)
        {
            // Export
            vatCode = 2;
        }
        else if (invoiceCountry.vat_code == 2 && deliveryCountry.vat_code == 2)
        {
            // Export
            vatCode = 2;
        }

        // Return the vat code
        return vatCode;

    } // End of the GetVatCode method

    /// <summary>
    /// Get the IP-address of the customer
    /// </summary>
    /// <returns>The IP-address as a string</returns>
    public static string GetUserIP()
    {
        string ipList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ipList) == false)
        {
            return ipList.Split(',')[0];
        }

        return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

    } // End of the GetUserIP method

    /// <summary>
    /// Protect a cookie value
    /// </summary>
    public static string ProtectCookieValue(string value, string purpose)
    {
        // Create the string to return
        string encodedString = "";

        try
        {
            // Get the byte array
            byte[] stream = Encoding.UTF8.GetBytes(value);

            // Protect the value
            byte[] encodedValue = MachineKey.Protect(stream, purpose);

            // Get encoded string
            encodedString = HttpServerUtility.UrlTokenEncode(encodedValue);
        }
        catch (Exception e)
        {
            string exMessage = e.Message;
        }
        
        // Return the encrypted value as a string
        return encodedString;

    } // End of the ProtectCookieValue method

    /// <summary>
    /// Unprotect a cookie value
    /// </summary>
    public static string UnprotectCookieValue(string value, string purpose)
    {
        // Create the string to return
        string decodedString = "";

        try
        {
            // Get the byte array
            byte[] stream = HttpServerUtility.UrlTokenDecode(value);

            // Unprotect the value
            byte[] decodedValue = MachineKey.Unprotect(stream, purpose);

            // Return the value as string
            decodedString = Encoding.UTF8.GetString(decodedValue);
        }
        catch (Exception e)
        {
            string exMessage = e.Message;
        }

        // Return the value as string
        return decodedString;

    } // End of the UnprotectCookieValue method

    /// <summary>
    /// Get a 404 not found page
    /// </summary>
    /// <returns>A string with html</returns>
    public static string GetHttpNotFoundPage()
    {
        // Create the string to return
        string htmlString = "";

        // Get the current domain
        Domain currentDomain = Tools.GetCurrentDomain();

        // Get the error page
        StaticPage staticPage = StaticPage.GetOneByConnectionId(5, currentDomain.front_end_language);
        staticPage = staticPage != null ? staticPage : new StaticPage();

        // Get the translated texts
        KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

        // Create the Route data
        System.Web.Routing.RouteData routeData = new System.Web.Routing.RouteData();
        routeData.Values.Add("controller", "home");

        // Create the controller context
        System.Web.Mvc.ControllerContext context = new System.Web.Mvc.ControllerContext(new System.Web.Routing.RequestContext(new HttpContextWrapper(HttpContext.Current), routeData), new Annytab.Webshop.Controllers.homeController());

        // Create the bread crumb list
        List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
        breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
        breadCrumbs.Add(new BreadCrumb(staticPage.link_name, "/home/error/404"));

        // Set form values
        context.Controller.ViewBag.BreadCrumbs = breadCrumbs;
        context.Controller.ViewBag.CurrentCategory = new Category();
        context.Controller.ViewBag.TranslatedTexts = tt;
        context.Controller.ViewBag.CurrentDomain = currentDomain;
        context.Controller.ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
        context.Controller.ViewBag.StaticPage = staticPage;
        context.Controller.ViewBag.PricesIncludesVat = HttpContext.Current.Session["PricesIncludesVat"] != null ? Convert.ToBoolean(HttpContext.Current.Session["PricesIncludesVat"]) : currentDomain.prices_includes_vat;

        // Render the view
        using (StringWriter stringWriter = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture))
        {
            string viewPath = currentDomain.custom_theme_id == 0 ? "/Views/home/error.cshtml" : "/Views/theme/error.cshtml";
            System.Web.Mvc.RazorView razor = new System.Web.Mvc.RazorView(context, viewPath, null, false, null);
            razor.Render(new System.Web.Mvc.ViewContext(context, razor, context.Controller.ViewData, context.Controller.TempData, stringWriter), stringWriter);
            htmlString += stringWriter.ToString();
        }

        //// Create the web page
        //notFoundString += "<html><head>";
        //notFoundString += "<meta charset=\"utf-8\">";
        //notFoundString += "<title>" + staticPage.title + "</title></head>";
        //notFoundString += "<body><div style=\"text-align:center;margin:200px auto auto auto;\"><h1>" + staticPage.title + "</h1>";
        //notFoundString += "<p>" + staticPage.main_content + "</p>";
        //notFoundString += "<a href=\"/\">" + tt.Get("start_page") + "</a></div></body>";
        //notFoundString += "</html>";

        // Return the string
        return htmlString;

    } // End of the GetHttpNotFoundPage method

    /// <summary>
    /// Get the first and last name from a name string
    /// </summary>
    /// <param name="name">The name to convert</param>
    /// <returns>A string array with a length of 2</returns>
    public static string[] GetFirstAndLastName(string name)
    {
        // Create the string array to return
        string[] names = new string[] { "", "" };

        // Split the name string by space
        string[] nameParts = name.Split(' ');

        if (nameParts.Length > 1)
        {
            for (int i = 0; i < nameParts.Length; i++)
            {
                if (i == nameParts.Length - 1)
                {
                    names[1] = nameParts[i];
                }
                else
                {
                    names[0] += " " + nameParts[i];
                }
            }
        }

        // Return the string array
        return names;

    } // End of the GetFirstAndLastName method

    /// <summary>
    /// Remove a key from cache
    /// </summary>
    public static void RemoveKeyFromCache(string key)
    {
        // Make sure that the cache reference not is null
        if (HttpContext.Current.Cache[key] != null)
        {
            // Remove the cached data by key
            HttpContext.Current.Cache.Remove(key);
        }

    } // End of the RemoveKeyFromCache method

} // End of the class