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
    /// This class controls the api for the custom design
    /// </summary>
    public class custom_designController : ApiController
    {
        #region Insert methods

        // Add a custom theme
        // POST api/custom_design/add_theme
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add_theme(CustomTheme post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 100);

            // Add the post
            Int32 insertId = (Int32)CustomTheme.Add(post);
            CustomTheme.AddCustomThemeTemplates(insertId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add_theme method

        // Add a custom template
        // POST api/custom_design/add_template
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add_template(CustomThemeTemplate post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (CustomTheme.MasterPostExists(post.custom_theme_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The theme does not exist");
            }

            // Make sure that the data is valid
            post.user_file_name = AnnytabDataValidation.TruncateString(post.user_file_name, 200);
            post.master_file_url = AnnytabDataValidation.TruncateString(post.master_file_url, 100);
            post.comment = AnnytabDataValidation.TruncateString(post.comment, 200);

            // Add the post
            CustomTheme.AddTemplate(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add_template method

        #endregion

        #region Update

        // Update a custom theme
        // PUT api/custom_design/update_theme
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update_theme(CustomTheme post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 100);

            // Get the saved post
            CustomTheme savedPost = CustomTheme.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            CustomTheme.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update_theme method

        // Update a custom template
        // PUT api/custom_design/update_template
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update_template(CustomThemeTemplate post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }

            // Make sure that the data is valid
            post.user_file_name = AnnytabDataValidation.TruncateString(post.user_file_name, 200);
            post.master_file_url = AnnytabDataValidation.TruncateString(post.master_file_url, 100);
            post.comment = AnnytabDataValidation.TruncateString(post.comment, 200);

            // Get the saved post
            CustomThemeTemplate savedPost = CustomThemeTemplate.GetOneById(post.custom_theme_id, post.user_file_name);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            CustomTheme.UpdateTemplate(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update_template method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/custom_design/get_theme_count_by_search?keywords=one%20two
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Int32 get_theme_count_by_search(string keywords = "")
        {
            // Create the string array
            string[] wordArray = new string[] { "" };

            // Recreate the array if keywords is different from null
            if (keywords != null)
            {
                wordArray = keywords.Split(' ');
            }

            // Get the count
            Int32 count = CustomTheme.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_theme_count_by_search method

        #endregion

        #region Get methods

        // Get a custom theme by id
        // GET api/custom_design/get_theme_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public CustomTheme get_theme_by_id(Int32 id = 0)
        {
            // Create the post to return
            CustomTheme post = CustomTheme.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_theme_by_id method

        // Get a custom template by id
        // GET api/custom_design/get_template_by_id/5?userFileName=master.cshtml
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public CustomThemeTemplate get_template_by_id(Int32 id = 0, string userFileName = "")
        {
            // Create the post to return
            CustomThemeTemplate post = CustomThemeTemplate.GetOneById(id, userFileName);

            // Return the post
            return post;

        } // End of the get_template_by_id method

        // Get all custom themes
        // GET api/custom_design/get_all_themes
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<CustomTheme> get_all_themes(string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<CustomTheme> posts = CustomTheme.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all_themes method

        // Get custom templates by theme id
        // GET api/custom_design/get_templates_by_theme_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<CustomThemeTemplate> get_templates_by_theme_id(Int32 id = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<CustomThemeTemplate> posts = CustomThemeTemplate.GetAllPostsByCustomThemeId(id, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_templates_by_theme_id method

        // Get all custom templates
        // GET api/custom_design/get_all_templates
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<CustomThemeTemplate> get_all_templates(string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<CustomThemeTemplate> posts = CustomThemeTemplate.GetAllPosts(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all_templates method

        // Get posts by a search
        // GET api/custom_design/get_themes_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<CustomTheme> get_themes_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<CustomTheme> posts = CustomTheme.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_themes_by_search method

        #endregion

        #region Delete methods

        // Delete a custom theme
        // DELETE api/custom_design/delete_theme/1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_theme(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = CustomTheme.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                return Request.CreateResponse<string>(HttpStatusCode.Conflict, "Foreign key constraint");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_theme method

        // Delete a custom template
        // DELETE api/custom_design/delete_template/1?userFileName=master.cshtml
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_template(Int32 id = 0, string userFileName = "")
        {
            // Delete the post
            CustomTheme.DeleteTemplateOnId(id, userFileName);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_template method

        #endregion

        #region File methods

        // Upload custom images
        // POST api/custom_design/upload_images
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
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath("/Content/images/user_design/");

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

        // Upload custom files
        // POST api/custom_design/upload_files
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public async Task<HttpResponseMessage> upload_files()
        {
            // Verify that this is an HTML Form file upload request
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.UnsupportedMediaType, "Files was not uploaded");
            }

            // Create the directory path
            string directoryPath = System.Web.HttpContext.Current.Server.MapPath("/Content/user_files/");

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
            return Request.CreateResponse<string>(HttpStatusCode.OK, "Files has been uploaded");

        } // End of the upload_files method

        // Delete a custom image
        // DELETE api/custom_design/delete_image?name=image.jpg
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_image(string name = "")
        {
            // Check the name for invalid characters
            if (AnnytabDataValidation.CheckNameCharacters(name) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The name contains characters that not are allowed");
            }

            // Create the image url
            string imageUrl = System.Web.HttpContext.Current.Server.MapPath("/Content/images/user_design/" + name);

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

        // Delete a custom file
        // DELETE api/custom_design/delete_file?name=file.js
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_file(string name = "")
        {
            // Check the name for invalid characters
            if (AnnytabDataValidation.CheckNameCharacters(name) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The name contains characters that not are allowed");
            }

            // Create the file url
            string fileUrl = System.Web.HttpContext.Current.Server.MapPath("/Content/user_files/" + name);

            // Delete the file if it exists
            if (System.IO.File.Exists(fileUrl))
            {
                System.IO.File.Delete(fileUrl);
            }
            else
            {
                return Request.CreateResponse<string>(HttpStatusCode.OK, "The file did not exist");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_image method

        // Delete all images
        // DELETE api/custom_design/delete_all_images
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_all_images()
        {
            // Create the image directory
            string imagesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Content/images/user_design/");

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

        // Delete all files
        // DELETE api/custom_design/delete_all_files
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete_all_files()
        {
            // Create the files directory
            string filesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Content/user_files/");

            try
            {
                // Get all the file names
                string[] fileUrlList = System.IO.Directory.GetFiles(filesDirectory);

                // Delete all the files in the directory
                for (int i = 0; i < fileUrlList.Length; i++)
                {
                    System.IO.File.Delete(fileUrlList[i]);
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete_all_files method

        #endregion

    } // End of the class

} // End of the namespace
