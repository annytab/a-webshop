using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for static pages
    /// </summary>
    public class static_pagesController : ApiController
    {
        #region Insert methods

        // Add a static page
        // POST api/static_pages/add?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(StaticPage post, Int32 languageId = 0)
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
            post.link_name = AnnytabDataValidation.TruncateString(post.link_name, 100);
            post.title = AnnytabDataValidation.TruncateString(post.title, 200);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);

            // Get a static page on page name
            StaticPage staticPageOnPageName = StaticPage.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (staticPageOnPageName != null && post.id != staticPageOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Add the post
            Int64 insertId = StaticPage.AddMasterPost(post);
            post.id = Convert.ToInt32(insertId);
            StaticPage.AddLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a static page
        // PUT api/static_pages/update?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(StaticPage post, Int32 languageId = 0)
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
            post.link_name = AnnytabDataValidation.TruncateString(post.link_name, 100);
            post.title = AnnytabDataValidation.TruncateString(post.title, 200);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);

            // Get the saved post
            StaticPage savedPost = StaticPage.GetOneById(post.id, languageId);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Get a static page on page name
            StaticPage staticPageOnPageName = StaticPage.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (staticPageOnPageName != null && post.id != staticPageOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Update the post
            StaticPage.UpdateMasterPost(post);
            StaticPage.UpdateLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a static page
        // PUT api/static_pages/translate?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(StaticPage post, Int32 languageId = 0)
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
            else if(StaticPage.MasterPostExists(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The static page does not exist");
            }

            // Make sure that the data is valid
            post.link_name = AnnytabDataValidation.TruncateString(post.link_name, 100);
            post.title = AnnytabDataValidation.TruncateString(post.title, 200);
            post.meta_description = AnnytabDataValidation.TruncateString(post.meta_description, 200);
            post.meta_keywords = AnnytabDataValidation.TruncateString(post.meta_keywords, 200);
            post.meta_robots = AnnytabDataValidation.TruncateString(post.meta_robots, 20);
            post.page_name = AnnytabDataValidation.TruncateString(post.page_name, 100);

            // Get a static page on page name
            StaticPage staticPageOnPageName = StaticPage.GetOneByPageName(post.page_name, languageId);

            // Check if the page name exists
            if (staticPageOnPageName != null && post.id != staticPageOnPageName.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The page name is not unique for the language");
            }

            // Get the post
            StaticPage savedPost = StaticPage.GetOneById(post.id, languageId);

            // Check if we should add or update the post
            if (savedPost == null)
            {
                StaticPage.AddLanguagePost(post, languageId);
            }
            else
            {
                StaticPage.UpdateLanguagePost(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translate was successful");

        } // End of the translate method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/static_pages/get_count_by_search?keywords=one%20two&languageId=1
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
            Int32 count = StaticPage.GetCountBySearch(wordArray, languageId);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a static page by id
        // GET api/static_pages/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public StaticPage get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            StaticPage post = StaticPage.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all static pages
        // GET api/static_pages/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<StaticPage> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<StaticPage> posts = StaticPage.GetAll(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get all active static pages
        // GET api/static_pages/get_all_active?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<StaticPage> get_all_active(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<StaticPage> posts = StaticPage.GetAllActive(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all_active method

        // Get posts by a search
        // GET api/static_pages/get_by_search?keywords=one%20two&languageId=1&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<StaticPage> get_by_search(string keywords = "", Int32 languageId = 0, Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<StaticPage> posts = StaticPage.GetBySearch(wordArray, languageId, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a static page
        // DELETE api/static_pages/delete/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = StaticPage.DeleteOnId(id);
            }
            else
            {
                errorCode = StaticPage.DeleteLanguagePostOnId(id, languageId);
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
