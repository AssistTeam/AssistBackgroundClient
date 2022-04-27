using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using AssistBackgroundClient.Services;
using AssistBackgroundClient.Settings;
using AssistBackgroundClient.Views;

namespace AssistBackgroundClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Assist", "BgClient"));
            AssistLog.Normal("Starting BG Client");
            AssistLog.Normal($"ARGS: {string.Join(" ", e.Args)}");
            ParseArguments(e);

#if DEBUG
            new DebugWindow().Show();
            
#else
            new StartupWindow().Show();     
#endif
        }


        private void ParseArguments(StartupEventArgs e)
        {
            foreach (var arg in e.Args)
            {
                var split = arg.Split(":");

                if (split.Length < 1)
                {
                    return;
                }

                switch (split[0])
                {
                    case "--patchline":
                        ApplicationSettings.ValPatchline = split[1];
                        break;
                    case "--hidden":
                        ApplicationSettings.HiddenEnabled = bool.Parse(split[1]);
                        break;
                    case "--discord":
                        ApplicationSettings.RPEnabled = bool.Parse(split[1]);
                        break;
                }
            }
        }
    }
}