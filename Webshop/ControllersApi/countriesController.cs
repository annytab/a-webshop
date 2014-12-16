using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for countries
    /// </summary>
    public class countriesController : ApiController
    {
        #region Insert methods

        // Add a country
        // POST api/countries/add?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Country post, Int32 languageId = 0)
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
            post.country_code = AnnytabDataValidation.TruncateString(post.country_code, 2);
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);

            // Add the post
            Int64 insertId = Country.AddMasterPost(post);
            post.id = Convert.ToInt32(insertId);
            Country.AddLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a country
        // PUT api/countries/update?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Country post, Int32 languageId = 0)
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
            post.country_code = AnnytabDataValidation.TruncateString(post.country_code, 2);
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);

            // Get the saved post
            Country savedPost = Country.GetOneById(post.id, languageId);

            // Check if the post exists
            if(savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Country.UpdateMasterPost(post);
            Country.UpdateLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a country
        // PUT api/countries/translate?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(Country post, Int32 languageId = 0)
        {
            // Check for errors
            if(post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(languageId) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (Country.MasterPostExists(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The country does not exist");
            }

            // Make sure that the data is valid
            post.country_code = AnnytabDataValidation.TruncateString(post.country_code, 2);
            post.name = AnnytabDataValidation.TruncateString(post.name, 50);

            // Get the post
            Country savedPost = Country.GetOneById(post.id, languageId);

            // Check if we should add or update the post
            if(savedPost == null)
            {
                Country.AddLanguagePost(post, languageId);
            }
            else
            {
                Country.UpdateLanguagePost(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translate was successful");

        } // End of the translate method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/countries/get_count_by_search?keywords=one%20two&languageId=1
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
            Int32 count = Country.GetCountBySearch(wordArray, languageId);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a country by id
        // GET api/countries/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Country get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            Country post = Country.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all countries
        // GET api/countries/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Country> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Country> posts = Country.GetAll(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/countries/get_by_search?keywords=one%20two&languageId=1&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Country> get_by_search(string keywords = "", Int32 languageId = 0, Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Country> posts = Country.GetBySearch(wordArray, languageId, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a country
        // DELETE api/countries/delete?languageId=0
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = Country.DeleteOnId(id);
            }
            else
            {
                errorCode = Country.DeleteLanguagePostOnId(id, languageId);
            }

            // Check if there is an error
            if(errorCode != 0)
            {
                return Request.CreateResponse<string>(HttpStatusCode.Conflict, "Foreign key constraint");
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The delete was successful");

        } // End of the delete method

        #endregion

    } // End of the class

} // End of the namespace
