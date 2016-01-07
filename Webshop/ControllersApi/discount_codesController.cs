using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for discount codes
    /// </summary>
    public class discount_codesController : ApiController
    {
        #region Insert methods

        // Add a discount code
        // POST api/discount_codes/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(DiscountCode post)
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
            else if (Currency.MasterPostExists(post.currency_code) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The currency does not exist");
            }

            // Make sure that the data is valid
            post.id = AnnytabDataValidation.TruncateString(post.id, 50);
            post.discount_value = AnnytabDataValidation.TruncateDecimal(post.discount_value, 0, 9.999M);
            post.end_date = AnnytabDataValidation.TruncateDateTime(post.end_date);
            post.minimum_order_value = AnnytabDataValidation.TruncateDecimal(post.minimum_order_value, 0, 999999999999.99M);

            // Check if the discount code exists
            if (DiscountCode.MasterPostExists(post.id) == true)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The id already exists");
            }

            // Add the post
            DiscountCode.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a discount code
        // PUT api/discount_codes/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(DiscountCode post)
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
            else if (Currency.MasterPostExists(post.currency_code) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The currency does not exist");
            }

            // Make sure that the data is valid
            post.id = AnnytabDataValidation.TruncateString(post.id, 50);
            post.discount_value = AnnytabDataValidation.TruncateDecimal(post.discount_value, 0, 9.999M);
            post.end_date = AnnytabDataValidation.TruncateDateTime(post.end_date);
            post.minimum_order_value = AnnytabDataValidation.TruncateDecimal(post.minimum_order_value, 0, 999999999999.99M);

            // Get the saved post
            DiscountCode savedPost = DiscountCode.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            DiscountCode.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/discount_codes/get_count_by_search?keywords=one%20two
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
            Int32 count = DiscountCode.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a discount code by id
        // GET api/discount_codes/get_by_id/SSSWWW
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public DiscountCode get_by_id(string id = "")
        {
            // Create the post to return
            DiscountCode post = DiscountCode.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all discount codes
        // GET api/discount_codes/get_all?sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<DiscountCode> get_all(string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<DiscountCode> posts = DiscountCode.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/discount_codes/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<DiscountCode> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<DiscountCode> posts = DiscountCode.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a discount code
        // DELETE api/discount_codes/delete/SSSMMWW
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(string id = "")
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = DiscountCode.DeleteOnId(id);

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
