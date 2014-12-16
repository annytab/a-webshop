using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a product option
/// </summary>
public class ProductOption
{
    #region Variables

    public Int32 product_option_type_id;
    public Int32 option_id;
    public string title;
    public string product_code_suffix;
    public string mpn_suffix;
    public decimal price_addition;
    public decimal freight_addition;
    public bool selected;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new product option with default properties
    /// </summary>
    public ProductOption()
    {
        // Set values for instance variables
        this.product_option_type_id = 0;
        this.option_id = 0;
        this.title = "";
        this.product_code_suffix = "";
        this.mpn_suffix = "";
        this.price_addition = 0;
        this.freight_addition = 0;
        this.selected = false;

    } // End of the constructor

    /// <summary>
    /// Create a new product option from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public ProductOption(SqlDataReader reader)
    {
        // Set values for instance variables
        this.product_option_type_id = Convert.ToInt32(reader["product_option_type_id"]);
        this.option_id = Convert.ToInt32(reader["option_id"]);
        this.title = reader["title"].ToString();
        this.product_code_suffix = reader["product_code_suffix"].ToString();
        this.mpn_suffix = reader["mpn_suffix"].ToString();
        this.price_addition = Convert.ToDecimal(reader["price_addition"]);
        this.freight_addition = Convert.ToDecimal(reader["freight_addition"]);
        this.selected = Convert.ToBoolean(reader["selected"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one product option post
    /// </summary>
    /// <param name="post">A reference to a product option post</param>
    public static void Add(ProductOption post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.product_options (product_option_type_id, option_id, mpn_suffix, " 
            + "price_addition, freight_addition) " +
            "VALUES (@product_option_type_id, @option_id, @mpn_suffix, @price_addition, @freight_addition);";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_option_type_id", post.product_option_type_id);
                cmd.Parameters.AddWithValue("@option_id", post.option_id);
                cmd.Parameters.AddWithValue("@mpn_suffix", post.mpn_suffix);
                cmd.Parameters.AddWithValue("@price_addition", post.price_addition);
                cmd.Parameters.AddWithValue("@freight_addition", post.freight_addition);

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
    /// Update a product option post
    /// </summary>
    /// <param name="post">A reference to a product option post</param>
    public static void Update(ProductOption post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.product_options SET mpn_suffix = @mpn_suffix, price_addition = @price_addition, "
            + "freight_addition = @freight_addition WHERE product_option_type_id = @product_option_type_id AND option_id = @option_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_option_type_id", post.product_option_type_id);
                cmd.Parameters.AddWithValue("@option_id", post.option_id);
                cmd.Parameters.AddWithValue("@mpn_suffix", post.mpn_suffix);
                cmd.Parameters.AddWithValue("@price_addition", post.price_addition);
                cmd.Parameters.AddWithValue("@freight_addition", post.freight_addition);

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
    /// Update price additions for product options
    /// </summary>
    /// <param name="priceMultiplier">The price multiplier as a decimal</param>
    public static void UpdatePriceAdditions(decimal priceMultiplier)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.product_options SET price_addition = ROUND(price_addition * @price_multiplier, 2);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@price_multiplier", priceMultiplier);

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

    } // End of the UpdatePriceAdditions method

    /// <summary>
    /// Update freight additions for product options
    /// </summary>
    /// <param name="priceMultiplier">The price multiplier as a decimal</param>
    public static void UpdateFreightAdditions(decimal priceMultiplier)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.product_options SET freight_addition = ROUND(freight_addition * @price_multiplier, 2);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@price_multiplier", priceMultiplier);

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

    } // End of the UpdateFreightAdditions method

    #endregion

    #region Get methods

    /// <summary>
    /// Get one product option post based on id
    /// </summary>
    /// <param name="productOptionTypeId">A product option type id</param>
    /// <param name="optionId">A option id</param>
    /// <returns>A reference to a product option post</returns>
    public static ProductOption GetOneById(Int32 productOptionTypeId, Int32 optionId)
    {
        // Create the post to return
        ProductOption post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT product_option_type_id, option_id, mpn_suffix, price_addition, freight_addition, "
            + "'Empty' AS product_code_suffix, 'Empty' AS title, 0 AS selected FROM dbo.product_options "
            + "WHERE product_option_type_id = @product_option_type_id AND option_id = @option_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@product_option_type_id", productOptionTypeId);
                cmd.Parameters.AddWithValue("@option_id", optionId);

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
                        post = new ProductOption(reader);
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
    /// Get one product option post based on id
    /// </summary>
    /// <param name="productOptionTypeId">A product option type id</param>
    /// <param name="optionId">A option id</param>
    /// <param name="optionId">A language id</param>
    /// <returns>A reference to a product option post</returns>
    public static ProductOption GetOneById(Int32 productOptionTypeId, Int32 optionId, Int32 languageId)
    {
        // Create the post to return
        ProductOption post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT P.product_option_type_id, P.option_id, P.mpn_suffix, P.price_addition, P.freight_addition, "
            + "O.product_code_suffix, D.title, 1 as selected FROM dbo.product_options AS P INNER JOIN "
            + "dbo.options AS O ON P.option_id = O.id INNER JOIN dbo.options_detail AS D ON P.option_id = D.option_id "
            + "WHERE P.product_option_type_id = @product_option_type_id AND P.option_id = @option_id AND " 
            + "D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@product_option_type_id", productOptionTypeId);
                cmd.Parameters.AddWithValue("@option_id", optionId);
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
                        post = new ProductOption(reader);
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
    /// Get all product options
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <returns>A list of product option posts</returns>
    public static List<ProductOption> GetAll(Int32 languageId)
    {
        // Create the list to return
        List<ProductOption> posts = new List<ProductOption>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT P.product_option_type_id, P.option_id, P.mpn_suffix, P.price_addition, P.freight_addition, "
            + "O.product_code_suffix, D.title, 1 as selected FROM dbo.product_options AS P INNER JOIN "
            + "dbo.options AS O ON P.option_id = O.id INNER JOIN dbo.options_detail AS D ON P.option_id = D.option_id "
            + "WHERE D.language_id = @language_id ORDER BY P.product_option_type_id ASC, O.sort_order ASC;";

        // The using block is used to call dispose automatically even if there are an exception.
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
                        posts.Add(new ProductOption(reader));
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
    /// Get options based on product option type id
    /// </summary>
    /// <param name="productOptionTypeId">The product option type id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A list of product option posts</returns>
    public static List<ProductOption> GetByProductOptionTypeId(Int32 productOptionTypeId, Int32 languageId)
    {
        // Create the list to return
        List<ProductOption> posts = new List<ProductOption>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT P.product_option_type_id, P.option_id, P.mpn_suffix, P.price_addition, P.freight_addition, "
            + "O.product_code_suffix, D.title, 1 as selected FROM dbo.product_options AS P INNER JOIN "
            + "dbo.options AS O ON P.option_id = O.id INNER JOIN dbo.options_detail AS D ON P.option_id = D.option_id "
            + "WHERE P.product_option_type_id = @product_option_type_id AND "
            + "D.language_id = @language_id ORDER BY O.sort_order ASC;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@product_option_type_id", productOptionTypeId);
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
                        posts.Add(new ProductOption(reader));
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

    } // End of the GetByProductOptionTypeId method

    /// <summary>
    /// Get all options that not are included in the list of product options
    /// </summary>
    /// <param name="productOption">A list of product options</param>
    /// <param name="optionTypeId">The option type id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A list of option posts</returns>
    public static List<ProductOption> GetOtherOptions(List<ProductOption> productOptions, Int32 optionTypeId, Int32 languageId)
    {
        // Create the list to return
        List<ProductOption> posts = new List<ProductOption>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT 0 AS product_option_type_id, D.option_id, '' AS mpn_suffix, 0 AS price_addition, 0 AS freight_addition, "
            + "O.product_code_suffix, D.title, 0 AS selected FROM dbo.options_detail AS D INNER JOIN dbo.options AS O ON D.option_id "
            + "= O.id WHERE O.option_type_id = @option_type_id AND D.language_id = @language_id";

        // Do not include product options
        if (productOptions != null)
        {
            for (int i = 0; i < productOptions.Count; i++)
            {
                sql += " AND O.id != @option_id_" + i.ToString();
            }
        }

        // Add the final touch to the select string
        sql += " ORDER BY O.sort_order ASC;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@option_type_id", optionTypeId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Add parameters for product options
                if (productOptions != null)
                {
                    for (int i = 0; i < productOptions.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@option_id_" + i.ToString(), productOptions[i].option_id);
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
                        posts.Add(new ProductOption(reader));
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

    } // End of the GetOtherOptions method

    /// <summary>
    /// This recursive method will get all combinations of a product (variants)
    /// </summary>
    /// <param name="productCombinations">A list that should be filled with product combinations</param>
    /// <param name="lists">A dictionary with product option lists</param>
    /// <param name="depth">The current depth</param>
    /// <param name="current">The current product option array</param>
    public static void GetProductCombinations(List<ProductOption[]> productCombinations, Dictionary<Int32, List<ProductOption>> productOptions, Int32 depth, ProductOption[] current)
    {

        // Loop the dictionary list
        for (int i = 0; i < productOptions[depth].Count; i++)
        {
            // Get the list of product variants
            List<ProductOption> variants = productOptions[depth];

            // Get the current variant
            current[depth] = variants[i];

            // Check if we have reached the end or not
            if (depth < productOptions.Count - 1)
            {
                GetProductCombinations(productCombinations, productOptions, depth + 1, current);
            }
            else
            {
                // Add a copy of the array to the results list
                ProductOption[] resultArray = new ProductOption[current.Length];
                current.CopyTo(resultArray, 0);
                productCombinations.Add(resultArray);
            }
        }

    } // End of the GetProductCombinations method

    #endregion

    #region Delete methods
    
    /// <summary>
    /// Delete a product option post on id
    /// </summary>
    /// <param name="product_option_type_id">The product option type id</param>
    /// <param name="option_id">The option id</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 product_option_type_id, Int32 option_id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.product_options WHERE product_option_type_id = @product_option_type_id " 
            + " AND option_id = @option_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_option_type_id", product_option_type_id);
                cmd.Parameters.AddWithValue("@option_id", option_id);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
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
