using System;
using System.Net.Mail;
using System.Data.SqlTypes;

/// <summary>
/// This class contains methods to make sure that data is valid
/// </summary>
public static class AnnytabDataValidation
{

    /// <summary>
    /// Check if a name includes correct characters
    /// </summary>
    /// <param name="name">A name</param>
    /// <returns>A boolean that indicates if the name is correct</returns>
    public static bool CheckNameCharacters(string name)
    {
        // Create the boolean to return
        bool isCorrect = true;

        // Create char arrays
        char[] characters = name.ToCharArray();
        char[] controlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-.".ToCharArray();
        for (int i = 0; i < characters.Length; i++)
        {
            bool correctChar = false;
            for (int j = 0; j < controlCharacters.Length; j++)
            {
                if (characters[i] == controlCharacters[j])
                {
                    correctChar = true;
                    break;
                }
            }

            if (correctChar == false)
            {
                isCorrect = false;
                break;
            }
        }

        // Return the boolean
        return isCorrect;

    } // End of the CheckNameCharacters method

    /// <summary>
    /// Check if a page name includes correct characters
    /// </summary>
    /// <param name="pageName">A page name</param>
    /// <returns>A boolean that indicates if the page name is correct</returns>
    public static bool CheckPageNameCharacters(string pageName)
    {
        // Create the boolean to return
        bool isCorrect = true;

        // Create char arrays
        char[] characters = pageName.ToCharArray();
        char[] controlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-".ToCharArray();
        for (int i = 0; i < characters.Length; i++)
        {
            bool correctChar = false;
            for (int j = 0; j < controlCharacters.Length; j++)
            {
                if (characters[i] == controlCharacters[j])
                {
                    correctChar = true;
                    break;
                }
            }

            if (correctChar == false)
            {
                isCorrect = false;
                break;
            }
        }

        // Return the boolean
        return isCorrect;

    } // End of the CheckPageNameCharacters method

    /// <summary>
    /// Check if an email address is valid
    /// </summary>
    /// <param name="email">An email address</param>
    /// <returns>A MailAddress or null, null if the address is incorrect</returns>
    public static MailAddress IsEmailAddressValid(string email)
    {
        // Create the mail address to return
        MailAddress mailAddress = null;

        try
        {
            mailAddress = new MailAddress(email);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }

        // Return the mail address
        return mailAddress;

    } // End of the IsEmailAddressValid method

    /// <summary>
    /// Truncate a decimal to a valid number
    /// </summary>
    /// <param name="number">The number to check</param>
    /// <param name="minValue">The minimum value allowed</param>
    /// <param name="maxValue">The maximum value allowed</param>
    /// <returns>A valid decimal value</returns>
    public static decimal TruncateDecimal(decimal number, decimal minValue, decimal maxValue)
    {
        // Check if the decimal is valid
        if (number < minValue)
        {
            number = minValue;
        }
        else if (number > maxValue)
        {
            number = maxValue;
        }

        // Return the number
        return number;

    } // End of the TruncateDecimal method

    /// <summary>
    /// Truncate date time
    /// </summary>
    /// <param name="dateTime">The date to check</param>
    /// <returns>A valid date</returns>
    public static DateTime TruncateDateTime(DateTime dateTime)
    {
        // Create the date time to return
        DateTime validDateTime = dateTime;

        // Make sure that the date time is between valid dates
        if (dateTime <= (DateTime)SqlDateTime.MinValue || dateTime >= (DateTime)SqlDateTime.MaxValue)
        {
            validDateTime = new DateTime(2000, 1, 1);
        }

        // Return the valid date time
        return validDateTime;

    } // End of the TruncateDateTime method

    /// <summary>
    /// Truncate a string
    /// </summary>
    public static string TruncateString(string post, Int32 maxLength)
    {
        // Truncate the string
        post = post.Length > maxLength ? post.Substring(0, maxLength) : post;

        // Return the string
        return post;

    } // End of the TruncateString method

    /// <summary>
    /// Get a valid locale code for payson
    /// </summary>
    /// <param name="language">A reference to the language</param>
    /// <returns>The locale code</returns>
    public static string GetPaysonLocaleCode(Language language)
    {
        // Create the string to return
        string localeCode = "EN";

        // Check if we should set another language code
        if(language.language_code.ToUpper() == "SV")
        {
            localeCode = "SV";
        }
        else if(language.language_code.ToUpper() == "FI")
        {
            localeCode = "FI";
        }

        // Return the locale code
        return localeCode;

    } // End of the GetPaysonLocaleCode method

} // End of the class