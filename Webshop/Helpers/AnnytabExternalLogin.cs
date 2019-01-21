using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

/// <summary>
/// This class includes helper methods for external logins
/// </summary>
public static class AnnytabExternalLogin
{
    /// <summary>
    /// Get a facebook user
    /// </summary>
    public async static Task<FacebookUser> GetFacebookUser(Domain current_domain, string code)
    {
        // Create variables
        FacebookAuthorization facebook_authorization = null;
        FacebookUser facebook_user = null;

        // Get a static http client
        HttpClient client = DefaultHttpClient.Get();

        // Create a request message
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.facebook.com/oauth/access_token?client_id=" + current_domain.facebook_app_id + "&redirect_uri="
            + current_domain.web_address + "/customer/facebook_login_callback" + "&client_secret=" + current_domain.facebook_app_secret + "&code=" + code);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Gzip"));
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Deflate"));

        // Get the response
        HttpResponseMessage response = await client.SendAsync(request);

        // Make sure that the response is successful
        if (response.IsSuccessStatusCode)
        {
            // Get facebook authorization
            facebook_authorization = JsonConvert.DeserializeObject<FacebookAuthorization>(await response.Content.ReadAsStringAsync());
        }
        else
        {
            // Get an error
            FacebookErrorRoot root = JsonConvert.DeserializeObject<FacebookErrorRoot>(await response.Content.ReadAsStringAsync());
        }

        // Make sure that facebook authorization not is null
        if (facebook_authorization == null)
        {
            return null;
        }

        // Create a request message with a modified url
        request = new HttpRequestMessage(HttpMethod.Get, "https://graph.facebook.com/me?fields=id,name&access_token=" + facebook_authorization.access_token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Gzip"));
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Deflate"));

        // Get the response
        response = await client.SendAsync(request);

        // Make sure that the response is successful
        if (response.IsSuccessStatusCode == true)
        {
            // Get a facebook user
            facebook_user = JsonConvert.DeserializeObject<FacebookUser>(await response.Content.ReadAsStringAsync());
        }
        else
        {
            // Get an error
            FacebookErrorRoot root = JsonConvert.DeserializeObject<FacebookErrorRoot>(await response.Content.ReadAsStringAsync());
        }

        // Return a facebook user
        return facebook_user;

    } // End of the GetFacebookUser method

    /// <summary>
    /// Get a google user
    /// </summary>
    public async static Task<GoogleUser> GetGoogleUser(Domain domain, string code)
    {
        // Create variables
        HttpRequestMessage request = null;
        HttpResponseMessage response = null;
        GoogleAuthorization google_authorization = null;
        GoogleUser google_user = null;

        // Get a static http client
        HttpClient client = DefaultHttpClient.Get();

        // Create a dictionary with data
        Dictionary<string, string> input = new Dictionary<string, string>(10);
        input.Add("code", code);
        input.Add("client_id", domain.google_app_id);
        input.Add("client_secret", domain.google_app_secret);
        input.Add("redirect_uri", domain.web_address + "/customer/google_login_callback");
        input.Add("grant_type", "authorization_code");

        // Use form data content
        using (FormUrlEncodedContent content = new FormUrlEncodedContent(input))
        {
            // Create a request message
            request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.google.com/o/oauth2/token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Gzip"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Deflate"));
            request.Content = content;

            // Get a response
            response = await client.SendAsync(request);

            // Make sure that the response is successful
            if (response.IsSuccessStatusCode == true)
            {
                // Get google authorization
                google_authorization = JsonConvert.DeserializeObject<GoogleAuthorization>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                // Get error information
                string data = await response.Content.ReadAsStringAsync();
            }
        }
            
        // Make sure that google authorization not is null
        if(google_authorization == null)
        {
            return null;
        }

        // Create a request message
        request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/plus/v1/people/me?key=" + domain.google_app_id);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", google_authorization.access_token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Gzip"));
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("Deflate"));

        // Get a response
        response = await client.SendAsync(request);

        // Make sure that the response is successful
        if (response.IsSuccessStatusCode == true)
        {
            // Get a google user
            google_user = JsonConvert.DeserializeObject<GoogleUser>(await response.Content.ReadAsStringAsync());
        }
        else
        {
            // Get error information
            string data = await response.Content.ReadAsStringAsync();
        }

        // Return a google user
        return google_user;

    } // End of the GetGoogleUser method

} // End of the class