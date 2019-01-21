
public class FacebookAuthorization
{
    #region Variables

    public string access_token { get; set; }
    public string token_type { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post
    /// </summary>
    public FacebookAuthorization()
    {
        // Set values for instance variables
        this.access_token = null;
        this.token_type = null;

    } // End of the constructor

    #endregion

} // End of the class