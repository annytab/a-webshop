using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.IO;

/// <summary>
/// This class represent a virtual path provider
/// </summary>
public class AnnytabPathProvider : VirtualPathProvider
{
    #region Variables

    private string _virtualThemeHash;
    private object lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new annytab path provider
    /// </summary>
    public AnnytabPathProvider()
        : base()
    {
        // Set values for instance variables

    } // End of the constructor

    #endregion

    #region Set and get the virtual theme hash

    /// <summary>
    /// Set the virtual theme hash (thread safe)
    /// </summary>
    private void SetVirtualThemeHash()
    {
        lock (lockObject)
        {
            _virtualThemeHash = Guid.NewGuid().ToString();
        }

    } // End of the SetVirtualThemeHash method

    /// <summary>
    /// Get the virtual theme hash (thread safe)
    /// </summary>
    /// <returns></returns>
    private string GetVirtualThemeHash()
    {
        lock (lockObject)
        {
            return _virtualThemeHash;
        }

    } // End of the GetVirtualThemeHash method

    #endregion

    #region Property methods

    /// <summary>
    /// This method is used to check if the path is virtual
    /// </summary>
    /// <param name="virtualPath">The virtual path as a string</param>
    /// <returns>A boolean that indicates if the path is virtual</returns>
    private bool IsPathVirtual(string virtualPath)
    {
        // Get the path relative to the application
        String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);

        if (checkPath.StartsWith("~/Views/theme") || checkPath.StartsWith("~/Content/theme") || checkPath.StartsWith("~/Scripts/theme"))
        {
            return true;
        }
        else
        {
            return false;
        }

    } // End of the IsPathVirtual method

    /// <summary>
    /// Check if the file exists
    /// </summary>
    /// <param name="virtualPath">The virtual path as a string</param>
    /// <returns>A boolean that indicates if the file exists</returns>
    public override bool FileExists(string virtualPath)
    {
        // Make sure that the path is virtual
        if (IsPathVirtual(virtualPath) == true)
        {
            // Determine whether the file exists on the virtual file system
            if (CheckIfFileExists(virtualPath) == true)
            {
                return true;
            }
            else
            {
                return Previous.FileExists(virtualPath);
            }
        }
        else
        {
            return Previous.FileExists(virtualPath);
        }

    } // End of the FileExists method

    /// <summary>
    /// Check if the virtual directory exists
    /// </summary>
    /// <param name="virtualDirectory">The virtual directory as a string</param>
    /// <returns>A boolean that indicates if the virtual directory exists</returns>
    public override bool DirectoryExists(string virtualDirectory)
    {
        // Check if the directory is virtual
        if (IsPathVirtual(virtualDirectory))
        {
            // Get the directory and return true
            return true;
        }
        else
        {
            return Previous.DirectoryExists(virtualDirectory);
        }

    } // End of the DirectoryExists method

    /// <summary>
    /// Check if the file exists
    /// </summary>
    /// <param name="virtualPath">The virtual path as a string</param>
    /// <returns>A boolean that indicates if the file exists</returns>
    private bool CheckIfFileExists(string virtualPath)
    {
        // Get the file name
        string fileName = Path.GetFileName(virtualPath);

        // Get the current domain
        Domain domain = Tools.GetCurrentDomain();

        // Create the theme id
        string themeId = "Theme_" + domain.custom_theme_id;

        // Get the virtual theme from cache
        Dictionary<string, string> virtualTheme = (Dictionary<string, string>)HttpContext.Current.Cache[themeId];

        // Check if the virtual theme is different from null
        if (virtualTheme == null)
        {
            // Create the virtual theme
            virtualTheme = CustomTheme.GetAllTemplatesById(domain.custom_theme_id);

            // Add the virtual theme to cache
            HttpContext.Current.Cache.Insert(themeId, virtualTheme, null, DateTime.UtcNow.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            SetVirtualThemeHash();
        }

        // Check if the file exists
        if (virtualTheme.ContainsKey(fileName) == true)
        {
            return true;
        }
        else
        {
            return false;
        }

    } // End of the CheckIfFileExists method

    #endregion

    #region Get methods

    /// <summary>
    /// Get the virtual file
    /// </summary>
    /// <param name="virtualPath">The virtual path as a string</param>
    /// <returns>A reference to a virtual file</returns>
    public override VirtualFile GetFile(string virtualPath)
    {
        if (IsPathVirtual(virtualPath))
            return new AnnytabFile(virtualPath, this);
        else
            return Previous.GetFile(virtualPath);

    } // End of the GetFile method

    /// <summary>
    /// Get the virtual directory
    /// </summary>
    /// <param name="virtualDirectory">The virtual directory as a string</param>
    /// <returns>A reference to a virtual directory</returns>
    public override VirtualDirectory GetDirectory(string virtualDirectory)
    {
        if (IsPathVirtual(virtualDirectory))
            return new AnnytabDirectory(virtualDirectory, this);
        else
            return Previous.GetDirectory(virtualDirectory);

    } // End of the GetDirectory method

    /// <summary>
    /// Get the content of a file as a string
    /// </summary>
    /// <param name="virtualPath">The virtual path as a string</param>
    /// <returns>The content of the file as a string</returns>
    public string GetFileContents(string virtualPath)
    {
        // Create the string to return
        string fileContent = "";

        // Get the file name
        string fileName = Path.GetFileName(virtualPath);

        // Get the current domain
        Domain domain = Tools.GetCurrentDomain();

        // Create the theme id
        string themeId = "Theme_" + domain.custom_theme_id;

        // Get the dictionary
        Dictionary<string, string> virtualFiles = (Dictionary<string, string>)HttpContext.Current.Cache[themeId];

        // Make sure that virtual files not is null
        if (virtualFiles != null)
        {
            fileContent = virtualFiles[fileName];
        }

        // Return the content
        return fileContent;

    } // End of the GetFileContents method

    /// <summary>
    /// Get the cache dependency
    /// </summary>
    public override CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
    {
        if (IsPathVirtual(virtualPath))
        {
            return null;
        }
        return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

    } // End of the GetCacheDependency method

    /// <summary>
    /// Get a file hash
    /// </summary>
    public override String GetFileHash(String virtualPath, System.Collections.IEnumerable virtualPathDependencies)
    {
        if (IsPathVirtual(virtualPath))
        {
            return GetVirtualThemeHash();
        }

        return Previous.GetFileHash(virtualPath, virtualPathDependencies);

    } // End of the GetFileHash method

    #endregion

} // End of the class