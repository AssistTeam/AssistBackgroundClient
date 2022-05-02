using System;
using System.Windows;
using System.Windows.Forms;
using AssistBackgroundClient.Services;

namespace AssistBackgroundClient.Views;

public partial class AssistBgSettingsWindow : Window
{
    public AssistBgSettingsWindow()
    {
        InitializeComponent();
    }
    
    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        var bL = GetTaskBarLocation();
        AssistLog.Normal(bL.ToString());
        MoveBottomRightEdgeOfWindowToMousePosition();
    }

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
    
    private enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT}

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
}