using System;

/// <summary>
/// This class represent a google user
/// </summary>
public class GoogleUser
{
    #region Variables

    public string kind { get; set; }
    public string etag { get; set; }
    public string objectType { get; set; }
    public string id { get; set; }
    public string displayName { get; set; }
    public GoogleName name { get; set; }
    public GoogleImage image { get; set; }
    public bool? isPlusUser { get; set; }
    public Int32? circledByCount { get; set; }
    public bool? verified { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post
    /// </summary>
    public GoogleUser()
    {
        // Set values for instance variables
        this.kind = null;
        this.etag = null;
        this.objectType = null;
        this.id = null;
        this.displayName = null;
        this.name = null;
        this.image = null;
        this.isPlusUser = null;
        this.circledByCount = null;
        this.verified = null;

    } // End of the constructor

    #endregion

} // End of the class

/// <summary>
/// This class represents a google name
/// </summary>
public class GoogleName
{
    #region Variables

    public string familyName { get; set; }
    public string givenName { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post
    /// </summary>
    public GoogleName()
    {
        // Set values for instance variables
        this.familyName = null;
        this.givenName = null;

    } // End of the constructor

    #endregion

} // End of the class

/// <summary>
/// This class represents a google image
/// </summary>
public class GoogleImage
{
    #region Variables

    public string url { get; set; }
    public bool? isDefault { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post
    /// </summary>
    public GoogleImage()
    {
        // Set values for instance variables
        this.url = null;
        this.isDefault = null;

    } // End of the constructor

    #endregion

} // End of the class