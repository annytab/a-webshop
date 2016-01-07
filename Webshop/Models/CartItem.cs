using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// This class represent a cart item
/// </summary>
[Serializable]
public class CartItem
{
    #region Variables

    public string product_code;
    public string manufacturer_code;
    public Int32 product_id;
    public string product_name;
    public decimal vat_percent;
    public decimal quantity;
    public decimal unit_price;
    public decimal unit_freight;
    public string variant_image_url;
    public bool use_local_images;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new cart item with default properties
    /// </summary>
    public CartItem()
    {
        // Set values for instance variables
        this.product_code = "";
        this.product_id = 0;
        this.manufacturer_code = "";
        this.product_name = "";
        this.vat_percent = 0;
        this.quantity = 0;
        this.unit_price = 0;
        this.unit_freight = 0;
        this.variant_image_url = "";
        this.use_local_images = false;

    } // End of the constructor

    #endregion

    #region Shopping cart methods

    /// <summary>
    /// Add a cart item to the shopping cart
    /// </summary>
    /// <param name="cartItem">A reference to a cart item</param>
    public static void AddToShoppingCart(CartItem cartItem)
    {
        // Get the shopping cart
        Dictionary<string, CartItem> shoppingCart = (Dictionary<string, CartItem>)HttpContext.Current.Session["ShoppingCart"];

        // Check if the session is null
        if (shoppingCart != null)
        {
            // Check if the product code exists in the shopping cart
            if (shoppingCart.ContainsKey(cartItem.product_code) == true)
            {
                // Get the cart item
                CartItem itemInCart = shoppingCart[cartItem.product_code];

                // Adjust the quantity
                itemInCart.quantity += cartItem.quantity;
            }
            else
            {
                // Add the cart item to the shopping cart
                shoppingCart.Add(cartItem.product_code, cartItem);
            }
        }
        else
        {
            // Create a new dictionary
            shoppingCart = new Dictionary<string, CartItem>(10);

            // Add the cart item to the shopping cart
            shoppingCart.Add(cartItem.product_code, cartItem);
        }

        // Update the session post
        HttpContext.Current.Session["ShoppingCart"] = shoppingCart;

    } // End of the AddCartItem method

    /// <summary>
    /// Update the quantity for a cart item in the shopping cart
    /// </summary>
    /// <param name="productCode"></param>
    /// <param name="quantity"></param>
    public static void UpdateCartQuantity(string productCode, decimal quantity)
    {
        // Get the shopping cart
        Dictionary<string, CartItem> shoppingCart = (Dictionary<string, CartItem>)HttpContext.Current.Session["ShoppingCart"];

        // Clear the shopping cart if a discount code is applied
        if (HttpContext.Current.Session["DiscountCodeId"] != null)
        {
            HttpContext.Current.Session.Remove("DiscountCodeId");
            HttpContext.Current.Session.Remove("ShoppingCart");
            HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
            return;
        }

        // Make sure that the session not is null
        if (shoppingCart != null)
        {
            // Check if the shopping cart contains the key
            if (shoppingCart.ContainsKey(productCode) == true)
            {
                // Get the cart item
                CartItem itemInCart = shoppingCart[productCode];

                // Adjust the quantity
                itemInCart.quantity += quantity;

                if(itemInCart.quantity <= 0)
                {
                    shoppingCart.Remove(productCode);
                }
            }

            // Update the session post
            HttpContext.Current.Session["ShoppingCart"] = shoppingCart;
        }

    } // End of the UpdateCartQuantity method

    /// <summary>
    /// Delete a cart item in the shopping cart
    /// </summary>
    /// <param name="productCode">The product code</param>
    public static void DeleteCartItem(string productCode)
    {
        // Get the shopping cart
        Dictionary<string, CartItem> shoppingCart = (Dictionary<string, CartItem>)HttpContext.Current.Session["ShoppingCart"];

        // Clear the shopping cart if a discount code is applied
        if(HttpContext.Current.Session["DiscountCodeId"] != null)
        {
            HttpContext.Current.Session.Remove("DiscountCodeId");
            HttpContext.Current.Session.Remove("ShoppingCart");
            HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
            return;
        }

        // Make sure that the session not is null
        if (shoppingCart != null)
        {
            // Check if the shopping cart contains the key
            if (shoppingCart.ContainsKey(productCode) == true)
            {
                shoppingCart.Remove(productCode);
            }

            // Update the session post
            HttpContext.Current.Session["ShoppingCart"] = shoppingCart;
        }

    } // End of the DeleteCartItem method

    /// <summary>
    /// Clear the shopping cart
    /// </summary>
    public static void ClearShoppingCart()
    {
        // Delete the shopping cart session post
        HttpContext.Current.Session.Remove("ShoppingCart");
        HttpContext.Current.Session.Remove("DiscountCodeId");
        HttpContext.Current.Session.Remove("GiftCards");

    } // End of the ClearShoppingCart method

    /// <summary>
    /// Set the discount code and modify the shopping cart
    /// </summary>
    /// <param name="discountCodeId">The discount code id as a string</param>
    public static void SetDiscountCode(string discountCodeId)
    {
        // Get the current domain
        Domain domain = Tools.GetCurrentDomain();

        // Get the discount code
        DiscountCode discountCode = DiscountCode.GetOneById(discountCodeId);
        
        // Get the currency
        Currency currency = Currency.GetOneById(domain.currency);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, currency.decimals);

        // Check if there is errors with the discount code
        if(discountCode == null)
        {
            // The discount code does not exist
            HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
            return;
        }
        else if (DateTime.UtcNow.AddDays(-1) > discountCode.end_date)
        {
            // The discount code is not valid anymore
            HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
            return;
        }
        else if(discountCode.language_id != domain.front_end_language)
        {
            // The discount code is not valid for the language
            HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
            return;
        }
        else if(discountCode.currency_code != currency.currency_code)
        {
            // The discount code is not valid for the currency
            HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
            return;
        }
        else
        {
            // Get the signed-in customer
            Customer customer = Customer.GetSignedInCustomer();

            // Check if the discount code already is used by the customer
            if(discountCode.once_per_customer == true && customer == null)
            {
                // The discount code does not exist
                HttpContext.Current.Session["CodeError"] = "customer_not_signed_in";
                return;
            }
            else if (discountCode.once_per_customer == true && Order.GetOneByDiscountCodeAndCustomerId(discountCodeId, customer.id) != null)
            {
                // The discount code is already used
                HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
                return;
            }

            // Get the shopping cart
            Dictionary<string, CartItem> shoppingCart = (Dictionary<string, CartItem>)HttpContext.Current.Session["ShoppingCart"];

            // Make sure that the shopping cart not is null
            if(shoppingCart == null)
            {
                // Shopping cart is empty
                HttpContext.Current.Session["CodeError"] = "empty_shopping_cart";
                return;
            }

            // Calculate the order sum
            decimal net_sum = 0;
            foreach (KeyValuePair<string, CartItem> post in shoppingCart)
            {
                // Get the cart item
                CartItem itemInCart = post.Value;

                // Add to the net sum
                net_sum += itemInCart.quantity * itemInCart.unit_price;
            }

            // Check the order minimum
            if(net_sum < discountCode.minimum_order_value)
            {
                HttpContext.Current.Session["CodeError"] = "invalid_discount_code";
                return;
            }

            // Loop the shopping cart
            foreach (KeyValuePair<string, CartItem> post in shoppingCart)
            {
                // Get the cart item
                CartItem itemInCart = post.Value;

                // Check if we should remove the freight
                if (discountCode.free_freight == true)
                {
                    // Remove the freight
                    itemInCart.unit_freight = 0;
                }

                // Get the product
                Product product = Product.GetOneById(itemInCart.product_id, domain.front_end_language);

                // Apply the discount
                if (discountCode.exclude_products_on_sale == false || (discountCode.exclude_products_on_sale == true && product.discount <= 0M))
                {
                    itemInCart.unit_price = Math.Round(itemInCart.unit_price * (1 - discountCode.discount_value) * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
                }
            }

            // Set the discount code
            HttpContext.Current.Session["DiscountCodeId"] = discountCodeId;
            HttpContext.Current.Session.Remove("CodeError");
        }

    } // End of the SetDiscountCode method

    #endregion

    #region Get methods

    /// <summary>
    /// Get a list with cart items
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <returns>A list with cart items</returns>
    public static List<CartItem> GetCartItems(Int32 languageId)
    {
        // Create the list to return
        List<CartItem> cartItems = new List<CartItem>(10);

        // Get the shopping cart
        Dictionary<string, CartItem> shoppingCart = (Dictionary<string, CartItem>)HttpContext.Current.Session["ShoppingCart"];
        
        // Make sure that the shopping cart not is null
        if (shoppingCart != null)
        {
            // Loop the shopping cart
            foreach(KeyValuePair<string, CartItem> post in shoppingCart)
            {
                // Add the cart item to the list
                cartItems.Add(post.Value);
            }
        }

        // Return the list
        return cartItems;

    } // End of the GetCartItems method

    /// <summary>
    /// Get cart statistics like quantity and amount
    /// </summary>
    /// <param name="domain">A reference to the current domain </param>
    /// <returns>A dictionary with cart values</returns>
    public static Dictionary<string, decimal> GetCartStatistics(Domain domain, bool pricesIncludesVat)
    {
        // Create the dictionary to return
        Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>(2);

        // Get the shopping cart
        Dictionary<string, CartItem> shoppingCart = (Dictionary<string, CartItem>)HttpContext.Current.Session["ShoppingCart"];

        // Get the currency
        Currency currency = Currency.GetOneById(domain.currency);

        // Calculate the decimal multiplier
        Int32 decimalMultiplier = (Int32)Math.Pow(10, currency.decimals);

        // Quantity and sum for the shopping cart
        decimal total_quantity = 0;
        decimal total_amount = 0;

        // Make sure that the session not is null
        if (shoppingCart != null)
        {
            // Loop the keys in cookie
            foreach(KeyValuePair<string, CartItem> post in shoppingCart)
            {
                // Get the cart item
                CartItem item = post.Value;
  
                // Create variables
                decimal quantity = item.quantity;
                decimal price = item.unit_price;

                // Check if the price should include vat
                if(pricesIncludesVat == true)
                {
                    price += price * item.vat_percent;
                }

                // Add to the total quantity
                total_quantity += item.quantity;

                // Add to the total sum (price * quantity)
                total_amount += Math.Round(price * quantity * 100, MidpointRounding.AwayFromZero) / decimalMultiplier;
            }
        }

        // Round the total amount
        total_amount = Math.Round(total_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Add values to the dictionary
        dictionary.Add("total_quantity", total_quantity);
        dictionary.Add("total_amount", total_amount);

        // Return the dictionary
        return dictionary;

    } // End of the GetCartStatistics method

    /// <summary>
    /// Get cart amounts (net_amount, freight_amount, vat_amount, rounding_amount, total_amount, total_mount_time)
    /// </summary>
    /// <param name="cartItems">A list of cart items</param>
    /// <param name="languageId">A language id</param>
    /// <param name="vatCode">The current vat code</param>
    /// <param name="decimalMultiplier">A decimal multiplier</param>
    /// <returns>A dicitionary with cart amounts</returns>
    public static Dictionary<string, decimal> GetCartAmounts(List<CartItem> cartItems, Int32 languageId, byte vatCode, Int32 decimalMultiplier)
    {
        // Create the dictionary
        Dictionary<string, decimal> cartAmounts = new Dictionary<string, decimal>(4);

        // Create variables
        decimal net_amount = 0;
        decimal freight_amount = 0;
        decimal vat_amount = 0;
        decimal rounding_amount = 0;
        decimal total_amount = 0;
        decimal total_mount_time = 0;

        // Loop the cart items
        for (int i = 0; i < cartItems.Count; i++)
        {
            // Get the product
            Product product = Product.GetOneById(cartItems[i].product_id, languageId);

            // Round the price and freight
            decimal price = Math.Round(cartItems[i].unit_price * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
            decimal freight = Math.Round(cartItems[i].unit_freight * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

            // Combine the price and the freight
            decimal priceFreight = (price + freight) * cartItems[i].quantity;
            priceFreight = Math.Round(priceFreight * 100) / 100;

            // Calculate the price value
            decimal priceValue = cartItems[i].quantity * price;
            priceValue = Math.Round(priceValue * 100) / 100;

            // Calculate the freight value
            decimal freightValue = cartItems[i].quantity * freight;
            freightValue = Math.Round(freightValue * 100) / 100;

            // Calculate the vat
            decimal vatValue = priceFreight * cartItems[i].vat_percent;

            // Add to sums
            net_amount += priceValue;
            freight_amount += freightValue;
            vat_amount += vatValue;
            total_mount_time += product != null ? product.mount_time_hours * cartItems[i].quantity : 0;
        }

        // Round sums
        net_amount = Math.Round(net_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        freight_amount = Math.Round(freight_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        vat_amount = Math.Round(vat_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Set vat as zero if the sale not is domestic
        if (vatCode != 0)
        {
            vat_amount = 0;
        }

        // Calculate the total amount without rounding
        decimal total_not_rounded = net_amount + freight_amount + vat_amount;

        // Round the total amount
        total_amount = Math.Round(total_not_rounded, MidpointRounding.AwayFromZero);

        // Calculate the rounding
        rounding_amount = Math.Round((total_amount - total_not_rounded) * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Add values to the dictionary
        cartAmounts.Add("net_amount", AnnytabDataValidation.TruncateDecimal(net_amount, 0, 999999999999.99M));
        cartAmounts.Add("freight_amount", AnnytabDataValidation.TruncateDecimal(freight_amount, 0, 999999999999.99M));
        cartAmounts.Add("vat_amount", AnnytabDataValidation.TruncateDecimal(vat_amount, 0, 999999999999.99M));
        cartAmounts.Add("rounding_amount", AnnytabDataValidation.TruncateDecimal(rounding_amount, -99.999M, 999.999M));
        cartAmounts.Add("total_amount", AnnytabDataValidation.TruncateDecimal(total_amount, 0, 999999999999.99M));
        cartAmounts.Add("total_mount_time", AnnytabDataValidation.TruncateDecimal(total_mount_time, 0, 999999.99M));

        // Return the dictionary
        return cartAmounts;

    } // End of the GetCartAmounts method

    /// <summary>
    /// Get a list of order rows
    /// </summary>
    /// <param name="cartItems">A list of cart items</param>
    /// <param name="vatCode">The current vat code</param>
    /// <param name="languageId">A language id</param>
    /// <param name="decimalMultiplier">A decimal multiplier</param>
    /// <returns>A list with order rows</returns>
    public static List<OrderRow> GetOrderRows(List<CartItem> cartItems, byte vatCode, Int32 languageId, Int32 decimalMultiplier)
    {
        // Create order rows
        List<OrderRow> orderRows = new List<OrderRow>(10);

        // Loop all the cart items
        Int16 rowCounter = 0;
        for(int i = 0; i < cartItems.Count; i++)
        {
            // Get data
            Product product = Product.GetOneById(cartItems[i].product_id, languageId);

            // Calculate the freight price
            decimal price = Math.Round(cartItems[i].unit_price * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
            decimal freight = Math.Round(cartItems[i].unit_freight * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
            decimal priceFreight = price + freight;

            // Create a new order row and add values to it
            OrderRow orderRow = new OrderRow();
            orderRow.product_code = AnnytabDataValidation.TruncateString(cartItems[i].product_code, 50);
            orderRow.manufacturer_code = AnnytabDataValidation.TruncateString(cartItems[i].manufacturer_code, 50);
            orderRow.product_id = product.id;
            orderRow.gtin = product.gtin;
            orderRow.product_name = AnnytabDataValidation.TruncateString(cartItems[i].product_name, 100);
            orderRow.vat_percent = vatCode != 0 ? 0 : cartItems[i].vat_percent;
            orderRow.quantity = AnnytabDataValidation.TruncateDecimal(cartItems[i].quantity, 0, 999999.99M);
            orderRow.unit_id = product.unit_id;
            orderRow.unit_price = AnnytabDataValidation.TruncateDecimal(priceFreight, 0, 9999999999.99M);
            orderRow.account_code = product.account_code;
            orderRow.supplier_erp_id = product.supplier_erp_id;
            orderRow.sort_order = rowCounter;

            // Add to the row counter
            rowCounter += 1;

            // Update product buys
            Product.UpdateBuys(product.id, AnnytabDataValidation.TruncateDecimal(cartItems[i].quantity + product.buys, 0, 9999999999.99M));

            // Add the row to the list
            orderRows.Add(orderRow);

            // Get bundle items
            List<ProductBundle> bundleItems = ProductBundle.GetByBundleProductId(product.id);

            // Add all the bundle items
            for(int j = 0; j < bundleItems.Count; j++)
            {
                // Get the product
                product = Product.GetOneById(bundleItems[j].product_id, languageId);

                // Get the value added tax
                ValueAddedTax valueAddedTax = ValueAddedTax.GetOneById(product.value_added_tax_id);

                // Create a new order row and add values to it
                orderRow = new OrderRow();
                orderRow.product_code = product.product_code;
                orderRow.manufacturer_code = product.manufacturer_code;
                orderRow.product_id = product.id;
                orderRow.gtin = product.gtin;
                orderRow.product_name = product.title;
                orderRow.vat_percent = vatCode != 0 ? 0 : valueAddedTax.value;
                orderRow.quantity = bundleItems[j].quantity;
                orderRow.unit_id = product.unit_id;
                orderRow.unit_price = 0;
                orderRow.account_code = product.account_code;
                orderRow.supplier_erp_id = product.supplier_erp_id;
                orderRow.sort_order = rowCounter;

                // Add to the row counter
                rowCounter += 1;

                // Update product buys
                Product.UpdateBuys(product.id, AnnytabDataValidation.TruncateDecimal(bundleItems[j].quantity + product.buys, 0, 9999999999.99M));

                // Add the row to the list
                orderRows.Add(orderRow);
            }
        }

        // Return the list
        return orderRows;

    } // End of the GetOrderRows method

    #endregion

} // End of the class
