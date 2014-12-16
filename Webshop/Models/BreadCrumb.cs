using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class represent a bread crumb
/// </summary>
public class BreadCrumb
{
    #region variables

    public string name;
    public string link;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new bread crumb with default properties
    /// </summary>
    public BreadCrumb()
    {
        // Set values for instance variables
        this.name = "";
        this.link = "";

    } // End of the constructor

    /// <summary>
    /// Create a new bread crumb with your properties
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="link">The link</param>
    public BreadCrumb(string name, string link)
    {
        // Set values for instance variables
        this.name = name;
        this.link = link;

    } // End of the constructor

    #endregion

} // End of the class