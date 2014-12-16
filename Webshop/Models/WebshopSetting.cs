using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class handles the settings for the webshop
/// </summary>
public class WebshopSetting
{
    #region Variables

    public string id;
    public string value;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new webshop setting with default properties
    /// </summary>
    public WebshopSetting()
    {
        // Set values for instance variables
        this.id = "";
        this.value = "";

    } // End of the constructor

    /// <summary>
    /// Create a new webshop setting from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public WebshopSetting(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = reader["id"].ToString();
        this.value = reader["value"].ToString();

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one webshop setting
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    public static void Add(string key, string value)
    {
        // Clear the cache
        RemoveCachedSettings();

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.webshop_settings (id, value) VALUES (@id, @value);";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", key);
                cmd.Parameters.AddWithValue("@value", value);

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
    /// Update a webshop setting
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    public static void Update(string key, string value)
    {

        // Clear the cache
        RemoveCachedSettings();

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.webshop_settings SET value = @value WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", key);
                cmd.Parameters.AddWithValue("@value", value);

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
    /// Get one value by key
    /// </summary>
    /// <param name="key">The key</param>
    /// <returns>The value as a string</returns>
    public static string GetOneByKey(string key)
    {
        // Create the string to return
        string value = "";

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.webshop_settings WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", key);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Get the value
                    value = cmd.ExecuteScalar().ToString();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the value
        return value;

    } // End of the GetOneByKey method

    /// <summary>
    /// Get all webshop settings from the cache or from the database if the cache is null
    /// </summary>
    /// <returns>A key string list with shop settings</returns>
    public static KeyStringList GetAllFromCache()
    {
        // Get the cached webshop settings
        KeyStringList webshopSettings = (KeyStringList)HttpContext.Current.Cache["WebshopSettings"];

        // Check if the webshop settings is different from null
        if (webshopSettings != null)
        {
            return webshopSettings;
        }

        // Get the webshop settings
        webshopSettings = GetAll();

        // Create the cache
        HttpContext.Current.Cache.Insert("WebshopSettings", webshopSettings);

        // Return the settings for the webshop
        return webshopSettings;

    } // End of the GetAllFromCache method

    /// <summary>
    /// Get all the webshop settings as a key string list
    /// </summary>
    /// <returns>A key string list of shop settings</returns>
    public static KeyStringList GetAll()
    {

        // Create the KeyStringList to return
        KeyStringList posts = new KeyStringList(20);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.webshop_settings ORDER BY id ASC;";

        // The using block is used to call dispose automatically even if there are an exception
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
                        posts.Add(reader["id"].ToString(), reader["value"].ToString());
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetAll method

    #endregion

    #region Helper methods

    /// <summary>
    /// Remove the cached webshop settings
    /// </summary>
    private static void RemoveCachedSettings()
    {
        // Make sure that the webshop settings is different from null
        if (HttpContext.Current.Cache["WebshopSettings"] != null)
        {
            // Remove the webshop settings
            HttpContext.Current.Cache.Remove("WebshopSettings");
        }

    } // End of the RemoveCachedSettings method

    #endregion

} // End of the class
