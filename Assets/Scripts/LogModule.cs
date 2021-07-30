using UnityEngine;

/***
 * 
 * If you would like to debug your build, you can make log as a file with this module.
 * 
 */
public class LogModule
{

    public static void WriteToLogFile(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#else
        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter("app-log.txt", true))
        {
            logFile.WriteLine(message);
        }
#endif
    }
}
