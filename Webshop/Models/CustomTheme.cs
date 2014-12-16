using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a custom theme
/// </summary>
public class CustomTheme
{
    #region Variables

    public static string virtualThemeHash = "";

    public Int32 id;
    public string name;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new custom theme with default properties
    /// </summary>
    public CustomTheme()
    {
        // Set values for instance variables
        this.id = 0;
        this.name = "";

    } // End of the constructor

    /// <summary>
    /// Create a new custom theme from a reader
    /// </summary>
    /// <param name="reader"></param>
    public CustomTheme(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.name = reader["name"].ToString();

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one custom theme
    /// </summary>
    /// <param name="post">A reference to a custom theme post</param>
    public static long Add(CustomTheme post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.custom_themes (name) VALUES (@name);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@name", post.name);

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

    /// <summary>
    /// Add custom theme templates
    /// </summary>
    /// <param name="id"></param>
    public static void AddCustomThemeTemplates(Int32 id)
    {
        // Get all the templates
        Dictionary<string, string> templates = CustomThemeTemplate.GetAllByCustomThemeId(id, "user_file_name", "ASC");
        
        // Add templates that does not exist
        if (templates.ContainsKey("front_category_menu.cshtml") == false) // 1
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_category_menu.cshtml", "/Views/shared_front/_category_menu.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_category_menu.cshtml"), "Creates the category menu with leveling."));
        }
        if (templates.ContainsKey("front_display_items.cshtml") == false) // 2
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_display_items.cshtml", "/Views/shared_front/_display_items.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_display_items.cshtml"), "Creates the list of categories and products to show on the home page and the category page."));
        }
        if (templates.ContainsKey("front_mobile_layout.cshtml") == false) // 3
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_mobile_layout.cshtml", "/Views/shared_front/_mobile_layout.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_mobile_layout.cshtml"), "Mobile layout for a mobile phone browser."));
        }
        if (templates.ContainsKey("front_paging_menu.cshtml") == false) // 4
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_paging_menu.cshtml", "/Views/shared_front/_paging_menu.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_paging_menu.cshtml"), "Creates the paging menu that shows under listed items in many files."));
        }
        if (templates.ContainsKey("front_product_accessories.cshtml") == false) // 5
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_product_accessories.cshtml", "/Views/shared_front/_product_accessories.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_product_accessories.cshtml"), "List of product accessories under a product."));
        }
        if (templates.ContainsKey("front_product_reviews.cshtml") == false) // 6
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_product_reviews.cshtml", "/Views/shared_front/_product_reviews.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_product_reviews.cshtml"), "List of reviews under a product."));
        }
        if (templates.ContainsKey("front_shared_scripts.cshtml") == false) // 7
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_shared_scripts.cshtml", "/Views/shared_front/_shared_scripts.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_shared_scripts.cshtml"), "Common scripts for google analytics, google plus, facebook and more."));
        }
        if(templates.ContainsKey("front_standard_layout.cshtml") == false) // 8
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_standard_layout.cshtml", "/Views/shared_front/_standard_layout.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/shared_front/_standard_layout.cshtml"), "Standard layout file for a normal browser."));
        }
        if (templates.ContainsKey("customer_menu.cshtml") == false) // 9
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "customer_menu.cshtml", "/Views/customer/_menu.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/_menu.cshtml"), "A menu for a signed in customer"));
        }
        if (templates.ContainsKey("customer_download_files.cshtml") == false) // 10
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "customer_download_files.cshtml", "/Views/customer/download_files.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/download_files.cshtml"), "A list with downloadable files that a customer has ordered, these files can be downloaded by the customer."));
        }
        if (templates.ContainsKey("edit_company.cshtml") == false) // 11
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "edit_company.cshtml", "/Views/customer/edit_company.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/edit_company.cshtml"), "The form where a company customer can edit his information."));
        }
        if (templates.ContainsKey("edit_person.cshtml") == false) // 12
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "edit_person.cshtml", "/Views/customer/edit_person.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/edit_person.cshtml"), "The form where a private person customer can edit his information."));
        }
        if (templates.ContainsKey("edit_customer_reviews.cshtml") == false) // 13
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "edit_customer_reviews.cshtml", "/Views/customer/edit_reviews.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/edit_reviews.cshtml"), "A list form where a customer can edit his reviews."));
        }
        if (templates.ContainsKey("forgot_email_password.cshtml") == false) // 14
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "forgot_email_password.cshtml", "/Views/customer/forgot_email_password.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/forgot_email_password.cshtml"), "A form where a customer can get his email or his password."));
        }
        if (templates.ContainsKey("customer_start_page.cshtml") == false) // 15
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "customer_start_page.cshtml", "/Views/customer/index.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/index.cshtml"), "The start page for the customer."));
        }
        if (templates.ContainsKey("customer_login.cshtml") == false) // 16
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "customer_login.cshtml", "/Views/customer/login.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/customer/login.cshtml"), "The form where a customer can log in to his account."));
        }
        if (templates.ContainsKey("category.cshtml") == false) // 17
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "category.cshtml", "/Views/home/category.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/category.cshtml"), "Displays a category with its categories and products."));
        }
        if (templates.ContainsKey("contact_us.cshtml") == false) // 18
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "contact_us.cshtml", "/Views/home/contact_us.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/contact_us.cshtml"), "A contact us form."));
        }
        if (templates.ContainsKey("error.cshtml") == false) // 19
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "error.cshtml", "/Views/home/error.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/error.cshtml"), "The page that shows error messages."));
        }
        if (templates.ContainsKey("home.cshtml") == false) // 20
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "home.cshtml", "/Views/home/index.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/index.cshtml"), "The entry page for the webshop."));
        }
        if (templates.ContainsKey("information.cshtml") == false) // 21
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "information.cshtml", "/Views/home/information.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/information.cshtml"), "Displays the content of a static page."));
        }
        if (templates.ContainsKey("product.cshtml") == false) // 22
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "product.cshtml", "/Views/home/product.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/product.cshtml"), "Displays a product with information and buy button."));
        }
        if (templates.ContainsKey("search.cshtml") == false) // 23
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "search.cshtml", "/Views/home/search.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/search.cshtml"), "Displays a list of products according to a search."));
        }
        if (templates.ContainsKey("terms_of_purchase.cshtml") == false) // 24
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "terms_of_purchase.cshtml", "/Views/home/terms_of_purchase.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/home/terms_of_purchase.cshtml"), "The terms of purchase page."));
        }
        if (templates.ContainsKey("order_confirmation_body.cshtml") == false) // 25
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "order_confirmation_body.cshtml", "/Views/order/_confirmation_body.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/order/_confirmation_body.cshtml"), "The order confirmation printable area, used to view, print and email the order confirmation."));
        }
        if (templates.ContainsKey("order_confirmation.cshtml") == false) // 26
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "order_confirmation.cshtml", "/Views/order/confirmation.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/order/confirmation.cshtml"), "The order confirmation wrapper file where the customer can view and print the order confirmation."));
        }
        if (templates.ContainsKey("checkout.cshtml") == false) // 27
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "checkout.cshtml", "/Views/order/index.cshtml",
                CustomThemeTemplate.GetMasterFileContent("/Views/order/index.cshtml"), "The checkout template, where the customer makes the order."));
        }
        if (templates.ContainsKey("front_default_style.css") == false) // 28
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_default_style.css", "/Content/annytab_css/front_default_style.css",
                CustomThemeTemplate.GetMasterFileContent("/Content/annytab_css/front_default_style.css"), "Css styling for the standard and the mobile layout."));
        }
        if (templates.ContainsKey("front_mobile_layout.css") == false) // 29
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_mobile_layout.css", "/Content/annytab_css/front_mobile_layout.css",
                CustomThemeTemplate.GetMasterFileContent("/Content/annytab_css/front_mobile_layout.css"), "Mobile layout css styling for the mobile layout file."));
        }
        if (templates.ContainsKey("front_standard_layout.css") == false) // 30
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "front_standard_layout.css", "/Content/annytab_css/front_standard_layout.css",
                CustomThemeTemplate.GetMasterFileContent("/Content/annytab_css/front_standard_layout.css"), "Standard layout css styling for the standard layout file."));
        }
        if (templates.ContainsKey("rateit.css") == false) // 31
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "rateit.css", "/Content/annytab_css/rateit.css",
                CustomThemeTemplate.GetMasterFileContent("/Content/annytab_css/rateit.css"), "The style for the rating component in product reviews."));
        }
        if (templates.ContainsKey("annytab.category-functions.js") == false) // 32
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "annytab.category-functions.js", "/Scripts/annytab_front/annytab.category-functions.js",
                CustomThemeTemplate.GetMasterFileContent("/Scripts/annytab_front/annytab.category-functions.js"), "Functions that apply to the category page, where a category is displayed."));
        }
        if (templates.ContainsKey("annytab.default-functions.js") == false) // 33
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "annytab.default-functions.js", "/Scripts/annytab_front/annytab.annytab.default-functions.js",
                CustomThemeTemplate.GetMasterFileContent("/Scripts/annytab_front/annytab.default-functions.js"), "Functions that apply to the entire website, both standard and mobile."));
        }
        if (templates.ContainsKey("annytab.home-functions.js") == false) // 34
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "annytab.home-functions.js", "/Scripts/annytab_front/annytab.home-functions.js",
                CustomThemeTemplate.GetMasterFileContent("/Scripts/annytab_front/annytab.home-functions.js"), "Functions that apply to the index page of the website, the home page."));
        }
        if (templates.ContainsKey("annytab.mobile-layout-functions.js") == false) // 35
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "annytab.mobile-layout-functions.js", "/Scripts/annytab_front/annytab.mobile-layout-functions.js",
                CustomThemeTemplate.GetMasterFileContent("/Scripts/annytab_front/annytab.mobile-layout-functions.js"), "Functions that apply to the mobile layout for the website."));
        }
        if (templates.ContainsKey("annytab.order-functions.js") == false) // 36
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "annytab.order-functions.js", "/Scripts/annytab_front/annytab.order-functions.js",
                CustomThemeTemplate.GetMasterFileContent("/Scripts/annytab_front/annytab.order-functions.js"), "Functions that apply to the checkout page."));
        }
        if (templates.ContainsKey("annytab.product-functions.js") == false) // 37
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "annytab.product-functions.js", "/Scripts/annytab_front/annytab.product-functions.js",
                CustomThemeTemplate.GetMasterFileContent("/Scripts/annytab_front/annytab.product-functions.js"), "Functions that apply to the product page, some functions relate to the recalculation of the product price."));
        }
        if (templates.ContainsKey("annytab.standard-layout-functions.js") == false) // 38
        {
            CustomThemeTemplate.Add(new CustomThemeTemplate(id, "annytab.standard-layout-functions.js", "/Scripts/annytab_front/annytab.standard-layout-functions.js",
                CustomThemeTemplate.GetMasterFileContent("/Scripts/annytab_front/annytab.standard-layout-functions.js"), "Functions that apply to the standard layout for the website."));
        }
        
    } // End of the AddCustomThemeTemplates method

    /// <summary>
    /// Add a template
    /// </summary>
    /// <param name="template">A reference to a template</param>
    public static void AddTemplate(CustomThemeTemplate template)
    {
        // Add the template
        CustomThemeTemplate.Add(template);

        // Remove the cache
        RemoveThemeCache(template.custom_theme_id);

    } // End of the AddTemplate method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a custom theme post
    /// </summary>
    /// <param name="post">A reference to a custom theme post</param>
    public static void Update(CustomTheme post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.custom_themes SET name = @name WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@name", post.name);

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
    /// Update a custom theme template
    /// </summary>
    /// <param name="template">A reference to a custom theme template</param>
    public static void UpdateTemplate(CustomThemeTemplate template)
    {
        // Update the the template
        CustomThemeTemplate.Update(template);

        // Remove the cache
        RemoveThemeCache(template.custom_theme_id);

    } // End of the UpdateTemplate method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of custom themes by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <returns>The number of custom themes as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.custom_themes WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
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
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.custom_themes WHERE id = @id;";

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
    /// Get one custom theme based on id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A reference to a custom theme post</returns>
    public static CustomTheme GetOneById(Int32 id)
    {
        // Create the post to return
        CustomTheme post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.custom_themes WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
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
                        post = new CustomTheme(reader);
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
    /// Get all custom themes
    /// </summary>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of custom theme posts</returns>
    public static List<CustomTheme> GetAll(string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<CustomTheme> posts = new List<CustomTheme>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.custom_themes ORDER BY " + sortField + " " + sortOrder + ";";

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

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(new CustomTheme(reader));
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
    /// Get custom themes that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The sort field</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of custom themes</returns>
    public static List<CustomTheme> GetBySearch(string[] keywords, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<CustomTheme> posts = new List<CustomTheme>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.custom_themes WHERE 1 = 1";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the select string
        sql += " ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception
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
                // avoid having our application to crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Fill the reader with data from the select command
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(new CustomTheme(reader));
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

    /// <summary>
    /// Get all the custom theme templates by custom theme id
    /// </summary>
    /// <param name="customThemeId">The custom theme id</param>
    /// <returns>A dictionary with custom theme templates</returns>
    public static Dictionary<string, string> GetAllTemplatesById(Int32 customThemeId)
    {
        // Get the templates from cache
        Dictionary<string, string> templates = CustomThemeTemplate.GetAllByCustomThemeId(customThemeId, "user_file_name", "ASC");

        // Return the templates
        return templates;

    } // End of the GetAllTemplatesById method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a custom theme and all the templates for this theme
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Get all the domains
        List<Domain> domains = Domain.GetAll("id", "ASC");

        // Loop the domains
        for (int i = 0; i < domains.Count; i++)
        {
            // Check if the theme is used by the domain
            if (domains[i].custom_theme_id == id)
            {
                return 5;
            }
        }

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.custom_themes WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);

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

    /// <summary>
    /// Delete all the templates for a custom theme id
    /// </summary>
    /// <param name="customThemeId"></param>
    public static void DeleteTemplatesOnId(Int32 customThemeId)
    {
        // Delete templates
        CustomThemeTemplate.DeleteOnId(customThemeId);

        // Remove the cache
        RemoveThemeCache(customThemeId);

    } // End of the DeleteTemplatesOnId method

    /// <summary>
    /// Delete a template on id
    /// </summary>
    /// <param name="customThemeId">The custom theme id</param>
    /// <param name="userFileName">The file name</param>
    public static void DeleteTemplateOnId(Int32 customThemeId, string userFileName)
    {
        // Delete the template
        CustomThemeTemplate.DeleteOnId(customThemeId, userFileName);

        // Remove the theme cache
        RemoveThemeCache(customThemeId);

    } // End of the DeleteTemplateOnId method

    #endregion

    #region Cache

    /// <summary>
    /// Remove the cache
    /// </summary>
    /// <param name="customThemeId">The custom theme id</param>
    public static void RemoveThemeCache(Int32 customThemeId)
    {
        // Create the theme id
        string themeId = "Theme_" + customThemeId.ToString();

        // Get the virtual path provider
        AnnytabPathProvider provider = (AnnytabPathProvider)System.Web.Hosting.HostingEnvironment.VirtualPathProvider;

        // Remove the theme
        provider.virtualThemes.Remove(themeId);

    } // End of the RemoveThemeCache method

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
        if (sortField != "id" && sortField != "name")
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