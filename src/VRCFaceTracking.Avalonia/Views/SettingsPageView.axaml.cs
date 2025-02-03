using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

namespace VRCFaceTracking.Avalonia.Views;

public partial class SettingsPageView : UserControl
{
    private readonly ComboBox _themeComboBox;

    public SettingsPageView()
    {
        InitializeComponent();

        _themeComboBox = this.Find<ComboBox>("ThemeCombo")!;
        _themeComboBox.SelectionChanged += ThemeComboBox_SelectionChanged;
    }

    ~SettingsPageView()
    {
        _themeComboBox.SelectionChanged -= ThemeComboBox_SelectionChanged;
    }

    private void ThemeComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        string theme = ((ComboBoxItem)_themeComboBox.SelectedItem!)!.Content!.ToString()!;
        ThemeVariant variant = ThemeVariant.Default;
        switch (theme)
        {
            case "System":
                variant = ThemeVariant.Default;
                break;
            case "Light":
                variant = ThemeVariant.Light;
                break;
            case "Dark":
                variant = ThemeVariant.Dark;
                break;
        }

        Application.Current!.RequestedThemeVariant = variant;
    }
}

