using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;

/// <summary>
/// This class handles the creation of a google shopping file
/// </summary>
public static class GoogleShopping
{

    #region Methods

    /// <summary>
    /// Create a google shopping file
    /// </summary>
    /// <param name="domain">A reference to the domain</param>
    /// <param name="title">The title for the channel</param>
    /// <param name="description">The description for the channel</param>
    public static void Create(Domain domain, string title, string description)
    {

        // Create the directory path
        string directoryPath = HttpContext.Current.Server.MapPath("/Content/domains/" + domain.id.ToString() + "/marketing/");

        // Check if the directory exists
        if (System.IO.Directory.Exists(directoryPath) == false)
        {
            // Create the directory
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        // Create the file
        string filepath = directoryPath + "GoogleShopping.xml.gz";

        // Get all data that we need
        List<Product> products = Product.GetAllActive(domain.front_end_language, "title", "ASC");
        Currency currency = Currency.GetOneById(domain.currency);
        Int32 decimalMultiplier = (Int32)Math.Pow(10, currency.decimals);
        KeyStringList tt = StaticText.GetAll(domain.front_end_language, "id", "ASC");
        Country country = Country.GetOneById(domain.country_id, domain.front_end_language);

        // Create variables
        GZipStream gzipStream = null;
        XmlTextWriter xmlTextWriter = null;

        try
        {
            // Create a gzip stream
            gzipStream = new GZipStream(new FileStream(filepath, FileMode.Create), CompressionMode.Compress);

            // Create a xml text writer
            xmlTextWriter = new XmlTextWriter(gzipStream, new UTF8Encoding(true));

            // Set the base url
            string baseUrl = domain.web_address;

            // Write the start of the document
            xmlTextWriter.WriteStartDocument();

            // Write the rss tag
            xmlTextWriter.WriteStartElement("rss");
            xmlTextWriter.WriteAttributeString("version", "2.0");
            xmlTextWriter.WriteAttributeString("xmlns:g", "http://base.google.com/ns/1.0");

            // Write the channel tag
            xmlTextWriter.WriteStartElement("channel");

            // Write information about the channel
            xmlTextWriter.WriteStartElement("title");
            xmlTextWriter.WriteString(title);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteStartElement("link");
            xmlTextWriter.WriteString(baseUrl);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteStartElement("description");
            xmlTextWriter.WriteString(description);
            xmlTextWriter.WriteEndElement();

            // Loop all the products
            for (int i = 0; i < products.Count; i++)
            {
                // Get all the product options
                List<ProductOptionType> productOptionTypes = ProductOptionType.GetByProductId(products[i].id, domain.front_end_language);
                
                // Check if the product has product options or not
                if (productOptionTypes.Count > 0)
                {
                    // Get all the product options
                    Dictionary<Int32, List<ProductOption>> productOptions = new Dictionary<Int32, List<ProductOption>>(productOptionTypes.Count);

                    // Loop all the product option types

                    for (int j = 0; j < productOptionTypes.Count; j++)
                    {
                        List<ProductOption> listProductOptions = ProductOption.GetByProductOptionTypeId(productOptionTypes[j].id, domain.front_end_language);
                        productOptions.Add(j, ProductOption.GetByProductOptionTypeId(productOptionTypes[j].id, domain.front_end_language));
                    }

                    // Get all the product combinations
                    List<ProductOption[]> productCombinations = new List<ProductOption[]>();
                    ProductOption.GetProductCombinations(productCombinations, productOptions, 0, new ProductOption[productOptions.Count]);

                    // Loop all the product combinations
                    foreach(ProductOption[] optionArray in productCombinations)
                    {
                        // Get a product copy
                        Product productCopy = products[i].Clone();

                        // Create an array with google variants
                        List<string[]> googleVariants = new List<string[]>();

                        // Loop all the product options in the array
                        Int32 optionCounter = 0;
                        foreach(ProductOption option in optionArray)
                        {
                            // Adjust product values
                            productCopy.product_code += option.product_code_suffix;
                            productCopy.manufacturer_code += option.mpn_suffix;
                            productCopy.title += " - " + option.title;
                            productCopy.unit_price += option.price_addition;
                            productCopy.unit_freight += option.freight_addition;
                            productCopy.variant_image_filename = productCopy.variant_image_filename.Replace("[" + optionCounter.ToString() + "]", option.product_code_suffix);

                            // Get the product option type
                            ProductOptionType productOptionType = ProductOptionType.GetOneById(option.product_option_type_id, domain.front_end_language);

                            // Add the google variant
                            if(productOptionType.google_name != "")
                            {
                                googleVariants.Add(new string[] { productOptionType.google_name, option.title });
                            }

                            // Add to the option counter
                            optionCounter++;
                        }

                        // Write the product item to the xml file
                        WriteProductItem(xmlTextWriter, domain, country, tt, productCopy, currency, decimalMultiplier, googleVariants);
                    }
                }
                else
                {
                    // Write the product item to the xml file
                    WriteProductItem(xmlTextWriter, domain, country, tt, products[i], currency, decimalMultiplier, new List<string[]>(0));
                }
            }

            // Write the end of the document (close rss and channel)
            xmlTextWriter.WriteEndDocument();

        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            // Close streams
            if (xmlTextWriter != null)
            {
                // Close the XmlTextWriter
                xmlTextWriter.Close();
            }
            if (gzipStream != null)
            {
                // Close the gzip stream
                gzipStream.Close();
            }
        }

    } // End of the Create method

    /// <summary>
    /// Write one product item to the file
    /// </summary>
    /// <param name="writer">A reference to an xml text writer</param>
    /// <param name="domain">A reference to a domain</param>
    /// <param name="country">A referende to a country</param>
    /// <param name="tt">A reference to a list of translated texts</param>
    /// <param name="product">A reference to a product</param>
    /// <param name="currency">A reference to a currency</param>
    /// <param name="decimalMultiplier">The decimal multiplier</param>
    /// <param name="googleVariants">A list with google variants</param>
    private static void WriteProductItem(XmlTextWriter writer, Domain domain, Country country, KeyStringList tt,  Product product, Currency currency, 
        Int32 decimalMultiplier, List<string[]> googleVariants)
    {
        
        // Get the value added tax
        ValueAddedTax valueAddedTax = ValueAddedTax.GetOneById(product.value_added_tax_id);

        // Get the unit
        Unit unit = Unit.GetOneById(product.unit_id, domain.front_end_language);

        // Get the category
        Category category = Category.GetOneById(product.category_id, domain.front_end_language);

        // Get a chain of parent categories
        List<Category> parentCategoryChain = Category.GetParentCategoryChain(category, domain.front_end_language);

        // Create the category string
        string categoryString = "";
        for (int i = 0; i < parentCategoryChain.Count; i++)
        {
            categoryString += parentCategoryChain[i].title;

            if(i < parentCategoryChain.Count - 1)
            {
                categoryString += " > ";
            }
        }

        // Write the start item tag
        writer.WriteStartElement("item");

        // Remove html from the title and the main content
        string title = StringHtmlExtensions.TruncateHtml(product.title, 150);
        string main_content = StringHtmlExtensions.TruncateHtml(product.main_content, 5000);

        // Write item base information
        writer.WriteStartElement("g:id");
        writer.WriteString(product.product_code);
        writer.WriteEndElement();
        writer.WriteStartElement("title");
        writer.WriteString(title);
        writer.WriteEndElement();
        writer.WriteStartElement("description");
        writer.WriteString(main_content);
        writer.WriteEndElement();
        writer.WriteStartElement("g:google_product_category");
        writer.WriteString(HttpUtility.HtmlEncode(product.google_category));
        writer.WriteEndElement();
        writer.WriteStartElement("g:product_type");
        writer.WriteString(HttpUtility.HtmlEncode(categoryString));
        writer.WriteEndElement();
        writer.WriteStartElement("link");
        writer.WriteString(domain.web_address + "/home/product/" + product.page_name);
        writer.WriteEndElement();
        writer.WriteStartElement("g:image_link");
        writer.WriteString(domain.web_address + Tools.GetProductMainImageUrl(product.id, domain.front_end_language, product.variant_image_filename, product.use_local_images));
        writer.WriteEndElement();
        writer.WriteStartElement("g:condition");
        writer.WriteString(product.condition);
        writer.WriteEndElement();

        // Calculate the price
        decimal price = product.unit_price * (currency.currency_base / currency.conversion_rate);
        price = Math.Round(price * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Add value added tax to the price
        if(country.country_code != "US" && country.country_code != "CA" && country.country_code != "IN")
        {
            price += Math.Round(price * valueAddedTax.value * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        }

        // Calculate the freight
        decimal freight = (product.unit_freight + product.toll_freight_addition) * (currency.currency_base / currency.conversion_rate);
        freight = Math.Round(freight * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Add value added tax to the freight
        if (country.country_code != "US" && country.country_code != "CA" && country.country_code != "IN")
        {
            freight += Math.Round(freight * valueAddedTax.value * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        }
        
        // Product availability and price
        writer.WriteStartElement("g:availability");
        writer.WriteString(GetGoogleAvailabilityStatus(product.availability_status));
        writer.WriteEndElement();
        writer.WriteStartElement("g:price");
        writer.WriteString(price.ToString(CultureInfo.InvariantCulture) + " " + currency.currency_code);
        writer.WriteEndElement();

        // Set the availability date
        if(product.availability_status == "availability_to_order")
        {
            writer.WriteStartElement("g:availability_date");
            writer.WriteString(product.availability_date.ToString());
            writer.WriteEndElement();   
        }

        // Unique product codes
        writer.WriteStartElement("g:brand");
        writer.WriteString(product.brand);
        writer.WriteEndElement();
        writer.WriteStartElement("g:gtin");
        writer.WriteString(product.gtin);
        writer.WriteEndElement();
        writer.WriteStartElement("g:mpn");
        writer.WriteString(product.manufacturer_code);
        writer.WriteEndElement();

        // An indentifier does not exist if brand and mpn or gtin is missing
        if((product.brand != "" && product.manufacturer_code != "") || product.gtin != "")
        {
            writer.WriteStartElement("g:identifier_exists");
            writer.WriteString("TRUE");
            writer.WriteEndElement();
        }
        else
        {
            writer.WriteStartElement("g:identifier_exists");
            writer.WriteString("FALSE");
            writer.WriteEndElement();
        }

        // Freight
        writer.WriteStartElement("g:shipping");
        writer.WriteStartElement("g:country");
        writer.WriteString(country.country_code);
        writer.WriteEndElement();
        writer.WriteStartElement("g:price");
        writer.WriteString(freight.ToString(CultureInfo.InvariantCulture) + " " + currency.currency_code);
        writer.WriteEndElement();
        writer.WriteEndElement();

        // Is bundle product
        if(ProductBundle.GetByBundleProductId(product.id).Count > 0)
        {
            writer.WriteStartElement("g:is_bundle");
            writer.WriteString("TRUE");
            writer.WriteEndElement();
        }

        // Gender
        if(product.gender != "")
        {
            writer.WriteStartElement("g:gender");
            writer.WriteString(product.gender);
            writer.WriteEndElement();
        }

        // Age group
        if (product.age_group != "")
        {
            writer.WriteStartElement("g:age_group");
            writer.WriteString(product.age_group);
            writer.WriteEndElement();
        }

        // Adult only
        if(product.adult_only == true)
        {
            writer.WriteStartElement("g:adult");
            writer.WriteString("TRUE");
            writer.WriteEndElement();
        }
        else
        {
            writer.WriteStartElement("g:adult");
            writer.WriteString("FALSE");
            writer.WriteEndElement();
        }

        // Unit pricing measure
        if(product.unit_pricing_measure > 0 && product.unit_pricing_base_measure > 0)
        {
            writer.WriteStartElement("g:unit_pricing_measure");
            writer.WriteString(product.unit_pricing_measure.ToString() + unit.unit_code_si);
            writer.WriteEndElement();
            writer.WriteStartElement("g:unit_pricing_base_measure");
            writer.WriteString(product.unit_pricing_base_measure.ToString() + unit.unit_code_si);
            writer.WriteEndElement();  
        }

        // Size type
        if (product.size_type != "")
        {
            writer.WriteStartElement("g:size_type");
            writer.WriteString(GetGoogleSizeType(product.size_type));
            writer.WriteEndElement();
        }

        // Size system
        if (product.size_system != "")
        {
            writer.WriteStartElement("g:size_system");
            writer.WriteString(product.size_system);
            writer.WriteEndElement();
        }

        // Energy efficiency class
        if (product.energy_efficiency_class != "")
        {
            writer.WriteStartElement("g:energy_efficiency_class");
            writer.WriteString(product.energy_efficiency_class);
            writer.WriteEndElement();
        }

        // Add the item group id
        if(googleVariants.Count > 0)
        {
            writer.WriteStartElement("g:item_group_id");
            writer.WriteString(product.id.ToString());
            writer.WriteEndElement();
        }

        // Add google variants
        for (int i = 0; i < googleVariants.Count; i++)
        {
            // Get the value pair
            string[] valuePair = googleVariants[i];

            writer.WriteStartElement(valuePair[0]);
            writer.WriteString(valuePair[1]);
            writer.WriteEndElement();
        }

        // Write the end of the item tag
        writer.WriteEndElement();

    } // End of the CreateProductPost method

    #endregion

    #region Helper methods

    /// <summary>
    /// Get the google availability status 
    /// </summary>
    /// <param name="availabilityCode">The availability code string</param>
    /// <returns>The gooogle availability status as string</returns>
    public static string GetGoogleAvailabilityStatus(string availabilityCode)
    {
        // Create the string to return
        string availability = "";

        // Get the availability status
        if(availabilityCode == "availability_in_stock")
        {
            availability = "in stock";
        }
        else if(availabilityCode == "availability_out_of_stock")
        {
            availability = "out of stock";
        }
        else if (availabilityCode == "availability_to_order")
        {
            availability = "in stock";
        }
        else if (availabilityCode == "availability_preorder")
        {
            availability = "preorder";
        }

        // Return the availability
        return availability;

    } // End of the GetGoogleAvailabilityStatus method

    /// <summary>
    /// Get the google size type
    /// </summary>
    /// <param name="sizeTypeCode">The size type code string</param>
    /// <returns>The gooogle availability status as string</returns>
    public static string GetGoogleSizeType(string sizeTypeCode)
    {
        // Create the string to return
        string sizeType = "";

        // Get the size type
        if (sizeTypeCode == "regular")
        {
            sizeType = "Regular";
        }
        else if (sizeTypeCode == "petite")
        {
            sizeType = "Petite";
        }
        else if (sizeTypeCode == "plus")
        {
            sizeType = "Plus";
        }
        else if (sizeTypeCode == "big_and_tall")
        {
            sizeType = "Big and Tall";
        }
        else if (sizeTypeCode == "maternity")
        {
            sizeType = "Maternity";
        }

        // Return the size type
        return sizeType;

    } // End of the GetGoogleSizeType method

    #endregion

} // End of the class