using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Iciclecreek.Avalonia.WindowManager;

namespace Dock.Avalonia.Controls;

public class WindowImpl : ManagedWindow
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (e.NameScope.Find<Control>("PART_TitleBar") is { } titleBar)
            titleBar.IsVisible = false;

        BorderBrush = Brushes.Transparent;
    }
}
