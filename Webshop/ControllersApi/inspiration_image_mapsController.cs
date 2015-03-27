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
    /// This class controls the api for inspiration image maps
    /// </summary>
    public class inspiration_image_mapsController : ApiController
    {
        #region Insert methods

        // Add a image map
        // POST api/inspiration_image_maps/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(InspirationImageMap post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (post.language_id != 0 && Language.MasterPostExists(post.language_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (Category.MasterPostExists(post.category_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The category does not exist");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);
            post.image_name = AnnytabDataValidation.TruncateString(post.image_name, 100);

            // Add the post
            InspirationImageMap.Add(post);;

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a image map
        // PUT api/inspiration_image_maps/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(InspirationImageMap post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (post.language_id != 0 && Language.MasterPostExists(post.language_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (Category.MasterPostExists(post.category_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The category does not exist");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);
            post.image_name = AnnytabDataValidation.TruncateString(post.image_name, 100);

            // Get the saved post
            InspirationImageMap savedPost = InspirationImageMap.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            InspirationImageMap.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/inspiration_image_maps/get_count_by_search?keywords=one%20two%20three
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Int32 get_count_by_search(string keywords = "")
        {
            // Create the string array
            string[] wordArray = new string[] { "" };

            // Recreate the array if keywords is different from null
            if (keywords != null)
            {
                wordArray = keywords.Split(' ');
            }

            // Get the count
            Int32 count = InspirationImageMap.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a image map by id
        // GET api/inspiration_image_maps/get_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public InspirationImageMap get_by_id(Int32 id = 0)
        {
            // Create the post to return
            InspirationImageMap post = InspirationImageMap.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all image maps
        // GET api/inspiration_image_maps/get_all?sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<InspirationImageMap> get_all(string sortField = "", string sortOrder = "")
        {
            // Get a list of image maps
            List<InspirationImageMap> posts = InspirationImageMap.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get image maps by category id
        // GET api/inspiration_image_maps/get_by_category_id/5?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<InspirationImageMap> get_by_category_id(Int32 id = 0, Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Get a list of image maps
            List<InspirationImageMap> posts = InspirationImageMap.GetByCategoryId(id, languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_category_id method

        // Get posts by a search
        // GET api/inspiration_image_maps/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<InspirationImageMap> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<InspirationImageMap> posts = InspirationImageMap.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        // Get the image url for a image map
        // GET api/inspiration_image_maps/get_image_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public string get_image_by_id(Int32 id = 0)
        {
            // Create the string to return
            string url = "";

            // Get the post
            InspirationImageMap post = InspirationImageMap.GetOneById(id);

            // Make sure that the post not is null
            if (post == null)
            {
                return url;
            }

            // Get the url
            url = Tools.GetInspirationImageUrl(post.id, post.image_name);

            // Return the url
            return url;

        } // End of the get_image_by_id method

        // Get all image urls
        // GET api/inspiration_image_maps/get_all_images
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Dictionary<Int32, string> get_all_images()
        {
            // Create the dictionary to return
            Dictionary<Int32, string> imageUrls = new Dictionary<Int32, string>();

            // Get all the image maps
            List<InspirationImageMap> posts = InspirationImageMap.GetAll("id", "ASC");

            // Loop all the posts
            for (int i = 0; i < posts.Count; i++)
            {
                // Add the url to the list
                imageUrls.Add(posts[i].id, Tools.GetInspirationImageUrl(posts[i].id, posts[i].image_name));
            }

            // Return the dictionary
            return imageUrls;

        } // End of the get_all_images method

        #endregion

        #region Delete methods

        // Delete a image map
        // DELETE api/inspiration_image_maps/delete/1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = InspirationImageMap.DeleteOnId(id);

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

        // Upload a inspiration image
        // POST api/inspiration_image_maps/upload_image/5
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public async Task<HttpResponseMessage> upload_image(Int32 id = 0)
        {
            // Verify that this is an HTML Form file upload request
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.UnsupportedMediaType, "The image was not uploaded");
            }

            // Create the directory path
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath(Tools.GetInspirationImageDirectoryUrl(id));

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
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The image has been uploaded");

        } // End of the upload_image method

        // Delete a inspiration image
        // DELETE api/inspiration_image_maps/delete_image/2
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_image(Int32 id)
        {
            // Get the directory
            string directoryUrl = System.Web.HttpContext.Current.Server.MapPath("/Content/inspiration/" + (id / 100).ToString() + "/" + id.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(directoryUrl))
            {
                System.IO.Directory.Delete(directoryUrl, true);
            }
            else
            {
                return Request.CreateResponse<string>(HttpStatusCode.OK, "The image did not exist");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_image method

        #endregion

    } // End of the class

} // End of the namespace
