using System;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Controllers;

/// <summary>
/// This class handles the authorization for the api
/// </summary>
public class ApiAuthorizeAttribute : AuthorizeAttribute
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
            // Get the allowed roles
            string[] allowedRoles = this.Roles.Split(',');

            string authToken = actionContext.Request.Headers.Authorization.Parameter;
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
            
            // Get the user name and password
            string userName = decodedToken.Substring(0, decodedToken.IndexOf(":"));
            string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

            // Check the user name, password and role
            if (CheckPasswordAndRole(userName, password, allowedRoles) == false)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }

    } // End of the OnAuthorization method

    /// <summary>
    /// Validate the user name, password and role
    /// </summary>
    /// <param name="username">The user name</param>
    /// <param name="password">The password</param>
    /// <param name="allowedRoles">The allowed roles as a string array</param>
    /// <returns>A boolean that indicates if the credentials is valid</returns>
    private static bool CheckPasswordAndRole(string username, string password, string[] allowedRoles)
    {
        // Create the boolean to return
        bool isValid = false;

        // Get the user
        Administrator apiUser = Administrator.GetOneByUserName(username);

        // Check if the user is authorized 
        if(apiUser != null && Administrator.ValidatePassword(username, password) == true)
        {
            // Check if the user has the correct role
            for (int i = 0; i < allowedRoles.Length; i++)
            {
                if (allowedRoles[i] == apiUser.admin_role)
                {
                    isValid = true;
                }
            } 
        }

        // Return the validation boolean
        return isValid;

    } // End of the CheckPasswordAndRole method

    #endregion

} // End of the class