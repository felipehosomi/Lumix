using System;
using System.Diagnostics;
using System.IO;

namespace Nordware.AddOn.Lumix.Core.Utils
{
    public class LogHelper
    {
        public static void LogFileException(Exception ex)
        {
            if (!Directory.Exists("c:\\temp\\"))
            {
                Directory.CreateDirectory("c:\\temp\\");
            }

            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);

            var classAndLineNumber = $"Class: {frame.GetFileName()} | Method: {frame.GetMethod()} | Line: {frame.GetFileLineNumber()}";

            StreamWriter sw = new StreamWriter("c:\\temp\\Addo Lumix Log.txt");
            sw.WriteLine(classAndLineNumber);
            sw.WriteLine(ex.StackTrace);
            sw.WriteLine(ex.Message);
            if (ex.InnerException != null)
            {                
                sw.WriteLine(ex.InnerException.Message);
                sw.WriteLine(ex.InnerException.StackTrace);
            }
            sw.Close();
        }
    }
}
