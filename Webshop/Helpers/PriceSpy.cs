using System;
using System.Globalization;
using System.Collections.Generic;
using System.Web;
using System.IO;

/// <summary>
/// This class handles the creation of a PriceSpy file
/// </summary>
public static class PriceSpy
{
    #region Write methods

    /// <summary>
    /// Create a PriceSpy file
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
        string filepath = directoryPath + "PriceSpy.txt";

        // Get all data that we need
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
            writer.WriteLine("product-code|product-name|manufacturer|product-url|category|price-excl-vat|shipping-excl-vat|stock-status|manufacturer-code|ean|image-url|condition");

            // Get products
            Int32 page = 1;
            List<Product> products = Product.GetActiveReliable(domain.front_end_language, 50, page, "title", "ASC");

            while(products.Count > 0)
            {
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

                // Get more products
                page = page + 1;
                products = Product.GetActiveReliable(domain.front_end_language, 50, page, "title", "ASC");
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

        // Remove html from the title
        string title = StringHtmlExtensions.StripHtml(product.title);
        title = title.Replace("|", " ");

        // Calculate the price
        decimal basePrice = product.unit_price * (currency.currency_base / currency.conversion_rate);
        decimal regularPrice = Math.Round(basePrice * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        decimal salePrice = Math.Round(basePrice * (1 - product.discount) * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Calculate the freight
        decimal freight = (product.unit_freight + product.toll_freight_addition) * (currency.currency_base / currency.conversion_rate);
        freight = Math.Round(freight * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Create the line to write to
        // product-code|product-name|manufacturer|product-url|category|price-excl-vat|shipping-excl-vat|stock-status|manufacturer-code|ean|image-url|condition
        string line = "";

        // Product code
        line += product.product_code + "|";

        // Product name
        line += title + "|";

        // Manufacturer
        line += product.brand + "|";

        // Product url
        line += domain.web_address + "/home/product/" + product.page_name + "|";

        // Category
        line += categoryString + "|";

        // Price excluding vat
        line += salePrice.ToString(CultureInfo.InvariantCulture) + "|";

        // Shipping excluding vat
        line += freight.ToString(CultureInfo.InvariantCulture) + "|";

        // Stock status
        line += GetAvailabilityStatus(product.availability_status) + "|";

        // Manufacturer code
        line += product.manufacturer_code + "|";

        // EAN
        line += product.gtin + "|";

        // Image url
        line += domain.web_address + Tools.GetProductMainImageUrl(product.id, domain.front_end_language, product.variant_image_filename, product.use_local_images) + "|";

        // Condition
        line += product.condition != "" ? product.condition : "new";

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
        string availability = "out of stock";

        // Get the availability status
        if (availabilityCode == "availability_in_stock")
        {
            availability = "in stock";
        }
        else if (availabilityCode == "availability_out_of_stock")
        {
            availability = "out of stock";
        }
        else if (availabilityCode == "availability_to_order")
        {
            availability = "out of stock";
        }
        else if (availabilityCode == "availability_preorder")
        {
            availability = "preorder";
        }

        // Return the availability
        return availability;

    } // End of the GetAvailabilityStatus method

    #endregion

} // End of the class