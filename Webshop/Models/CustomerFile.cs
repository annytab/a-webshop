using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a file that can be downloadable by a customer
/// </summary>
public class CustomerFile
{
    #region Variables

    public Int32 customer_id;
    public Int32 product_id;
    public Int32 language_id;
    public DateTime order_date;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new customer file with default properties
    /// </summary>
    public CustomerFile()
    {
        // Set values for instance variables
        this.customer_id = 0;
        this.product_id = 0;
        this.language_id = 0;
        this.order_date = new DateTime(2000, 1, 1);

    } // End of the constructor

    /// <summary>
    /// Create a new customer file from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public CustomerFile(SqlDataReader reader)
    {
        // Set values for instance variables
        this.customer_id = Convert.ToInt32(reader["customer_id"]);
        this.product_id = Convert.ToInt32(reader["product_id"]);
        this.language_id = Convert.ToInt32(reader["language_id"]);
        this.order_date = Convert.ToDateTime(reader["order_date"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one customer file post
    /// </summary>
    /// <param name="post">A reference to a customer file post</param>
    public static void Add(CustomerFile post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.customers_files (customer_id, product_id, language_id, order_date) "
            + "VALUES (@customer_id, @product_id, @language_id, @order_date);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@customer_id", post.customer_id);
                cmd.Parameters.AddWithValue("@product_id", post.product_id);
                cmd.Parameters.AddWithValue("@language_id", post.language_id);
                cmd.Parameters.AddWithValue("@order_date", post.order_date);

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

    /// <summary>
    /// Add customer files 
    /// </summary>
    /// <param name="order">A reference to the order</param>
    public static void AddCustomerFiles(Order order)
    {
        // Get the current domain
        Domain domain = Tools.GetCurrentDomain();

        // Get the order rows
        List<OrderRow> orderRows = OrderRow.GetByOrderId(order.id);

        // Get the customer
        Customer customer = Customer.GetOneById(order.customer_id);

        // Loop all of the order rows
        for (int i = 0; i < orderRows.Count; i++)
        {
            // Get the product
            Product product = Product.GetOneById(orderRows[i].product_id, domain.back_end_language);

            // Continue if the product is null
            if (product == null) { continue; }

            // Check if the product has a downloadable files
            if (product.downloadable_files == true)
            {
                // Create the customer file
                CustomerFile customerFile = new CustomerFile();
                customerFile.customer_id = customer.id;
                customerFile.product_id = product.id;
                customerFile.language_id = customer.language_id;
                customerFile.order_date = order.order_date;

                // Check if the file already exists
                CustomerFile savedFile = CustomerFile.GetOneById(customerFile.customer_id, customerFile.product_id, customerFile.language_id);

                if (savedFile == null)
                {
                    // Add the customer file
                    CustomerFile.Add(customerFile);
                }
            }
        }

    } // End of the AddCustomerFiles method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of customer files for a customer
    /// </summary>
    /// <param name="customerId">The customer id</param>
    /// <returns>The number of file orders as an int</returns>
    public static Int32 GetCountByCustomerId(Int32 customerId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(customer_id) AS count FROM dbo.customers_files WHERE customer_id = @customer_id;";

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

    #endregion

    #region Get methods

    /// <summary>
    /// Get one customer file based on id
    /// </summary>
    /// <param name="customerId">The customer id</param>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a customer file post</returns>
    public static CustomerFile GetOneById(Int32 customerId, Int32 productId, Int32 languageId)
    {
        // Create the post to return
        CustomerFile post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers_files WHERE customer_id = @customer_id AND " 
            + "product_id = @product_id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@customer_id", customerId);
                cmd.Parameters.AddWithValue("@product_id", productId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
                        post = new CustomerFile(reader);
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
    /// Get customer files for a specific customer
    /// </summary>
    /// <param name="customerId">The id of the customer</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of customer file posts</returns>
    public static List<CustomerFile> GetByCustomerId(Int32 customerId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<CustomerFile> posts = new List<CustomerFile>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers_files WHERE customer_id = @customer_id ORDER BY " + sortField + " " + sortOrder + " "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
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
                        posts.Add(new CustomerFile(reader));
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

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a customer file by id
    /// </summary>
    /// <param name="customerId">The id for the customer</param>
    /// <param name="product_id">The id for the product</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 customerId, Int32 product_id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.customers_files WHERE customer_id = @customer_id AND product_id = @product_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@customer_id", customerId);
                cmd.Parameters.AddWithValue("@product_id", product_id);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
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

    /// <summary>
    /// Delete customer files on customer id
    /// </summary>
    /// <param name="customerId">The id for the customer</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnCustomerId(Int32 customerId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.customers_files WHERE customer_id = @customer_id;";

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

    } // End of the DeleteOnCustomerId method

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
        if (sortField != "customer_id" && sortField != "product_id" && sortField != "language_id" && sortField != "order_date")
        {
            sortField = "order_date";
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