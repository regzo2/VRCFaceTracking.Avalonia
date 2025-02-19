using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VRCFaceTracking.Avalonia.Controls;

public partial class SettingsExpanderToggle : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<SettingsExpander, string>(nameof(Title));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<SettingsExpander, string>(nameof(Description));

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly StyledProperty<object> ExpanderContentProperty =
            AvaloniaProperty.Register<SettingsExpander, object>(nameof(ExpanderContent));

    public object ExpanderContent
    {
        get => GetValue(ExpanderContentProperty);
        set => SetValue(ExpanderContentProperty, value);
    }

    public static readonly StyledProperty<bool> ExpanderContentToggleProperty =
            AvaloniaProperty.Register<SettingsExpander, bool>(nameof(ExpanderContentToggle));

    public object ExpanderContentToggle
    {
        get => GetValue(ExpanderContentToggleProperty);
        set => SetValue(ExpanderContentToggleProperty, value);
    }

    public SettingsExpanderToggle()
    {
        InitializeComponent();
        DataContext = this;
    }
}
