using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssistBackgroundClient.Services;

namespace AssistBackgroundClient.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
    private BackgroundService BgService => ApplicationService.BackgroundService;

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
        //vLogView += "\n" + a;
    }

    public async Task DefaultStartup()
    {
        Status = "Starting...";
        //await BgService.StartGame();
        Status = "Client Started.";
    }
}