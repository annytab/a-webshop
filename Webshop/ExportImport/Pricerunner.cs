using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// This class handles the creation of a PriceRunner file
/// </summary>
public static class PriceRunner
{
    #region Write methods

    /// <summary>
    /// Create a PriceRunner file
    /// </summary>
    /// <param name="domain">A reference to the domain</param>
    public static void Create(Domain domain)
    {
        // Create the directory path
        string directoryPath = HttpContext.Current.Server.MapPath("/Content/domains/" + domain.id.ToString() + "/marketing/");

        // Check if the directory exists
        if (System.IO.Directory.Exists(directoryPath) == false)
        {
            // Create the directory
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        // Create the filepath
        string filepath = directoryPath + "PriceRunner.txt";

        // Get all data that we need
        List<Product> products = Product.GetAllActive(domain.front_end_language, "title", "ASC");
        Currency currency = Currency.GetOneById(domain.currency);
        Int32 decimalMultiplier = (Int32)Math.Pow(10, currency.decimals);
        Country country = Country.GetOneById(domain.country_id, domain.front_end_language);

        // Create a stream writer
        StreamWriter writer = null;

        try
        {
            // Create the file to write UTF-8 encoded text
            writer = File.CreateText(filepath);

            // Write the heading for the file
            writer.WriteLine("Category|Product name|SKU|Price|Shipping Cost|Product URL|Manufacturer SKU|Manufacturer|EAN or UPC|Description|Image URL|Stock Status|Delivery time|Product state|ISBN");

            // Loop all the products
            for (int i = 0; i < products.Count; i++)
            {
                // Do not include affiliate products
                if (products[i].affiliate_link != "")
                {
                    continue;
                }

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
                    foreach (ProductOption[] optionArray in productCombinations)
                    {
                        // Get a product copy
                        Product productCopy = products[i].Clone();

                        // Loop all the product options in the array
                        Int32 optionCounter = 0;
                        foreach (ProductOption option in optionArray)
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

                            // Add to the option counter
                            optionCounter++;
                        }

                        // Write the product item to the file
                        WriteProductItem(writer, domain, country, productCopy, currency, decimalMultiplier);
                    }
                }
                else
                {
                    // Write the product item to the file
                    WriteProductItem(writer, domain, country, products[i], currency, decimalMultiplier);
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            // Close the stream writer if it is different from null
            if (writer != null)
            {
                writer.Close();
            }
        }

    } // End of the Create method

    /// <summary>
    /// Write one product item to the file
    /// </summary>
    /// <param name="writer">A reference to an stream writer</param>
    /// <param name="domain">A reference to a domain</param>
    /// <param name="country">A referende to a country</param>
    /// <param name="product">A reference to a product</param>
    /// <param name="currency">A reference to a currency</param>
    /// <param name="decimalMultiplier">The decimal multiplier</param>
    private static void WriteProductItem(StreamWriter writer, Domain domain, Country country, Product product, Currency currency,
        Int32 decimalMultiplier)
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

            if (i < parentCategoryChain.Count - 1)
            {
                categoryString += " > ";
            }
        }

        // Remove html from the title and the main content
        string title = StringHtmlExtensions.StripHtml(product.title);
        title = title.Replace("|", "");
        string main_content = Regex.Replace(product.main_content, @"(<br\s*[\/]>)+", " ");
        main_content = StringHtmlExtensions.StripHtml(main_content);
        main_content = Regex.Replace(main_content, @"\r\n?|\n", "");
        main_content = main_content.Replace("|", "");
        main_content = AnnytabDataValidation.TruncateString(main_content, 5000);
        
        // Calculate the price
        decimal basePrice = product.unit_price * (currency.currency_base / currency.conversion_rate);
        decimal regularPrice = Math.Round(basePrice * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        decimal salePrice = Math.Round(basePrice * (1 - product.discount) * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Get the country code as upper case letters
        string countryCodeUpperCase = country.country_code.ToUpper();

        // Add value added tax to the price
        if (countryCodeUpperCase != "US" && countryCodeUpperCase != "CA" && countryCodeUpperCase != "IN")
        {
            regularPrice += Math.Round(regularPrice * valueAddedTax.value * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
            salePrice += Math.Round(salePrice * valueAddedTax.value * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        }

        // Calculate the freight
        decimal freight = (product.unit_freight + product.toll_freight_addition) * (currency.currency_base / currency.conversion_rate);
        freight = Math.Round(freight * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Add value added tax to the freight
        if (countryCodeUpperCase != "US" && countryCodeUpperCase != "CA" && countryCodeUpperCase != "IN")
        {
            freight += Math.Round(freight * valueAddedTax.value * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        }

        // Create the line to write to
        // Category|Product name|SKU|Price|Shipping Cost|Product URL|Manufacturer SKU|Manufacturer|EAN or UPC|Description|Image URL|Stock Status|Delivery time|Product state|ISBN
        string line = "";

        // Category
        line += categoryString + "|";

        // Product name
        line += title + "|";

        // SKU
        line += product.product_code + "|";

        // Price
        line += salePrice.ToString(CultureInfo.InvariantCulture) + "|";

        // Shipping Cost
        line += freight.ToString(CultureInfo.InvariantCulture) + "|";

        // Product URL
        line += domain.web_address + "/home/product/" + product.page_name + "|";

        // Manufacturer SKU
        line += product.manufacturer_code + "|";

        // Manufacturer
        line += product.brand + "|";

        // EAN or UPC
        line += product.gtin + "|";

        // Description
        line += main_content + "|";

        // Image URL
        line += domain.web_address + Tools.GetProductMainImageUrl(product.id, domain.front_end_language, product.variant_image_filename, product.use_local_images) + "|";

        // Stock Status
        line += GetAvailabilityStatus(product.availability_status) + "|";

        // Delivery time
        line += product.delivery_time + "|";

        // Product state
        line += product.condition != "" ? product.condition + "|" : "new|";

        // ISBN
        line += product.gtin;

        // Write the line to the file
        writer.WriteLine(line);

    } // End of the WriteProductItem method

    #endregion

    #region Helper methods

    /// <summary>
    /// Get the availability status 
    /// </summary>
    /// <param name="availabilityCode">The availability code string</param>
    /// <returns>The availability status as string</returns>
    public static string GetAvailabilityStatus(string availabilityCode)
    {
        // Create the string to return
        string availability = "Out of Stock";

        // Get the availability status
        if (availabilityCode == "availability_in_stock")
        {
            availability = "In Stock";
        }
        else if (availabilityCode == "availability_out_of_stock")
        {
            availability = "Out of Stock";
        }
        else if (availabilityCode == "availability_to_order")
        {
            availability = "Out of Stock";
        }
        else if (availabilityCode == "availability_preorder")
        {
            availability = "Preorder";
        }

        // Return the availability
        return availability;

    } // End of the GetAvailabilityStatus method

    #endregion

} // End of the class