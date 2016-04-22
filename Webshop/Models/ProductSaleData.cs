using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// This class represent sale statistics for a product
/// </summary>
public class ProductSaleData
{
    #region Variables

    public string product_code;
    public string product_name;
    public decimal quantity;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new product sale statistics with default properties
    /// </summary>
    public ProductSaleData()
    {
        // Set values for instance variables
        this.product_code = "";
        this.product_name = "";
        this.quantity = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new product sale statitistics post from a reader
    /// </summary>
    /// <param name="reader"></param>
    public ProductSaleData(SqlDataReader reader)
    {
        // Set values for instance variables
        this.product_code = reader["product_code"].ToString();
        this.product_name = reader["product_name"].ToString();
        this.quantity = Convert.ToDecimal(reader["quantity"]);

    } // End of the constructor

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number products sold in the year
    /// </summary>
    /// <returns>The number of product posts as an int</returns>
    public static Int32 GetCountByYear(string countryCode, string year)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT R.product_code FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year GROUP BY R.product_code) AS count;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);

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

    } // End of the GetCountByYear method

    /// <summary>
    /// Count the number products sold in the month
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of product posts as an int</returns>
    public static Int32 GetCountByMonth(string countryCode, string year, string month)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT R.product_code FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year AND MONTH(O.order_date) = @month GROUP BY R.product_code) AS count;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);

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

    } // End of the GetCountByMonth method

    /// <summary>
    /// Count the number products sold in the week
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of product posts as an int</returns>
    public static Int32 GetCountByWeek(string countryCode, string year, string week)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT R.product_code FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year AND DATEPART(week, O.order_date) = @week GROUP BY R.product_code) AS count;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@week", week);

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

    } // End of the GetCountByWeek method

    /// <summary>
    /// Count the number products sold in the day
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <returns>The number of product posts as an int</returns>
    public static Int32 GetCountByDay(string countryCode, string year, string day)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT R.product_code FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year AND DATEPART(dayofyear, O.order_date) = @day GROUP BY R.product_code) AS count;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@day", day);

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

    } // End of the GetCountByDay method

    #endregion

    #region Get methods

    /// <summary>
    /// Get product sale statistics for a year
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of product sale data posts</returns>
    public static List<ProductSaleData> GetByYear(string countryCode, string year, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<ProductSaleData> posts = new List<ProductSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT R.product_code, R.product_name, SUM(R.quantity) as quantity "
            + "FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year GROUP BY R.product_code, R.product_name ORDER BY quantity DESC "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
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
                        posts.Add(new ProductSaleData(reader));
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
    /// Get product sale statistics for a month
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="month">The month as a string (1 to 12)</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of product sale data posts</returns>
    public static List<ProductSaleData> GetByMonth(string countryCode, string year, string month, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<ProductSaleData> posts = new List<ProductSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT R.product_code, R.product_name, SUM(R.quantity) as quantity "
            + "FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year AND MONTH(O.order_date) = @month GROUP BY R.product_code, R.product_name ORDER BY quantity DESC "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
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
                        posts.Add(new ProductSaleData(reader));
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
    /// Get product sale statistics for a week
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="week">The week as a string (1 to 52)</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of product sale data posts</returns>
    public static List<ProductSaleData> GetByWeek(string countryCode, string year, string week, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<ProductSaleData> posts = new List<ProductSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT R.product_code, R.product_name, SUM(R.quantity) as quantity "
            + "FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year AND DATEPART(week, O.order_date) = @week GROUP BY R.product_code, R.product_name ORDER BY quantity DESC "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@week", week);
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
                        posts.Add(new ProductSaleData(reader));
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
    /// Get product sale statistics for a day
    /// </summary>
    /// <param name="countryCode">The country code</param>
    /// <param name="year">The year as a 4 digit string</param>
    /// <param name="day">The day as a string (1 to 366)</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <returns>A list of product sale data posts</returns>
    public static List<ProductSaleData> GetByDay(string countryCode, string year, string day, Int32 pageSize, Int32 pageNumber)
    {
        // Create the list to return
        List<ProductSaleData> posts = new List<ProductSaleData>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT R.product_code, R.product_name, SUM(R.quantity) as quantity "
            + "FROM dbo.order_rows AS R INNER JOIN dbo.orders AS O ON R.order_id = O.id "
            + "AND O.document_type = @document_type ";

        // Filter by country code
        if (countryCode != "")
        {
            sql += "AND O.country_code = @country_code ";
        }

        sql += "AND YEAR(O.order_date) = @year AND DATEPART(dayofyear, O.order_date) = @day GROUP BY R.product_code, R.product_name ORDER BY quantity DESC "
            + "OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@document_type", 1);
                cmd.Parameters.AddWithValue("@country_code", countryCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@day", day);
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
                        posts.Add(new ProductSaleData(reader));
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
