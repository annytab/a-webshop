using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for units
    /// </summary>
    public class unitsController : ApiController
    {
        #region Insert methods

        // Add a unit
        // POST api/units/add?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Unit post, Int32 languageId = 0)
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
            post.unit_code_si = AnnytabDataValidation.TruncateString(post.unit_code_si, 10);
            post.erp_code = AnnytabDataValidation.TruncateString(post.erp_code, 10);
            post.unit_code = AnnytabDataValidation.TruncateString(post.unit_code, 10);
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);

            // Add the post
            Int64 insertId = Unit.AddMasterPost(post);
            post.id = Convert.ToInt32(insertId);
            Unit.AddLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a unit
        // PUT api/units/update?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Unit post, Int32 languageId = 0)
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
            post.unit_code_si = AnnytabDataValidation.TruncateString(post.unit_code_si, 10);
            post.erp_code = AnnytabDataValidation.TruncateString(post.erp_code, 10);
            post.unit_code = AnnytabDataValidation.TruncateString(post.unit_code, 10);
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);

            // Get the saved post
            Unit savedPost = Unit.GetOneById(post.id, languageId);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Unit.UpdateMasterPost(post);
            Unit.UpdateLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a unit
        // PUT api/units/translate?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(Unit post, Int32 languageId = 0)
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
            else if(Unit.MasterPostExists(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The unit does not exist");
            }

            // Make sure that the data is valid
            post.unit_code_si = AnnytabDataValidation.TruncateString(post.unit_code_si, 10);
            post.erp_code = AnnytabDataValidation.TruncateString(post.erp_code, 10);
            post.unit_code = AnnytabDataValidation.TruncateString(post.unit_code, 10);
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);

            // Get the post
            Unit savedPost = Unit.GetOneById(post.id, languageId);

            // Check if we should add or update the post
            if (savedPost == null)
            {
                Unit.AddLanguagePost(post, languageId);
            }
            else
            {
                Unit.UpdateLanguagePost(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translate was successful");

        } // End of the translate method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/units/get_count_by_search?keywords=one%20two&languageId=1
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
            Int32 count = Unit.GetCountBySearch(wordArray, languageId);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a unit by id
        // GET api/units/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Unit get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            Unit post = Unit.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all units
        // GET api/units/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Unit> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Unit> posts = Unit.GetAll(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get all units as a dictionary
        // GET api/units/get_all_as_dictionary?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Dictionary<Int32, Unit> get_all_as_dictionary(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            Dictionary<Int32, Unit> posts = Unit.GetAllAsDictionary(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all_as_dictionary method

        // Get posts by a search
        // GET api/units/get_by_search?keywords=one%20two&languageId=1&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Unit> get_by_search(string keywords = "", Int32 languageId = 0, Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Unit> posts = Unit.GetBySearch(wordArray, languageId, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a unit
        // DELETE api/units/delete/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = Unit.DeleteOnId(id);
            }
            else
            {
                errorCode = Unit.DeleteLanguagePostOnId(id, languageId);
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
