using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for companies
    /// </summary>
    public class companiesController : ApiController
    {
        #region Insert methods

        // Add a company
        // POST api/companies/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Company post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 100);
            post.registered_office = AnnytabDataValidation.TruncateString(post.registered_office, 100);
            post.org_number = AnnytabDataValidation.TruncateString(post.org_number, 100);
            post.vat_number = AnnytabDataValidation.TruncateString(post.vat_number, 100);
            post.phone_number = AnnytabDataValidation.TruncateString(post.phone_number, 100);
            post.mobile_phone_number = AnnytabDataValidation.TruncateString(post.mobile_phone_number, 100);
            post.email = AnnytabDataValidation.TruncateString(post.email, 100);
            post.post_address_1 = AnnytabDataValidation.TruncateString(post.post_address_1, 100);
            post.post_address_2 = AnnytabDataValidation.TruncateString(post.post_address_2, 100);
            post.post_code = AnnytabDataValidation.TruncateString(post.post_code, 100);
            post.post_city = AnnytabDataValidation.TruncateString(post.post_city, 100);
            post.post_country = AnnytabDataValidation.TruncateString(post.post_country, 100);

            // Add the post
            Company.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a company
        // PUT api/companies/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Company post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }

            // Make sure that the data is valid
            post.name = AnnytabDataValidation.TruncateString(post.name, 100);
            post.registered_office = AnnytabDataValidation.TruncateString(post.registered_office, 100);
            post.org_number = AnnytabDataValidation.TruncateString(post.org_number, 100);
            post.vat_number = AnnytabDataValidation.TruncateString(post.vat_number, 100);
            post.phone_number = AnnytabDataValidation.TruncateString(post.phone_number, 100);
            post.mobile_phone_number = AnnytabDataValidation.TruncateString(post.mobile_phone_number, 100);
            post.email = AnnytabDataValidation.TruncateString(post.email, 100);
            post.post_address_1 = AnnytabDataValidation.TruncateString(post.post_address_1, 100);
            post.post_address_2 = AnnytabDataValidation.TruncateString(post.post_address_2, 100);
            post.post_code = AnnytabDataValidation.TruncateString(post.post_code, 100);
            post.post_city = AnnytabDataValidation.TruncateString(post.post_city, 100);
            post.post_country = AnnytabDataValidation.TruncateString(post.post_country, 100);

            // Get the saved post
            Company savedPost = Company.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Company.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/companies/get_count_by_search?keywords=one%20two
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
            Int32 count = Company.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a company by id
        // GET api/companies/get_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Company get_by_id(Int32 id = 0)
        {
            // Create the post to return
            Company post = Company.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all companies
        // GET api/companies/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Company> get_all(string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Company> posts = Company.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/companies/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Company> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Company> posts = Company.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a company
        // DELETE api/companies/delete
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = Company.DeleteOnId(id);

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
