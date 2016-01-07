using System.Net.Http;
using System.Net.Http.Headers;

/// <summary>
/// This class represent a multipart form data stream provider to upload files from the web api
/// </summary>
public class AnnytabMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
{
    #region Variables

    public string filename;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new multipart form data stream provider
    /// </summary>
    /// <param name="rootPath">The root path</param>
    /// <param name="filename">The filename</param>
    public AnnytabMultipartFormDataStreamProvider(string rootPath, string filename)
        : base(rootPath)
    {
        // Set values for instance variables
        this.filename = filename;

    } // End of the constructor

    #endregion

    #region Methods

    /// <summary>
    /// Get the name of the file
    /// </summary>
    /// <param name="headers"></param>
    /// <returns></returns>
    public override string GetLocalFileName(HttpContentHeaders headers)
    {
        // Create the string to return
        string name = "no_name";

        // Set the filename
        if(this.filename != "")
        {
            name = this.filename;
        }
        else if(string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) == false)
        {
            name = headers.ContentDisposition.FileName;
            name = name.Replace("\"", string.Empty);
        }

        // Return the filename
        return name;

    } // End of the GetLocalFileName method

    #endregion

} // End of the class
