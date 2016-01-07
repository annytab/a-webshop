using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// This class represent a payment option
/// </summary>
public class PaymentOption
{
    #region Variables

    public Int32 id;
    public string product_code;
    public string name;
    public string payment_term_code;
    public decimal fee;
    public Int32 unit_id;
    public Int32 connection;
    public Int32 value_added_tax_id;
    public string account_code;
    public bool inactive;
    
    #endregion

    #region Constructors

    /// <summary>
    /// Create a new payment option with default properties
    /// </summary>
    public PaymentOption()
    {
        // Set values for instance variables
        this.id = 0;
        this.product_code = "";
        this.name = "";
        this.payment_term_code = "";
        this.fee = 0.00M;
        this.unit_id = 0;
        this.connection = 0;
        this.value_added_tax_id = 0;
        this.account_code = "";
        this.inactive = false;

    } // End of the constructor

    /// <summary>
    /// Create a new payment option from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public PaymentOption(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.product_code = reader["product_code"].ToString();
        this.name = reader["name"].ToString();
        this.payment_term_code = reader["payment_term_code"].ToString();
        this.fee = Convert.ToDecimal(reader["fee"]);
        this.unit_id = Convert.ToInt32(reader["unit_id"]);
        this.connection = Convert.ToInt32(reader["connection"]);
        this.value_added_tax_id = Convert.ToInt32(reader["value_added_tax_id"]);
        this.account_code = reader["account_code"].ToString();
        this.inactive = Convert.ToBoolean(reader["inactive"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one payment option master post
    /// </summary>
    /// <param name="post">A reference to a payment option post</param>
    public static long AddMasterPost(PaymentOption post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.payment_options (product_code, payment_term_code, fee, unit_id, connection) "
            + "VALUES (@product_code, @payment_term_code, @fee, @unit_id, @connection);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_code", post.product_code);
                cmd.Parameters.AddWithValue("@payment_term_code", post.payment_term_code);
                cmd.Parameters.AddWithValue("@fee", post.fee);
                cmd.Parameters.AddWithValue("@unit_id", post.unit_id);
                cmd.Parameters.AddWithValue("@connection", post.connection);
                
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

    } // End of the AddMasterPost method

    /// <summary>
    /// Add one payment option language post
    /// </summary>
    /// <param name="post">A reference to a payment option post</param>
    /// <param name="languageId">The language id</param>
    public static void AddLanguagePost(PaymentOption post, Int32 languageId)
    {

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.payment_options_detail (payment_option_id, language_id, name, "
            + "value_added_tax_id, account_code, inactive) "
            + "VALUES (@payment_option_id, @language_id, @name, @value_added_tax_id, @account_code, @inactive);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@payment_option_id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@name", post.name);
                cmd.Parameters.AddWithValue("@value_added_tax_id", post.value_added_tax_id);
                cmd.Parameters.AddWithValue("@account_code", post.account_code);
                cmd.Parameters.AddWithValue("@inactive", post.inactive);

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

    } // End of the AddLanguagePost method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a payment option master post
    /// </summary>
    /// <param name="post">A reference to a payment option post</param>
    public static void UpdateMasterPost(PaymentOption post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.payment_options SET product_code = @product_code, payment_term_code = @payment_term_code, "
            + "fee = @fee, unit_id = @unit_id, connection = @connection WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@product_code", post.product_code);
                cmd.Parameters.AddWithValue("@payment_term_code", post.payment_term_code);
                cmd.Parameters.AddWithValue("@fee", post.fee);
                cmd.Parameters.AddWithValue("@unit_id", post.unit_id); 
                cmd.Parameters.AddWithValue("@connection", post.connection);
                
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

    } // End of the UpdateMasterPost method

    /// <summary>
    /// Update a payment option language post
    /// </summary>
    /// <param name="post">A reference to a payment option post</param>
    /// <param name="languageId">The language id</param>
    public static void UpdateLanguagePost(PaymentOption post, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.payment_options_detail SET name = @name, value_added_tax_id = "
            + "@value_added_tax_id, account_code = @account_code, inactive = @inactive WHERE "
            + "payment_option_id = @id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@name", post.name);
                cmd.Parameters.AddWithValue("@value_added_tax_id", post.value_added_tax_id);
                cmd.Parameters.AddWithValue("@account_code", post.account_code);
                cmd.Parameters.AddWithValue("@inactive", post.inactive);

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

    } // End of the UpdateLanguagePost method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of payment options by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The language id</param>
    /// <returns>The number of payment options as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.payment_options_detail AS D INNER JOIN "
            + "dbo.payment_options AS P ON D.payment_option_id = P.id WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(P.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR P.product_code LIKE @keyword_" + i.ToString()
                + " OR D.name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.payment_options WHERE id = @id;";

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
    /// Get one payment option based on id
    /// </summary>
    /// <param name="paymentOptionId">A payment option id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a payment option post</returns>
    public static PaymentOption GetOneById(Int32 paymentOptionId, Int32 languageId)
    {
        // Create the post to return
        PaymentOption post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.payment_options_detail AS D INNER JOIN dbo.payment_options AS P ON "
            + "D.payment_option_id = P.id WHERE P.id = @id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@id", paymentOptionId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new PaymentOption(reader);
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
    /// Get all payment options for a specific language
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of payment option posts</returns>
    public static List<PaymentOption> GetAll(Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<PaymentOption> posts = new List<PaymentOption>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.payment_options_detail AS D INNER JOIN dbo.payment_options AS P ON "
            + "D.payment_option_id = P.id WHERE D.language_id = @language_id ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(new PaymentOption(reader));
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
    /// Get all the active payment options for a specific language
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of payment option posts</returns>
    public static List<PaymentOption> GetAllActive(Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<PaymentOption> posts = new List<PaymentOption>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.payment_options_detail AS D INNER JOIN dbo.payment_options AS P ON "
            + "D.payment_option_id = P.id WHERE D.language_id = @language_id AND D.inactive = 0 ORDER BY " 
            + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(new PaymentOption(reader));
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

    } // End of the GetAllActive method

    /// <summary>
    /// Get payment options that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The language id</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The sort field</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of payment options</returns>
    public static List<PaymentOption> GetBySearch(string[] keywords, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<PaymentOption> posts = new List<PaymentOption>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.payment_options_detail AS D INNER JOIN dbo.payment_options AS P ON "
            + "D.payment_option_id = P.id WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(P.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR P.product_code LIKE @keyword_" + i.ToString()
                + " OR D.name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the select string
        sql += " ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);
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
                        posts.Add(new PaymentOption(reader));
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

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a payment option post and all language posts for this payment option post on id
    /// </summary>
    /// <param name="id">The id number for the payment option post</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.payment_options WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);

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
    /// Delete a language payment option post on id
    /// </summary>
    /// <param name="id">The id number for the payment option post</param>
    /// <param name="languageId">The language id</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteLanguagePostOnId(Int32 id, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.payment_options_detail WHERE payment_option_id = @id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

    } // End of the DeleteLanguagePostOnId method

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
        if (sortField != "id" && sortField != "product_code" && sortField != "name" && sortField != "fee" 
            && sortField != "inactive")
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
