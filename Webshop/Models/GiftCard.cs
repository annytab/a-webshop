using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a gift card
/// </summary>
[Serializable]
public class GiftCard
{
    #region Variables

    public string id;
    public Int32 language_id;
    public string currency_code;
    public decimal amount;
    public DateTime end_date;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new gift card with default properties
    /// </summary>
    public GiftCard()
    {
        // Set values for instance variables
        this.id = "";
        this.language_id = 0;
        this.currency_code = "";
        this.amount = 0;
        this.end_date = DateTime.UtcNow;

    } // End of the constructor

    /// <summary>
    /// Create a new gift card from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public GiftCard(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = reader["id"].ToString();
        this.language_id = Convert.ToInt32(reader["language_id"]);
        this.currency_code = reader["currency_code"].ToString();
        this.amount = Convert.ToDecimal(reader["amount"]);
        this.end_date = Convert.ToDateTime(reader["end_date"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one gift card
    /// </summary>
    /// <param name="post">A reference to a gift card post</param>
    public static void Add(GiftCard post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.gift_cards (id, language_id, currency_code, amount, end_date) "
            + "VALUES (@id, @language_id, @currency_code, @amount, @end_date);";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@language_id", post.language_id);
                cmd.Parameters.AddWithValue("@currency_code", post.currency_code);
                cmd.Parameters.AddWithValue("@amount", post.amount);
                cmd.Parameters.AddWithValue("@end_date", post.end_date);

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
    /// Update a gift card
    /// </summary>
    /// <param name="post">A reference to a gift card post</param>
    public static void Update(GiftCard post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.gift_cards SET language_id = @language_id, currency_code = @currency_code, amount = @amount, "
            + "end_date = @end_date WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@language_id", post.language_id);
                cmd.Parameters.AddWithValue("@currency_code", post.currency_code);
                cmd.Parameters.AddWithValue("@amount", post.amount);
                cmd.Parameters.AddWithValue("@end_date", post.end_date);

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
    /// Count the number of gift cards by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <returns>The number of gift cards as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.gift_cards WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (id LIKE @keyword_" + i.ToString() + " OR CAST(language_id AS nvarchar(20)) LIKE @keyword_" + i.ToString()
                + " OR currency_code LIKE @keyword_" + i.ToString() + ")";
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
    /// <param name="id">The id for the gift card</param>
    /// <returns>A boolean that indicates if the post exists</returns>
    public static bool MasterPostExists(string id)
    {
        // Create the boolean to return
        bool postExists = false;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.gift_cards WHERE id = @id;";

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
    /// Get one gift card based on id
    /// </summary>
    /// <param name="id">The id for the post</param>
    /// <returns>A reference to a gift card post</returns>
    public static GiftCard GetOneById(string id)
    {
        // Create the post to return
        GiftCard post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.gift_cards WHERE id = @id;";

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
                        post = new GiftCard(reader);
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
    /// Get all gift cards
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of gift cards</returns>
    public static List<GiftCard> GetAll(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<GiftCard> posts = new List<GiftCard>();

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.gift_cards ORDER BY " + sortField + " " + sortOrder + ";";

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
                        posts.Add(new GiftCard(reader));
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
    /// Get gift cards by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of gift cards</returns>
    public static List<GiftCard> GetBySearch(string[] keywords, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<GiftCard> posts = new List<GiftCard>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.gift_cards WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (id LIKE @keyword_" + i.ToString() + " OR CAST(language_id AS nvarchar(20)) LIKE @keyword_" + i.ToString() 
                + " OR currency_code LIKE @keyword_" + i.ToString() + ")";
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
                        posts.Add(new GiftCard(reader));
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
    /// Delete a gift card post on id
    /// </summary>
    /// <param name="id">The id of the gift card post</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(string id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.gift_cards WHERE id = @id;";

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
        if (sortField != "id" && sortField != "language_id" && sortField != "currency_code"
            && sortField != "amount" && sortField != "end_date")
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