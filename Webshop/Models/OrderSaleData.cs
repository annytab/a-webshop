using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent order sale data
/// </summary>
public class OrderSaleData
{
    #region Variables

    public string year;
    public string datepart;
    public Int32 quantity;
    public decimal net_sum;
    public decimal vat_sum;
    public decimal total_sum;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new order data post with default properties
    /// </summary>
    public OrderSaleData()
    {
        // Set values for instance variables
        this.year = "";
        this.datepart = "";
        this.quantity = 0;
        this.net_sum = 0;
        this.vat_sum = 0;
        this.total_sum = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new order data post from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public OrderSaleData(SqlDataReader reader)
    {
        // Set values for instance variables
        this.year = reader["year"].ToString();
        this.datepart = reader["datepart"].ToString();
        this.quantity = Convert.ToInt32(reader["quantity"]);
        this.net_sum = Convert.ToDecimal(reader["net_sum"]);
        this.vat_sum = Convert.ToDecimal(reader["vat_sum"]);
        this.total_sum = Convert.ToDecimal(reader["total_sum"]);

    } // End of the constructor

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of years
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of order posts as an int</returns>
    public static Int32 GetCountOfYears(string countryCode)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT YEAR(order_date) AS year FROM dbo.orders "
            + "WHERE document_type = 1 ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the group by statement
        sql += "GROUP BY YEAR(order_date)) orders;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);

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

    } // End of the GetCountOfYears method

    /// <summary>
    /// Count the number of months
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of order posts as an int</returns>
    public static Int32 GetCountOfMonths(string countryCode)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT YEAR(order_date) AS year, MONTH(order_date) as datepart FROM dbo.orders " 
            + "WHERE document_type = 1 ";

        // Filter by country code
        if(countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the group by statement
        sql  += "GROUP BY YEAR(order_date), MONTH(order_date)) orders;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);

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

    } // End of the GetCountOfMonths method

    /// <summary>
    /// Count the number of weeks
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of order posts as an int</returns>
    public static Int32 GetCountOfWeeks(string countryCode)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT YEAR(order_date) AS year, DATEPART(week, order_date) as datepart FROM dbo.orders "
            + "WHERE document_type = 1 ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the group by statement
        sql += "GROUP BY YEAR(order_date), DATEPART(week, order_date)) orders;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);

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

    } // End of the GetCountOfWeeks method

    /// <summary>
    /// Count the number of days
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of order posts as an int</returns>
    public static Int32 GetCountOfDays(string countryCode)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT YEAR(order_date) AS year, DATEPART(dayofyear, order_date) as datepart FROM dbo.orders "
            + "WHERE document_type = 1 ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the group by statement
        sql += "GROUP BY YEAR(order_date), DATEPART(dayofyear, order_date)) orders;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);

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

    } // End of the GetCountOfDays method

    #endregion

    #region Get methods

    /// <summary>
    /// Get order sale statistics grouped by year
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of order sale data posts</returns>
    public static List<OrderSaleData> GetByYear(string countryCode, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<OrderSaleData> posts = new List<OrderSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT YEAR(order_date) AS year, 0 as datepart, COUNT(id) AS quantity, "
            + "SUM(net_sum * conversion_rate) AS net_sum, SUM(vat_sum * conversion_rate) AS vat_sum, "
            + "SUM(total_sum * conversion_rate) AS total_sum FROM dbo.orders WHERE document_type = 1 ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the final touch to the sql string
        sql += "GROUP BY YEAR(order_date) ORDER BY YEAR(order_date) DESC OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                        posts.Add(new OrderSaleData(reader));
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

    } // End of the GetByYear method


    /// <summary>
    /// Get order sale statistics grouped by month
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of order sale data posts</returns>
    public static List<OrderSaleData> GetByMonth(string countryCode, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<OrderSaleData> posts = new List<OrderSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT YEAR(order_date) AS year, MONTH(order_date) as datepart, COUNT(id) AS quantity, "
            + "SUM(net_sum * conversion_rate) AS net_sum, SUM(vat_sum * conversion_rate) AS vat_sum, "
            + "SUM(total_sum * conversion_rate) AS total_sum FROM dbo.orders WHERE document_type = 1 ";

            // Filter by country code
            if (countryCode != "")
            {
                sql += "AND country_code = @country_code ";
            }    

            // Add the final touch to the sql string
            sql += "GROUP BY YEAR(order_date), MONTH(order_date) ORDER BY YEAR(order_date), MONTH(order_date) DESC " 
                + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                        posts.Add(new OrderSaleData(reader));
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

    } // End of the GetByMonth method

    /// <summary>
    /// Get order sale statistics grouped by week
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of order sale data posts</returns>
    public static List<OrderSaleData> GetByWeek(string countryCode, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<OrderSaleData> posts = new List<OrderSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT YEAR(order_date) AS year, DATEPART(week, order_date) as datepart, COUNT(id) AS quantity, "
            + "SUM(net_sum * conversion_rate) AS net_sum, SUM(vat_sum * conversion_rate) AS vat_sum, "
            + "SUM(total_sum * conversion_rate) AS total_sum FROM dbo.orders WHERE document_type = 1 ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the final touch to the sql string
        sql += "GROUP BY YEAR(order_date), DATEPART(week, order_date) ORDER BY YEAR(order_date), DATEPART(week, order_date) DESC "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                        posts.Add(new OrderSaleData(reader));
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

    } // End of the GetByWeek method

    /// <summary>
    /// Get order sale statistics grouped by day
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of order sale data posts</returns>
    public static List<OrderSaleData> GetByDay(string countryCode, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<OrderSaleData> posts = new List<OrderSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT YEAR(order_date) AS year, DATEPART(dayofyear, order_date) as datepart, COUNT(id) AS quantity, "
            + "SUM(net_sum * conversion_rate) AS net_sum, SUM(vat_sum * conversion_rate) AS vat_sum, "
            + "SUM(total_sum * conversion_rate) AS total_sum FROM dbo.orders WHERE document_type = 1 ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND country_code = @country_code ";
        }

        // Add the final touch to the sql string
        sql += "GROUP BY YEAR(order_date), DATEPART(dayofyear, order_date) ORDER BY YEAR(order_date), DATEPART(dayofyear, order_date) DESC "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                        posts.Add(new OrderSaleData(reader));
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

    } // End of the GetByDay method

    #endregion

} // End of the class