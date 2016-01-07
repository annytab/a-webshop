using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// This class represent a gift card that has been used in an order
/// </summary>
public class OrderGiftCard
{
    #region Variables

    public Int32 order_id;
    public string gift_card_id;
    public decimal amount;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post with default properties
    /// </summary>
    public OrderGiftCard()
    {
        // Set values for instance variables
        this.order_id = 0;
        this.gift_card_id = "";
        this.amount = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new post from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public OrderGiftCard(SqlDataReader reader)
    {
        // Set values for instance variables
        this.order_id = Convert.ToInt32(reader["order_id"]);
        this.gift_card_id = reader["gift_card_id"].ToString();
        this.amount = Convert.ToDecimal(reader["amount"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one order gift card
    /// </summary>
    /// <param name="post">A reference to a order gift card post</param>
    public static void Add(OrderGiftCard post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.orders_gift_cards (order_id, gift_card_id, amount) "
            + "VALUES (@order_id, @gift_card_id, @amount);";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", post.order_id);
                cmd.Parameters.AddWithValue("@gift_card_id", post.gift_card_id);
                cmd.Parameters.AddWithValue("@amount", post.amount);

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
    /// Update a order gift card post
    /// </summary>
    /// <param name="post">A reference to a order gift card post</param>
    public static void Update(OrderGiftCard post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.orders_gift_cards SET amount = @amount WHERE order_id = @order_id AND gift_card_id = @gift_card_id;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", post.order_id);
                cmd.Parameters.AddWithValue("@gift_card_id", post.gift_card_id);
                cmd.Parameters.AddWithValue("@amount", post.amount);

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
    /// Get one order gift card on id
    /// </summary>
    /// <param name="orderId">An order id</param>
    /// <param name="giftCardId">A gift card id</param>
    /// <returns>A reference to a order gift card post</returns>
    public static OrderGiftCard GetOneById(Int32 orderId, string giftCardId)
    {
        // Create the post to return
        OrderGiftCard post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders_gift_cards WHERE order_id = @order_id AND gift_card_id = @gift_card_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@gift_card_id", giftCardId);

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
                        post = new OrderGiftCard(reader);
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
    /// Get order gift cards by order id
    /// </summary>
    /// <param name="orderId">The id of the order</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of order gift card posts</returns>
    public static List<OrderGiftCard> GetByOrderId(Int32 orderId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<OrderGiftCard> posts = new List<OrderGiftCard>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders_gift_cards WHERE order_id = @order_id ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", orderId);

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
                        posts.Add(new OrderGiftCard(reader));
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

    } // End of the GetByOrderId method

    /// <summary>
    /// Get order gift cards by gift card id
    /// </summary>
    /// <param name="giftCardId">The id of the gift card</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of order gift card posts</returns>
    public static List<OrderGiftCard> GetByGiftCardId(string giftCardId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<OrderGiftCard> posts = new List<OrderGiftCard>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders_gift_cards WHERE gift_card_id = @gift_card_id ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@gift_card_id", giftCardId);

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
                        posts.Add(new OrderGiftCard(reader));
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

    } // End of the GetByGiftCardId method

    /// <summary>
    /// Get all order gift cards
    /// </summary>
    /// <returns>A list of order gift card posts</returns>
    public static List<OrderGiftCard> GetAll()
    {
        // Create the list to return
        List<OrderGiftCard> posts = new List<OrderGiftCard>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.orders_gift_cards ORDER BY order_id ASC, amount ASC;";

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
                        posts.Add(new OrderGiftCard(reader));
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

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a order gift card post on id
    /// </summary>
    /// <param name="orderId">The id for an order</param>
    /// <param name="giftCardId">The id for a gift card</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 orderId, string giftCardId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.orders_gift_cards WHERE order_id = @order_id AND gift_card_id = @gift_card_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@gift_card_id", giftCardId);

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

    #region Validation

    /// <summary>
    /// Get a valid sort field
    /// </summary>
    /// <param name="sortField">The sort field</param>
    /// <returns>A valid sort field as a string</returns>
    public static string GetValidSortField(string sortField)
    {
        // Make sure that the sort field is valid
        if (sortField != "order_id" && sortField != "gift_card_id" && sortField != "amount")
        {
            sortField = "order_id";
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