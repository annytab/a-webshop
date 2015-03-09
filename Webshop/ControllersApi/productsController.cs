using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for products
    /// </summary>
    public class productsController : ApiController
    {
        #region Insert methods

        // Add a product
        // POST api/products/add?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Product post, Int32 languageId = 0)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(languageId) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (Category.MasterPostExists(post.category_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The category does not exist");
            }
            else if (Unit.MasterPostExists(post.unit_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The unit does not exist");
            }
            else if (ValueAddedTax.MasterPostExists(post.value_added_tax_id) == false)
            { 
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The value added tax does not exist");
            }

            // Make sure that the data is valid
            post.product_code = AnnytabDataValidation.TruncateString(post.product_code, 20);
            post.manufacturer_code = AnnytabDataValidation.TruncateString(post.manufacturer_code, 20);
            post.gtin = AnnytabDataValidation.TruncateString(post.gtin, 20);
            post.unit_price = AnnytabDataValidation.TruncateDecimal(post.unit_price, 0, 9999999999.99M);
            post.unit_freight = AnnytabDataValidation.TruncateDecimal(post.unit_freight, 0, 9999999999.99M);
            post.mount_time_hours = AnnytabDataValidation.TruncateDecimal(post.mount_time_hours, 0, 9999.99M);
            post.brand = AnnytabDataValidation.TruncateString(post.brand, 50);
            post.supplier_erp_id = AnnytabDataValidation.TruncateString(post.supplier_erp_id, 20);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.buys = AnnytabDataValidation.TruncateDecimal(post.buys, 0, 9999999999.99M);
            post.condition = AnnytabDataValidation.TruncateString(post.condition, 20);
            post.variant_image_filename = AnnytabDataValidation.TruncateString(post.variant_image_filename, 50);
            post.availability_status = AnnytabDataValidation.TruncateString(post.availability_status, 50);
            post.availability_date = AnnytabDataValidation.TruncateDateTime(post.availability_date);
            post.gender = AnnytabDataValidation.TruncateString(post.gender, 20);
            post.age_group = AnnytabDataValidation.TruncateString(post.age_group, 20);
            post.title = AnnytabDataValidation.TruncateString(post.title, 200);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);
            post.delivery_time = AnnytabDataValidation.TruncateString(post.delivery_time, 50);
            post.affiliate_link = AnnytabDataValidation.TruncateString(post.affiliate_link, 100);
            post.rating = AnnytabDataValidation.TruncateDecimal(post.rating, 0, 999999.99M);
            post.toll_freight_addition = AnnytabDataValidation.TruncateDecimal(post.toll_freight_addition, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);
            post.google_category = AnnytabDataValidation.TruncateString(post.google_category, 300);
            post.unit_pricing_measure = AnnytabDataValidation.TruncateDecimal(post.unit_pricing_measure, 0, 99999.99999M);
            post.size_type = AnnytabDataValidation.TruncateString(post.size_type, 20);
            post.size_system = AnnytabDataValidation.TruncateString(post.size_system, 10);
            post.energy_efficiency_class = AnnytabDataValidation.TruncateString(post.energy_efficiency_class, 10);
            post.date_added = AnnytabDataValidation.TruncateDateTime(post.date_added);
            post.discount = AnnytabDataValidation.TruncateDecimal(post.discount, 0, 9.999M);

            // Get a product on page name
            Product productOnPageName = Product.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (productOnPageName != null && post.id != productOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Add the post
            Int64 insertId = Product.AddMasterPost(post);
            post.id = Convert.ToInt32(insertId);
            Product.AddLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a product
        // PUT api/products/update?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Product post, Int32 languageId = 0)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(languageId) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (Category.MasterPostExists(post.category_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The category does not exist");
            }
            else if (Unit.MasterPostExists(post.unit_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The unit does not exist");
            }
            else if (ValueAddedTax.MasterPostExists(post.value_added_tax_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The value added tax does not exist");
            }

            // Make sure that the data is valid
            post.product_code = AnnytabDataValidation.TruncateString(post.product_code, 20);
            post.manufacturer_code = AnnytabDataValidation.TruncateString(post.manufacturer_code, 20);
            post.gtin = AnnytabDataValidation.TruncateString(post.gtin, 20);
            post.unit_price = AnnytabDataValidation.TruncateDecimal(post.unit_price, 0, 9999999999.99M);
            post.unit_freight = AnnytabDataValidation.TruncateDecimal(post.unit_freight, 0, 9999999999.99M);
            post.mount_time_hours = AnnytabDataValidation.TruncateDecimal(post.mount_time_hours, 0, 9999.99M);
            post.brand = AnnytabDataValidation.TruncateString(post.brand, 50);
            post.supplier_erp_id = AnnytabDataValidation.TruncateString(post.supplier_erp_id, 20);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.buys = AnnytabDataValidation.TruncateDecimal(post.buys, 0, 9999999999.99M);
            post.condition = AnnytabDataValidation.TruncateString(post.condition, 20);
            post.variant_image_filename = AnnytabDataValidation.TruncateString(post.variant_image_filename, 50);
            post.availability_status = AnnytabDataValidation.TruncateString(post.availability_status, 50);
            post.availability_date = AnnytabDataValidation.TruncateDateTime(post.availability_date);
            post.gender = AnnytabDataValidation.TruncateString(post.gender, 20);
            post.age_group = AnnytabDataValidation.TruncateString(post.age_group, 20);
            post.title = AnnytabDataValidation.TruncateString(post.title, 200);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);
            post.delivery_time = AnnytabDataValidation.TruncateString(post.delivery_time, 50);
            post.affiliate_link = AnnytabDataValidation.TruncateString(post.affiliate_link, 100);
            post.rating = AnnytabDataValidation.TruncateDecimal(post.rating, 0, 999999.99M);
            post.toll_freight_addition = AnnytabDataValidation.TruncateDecimal(post.toll_freight_addition, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);
            post.google_category = AnnytabDataValidation.TruncateString(post.google_category, 300);
            post.unit_pricing_measure = AnnytabDataValidation.TruncateDecimal(post.unit_pricing_measure, 0, 99999.99999M);
            post.size_type = AnnytabDataValidation.TruncateString(post.size_type, 20);
            post.size_system = AnnytabDataValidation.TruncateString(post.size_system, 10);
            post.energy_efficiency_class = AnnytabDataValidation.TruncateString(post.energy_efficiency_class, 10);
            post.date_added = AnnytabDataValidation.TruncateDateTime(post.date_added);
            post.discount = AnnytabDataValidation.TruncateDecimal(post.discount, 0, 9.999M);

            // Get the saved post
            Product savedPost = Product.GetOneById(post.id, languageId);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Get a product on page name
            Product productOnPageName = Product.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (productOnPageName != null && post.id != productOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Update the post
            Product.UpdateMasterPost(post);
            Product.UpdateLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a product
        // PUT api/products/translate?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(Product post, Int32 languageId = 0)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(languageId) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if(Product.MasterPostExists(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product does not exist");
            }
            else if (ValueAddedTax.MasterPostExists(post.value_added_tax_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The value added tax does not exist");
            }

            // Make sure that the data is valid
            post.title = AnnytabDataValidation.TruncateString(post.title, 200);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);
            post.delivery_time = AnnytabDataValidation.TruncateString(post.delivery_time, 50);
            post.affiliate_link = AnnytabDataValidation.TruncateString(post.affiliate_link, 100);
            post.rating = AnnytabDataValidation.TruncateDecimal(post.rating, 0, 999999.99M);
            post.toll_freight_addition = AnnytabDataValidation.TruncateDecimal(post.toll_freight_addition, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);
            post.google_category = AnnytabDataValidation.TruncateString(post.google_category, 300);
            post.availability_status = AnnytabDataValidation.TruncateString(post.availability_status, 50);
            post.availability_date = AnnytabDataValidation.TruncateDateTime(post.availability_date);

            // Get a product on page name
            Product productOnPageName = Product.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (productOnPageName != null && post.id != productOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Get the post
            Product savedPost = Product.GetOneById(post.id, languageId);

            // Check if we should add or update the post
            if (savedPost == null)
            {
                Product.AddLanguagePost(post, languageId);
            }
            else
            {
                Product.UpdateLanguagePost(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translate was successful");

        } // End of the translate method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/products/get_count_by_search?keywords=one%20two&languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Int32 get_count_by_search(string keywords = "", Int32 languageId = 0)
        {
            // Create the string array
            string[] wordArray = new string[] { "" };

            // Recreate the array if keywords is different from null
            if (keywords != null)
            {
                wordArray = keywords.Split(' ');
            }

            // Get the count
            Int32 count = Product.GetCountBySearch(wordArray, languageId);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a product by id
        // GET api/products/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Product get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            Product post = Product.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all products
        // GET api/products/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Product> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Product> posts = Product.GetAll(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get all active products
        // GET api/products/get_all_active?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Product> get_all_active(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Product> posts = Product.GetAllActive(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all_active method

        // Get all the unique products (all product combinations)
        // GET api/products/get_all_active_and_unique?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Product> get_all_active_and_unique(Int32 languageId = 0)
        {
            // Create the list to return
            List<Product> posts = Product.GetAllActiveUniqueProducts(languageId);

            // Return the list
            return posts;

        } // End of the get_all_active_and_unique method

        // Get products by category id
        // GET api/products/get_by_category_id/1?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Product> get_by_category_id(Int32 id = 0, Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Product> posts = Product.GetByCategoryId(id, languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_category_id method

        // Get active products by category id
        // GET api/products/get_active_by_category_id/1?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Product> get_active_by_category_id(Int32 id = 0, Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Product> posts = Product.GetActiveByCategoryId(id, languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_active_by_category_id method

        // Get all image urls
        // GET api/products/get_all_images?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Dictionary<Int32, List<string>> get_all_images(Int32 languageId = 0)
        {
            // Create the dictionary to return
            Dictionary<Int32, List<string>> productUrls = new Dictionary<Int32, List<string>>();

            // Get all the products
            List<Product> posts = Product.GetAll(languageId, "id", "ASC");

            // Loop all the products
            for (int i = 0; i < posts.Count; i++)
            {
                // Create a list
                List<string> urls = new List<string>();

                // Add the main image
                urls.Add(Tools.GetProductMainImageUrl(posts[i].id, languageId, "", posts[i].use_local_images));

                // Add other images
                urls.AddRange(Tools.GetOtherProductImageUrls(posts[i].id, languageId, posts[i].use_local_images));

                // Add the list to the dictionary
                productUrls.Add(posts[i].id, urls);
            }

            // Return the dictionary
            return productUrls;

        } // End of the get_all_images method

        // Get image urls for a product
        // GET api/products/get_images_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<string> get_images_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the list to return
            List<string> urls = new List<string>();

            // Get the post
            Product post = Product.GetOneById(id, languageId);

            // Add the main image
            urls.Add(Tools.GetProductMainImageUrl(id, languageId, "", post.use_local_images));

            // Add other images
            urls.AddRange(Tools.GetOtherProductImageUrls(id, languageId, post.use_local_images));

            // Return the post
            return urls;

        } // End of the get_images_by_id method

        // Get the file version date for a file
        // GET api/products/get_file_version_date/5?languageId=1&fileName=some_name.jpg
        [HttpGet]
        public DateTime get_file_version_date(Int32 id = 0, Int32 languageId = 0, string fileName = "")
        {
            // Create the file path
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/Content/products/" + (id / 100).ToString() + "/" + id.ToString() + "/" 
                + languageId.ToString() + "/dc_files/" + fileName);

            // Get the version date
            DateTime versionDate = Product.GetFileVersionDate(filePath);

            // Return the date
            return versionDate;

        } // End of the get_file_version_date method

        // Get a file by downloading it
        // GET api/products/get_file/5?languageId=1&fileName=some_name.jpg
        [ApiCustomerAuthorize]
        [HttpGet]
        public HttpResponseMessage get_file(Int32 id = 0, Int32 languageId = 0, string fileName = "")      
        {

            // Get the authorization token
            string authToken = Request.Headers.Authorization.Parameter;
            string decodedToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
            
            // Get the email and password
            string email = decodedToken.Substring(0, decodedToken.IndexOf(":"));
            string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

            // Get the language
            if(Language.GetOneById(languageId) == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }

            // Get the customer
            Customer customer = Customer.GetOneByEmail(email);

            // Check if the customer exists and if the password is correct
            if (customer == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The customer does not exist");
            }

            // Get the product
            Product product = Product.GetOneById(id, languageId);

            // Check if the product exist
            if(product == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product does not exist");
            }

            // Get the customer file
            CustomerFile customerFile = CustomerFile.GetOneById(customer.id, product.id, customer.language_id);

            // Check if the customer file exists
            if(customerFile == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The customer are not eligible to download the file");
            }

            // Create the language id
            string strLanguageId = product.use_local_files == true ? customer.language_id.ToString() : "0";

            // Create the file path
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/Content/products/" + (id / 100).ToString() + "/" + id.ToString() + "/"
                + strLanguageId + "/dc_files/" + fileName);

            // Return the file if it exists
            if(File.Exists(filePath) == true)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                FileStream stream = new FileStream(filePath, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            else
            {
                // The file does not exist
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The file does not exist");
            }
            
        } // End of the get_file method

        // Get posts by a search
        // GET api/products/get_by_search?keywords=one%20two&languageId=1&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Product> get_by_search(string keywords = "", Int32 languageId = 0, Int32 pageSize = 0, Int32 pageNumber = 0,
            string sortField = "", string sortOrder = "")
        {
            // Create the string array
            string[] wordArray = new string[] { "" };

            // Recreate the array if keywords is different from null
            if (keywords != null)
            {
                wordArray = keywords.Split(' ');
            }

            // Create the list to return
            List<Product> posts = Product.GetBySearch(wordArray, languageId, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a product
        // DELETE api/products/delete/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = Product.DeleteOnId(id);
            }
            else
            {
                errorCode = Product.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if (errorCode != 0)
            {
                return Request.CreateResponse<string>(HttpStatusCode.Conflict, "Foreign key constraint");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete method

        #endregion

        #region File methods

        // Upload a main image
        // POST api/products/upload_main_image/5?languageId=0
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public async Task<HttpResponseMessage> upload_main_image(Int32 id = 0, Int32 languageId = 0)
        {
            // Verify that this is an HTML Form file upload request
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.UnsupportedMediaType, "The image was not uploaded");
            }

            // Create the directory path
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath("/Content/products/" + (id / 100).ToString() + "/" + id.ToString() + "/" + languageId.ToString() + "/");

            // Check if the directory exists
            if (System.IO.Directory.Exists(directoryPath) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(directoryPath);
            }

            // Create a stream provider
            AnnytabMultipartFormDataStreamProvider streamProvider = new AnnytabMultipartFormDataStreamProvider(directoryPath, "main_image.jpg");

            // Read the content and upload files
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The image has been uploaded");

        } // End of the upload_main_image method

        // Upload other images
        // POST api/products/upload_other_images/5?languageId=0
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public async Task<HttpResponseMessage> upload_other_images(Int32 id = 0, Int32 languageId = 0)
        {
            // Verify that this is an HTML Form file upload request
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.UnsupportedMediaType, "Images was not uploaded");
            }

            // Create the directory path
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath("/Content/products/" + (id / 100).ToString() + "/" + id.ToString() + "/" + languageId.ToString() + "/other_images/");

            // Check if the directory exists
            if (System.IO.Directory.Exists(directoryPath) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(directoryPath);
            }

            // Create a stream provider
            AnnytabMultipartFormDataStreamProvider streamProvider = new AnnytabMultipartFormDataStreamProvider(directoryPath, "");

            // Read the content and upload files
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "Images has been uploaded");

        } // End of the upload_other_images method

        // Delete images for a product id
        // DELETE api/products/delete_images_by_id/1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_images_by_id(Int32 id = 0)
        {
            // Define the directory url for product images
            string productImageDirectory = System.Web.HttpContext.Current.Server.MapPath("/Content/products/" + (id / 100).ToString() + "/" + id.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(productImageDirectory))
            {
                System.IO.Directory.Delete(productImageDirectory, true);
            }
            else
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The directory does not exist");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_images_by_id method

        // Delete images for a product id and a language id
        // DELETE api/products/delete_images_by_id_and_language/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_images_by_id_and_language(Int32 id = 0, Int32 languageId = 0)
        {
            // Define the directory url for product images
            string productImageDirectory = System.Web.HttpContext.Current.Server.MapPath("/Content/products/" + (id / 100).ToString() + "/" + id.ToString() + "/" + languageId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(productImageDirectory))
            {
                System.IO.Directory.Delete(productImageDirectory, true);
            }
            else
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The directory does not exist");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_images_by_id_and_language method

        #endregion

    } // End of the class

} // End of the namespace
