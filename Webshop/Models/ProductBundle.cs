using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a product bundle
/// </summary>
public class ProductBundle
{
    
    #region Variables

    public Int32 bundle_product_id;
    public Int32 product_id;
    public decimal quantity;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new product bundle with default properties
    /// </summary>
    public ProductBundle()
    {
        // Set values for instance variables
        this.bundle_product_id = 0;
        this.product_id = 0;
        this.quantity = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new product bundle from a reader
    /// </summary>
    /// <param name="reader">A reference to reader</param>
    public ProductBundle(SqlDataReader reader)
    {
        // Set values for instance variables
        this.bundle_product_id = Convert.ToInt32(reader["bundle_product_id"]);
        this.product_id = Convert.ToInt32(reader["product_id"]);
        this.quantity = Convert.ToDecimal(reader["quantity"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one product bundle
    /// </summary>
    /// <param name="post">A reference to a product bundle post</param>
    public static void Add(ProductBundle post)
    {

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.product_bundles (bundle_product_id, product_id, quantity) "
            + "VALUES (@bundle_product_id, @product_id, @quantity);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@bundle_product_id", post.bundle_product_id);
                cmd.Parameters.AddWithValue("@product_id", post.product_id);
                cmd.Parameters.AddWithValue("@quantity", post.quantity);

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
    /// Update a product bundle
    /// </summary>
    /// <param name="post">A reference to a product bundle post</param>
    public static void Update(ProductBundle post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.product_bundles SET quantity = @quantity WHERE bundle_product_id = @bundle_product_id AND "
            + "product_id = @product_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@bundle_product_id", post.bundle_product_id);
                cmd.Parameters.AddWithValue("@product_id", post.product_id);
                cmd.Parameters.AddWithValue("@quantity", post.quantity);

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
    /// Get one product bundle on id
    /// </summary>
    /// <param name="bundleProductId">A bundle product id</param>
    /// <param name="productId">An product id</param>
    /// <returns>A reference to a product bundle post</returns>
    public static ProductBundle GetOneById(Int32 bundleProductId, Int32 productId)
    {
        // Create the post to return
        ProductBundle post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.product_bundles WHERE bundle_product_id = @bundle_product_id AND " 
            + "product_id = @product_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@bundle_product_id", bundleProductId);
                cmd.Parameters.AddWithValue("@product_id", productId);

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
                        post = new ProductBundle(reader);
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
    /// Get all product bundles
    /// </summary>
    /// <returns>A list of bundle product posts</returns>
    public static List<ProductBundle> GetAll()
    {
        // Create the list to return
        List<ProductBundle> posts = new List<ProductBundle>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.product_bundles ORDER BY bundle_product_id ASC, product_id ASC;";

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
                        posts.Add(new ProductBundle(reader));
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
    /// Get product bundles by the bundle product id
    /// </summary>
    /// <param name="bundleProductId">The id of the bundle product</param>
    /// <returns>A list of bundle product posts</returns>
    public static List<ProductBundle> GetByBundleProductId(Int32 bundleProductId)
    {
        // Create the list to return
        List<ProductBundle> posts = new List<ProductBundle>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.product_bundles WHERE bundle_product_id = @bundle_product_id "
            + "ORDER BY product_id ASC;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@bundle_product_id", bundleProductId);

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
                        posts.Add(new ProductBundle(reader));
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

    } // End of the GetByBundleProductId method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a product bundle post on id
    /// </summary>
    /// <param name="bundleProductId">The id number for the bundle product</param>
    /// <param name="productId">The id number for the product</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 bundleProductId, Int32 productId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.product_bundles WHERE bundle_product_id = @bundle_product_id AND " 
            + "product_id = @product_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@bundle_product_id", bundleProductId);
                cmd.Parameters.AddWithValue("@product_id", productId);

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