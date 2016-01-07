using System.Web.Hosting;
using System.IO;

/// <summary>
/// This class represent a virtual file
/// </summary>
public class AnnytabFile : VirtualFile
{

    #region Variables

    public AnnytabPathProvider pathProvider;
    public string virtualPath;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new annytab file
    /// </summary>
    /// <param name="virtualPath">The virtual path as a string</param>
    /// <param name="pathProvider">A reference to a path provider</param>
    public AnnytabFile(string virtualPath, AnnytabPathProvider pathProvider)
        :base(virtualPath)
    {
        // Set values for instance variables
        this.pathProvider = pathProvider;
        this.virtualPath = virtualPath;

    } // End of the constructor

    #endregion

    #region Get methods

    /// <summary>
    /// Open the content of the file
    /// </summary>
    /// <returns>A reference to a stream</returns>
    public override Stream Open()
    {
        // Get the contents of the file
        string fileContents = pathProvider.GetFileContents(this.virtualPath);
        
        // Create a new memory stream
        Stream stream = new MemoryStream();

        // Make sure that there is contents in the file
        if (fileContents != null)
        {
            // Put the page content on the stream
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(fileContents);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
        }

        // Return the stream
        return stream;

    } // End of the Open method

    #endregion
       
} // End of the class