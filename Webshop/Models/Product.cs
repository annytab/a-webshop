using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// This class represent a product for a specific language
/// </summary>
public class Product
{
    #region Variables

    public Int32 id;
    public string product_code;
    public string manufacturer_code;
    public string gtin;
    public decimal unit_price;
    public decimal unit_freight;
    public Int32 unit_id;
    public decimal mount_time_hours;
    public bool from_price;
    public Int32 category_id;
    public string brand;
    public string supplier_erp_id;
    public string meta_robots;
    public Int32 page_views;
    public decimal buys;
    public Int32 added_in_basket;
    public string condition;
    public string variant_image_filename;
    public string gender;
    public string age_group;
    public bool adult_only;
    public decimal unit_pricing_measure;
    public Int32 unit_pricing_base_measure;
    public Int32 comparison_unit_id;
    public string energy_efficiency_class;
    public bool downloadable_files;
    public DateTime date_added;
    public decimal discount;
    public string title;
    public string main_content;
    public string extra_content;
    public string meta_description;
    public string meta_keywords;
    public string page_name;
    public string delivery_time;
    public string affiliate_link;
    public decimal rating;
    public decimal toll_freight_addition;
    public Int32 value_added_tax_id;
    public string account_code;
    public string google_category;
    public bool use_local_images;
    public bool use_local_files;
    public string availability_status;
    public DateTime availability_date;
    public string size_type;
    public string size_system;
    public bool inactive;
    
    #endregion

    #region Constructors

    /// <summary>
    /// Create a new product with default properties
    /// </summary>
    public Product()
    {
        // Set values for instance variables
        this.id = 0;
        this.product_code = "";
        this.manufacturer_code = "";
        this.gtin = "";
        this.unit_price = 0;
        this.unit_freight = 0;
        this.unit_id = 0;
        this.mount_time_hours = 0;
        this.from_price = false;
        this.category_id = 0;
        this.brand = "";
        this.supplier_erp_id = "";
        this.meta_robots = "";
        this.page_views = 0;
        this.buys = 0;
        this.added_in_basket = 0;
        this.condition = "";
        this.variant_image_filename = "";
        this.gender = "";
        this.age_group = "";
        this.adult_only = false;
        this.unit_pricing_measure = 0;
        this.unit_pricing_base_measure = 0;
        this.comparison_unit_id = 0;
        this.energy_efficiency_class = "";
        this.downloadable_files = false;
        this.date_added = DateTime.UtcNow;
        this.discount = 0;
        this.title = "";
        this.main_content = "";
        this.extra_content = "";
        this.meta_description = "";
        this.meta_keywords = "";
        this.page_name = "";
        this.delivery_time = "";
        this.affiliate_link = "";
        this.rating = 0;
        this.toll_freight_addition = 0;
        this.value_added_tax_id = 0;
        this.account_code = "";
        this.google_category = "";
        this.use_local_images = false;
        this.use_local_files = false;
        this.availability_status = "";
        this.availability_date = new DateTime(2000, 1, 1);
        this.size_type = "";
        this.size_system = "";
        this.inactive = false;

    } // End of the constructor

    /// <summary>
    /// Create a new product from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public Product(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.product_code = reader["product_code"].ToString();
        this.manufacturer_code = reader["manufacturer_code"].ToString();
        this.gtin = reader["gtin"].ToString();
        this.unit_price = Convert.ToDecimal(reader["unit_price"]);
        this.unit_freight = Convert.ToDecimal(reader["unit_freight"]);
        this.unit_id = Convert.ToInt32(reader["unit_id"]);
        this.mount_time_hours = Convert.ToDecimal(reader["mount_time_hours"]);
        this.from_price = Convert.ToBoolean(reader["from_price"]);
        this.category_id = Convert.ToInt32(reader["category_id"]);
        this.brand = reader["brand"].ToString();
        this.supplier_erp_id = reader["supplier_erp_id"].ToString();
        this.meta_robots = reader["meta_robots"].ToString();
        this.page_views = Convert.ToInt32(reader["page_views"]);
        this.buys = Convert.ToDecimal(reader["buys"]);
        this.added_in_basket = Convert.ToInt32(reader["added_in_basket"]);
        this.condition = reader["condition"].ToString();
        this.variant_image_filename = reader["variant_image_filename"].ToString();
        this.gender = reader["gender"].ToString();
        this.age_group = reader["age_group"].ToString();
        this.adult_only = Convert.ToBoolean(reader["adult_only"]);
        this.unit_pricing_measure = Convert.ToDecimal(reader["unit_pricing_measure"]);
        this.unit_pricing_base_measure = Convert.ToInt32(reader["unit_pricing_base_measure"]);
        this.comparison_unit_id = Convert.ToInt32(reader["comparison_unit_id"]);
        this.energy_efficiency_class = reader["energy_efficiency_class"].ToString();
        this.downloadable_files = Convert.ToBoolean(reader["downloadable_files"]);
        this.date_added = Convert.ToDateTime(reader["date_added"]);
        this.discount = Convert.ToDecimal(reader["discount"]);
        this.title = reader["title"].ToString();
        this.main_content = reader["main_content"].ToString();
        this.extra_content = reader["extra_content"].ToString();
        this.meta_description = reader["meta_description"].ToString();
        this.meta_keywords = reader["meta_keywords"].ToString();
        this.page_name = reader["page_name"].ToString();
        this.delivery_time = reader["delivery_time"].ToString();
        this.affiliate_link = reader["affiliate_link"].ToString();
        this.rating = Convert.ToDecimal(reader["rating"]);
        this.toll_freight_addition = Convert.ToDecimal(reader["toll_freight_addition"]);
        this.value_added_tax_id = Convert.ToInt32(reader["value_added_tax_id"]);
        this.account_code = reader["account_code"].ToString();
        this.google_category = reader["google_category"].ToString();
        this.use_local_images = Convert.ToBoolean(reader["use_local_images"]);
        this.use_local_files = Convert.ToBoolean(reader["use_local_files"]);
        this.availability_status = reader["availability_status"].ToString();
        this.availability_date = Convert.ToDateTime(reader["availability_date"]);
        this.size_type = reader["size_type"].ToString();
        this.size_system = reader["size_system"].ToString();
        this.inactive = Convert.ToBoolean(reader["inactive"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one master product post
    /// </summary>
    /// <param name="post">A reference to a product post</param>
    public static long AddMasterPost(Product post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.products (product_code, manufacturer_code, gtin, unit_price, unit_freight, unit_id, "
            + "mount_time_hours, from_price, category_id, brand, supplier_erp_id, meta_robots, page_views, buys, added_in_basket, condition, "
            + "variant_image_filename, gender, age_group, adult_only, unit_pricing_measure, unit_pricing_base_measure, comparison_unit_id, " 
            + "energy_efficiency_class, downloadable_files, date_added, discount) "
            + "VALUES (@product_code, @manufacturer_code, @gtin, @unit_price, @unit_freight, @unit_id, @mount_time_hours, @from_price, "
            + "@category_id, @brand, @supplier_erp_id, @meta_robots, @page_views, @buys, @added_in_basket, "
            + "@condition, @variant_image_filename, @gender, @age_group, @adult_only, @unit_pricing_measure, @unit_pricing_base_measure, "
            + "@comparison_unit_id, @energy_efficiency_class, @downloadable_files, @date_added, @discount);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_code", post.product_code);
                cmd.Parameters.AddWithValue("@manufacturer_code", post.manufacturer_code);
                cmd.Parameters.AddWithValue("@gtin", post.gtin);
                cmd.Parameters.AddWithValue("@unit_price", post.unit_price);
                cmd.Parameters.AddWithValue("@unit_freight", post.unit_freight);
                cmd.Parameters.AddWithValue("@unit_id", post.unit_id);
                cmd.Parameters.AddWithValue("@mount_time_hours", post.mount_time_hours);
                cmd.Parameters.AddWithValue("@from_price", post.from_price);
                cmd.Parameters.AddWithValue("@category_id", post.category_id);
                cmd.Parameters.AddWithValue("@brand", post.brand);
                cmd.Parameters.AddWithValue("@supplier_erp_id", post.supplier_erp_id);
                cmd.Parameters.AddWithValue("@meta_robots", post.meta_robots);
                cmd.Parameters.AddWithValue("@page_views", post.page_views);
                cmd.Parameters.AddWithValue("@buys", post.buys);
                cmd.Parameters.AddWithValue("@added_in_basket", post.added_in_basket);
                cmd.Parameters.AddWithValue("@condition", post.condition);
                cmd.Parameters.AddWithValue("@variant_image_filename", post.variant_image_filename);
                cmd.Parameters.AddWithValue("@gender", post.gender);
                cmd.Parameters.AddWithValue("@age_group", post.age_group);
                cmd.Parameters.AddWithValue("@adult_only", post.adult_only);
                cmd.Parameters.AddWithValue("@unit_pricing_measure", post.unit_pricing_measure);
                cmd.Parameters.AddWithValue("@unit_pricing_base_measure", post.unit_pricing_base_measure);
                cmd.Parameters.AddWithValue("@comparison_unit_id", post.comparison_unit_id);
                cmd.Parameters.AddWithValue("@energy_efficiency_class", post.energy_efficiency_class);
                cmd.Parameters.AddWithValue("@downloadable_files", post.downloadable_files);
                cmd.Parameters.AddWithValue("@date_added", post.date_added);
                cmd.Parameters.AddWithValue("@discount", post.discount);

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

    } // End of the AddMasterPost method

    /// <summary>
    /// Add one language product post
    /// </summary>
    /// <param name="post">A reference to a product</param>
    /// <param name="languageId">The id of the language</param>
    public static void AddLanguagePost(Product post, int languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.products_detail (product_id, language_id, title, main_content, extra_content, meta_description, "
            + "meta_keywords, page_name, delivery_time, affiliate_link, rating, toll_freight_addition, "
            + "value_added_tax_id, account_code, google_category, use_local_images, use_local_files, availability_status, "
            + "availability_date, size_type, size_system, inactive) "
            + "VALUES (@product_id, @language_id, @title, @main_content, @extra_content, @meta_description, @meta_keywords, "
            + "@page_name, @delivery_time, @affiliate_link, @rating, @toll_freight_addition, "
            + "@value_added_tax_id, @account_code, @google_category, @use_local_images, @use_local_files, @availability_status, "
            + "@availability_date, @size_type, @size_system, @inactive);";

        // The using block is used to call dispose automatically even if there are is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@title", post.title);
                cmd.Parameters.AddWithValue("@main_content", post.main_content);
                cmd.Parameters.AddWithValue("@extra_content", post.extra_content);
                cmd.Parameters.AddWithValue("@meta_description", post.meta_description);
                cmd.Parameters.AddWithValue("@meta_keywords", post.meta_keywords);
                cmd.Parameters.AddWithValue("@page_name", post.page_name);
                cmd.Parameters.AddWithValue("@delivery_time", post.delivery_time);
                cmd.Parameters.AddWithValue("@affiliate_link", post.affiliate_link);
                cmd.Parameters.AddWithValue("@rating", post.rating);
                cmd.Parameters.AddWithValue("@toll_freight_addition", post.toll_freight_addition);
                cmd.Parameters.AddWithValue("@value_added_tax_id", post.value_added_tax_id);
                cmd.Parameters.AddWithValue("@account_code", post.account_code);
                cmd.Parameters.AddWithValue("@google_category", post.google_category);
                cmd.Parameters.AddWithValue("@use_local_images", post.use_local_images);
                cmd.Parameters.AddWithValue("@use_local_files", post.use_local_files);
                cmd.Parameters.AddWithValue("@availability_status", post.availability_status);
                cmd.Parameters.AddWithValue("@availability_date", post.availability_date);
                cmd.Parameters.AddWithValue("@size_type", post.size_type);
                cmd.Parameters.AddWithValue("@size_system", post.size_system);
                cmd.Parameters.AddWithValue("@inactive", post.inactive);

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

    } // End of the AddLanguagePost method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a master product post
    /// </summary>
    /// <param name="post">A reference to a product post</param>
    public static void UpdateMasterPost(Product post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET product_code = @product_code, manufacturer_code = @manufacturer_code, "
            + "gtin = @gtin, unit_price = @unit_price, unit_freight = @unit_freight, unit_id = @unit_id, "
            + "mount_time_hours = @mount_time_hours, from_price = @from_price, category_id = @category_id, brand = @brand, "
            + "supplier_erp_id = @supplier_erp_id, meta_robots = @meta_robots, page_views = @page_views, buys = @buys, "
            + "added_in_basket = @added_in_basket, condition = @condition, variant_image_filename = @variant_image_filename, "
            + "gender = @gender, age_group = @age_group, adult_only = @adult_only, unit_pricing_measure = @unit_pricing_measure, "
            + "unit_pricing_base_measure = @unit_pricing_base_measure, comparison_unit_id = @comparison_unit_id, "
            + "energy_efficiency_class = @energy_efficiency_class, downloadable_files = @downloadable_files, " 
            + "date_added = @date_added, discount = @discount WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@product_code", post.product_code);
                cmd.Parameters.AddWithValue("@manufacturer_code", post.manufacturer_code);
                cmd.Parameters.AddWithValue("@gtin", post.gtin);
                cmd.Parameters.AddWithValue("@unit_price", post.unit_price);
                cmd.Parameters.AddWithValue("@unit_freight", post.unit_freight);
                cmd.Parameters.AddWithValue("@unit_id", post.unit_id);
                cmd.Parameters.AddWithValue("@mount_time_hours", post.mount_time_hours);
                cmd.Parameters.AddWithValue("@from_price", post.from_price);
                cmd.Parameters.AddWithValue("@category_id", post.category_id);
                cmd.Parameters.AddWithValue("@brand", post.brand);
                cmd.Parameters.AddWithValue("@supplier_erp_id", post.supplier_erp_id);
                cmd.Parameters.AddWithValue("@meta_robots", post.meta_robots);
                cmd.Parameters.AddWithValue("@page_views", post.page_views);
                cmd.Parameters.AddWithValue("@buys", post.buys);
                cmd.Parameters.AddWithValue("@added_in_basket", post.added_in_basket);
                cmd.Parameters.AddWithValue("@condition", post.condition);
                cmd.Parameters.AddWithValue("@variant_image_filename", post.variant_image_filename);
                cmd.Parameters.AddWithValue("@gender", post.gender);
                cmd.Parameters.AddWithValue("@age_group", post.age_group);
                cmd.Parameters.AddWithValue("@adult_only", post.adult_only);
                cmd.Parameters.AddWithValue("@unit_pricing_measure", post.unit_pricing_measure);
                cmd.Parameters.AddWithValue("@unit_pricing_base_measure", post.unit_pricing_base_measure);
                cmd.Parameters.AddWithValue("@comparison_unit_id", post.comparison_unit_id);
                cmd.Parameters.AddWithValue("@energy_efficiency_class", post.energy_efficiency_class);
                cmd.Parameters.AddWithValue("@downloadable_files", post.downloadable_files);
                cmd.Parameters.AddWithValue("@date_added", post.date_added);
                cmd.Parameters.AddWithValue("@discount", post.discount);

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

    } // End of the UpdateMasterPost method

    /// <summary>
    /// Update a language product post
    /// </summary>
    /// <param name="post">A reference to a language product post</param>
    /// <param name="languageId">The id of the language</param>
    public static void UpdateLanguagePost(Product post, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products_detail SET title = @title, main_content = @main_content, extra_content = @extra_content, meta_description = @meta_description, "
            + "meta_keywords = @meta_keywords, page_name = @page_name, delivery_time = @delivery_time, affiliate_link = @affiliate_link, rating = @rating, "
            + "toll_freight_addition = @toll_freight_addition, value_added_tax_id = @value_added_tax_id, account_code = @account_code, "
            + "google_category = @google_category, use_local_images = @use_local_images, use_local_files = @use_local_files, availability_status = @availability_status, "
            + "availability_date = @availability_date, size_type = @size_type, size_system = @size_system, inactive = @inactive " 
            + "WHERE product_id = @product_id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@title", post.title);
                cmd.Parameters.AddWithValue("@main_content", post.main_content);
                cmd.Parameters.AddWithValue("@extra_content", post.extra_content);
                cmd.Parameters.AddWithValue("@meta_description", post.meta_description);
                cmd.Parameters.AddWithValue("@meta_keywords", post.meta_keywords);
                cmd.Parameters.AddWithValue("@page_name", post.page_name);
                cmd.Parameters.AddWithValue("@delivery_time", post.delivery_time);
                cmd.Parameters.AddWithValue("@affiliate_link", post.affiliate_link);
                cmd.Parameters.AddWithValue("@rating", post.rating);
                cmd.Parameters.AddWithValue("@toll_freight_addition", post.toll_freight_addition);
                cmd.Parameters.AddWithValue("@value_added_tax_id", post.value_added_tax_id);
                cmd.Parameters.AddWithValue("@account_code", post.account_code);
                cmd.Parameters.AddWithValue("@google_category", post.google_category);
                cmd.Parameters.AddWithValue("@use_local_images", post.use_local_images);
                cmd.Parameters.AddWithValue("@use_local_files", post.use_local_files);
                cmd.Parameters.AddWithValue("@availability_status", post.availability_status);
                cmd.Parameters.AddWithValue("@availability_date", post.availability_date);
                cmd.Parameters.AddWithValue("@size_type", post.size_type);
                cmd.Parameters.AddWithValue("@size_system", post.size_system);
                cmd.Parameters.AddWithValue("@inactive", post.inactive);

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

    } // End of the UpdateLanguagePost method

    /// <summary>
    /// Update page views for the product
    /// </summary>
    /// <param name="id">The id for the product</param>
    /// <param name="pageViews">The total number of page views</param>
    public static void UpdatePageviews(Int32 id, Int32 pageViews)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET page_views = @page_views WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@page_views", pageViews);

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

    } // End of the UpdatePageviews method

    /// <summary>
    /// Update added in basket for the product
    /// </summary>
    /// <param name="id">The product id</param>
    /// <param name="quantity">The new quantity</param>
    public static void UpdateAddedInBasket(Int32 id, Int32 quantity)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET added_in_basket = @added_in_basket WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@added_in_basket", quantity);

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

    } // End of the UpdateAddedInBasket method

    /// <summary>
    /// Update buys for the product
    /// </summary>
    /// <param name="id">The product id</param>
    /// <param name="quantity">The quantity bought</param>
    public static void UpdateBuys(Int32 id, decimal quantity)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET buys = @buys WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@buys", quantity);

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

    } // End of the UpdateBuys method

    /// <summary>
    /// Reset statistics for all products
    /// </summary>
    public static void ResetStatistics()
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET page_views = @page_views, added_in_basket = @added_in_basket, buys = @buys;";

        // The using block is used to call dispose automatically even if there is an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@page_views", 0);
                cmd.Parameters.AddWithValue("@added_in_basket", 0);
                cmd.Parameters.AddWithValue("@buys", 0);

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

    } // End of the ResetStatistics method

    /// <summary>
    /// Reset statistics for a product
    /// </summary>
    /// <param name="id">The product id</param>
    public static void ResetStatistics(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET page_views = @page_views, added_in_basket = @added_in_basket, buys = @buys WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@page_views", 0);
                cmd.Parameters.AddWithValue("@added_in_basket", 0);
                cmd.Parameters.AddWithValue("@buys", 0);

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

    } // End of the ResetStatistics method

    /// <summary>
    /// Update the rating for the product
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="languageId">The language id</param>
    public static void UpdateRating(Int32 productId, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products_detail SET rating = ISNULL((SELECT (SUM(rating) / COUNT(id)) AS rating "
            + "FROM dbo.product_reviews WHERE product_id = @product_id AND language_id = @language_id),0) "
            + "WHERE product_id = @product_id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@product_id", productId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

    } // End of the UpdateRating method

    /// <summary>
    /// Update product unit prices
    /// </summary>
    /// <param name="priceMultiplier">The price multiplier as a decimal</param>
    public static void UpdateUnitPrices(decimal priceMultiplier)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET unit_price = ROUND(unit_price * @price_multiplier, 2);";

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

    } // End of the UpdateUnitPrices method

    /// <summary>
    /// Update product unit freights
    /// </summary>
    /// <param name="priceMultiplier">The price multiplier as a decimal</param>
    public static void UpdateUnitFreights(decimal priceMultiplier)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET unit_freight = ROUND(unit_freight * @price_multiplier, 2);"
            + "UPDATE dbo.products_detail SET toll_freight_addition = ROUND(toll_freight_addition * @price_multiplier, 2);";

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

    } // End of the UpdateUnitFreights method

    /// <summary>
    /// Set if the product has downloadable files or not
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="hasDownloadableFiles">A boolean that indicates if the product has downloadable files</param>
    public static void SetHasDownloadableFiles(Int32 productId, bool hasDownloadableFiles)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.products SET downloadable_files = @downloadable_files WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", productId);
                cmd.Parameters.AddWithValue("@downloadable_files", hasDownloadableFiles);

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

    } // End of the SetHasDownloadableFiles method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of products by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of products as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(P.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString()
                + " OR D.meta_description LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

    /// <summary>
    /// Count the number of active products by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of products as an int</returns>
    public static Int32 GetActiveCountBySearch(string[] keywords, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (P.brand LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString() 
                + " OR D.meta_description LIKE @keyword_" + i.ToString() + " OR D.meta_keywords LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.products WHERE id = @id;";

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
    /// Get one product based on id
    /// </summary>
    /// <param name="productId">A product id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a product post</returns>
    public static Product GetOneById(Int32 productId, Int32 languageId)
    {
        // Create the post to return
        Product post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE P.id = @id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", productId);
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
                        post = new Product(reader);
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
    /// Get one product based on page name
    /// </summary>
    /// <param name="pageName">A page name</param>
    /// <returns>A reference to a product post</returns>
    public static Product GetOneByPageName(string pageName, Int32 languageId)
    {
        // Create the post to return
        Product post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id "
            + "= P.id WHERE D.page_name = @page_name AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@page_name", pageName);
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
                        post = new Product(reader);
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

    } // End of the GetOneByPageName method

    /// <summary>
    /// Get all the products for a specific language
    /// </summary>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of products</returns>
    public static List<Product> GetAll(Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Product> posts = new List<Product>(10);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE D.language_id = @language_id ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

                    // Loop through the reader as long as there is something to read.
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

    } // End of the GetAll method

    /// <summary>
    /// Get all the active products for a specific language
    /// </summary>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of products</returns>
    public static List<Product> GetAllActive(Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Product> posts = new List<Product>(10);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0 ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

                    // Loop through the reader as long as there is something to read.
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

    } // End of the GetAllActive method

    /// <summary>
    /// Get all active and unique products, all possible combinations
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <returns>A list with products</returns>
    public static List<Product> GetAllActiveUniqueProducts(Int32 languageId)
    {
        // Get all the products
        List<Product> products = Product.GetAllActive(languageId, "id", "ASC");

        // Create the list to return
        List<Product> uniqueProducts = new List<Product>(products.Count);

        // Loop all the products
        for (int i = 0; i < products.Count; i++)
        {
            // Get all the product options
            List<ProductOptionType> productOptionTypes = ProductOptionType.GetByProductId(products[i].id, languageId);

            // Check if the product has product options
            if (productOptionTypes.Count > 0)
            {
                // Get all the product options
                Dictionary<Int32, List<ProductOption>> productOptions = new Dictionary<Int32, List<ProductOption>>(productOptionTypes.Count);

                // Loop all the product option types
                for (int j = 0; j < productOptionTypes.Count; j++)
                {
                    List<ProductOption> listProductOptions = ProductOption.GetByProductOptionTypeId(productOptionTypes[j].id, languageId);

                    // Make sure that the list contains options
                    if(listProductOptions.Count > 0)
                    {
                        productOptions.Add(j, ProductOption.GetByProductOptionTypeId(productOptionTypes[j].id, languageId));
                    }
                }

                // Get all the product combinations
                List<ProductOption[]> productCombinations = new List<ProductOption[]>();
                ProductOption.GetProductCombinations(productCombinations, productOptions, 0, new ProductOption[productOptions.Count]);

                // Loop all the product combinations
                foreach (ProductOption[] optionArray in productCombinations)
                {
                    // Get a product copy
                    Product productCopy = products[i].Clone();

                    // Loop all the product options in the array
                    Int32 optionCounter = 0;
                    foreach (ProductOption option in optionArray)
                    {
                        // Adjust product values
                        productCopy.product_code += option.product_code_suffix;
                        productCopy.manufacturer_code += option.mpn_suffix;
                        productCopy.title += " - " + option.title;
                        productCopy.unit_price += option.price_addition;
                        productCopy.unit_freight += option.freight_addition;
                        productCopy.variant_image_filename = productCopy.variant_image_filename.Replace("[" + optionCounter.ToString() + "]", option.product_code_suffix);

                        // Add to the option counter
                        optionCounter++;
                    }

                    // Add the unique product to the list
                    uniqueProducts.Add(productCopy);
                }
            }
            else
            {
                // Add the product to the list
                uniqueProducts.Add(products[i]);
            }
        }

        // Return the list
        return uniqueProducts;

    } // End of the GetAllActiveUniqueProducts method

    /// <summary>
    /// Get products for a specific category
    /// </summary>
    /// <param name="categoryId">The id of the category</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of product posts</returns>
    public static List<Product> GetByCategoryId(Int32 categoryId, Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Product> posts = new List<Product>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE P.category_id = @category_id AND D.language_id = @language_id "
            + "ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@category_id", categoryId);
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

    } // End of the GetByCategoryId method

    /// <summary>
    /// Get active products for a specific category
    /// </summary>
    /// <param name="categoryId">The id of the category</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of product posts</returns>
    public static List<Product> GetActiveByCategoryId(Int32 categoryId, Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Product> posts = new List<Product>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE P.category_id = @category_id AND D.language_id = @language_id AND D.inactive = 0 "
            + "ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@category_id", categoryId);
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

    } // End of the GetActiveByCategoryId method

    /// <summary>
    /// Get products that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order ASC or DESC</param>
    /// <returns>A list of products</returns>
    public static List<Product> GetBySearch(string[] keywords, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Product> posts = new List<Product>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id " 
            + "WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(P.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString()
                + " OR D.meta_description LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += " ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);
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
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
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

    } // End of the GetBySearch method

    /// <summary>
    /// Get active products that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order ASC or DESC</param>
    /// <param name="pricesIncludesVat">A boolean that indicates if prices should include vat</param>
    /// <returns>A list of products</returns>
    public static List<Product> GetActiveBySearch(string[] keywords, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder, bool pricesIncludesVat)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Product> posts = new List<Product>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0 ";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += "AND (P.brand LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString()
                + " OR D.meta_description LIKE @keyword_" + i.ToString() + " OR D.meta_keywords LIKE @keyword_" + i.ToString() + ") ";
        }

        // Add the final touch to the sql string
        sql += "ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // Sort with prices that includes vat
        if (sortField.ToLower() == "unit_price" && pricesIncludesVat == true)
        {
            sql = "SELECT *, (P.unit_price * (1 - P.discount) * (1 + V.value)) AS unit_price_with_vat FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "INNER JOIN dbo.value_added_taxes AS V ON D.value_added_tax_id = V.id WHERE D.language_id = @language_id ";

            // Append keywords to the sql string
            for (int i = 0; i < keywords.Length; i++)
            {
                sql += "AND (P.brand LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString()
                    + " OR D.meta_description LIKE @keyword_" + i.ToString() + " OR D.meta_keywords LIKE @keyword_" + i.ToString() + ") ";
            }

            // Add the final touch to the sql string
            sql += "ORDER BY unit_price_with_vat " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";
        }

        // Sort with discount prices
        if (sortField.ToLower() == "unit_price" && pricesIncludesVat == false)
        {
            sql = "SELECT *, (P.unit_price * (1 - P.discount)) AS discount_price FROM dbo.products_detail AS D INNER JOIN dbo.products AS P ON D.product_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0 ";

            // Append keywords to the sql string
            for (int i = 0; i < keywords.Length; i++)
            {
                sql += "AND (P.brand LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString()
                    + " OR D.meta_description LIKE @keyword_" + i.ToString() + " OR D.meta_keywords LIKE @keyword_" + i.ToString() + ") ";
            }

            // Add the final touch to the sql string
            sql += "ORDER BY discount_price " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";
        }

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);
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
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
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

    } // End of the GetActiveBySearch method

    /// <summary>
    /// Get the file version date
    /// </summary>
    /// <param name="fullFilePath">The full path to the file</param>
    /// <returns>The file version date, DateTime.MinValue if no file is found</returns>
    public static DateTime GetFileVersionDate(string fullFilePath)
    {
        // Create the date to return
        DateTime versionDate = DateTime.MinValue;

        try
        {
            // Get the date
            versionDate = System.IO.File.GetLastWriteTime(fullFilePath);
        }
        catch(Exception ex)
        {
            string exMessage = ex.Message;
        }
        
        // Return the version date
        return versionDate;

    } // End of the GetFileVersionDate method

    /// <summary>
    /// Create a new copy of the product
    /// </summary>
    /// <returns>A new product clone</returns>
    public Product Clone()
    {
        return (Product)this.MemberwiseClone();

    } //End of the 

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a product post on id
    /// </summary>
    /// <param name="id">The id number for the product</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.products WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
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

    /// <summary>
    /// Delete a language product post on id
    /// </summary>
    /// <param name="id">The id number for the product post</param>
    /// <param name="languageId">The language id</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteLanguagePostOnId(Int32 id, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.products_detail WHERE product_id = @id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

    } // End of the DeleteLanguagePostOnId method

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
        if (sortField != "id" && sortField != "unit_price" && sortField != "title" && sortField != "page_views"
            && sortField != "rating" && sortField != "buys" && sortField != "added_in_basket"
            && sortField != "date_added" && sortField != "inactive")
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