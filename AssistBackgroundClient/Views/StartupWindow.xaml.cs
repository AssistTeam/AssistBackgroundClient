using System.Drawing;
using System.Windows;
using System.Windows.Forms;
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
        PlaceOnScreen();
    }

    void PlaceOnScreen()
    {
        Screen targetScreen = Screen.PrimaryScreen;

        Rectangle viewport = targetScreen.WorkingArea;
        Top = (viewport.Height - this.Height) / 2
              + viewport.Top;
        Left = (viewport.Width - this.Width) / 2
               + viewport.Left; ;
    }

    private async void StartupWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.DefaultStartup();
        this.Close();
    }
}