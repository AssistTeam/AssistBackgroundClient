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
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private AssistBgSettingsWindow _settingsWindow;
        protected override void OnStartup(StartupEventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Assist", "BgClient"));
            AssistLog.Normal("Starting BG Client");
            AssistLog.Normal($"ARGS: {string.Join(" ", e.Args)}");
            ParseArguments(e);
            _settingsWindow = new AssistBgSettingsWindow();
            
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (sender, args) => OpenSettingsWindow();
            _notifyIcon.Icon = AssistBackgroundClient.Properties.Resources.assistLogo;
            _notifyIcon.Visible = true;

            CreateContextMenu();
#if DEBUG
            //Current.MainWindow = new StartupWindow();     
            Current.MainWindow = new DebugWindow();
            
#else
            Current.MainWindow = new StartupWindow();     
#endif
            
            Current.MainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            AssistLog.Error("Unhandled Ex Source: " + e.Exception.Source);
            AssistLog.Error("Unhandled Ex StackTrace: " + e.Exception.StackTrace);
            AssistLog.Error("Unhandled Ex Message: " + e.Exception.Message);
            MessageBox.Show(e.Exception.Message, "AssistBG Hit an Error : Logfile Created : If the error persists please reach out on the official discord server.", MessageBoxButton.OK, MessageBoxImage.Warning);
            
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
        
        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
                new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Open Settings").Click += (sender, args) => OpenSettingsWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }


        private void OpenSettingsWindow()
        {
            if (_settingsWindow.IsVisible)
            {
                if (_settingsWindow.WindowState == WindowState.Minimized)
                {
                    _settingsWindow.WindowState = WindowState.Normal;
                }
                _settingsWindow.Activate();
            }
            else
            {
                _settingsWindow.Show();
            }
        }
        private void ExitApplication()
        {
            _notifyIcon.Dispose();
            _notifyIcon.Visible = false;
            _notifyIcon = null;
            Current.Shutdown();
        }
    }
}