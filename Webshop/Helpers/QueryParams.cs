using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

/// <summary>
/// This class represents parameters
/// </summary>
public class QueryParams
{
    #region Variables

    public string keywords;
    public string sort_field;
    public string sort_order;
    public Int32 page_size;
    public Int32 current_page;

    #endregion

    #region Constructors

    /// <summary>
    /// Create new query parameters with default properties
    /// </summary>
    public QueryParams()
    {
        // Set values for instance variables
        this.keywords = "";
        this.sort_field = "";
        this.sort_order = "ASC";
        this.page_size = 10;
        this.current_page = 1;

    } // End of the constructor

    /// <summary>
    /// Create new query parameters from a request
    /// </summary>
    /// <param name="request">A reference to a request object</param>
    public QueryParams(HttpRequestBase request)
    {
        // Set values for instance variables
        this.keywords = "";
        this.sort_field = "";
        this.sort_order = "ASC";
        this.page_size = 10;
        this.current_page = 1;

        // Get the keywords
        if (request.Params["kw"] != null)
        {
            this.keywords = HttpContext.Current.Server.UrlDecode(request.Params["kw"]);
        }

        // Get the sort field
        if (request.Params["sf"] != null)
        {
            this.sort_field = request.Params["sf"];
        }

        // Get the sort order
        if (request.Params["so"] != null)
        {
            this.sort_order = request.Params["so"];
        }

        // Get the page size
        if (request.Params["pz"] != null)
        {
            try
            {
                this.page_size = Int32.Parse(request.Params["pz"]);
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
            }
        }

        // Get the page number
        if (request.Params["qp"] != null)
        {
            try
            {
                this.current_page = Int32.Parse(request.Params["qp"]);
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
                this.current_page = 1;
            }
        }

    } // End of the constructor

    /// <summary>
    /// Create new query parameters from a query string
    /// </summary>
    /// <param name="request">A reference to a request object</param>
    public QueryParams(string url)
    {
        // Set values for instance variables
        this.keywords = "";
        this.sort_field = "";
        this.sort_order = "ASC";
        this.page_size = 10;
        this.current_page = 1;

        // Get the index of the question mark
        Int32 iqs = url.IndexOf('?');

        // Return if the query string is null
        if (url == null || iqs == -1)
        {
            return;
        }

        // Get the query string
        string queryString = (iqs < url.Length - 1) ? url.Substring(iqs + 1) : "";

        // Create a name value collection
        NameValueCollection collection = HttpUtility.ParseQueryString(queryString);

        // Get the keywords
        if (collection["kw"] != null)
        {
            this.keywords = HttpContext.Current.Server.UrlDecode(collection["kw"]);
        }

        // Get the sort field
        if (collection["sf"] != null)
        {
            this.sort_field = collection["sf"];
        }

        // Get the sort order
        if (collection["so"] != null)
        {
            this.sort_order = collection["so"];
        }

        // Get the page size
        if (collection["pz"] != null)
        {
            try
            {
                this.page_size = Int32.Parse(collection["pz"]);
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
            }
        }

        // Get the page number
        if (collection["qp"] != null)
        {
            try
            {
                this.current_page = Int32.Parse(collection["qp"]);
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
                this.current_page = 1;
            }
        }

    } // End of the constructor

    #endregion

    #region Get methods

    /// <summary>
    /// Get a query string from query parameters
    /// </summary>
    /// <param name="parameters">A reference to query parameters</param>
    /// <returns>A query string</returns>
    public static string GetQueryString(QueryParams parameters)
    {
        // Create the string to return
        string queryString = "";

        // Create the string to return
        if(parameters != null)
        {
            queryString = "?kw=" + HttpContext.Current.Server.UrlEncode(parameters.keywords) + "&sf=" + parameters.sort_field + "&so=" + parameters.sort_order + "&pz=" + parameters.page_size
           + "&qp=" + parameters.current_page;
        }

        // Return the query string
        return queryString;

    } // End of the GetQueryString method

    #endregion

} // End of the class