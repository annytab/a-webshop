using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for product option types
    /// </summary>
    public class product_option_typesController : ApiController
    {
        #region Insert methods

        // Add a product option type
        // POST api/product_option_types/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(ProductOptionType post)
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
            else if (OptionType.MasterPostExists(post.option_type_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The option type does not exist");
            }

            // Add the post
            ProductOptionType.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a product option type
        // PUT api/product_option_types/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(ProductOptionType post)
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
            else if (OptionType.MasterPostExists(post.option_type_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The option type does not exist");
            }

            // Get the saved post
            ProductOptionType savedPost = ProductOptionType.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            ProductOptionType.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Get methods

        // Get a product option type by id
        // GET api/product_option_types/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public ProductOptionType get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            ProductOptionType post = ProductOptionType.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get by product id
        // GET api/product_option_types/get_by_product_id/1?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductOptionType> get_by_product_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the list to return
            List<ProductOptionType> posts = ProductOptionType.GetByProductId(id, languageId);

            // Return the list
            return posts;

        } // End of the get_by_product_id method

        // Get all product option types
        // GET api/product_option_types/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductOptionType> get_all(Int32 languageId = 0)
        {
            // Create the list to return
            List<ProductOptionType> posts = ProductOptionType.GetAll(languageId);

            // Return the list
            return posts;

        } // End of the get_all method

        #endregion

        #region Delete methods

        // Delete a product option type
        // DELETE api/product_option_types/delete/1
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = ProductOptionType.DeleteOnId(id);

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
