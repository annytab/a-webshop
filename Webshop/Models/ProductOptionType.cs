using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a product option type
/// </summary>
public class ProductOptionType
{
    #region Variables

    public Int32 id;
    public Int32 product_id;
    public Int32 option_type_id;
    public string google_name;
    public string title;
    public Int16 sort_order;
    public bool selected;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new product option type with default properties
    /// </summary>
    public ProductOptionType()
    {
        // Set values for instance variables
        this.id = 0;
        this.product_id = 0;
        this.option_type_id = 0;
        this.google_name = "";
        this.title = "";
        this.sort_order = 0;
        this.selected = false;

    } // End of the constructor

    /// <summary>
    /// Create a new product option type from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public ProductOptionType(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.product_id = Convert.ToInt32(reader["product_id"]);
        this.option_type_id = Convert.ToInt32(reader["option_type_id"]);
        this.google_name = reader["google_name"].ToString();
        this.title = reader["title"].ToString();
        this.sort_order = Convert.ToInt16(reader["sort_order"]);
        this.selected = Convert.ToBoolean(reader["selected"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one product option type post
    /// </summary>
    /// <param name="post">A reference to a product option type post</param>
    public static long Add(ProductOptionType post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.product_option_types (product_id, option_type_id, sort_order) " +
            "VALUES (@product_id, @option_type_id, @sort_order);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The Using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", post.product_id);
                cmd.Parameters.AddWithValue("@option_type_id", post.option_type_id);
                cmd.Parameters.AddWithValue("@sort_order", post.sort_order);

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
    /// Update a product option type post
    /// </summary>
    /// <param name="post">A reference to a product option type post</param>
    public static void Update(ProductOptionType post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.product_option_types SET sort_order = @sort_order "
            + "WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
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
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.product_option_types WHERE id = @id;";

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
    /// Get one product option type based on id
    /// </summary>
    /// <param name="id">An id</param>
    /// <returns>A reference to a product option type post</returns>
    public static ProductOptionType GetOneById(Int32 id)
    {
        // Create the post to return
        ProductOptionType post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT id, product_id, option_type_id, 'Empty' AS google_name, 'Empty' AS title, sort_order, 0 as selected FROM dbo.product_option_types "
            + "WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@id", id);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Fill the reader with one row of data
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new ProductOptionType(reader);
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
    /// Get one product option type based on id
    /// </summary>
    /// <param name="id">An id</param>
    /// <param name="languageId">A language id</param>
    /// <returns>A reference to a product option type post</returns>
    public static ProductOptionType GetOneById(Int32 id, Int32 languageId)
    {
        // Create the post to return
        ProductOptionType post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT T.id, T.product_id, T.option_type_id, O.google_name, D.title, T.sort_order, 1 as selected FROM dbo.product_option_types "
            + "AS T INNER JOIN dbo.option_types_detail AS D ON T.option_type_id = D.option_type_id INNER JOIN dbo.option_types AS O ON D.option_type_id = O.id "
            + "WHERE T.id = @id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Fill the reader with one row of data
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new ProductOptionType(reader);
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
    /// Get all product option types
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <returns>A list of product option type posts</returns>
    public static List<ProductOptionType> GetAll(Int32 languageId)
    {
        // Create the list to return
        List<ProductOptionType> posts = new List<ProductOptionType>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT T.id, T.product_id, T.option_type_id, O.google_name, D.title, T.sort_order, 1 as selected FROM dbo.product_option_types "
            + "AS T INNER JOIN dbo.option_types_detail AS D ON T.option_type_id = D.option_type_id INNER JOIN dbo.option_types AS O ON D.option_type_id = O.id "
            + "WHERE D.language_id = @language_id ORDER BY T.sort_order ASC;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
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

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new ProductOptionType(reader));
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
    /// Get product option types based on product id
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A list of product option type posts</returns>
    public static List<ProductOptionType> GetByProductId(Int32 productId, Int32 languageId)
    {
        // Create the list to return
        List<ProductOptionType> posts = new List<ProductOptionType>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT T.id, T.product_id, T.option_type_id, O.google_name, D.title, T.sort_order, 1 as selected FROM dbo.product_option_types "
            + "AS T INNER JOIN dbo.option_types_detail AS D ON T.option_type_id = D.option_type_id INNER JOIN dbo.option_types AS O ON D.option_type_id = O.id "
            + "WHERE T.product_id = @product_id AND D.language_id = @language_id ORDER BY T.sort_order ASC;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", productId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
                        posts.Add(new ProductOptionType(reader));
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

    } // End of the GetByProductId method

    /// <summary>
    /// Get all option types that not are included in the list of product option types
    /// </summary>
    /// <param name="productOptionTypes">A list of product option types</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A list of product option type posts</returns>
    public static List<ProductOptionType> GetOtherOptionTypes(List<ProductOptionType> productOptionTypes, Int32 languageId)
    {
        // Create the list to return
        List<ProductOptionType> posts = new List<ProductOptionType>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT 0 AS id, 0 as product_id, D.option_type_id, O.google_name, D.title, 0 AS sort_order, 0 AS selected "
            + "FROM dbo.option_types_detail AS D INNER JOIN dbo.option_types AS O ON D.option_type_id = O.id WHERE D.language_id = @language_id";

        // Do not include product option types
        if (productOptionTypes != null)
        {
            for (int i = 0; i < productOptionTypes.Count; i++)
            {
                sql += " AND O.id != @option_type_id_" + i.ToString();
            }
        }

        // Add the final touch to the select string
        sql += " ORDER BY O.id ASC;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Add parameters for product option types
                if (productOptionTypes != null)
                {
                    for (int i = 0; i < productOptionTypes.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@option_type_id_" + i.ToString(), productOptionTypes[i].option_type_id);
                    }
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
                        posts.Add(new ProductOptionType(reader));
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

    } // End of the GetOtherOptionTypesByProductId method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a product option type post on id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.product_option_types WHERE id = @id;";

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

} // End of the class
