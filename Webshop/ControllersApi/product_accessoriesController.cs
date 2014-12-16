using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for product accessories
    /// </summary>
    public class product_accessoriesController : ApiController
    {
        #region Insert methods

        // Add a product accessory
        // POST api/product_accessories/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(ProductAccessory post)
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
            else if (Product.MasterPostExists(post.accessory_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The accessory does not exist");
            }
            else if(ProductAccessory.GetOneById(post.product_id, post.accessory_id) != null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The product accessory already exists");
            }

            // Add the post
            ProductAccessory.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Get methods

        // Get a product accessory by id
        // GET api/product_accessories/get_by_id/1?accessoryId=2
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public ProductAccessory get_by_id(Int32 id = 0, Int32 accessoryId = 0)
        {
            // Create the post to return
            ProductAccessory post = ProductAccessory.GetOneById(id, accessoryId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get product accessories by product id as products
        // GET api/product_accessories/get_by_product_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Product> get_by_product_id(Int32 id = 0, Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Product> posts = ProductAccessory.GetByProductId(id, languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_product_id method

        // Get all product accessories
        // GET api/product_accessories/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<ProductAccessory> get_all()
        {
            // Create the list to return
            List<ProductAccessory> posts = ProductAccessory.GetAll();

            // Return the list
            return posts;

        } // End of the get_all method

        #endregion

        #region Delete methods

        // Delete a product accessory
        // DELETE api/product_accessories/delete/1?accessoryId=2
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 accessoryId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = ProductAccessory.DeleteOnId(id, accessoryId);

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
