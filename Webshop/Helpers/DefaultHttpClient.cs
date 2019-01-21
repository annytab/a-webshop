using System;
using System.Net;
using System.Net.Http;

/// <summary>
/// Summary description for DoxservrClient
/// </summary>
public sealed class DefaultHttpClient
{
    #region Variables

    private static HttpClient client = null;
    private static object syncRoot = new Object();

    #endregion

    #region Constructors

    public DefaultHttpClient() { }

    #endregion

    #region Get methods

    /// <summary>
    /// Get a http client
    /// </summary>
    public static HttpClient Get()
    {
        if (client == null)
        {
            lock (syncRoot)
            {
                if (client == null)
                {
                    try
                    {
                        // Create a http client handler
                        HttpClientHandler handler = new HttpClientHandler
                        {
                            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                        };

                        // Create a http client
                        client = new HttpClient(handler);
                        client.Timeout = TimeSpan.FromSeconds(100);
                    }
                    catch (Exception ex)
                    {
                        string exMessage = ex.Message;
                    }
                }
            }
        }

        // Return a client instance
        return client;

    } // End of the Get method

    #endregion

} // End of the class