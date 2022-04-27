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

    private void DefSeqBtn_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Starting Default Seq");
    }

    private async void LaunchValBtn_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Launching Valorant");
        await _bgService.StartGame();
    }

    private void ConnToVal_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Attempting to Connect to Valorant");
    }

    private void StartDiscordBtn_OnClick(object sender, RoutedEventArgs e)
    {
        AssistLog.Debug("Attempting to Connect to Discord");
    }
}