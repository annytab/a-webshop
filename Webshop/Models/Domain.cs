using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a domain
/// </summary>
public class Domain
{
    #region Variables

    public Int32 id;
    public string webshop_name;
    public string domain_name;
    public string web_address;
    public Int32 country_id;
    public Int32 front_end_language;
    public Int32 back_end_language;
    public string currency;
    public Int32 company_id;
    public byte default_display_view;
    [Obsolete("Is not used anymore.")]
    public byte mobile_display_view;
    public Int32 custom_theme_id;
    public bool prices_includes_vat;
    public string analytics_tracking_id;
    public string facebook_app_id;
    public string facebook_app_secret;
    public string google_app_id;
    public string google_app_secret;
    public bool noindex;

    // Create a static write lock
    private static object writeLock = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new domain with default properties
    /// </summary>
    public Domain()
    {
        // Set values for instance variables
        this.id = 0;
        this.webshop_name = "";
        this.domain_name = "";
        this.web_address = "";
        this.country_id = 0;
        this.front_end_language = 0;
        this.back_end_language = 0;
        this.currency = "";
        this.company_id = 0;
        this.default_display_view = 0;
        this.mobile_display_view = 0;
        this.custom_theme_id = 0;
        this.prices_includes_vat = false;
        this.analytics_tracking_id = "";
        this.facebook_app_id = "";
        this.facebook_app_secret = "";
        this.google_app_id = "";
        this.google_app_secret = "";
        this.noindex = false;

    } // End of the constructor

    /// <summary>
    /// Create a new domain from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public Domain(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.webshop_name = reader["webshop_name"].ToString();
        this.domain_name = reader["domain_name"].ToString();
        this.web_address = reader["web_address"].ToString();
        this.country_id = Convert.ToInt32(reader["country_id"].ToString());
        this.front_end_language = Convert.ToInt32(reader["front_end_language"]);
        this.back_end_language = Convert.ToInt32(reader["back_end_language"]);
        this.currency = reader["currency"].ToString();
        this.company_id = Convert.ToInt32(reader["company_id"]);
        this.default_display_view = Convert.ToByte(reader["default_display_view"]);
        this.mobile_display_view = this.default_display_view;
        this.custom_theme_id = Convert.ToInt32(reader["custom_theme_id"]);
        this.prices_includes_vat = Convert.ToBoolean(reader["prices_includes_vat"]);
        this.analytics_tracking_id = reader["analytics_tracking_id"].ToString();
        this.facebook_app_id = reader["facebook_app_id"].ToString();
        this.facebook_app_secret = reader["facebook_app_secret"].ToString();
        this.google_app_id = reader["google_app_id"].ToString();
        this.google_app_secret = reader["google_app_secret"].ToString();
        this.noindex = Convert.ToBoolean(reader["noindex"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one domain
    /// </summary>
    /// <param name="post">A reference to a domain post</param>
    public static long Add(Domain post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.web_domains (webshop_name, domain_name, web_address, country_id, front_end_language, "
            + "back_end_language, currency, company_id, default_display_view, custom_theme_id, prices_includes_vat, "
            + "analytics_tracking_id, facebook_app_id, facebook_app_secret, google_app_id, google_app_secret, noindex) "
            + "VALUES (@webshop_name, @domain_name, @web_address, @country_id, @front_end_language, @back_end_language, "
            + "@currency, @company_id, @default_display_view, @custom_theme_id, @prices_includes_vat, "
            + "@analytics_tracking_id, @facebook_app_id, @facebook_app_secret, @google_app_id, @google_app_secret, @noindex);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@webshop_name", post.webshop_name);
                cmd.Parameters.AddWithValue("@domain_name", post.domain_name);
                cmd.Parameters.AddWithValue("@web_address", post.web_address);
                cmd.Parameters.AddWithValue("@country_id", post.country_id);
                cmd.Parameters.AddWithValue("@front_end_language", post.front_end_language);
                cmd.Parameters.AddWithValue("@back_end_language", post.back_end_language);
                cmd.Parameters.AddWithValue("@currency", post.currency);
                cmd.Parameters.AddWithValue("@company_id", post.company_id);
                cmd.Parameters.AddWithValue("@default_display_view", post.default_display_view);
                cmd.Parameters.AddWithValue("@custom_theme_id", post.custom_theme_id);
                cmd.Parameters.AddWithValue("@prices_includes_vat", post.prices_includes_vat);
                cmd.Parameters.AddWithValue("@analytics_tracking_id", post.analytics_tracking_id);
                cmd.Parameters.AddWithValue("@facebook_app_id", post.facebook_app_id);
                cmd.Parameters.AddWithValue("@facebook_app_secret", post.facebook_app_secret);
                cmd.Parameters.AddWithValue("@google_app_id", post.google_app_id);
                cmd.Parameters.AddWithValue("@google_app_secret", post.google_app_secret);
                cmd.Parameters.AddWithValue("@noindex", post.noindex);

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
    /// Update a domain post
    /// </summary>
    /// <param name="post">A reference to a domain post</param>
    public static void Update(Domain post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.web_domains SET webshop_name = @webshop_name, domain_name = @domain_name, web_address = @web_address, "
            + "country_id = @country_id, front_end_language = @front_end_language, back_end_language = @back_end_language, "
            + "currency = @currency, company_id = @company_id, default_display_view = @default_display_view, "
            + "custom_theme_id = @custom_theme_id, prices_includes_vat = @prices_includes_vat, analytics_tracking_id = @analytics_tracking_id, "
            + "facebook_app_id = @facebook_app_id, facebook_app_secret = @facebook_app_secret, google_app_id = @google_app_id, "
            + "google_app_secret = @google_app_secret, noindex = @noindex WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@webshop_name", post.webshop_name);
                cmd.Parameters.AddWithValue("@domain_name", post.domain_name);
                cmd.Parameters.AddWithValue("@web_address", post.web_address);
                cmd.Parameters.AddWithValue("@country_id", post.country_id);
                cmd.Parameters.AddWithValue("@front_end_language", post.front_end_language);
                cmd.Parameters.AddWithValue("@back_end_language", post.back_end_language);
                cmd.Parameters.AddWithValue("@currency", post.currency);
                cmd.Parameters.AddWithValue("@company_id", post.company_id);
                cmd.Parameters.AddWithValue("@default_display_view", post.default_display_view);
                cmd.Parameters.AddWithValue("@custom_theme_id", post.custom_theme_id);
                cmd.Parameters.AddWithValue("@prices_includes_vat", post.prices_includes_vat);
                cmd.Parameters.AddWithValue("@analytics_tracking_id", post.analytics_tracking_id);
                cmd.Parameters.AddWithValue("@facebook_app_id", post.facebook_app_id);
                cmd.Parameters.AddWithValue("@facebook_app_secret", post.facebook_app_secret);
                cmd.Parameters.AddWithValue("@google_app_id", post.google_app_id);
                cmd.Parameters.AddWithValue("@google_app_secret", post.google_app_secret);
                cmd.Parameters.AddWithValue("@noindex", post.noindex);

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

        // Remove the domain from cache
        Tools.RemoveKeyFromCache(post.domain_name);

    } // End of the Update method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of domains by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <returns>The number of domains as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.web_domains WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (webshop_name LIKE @keyword_" + i.ToString() + " OR domain_name LIKE @keyword_" + i.ToString() + ")";
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
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.web_domains WHERE id = @id;";

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
    /// Get one domain based on id
    /// </summary>
    /// <param name="id">The id for the post</param>
    /// <returns>A reference to a domain post</returns>
    public static Domain GetOneById(Int32 id)
    {
        // Create the post to return
        Domain post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.web_domains WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);

                // Create a MySqlDataReader
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
                        post = new Domain(reader);
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
    /// Get a domain by domain name from the cache or the database
    /// </summary>
    /// <param name="domainName">The domain name</param>
    /// <returns>A reference to the domain</returns>
    public static Domain GetOneByDomainName(string domainName)
    {
        // Get the domain from cache
        Domain domain = (Domain)HttpContext.Current.Cache[domainName];

        // Check if the domain is null
        if(domain == null)
        {
            // Add a lock to only insert once
            lock(writeLock)
            {
                // Check if the cache still is null
                if(HttpContext.Current.Cache[domainName] == null)
                {
                    // Get the domain from the database
                    domain = GetOneByName(domainName);

                    // Insert the domain in the cache
                    if (domain != null)
                    {
                        HttpContext.Current.Cache.Insert(domainName, domain, null, DateTime.UtcNow.AddHours(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
                else
                {
                    // Get the domain from cache
                    domain = (Domain)HttpContext.Current.Cache[domainName];
                }
            }
        }

        // Return the domain
        return domain;

    } // End of the GetOneByDomainName method

    /// <summary>
    /// Get one domain based on the domain name
    /// </summary>
    /// <param name="domainName">The domain name for the post</param>
    /// <returns>A reference to a domain post</returns>
    private static Domain GetOneByName(string domainName)
    {
        // Create the post to return
        Domain post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.web_domains WHERE domain_name = @domain_name;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@domain_name", domainName);

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
                        post = new Domain(reader);
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

    } // End of the GetOneByName method

    /// <summary>
    /// Get all domains
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of domains</returns>
    public static List<Domain> GetAll(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Domain> posts = new List<Domain>();

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.web_domains ORDER BY " + sortField + " " + sortOrder + ";";

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
                        posts.Add(new Domain(reader));
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
    /// Get domains by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The sort field</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of domains</returns>
    public static List<Domain> GetBySearch(string[] keywords, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Domain> posts = new List<Domain>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.web_domains WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (webshop_name LIKE @keyword_" + i.ToString() + " OR domain_name LIKE @keyword_" + i.ToString() + ")";
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
                        posts.Add(new Domain(reader));
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
    /// Delete a domain post on id
    /// </summary>
    /// <param name="id">The id of the domain post</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Get the domain
        Domain domain = Domain.GetOneById(id);

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.web_domains WHERE id = @id;";

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

        // Remove the domain from cache
        Tools.RemoveKeyFromCache(domain.domain_name);

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
        if (sortField != "id" && sortField != "webshop_name" && sortField != "domain_name")
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
