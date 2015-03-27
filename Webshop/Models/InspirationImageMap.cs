using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a image map used for inspiration
/// </summary>
public class InspirationImageMap
{
    #region Variables

    public Int32 id;
    public Int32 language_id;
    public string name;
    public string image_name;
    public string image_map_points;
    public Int32 category_id;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new image map with default properties
    /// </summary>
    public InspirationImageMap()
    {
        // Set values for instance variables
        this.id = 0;
        this.language_id = 0;
        this.name = "";
        this.image_name = "";
        this.image_map_points = "";
        this.category_id = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new image map from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public InspirationImageMap(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.language_id = Convert.ToInt32(reader["language_id"]);
        this.name = reader["name"].ToString();
        this.image_name = reader["image_name"].ToString();
        this.image_map_points = reader["image_map_points"].ToString();
        this.category_id = Convert.ToInt32(reader["category_id"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one image map
    /// </summary>
    /// <param name="post">A reference to a image map post</param>
    public static long Add(InspirationImageMap post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.inspiration_image_maps (language_id, name, image_name, image_map_points, category_id) "
            + "VALUES (@language_id, @name, @image_name, @image_map_points, @category_id);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", post.language_id);
                cmd.Parameters.AddWithValue("@name", post.name);
                cmd.Parameters.AddWithValue("@image_name", post.image_name);
                cmd.Parameters.AddWithValue("@image_map_points", post.image_map_points);
                cmd.Parameters.AddWithValue("@category_id", post.category_id);

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
    /// Update a image map post
    /// </summary>
    /// <param name="post">A reference to a image map post</param>
    public static void Update(InspirationImageMap post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.inspiration_image_maps SET language_id = @language_id, name = @name, image_name = @image_name, "
            + "image_map_points = @image_map_points, category_id = @category_id WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The Using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@language_id", post.language_id);
                cmd.Parameters.AddWithValue("@name", post.name);
                cmd.Parameters.AddWithValue("@image_name", post.image_name);
                cmd.Parameters.AddWithValue("@image_map_points", post.image_map_points);
                cmd.Parameters.AddWithValue("@category_id", post.category_id);

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

    #region Count methods

    /// <summary>
    /// Count the number of image maps by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <returns>The number of image maps as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.inspiration_image_maps WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR name LIKE @keyword_" + i.ToString()
               + " OR CAST(category_id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + ")";
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
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.inspiration_image_maps WHERE id = @id;";

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
    /// Get one image map based on id
    /// </summary>
    /// <param name="id">The id for the post</param>
    /// <returns>A reference to a image map post</returns>
    public static InspirationImageMap GetOneById(Int32 id)
    {
        // Create the post to return
        InspirationImageMap post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.inspiration_image_maps WHERE id = @id;";

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
                        post = new InspirationImageMap(reader);
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
    /// Get all image maps
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of image maps</returns>
    public static List<InspirationImageMap> GetAll(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<InspirationImageMap> posts = new List<InspirationImageMap>();

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.inspiration_image_maps ORDER BY " + sortField + " " + sortOrder + ";";

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
                        posts.Add(new InspirationImageMap(reader));
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
    /// Get image maps by category id and language id
    /// </summary>
    /// <param name="categoryId">The id of a category</param>
    /// <param name="languageId">The language id</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of image map posts</returns>
    public static List<InspirationImageMap> GetByCategoryId(Int32 categoryId, Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<InspirationImageMap> posts = new List<InspirationImageMap>(10);

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.inspiration_image_maps WHERE language_id = @language_id AND category_id = "
            + "@category_id ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@category_id", categoryId);

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
                        posts.Add(new InspirationImageMap(reader));
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

    } // End of the GetByCategoryId method

    /// <summary>
    /// Get image maps by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of image maps</returns>
    public static List<InspirationImageMap> GetBySearch(string[] keywords, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<InspirationImageMap> posts = new List<InspirationImageMap>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.inspiration_image_maps WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR name LIKE @keyword_" + i.ToString()
               + " OR CAST(category_id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + ")";
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
                        posts.Add(new InspirationImageMap(reader));
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
    /// Delete a image map post on id
    /// </summary>
    /// <param name="id">The id of the image map post</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.inspiration_image_maps WHERE id = @id;";

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
        if (sortField != "id" && sortField != "language_id" && sortField != "name" && sortField != "category_id")
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