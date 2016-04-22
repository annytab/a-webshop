using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// This class represent a product accessory
/// </summary>
public class ProductAccessory
{
    #region Variables

    public Int32 product_id;
    public Int32 accessory_id;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new product accessory with default properties
    /// </summary>
    public ProductAccessory()
    {
        // Set values for instance variables
        this.product_id = 0;
        this.accessory_id = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new product accessory from a reader
    /// </summary>
    /// <param name="reader">A reference to reader</param>
    public ProductAccessory(SqlDataReader reader)
    {
        // Set values for instance variables
        this.product_id = Convert.ToInt32(reader["product_id"]);
        this.accessory_id = Convert.ToInt32(reader["accessory_id"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one product accessory
    /// </summary>
    /// <param name="post">A reference to a product accessory post</param>
    public static void Add(ProductAccessory post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.product_accessories (product_id, accessory_id) "
            + "VALUES (@product_id, @accessory_id);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", post.product_id);
                cmd.Parameters.AddWithValue("@accessory_id", post.accessory_id);

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

    #region Get methods

    /// <summary>
    /// Get one product accessory on id
    /// </summary>
    /// <param name="productId">A product id</param>
    /// <param name="accessoryId">An accessory id</param>
    /// <returns>A reference to a product accessory post</returns>
    public static ProductAccessory GetOneById(Int32 productId, Int32 accessoryId)
    {
        // Create the post to return
        ProductAccessory post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.product_accessories WHERE product_id = @product_id AND " 
            + "accessory_id = @accessory_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", productId);
                cmd.Parameters.AddWithValue("@accessory_id", accessoryId);

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
                        post = new ProductAccessory(reader);
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
    /// Get all product accessories
    /// </summary>
    /// <returns>A list of product accessory posts</returns>
    public static List<ProductAccessory> GetAll()
    {
        // Create the list to return
        List<ProductAccessory> posts = new List<ProductAccessory>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.product_accessories ORDER BY product_id ASC, accessory_id ASC;";

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
                        posts.Add(new ProductAccessory(reader));
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
    /// Get product accessories by product id
    /// </summary>
    /// <param name="productId">The id of the product</param>
    /// <param name="languageId">The language id</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of product posts</returns>
    public static List<Product> GetByProductId(Int32 productId, Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = Product.GetValidSortField(sortField);
        sortOrder = Product.GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Product> posts = new List<Product>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.product_accessories AS A INNER JOIN dbo.products AS P ON A.accessory_id "
            + "= P.id AND A.product_id = @product_id INNER JOIN dbo.products_detail AS D ON P.id = D.product_id "
            + "AND D.language_id = @language_id ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
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

                    // Fill the reader with data from the select command
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(new Product(reader));
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

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a product accessory post on id
    /// </summary>
    /// <param name="productId">The id number for the product</param>
    /// <param name="accessoryId">The id number for the accessory</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 productId, Int32 accessoryId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.product_accessories WHERE product_id = @product_id AND " 
            + "accessory_id = @accessory_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", productId);
                cmd.Parameters.AddWithValue("@accessory_id", accessoryId);

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