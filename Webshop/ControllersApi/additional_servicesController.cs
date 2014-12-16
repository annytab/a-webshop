using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for additional services
    /// </summary>
    public class additional_servicesController : ApiController
    {

        #region Insert methods

        // Add a additional service
        // POST api/additional_services/add?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(AdditionalService post, Int32 languageId = 0)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(languageId) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (Unit.MasterPostExists(post.unit_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The unit does not exist");
            }
            else if (ValueAddedTax.MasterPostExists(post.value_added_tax_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The value added tax does not exist");
            }

            // Make sure that the data is valid
            post.product_code = AnnytabDataValidation.TruncateString(post.product_code, 50);
            post.name = AnnytabDataValidation.TruncateString(post.name, 100);
            post.fee = AnnytabDataValidation.TruncateDecimal(post.fee, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);

            // Add the post
            Int64 insertId = AdditionalService.AddMasterPost(post);
            post.id = Convert.ToInt32(insertId);
            AdditionalService.AddLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a additional service
        // PUT api/additional_services/update?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(AdditionalService post, Int32 languageId = 0)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(languageId) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (Unit.MasterPostExists(post.unit_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The unit does not exist");
            }
            else if (ValueAddedTax.MasterPostExists(post.value_added_tax_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The value added tax does not exist");
            }

            // Make sure that the data is valid
            post.product_code = AnnytabDataValidation.TruncateString(post.product_code, 50);
            post.name = AnnytabDataValidation.TruncateString(post.name, 100);
            post.fee = AnnytabDataValidation.TruncateDecimal(post.fee, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);

            // Get the saved post
            AdditionalService savedPost = AdditionalService.GetOneById(post.id, languageId);

            // Check if the service exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            AdditionalService.UpdateMasterPost(post);
            AdditionalService.UpdateLanguagePost(post, languageId);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        // Translate a additional service
        // PUT api/additional_services/translate?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage translate(AdditionalService post, Int32 languageId = 0)
        {
            // Check for errors
            if (post == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post is null");
            }
            else if (Language.MasterPostExists(languageId) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The language does not exist");
            }
            else if (AdditionalService.MasterPostExists(post.id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The additional service does not exist");
            }           
            else if (ValueAddedTax.MasterPostExists(post.value_added_tax_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The value added tax does not exist");
            }

            // Make sure that the data is valid
            post.product_code = AnnytabDataValidation.TruncateString(post.product_code, 50);
            post.name = AnnytabDataValidation.TruncateString(post.name, 100);
            post.fee = AnnytabDataValidation.TruncateDecimal(post.fee, 0, 9999999999.99M);
            post.account_code = AnnytabDataValidation.TruncateString(post.account_code, 10);

            // Get the saved post
            AdditionalService savedPost = AdditionalService.GetOneById(post.id, languageId);

            // Check if we should add or update the post
            if (savedPost == null)
            {
                AdditionalService.AddLanguagePost(post, languageId);
            }
            else
            {
                AdditionalService.UpdateLanguagePost(post, languageId);
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The translation was successful");

        } // End of the translate method

        #endregion

        #region Count methods

        // Get the count of posts by a search
        // GET api/additional_services/get_count_by_search?keywords=one%20two&languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public Int32 get_count_by_search(string keywords = "", Int32 languageId = 0)
        {
            // Create the string array
            string[] wordArray = new string[] {""};

            // Recreate the array if keywords is different from null
            if (keywords != null)
            {
                wordArray = keywords.Split(' ');
            }

            // Get the count
            Int32 count = AdditionalService.GetCountBySearch(wordArray, languageId);

            // Return the count
            return count;

        } // End of the get_count_by_search method

        #endregion

        #region Get methods

        // Get a additional service by id
        // GET api/additional_services/get_by_id/5?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public AdditionalService get_by_id(Int32 id = 0, Int32 languageId = 0)
        {
            // Create the post to return
            AdditionalService post = AdditionalService.GetOneById(id, languageId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get all additional services
        // GET api/additional_services/get_all?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<AdditionalService> get_all(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<AdditionalService> posts = AdditionalService.GetAll(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all method

        // Get all the active additional services
        // GET api/additional_services/get_all_active?languageId=1
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<AdditionalService> get_all_active(Int32 languageId = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<AdditionalService> posts = AdditionalService.GetAllActive(languageId, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_all_active method

        // Get posts by a search
        // GET api/additional_services/get_by_search?keywords=one%20two&languageId=1&pageSize=10&pageNumber=2&sortField=id&sortOrder=ASC
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<AdditionalService> get_by_search(string keywords = "", Int32 languageId = 0, Int32 pageSize = 0, Int32 pageNumber = 0, 
            string sortField = "", string sortOrder = "")
        {
            // Create the string array
            string[] wordArray = new string[] {""};

            // Recreate the array if keywords is different from null
            if (keywords != null)
            {
                wordArray = keywords.Split(' ');
            }

            // Create the list to return
            List<AdditionalService> posts = AdditionalService.GetBySearch(wordArray, languageId, pageSize, pageNumber, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_search method

        #endregion

        #region Delete methods

        // Delete a additional service post
        // DELETE api/additional_services/delete/1?languageId=5
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, Int32 languageId = 0)
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the master post or just a language post
            if (languageId == 0)
            {
                errorCode = AdditionalService.DeleteOnId(id);
            }
            else
            {
                errorCode = AdditionalService.DeleteLanguagePostOnId(id, languageId);
            }

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
