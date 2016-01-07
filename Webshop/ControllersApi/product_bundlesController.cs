using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for product bundles
    /// </summary>
    public class product_bundlesController : ApiController
    {
        #region Insert methods

        // Add a product bundle
        // POST api/product_bundles/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(ProductBundle post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Product.MasterPostExists(post.bundle_product_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The bundle product does not exist");
            }
            else if (Product.MasterPostExists(post.product_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product does not exist");
            }
            else if (ProductBundle.GetOneById(post.bundle_product_id, post.product_id) != null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product bundle already exists");
            }

            // Make sure that the data is valid
            post.quantity = AnnytabDataValidation.TruncateDecimal(post.quantity, 0, 999999.99M);

            // Add the post
            ProductBundle.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a product bundle
        // PUT api/product_bundles/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(ProductBundle post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }

            // Make sure that the data is valid
            post.quantity = AnnytabDataValidation.TruncateDecimal(post.quantity, 0, 999999.99M);

            // Get the saved post
            ProductBundle savedPost = ProductBundle.GetOneById(post.bundle_product_id, post.product_id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            ProductBundle.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Get methods

        // Get a product bundle by id
        // GET api/product_bundles/get_by_id/1?productId=2
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public ProductBundle get_by_id(Int32 id = 0, Int32 productId = 0)
        {
            // Create the post to return
            ProductBundle post = ProductBundle.GetOneById(id, productId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get product bundles by bundle product id
        // GET api/product_bundles/get_by_bundle_product_id/1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductBundle> get_by_bundle_product_id(Int32 id = 0)
        {
            // Create the list to return
            List<ProductBundle> posts = ProductBundle.GetByBundleProductId(id);

            // Return the list
            return posts;

        } // End of the get_by_bundle_product_id method

        // Get all product bundles
        // GET api/product_bundles/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductBundle> get_all()
        {
            // Create the list to return
            List<ProductBundle> posts = ProductBundle.GetAll();

            // Return the list
            return posts;

        } // End of the get_all method

        #endregion

        #region Delete methods

        // Delete a product bundle
        // DELETE api/product_bundles/delete/1?productId=2
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 productId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = ProductBundle.DeleteOnId(id, productId);

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
