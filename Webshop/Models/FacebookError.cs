using System;

/// <summary>
/// Facebook error
/// </summary>
public class FacebookErrorRoot
{
    #region Variables

    public FacebookError error { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post
    /// </summary>
    public FacebookErrorRoot()
    {
        // Set values for instance variables
        this.error = null;

    } // End of the constructor

    #endregion

} // End of the class

/// <summary>
/// Facebook error
/// </summary>
public class FacebookError
{
    #region Variables

    public string message { get; set; }
    public string type { get; set; }
    public Int32? code { get; set; }
    public Int32? error_subcode { get; set; }
    public string fbtrace_id { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post
    /// </summary>
    public FacebookError()
    {
        this.message = null;
        this.type = null;
        this.code = null;
        this.error_subcode = null;
        this.fbtrace_id = null;

    } // End of the constructor

    #endregion

} // End of the class