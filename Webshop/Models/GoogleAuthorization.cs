/// <summary>
/// This class represent google authorization
/// </summary>
public class GoogleAuthorization
{
    #region Variables

    public string access_token { get; set; }
    public string expires_in { get; set; }
    public string refresh_token { get; set; }
    public string scope { get; set; }
    public string token_type { get; set; }
    public string id_token { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post
    /// </summary>
    public GoogleAuthorization()
    {
        // Set values for instance variables
        this.access_token = null;
        this.expires_in = null;
        this.refresh_token = null;
        this.scope = null;
        this.token_type = null;
        this.id_token = null;

    } // End of the constructor

    #endregion

} // End of the class