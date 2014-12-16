using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for product options
    /// </summary>
    public class product_optionsController : ApiController
    {
        #region Insert methods

        // Add a product option
        // POST api/product_options/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(ProductOption post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (ProductOptionType.MasterPostExists(post.product_option_type_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product option type does not exist");
            }
            else if (Option.MasterPostExists(post.option_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The option does not exist");
            }

            // Make sure that the data is valid
            post.mpn_suffix = AnnytabDataValidation.TruncateString(post.mpn_suffix, 10);
            post.price_addition = AnnytabDataValidation.TruncateDecimal(post.price_addition, 0, 9999999999.99M);
            post.freight_addition = AnnytabDataValidation.TruncateDecimal(post.freight_addition, 0, 9999999999.99M);

            // Add the post
            ProductOption.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a product option
        // PUT api/product_options/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(ProductOption post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (ProductOptionType.MasterPostExists(post.product_option_type_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product option type does not exist");
            }
            else if (Option.MasterPostExists(post.option_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The option does not exist");
            }

            // Make sure that the data is valid
            post.mpn_suffix = AnnytabDataValidation.TruncateString(post.mpn_suffix, 10);
            post.price_addition = AnnytabDataValidation.TruncateDecimal(post.price_addition, 0, 9999999999.99M);
            post.freight_addition = AnnytabDataValidation.TruncateDecimal(post.freight_addition, 0, 9999999999.99M);

            // Get the saved post
            ProductOption savedPost = ProductOption.GetOneById(post.product_option_type_id, post.option_id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            ProductOption.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Get methods

        // Get a product option by id
        // GET api/product_options/get_by_id/1?optionId=2&languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public ProductOption get_by_id(Int32 id = 0, Int32 optionId = 0, Int32 languageId = 0)
        {
            // Create the post to return
            ProductOption post = ProductOption.GetOneById(id, optionId, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get product options by product option type id
        // GET api/product_options/get_by_product_option_type_id/1?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductOption> get_by_product_option_type_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the list to return
            List<ProductOption> posts = ProductOption.GetByProductOptionTypeId(id, languageId);

            // Return the list
            return posts;

        } // End of the get_by_product_option_type_id method

        // Get all product options
        // GET api/product_options/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductOption> get_all(Int32 languageId = 0)
        {
            // Create the list to return
            List<ProductOption> posts = ProductOption.GetAll(languageId);

            // Return the list
            return posts;

        } // End of the get_all method

        #endregion

        #region Delete methods

        // Delete a product option
        // DELETE api/product_options/delete/1?optionId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 optionId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = ProductOption.DeleteOnId(id, optionId);

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
