using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for categories
    /// </summary>
    public class categoriesController : ApiController
    {
        #region Insert methods

        // Add a category
        // POST api/categories/add?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Category post, Int32 languageId = 0)
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

            // Make sure that the data is valid
            post.title = AnnytabDataValidation.TruncateString(post.title, 100);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);
            post.date_added = AnnytabDataValidation.TruncateDateTime(post.date_added);

            // Get a category on page name
            Category categoryOnPageName = Category.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (categoryOnPageName != null && post.id != categoryOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Add the post
            Int64 insertId = Category.AddMasterPost(post);
            post.id = Convert.ToInt32(insertId);
            Category.AddLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a category
        // PUT api/categories/update?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Category post, Int32 languageId = 0)
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

            // Make sure that the data is valid
            post.title = AnnytabDataValidation.TruncateString(post.title, 100);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);
            post.date_added = AnnytabDataValidation.TruncateDateTime(post.date_added);

            // Get the saved post
            Category savedPost = Category.GetOneById(post.id, languageId);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Get a category on page name
            Category categoryOnPageName = Category.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (categoryOnPageName != null && post.id != categoryOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Update the post
            Category.UpdateMasterPost(post);
            Category.UpdateLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a category
        // PUT api/categories/translate?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(Category post, Int32 languageId = 0)
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
            else if (Category.MasterPostExists(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The category does not exist");
            }

            // Make sure that the data is valid
            post.title = AnnytabDataValidation.TruncateString(post.title, 100);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);

            // Get a category on page name
            Category categoryOnPageName = Category.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (categoryOnPageName != null && post.id != categoryOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Get the post
            Category savedPost = Category.GetOneById(post.id, languageId);

            // Check if we should add or update the post
            if (savedPost == null)
            {
                Category.AddLanguagePost(post, languageId);
            }
            else
            {
                Category.UpdateLanguagePost(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translate was successful");

        } // End of the translate method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/categories/get_count_by_search?keywords=one%20two&languageId=1
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
            Int32 count = Category.GetCountBySearch(wordArray, languageId);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a category by id
        // GET api/categories/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Category get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            Category post = Category.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all categories
        // GET api/categories/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Category> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Category> posts = Category.GetAll(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get all active categories
        // GET api/categories/get_all_active?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Category> get_all_active(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Category> posts = Category.GetAllActive(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all_active method

        // Get child categories
        // GET api/categories/get_child_categories/0?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Category> get_child_categories(Int32 id = 0, Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Category> posts = Category.GetChildCategories(id, languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_child_categories method

        // Get active child categories
        // GET api/categories/get_active_child_categories/0?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Category> get_active_child_categories(Int32 id = 0, Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Category> posts = Category.GetActiveChildCategories(id, languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_active_child_categories method

        // Get image urls for a category
        // GET api/categories/get_images_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<string> get_images_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the list to return
            List<string> urls = new List<string>();

            // Get the category
            Category post = Category.GetOneById(id, languageId);

            // Add the main image
            urls.Add(Tools.GetCategoryMainImageUrl(id, languageId, post.use_local_images));

            // Add environment images
            urls.AddRange(Tools.GetEnvironmentImageUrls(id, languageId, post.use_local_images));

            // Return the post
            return urls;

        } // End of the get_images_by_id method

        // Get all image urls
        // GET api/categories/get_all_images?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Dictionary<Int32, List<string>> get_all_images(Int32 languageId = 0)
        {
            // Create the dictionary to return
            Dictionary<Int32, List<string>> categoryUrls = new Dictionary<Int32, List<string>>();

            // Get all the categories
            List<Category> posts = Category.GetAll(languageId, "id", "ASC");

            // Loop all the categories
            for (int i = 0; i < posts.Count; i++)
            {
                // Create a list
                List<string> urls = new List<string>();

                // Add the main image
                urls.Add(Tools.GetCategoryMainImageUrl(posts[i].id, languageId, posts[i].use_local_images));

                // Add environment images
                urls.AddRange(Tools.GetEnvironmentImageUrls(posts[i].id, languageId, posts[i].use_local_images));

                // Add the list to the dictionary
                categoryUrls.Add(posts[i].id, urls);
            }

            // Return the dictionary
            return categoryUrls;

        } // End of the get_all_images method

        // Get posts by a search
        // GET api/categories/get_by_search?keywords=one%20two&languageId=1&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Category> get_by_search(string keywords = "", Int32 languageId = 0, Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Category> posts = Category.GetBySearch(wordArray, languageId, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a category
        // DELETE api/categories/delete/0?languageId=0
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = Category.DeleteOnId(id);
            }
            else
            {
                errorCode = Category.DeleteLanguagePostOnId(id, languageId);
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
        // POST api/categories/upload_main_image/5?languageId=0
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
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath("/Content/categories/" + (id / 100).ToString() + "/" + id.ToString() + "/" + languageId.ToString() + "/");

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

        // Upload environment images
        // POST api/categories/upload_environment_images/5?languageId=0
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public async Task<HttpResponseMessage> upload_environment_images(Int32 id = 0, Int32 languageId = 0)
        {
            // Verify that this is an HTML Form file upload request
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.UnsupportedMediaType, "Images was not uploaded");
            }

            // Create the directory path
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath("/Content/categories/" + (id / 100).ToString() + "/" + id.ToString() + "/" + languageId.ToString() + "/environment_images/");

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

        } // End of the upload_environment_images method

        // Delete images for a category id
        // DELETE api/categories/delete_images_by_id/1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_images_by_id(Int32 id = 0)
        {
            // Define the directory url for category images
            string categoryImageDirectory = System.Web.HttpContext.Current.Server.MapPath("/Content/categories/" + (id / 100).ToString() + "/" + id.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(categoryImageDirectory))
            {
                System.IO.Directory.Delete(categoryImageDirectory, true);
            }
            else
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The directory does not exist");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_images_by_id method

        // Delete images for a category id and a language id
        // DELETE api/categories/delete_images_by_id_and_language/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_images_by_id_and_language(Int32 id = 0, Int32 languageId = 0)
        {
            // Define the directory url for category images
            string categoryImageDirectory = System.Web.HttpContext.Current.Server.MapPath("/Content/categories/" + (id / 100).ToString() + "/" + id.ToString() + "/" + languageId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(categoryImageDirectory))
            {
                System.IO.Directory.Delete(categoryImageDirectory, true);
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
