using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace Annytab.Webshop.ControllersApi
{
    /// <summary>
    /// This class controls the api for payment callbacks
    /// </summary>
    public class paymentsController : ApiController
    {
        #region Payson

        // Handle a payson callback
        // POST api/payments/payson_notification/5
        [HttpPost]
        public async Task<HttpResponseMessage> payson_notification(Int32 id = 0)
        {

            // Read the content
            string content = await Request.Content.ReadAsStringAsync();
            
            // Check for errors
            if (content == null || content == "")
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The content is null or empty");
            }

            // Get the webshop settings
            KeyStringList webshopSettings = WebshopSetting.GetAllFromCache();

            // Get the order
            Order order = Order.GetOneById(id);

            // Make sure that the orde exists
            if (order == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The order does not exist");
            }

            // Get credentials
            string userId = webshopSettings.Get("PAYSON-AGENT-ID");
            string md5Key = webshopSettings.Get("PAYSON-MD5-KEY");
            bool paysonTest = false;
            bool.TryParse(webshopSettings.Get("PAYSON-TEST"), out paysonTest);

            // Create the payson api
            PaysonIntegration.PaysonApi paysonApi = new PaysonIntegration.PaysonApi(userId, md5Key, null, paysonTest);

            // Validate the content
            PaysonIntegration.Response.ValidateResponse validateResponse = paysonApi.MakeValidateIpnContentRequest(content);

            // Check if the validation was successful
            if (validateResponse.Success)
            {
                // Get the data
                PaysonIntegration.Utils.PaymentType? paymentType = validateResponse.ProcessedIpnMessage.PaymentType;
                PaysonIntegration.Utils.PaymentStatus? paymentStatus = validateResponse.ProcessedIpnMessage.PaymentStatus;
                PaysonIntegration.Utils.InvoiceStatus? invoiceStatus = validateResponse.ProcessedIpnMessage.InvoiceStatus;

                // Check the payment type and payment status
                if (paymentType == PaysonIntegration.Utils.PaymentType.Direct && paymentStatus == PaysonIntegration.Utils.PaymentStatus.Completed)
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_paid");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if (paymentType == PaysonIntegration.Utils.PaymentType.Invoice && invoiceStatus == PaysonIntegration.Utils.InvoiceStatus.OrderCreated)
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(id, "payment_status_invoice_approved");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "The post has been processed");

        } // End of the payson_notification method

        #endregion

        #region PayEx

        // Handle a payex callback
        // POST api/payments/payex_callback
        [HttpPost]
        public async Task<HttpResponseMessage> payex_callback()
        {
            // Read the content
            string content = await Request.Content.ReadAsStringAsync();

            // Check for errors
            if (content == null || content == "")
            {
                return Request.CreateResponse<string>(HttpStatusCode.OK, "FAILURE");
            }

            // Convert the content to a name value collection
            NameValueCollection collection = System.Web.HttpUtility.ParseQueryString(content);

            // Get the data
            string orderRef = collection["orderRef"] != null ? collection["orderRef"] : "";

            // Complete the order
            Dictionary<string, string> response = PayExManager.CompleteOrder(orderRef);

            // Get response variables
            string error_code = response.ContainsKey("error_code") == true ? response["error_code"] : "";
            string transaction_status = response.ContainsKey("transaction_status") == true ? response["transaction_status"] : "";
            string transaction_number = response.ContainsKey("transaction_number") == true ? response["transaction_number"] : "";
            string payment_method = response.ContainsKey("payment_method") == true ? response["payment_method"] : "";
            bool alreadyCompleted = response.ContainsKey("already_completed") == true ? Convert.ToBoolean(response["already_completed"]) : false;
            Int32 order_id = 0;
            if (response.ContainsKey("order_id") == true)
            {
                Int32.TryParse(response["order_id"], out order_id);
            }

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the order
            Order order = Order.GetOneById(order_id);

            // Make sure that the order exists
            if(order == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "The order does not exist");
            }

            // Make sure that callback is accepted
            if (error_code == "OK")
            {
                // Save the transaction number
                Order.SetPaymentToken(order.id, transaction_number);

                // Get the payment option
                PaymentOption paymentOption = PaymentOption.GetOneById(order.payment_option, domain.back_end_language);

                if (paymentOption.connection == 403 && transaction_status == "3")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(order.id, "payment_status_invoice_approved");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if (paymentOption.connection == 402 && transaction_status == "0")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(order.id, "payment_status_paid");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if ((paymentOption.connection == 401 || paymentOption.connection == 404) && transaction_status == "0")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(order.id, "payment_status_paid");

                    // Add customer files
                    CustomerFile.AddCustomerFiles(order);
                }
                else if (paymentOption.connection == 403 && transaction_status != "5")
                {
                    // Update the order status
                    Order.UpdatePaymentStatus(order.id, "payment_status_invoice_not_approved");
                }
            }

            // Return the success response
            return Request.CreateResponse<string>(HttpStatusCode.OK, "OK");

        } // End of the payex_callback method

        #endregion

    } // End of the class

} // End of the namespace
