using System;
using System.IO;
using System.Windows.Forms;

namespace GrandoLib
{
    public class Logger
    {
        public Logger()
        {
            if (!File.Exists("Log.txt"))
                File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt"));
        }

        public static void WriteLog(string context, Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt"), true);
                sw.WriteLine(DateTime.Now.ToString() + ": Error in " + context + "\r\n\t" + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                if (context.Equals("Connection"))
                    MessageBox.Show(null,"BingPaper requires Internet connection.", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                WriteLog("Logger", e);
            }
        }

        public static void WriteLog(string message)
        {
            try
            {
                StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt"), true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                WriteLog("Logger", ex);
            }
        }

        public static void WriteLog(string[] message)
        {
            try
            {
                if (message.Length > 0)
                {
                    StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt"), true);
                    sw.WriteLine(DateTime.Now.ToString() + ": ");
                    foreach (string text in message)
                        sw.WriteLine("\t" + text);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog("Logger", ex);
            }
        }
    }
}
