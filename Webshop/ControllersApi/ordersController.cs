using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls api for orders
    /// </summary>
    public class ordersController : ApiController
    {
        #region Insert methods

        // Add a order
        // POST api/orders/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Order post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if(Company.MasterPostExists(post.company_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The company does not exist");
            }
            else if (Customer.MasterPostExists(post.customer_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The customer does not exist");
            }
            else if (Country.MasterPostExists(post.invoice_country_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The invoice country does not exist");
            }
            else if (Country.MasterPostExists(post.delivery_country_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The delivery country does not exist");
            }
            else if (PaymentOption.MasterPostExists(post.payment_option) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The payment option does not exist");
            }

            // Make sure that the data is valid
            post.order_date = AnnytabDataValidation.TruncateDateTime(post.order_date);
            post.country_code = AnnytabDataValidation.TruncateString(post.country_code, 2);
            post.language_code = AnnytabDataValidation.TruncateString(post.language_code, 2);
            post.currency_code = AnnytabDataValidation.TruncateString(post.currency_code, 3);
            post.conversion_rate = AnnytabDataValidation.TruncateDecimal(post.conversion_rate, 0, 9999.999999M);
            post.customer_org_number = AnnytabDataValidation.TruncateString(post.customer_org_number, 20);
            post.customer_vat_number = AnnytabDataValidation.TruncateString(post.customer_vat_number, 20);
            post.customer_name = AnnytabDataValidation.TruncateString(post.customer_name, 100);
            post.customer_phone = AnnytabDataValidation.TruncateString(post.customer_phone, 100);
            post.customer_mobile_phone = AnnytabDataValidation.TruncateString(post.customer_mobile_phone, 100);
            post.customer_email = AnnytabDataValidation.TruncateString(post.customer_email, 100);
            post.invoice_name = AnnytabDataValidation.TruncateString(post.invoice_name, 100);
            post.invoice_address_1 = AnnytabDataValidation.TruncateString(post.invoice_address_1, 100);
            post.invoice_address_2 = AnnytabDataValidation.TruncateString(post.invoice_address_2, 100);
            post.invoice_post_code = AnnytabDataValidation.TruncateString(post.invoice_post_code, 100);
            post.invoice_city = AnnytabDataValidation.TruncateString(post.invoice_city, 100);
            post.delivery_name = AnnytabDataValidation.TruncateString(post.delivery_name, 100);
            post.delivery_address_1 = AnnytabDataValidation.TruncateString(post.delivery_address_1, 100);
            post.delivery_address_2 = AnnytabDataValidation.TruncateString(post.delivery_address_2, 100);
            post.delivery_post_code = AnnytabDataValidation.TruncateString(post.delivery_post_code, 100);
            post.delivery_city = AnnytabDataValidation.TruncateString(post.delivery_city, 100);
            post.net_sum = AnnytabDataValidation.TruncateDecimal(post.net_sum, 0, 999999999999.99M);
            post.vat_sum = AnnytabDataValidation.TruncateDecimal(post.vat_sum, 0, 999999999999.99M);
            post.rounding_sum = AnnytabDataValidation.TruncateDecimal(post.rounding_sum, -99.999M, 999.999M);
            post.total_sum = AnnytabDataValidation.TruncateDecimal(post.total_sum, 0, 999999999999.99M);
            post.payment_token = AnnytabDataValidation.TruncateString(post.payment_token, 50);
            post.payment_status = AnnytabDataValidation.TruncateString(post.payment_status, 50);
            post.order_status = AnnytabDataValidation.TruncateString(post.order_status, 50);
            post.desired_date_of_delivery = AnnytabDataValidation.TruncateDateTime(post.desired_date_of_delivery);
            post.discount_code = AnnytabDataValidation.TruncateString(post.discount_code, 50);

            // Add the post
            Order.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update methods

        // Update a order
        // PUT api/orders/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Order post)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Company.MasterPostExists(post.company_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The company does not exist");
            }
            else if (Customer.MasterPostExists(post.customer_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The customer does not exist");
            }
            else if (Country.MasterPostExists(post.invoice_country_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The invoice country does not exist");
            }
            else if (Country.MasterPostExists(post.delivery_country_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The delivery country does not exist");
            }
            else if (PaymentOption.MasterPostExists(post.payment_option) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The payment option does not exist");
            }

            // Make sure that the data is valid
            post.order_date = AnnytabDataValidation.TruncateDateTime(post.order_date);
            post.country_code = AnnytabDataValidation.TruncateString(post.country_code, 2);
            post.language_code = AnnytabDataValidation.TruncateString(post.language_code, 2);
            post.currency_code = AnnytabDataValidation.TruncateString(post.currency_code, 3);
            post.conversion_rate = AnnytabDataValidation.TruncateDecimal(post.conversion_rate, 0, 9999.999999M);
            post.customer_org_number = AnnytabDataValidation.TruncateString(post.customer_org_number, 20);
            post.customer_vat_number = AnnytabDataValidation.TruncateString(post.customer_vat_number, 20);
            post.customer_name = AnnytabDataValidation.TruncateString(post.customer_name, 100);
            post.customer_phone = AnnytabDataValidation.TruncateString(post.customer_phone, 100);
            post.customer_mobile_phone = AnnytabDataValidation.TruncateString(post.customer_mobile_phone, 100);
            post.customer_email = AnnytabDataValidation.TruncateString(post.customer_email, 100);
            post.invoice_name = AnnytabDataValidation.TruncateString(post.invoice_name, 100);
            post.invoice_address_1 = AnnytabDataValidation.TruncateString(post.invoice_address_1, 100);
            post.invoice_address_2 = AnnytabDataValidation.TruncateString(post.invoice_address_2, 100);
            post.invoice_post_code = AnnytabDataValidation.TruncateString(post.invoice_post_code, 100);
            post.invoice_city = AnnytabDataValidation.TruncateString(post.invoice_city, 100);
            post.delivery_name = AnnytabDataValidation.TruncateString(post.delivery_name, 100);
            post.delivery_address_1 = AnnytabDataValidation.TruncateString(post.delivery_address_1, 100);
            post.delivery_address_2 = AnnytabDataValidation.TruncateString(post.delivery_address_2, 100);
            post.delivery_post_code = AnnytabDataValidation.TruncateString(post.delivery_post_code, 100);
            post.delivery_city = AnnytabDataValidation.TruncateString(post.delivery_city, 100);
            post.net_sum = AnnytabDataValidation.TruncateDecimal(post.net_sum, 0, 999999999999.99M);
            post.vat_sum = AnnytabDataValidation.TruncateDecimal(post.vat_sum, 0, 999999999999.99M);
            post.rounding_sum = AnnytabDataValidation.TruncateDecimal(post.rounding_sum, -99.999M, 999.999M);
            post.total_sum = AnnytabDataValidation.TruncateDecimal(post.total_sum, 0, 999999999999.99M);
            post.payment_token = AnnytabDataValidation.TruncateString(post.payment_token, 50);
            post.payment_status = AnnytabDataValidation.TruncateString(post.payment_status, 50);
            post.order_status = AnnytabDataValidation.TruncateString(post.order_status, 50);
            post.desired_date_of_delivery = AnnytabDataValidation.TruncateDateTime(post.desired_date_of_delivery);
            post.discount_code = AnnytabDataValidation.TruncateString(post.discount_code, 50);

            // Get the saved post
            Order savedPost = Order.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Order.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Update the exported to erp flag
        // PUT api/orders/set_as_exported
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpGet]
        public HttpResponseMessage set_as_exported(Int32 id = 0)
        {
            // Set the order as exported to erp
            Order.SetAsExportedToErp(id);

            // Create the response to return
            HttpResponseMessage response = Request.CreateResponse<string>(HttpStatusCode.OK, "Order has been updated");

            // Return the response
            return response;

        } // End of the set_as_exported method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/orders/get_count_by_search?keywords=one%20two
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
            Int32 count = Order.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        // Get the count of posts by customer id
        // GET api/orders/get_count_by_customer_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Int32 get_count_by_customer_id(Int32 id = 0)
        {
            // Get the count
            Int32 count = Order.GetCountByCustomerId(id);

            // Return the count
            return count;

        } // End of the get_count_by_customer_id method

        // Get the count of posts by a discount code id
        // GET api/orders/get_count_by_discount_code/SSSWWWW
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Int32 get_count_by_discount_code(string id = "")
        {
            // Get the count
            Int32 count = Order.GetCountByDiscountCode(id);

            // Return the count
            return count;

        } // End of the get_count_by_discount_code method

        #endregion

        #region Get methods

        // Get one order by id
        // GET api/orders/get_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Order get_by_id(Int32 id = 0)
        {
            // Create the post to return
            Order post = Order.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get one order by discount code and customer id
        // GET api/orders/get_by_discount_code_and_customer/SSSWWW?customerId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Order get_by_discount_code_and_customer(string id = "", Int32 customerId = 0)
        {
            // Create the post to return
            Order post = Order.GetOneByDiscountCodeAndCustomerId(id, customerId);

            // Return the post
            return post;

        } // End of the get_by_discount_code_and_customer method

        // Get all the orders that not are exported
        // GET api/orders/get_not_exported
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Order> get_not_exported(string sortField = "", string sortOrder = "")
        {
            // Get all the posts
            List<Order> posts = Order.GetNotExportedToErp(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_not_exported method

        // Get all the orders that not are exported
        // GET api/orders/get_not_exported_by_company_id/1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Order> get_not_exported_by_company_id(Int32 id = 0, string sortField = "", string sortOrder = "")
        {
            // Get all the posts
            List<Order> posts = Order.GetNotExportedToErp(id, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_not_exported_by_company_id method

        // Get all the orders
        // GET api/orders/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Order> get_all(string sortField = "", string sortOrder = "")
        {
            // Get all the posts
            List<Order> posts = Order.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/orders/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Order> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Order> posts = Order.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        // Get orders by customer id
        // GET api/orders/get_by_customer_id/5?pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Order> get_by_customer_id(Int32 id = 0, Int32 pageSize = 0, Int32 pageNumber = 0,
            string sortField = "", string sortOrder = "")
        {
            // Get all the posts
            List<Order> posts = Order.GetByCustomerId(id, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_customer_id method

        // Get orders by discount code
        // GET api/orders/get_by_discount_code/SSSWWWW?pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Order> get_by_discount_code(string id = "", Int32 pageSize = 0, Int32 pageNumber = 0,
            string sortField = "", string sortOrder = "")
        {
            // Get all the posts
            List<Order> posts = Order.GetByDiscountCode(id, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_discount_code method

        #endregion

        #region Delete methods

        // Delete a order
        // DELETE api/orders/delete
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = Order.DeleteOnId(id);

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
