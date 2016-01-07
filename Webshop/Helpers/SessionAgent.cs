using System.Threading;

/// <summary>
/// This class is used to remove expired sessions
/// </summary>
public static class SessionAgent
{
    /// <summary>
    /// This method runs in the background
    /// </summary>
    public static void Run()
    {
        while(true)
        {
            // Sleep for 30 minutes 1800000
            Thread.Sleep(1800000);

            // Do the work
            WebshopSession.DeleteAllExpired();
        }

    } // End of the Run method

} // End of the class