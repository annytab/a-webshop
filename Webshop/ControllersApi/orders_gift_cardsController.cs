using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for orders gift cards
    /// </summary>
    public class orders_gift_cardsController : ApiController
    {
        #region Insert methods

        // Add a order gift card post
        // POST api/orders_gift_cards/add
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpPost]
        public HttpResponseMessage add(OrderGiftCard post)
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
            else if (GiftCard.MasterPostExists(post.gift_card_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The gift card does not exist");
            }

            // Make sure that the data is valid
            post.gift_card_id = AnnytabDataValidation.TruncateString(post.gift_card_id, 50);
            post.amount = AnnytabDataValidation.TruncateDecimal(post.amount, 0, 999999999999M);

            // Check if the order gift card exists
            if (OrderGiftCard.GetOneById(post.order_id, post.gift_card_id) != null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The post already exists");
            }

            // Add the post
            OrderGiftCard.Add(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been added");

        } // End of the add method

        #endregion

        #region Update

        // Update a order gift card post
        // PUT api/orders_gift_cards/update
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST")]
        [HttpPut]
        public HttpResponseMessage update(OrderGiftCard post)
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
            else if (GiftCard.MasterPostExists(post.gift_card_id) == false)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The gift card does not exist");
            }

            // Make sure that the data is valid
            post.gift_card_id = AnnytabDataValidation.TruncateString(post.gift_card_id, 50);
            post.amount = AnnytabDataValidation.TruncateDecimal(post.amount, 0, 999999999999M);

            // Get the saved post
            OrderGiftCard savedPost = OrderGiftCard.GetOneById(post.order_id, post.gift_card_id);

            // Check if the post exists
            if (savedPost == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The record does not exist");
            }

            // Update the post
            OrderGiftCard.Update(post);

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The update was successful");

        } // End of the update method

        #endregion

        #region Get methods

        // Get a order gift card by id
        // GET api/orders_gift_cards/get_by_id/1?giftCardId=FDFSDFSDF
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public OrderGiftCard get_by_id(Int32 id = 0, string giftCardId = "")
        {
            // Create the post to return
            OrderGiftCard post = OrderGiftCard.GetOneById(id, giftCardId);

            // Return the post
            return post;

        } // End of the get_by_id method

        // Get order gift cards by order id
        // GET api/orders_gift_cards/get_by_order_id/5
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<OrderGiftCard> get_by_order_id(Int32 id = 0, string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<OrderGiftCard> posts = OrderGiftCard.GetByOrderId(id, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_order_id method

        // Get order gift cards by gift card id
        // GET api/orders_gift_cards/get_by_gift_card_id/SSSEEGGG
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<OrderGiftCard> get_by_gift_card_id(string id = "", string sortField = "", string sortOrder = "")
        {
            // Create the list to return
            List<OrderGiftCard> posts = OrderGiftCard.GetByGiftCardId(id, sortField, sortOrder);

            // Return the list
            return posts;

        } // End of the get_by_gift_card_id method

        // Get all order gift cards
        // GET api/orders_gift_cards/get_all
        [ApiAuthorize(Roles = "API_FULL_TRUST,API_MEDIUM_TRUST,API_MINIMAL_TRUST")]
        [HttpGet]
        public List<OrderGiftCard> get_all()
        {
            // Create the list to return
            List<OrderGiftCard> posts = OrderGiftCard.GetAll();

            // Return the list
            return posts;

        } // End of the get_all method

        #endregion

        #region Delete methods

        // Delete a order gift card
        // DELETE api/orders_gift_cards/delete/1?giftCardId=2
        [ApiAuthorize(Roles = "API_FULL_TRUST")]
        [HttpDelete]
        public HttpResponseMessage delete(Int32 id = 0, string giftCardId = "")
        {
            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the post
            errorCode = OrderGiftCard.DeleteOnId(id, giftCardId);

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
