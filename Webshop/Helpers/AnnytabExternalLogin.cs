using System.Collections.Generic;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// This class includes helper methods for external logins
/// </summary>
public static class AnnytabExternalLogin
{
    /// <summary>
    /// Get a facebook access token
    /// </summary>
    /// <param name="domain">A reference to the current domain</param>
    /// <param name="code">The code to exchange for an acess token</param>
    /// <returns>The access token as a string</returns>
    public async static Task<string> GetFacebookAccessToken(Domain domain, string code)
    {
        // Create a string to get the access token
        string accessString = "";

        // Create the url
        string url = "https://graph.facebook.com/oauth/access_token?client_id=" + domain.facebook_app_id + "&redirect_uri="
            + HttpContext.Current.Server.UrlEncode(domain.web_address + "/customer/facebook_login_callback") + "&client_secret=" 
            + domain.facebook_app_secret + "&code=" + code;

        // Create a http client
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Get the post
        HttpResponseMessage response = await client.GetAsync(url);

        // Make sure that the response is successful
        if (response.IsSuccessStatusCode)
        {
            accessString = await response.Content.ReadAsStringAsync();
        }

        // Dispose of the client
        client.Dispose();

        // Convert the string to a json object
        JObject token = AnnytabDataValidation.GetJsonObject(accessString);

        // Get the access token
        string access_token = token != null ? AnnytabDataValidation.GetJsonString(token["access_token"]) : "";

        // Return the access token
        return access_token;

    } // End of the GetFacebookAccessToken method

    /// <summary>
    /// Get a facebook user as a dictionary
    /// </summary>
    /// <param name="domain">A reference to the current domain</param>
    /// <param name="access_token">The access token</param>
    /// <returns>A dictionary with user information</returns>
    public async static Task<Dictionary<string, object>> GetFacebookUser(Domain domain, string access_token)
    {
        // Create a dictionary to return
        Dictionary<string, object> facebookUser = new Dictionary<string, object>();

        // Create the new url
        string url = "https://graph.facebook.com/me?access_token=" + access_token;

        // Create a http client
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Get the facebook user
        HttpResponseMessage response = await client.GetAsync(url);

        // Make sure that the response is successful
        if (response.IsSuccessStatusCode)
        {
            facebookUser = await response.Content.ReadAsAsync<Dictionary<string, object>>();
        }

        // Dispose of the client
        client.Dispose();

        // Return the facebook user
        return facebookUser;

    } // End of the GetFacebookUser method

    /// <summary>
    /// Get a google access token
    /// </summary>
    /// <param name="domain">A reference to the current domain</param>
    /// <param name="code">The code to exchange for an acess token</param>
    /// <returns>The access token as a string</returns>
    public async static Task<string> GetGoogleAccessToken(Domain domain, string code)
    {
        // Create a http client
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Create a dictionary with data
        Dictionary<string, string> data = new Dictionary<string, string>(10);
        data.Add("code", code);
        data.Add("client_id", domain.google_app_id);
        data.Add("client_secret", domain.google_app_secret);
        data.Add("redirect_uri", domain.web_address + "/customer/google_login_callback");
        data.Add("grant_type", "authorization_code");

        // Create the content
        HttpContent content = new FormUrlEncodedContent(data);

        // Post the data
        HttpResponseMessage response = await client.PostAsync("https://accounts.google.com/o/oauth2/token", content);

        // Create a string for the response data
        string responseData = "";

        // Make sure that the response is successful
        if (response.IsSuccessStatusCode == true)
        {
            // Get the response data
            responseData = await response.Content.ReadAsStringAsync();
        }

        // Close the client
        client.Dispose();

        // Convert the json response to a dictionary
        Dictionary<string, object> googleData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData);

        // Get the access token
        string access_token = googleData.ContainsKey("access_token") == true ? googleData["access_token"].ToString() : "";

        // Return the access token
        return access_token;

    } // End of the GetGoogleAccessToken method

    /// <summary>
    /// Get a google user as a dictionary
    /// </summary>
    /// <param name="domain">A reference to the current domain</param>
    /// <param name="access_token">The access token</param>
    /// <returns>A dictionary with user information</returns>
    public async static Task<Dictionary<string, object>> GetGoogleUser(Domain domain, string access_token)
    {
        // Create the dictionary to return
        Dictionary<string, object> googleUser = new Dictionary<string, object>();

        // Create a http client
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Get the post
        HttpResponseMessage response = await client.GetAsync("https://www.googleapis.com/plus/v1/people/me?key=" + domain.google_app_id);

        // Make sure that the response is successful
        if (response.IsSuccessStatusCode)
        {
            googleUser = await response.Content.ReadAsAsync<Dictionary<string, object>>();
        }

        // Close the client
        client.Dispose();

        // Return the google user
        return googleUser;

    } // End of the GetGoogleUser method

} // End of the class