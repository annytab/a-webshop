using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for customers
    /// </summary>
    public class customersController : ApiController
    {
        #region Insert methods

        // Add a customer
        // POST api/customers/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(Customer post)
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
            else if (Country.MasterPostExists(post.invoice_country) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The invoice country does not exist");
            }
            else if (Country.MasterPostExists(post.delivery_country) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The delivery country does not exist");
            }

            // Make sure that the data is valid
            post.email = AnnytabDataValidation.TruncateString(post.email, 100);
            post.customer_password = PasswordHash.CreateHash(post.customer_password);
            post.org_number = AnnytabDataValidation.TruncateString(post.org_number, 20);
            post.vat_number = AnnytabDataValidation.TruncateString(post.vat_number, 20);
            post.contact_name = AnnytabDataValidation.TruncateString(post.contact_name, 100);
            post.phone_number = AnnytabDataValidation.TruncateString(post.phone_number, 100);
            post.mobile_phone_number = AnnytabDataValidation.TruncateString(post.mobile_phone_number, 100);
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
            post.facebook_user_id = AnnytabDataValidation.TruncateString(post.facebook_user_id, 50);
            post.google_user_id = AnnytabDataValidation.TruncateString(post.google_user_id, 50);

            // Get a customer on the email address
            Customer customerOnEmail = Customer.GetOneByEmail(post.email);

            // Check if the email exists
            if (customerOnEmail != null && post.id != customerOnEmail.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The email is not unique");
            }

            // Add the post
            Int32 insertId = (Int32)Customer.Add(post);

            // Update the password
            Customer.UpdatePassword(insertId, post.customer_password);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a customer
        // PUT api/customers/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(Customer post)
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
            else if (Country.MasterPostExists(post.invoice_country) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The invoice country does not exist");
            }
            else if (Country.MasterPostExists(post.delivery_country) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The delivery country does not exist");
            }

            // Make sure that the data is valid
            post.email = AnnytabDataValidation.TruncateString(post.email, 100);
            post.org_number = AnnytabDataValidation.TruncateString(post.org_number, 20);
            post.vat_number = AnnytabDataValidation.TruncateString(post.vat_number, 20);
            post.contact_name = AnnytabDataValidation.TruncateString(post.contact_name, 100);
            post.phone_number = AnnytabDataValidation.TruncateString(post.phone_number, 100);
            post.mobile_phone_number = AnnytabDataValidation.TruncateString(post.mobile_phone_number, 100);
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
            post.facebook_user_id = AnnytabDataValidation.TruncateString(post.facebook_user_id, 50);
            post.google_user_id = AnnytabDataValidation.TruncateString(post.google_user_id, 50);

            // Get a customer on the email address
            Customer customerOnEmail = Customer.GetOneByEmail(post.email);

            // Check if the email exists
            if (customerOnEmail != null && post.id != customerOnEmail.id)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The email is not unique");
            }

            // Get the saved post
            Customer savedPost = Customer.GetOneById(post.id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            Customer.Update(post);

            // Only update the password if it has changed
            if(post.customer_password != "")
            {
                // Update the password
                Customer.UpdatePassword(post.id, PasswordHash.CreateHash(post.customer_password));
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/customers/get_count_by_search?keywords=one%20two
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
            Int32 count = Customer.GetCountBySearch(wordArray);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a customer by id
        // GET api/customers/get_by_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpGet]
        public Customer get_by_id(Int32 id = 0)
        {
            // Create the post to return
            Customer post = Customer.GetOneById(id);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all customers
        // GET api/customers/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpGet]
        public List<Customer> get_all(string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<Customer> posts = Customer.GetAll(sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get posts by a search
        // GET api/customers/get_by_search?keywords=one%20two&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<Customer> get_by_search(string keywords = "", Int32 pageSize = 0, Int32 pageNumber = 0,
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
            List<Customer> posts = Customer.GetBySearch(wordArray, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a customer
        // DELETE api/customers/delete/5
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = Customer.DeleteOnId(id);

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
