using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for campaigns
    /// </summary>
    public class campaignsController : ApiController
    {
        #region Insert methods

        // Add a campaign
        // POST api/campaigns/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Campaign post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(post.language_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);
            post.category_name = AnnytabDataValidation.TruncateString(post.category_name, 50);
            post.image_name = AnnytabDataValidation.TruncateString(post.image_name, 100);
            post.link_url = AnnytabDataValidation.TruncateString(post.link_url, 200);

            // Add the post
            Campaign.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a campaign
        // PUT api/campaigns/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Campaign post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(post.language_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);
            post.category_name = AnnytabDataValidation.TruncateString(post.category_name, 50);
            post.image_name = AnnytabDataValidation.TruncateString(post.image_name, 100);
            post.link_url = AnnytabDataValidation.TruncateString(post.link_url, 200);

            // Get the saved post
            Campaign savedPost = Campaign.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Campaign.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/campaigns/get_count_by_search?keywords=one%20two%20three
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
            Int32 count = Campaign.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a campaign by id
        // GET api/campaigns/get_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Campaign get_by_id(Int32 id = 0)
        {
            // Create the post to return
            Campaign post = Campaign.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all campaigns, set the language id to 0 to get all posts
        // GET api/campaigns/get_all?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Campaign> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Campaign> posts = new List<Campaign>(0);

            // Get the list of campaigns
            if(languageId == 0)
            {
                posts = Campaign.GetAll(sortField, sortOrder);
            }
            else
            {
                posts = Campaign.GetByLanguageId(languageId, sortField, sortOrder);
            }

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/campaigns/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Campaign> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Campaign> posts = Campaign.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a campaign
        // DELETE api/campaigns/delete/1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = Campaign.DeleteOnId(id);

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

        // Upload campaign images
        // POST api/campaigns/upload_images
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public async Task<HttpResponseMessage> upload_images()
        {
            // Verify that this is an HTML Form file upload request
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.UnsupportedMediaType, "Images was not uploaded");
            }

            // Create the directory path
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath("/Content/campaigns/images/");

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

        } // End of the upload_images method

        // Delete a campaign image
        // DELETE api/campaigns/delete_image?name=image.jpg
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_image(string name = "")
        {
            // Check the name for invalid characters
            if(AnnytabDataValidation.CheckNameCharacters(name) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The name contains characters that not are allowed");
            }

            // Create the image url
            string imageUrl = System.Web.HttpContext.Current.Server.MapPath("/Content/campaigns/images/" + name);

            // Delete the image if it exists
            if (System.IO.File.Exists(imageUrl))
            {
                System.IO.File.Delete(imageUrl);
            }
            else
            {
                return Request.CreateResponse<string>(HttpStatusCode.OK, "The image did not exist");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_image method

        // Delete all campaign images
        // DELETE api/campaigns/delete_all_images
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_all_images()
        {
            // Create the image directory
            string imagesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Content/campaigns/images/");

            try
            {
                // Get all the file names
                string[] imageUrlList = System.IO.Directory.GetFiles(imagesDirectory);

                // Delete all the images in the directory
                for (int i = 0; i < imageUrlList.Length; i++)
                {
                    System.IO.File.Delete(imageUrlList[i]);
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_all_images method

        #endregion

    } // End of the class

} // End of the namespace
