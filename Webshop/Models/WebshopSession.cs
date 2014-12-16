using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a webshop session
/// </summary>
public class WebshopSession
{
    #region Variables

    public string id;
    public string application_name;
    public DateTime created_date;
    public DateTime expires_date;
    public DateTime lock_date;
    public Int32 lock_id;
    public Int32 timeout_limit;
    public bool locked;
    public string session_items;
    public Int32 flags;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new webshop session with default properties
    /// </summary>
    public WebshopSession()
    {
        // Set values for instance variables
        this.id = "";
        this.application_name = "";
        this.created_date = DateTime.Now;
        this.expires_date = DateTime.Now;
        this.lock_date = DateTime.Now;
        this.lock_id = 0;
        this.timeout_limit = 0;
        this.locked = false;
        this.session_items = "";
        this.flags = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new webshop session from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public WebshopSession(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = reader["id"].ToString();
        this.application_name = reader["application_name"].ToString();
        this.created_date = Convert.ToDateTime(reader["created_date"]);
        this.expires_date = Convert.ToDateTime(reader["expires_date"]);
        this.lock_date = Convert.ToDateTime(reader["lock_date"]);
        this.lock_id = Convert.ToInt32(reader["lock_id"]);
        this.timeout_limit = Convert.ToInt32(reader["timeout_limit"]);
        this.locked = Convert.ToBoolean(reader["locked"]);
        this.session_items = reader["session_items"].ToString();
        this.flags = Convert.ToInt32(reader["flags"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one webshop session post
    /// </summary>
    /// <param name="post">A reference to a webshop session post</param>
    public static void Add(WebshopSession post)
    {

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.webshop_sessions (id, application_name, created_date, expires_date, " 
            + "lock_date, lock_id, timeout_limit, locked, session_items, flags) "
            + "VALUES (@id, @application_name, @created_date, @expires_date, @lock_date, @lock_id, "
            + "@timeout_limit, @locked, @session_items, @flags);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@application_name", post.application_name);
                cmd.Parameters.AddWithValue("@created_date", post.created_date);
                cmd.Parameters.AddWithValue("@expires_date", post.expires_date);
                cmd.Parameters.AddWithValue("@lock_date", post.lock_date);
                cmd.Parameters.AddWithValue("@lock_id", post.lock_id);
                cmd.Parameters.AddWithValue("@timeout_limit", post.timeout_limit);
                cmd.Parameters.AddWithValue("@locked", post.locked);
                cmd.Parameters.AddWithValue("@session_items", post.session_items);
                cmd.Parameters.AddWithValue("@flags", post.flags);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the Add method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a webshop session post
    /// </summary>
    /// <param name="post">A reference to a webshop session post</param>
    public static void Update(WebshopSession post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.webshop_sessions SET created_date = @created_date, expires_date = @expires_date, " 
            + "lock_date = @lock_date, lock_id = @lock_id, timeout_limit = @timeout_limit, locked = @locked, " 
            + "session_items = @session_items, flags = @flags "
            + "WHERE id = @id AND application_name = @application_name;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@application_name", post.application_name);
                cmd.Parameters.AddWithValue("@created_date", post.created_date);
                cmd.Parameters.AddWithValue("@expires_date", post.expires_date);
                cmd.Parameters.AddWithValue("@lock_date", post.lock_date);
                cmd.Parameters.AddWithValue("@lock_id", post.lock_id);
                cmd.Parameters.AddWithValue("@timeout_limit", post.timeout_limit);
                cmd.Parameters.AddWithValue("@locked", post.locked);
                cmd.Parameters.AddWithValue("@session_items", post.session_items);
                cmd.Parameters.AddWithValue("@flags", post.flags);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the Update method

    /// <summary>
    /// Update a webshop session post with lock id
    /// </summary>
    /// <param name="post">A reference to a webshop session post</param>
    public static void UpdateWithLockId(WebshopSession post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.webshop_sessions SET expires_date = @expires_date, "
            + "locked = @locked, session_items = @session_items "
            + "WHERE id = @id AND application_name = @application_name AND lock_id = @lock_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@application_name", post.application_name);
                cmd.Parameters.AddWithValue("@lock_id", post.lock_id);
                cmd.Parameters.AddWithValue("@expires_date", post.expires_date);
                cmd.Parameters.AddWithValue("@locked", post.locked);
                cmd.Parameters.AddWithValue("@session_items", post.session_items);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the UpdateWithLockId method

    /// <summary>
    /// Update the expiration date for a webshop session
    /// </summary>
    /// <param name="post">A reference to a webshop session post</param>
    public static void UpdateExpirationDate(WebshopSession post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.webshop_sessions SET expires_date = @expires_date "
            + "WHERE id = @id AND application_name = @application_name;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@application_name", post.application_name);
                cmd.Parameters.AddWithValue("@expires_date", post.expires_date);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the UpdateExpirationDate method

    /// <summary>
    /// Lock a webshop session
    /// </summary>
    /// <param name="post">A reference to a webshop session post</param>
    public static Int32 Lock(WebshopSession post)
    {
        // Create the int to return
        Int32 postsAffected = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.webshop_sessions SET lock_date = @lock_date, locked = @locked "
            + "WHERE id = @id AND application_name = @application_name AND locked = 0 AND expires_date > @expires_date;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@application_name", post.application_name);
                cmd.Parameters.AddWithValue("@expires_date", post.expires_date);
                cmd.Parameters.AddWithValue("@lock_date", post.lock_date);
                cmd.Parameters.AddWithValue("@locked", post.locked);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    postsAffected = cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

        // Return the int
        return postsAffected;

    } // End of the Lock method

    /// <summary>
    /// Unlock a webshop session
    /// </summary>
    /// <param name="post">A reference to a webshop session post</param>
    public static void Unlock(WebshopSession post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.webshop_sessions SET expires_date = @expires_date, locked = @locked "
            + "WHERE id = @id AND application_name = @application_name AND lock_id = @lock_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@application_name", post.application_name);
                cmd.Parameters.AddWithValue("@lock_id", post.lock_id);
                cmd.Parameters.AddWithValue("@expires_date", post.expires_date);
                cmd.Parameters.AddWithValue("@locked", post.locked);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the Unlock method

    /// <summary>
    /// Update the lock id and flags
    /// </summary>
    /// <param name="post">A reference to a webshop session post</param>
    public static void UpdateLockIdAndFlags(string id, string applicationName, Int32 lockId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.webshop_sessions SET lock_id = @lock_id, flags = 0 "
            + "WHERE id = @id AND application_name = @application_name;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@application_name", applicationName);
                cmd.Parameters.AddWithValue("@lock_id", lockId);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the UpdateLockIdAndFlags method

    #endregion

    #region Get methods

    /// <summary>
    /// Get one webshop session based on id
    /// </summary>
    /// <param name="id">The id</param>
    /// <param name="applicationName">The application name</param>
    /// <returns>A reference to a webshop session post</returns>
    public static WebshopSession GetOneById(string id, string applicationName)
    {
        // Create the post to return
        WebshopSession post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.webshop_sessions WHERE id = @id AND application_name = @application_name;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@application_name", applicationName);

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
                        post = new WebshopSession(reader);
                    }
                }
                catch (Exception e)
                {
                    // We do not want to throw an exception
                    string exMessage = e.Message;
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

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a webshop session post on id
    /// </summary>
    /// <param name="id">The id for session</param>
    /// <param name="applicationName">The application name</param>
    public static void DeleteOnId(string id, string applicationName)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.webshop_sessions WHERE id = @id AND application_name = @application_name;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@application_name", applicationName);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the DeleteOnId method

    /// <summary>
    /// Delete a webshop session post on id
    /// </summary>
    /// <param name="id">The id for session</param>
    /// <param name="applicationName">The application name</param>
    /// <param name="lockId">The lock id</param>
    public static void DeleteOnId(string id, string applicationName, Int32 lockId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.webshop_sessions WHERE id = @id AND application_name = @application_name " 
            + "AND lock_id = @lock_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@application_name", applicationName);
                cmd.Parameters.AddWithValue("@lock_id", lockId);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the DeleteOnId method

    /// <summary>
    /// Delete all webshop sessions that has expired
    /// </summary>
    public static void DeleteAllExpired()
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.webshop_sessions WHERE expires_date < @expires_date;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@expires_date", DateTime.Now);

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
                    // We do not want to throw an exception
                    string exMessage = e.Message;
                }
            }
        }

    } // End of the DeleteAllExpired method

    #endregion

} // End of the class