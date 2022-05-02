using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssistBackgroundClient.Services
{
    public class AssistLog
    {
        private static string logPath { get; }
        private static string logfolderpath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Assist", "Background_Logs");

        private List<string> log;

        static AssistLog()
        {
            // Create Log file
            Directory.CreateDirectory(logfolderpath);
            int fileCount = Directory.GetFiles(logfolderpath, "*.*", SearchOption.TopDirectoryOnly).Length;
            logPath = Path.Combine(logfolderpath, $"ASSBG_Log_{++fileCount}.txt");
            File.CreateText(logPath).Dispose();
        }

        public static void Normal(string message)
        {
            WriteToLog("[NORMAL] " + message);
        }
        public static void Error(string message)
        {
            WriteToLog("[ERROR] " + message);
        }

        public static void Debug(string message)
        {
            WriteToLog("[DEBUG] " + message);
        }

        private static void WriteToLog(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logPath, append: true))
                {
                    sw.WriteLine($"[{DateTime.Now.ToString()}] : {message}");
                }
                //ApplicationService.ApplicationViewModel.addLog(message);
                Trace.WriteLine($"[{DateTime.Now.ToString()}] : {message}");
            }
            catch (Exception e)
            {
               return;
            }
            
        }
        
    }
}
