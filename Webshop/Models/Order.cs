using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent an order
/// </summary>
public class Order
{
    #region Variables

    public Int32 id;
    public byte document_type;
    public DateTime order_date;
    public Int32 company_id;
    public string country_code;
    public string language_code;
    public string currency_code;
    public decimal conversion_rate;
    public Int32 customer_id;
    public byte customer_type;
    public string customer_org_number;
    public string customer_vat_number;
    public string customer_name;
    public string customer_phone;
    public string customer_mobile_phone;
    public string customer_email;
    public string invoice_name;
    public string invoice_address_1;
    public string invoice_address_2;
    public string invoice_post_code;
    public string invoice_city;
    public Int32 invoice_country_id;
    public string delivery_name;
    public string delivery_address_1;
    public string delivery_address_2;
    public string delivery_post_code;
    public string delivery_city;
    public Int32 delivery_country_id;
    public decimal net_sum;
    public decimal vat_sum;
    public decimal rounding_sum;
    public decimal total_sum;
    public byte vat_code;
    public Int32 payment_option;
    public string payment_token;
    public string payment_status;
    public bool exported_to_erp;
    public string order_status;
    public DateTime desired_date_of_delivery;
    public string discount_code;
    public decimal gift_cards_amount;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new order with default properties
    /// </summary>
    public Order()
    {
        // Set values for instance variables
        this.id = 0;
        this.document_type = 0;
        this.order_date = new DateTime(2000, 1, 1);
        this.company_id = 0;
        this.country_code = "";
        this.language_code = "";
        this.currency_code = "";
        this.conversion_rate = 0;
        this.customer_id = 0;
        this.customer_type = 0;
        this.customer_org_number = "";
        this.customer_vat_number = "";
        this.customer_name = "";
        this.customer_phone = "";
        this.customer_mobile_phone = "";
        this.customer_email = "";
        this.invoice_name = "";
        this.invoice_address_1 = "";
        this.invoice_address_2 = "";
        this.invoice_post_code = "";
        this.invoice_city = "";
        this.invoice_country_id = 0;
        this.delivery_name = "";
        this.delivery_address_1 = "";
        this.delivery_address_2 = "";
        this.delivery_post_code = "";
        this.delivery_city = "";
        this.delivery_country_id = 0;
        this.net_sum = 0;
        this.vat_sum = 0;
        this.rounding_sum = 0;
        this.total_sum = 0;
        this.vat_code = 0;
        this.payment_option = 0;
        this.payment_token = "";
        this.payment_status = "";
        this.exported_to_erp = false;
        this.order_status = "";
        this.desired_date_of_delivery = DateTime.UtcNow;
        this.discount_code = "";
        this.gift_cards_amount = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new order from a reader
    /// </summary>
    /// <param name="reader">A reference to a order</param>
    public Order(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.document_type = Convert.ToByte(reader["document_type"]);
        this.order_date = Convert.ToDateTime(reader["order_date"]);
        this.company_id = Convert.ToInt32(reader["company_id"]);
        this.country_code = reader["country_code"].ToString();
        this.language_code = reader["language_code"].ToString();
        this.currency_code = reader["currency_code"].ToString();
        this.conversion_rate = Convert.ToDecimal(reader["conversion_rate"]);
        this.customer_id = Convert.ToInt32(reader["customer_id"]);
        this.customer_type = Convert.ToByte(reader["customer_type"]);
        this.customer_org_number = reader["customer_org_number"].ToString();
        this.customer_vat_number = reader["customer_vat_number"].ToString();
        this.customer_name = reader["customer_name"].ToString();
        this.customer_phone = reader["customer_phone"].ToString();
        this.customer_mobile_phone = reader["customer_mobile_phone"].ToString();
        this.customer_email = reader["customer_email"].ToString();
        this.invoice_name = reader["invoice_name"].ToString();
        this.invoice_address_1 = reader["invoice_address_1"].ToString();
        this.invoice_address_2 = reader["invoice_address_2"].ToString();
        this.invoice_post_code = reader["invoice_post_code"].ToString();
        this.invoice_city = reader["invoice_city"].ToString();
        this.invoice_country_id = Convert.ToInt32(reader["invoice_country_id"]);
        this.delivery_name = reader["delivery_name"].ToString();
        this.delivery_address_1 = reader["delivery_address_1"].ToString();
        this.delivery_address_2 = reader["delivery_address_2"].ToString();
        this.delivery_post_code = reader["delivery_post_code"].ToString();
        this.delivery_city = reader["delivery_city"].ToString();
        this.delivery_country_id = Convert.ToInt32(reader["delivery_country_id"]);
        this.net_sum = Convert.ToDecimal(reader["net_sum"]);
        this.vat_sum = Convert.ToDecimal(reader["vat_sum"]);
        this.rounding_sum = Convert.ToDecimal(reader["rounding_sum"]);
        this.total_sum = Convert.ToDecimal(reader["total_sum"]);
        this.vat_code = Convert.ToByte(reader["vat_code"]);
        this.payment_option = Convert.ToInt32(reader["payment_option"]);
        this.payment_token = reader["payment_token"].ToString();
        this.payment_status = reader["payment_status"].ToString();
        this.exported_to_erp = Convert.ToBoolean(reader["exported_to_erp"]);
        this.order_status = reader["order_status"].ToString();
        this.desired_date_of_delivery = Convert.ToDateTime(reader["desired_date_of_delivery"]);
        this.discount_code = reader["discount_code"].ToString();
        this.gift_cards_amount = Convert.ToDecimal(reader["gift_cards_amount"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one order
    /// </summary>
    /// <param name="post">A reference to a order post</param>
    public static long Add(Order post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.orders (document_type, order_date, company_id, country_code, language_code, currency_code, conversion_rate, customer_id, "
            + "customer_type, customer_org_number, customer_vat_number, customer_name, customer_phone, customer_mobile_phone, customer_email, invoice_name, "
            + "invoice_address_1, invoice_address_2, invoice_post_code, invoice_city, invoice_country_id, "
            + "delivery_name, delivery_address_1, delivery_address_2, delivery_post_code, delivery_city, "
            + "delivery_country_id, net_sum, vat_sum, rounding_sum, total_sum, vat_code, payment_option, payment_token, payment_status, exported_to_erp, "
            + "order_status, desired_date_of_delivery, discount_code, gift_cards_amount) "
            + "VALUES (@document_type, @order_date, @company_id, @country_code, @language_code, @currency_code, @conversion_rate, @customer_id, @customer_type, "
            + "@customer_org_number, @customer_vat_number, @customer_name, @customer_phone, @customer_mobile_phone, @customer_email, @invoice_name, @invoice_address_1, "
            + "@invoice_address_2, @invoice_post_code, @invoice_city, @invoice_country_id, @delivery_name, @delivery_address_1, "
            + "@delivery_address_2, @delivery_post_code, @delivery_city, @delivery_country_id, @net_sum, @vat_sum, @rounding_sum, @total_sum, @vat_code, "
            + "@payment_option, @payment_token, @payment_status, @exported_to_erp, @order_status, @desired_date_of_delivery, @discount_code, @gift_cards_amount);" 
            + "SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", post.document_type);
                cmd.Parameters.AddWithValue("@order_date", post.order_date);
                cmd.Parameters.AddWithValue("@company_id", post.company_id);
                cmd.Parameters.AddWithValue("@country_code", post.country_code);
                cmd.Parameters.AddWithValue("@language_code", post.language_code);
                cmd.Parameters.AddWithValue("@currency_code", post.currency_code);
                cmd.Parameters.AddWithValue("@conversion_rate", post.conversion_rate);
                cmd.Parameters.AddWithValue("@customer_id", post.customer_id);
                cmd.Parameters.AddWithValue("@customer_type", post.customer_type);
                cmd.Parameters.AddWithValue("@customer_org_number", post.customer_org_number);
                cmd.Parameters.AddWithValue("@customer_vat_number", post.customer_vat_number);
                cmd.Parameters.AddWithValue("@customer_name", post.customer_name);
                cmd.Parameters.AddWithValue("@customer_phone", post.customer_phone);
                cmd.Parameters.AddWithValue("@customer_mobile_phone", post.customer_mobile_phone);
                cmd.Parameters.AddWithValue("@customer_email", post.customer_email);
                cmd.Parameters.AddWithValue("@invoice_name", post.invoice_name);
                cmd.Parameters.AddWithValue("@invoice_address_1", post.invoice_address_1);
                cmd.Parameters.AddWithValue("@invoice_address_2", post.invoice_address_2);
                cmd.Parameters.AddWithValue("@invoice_post_code", post.invoice_post_code);
                cmd.Parameters.AddWithValue("@invoice_city", post.invoice_city);
                cmd.Parameters.AddWithValue("@invoice_country_id", post.invoice_country_id);
                cmd.Parameters.AddWithValue("@delivery_name", post.delivery_name);
                cmd.Parameters.AddWithValue("@delivery_address_1", post.delivery_address_1);
                cmd.Parameters.AddWithValue("@delivery_address_2", post.delivery_address_2);
                cmd.Parameters.AddWithValue("@delivery_post_code", post.delivery_post_code);
                cmd.Parameters.AddWithValue("@delivery_city", post.delivery_city);
                cmd.Parameters.AddWithValue("@delivery_country_id", post.delivery_country_id);
                cmd.Parameters.AddWithValue("@net_sum", post.net_sum);
                cmd.Parameters.AddWithValue("@vat_sum", post.vat_sum);
                cmd.Parameters.AddWithValue("@rounding_sum", post.rounding_sum);
                cmd.Parameters.AddWithValue("@total_sum", post.total_sum);
                cmd.Parameters.AddWithValue("@vat_code", post.vat_code);
                cmd.Parameters.AddWithValue("@payment_option", post.payment_option);
                cmd.Parameters.AddWithValue("@payment_token", post.payment_token);
                cmd.Parameters.AddWithValue("@payment_status", post.payment_status);
                cmd.Parameters.AddWithValue("@exported_to_erp", post.exported_to_erp);
                cmd.Parameters.AddWithValue("@order_status", post.order_status);
                cmd.Parameters.AddWithValue("@desired_date_of_delivery", post.desired_date_of_delivery);
                cmd.Parameters.AddWithValue("@discount_code", post.discount_code);
                cmd.Parameters.AddWithValue("@gift_cards_amount", post.gift_cards_amount);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the insert
                    idOfInsert = Convert.ToInt64(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the id of the inserted item
        return idOfInsert;

    } // End of the Add method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a order post
    /// </summary>
    /// <param name="post">A reference to a order post</param>
    public static void Update(Order post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.orders SET document_type = @document_type, order_date = @order_date, company_id = @company_id, country_code = @country_code, "
            + "language_code = @language_code, currency_code = @currency_code, conversion_rate = @conversion_rate, customer_id = @customer_id,  customer_type = @customer_type, "
            + "customer_org_number = @customer_org_number, customer_vat_number = @customer_vat_number, customer_name = @customer_name, customer_phone = @customer_phone, "
            + "customer_mobile_phone = @customer_mobile_phone, customer_email = @customer_email, invoice_name = @invoice_name, "
            + "invoice_address_1 = @invoice_address_1, invoice_address_2 = @invoice_address_2, invoice_post_code = @invoice_post_code, "
            + "invoice_city = @invoice_city, invoice_country_id = @invoice_country_id, delivery_name = @delivery_name, "
            + "delivery_address_1 = @delivery_address_1, delivery_address_2 = @delivery_address_2, delivery_post_code = @delivery_post_code, "
            + "delivery_city = @delivery_city, delivery_country_id = @delivery_country_id, net_sum = @net_sum, vat_sum = @vat_sum, "
            + "rounding_sum = @rounding_sum, total_sum = @total_sum, vat_code = @vat_code, payment_option = @payment_option, "
            + "payment_token = @payment_token, payment_status = @payment_status, exported_to_erp = @exported_to_erp, order_status = @order_status, "
            + "desired_date_of_delivery = @desired_date_of_delivery, discount_code = @discount_code, gift_cards_amount = @gift_cards_amount WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@document_type", post.document_type);
                cmd.Parameters.AddWithValue("@order_date", post.order_date);
                cmd.Parameters.AddWithValue("@company_id", post.company_id);
                cmd.Parameters.AddWithValue("@country_code", post.country_code);
                cmd.Parameters.AddWithValue("@language_code", post.language_code);
                cmd.Parameters.AddWithValue("@currency_code", post.currency_code);
                cmd.Parameters.AddWithValue("@conversion_rate", post.conversion_rate);
                cmd.Parameters.AddWithValue("@customer_id", post.customer_id);
                cmd.Parameters.AddWithValue("@customer_type", post.customer_type);
                cmd.Parameters.AddWithValue("@customer_org_number", post.customer_org_number);
                cmd.Parameters.AddWithValue("@customer_vat_number", post.customer_vat_number);
                cmd.Parameters.AddWithValue("@customer_name", post.customer_name);
                cmd.Parameters.AddWithValue("@customer_phone", post.customer_phone);
                cmd.Parameters.AddWithValue("@customer_mobile_phone", post.customer_mobile_phone);
                cmd.Parameters.AddWithValue("@customer_email", post.customer_email);
                cmd.Parameters.AddWithValue("@invoice_name", post.invoice_name);
                cmd.Parameters.AddWithValue("@invoice_address_1", post.invoice_address_1);
                cmd.Parameters.AddWithValue("@invoice_address_2", post.invoice_address_2);
                cmd.Parameters.AddWithValue("@invoice_post_code", post.invoice_post_code);
                cmd.Parameters.AddWithValue("@invoice_city", post.invoice_city);
                cmd.Parameters.AddWithValue("@invoice_country_id", post.invoice_country_id);
                cmd.Parameters.AddWithValue("@delivery_name", post.delivery_name);
                cmd.Parameters.AddWithValue("@delivery_address_1", post.delivery_address_1);
                cmd.Parameters.AddWithValue("@delivery_address_2", post.delivery_address_2);
                cmd.Parameters.AddWithValue("@delivery_post_code", post.delivery_post_code);
                cmd.Parameters.AddWithValue("@delivery_city", post.delivery_city);
                cmd.Parameters.AddWithValue("@delivery_country_id", post.delivery_country_id);
                cmd.Parameters.AddWithValue("@net_sum", post.net_sum);
                cmd.Parameters.AddWithValue("@vat_sum", post.vat_sum);
                cmd.Parameters.AddWithValue("@rounding_sum", post.rounding_sum);
                cmd.Parameters.AddWithValue("@total_sum", post.total_sum);
                cmd.Parameters.AddWithValue("@vat_code", post.vat_code);
                cmd.Parameters.AddWithValue("@payment_option", post.payment_option);
                cmd.Parameters.AddWithValue("@payment_token", post.payment_token);
                cmd.Parameters.AddWithValue("@payment_status", post.payment_status);
                cmd.Parameters.AddWithValue("@exported_to_erp", post.exported_to_erp);
                cmd.Parameters.AddWithValue("@order_status", post.order_status);
                cmd.Parameters.AddWithValue("@desired_date_of_delivery", post.desired_date_of_delivery);
                cmd.Parameters.AddWithValue("@discount_code", post.discount_code);
                cmd.Parameters.AddWithValue("@gift_cards_amount", post.gift_cards_amount);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the Update method

    /// <summary>
    /// Update the payment status for the order
    /// </summary>
    /// <param name="orderId">The order id</param>
    /// <param name="statusCode">The status code</param>
    public static void UpdatePaymentStatus(Int32 orderId, string statusCode)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.orders SET payment_status = @payment_status WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.Parameters.AddWithValue("@payment_status", statusCode);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the UpdatePaymentStatus method

    /// <summary>
    /// Update the order status
    /// </summary>
    /// <param name="orderId">The order id</param>
    /// <param name="statusCode">The status code</param>
    public static void UpdateOrderStatus(Int32 orderId, string statusCode)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.orders SET order_status = @order_status WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.Parameters.AddWithValue("@order_status", statusCode);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the UpdateOrderStatus method

    /// <summary>
    /// Set the order as exported to the erp program
    /// </summary>
    /// <param name="orderId"></param>
    public static void SetAsExportedToErp(Int32 orderId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.orders SET exported_to_erp = @exported_to_erp WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.Parameters.AddWithValue("@exported_to_erp", true);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the SetAsExportedToErp method

    /// <summary>
    /// Set the payment token for the order
    /// </summary>
    /// <param name="orderId">The order id</param>
    /// <param name="paymentToken">The payment token for the order</param>
    public static void SetPaymentToken(Int32 orderId, string paymentToken)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.orders SET payment_token = @payment_token WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.Parameters.AddWithValue("@payment_token", paymentToken);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the SetPaymentToken method

    /// <summary>
    /// Update the gift cards amount for the order
    /// </summary>
    /// <param name="orderId">The order id</param>
    /// <param name="giftCardsAmount">The total sum of all gift cards that has been used for the order</param>
    public static void UpdateGiftCardsAmount(Int32 orderId, decimal giftCardsAmount)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.orders SET gift_cards_amount = @gift_cards_amount WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.Parameters.AddWithValue("@gift_cards_amount", giftCardsAmount);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the UpdateGiftCardsAmount method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of orders by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <returns>The number of orders as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.orders WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR customer_name LIKE @keyword_" + i.ToString()
                + " OR invoice_name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters for search keywords
                for (int i = 0; i < keywords.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@keyword_" + i.ToString(), "%" + keywords[i].ToString() + "%");
                }

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountBySearch method

    /// <summary>
    /// Count the number of orders by customer id
    /// </summary>
    /// <param name="customerId">The customer id</param>
    /// <returns>The number of orders as an int</returns>
    public static Int32 GetCountByCustomerId(Int32 customerId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.orders WHERE customer_id = @customer_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@customer_id", customerId);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountByCustomerId method

    /// <summary>
    /// Count the number of orders in the year
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of orders as an int</returns>
    public static Int32 GetCountByYear(string countryCode, string year)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if(countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }
            
        // Add the final touch to the sql string
        sql += "AND YEAR(order_date) = @year;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountByYear method

    /// <summary>
    /// Count the number of orders in the month
    /// </summary>
    /// <returns>The number of orders as an int</returns>
    public static Int32 GetCountByMonth(string countryCode, string year, string month)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }
        
        // Add the final touch to the sql string
        sql += "AND YEAR(order_date) = @year AND MONTH(order_date) = @month;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountByMonth method

    /// <summary>
    /// Count the number of orders in the week
    /// </summary>
    /// <returns>The number of orders as an int</returns>
    public static Int32 GetCountByWeek(string countryCode, string year, string week)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the final touch to the sql string
        sql += "AND YEAR(order_date) = @year AND DATEPART(week, order_date) = @week;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@week", week);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountByWeek method

    /// <summary>
    /// Count the number of orders in a day
    /// </summary>
    /// <returns>The number of orders as an int</returns>
    public static Int32 GetCountByDay(string countryCode, string year, string day)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the final touch to the sql string
        sql += "AND YEAR(order_date) = @year AND DATEPART(dayofyear, order_date) = @day;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@day", day);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountByDay method

    /// <summary>
    /// Count the number of orders by discount code
    /// </summary>
    /// <param name="discountCodeId">The id of the discount code</param>
    /// <returns>The number of orders as an int</returns>
    public static Int32 GetCountByDiscountCode(string discountCodeId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.orders WHERE discount_code = @discount_code;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@discount_code", discountCodeId);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountByDiscountCode method

    #endregion

    #region Get methods

    /// <summary>
    /// Check if a master post exists
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A boolean that indicates if the post exists</returns>
    public static bool MasterPostExists(Int32 id)
    {
        // Create the boolean to return
        bool postExists = false;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.orders WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@id", id);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Check if the post exist
                    postExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0 ? true : false;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the boolean
        return postExists;

    } // End of the MasterPostExists method

    /// <summary>
    /// Get one order based on id
    /// </summary>
    /// <param name="orderId">A order id</param>
    /// <returns>A reference to a order post</returns>
    public static Order GetOneById(Int32 orderId)
    {
        // Create the post to return
        Order post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", orderId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Order(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneById method

    /// <summary>
    /// Get one order based on discount code and customer id
    /// </summary>
    /// <param name="discountCodeId">The id of a discount code</param>
    /// <param name="customerId">The id of a customer</param>
    /// <returns>A reference to a order post</returns>
    public static Order GetOneByDiscountCodeAndCustomerId(string discountCodeId, Int32 customerId)
    {
        // Create the post to return
        Order post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE discount_code = @discount_code AND customer_id = @customer_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@discount_code", discountCodeId);
                cmd.Parameters.AddWithValue("@customer_id", customerId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Order(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneByDiscountCodeAndCustomerId method

    /// <summary>
    /// Get all the orders
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetAll(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(10);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetAll method

    /// <summary>
    /// Get all the orders that not has been exported to an ERP system
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetNotExportedToErp(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(10);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE exported_to_erp = @exported_to_erp AND order_status "
            + "!= @order_status ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@exported_to_erp", false);
                cmd.Parameters.AddWithValue("@order_status", "order_status_cancelled");

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetNotExportedToErp method

    /// <summary>
    /// Get all the orders that not has been exported to an ERP system
    /// </summary>
    /// <param name="companyId">The company id</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetNotExportedToErp(Int32 companyId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(10);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE company_id = @company_id AND exported_to_erp = @exported_to_erp "
            + "AND order_status != @order_status ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@company_id", companyId);
                cmd.Parameters.AddWithValue("@exported_to_erp", false);
                cmd.Parameters.AddWithValue("@order_status", "order_status_cancelled");

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetNotExportedToErp method

    /// <summary>
    /// Get orders that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The sort field</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetBySearch(string[] keywords, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR customer_name LIKE @keyword_" + i.ToString()
                + " OR invoice_name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += " ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Add parameters for search keywords
                for (int i = 0; i < keywords.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@keyword_" + i.ToString(), "%" + keywords[i].ToString() + "%");
                }

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetBySearch method

    /// <summary>
    /// Get orders by customer id
    /// </summary>
    /// <param name="customerId">The customer id</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetByCustomerId(Int32 customerId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE customer_id = @customer_id ORDER BY " + sortField + " " 
            + sortOrder + " " + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@customer_id", customerId);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetByCustomerId method

    /// <summary>
    /// Get orders by discount code
    /// </summary>
    /// <param name="discountCodeId">The id of the discount code</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetByDiscountCode(string discountCodeId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE discount_code = @discount_code ORDER BY " + sortField + " "
            + sortOrder + " " + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@discount_code", discountCodeId);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetByDiscountCode method

    /// <summary>
    /// Get all orders for a year
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetByYear(string countryCode, string year, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        sql += "AND YEAR(order_date) = @year ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetByYear method

    /// <summary>
    /// Get all orders for a month
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="month">The month as a string (1 to 12)</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetByMonth(string countryCode, string year, string month, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }
        
        sql += "AND YEAR(order_date) = @year AND MONTH(order_date) = @month ORDER BY " + sortField + " " + sortOrder + " "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetByMonth method

    /// <summary>
    /// Get all orders for a week
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="week">The week as a string</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetByWeek(string countryCode, string year, string week, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        sql += "AND YEAR(order_date) = @year AND DATEPART(week, order_date) = @week ORDER BY " + sortField + " " + sortOrder + " "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@week", week);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetByWeek method

    /// <summary>
    /// Get all orders for a day
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="day">The day as a string</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The sort field</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of orders</returns>
    public static List<Order> GetByDay(string countryCode, string year, string day, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Order> posts = new List<Order>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders WHERE document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        sql += "AND YEAR(order_date) = @year AND DATEPART(dayofyear, order_date) = @day ORDER BY " + sortField + " " + sortOrder + " "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@day", day);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Order(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetByDay method

    /// <summary>
    /// Get the amounts of the order: net_amount, vat_amount and total_amount
    /// </summary>
    /// <param name="orderRows">A reference to a list of order rows</param>
    /// <param name="vatCode">A code that indicates if vat should be added or not</param>
    /// <param name="decimalMultiplier">The decimal multiplier</param>
    /// <returns>A dictionary with the amounts of the order</returns>
    public static Dictionary<string, decimal> GetOrderAmounts(List<OrderRow> orderRows, byte vatCode, Int32 decimalMultiplier)
    {
        // Create the dictionary
        Dictionary<string, decimal> orderAmounts = new Dictionary<string, decimal>(3);

        // Create calculate variables
        decimal net_amount = 0;
        decimal vat_amount = 0;
        decimal rounding_amount = 0;
        decimal total_amount = 0;

        // Loop the order rows
        for (int i = 0; i < orderRows.Count; i++ )
        {
            // Calculate values
            decimal priceValue = Math.Round(orderRows[i].quantity * orderRows[i].unit_price * 100, MidpointRounding.AwayFromZero) / 100;
            decimal vatSumValue = priceValue * orderRows[i].vat_percent;

            // Add to sums
            net_amount += priceValue;
            vat_amount += vatSumValue;
        }

        // Round sums
        net_amount = Math.Round(net_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        vat_amount = Math.Round(vat_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Calculate the total sum
        if(vatCode != 0)
        {
            vat_amount = 0;
        }

        // Calculate the total amount without rounding
        decimal total_not_rounded = net_amount + vat_amount;

        // Round the total amount
        total_amount = Math.Round(total_not_rounded, MidpointRounding.AwayFromZero);

        // Calculate the rounding
        rounding_amount = Math.Round((total_amount - total_not_rounded) * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;

        // Add values to the dictionary
        orderAmounts.Add("net_amount", AnnytabDataValidation.TruncateDecimal(net_amount, 0, 999999999999.99M));
        orderAmounts.Add("vat_amount", AnnytabDataValidation.TruncateDecimal(vat_amount, 0, 999999999999.99M));
        orderAmounts.Add("rounding_amount", AnnytabDataValidation.TruncateDecimal(rounding_amount, -99.999M, 999.999M));
        orderAmounts.Add("total_amount", AnnytabDataValidation.TruncateDecimal(total_amount, 0, 999999999999.99M));

        // Return the dictionary
        return orderAmounts;

    } // End of the GetOrderAmounts method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a order post on id
    /// </summary>
    /// <param name="id">The id number for the order</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.orders WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    // Check for a foreign key constraint error
                    if (e.Number == 547)
                    {
                        return 5;
                    }
                    else
                    {
                        throw e;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the code for success
        return 0;

    } // End of the DeleteOnId method

    #endregion

    #region Validation

    /// <summary>
    /// Get a valid sort field
    /// </summary>
    /// <param name="sortField">The sort field</param>
    /// <returns>A valid sort field as a string</returns>
    public static string GetValidSortField(string sortField)
    {
        // Make sure that the sort field is valid
        if (sortField != "id" && sortField != "invoice_name" && sortField != "order_date" && sortField != "total_sum")
        {
            sortField = "id";
        }

        // Return the string
        return sortField;

    } // End of the GetValidSortField method

    /// <summary>
    /// Get a valid sort order
    /// </summary>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A valid sort order as a string</returns>
    public static string GetValidSortOrder(string sortOrder)
    {
        // Make sure that the sort order is valid
        if (sortOrder != "ASC" && sortOrder != "DESC")
        {
            sortOrder = "ASC";
        }

        // Return the string
        return sortOrder;

    } // End of the GetValidSortOrder method

    #endregion

} // End of the class