using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// This class represent a order row
/// </summary>
public class OrderRow
{
    #region Variables

    public Int32 order_id;
    public string product_code;
    public string manufacturer_code;
    public Int32 product_id;
    public string gtin;
    public string product_name;
    public decimal vat_percent;
    public decimal quantity;
    public Int32 unit_id;
    public decimal unit_price;
    public string account_code;
    public string supplier_erp_id;
    public Int16 sort_order;
    
    #endregion

    #region Constructors

    /// <summary>
    /// Create a new order row with default properties
    /// </summary>
    public OrderRow()
    {
        // Set values for instance variables
        this.order_id = 0;
        this.product_code = "";
        this.manufacturer_code = "";
        this.product_id = 0;
        this.gtin = "";
        this.product_name = "";
        this.vat_percent = 0;
        this.quantity = 0;
        this.unit_id = 0;
        this.unit_price = 0;
        this.account_code = "";
        this.supplier_erp_id = "";
        this.sort_order = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new order row from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public OrderRow(SqlDataReader reader)
    {
        // Set values for instance variables
        this.order_id = Convert.ToInt32(reader["order_id"]);
        this.product_code = reader["product_code"].ToString();
        this.manufacturer_code = reader["manufacturer_code"].ToString();
        this.product_id = Convert.ToInt32(reader["product_id"]);
        this.gtin = reader["gtin"].ToString();
        this.product_name = reader["product_name"].ToString();
        this.vat_percent = Convert.ToDecimal(reader["vat_percent"]);
        this.quantity = Convert.ToDecimal(reader["quantity"]);
        this.unit_id = Convert.ToInt32(reader["unit_id"]);
        this.unit_price = Convert.ToDecimal(reader["unit_price"]);
        this.account_code = reader["account_code"].ToString();
        this.supplier_erp_id = reader["supplier_erp_id"].ToString();
        this.sort_order = Convert.ToInt16(reader["sort_order"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one order row
    /// </summary>
    /// <param name="post">A reference to a order row post</param>
    public static void Add(OrderRow post)
    {

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.order_rows (order_id, product_code, manufacturer_code, product_id, gtin, "
            + "product_name, vat_percent, quantity, unit_id, unit_price, account_code, supplier_erp_id, sort_order) "
            + "VALUES (@order_id, @product_code, @manufacturer_code, @product_id, @gtin, @product_name, "
            + "@vat_percent, @quantity, @unit_id, @unit_price, @account_code, @supplier_erp_id, @sort_order);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", post.order_id);
                cmd.Parameters.AddWithValue("@product_code", post.product_code);
                cmd.Parameters.AddWithValue("@manufacturer_code", post.manufacturer_code);
                cmd.Parameters.AddWithValue("@product_id", post.product_id);
                cmd.Parameters.AddWithValue("@gtin", post.gtin);
                cmd.Parameters.AddWithValue("@product_name", post.product_name);
                cmd.Parameters.AddWithValue("@vat_percent", post.vat_percent);
                cmd.Parameters.AddWithValue("@quantity", post.quantity);
                cmd.Parameters.AddWithValue("@unit_id", post.unit_id);
                cmd.Parameters.AddWithValue("@unit_price", post.unit_price);
                cmd.Parameters.AddWithValue("@account_code", post.account_code);
                cmd.Parameters.AddWithValue("@supplier_erp_id", post.supplier_erp_id);
                cmd.Parameters.AddWithValue("@sort_order", post.sort_order);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the insert
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the Add method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a order row post
    /// </summary>
    /// <param name="post">A reference to a order row post</param>
    public static void Update(OrderRow post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.order_rows SET manufacturer_code = @manufacturer_code, product_id = @product_id, "
            + "gtin = @gtin, product_name = @product_name, vat_percent = @vat_percent, quantity = @quantity, "
            + "unit_id = @unit_id, unit_price = @unit_price, account_code = @account_code, "
            + "supplier_erp_id = @supplier_erp_id, sort_order = @sort_order WHERE order_id = @order_id AND product_code = @product_code;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", post.order_id);
                cmd.Parameters.AddWithValue("@product_code", post.product_code);
                cmd.Parameters.AddWithValue("@manufacturer_code", post.manufacturer_code);
                cmd.Parameters.AddWithValue("@product_id", post.product_id);
                cmd.Parameters.AddWithValue("@gtin", post.gtin);
                cmd.Parameters.AddWithValue("@product_name", post.product_name);
                cmd.Parameters.AddWithValue("@vat_percent", post.vat_percent);
                cmd.Parameters.AddWithValue("@quantity", post.quantity);
                cmd.Parameters.AddWithValue("@unit_id", post.unit_id);
                cmd.Parameters.AddWithValue("@unit_price", post.unit_price);
                cmd.Parameters.AddWithValue("@account_code", post.account_code);
                cmd.Parameters.AddWithValue("@supplier_erp_id", post.supplier_erp_id);
                cmd.Parameters.AddWithValue("@sort_order", post.sort_order);

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

    #endregion

    #region Get methods

    /// <summary>
    /// Get one order row based on id
    /// </summary>
    /// <param name="orderId">A order row id</param>
    /// <param name="productCode">A product code</param>
    /// <returns>A reference to a order row post</returns>
    public static OrderRow GetOneById(Int32 orderId, string productCode)
    {
        // Create the post to return
        OrderRow post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.order_rows WHERE order_id = @order_id AND " 
            + "product_code = @product_code;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@product_code", productCode);

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
                        post = new OrderRow(reader);
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
    /// Get all order rows
    /// </summary>
    /// <returns>A list of order row posts</returns>
    public static List<OrderRow> GetAll()
    {
        // Create the list to return
        List<OrderRow> posts = new List<OrderRow>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.order_rows ORDER BY order_id ASC, sort_order ASC;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(new OrderRow(reader));
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

    } // End of the GetByOrderId method

    /// <summary>
    /// Get order rows for a specific order id
    /// </summary>
    /// <param name="orderId">The id of the order</param>
    /// <returns>A list of order row posts</returns>
    public static List<OrderRow> GetByOrderId(Int32 orderId)
    {
        // Create the list to return
        List<OrderRow> posts = new List<OrderRow>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.order_rows WHERE order_id = @order_id ORDER BY sort_order ASC;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", orderId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(new OrderRow(reader));
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

    } // End of the GetByOrderId method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a order row post on id
    /// </summary>
    /// <param name="orderId">The id number for the order</param>
    /// <param name="productCode">A product code</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 orderId, string productCode)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.order_rows WHERE order_id = @order_id AND product_code = @product_code;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@product_code", productCode);

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

} // End of the class