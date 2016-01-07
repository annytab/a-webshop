using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a customer
/// </summary>
public class Customer
{
    #region Variables

    public Int32 id;
    public Int32 language_id;
    public string email;
    public string customer_password;
    public byte customer_type;
    public string org_number;
    public string vat_number;
    public string contact_name;
    public string phone_number;
    public string mobile_phone_number;
    public string invoice_name;
    public string invoice_address_1;
    public string invoice_address_2;
    public string invoice_post_code;
    public string invoice_city;
    public Int32 invoice_country;
    public string delivery_name;
    public string delivery_address_1;
    public string delivery_address_2;
    public string delivery_post_code;
    public string delivery_city;
    public Int32 delivery_country;
    public bool newsletter;
    public string facebook_user_id;
    public string google_user_id;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new customer with default properties
    /// </summary>
    public Customer()
    {
        // Set values for instance variables
        this.id = 0;
        this.language_id = 0;
        this.email = "";
        this.customer_password = "";
        this.customer_type = 0;
        this.org_number = "";
        this.vat_number = "";
        this.contact_name = "";
        this.phone_number = "";
        this.mobile_phone_number = "";
        this.invoice_name = "";
        this.invoice_address_1 = "";
        this.invoice_address_2 = "";
        this.invoice_post_code = "";
        this.invoice_city = "";
        this.invoice_country = 0;
        this.delivery_name = "";
        this.delivery_address_1 = "";
        this.delivery_address_2 = "";
        this.delivery_post_code = "";
        this.delivery_city = "";
        this.delivery_country = 0;
        this.newsletter = true;
        this.facebook_user_id = "";
        this.google_user_id = "";

    } // End of the constructor

    /// <summary>
    /// Create a new customer from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public Customer(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.language_id = Convert.ToInt32(reader["language_id"]);
        this.email = reader["email"].ToString();
        this.customer_password = "";
        this.customer_type = Convert.ToByte(reader["customer_type"]);
        this.org_number = reader["org_number"].ToString();
        this.vat_number = reader["vat_number"].ToString();
        this.contact_name = reader["contact_name"].ToString();
        this.phone_number = reader["phone_number"].ToString();
        this.mobile_phone_number = reader["mobile_phone_number"].ToString();
        this.invoice_name = reader["invoice_name"].ToString();
        this.invoice_address_1 = reader["invoice_address_1"].ToString();
        this.invoice_address_2 = reader["invoice_address_2"].ToString();
        this.invoice_post_code = reader["invoice_post_code"].ToString();
        this.invoice_city = reader["invoice_city"].ToString();
        this.invoice_country = Convert.ToInt32(reader["invoice_country"]);
        this.delivery_name = reader["delivery_name"].ToString();
        this.delivery_address_1 = reader["delivery_address_1"].ToString();
        this.delivery_address_2 = reader["delivery_address_2"].ToString();
        this.delivery_post_code = reader["delivery_post_code"].ToString();
        this.delivery_city = reader["delivery_city"].ToString();
        this.delivery_country = Convert.ToInt32(reader["delivery_country"]);
        this.newsletter = Convert.ToBoolean(reader["newsletter"]);
        this.facebook_user_id = reader["facebook_user_id"].ToString();
        this.google_user_id = reader["google_user_id"].ToString();

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one customer
    /// </summary>
    /// <param name="post">A reference to a customer post</param>
    public static long Add(Customer post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.customers (email, language_id, customer_password, customer_type, org_number, vat_number, contact_name, "
            + "phone_number, mobile_phone_number, invoice_name, invoice_address_1, invoice_address_2, invoice_post_code, "
            + "invoice_city, invoice_country, delivery_name, delivery_address_1, delivery_address_2, "
            + "delivery_post_code, delivery_city, delivery_country, newsletter, facebook_user_id, google_user_id) "
            + "VALUES (@email, @language_id, @customer_password, @customer_type, @org_number, @vat_number, @contact_name, @phone_number, @mobile_phone_number, " 
            + "@invoice_name, @invoice_address_1, @invoice_address_2, @invoice_post_code, @invoice_city, "
            + "@invoice_country, @delivery_name, @delivery_address_1, @delivery_address_2, @delivery_post_code, "
            + "@delivery_city, @delivery_country, @newsletter, @facebook_user_id, @google_user_id);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@email", post.email);
                cmd.Parameters.AddWithValue("@language_id", post.language_id);
                cmd.Parameters.AddWithValue("@customer_password", "");
                cmd.Parameters.AddWithValue("@org_number", post.org_number);
                cmd.Parameters.AddWithValue("@customer_type", post.customer_type);
                cmd.Parameters.AddWithValue("@vat_number", post.vat_number);
                cmd.Parameters.AddWithValue("@contact_name", post.contact_name);
                cmd.Parameters.AddWithValue("@phone_number", post.phone_number);
                cmd.Parameters.AddWithValue("@mobile_phone_number", post.mobile_phone_number);
                cmd.Parameters.AddWithValue("@invoice_name", post.invoice_name);
                cmd.Parameters.AddWithValue("@invoice_address_1", post.invoice_address_1);
                cmd.Parameters.AddWithValue("@invoice_address_2", post.invoice_address_2);
                cmd.Parameters.AddWithValue("@invoice_post_code", post.invoice_post_code);
                cmd.Parameters.AddWithValue("@invoice_city", post.invoice_city);
                cmd.Parameters.AddWithValue("@invoice_country", post.invoice_country);
                cmd.Parameters.AddWithValue("@delivery_name", post.delivery_name);
                cmd.Parameters.AddWithValue("@delivery_address_1", post.delivery_address_1);
                cmd.Parameters.AddWithValue("@delivery_address_2", post.delivery_address_2);
                cmd.Parameters.AddWithValue("@delivery_post_code", post.delivery_post_code);
                cmd.Parameters.AddWithValue("@delivery_city", post.delivery_city);
                cmd.Parameters.AddWithValue("@delivery_country", post.delivery_country);
                cmd.Parameters.AddWithValue("@newsletter", post.newsletter);
                cmd.Parameters.AddWithValue("@facebook_user_id", post.facebook_user_id);
                cmd.Parameters.AddWithValue("@google_user_id", post.google_user_id);

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
    /// Update a customer post
    /// </summary>
    /// <param name="post">A reference to a customer post</param>
    public static void Update(Customer post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.customers SET email = @email, language_id = @language_id, customer_type = @customer_type, org_number = @org_number, "
            + "vat_number = @vat_number, contact_name = @contact_name, phone_number = @phone_number, mobile_phone_number =  @mobile_phone_number, "
            + "invoice_name = @invoice_name, invoice_address_1 = @invoice_address_1, invoice_address_2 = @invoice_address_2, invoice_post_code = @invoice_post_code, "
            + "invoice_city = @invoice_city, invoice_country = @invoice_country, delivery_name = @delivery_name, "
            + "delivery_address_1 = @delivery_address_1, delivery_address_2 = @delivery_address_2, delivery_post_code = @delivery_post_code, "
            + "delivery_city = @delivery_city, delivery_country = @delivery_country, newsletter = @newsletter, facebook_user_id = @facebook_user_id, "
            + "google_user_id = @google_user_id WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@language_id", post.language_id);
                cmd.Parameters.AddWithValue("@email", post.email);
                cmd.Parameters.AddWithValue("@customer_type", post.customer_type);
                cmd.Parameters.AddWithValue("@org_number", post.org_number);
                cmd.Parameters.AddWithValue("@vat_number", post.vat_number);
                cmd.Parameters.AddWithValue("@contact_name", post.contact_name);
                cmd.Parameters.AddWithValue("@phone_number", post.phone_number);
                cmd.Parameters.AddWithValue("@mobile_phone_number", post.mobile_phone_number);
                cmd.Parameters.AddWithValue("@invoice_name", post.invoice_name);
                cmd.Parameters.AddWithValue("@invoice_address_1", post.invoice_address_1);
                cmd.Parameters.AddWithValue("@invoice_address_2", post.invoice_address_2);
                cmd.Parameters.AddWithValue("@invoice_post_code", post.invoice_post_code);
                cmd.Parameters.AddWithValue("@invoice_city", post.invoice_city);
                cmd.Parameters.AddWithValue("@invoice_country", post.invoice_country);
                cmd.Parameters.AddWithValue("@delivery_name", post.delivery_name);
                cmd.Parameters.AddWithValue("@delivery_address_1", post.delivery_address_1);
                cmd.Parameters.AddWithValue("@delivery_address_2", post.delivery_address_2);
                cmd.Parameters.AddWithValue("@delivery_post_code", post.delivery_post_code);
                cmd.Parameters.AddWithValue("@delivery_city", post.delivery_city);
                cmd.Parameters.AddWithValue("@delivery_country", post.delivery_country);
                cmd.Parameters.AddWithValue("@newsletter", post.newsletter);
                cmd.Parameters.AddWithValue("@facebook_user_id", post.facebook_user_id);
                cmd.Parameters.AddWithValue("@google_user_id", post.google_user_id);

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
    /// Update the password for a customer
    /// </summary>
    /// <param name="id">The id of the customer</param>
    /// <param name="password">The new password</param>
    public static void UpdatePassword(Int32 id, string password)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.customers SET customer_password = @customer_password WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@customer_password", password);

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

    } // End of the UpdatePassword method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of customers by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <returns>The number of customers as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.customers WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR invoice_name LIKE @keyword_" + i.ToString()
                + " OR org_number LIKE @keyword_" + i.ToString() + " OR contact_name LIKE @keyword_" + i.ToString() + ")";
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
        string sql = "SELECT COUNT(id) AS count FROM dbo.customers WHERE id = @id;";

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
    /// Check if the password is correct
    /// </summary>
    /// <param name="userName">The user name</param>
    /// <param name="password">The password</param>
    /// <returns>A boolean that indicates if the password is correct</returns>
    public static bool ValidatePassword(string userName, string password)
    {
        // Create the string to hold the password
        string passwordHash = "";

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT customer_password AS count FROM dbo.customers WHERE email = @email;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@email", userName);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Get the password hash
                    passwordHash = cmd.ExecuteScalar().ToString();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the boolean that indicates if the password is correct
        return PasswordHash.ValidatePassword(password, passwordHash);

    } // End of the ValidatePassword method

    /// <summary>
    /// Get one customer based on id
    /// </summary>
    /// <param name="customerId">A customer id</param>
    /// <returns>A reference to a customer post</returns>
    public static Customer GetOneById(Int32 customerId)
    {
        // Create the post to return
        Customer post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", customerId);

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
                        post = new Customer(reader);
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
    /// Get one customer based on facebook user id
    /// </summary>
    /// <param name="facebookUserId">A facebook user id</param>
    /// <returns>A reference to a customer post</returns>
    public static Customer GetOneByFacebookUserId(string facebookUserId)
    {
        // Create the post to return
        Customer post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers WHERE facebook_user_id = @facebook_user_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@facebook_user_id", facebookUserId);

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
                        post = new Customer(reader);
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

    } // End of the GetOneByFacebookUserId method

    /// <summary>
    /// Get one customer based on google user id
    /// </summary>
    /// <param name="googleUserId">A google user id</param>
    /// <returns>A reference to a customer post</returns>
    public static Customer GetOneByGoogleUserId(string googleUserId)
    {
        // Create the post to return
        Customer post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers WHERE google_user_id = @google_user_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@google_user_id", googleUserId);

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
                        post = new Customer(reader);
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

    } // End of the GetOneByGoogleUserId method

    /// <summary>
    /// Get one customer based on email
    /// </summary>
    /// <param name="email">A email address</param>
    /// <returns>A reference to a customer post</returns>
    public static Customer GetOneByEmail(string email)
    {
        // Create the post to return
        Customer post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers WHERE email = @email;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@email", email);

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
                        post = new Customer(reader);
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

    } // End of the GetOneByEmail method

    /// <summary>
    /// Get one customer based on the organization number
    /// </summary>
    /// <param name="orgNumber">A organization number</param>
    /// <returns>A reference to a customer post</returns>
    public static Customer GetOneByOrgNumber(string orgNumber)
    {
        // Create the post to return
        Customer post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers WHERE org_number = @org_number;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@org_number", orgNumber);

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
                        post = new Customer(reader);
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

    } // End of the GetOneByOrgNumber method

    /// <summary>
    /// Get all the customers
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of customer posts</returns>
    public static List<Customer> GetAll(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Customer> posts = new List<Customer>(10);

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers ORDER BY " + sortField + " " + sortOrder + ";";

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

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        posts.Add(new Customer(reader));
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
    /// Get customers that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The sort field</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of customers</returns>
    public static List<Customer> GetBySearch(string[] keywords, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Customer> posts = new List<Customer>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.customers WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR invoice_name LIKE @keyword_" + i.ToString()
                + " OR org_number LIKE @keyword_" + i.ToString() + " OR contact_name LIKE @keyword_" + i.ToString() + ")";
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
                        posts.Add(new Customer(reader));
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
    /// Get the signed-in customer
    /// </summary>
    /// <returns>A reference to a customer</returns>
    public static Customer GetSignedInCustomer()
    {
        // Create the customer to return
        Customer customer = null;

        // Get the customer cookie
        HttpCookie customerCookie = HttpContext.Current.Request.Cookies.Get("CustomerCookie");

        if (customerCookie != null)
        {
            Int32 customerId = 0;
            Int32.TryParse(Tools.UnprotectCookieValue(customerCookie.Value, "CustomerLogin"), out customerId);
            customer = Customer.GetOneById(customerId);
        }

        // Return the customer
        return customer;

    } // End of the GetSignedInCustomer method

    /// <summary>
    /// Get customer email addresses
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <param name="forNewsletter">A boolean that indicates if the customer has accepted newsletter</param>
    /// <returns>A string with customer emails</returns>
    public static string GetEmailAddresses(Int32 languageId, bool forNewsletter)
    {
        // Create the string
        string emailString = "";

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT email FROM dbo.customers WHERE language_id = @language_id ";

        // Check if we should exclude customer that do not want newsletters
        if(forNewsletter == true)
        {
            sql += "AND newsletter = 1 ";
        }

        // Add the final touch to the sql string
        sql += "ORDER BY id ASC;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
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

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        emailString += reader["email"].ToString() + ";";
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

        // Trim the end of the string
        emailString = emailString.TrimEnd(';');

        // Return the emailString
        return emailString;

    } // End of the GetEmailAddresses method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a customer post on id
    /// </summary>
    /// <param name="id">The id number for the customer</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.customers WHERE id = @id;";

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
        if (sortField != "id" && sortField != "invoice_name" && sortField != "org_number" && sortField != "contact_name")
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