using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Web.SessionState;
using System.IO;

/// <summary>
/// This is a custom session state provider to save session state in an MS SQL database
/// </summary>
public class SqlSessionStateProvider : SessionStateStoreProviderBase
{

    #region Variables

    public SessionStateSection sessionStateConfiguration = null;
    public string applicationName = "";

    #endregion

    #region Initialization

    /// <summary>
    /// Initialize the session state provider
    /// </summary>
    public override void Initialize(string name, NameValueCollection config)
    {
        
        if (config == null)
            throw new ArgumentNullException("config");

        if (name == null || name.Length == 0)
            name = "SqlSessionStateProvider";

        if (String.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "Sql session state provider");
        }

        // Initialize the abstract base class.
        base.Initialize(name, config);

        // Set the application name
        this.applicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;

        // Get the session state configuration
        Configuration cfg = WebConfigurationManager.OpenWebConfiguration(this.applicationName);
        sessionStateConfiguration = (SessionStateSection)cfg.GetSection("system.web/sessionState");

    } // End of the Initialize method

    #endregion

    #region Add methods

    /// <summary>
    /// Create an uninitialized item
    /// </summary>
    public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
    {
        // Create a webshop session post
        WebshopSession webshopSession = new WebshopSession();
        webshopSession.id = id;
        webshopSession.application_name = this.applicationName;
        webshopSession.created_date = DateTime.UtcNow;
        webshopSession.expires_date = DateTime.UtcNow.AddMinutes((Double)timeout);
        webshopSession.lock_date = DateTime.UtcNow;
        webshopSession.lock_id = 0;
        webshopSession.timeout_limit = timeout;
        webshopSession.locked = false;
        webshopSession.session_items = "";
        webshopSession.flags = 1;

        // Add the session
        WebshopSession.Add(webshopSession);

    } // End of the CreateUninitializedItem method

    /// <summary>
    /// Create a new data store
    /// </summary>
    public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
    {
        return new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), timeout);

    } // End of the CreateNewStoreData method

    #endregion

    #region Update methods

    /// <summary>
    /// Set the callback for session expiration
    /// </summary>
    public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
    {
        return false;

    } // End of the SetItemExpireCallback method

    /// <summary>
    /// Set and realease a session post
    /// </summary>
    public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
    {
        // Serialize the SessionStateItemCollection as a string.
        string sessItems = Serialize((SessionStateItemCollection)item.Items);

        // Create a webshop session
        WebshopSession webshopSession = new WebshopSession();
        webshopSession.id = id;
        webshopSession.application_name = this.applicationName;
        webshopSession.created_date = DateTime.UtcNow;
        webshopSession.expires_date = DateTime.UtcNow.AddMinutes((Double)item.Timeout);
        webshopSession.lock_date = DateTime.UtcNow;
        webshopSession.lock_id = 0;
        webshopSession.timeout_limit = item.Timeout;
        webshopSession.locked = false;
        webshopSession.session_items = sessItems;
        webshopSession.flags = 0;

        if (newItem == true)
        {
            // Delete the session if it exists
            WebshopSession.DeleteOnId(id, this.applicationName);

            // Add the session
            WebshopSession.Add(webshopSession);

        }
        else
        {
            // Update session values
            webshopSession.lock_id = (Int32)lockId;

            // Update the session
            WebshopSession.UpdateWithLockId(webshopSession);
        }

    } // End of the SetAndReleaseItemExclusive method

    /// <summary>
    /// Release an session item
    /// </summary>
    public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
    {
        // Create a webshop session post
        WebshopSession webshopSession = new WebshopSession();
        webshopSession.id = id;
        webshopSession.application_name = this.applicationName;
        webshopSession.lock_id = (Int32)lockId;
        webshopSession.expires_date = DateTime.UtcNow.AddMinutes(this.sessionStateConfiguration.Timeout.TotalMinutes);
        webshopSession.locked = false;

        // Update the lock
        WebshopSession.Unlock(webshopSession);

    } // End of the ReleaseItemExclusive method

    /// <summary>
    /// Reset the timeout for the session
    /// </summary>
    public override void ResetItemTimeout(HttpContext context, string id)
    {
        // Create a webshop session post
        WebshopSession webshopSession = new WebshopSession();
        webshopSession.id = id;
        webshopSession.application_name = this.applicationName;
        webshopSession.expires_date = DateTime.UtcNow.AddMinutes(this.sessionStateConfiguration.Timeout.TotalMinutes);

        // Update the expiration date
        WebshopSession.UpdateExpirationDate(webshopSession);

    } // End of the ResetItemTimeout method

    #endregion

    #region Get methods

    /// <summary>
    /// Get a item
    /// </summary>
    public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
    {
        return GetSessionStoreItem(false, context, id, out locked, out lockAge, out lockId, out actionFlags);

    } // End of the GetItem method


    /// <summary>
    /// Get a item exclusive
    /// </summary>
    public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
    {
        return GetSessionStoreItem(true, context, id, out locked, out lockAge, out lockId, out actionFlags);

    } // End of the GetItemExclusive method


    /// <summary>
    /// Get a session store item
    /// </summary>
    private SessionStateStoreData GetSessionStoreItem(bool lockRecord, HttpContext context, string id, out bool locked, out TimeSpan lockAge, 
        out object lockId, out SessionStateActions actionFlags)
    {
        // Initial values for return value and out parameters
        SessionStateStoreData item = null;
        lockAge = TimeSpan.Zero;
        lockId = null;
        locked = false;
        actionFlags = 0;

        // String to hold serialized SessionStateItemCollection.
        string serializedItems = "";
        // True if a record is found in the database.
        bool foundRecord = false;
        // True if the returned session item is expired and needs to be deleted.
        bool deleteData = false;
        // Timeout value from the data store.
        int timeout = 0;

        // Create a webshop session
        WebshopSession webshopSession = new WebshopSession();

        if (lockRecord == true)
        {
            // Update the webshop session
            webshopSession.id = id;
            webshopSession.application_name = this.applicationName;
            webshopSession.expires_date = DateTime.UtcNow;
            webshopSession.lock_date = DateTime.UtcNow;
            webshopSession.locked = true;

            // Lock the webshop session
            Int32 postsAffected = WebshopSession.Lock(webshopSession);

            // Set the locked variable
            locked = postsAffected == 0 ? true : false;
        }

        // Get the current session
        webshopSession = WebshopSession.GetOneById(id, this.applicationName);

        if(webshopSession != null)
        {
            if (webshopSession.expires_date < DateTime.UtcNow)
            {
                locked = false;
                deleteData = true;
            }
            else
            {
                foundRecord = true;
            }

            // Set data
            serializedItems = webshopSession.session_items;
            lockId = webshopSession.lock_id;
            lockAge = DateTime.UtcNow.Subtract(webshopSession.lock_date);
            actionFlags = (SessionStateActions)webshopSession.flags;
            timeout = webshopSession.timeout_limit;
        }

       
        // If the returned session item is expired, 
        // delete the record from the data source.
        if (deleteData)
        {
            WebshopSession.DeleteOnId(id, this.applicationName);
        }

        // The record was not found. Ensure that locked is false.
        if (foundRecord == false)
        {
            locked = false;
        }
            
        // If the record was found and you obtained a lock, then set 
        // the lockId, clear the actionFlags,
        // and create the SessionStateStoreItem to return.
        if (foundRecord && !locked)
        {
            lockId = (int)lockId + 1;

            // Update the lock id and flags
            WebshopSession.UpdateLockIdAndFlags(id, this.applicationName, (Int32)lockId);

            // If the actionFlags parameter is not InitializeItem, deserialize the stored SessionStateItemCollection.
            if (actionFlags == SessionStateActions.InitializeItem)
                item = CreateNewStoreData(context, (Int32)this.sessionStateConfiguration.Timeout.TotalMinutes);
            else
                item = Deserialize(context, serializedItems, timeout);
        }

        // Return the session item
        return item;

    } // End of the GetSessionStoreItem method

    #endregion

    #region Serialization

    /// <summary>
    /// Serialize session items
    /// </summary>
    private string Serialize(SessionStateItemCollection items)
    {

        // Create the string to return
        string sessionItems = "";

        // Create variables
        MemoryStream stream = null;
        BinaryWriter writer = null;

        try
        {
            // Create the binary writer
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);

            // Serialize session items
            if (items != null)
                items.Serialize(writer);

            // Get the serialized string
            sessionItems = Convert.ToBase64String(stream.ToArray());

        }
        catch(Exception e)
        {
            // We do not want to throw an exception
            string exMessage = e.Message;
        }
        finally
        {
            // Close the binary writer
            if(writer != null)
            {
                writer.Close();
            }
        }

        // Return the string
        return sessionItems;

    } // End of the Serialize method

    /// <summary>
    /// Deserialize session items
    /// </summary>
    private SessionStateStoreData Deserialize(HttpContext context, string serializedItems, int timeout)
    {

        // Create variables
        SessionStateItemCollection sessionItems = new SessionStateItemCollection();
        MemoryStream stream = null;
        BinaryReader reader = null;

        try
        {
            // Create the stream
            stream = new MemoryStream(Convert.FromBase64String(serializedItems));
            
            // Deserialize the stream
            if(stream.Length > 0)
            {
                reader = new BinaryReader(stream);
                sessionItems = SessionStateItemCollection.Deserialize(reader);  
            }
        }
        catch(Exception e)
        {
            // We do not want to throw an exception
            string exMessage = e.Message;
        }
        finally
        {
            // Close the reader if it is different from null
            if(reader != null)
            {
                reader.Close();
            }
        }

        // Return the data
        return new SessionStateStoreData(sessionItems, SessionStateUtility.GetSessionStaticObjects(context), timeout);

    } // End of the Deserialize method

    #endregion

    #region Delete methods

    /// <summary>
    /// Remove a session item
    /// </summary>
    public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
    {
        // Delete the session post
        WebshopSession.DeleteOnId(id, this.applicationName, (int)lockId);
        
    } // End of the RemoveItem method

    #endregion

    #region Not implemented methods

    public override void Dispose(){}
    public override void InitializeRequest(HttpContext context){}
    public override void EndRequest(HttpContext context){}

    #endregion

} // End of the class