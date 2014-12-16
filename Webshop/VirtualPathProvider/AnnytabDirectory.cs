using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Collections;

/// <summary>
/// This class represent a virtual directory
/// </summary>
public class AnnytabDirectory : VirtualDirectory
{
    #region Variables

    public AnnytabPathProvider pathProvider;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new annytab directory
    /// </summary>
    /// <param name="virtualDirectory">The virtual directory as a string</param>
    /// <param name="pathProvider">A reference to the pathProvider</param>
    public AnnytabDirectory(string virtualDirectory, AnnytabPathProvider pathProvider)
        : base(virtualDirectory)
    {
        // Set values for instance variables
        this.pathProvider = pathProvider;

    } // End of the constructor

    #endregion

    #region Overrides

    private ArrayList children = new ArrayList();
    public override IEnumerable Children { get { return children; } }
    private ArrayList directories = new ArrayList();
    public override IEnumerable Directories { get { return directories; } }
    private ArrayList files = new ArrayList();
    public override IEnumerable Files { get { return files; } }

    #endregion

} // End of the class