using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;
using Dock.Avalonia.Controls;
using Dock.Model.Core;
using Iciclecreek.Avalonia.WindowManager;

namespace Dock.Avalonia.Internal;

internal class DragPreviewHelper
{
    private DragPreviewControl? _control;
    private DragPreviewWindow? _window;

    private static WindowsPanel? FindDefaultWindowsPanel()
    {
        // search from top down
        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            return singleView.MainView?.FindDescendantOfType<WindowsPanel>(true);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow?.FindDescendantOfType<WindowsPanel>(true);
        return null;
    }

    private static PixelPoint GetPositionWithinWindow(ManagedWindow? window, PixelPoint position, PixelPoint offset)
    {
        var windowsPanel = window?.WindowsPanel ?? FindDefaultWindowsPanel();
        var screen = TopLevel.GetTopLevel(windowsPanel)?.Screens?.ScreenFromPoint(position);
        
        if (screen is not null && windowsPanel is not null)
        {
            var target = windowsPanel.PointToClient(position + offset);
            var targetPosition = new PixelPoint((int)target.X, (int)target.Y);
            if (screen.WorkingArea.Contains(targetPosition))
                return targetPosition;
        }

        return position;
    }

    public void Show(IDockable dockable, PixelPoint position, PixelPoint offset)
    {
        Hide();

        _control = new DragPreviewControl {Status = string.Empty};

        _window = new DragPreviewWindow {Content = _control, DataContext = dockable};

        _window.Position = GetPositionWithinWindow(_window, position, offset);

        _window.Show();
    }

    public void Move(PixelPoint position, PixelPoint offset, string status)
    {
        if (_window is null || _control is null)
            return;

        _control.Status = status;
        _window.Position = GetPositionWithinWindow(_window, position, offset);
    }

    public void Hide()
    {
        if (_window is null)
            return;

        _window.Close();
        _window = null;
        _control = null;
    }
}
