using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a custom theme template
/// </summary>
public class CustomThemeTemplate
{
    #region Variables

    public Int32 custom_theme_id;
    public string user_file_name;
    public string master_file_url;
    public string user_file_content;
    public string comment;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new custom theme template with default properties
    /// </summary>
    public CustomThemeTemplate()
    {
        // Set values for instance variables
        this.custom_theme_id = 0;
        this.user_file_name = "";
        this.master_file_url = "";
        this.user_file_content = "";
        this.comment = "";

    } // End of the constructor

    /// <summary>
    /// Create a new custom theme template with properties from parameters
    /// </summary>
    public CustomThemeTemplate(Int32 id, string userFileName, string masterFileUrl, string userFileContent, string comment)
    {
        // Set values for instance variables
        this.custom_theme_id = id;
        this.user_file_name = userFileName;
        this.master_file_url = masterFileUrl;
        this.user_file_content = userFileContent;
        this.comment = comment;

    } // End of the constructor

    /// <summary>
    /// Create a new custom theme template from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public CustomThemeTemplate(SqlDataReader reader)
    {
        // Set values for instance variables
        this.custom_theme_id = Convert.ToInt32(reader["custom_theme_id"]);
        this.user_file_name = reader["user_file_name"].ToString();
        this.master_file_url = reader["master_file_url"].ToString();
        this.user_file_content = reader["user_file_content"].ToString();
        this.comment = reader["comment"].ToString();

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one custom theme template
    /// </summary>
    /// <param name="post">A reference to a custom theme template post</param>
    public static void Add(CustomThemeTemplate post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.custom_themes_templates (custom_theme_id, user_file_name, master_file_url, user_file_content, comment) "
            + "VALUES (@custom_theme_id, @user_file_name, @master_file_url, @user_file_content, @comment);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@custom_theme_id", post.custom_theme_id);
                cmd.Parameters.AddWithValue("@user_file_name", post.user_file_name);
                cmd.Parameters.AddWithValue("@master_file_url", post.master_file_url);
                cmd.Parameters.AddWithValue("@user_file_content", post.user_file_content);
                cmd.Parameters.AddWithValue("@comment", post.comment);

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
    /// Update a custom theme template file
    /// </summary>
    /// <param name="template">A reference to a custom theme template post</param>
    public static void Update(CustomThemeTemplate template)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.custom_themes_templates SET user_file_content = @user_file_content, "
            + "comment = @comment WHERE custom_theme_id = @custom_theme_id AND user_file_name = @user_file_name;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@custom_theme_id", template.custom_theme_id);
                cmd.Parameters.AddWithValue("@user_file_name", template.user_file_name);
                cmd.Parameters.AddWithValue("@user_file_content", template.user_file_content);
                cmd.Parameters.AddWithValue("@comment", template.comment);

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
    /// Get one post by id
    /// </summary>
    /// <param name="customThemeId">The id</param>
    /// <param name="userFileName">The user file name</param>
    /// <returns>A reference to a custom theme template</returns>
    public static CustomThemeTemplate GetOneById(Int32 customThemeId, string userFileName)
    {
        // Create the post to return
        CustomThemeTemplate post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.custom_themes_templates WHERE custom_theme_id = @custom_theme_id " 
            + "AND user_file_name = @user_file_name;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@custom_theme_id", customThemeId);
                cmd.Parameters.AddWithValue("@user_file_name", userFileName);

                // Create a reader
                SqlDataReader reader = null;

                // The try/catch/finally statement is used to handle unusual exceptions in the code to
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
                        post = new CustomThemeTemplate(reader);
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
    /// Get all the templates with the specified custom theme id as a dictionary
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list with all custom theme templates</returns>
    public static Dictionary<string, string> GetAllByCustomThemeId(Int32 customThemeId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the KeyStringList to return
        Dictionary<string, string> posts = new Dictionary<string, string>(40);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.custom_themes_templates WHERE custom_theme_id = @custom_theme_id " 
            + "ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            
            // The using block is used to call dispose automatically even if there is an exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@custom_theme_id", customThemeId);

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
                        posts.Add(reader["user_file_name"].ToString(), reader["user_file_content"].ToString());
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

    } // End of the GetAllByCustomThemeId method

    /// <summary>
    /// Get all the custom theme templates as a list with posts
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of custom theme template posts</returns>
    public static List<CustomThemeTemplate> GetAllPosts(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<CustomThemeTemplate> posts = new List<CustomThemeTemplate>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.custom_themes_templates ORDER BY custom_theme_id ASC, " 
            + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
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
                        posts.Add(new CustomThemeTemplate(reader));
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

    } // End of the GetAllPosts method

    /// <summary>
    /// Get all custom theme templates by custom theme id as a list with posts
    /// </summary>
    /// <param name="customThemeId">The custom theme id</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of custom theme template posts</returns>
    public static List<CustomThemeTemplate> GetAllPostsByCustomThemeId(Int32 customThemeId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<CustomThemeTemplate> posts = new List<CustomThemeTemplate>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.custom_themes_templates WHERE custom_theme_id = @custom_theme_id "
            + "ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@custom_theme_id", customThemeId);

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
                        posts.Add(new CustomThemeTemplate(reader));
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

    } // End of the GetAllPostsByCustomThemeId method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a custom theme template post on id
    /// </summary>
    /// <param name="customThemeId">The id of the custom theme template post</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 customThemeId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.custom_themes_templates WHERE custom_theme_id = @custom_theme_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@custom_theme_id", customThemeId);

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
    /// Delete a custom theme template post on id
    /// </summary>
    /// <param name="customThemeId">The id of the custom theme template post</param>
    /// <param name="userFileName">The user file name</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 customThemeId, string userFileName)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.custom_themes_templates WHERE custom_theme_id = @custom_theme_id " 
            + "AND user_file_name = @user_file_name;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@custom_theme_id", customThemeId);
                cmd.Parameters.AddWithValue("@user_file_name", userFileName);

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
        if (sortField != "user_file_name" && sortField != "master_file_url")
        {
            sortField = "user_file_name";
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

    #region Helper methods

    /// <summary>
    /// Get the content of the master file
    /// </summary>
    /// <param name="filename">The file name</param>
    /// <returns>The content of the file as a string</returns>
    public static string GetMasterFileContent(string filename)
    {
        // Create the string to return
        string content = "";

        // Create the stream reader
        StreamReader reader = null;

        try
        {
            reader = new StreamReader(HttpContext.Current.Server.MapPath(filename));
            content = reader.ReadToEnd();
        }
        catch (Exception e)
        {
            string exMessage = e.Message;
        }
        finally
        {
            // Make sure that the reader is different from null
            if (reader != null)
            {
                // Close the reader
                reader.Close();
            }
        }

        // Return the file content
        return content;

    } // End of the GetMasterFileContent method

    #endregion

} // End of the class