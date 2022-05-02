using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AssistBackgroundClient.Services;

namespace AssistBackgroundClient.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
    private BackgroundService _bgService => ApplicationService.BackgroundService;
    private Thread _checkThread;
    private bool _checkRunning = true;
    private string _status;
    public string Status {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private string vLogs;

    public string vLogView
    {
        get => vLogs;
        set => SetProperty(ref vLogs, value);
    }

    public void addLog(string a )
    {
        vLogView += "\n" + a;
    }

    public async Task DefaultStartup()
    {
        AssistLog.Debug("Starting Default Seq");
        Status = "Starting...";
        await _bgService.StartGame();
        Status = "Client Started.";
        
        await _bgService.StartService();
        StartCheckThread();
    }

    private async void StartCheckThread()
    {
        object lok = new();
        _checkThread = new(() =>
        {
            while (_checkRunning)
            {
                lock (lok)
                {
                    if (_bgService.bHasValorantExited)
                        ApplicationService.ApplicationViewModel._checkRunning = false;
                }
            }
            Environment.Exit(0);
        });

        _checkThread.Start();
    }
}