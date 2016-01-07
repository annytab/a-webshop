using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for order rows
    /// </summary>
    public class order_rowsController : ApiController
    {
        #region Insert methods

        // Add a order row
        // POST api/order_rows/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(OrderRow post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Order.MasterPostExists(post.order_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The order does not exist");
            }
            else if (OrderRow.GetOneById(post.order_id, post.product_code) != null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The order row is not unique");
            }
            else if(Unit.MasterPostExists(post.unit_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The unit does not exist");
            }

            // Make sure that the data is valid
            post.product_code = AnnytabDataValidation.TruncateString(post.product_code, 50);
            post.manufacturer_code = AnnytabDataValidation.TruncateString(post.manufacturer_code, 50);
            post.gtin = AnnytabDataValidation.TruncateString(post.gtin, 20);
            post.product_name = AnnytabDataValidation.TruncateString(post.product_name, 100);
            post.vat_percent = AnnytabDataValidation.TruncateDecimal(post.vat_percent, 0, 9.99999M);
            post.quantity = AnnytabDataValidation.TruncateDecimal(post.quantity, 0, 999999.99M);
            post.unit_price = AnnytabDataValidation.TruncateDecimal(post.unit_price, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);
            post.supplier_erp_id = AnnytabDataValidation.TruncateString(post.supplier_erp_id, 20);

            // Add the post
            OrderRow.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update methods

        // Update a order row
        // PUT api/order_rows/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(OrderRow post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Order.MasterPostExists(post.order_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The order does not exist");
            }
            else if (Unit.MasterPostExists(post.unit_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The unit does not exist");
            }

            // Make sure that the data is valid
            post.product_code = AnnytabDataValidation.TruncateString(post.product_code, 50);
            post.manufacturer_code = AnnytabDataValidation.TruncateString(post.manufacturer_code, 50);
            post.gtin = AnnytabDataValidation.TruncateString(post.gtin, 20);
            post.product_name = AnnytabDataValidation.TruncateString(post.product_name, 100);
            post.vat_percent = AnnytabDataValidation.TruncateDecimal(post.vat_percent, 0, 9.99999M);
            post.quantity = AnnytabDataValidation.TruncateDecimal(post.quantity, 0, 999999.99M);
            post.unit_price = AnnytabDataValidation.TruncateDecimal(post.unit_price, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);
            post.supplier_erp_id = AnnytabDataValidation.TruncateString(post.supplier_erp_id, 20);

            // Get the saved post
            OrderRow savedPost = OrderRow.GetOneById(post.order_id, post.product_code);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            OrderRow.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Get methods

        // Get one order row by id
        // GET api/order_rows/get_by_id/5?productCode=AA
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public OrderRow get_by_id(Int32 id = 0, string productCode = "")
        {
            // Create the post to return
            OrderRow post = OrderRow.GetOneById(id, productCode);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all the order rows for an order id
        // GET api/order_rows/get_by_order_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<OrderRow> get_by_order_id(Int32 id = 0)
        {
            // Create the list to return
            List<OrderRow> posts = OrderRow.GetByOrderId(id);

            // Return the list
            return posts;

        } // End of the get_by_order_id method

        // Get all the order rows
        // GET api/order_rows/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<OrderRow> get_all()
        {
            // Create the list to return
            List<OrderRow> posts = new List<OrderRow>(10);

            // Get all the posts
            posts = OrderRow.GetAll();

            // Return the list
            return posts;

        } // End of the get_all method

        #endregion

        #region Delete methods

        // Delete a order row
        // DELETE api/order_rows/delete/1?productCode=AA
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, string productCode = "")
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = OrderRow.DeleteOnId(id, productCode);

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
