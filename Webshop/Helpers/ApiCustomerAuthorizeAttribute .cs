using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Controllers;

/// <summary>
/// This class handles the customer authorization for the api
/// </summary>
public class ApiCustomerAuthorizeAttribute : AuthorizeAttribute
{
    #region Methods

    /// <summary>
    /// This method checks if the request is authorized
    /// </summary>
    /// <param name="actionContext">A reference to the action context</param>
    public override void OnAuthorization(HttpActionContext actionContext)
    {

        // Check if the authorization header exists
        if (actionContext.Request.Headers.Authorization == null)
        {
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        }
        else
        {
            // Get the authorization token
            string authToken = actionContext.Request.Headers.Authorization.Parameter;
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
            
            // Get the email and password
            string email = decodedToken.Substring(0, decodedToken.IndexOf(":"));
            string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

            // Check the email and password
            if (CheckEmailAndPassword(email, password) == false)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }

    } // End of the OnAuthorization method

    /// <summary>
    /// Validate the email and password
    /// </summary>
    /// <param name="email">The email</param>
    /// <param name="password">The password</param>
    /// <returns>A boolean that indicates if the credentials is valid</returns>
    private static bool CheckEmailAndPassword(string email, string password)
    {
        // Create the boolean to return
        bool isValid = false;

        // Get the customer
        Customer customer = Customer.GetOneByEmail(email);

        // Check if the user is authorized
        if (customer != null && Customer.ValidatePassword(email, password) == true)
        {
            isValid = true;
        }

        // Return the validation boolean
        return isValid;

    } // End of the CheckEmailAndPassword method

    #endregion

} // End of the class