using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for domains
    /// </summary>
    public class domainsController : ApiController
    {
        #region Insert methods

        // Add a domain
        // POST api/domains/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Domain post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Country.MasterPostExists(post.country_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The country does not exist");
            }
            else if (Language.MasterPostExists(post.front_end_language) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The front end language does not exist");
            }
            else if (Language.MasterPostExists(post.back_end_language) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The back end language does not exist");
            }
            else if (Currency.MasterPostExists(post.currency) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The currency does not exist");
            }
            else if (Company.MasterPostExists(post.company_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The company does not exist");
            }
            else if (post.custom_theme_id != 0 && CustomTheme.MasterPostExists(post.custom_theme_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The theme does not exist");
            }
            else if (Domain.GetOneByDomainName(post.domain_name) != null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The domain name is not unique");
            }

            // Make sure that the data is valid
            post.webshop_name = AnnytabDataValidation.TruncateString(post.webshop_name, 100);
            post.domain_name = AnnytabDataValidation.TruncateString(post.domain_name, 100);
            post.web_address = AnnytabDataValidation.TruncateString(post.web_address, 100);
            post.analytics_tracking_id = AnnytabDataValidation.TruncateString(post.analytics_tracking_id, 50);
            post.facebook_app_id = AnnytabDataValidation.TruncateString(post.facebook_app_id, 50);
            post.facebook_app_secret = AnnytabDataValidation.TruncateString(post.facebook_app_secret, 50);
            post.google_app_id = AnnytabDataValidation.TruncateString(post.google_app_id, 100);
            post.google_app_secret = AnnytabDataValidation.TruncateString(post.google_app_secret, 50);

            // Add the post
            Domain.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a domain
        // PUT api/domains/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Domain post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Country.MasterPostExists(post.country_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The country does not exist");
            }
            else if (Language.MasterPostExists(post.front_end_language) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The front end language does not exist");
            }
            else if (Language.MasterPostExists(post.back_end_language) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The back end language does not exist");
            }
            else if (Currency.MasterPostExists(post.currency) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The currency does not exist");
            }
            else if (Company.MasterPostExists(post.company_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The company does not exist");
            }
            else if (post.custom_theme_id != 0 && CustomTheme.MasterPostExists(post.custom_theme_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The theme does not exist");
            }

            // Make sure that the domain name is unique
            Domain domainOnName = Domain.GetOneByDomainName(post.domain_name);

            if (domainOnName != null && domainOnName.id != post.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The domain name is not unique");
            }

            // Make sure that the data is valid
            post.webshop_name = AnnytabDataValidation.TruncateString(post.webshop_name, 100);
            post.domain_name = AnnytabDataValidation.TruncateString(post.domain_name, 100);
            post.web_address = AnnytabDataValidation.TruncateString(post.web_address, 100);
            post.analytics_tracking_id = AnnytabDataValidation.TruncateString(post.analytics_tracking_id, 50);
            post.facebook_app_id = AnnytabDataValidation.TruncateString(post.facebook_app_id, 50);
            post.facebook_app_secret = AnnytabDataValidation.TruncateString(post.facebook_app_secret, 50);
            post.google_app_id = AnnytabDataValidation.TruncateString(post.google_app_id, 100);
            post.google_app_secret = AnnytabDataValidation.TruncateString(post.google_app_secret, 50);

            // Get the saved post
            Domain savedPost = Domain.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Domain.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/domains/get_count_by_search?keywords=one%20two
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
            Int32 count = Domain.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a domain by id
        // GET api/domains/get_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Domain get_by_id(Int32 id = 0)
        {
            // Create the post to return
            Domain post = Domain.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all domains
        // GET api/domains/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Domain> get_all(string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Domain> posts = Domain.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/domains/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Domain> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Domain> posts = Domain.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a domain
        // DELETE api/domains/delete/5
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = Domain.DeleteOnId(id);

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
