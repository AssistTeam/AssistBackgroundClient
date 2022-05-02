using System;
using System.Windows;
using System.Windows.Forms;
using AssistBackgroundClient.Services;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace AssistBackgroundClient.Views;

public partial class AssistBgSettingsWindow : Window
{
    private enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT}
    public AssistBgSettingsWindow()
    {
        InitializeComponent();
    }

    #region Window Methods/Events
    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        var bL = GetTaskBarLocation();
        AssistLog.Normal(bL.ToString());
        MoveBottomRightEdgeOfWindowToMousePosition();
    }
    private void UIElement_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        MoveBottomRightEdgeOfWindowToMousePosition();
    }
    private async void AssistBgSettingsWindow_OnInitialized(object? sender, EventArgs e)
    {
        discordRpcToggle.IsChecked = Settings.ApplicationSettings.RPEnabled;
    }
    private void AssistBgSettingsWindow_OnMouseLeave(object sender, MouseEventArgs e)
    {
        this.Hide();
    }
    #endregion
    
    private void Quit_Btn(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
    
    #region Discord RPC Button Toggles
    private void discordRpcToggle_Checked(object sender, RoutedEventArgs e)
    {
        
    }

    private void discordRpcToggle_UnChecked(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region Misc

    
    private void MoveBottomRightEdgeOfWindowToMousePosition()
    {
        var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
        var mouse = transform.Transform(GetMousePosition());
        switch (GetTaskBarLocation())
        {
            case TaskBarLocation.TOP:
                Left = mouse.X - ActualWidth;
                Top = mouse.Y +(Height + 50) - ActualHeight;
                break;
            case TaskBarLocation.RIGHT:
                Left = mouse.X - 50 - ActualWidth;
                Top = mouse.Y - 50 - ActualHeight;
                break;
            case TaskBarLocation.LEFT:
                Left = mouse.X + (Width + 100) - ActualWidth;
                Top = mouse.Y - 100 - ActualHeight;
                break;
            default:
                Left = mouse.X - ActualWidth;
                Top = mouse.Y - 100 - ActualHeight;
                break;
        }
    }
    public System.Windows.Point GetMousePosition()
    {
        System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
        return new System.Windows.Point(point.X, point.Y);
    }
    private TaskBarLocation GetTaskBarLocation()
    {
        TaskBarLocation taskBarLocation = TaskBarLocation.BOTTOM;
        bool taskBarOnTopOrBottom = (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width);
        if (taskBarOnTopOrBottom)
        {
            if (Screen.PrimaryScreen.WorkingArea.Top > 0) taskBarLocation = TaskBarLocation.TOP;
        }
        else
        {
            if (Screen.PrimaryScreen.WorkingArea.Left > 0)
            {
                taskBarLocation = TaskBarLocation.LEFT;
            }
            else
            {
                taskBarLocation = TaskBarLocation.RIGHT;
            }
        }
        return taskBarLocation;
    }

    #endregion
}