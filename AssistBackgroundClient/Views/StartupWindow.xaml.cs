using System.Windows;
using AssistBackgroundClient.Services;
using AssistBackgroundClient.ViewModels;

namespace AssistBackgroundClient.Views;

public partial class StartupWindow : Window
{
    private readonly ApplicationViewModel _viewModel;
    public StartupWindow()
    {
        DataContext = _viewModel = ApplicationService.ApplicationViewModel;
        InitializeComponent();
    }

    private async void StartupWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.DefaultStartup();
    }
}