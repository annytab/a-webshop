using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class represent a dictionary with strings
/// </summary>
public class KeyStringList
{
    #region Variables

    public Dictionary<string, string> dictionary;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new key string list with default properties
    /// </summary>
    public KeyStringList()
    {
        // Set values for instance variables
        this.dictionary = new Dictionary<string, string>(10);

    } // End of the constructor

    /// <summary>
    /// Create a new key string list
    /// </summary>
    /// <param name="capacity">The initial capacity</param>
    public KeyStringList(Int32 capacity)
    {
        // Set values for instance variables
        this.dictionary = new Dictionary<string, string>(capacity);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add a item to the key string list
    /// </summary>
    /// <param name="key">The key as a string</param>
    /// <param name="value">The value as a string</param>
    public void Add(string key, string value)
    {
        // Add the value to the dictionary
        dictionary.Add(key, value);

    } // End of the Add method

    #endregion

    #region Update methods

    /// <summary>
    /// Update the value for the key
    /// </summary>
    /// <param name="key">The key as a string</param>
    /// <param name="value">The value as a string</param>
    public void Update(string key, string value)
    {
        // Update the value
        dictionary[key] = value;

    } // End of the Update method

    #endregion

    #region Get methods

    /// <summary>
    /// Get the value from the dictionary, the key is returned if the key not is found
    /// </summary>
    /// <param name="key">The key as a string</param>
    /// <returns>The value as a string</returns>
    public string Get(string key)
    {
        // Create the string to return
        string value = key;

        // Check if the dictionary contains the key
        if (this.dictionary.ContainsKey(key))
        {
            value = this.dictionary[key];
        }

        // Return the value
        return value;

    } // End of the Get method

    #endregion

} // End of the class
