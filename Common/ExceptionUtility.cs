using System;
using System.IO;

namespace CommonFrontEnd.Common
{
    public sealed class ExceptionUtility
    {
        private ExceptionUtility()
        { }
        public static void LogError(Exception ex)
        {
            string message = string.Format("Time: {0}", System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            DirectoryInfo LogPath = new DirectoryInfo(
     Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"LogFiles/" + System.DateTime.Now.ToString("dd-MM-yyyy") + "LogFile.txt")));

            if (!Directory.Exists(Environment.CurrentDirectory +  "/LogFiles"))
                Directory.CreateDirectory(Environment.CurrentDirectory + "/LogFiles");

            using (StreamWriter writer = new StreamWriter(LogPath.ToString(), true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}
