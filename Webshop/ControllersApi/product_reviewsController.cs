using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for product reviews
    /// </summary>
    public class product_reviewsController : ApiController
    {
        #region Insert methods

        // Add a product review
        // POST api/product_reviews/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(ProductReview post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Product.MasterPostExists(post.product_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product does not exist");
            }
            else if (Customer.MasterPostExists(post.customer_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The customer does not exist");
            }
            else if (Language.MasterPostExists(post.language_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }

            // Make sure that the data is valid
            post.review_date = AnnytabDataValidation.TruncateDateTime(post.review_date);
            post.rating = AnnytabDataValidation.TruncateDecimal(post.rating, 0, 999999.99M);

            // Add the post
            ProductReview.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a product review
        // PUT api/product_reviews/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(ProductReview post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Product.MasterPostExists(post.product_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product does not exist");
            }
            else if (Customer.MasterPostExists(post.customer_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The customer does not exist");
            }
            else if (Language.MasterPostExists(post.language_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }

            // Make sure that the data is valid
            post.review_date = AnnytabDataValidation.TruncateDateTime(post.review_date);
            post.rating = AnnytabDataValidation.TruncateDecimal(post.rating, 0, 999999.99M);

            // Get the saved post
            ProductReview savedPost = ProductReview.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            ProductReview.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/product_reviews/get_count_by_search?keywords=one%20two
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
            Int32 count = ProductReview.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a product review by id
        // GET api/product_reviews/get_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public ProductReview get_by_id(Int32 id = 0)
        {
            // Create the post to return
            ProductReview post = ProductReview.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all product reviews
        // GET api/product_reviews/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductReview> get_all(string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<ProductReview> posts = ProductReview.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/product_reviews/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductReview> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<ProductReview> posts = ProductReview.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a product review
        // DELETE api/product_reviews/delete/5
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = ProductReview.DeleteOnId(id);

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
