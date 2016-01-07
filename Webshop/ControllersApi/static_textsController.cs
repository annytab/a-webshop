using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for static texts
    /// </summary>
    public class static_textsController : ApiController
    {
        #region Insert methods

        // Add a static text
        // POST api/static_texts/add?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(StaticText post, Int32 languageId = 0)
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
            post.id = AnnytabDataValidation.TruncateString(post.id, 100);
            post.value = AnnytabDataValidation.TruncateString(post.value, 200);

            // Check if the id exists
            if(StaticText.MasterPostExists(post.id) == true)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The id already exists");
            }

            // Make sure that the id contains valid characters
            if (AnnytabDataValidation.CheckPageNameCharacters(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The id contains characters that not are allowed");
            }

            // Add the post
            StaticText.Add(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a static text
        // PUT api/static_texts/update?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(StaticText post, Int32 languageId = 0)
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
            post.id = AnnytabDataValidation.TruncateString(post.id, 100);
            post.value = AnnytabDataValidation.TruncateString(post.value, 200);

            // Make sure that the id contains valid characters
            if (AnnytabDataValidation.CheckPageNameCharacters(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The id contains characters that not are allowed");
            }

            // Get the saved post
            StaticText savedPost = StaticText.GetOneById(post.id, languageId);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            StaticText.Update(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a static text
        // PUT api/static_texts/translate?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(StaticText post, Int32 languageId = 0)
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
            post.id = AnnytabDataValidation.TruncateString(post.id, 100);
            post.value = AnnytabDataValidation.TruncateString(post.value, 200);

            // Make sure that the id contains valid characters
            if (AnnytabDataValidation.CheckPageNameCharacters(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The id contains characters that not are allowed");
            }

            // Get the saved post
            StaticText savedPost = StaticText.GetOneById(post.id, languageId);

            // Check if the post exists
            if (savedPost == null)
            {
                StaticText.Add(post, languageId);
            }
            else
            {
                StaticText.Update(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translate was successful");

        } // End of the translate method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/static_texts/get_count_by_search?keywords=one%20two&languageId=1
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
            Int32 count = StaticText.GetCountBySearch(wordArray, languageId);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a static text by id
        // GET api/static_texts/get_by_id/key?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public StaticText get_by_id(string id = "", Int32 languageId = 0)
        {
            // Create the post to return
            StaticText post = StaticText.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all static texts
        // GET api/static_texts/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Dictionary<string, string> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            KeyStringList posts = StaticText.GetAll(languageId, sortField, sortOrder);

            // Return the list
            return posts.dictionary;

        } // End of the get_all method

        // Get posts by a search
        // GET api/static_texts/get_by_search?keywords=one%20two&languageId=1&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<StaticText> get_by_search(string keywords = "", Int32 languageId = 0, Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<StaticText> posts = StaticText.GetBySearch(wordArray, languageId, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a static text
        // DELETE api/static_texts/delete/key?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(string id = "", Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = StaticText.DeleteOnId(id);
            }
            else
            {
                errorCode = StaticText.DeleteLanguagePostOnId(id, languageId);
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
