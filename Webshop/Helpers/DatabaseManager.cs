using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.IO.Compression;
using System.Data.SqlClient;

/// <summary>
/// This class handles the creation of the database and upgrades of the database
/// </summary>
public static class DatabaseManager
{
    #region Variables

    public static Int32 DATABASE_VERSION = 2; // The version number is +1 compared to the file version number

    #endregion

    #region Upgrade methods

    /// <summary>
    /// Upgrade the database to the current version
    /// </summary>
    public static void UpgradeDatabase()
    {
        // Get the current database version
        Int32 currentDatabaseVersion = GetDatabaseVersion();

        // Loop and upgrade database versions
        for (int i = currentDatabaseVersion; i <= DATABASE_VERSION; i++)
        {
            // Upgrade the database
            UpgradeToVersion(i);
        }

        // Get all the custom themes
        List<CustomTheme> customThemes = CustomTheme.GetAll("id", "ASC");

        // Check if we should add newly added templates
        for (int i = 0; i < customThemes.Count; i++)
        {
            CustomTheme.AddCustomThemeTemplates(customThemes[i].id);
        }

        // Save database version information
        SetDatabaseVersion(DATABASE_VERSION);

    } // End of the UpgradeDatabase version

    /// <summary>
    /// Upgrade the database to the specified version
    /// </summary>
    /// <param name="databaseVersion">The current database version</param>
    private static void UpgradeToVersion(Int32 databaseVersion)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = GetSqlString(databaseVersion);

        // Make sure that the sql string not is null
        if (sql == null)
            return;

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The Using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the insert
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the UpgradeToVersion method

    #endregion

    #region Helper methods

    /// <summary>
    /// Get the sql string from a file
    /// </summary>
    /// <param name="databaseVersion">The database version</param>
    /// <returns>An sql string</returns>
    private static string GetSqlString(Int32 databaseVersion)
    {
        // Create the string to return
        string fileContent = "";

        // Set the filename
        string filename = HttpContext.Current.Server.MapPath("/DatabaseFiles/db_version_" + databaseVersion.ToString() + ".sql");

        // Check if the file exists
        if (File.Exists(filename) == false)
            return null;

        // Create variables
        StreamReader reader = null;

        try
        {
            // Create the stream reader
            reader = new StreamReader(filename, true);

            // Read the file
            fileContent = reader.ReadToEnd();

        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            // Make sure that the reader is different from null
            if (reader != null)
            {
                // Close the reader
                reader.Close();
            }
        }

        // Return the contents of the file
        return fileContent;

    } // End of the GetSqlString method

    #endregion

    #region Database versioning

    /// <summary>
    /// Save the database version to an xml file
    /// </summary>
    /// <param name="databaseVersion">The database version to update to</param>
    /// <returns>A error message</returns>
    public static void SetDatabaseVersion(Int32 databaseVersion)
    {

        // Set the filename
        string filename = HttpContext.Current.Server.MapPath("/DatabaseFiles/DatabaseVersion.xml.gz");

        // Create variables
        GZipStream gzipStream = null;
        XmlTextWriter xmlTextWriter = null;

        try
        {
            // Create a gzip stream
            gzipStream = new GZipStream(new FileStream(filename, FileMode.Create), CompressionMode.Compress);

            // Create a xml text writer
            xmlTextWriter = new XmlTextWriter(gzipStream, new UTF8Encoding(true));

            // Write the start tag of the document
            xmlTextWriter.WriteStartDocument();

            // Write the root element
            xmlTextWriter.WriteStartElement("Database");

            // Write information to the document
            CreateXmlRow(xmlTextWriter, "Version", databaseVersion.ToString());

            // Write the end tag for the xml document
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();

        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            // Close streams
            if (xmlTextWriter != null)
            {
                // Close the XmlTextWriter
                xmlTextWriter.Close();
            }
            if (gzipStream != null)
            {
                // Close the gzip stream
                gzipStream.Close();
            }
        }

    } // End of the SetDatabaseVersion method

    /// <summary>
    /// Create a xml row
    /// </summary>
    /// <param name="xmlTextWriter">A reference to a XmlTextWriter</param>
    /// <param name="name">The name of the xml tag</param>
    /// <param name="value">The value of the xml tag</param>
    private static void CreateXmlRow(XmlTextWriter xmlTextWriter, string name, string value)
    {
        xmlTextWriter.WriteStartElement(name);
        xmlTextWriter.WriteString(value);
        xmlTextWriter.WriteEndElement();

    } // End of the CreateXmlRow method

    /// <summary>
    /// Get the database version from an xml file
    /// </summary>
    /// <returns>The database version as an int</returns>
    public static Int32 GetDatabaseVersion()
    {
        // Create the int to return
        Int32 databaseVersion = 0;

        // Set the filename
        string filename = HttpContext.Current.Server.MapPath("/DatabaseFiles/DatabaseVersion.xml.gz");

        // Check if the file exists
        if (File.Exists(filename) == false)
            return databaseVersion;

        // Create variables
        GZipStream gzipStream = null;
        XmlTextReader xmlTextReader = null;

        try
        {
            // Create a gzip stream
            gzipStream = new GZipStream(new FileStream(filename, FileMode.Open), CompressionMode.Decompress);

            // Create a xml text reader
            xmlTextReader = new XmlTextReader(gzipStream);

            // Read the xml file
            while (xmlTextReader.Read())
            {
                // Get specific information
                if (xmlTextReader.Name == "Version")
                    Int32.TryParse(xmlTextReader.ReadString(), out databaseVersion);
            }

        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            // Close streams
            if (xmlTextReader != null)
            {
                // Close the XmlTextReader
                xmlTextReader.Close();
            }
            if (gzipStream != null)
            {
                // Close the gzip stream
                gzipStream.Close();
            }
        }

        // Return the database version
        return databaseVersion;

    } // End of the GetDatabaseVersion method

    #endregion

} // End of the class
