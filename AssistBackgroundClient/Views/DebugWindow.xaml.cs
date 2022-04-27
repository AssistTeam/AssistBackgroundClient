using System.Threading.Tasks;
using System.Windows;
using AssistBackgroundClient.Services;

namespace AssistBackgroundClient.Views;

public partial class DebugWindow : Window
{
    private BackgroundService _bgService => ApplicationService.BackgroundService;
    public DebugWindow()
    {
        DataContext = ApplicationService.ApplicationViewModel;
        InitializeComponent();
    }

    private async void DefSeqBtn_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Starting Default Seq");
        await _bgService.StartGame();
        await _bgService.StartService();
        await DebugSettings();
    }

    private async void LaunchValBtn_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Launching Valorant");
        await _bgService.StartGame();
        await DebugSettings();
    }

    private async void ConnToVal_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Attempting to Connect to Valorant");
        await _bgService.StartService();
        await DebugSettings();
    }

    private void StartDiscordBtn_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Attempting to Connect to Discord");
    }

    private async Task DebugSettings()
    {
        while (true)
        {
            await _bgService.LogService();
            scroller.ScrollToBottom();
            await Task.Delay(10000);
        }
    }
}