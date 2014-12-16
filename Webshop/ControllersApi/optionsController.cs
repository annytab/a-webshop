using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for options
    /// </summary>
    public class optionsController : ApiController
    {
        #region Insert methods

        // Add a option
        // POST api/options/add?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Option post, Int32 languageId = 0)
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
            else if (OptionType.MasterPostExists(post.option_type_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The option type does not exist");
            }

            // Make sure that the data is valid
            post.title = AnnytabDataValidation.TruncateString(post.title, 50);
            post.product_code_suffix = AnnytabDataValidation.TruncateString(post.product_code_suffix, 10);

            // Add the post
            Int64 insertId = Option.AddMasterPost(post);
            post.id = Convert.ToInt32(insertId);
            Option.AddLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a option
        // PUT api/options/update?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Option post, Int32 languageId = 0)
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
            else if (OptionType.MasterPostExists(post.option_type_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The option type does not exist");
            }

            // Make sure that the data is valid
            post.title = AnnytabDataValidation.TruncateString(post.title, 50);
            post.product_code_suffix = AnnytabDataValidation.TruncateString(post.product_code_suffix, 10);

            // Get the saved post
            Option savedPost = Option.GetOneById(post.id, languageId);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Option.UpdateMasterPost(post);
            Option.UpdateLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a option
        // PUT api/options/translate?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(Option post, Int32 languageId = 0)
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
            else if (Option.MasterPostExists(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The option does not exist");
            }

            // Make sure that the data is valid
            post.title = AnnytabDataValidation.TruncateString(post.title, 50);
            post.product_code_suffix = AnnytabDataValidation.TruncateString(post.product_code_suffix, 10);

            // Get the post
            Option savedPost = Option.GetOneById(post.id, languageId);

            // Check if we should add or update the post
            if (savedPost == null)
            {
                Option.AddLanguagePost(post, languageId);
            }
            else
            {
                Option.UpdateLanguagePost(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translate was successful");

        } // End of the translate method

        #endregion

        #region Get methods

        // Get a option by id
        // GET api/options/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Option get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            Option post = Option.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get options by option type id
        // GET api/options/get_by_option_type_id/1?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Option> get_by_option_type_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the list to return
            List<Option> posts = Option.GetByOptionTypeId(id, languageId);

            // Return the list
            return posts;

        } // End of the get_by_option_type_id method

        // Get all options
        // GET api/options/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Option> get_all(Int32 languageId = 0)
        {
            // Create the list to return
            List<Option> posts = Option.GetAll(languageId);

            // Return the list
            return posts;

        } // End of the get_all method

        #endregion

        #region Delete methods

        // Delete a option
        // DELETE api/options/delete?languageId=0
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = Option.DeleteOnId(id);
            }
            else
            {
                errorCode = Option.DeleteLanguagePostOnId(id, languageId);
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

    } // End of the class

} // End of the namespace
