using AssistBackgroundClient.ViewModels;

namespace AssistBackgroundClient.Services;

public class ApplicationService
{
    public static BackgroundService BackgroundService { get; } = new BackgroundService();
    public static ApplicationViewModel ApplicationViewModel { get; } = new ApplicationViewModel();
}