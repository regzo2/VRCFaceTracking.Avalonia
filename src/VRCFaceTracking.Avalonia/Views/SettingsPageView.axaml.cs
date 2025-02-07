using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Contracts.Services;
using VRCFaceTracking.Core.Contracts.Services;

namespace VRCFaceTracking.Avalonia.Views;

public partial class SettingsPageView : UserControl
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IMainService _mainService;
    private readonly ComboBox _themeComboBox;

    public SettingsPageView()
    {
        InitializeComponent();

        _themeSelectorService = Ioc.Default.GetService<IThemeSelectorService>()!;
        _themeComboBox = this.Find<ComboBox>("ThemeCombo")!;
        _themeComboBox.SelectionChanged += ThemeComboBox_SelectionChanged;
        _mainService = Ioc.Default.GetService<IMainService>()!;

        if (_themeSelectorService.Theme is null)
        {
            _themeSelectorService.SetThemeAsync(ThemeVariant.Default);
            return;
        }

        int index = 0;
        switch (_themeSelectorService.Theme.ToString())
        {
            case "Default":
                index = 0;
                break;
            case "Light":
                index = 1;
                break;
            case "Dark":
                index = 2;
                break;
        }

        _themeComboBox.SelectedIndex = index;
    }

    ~SettingsPageView()
    {
        _themeComboBox.SelectionChanged -= ThemeComboBox_SelectionChanged;
    }

    private void ThemeComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ThemeVariant variant = ThemeVariant.Default;
        var item = _themeComboBox.SelectedItem as ComboBoxItem;
        switch (item!.Content)
        {
            case "Default":
                variant = ThemeVariant.Default;
                break;
            case "Light":
                variant = ThemeVariant.Light;
                break;
            case "Dark":
                variant = ThemeVariant.Dark;
                break;
        }
        Dispatcher.UIThread.InvokeAsync(async () => await _themeSelectorService.SetThemeAsync(variant));
    }

    private void Calibration_OnClick(object? sender, RoutedEventArgs e)
    {

    }

    private void ReInit_OnClick(object? sender, RoutedEventArgs e)
    {
        _mainService.Teardown();
        _mainService.InitializeAsync();
    }

    private void Reset_OnClick(object? sender, RoutedEventArgs e)
    {
        // Create a file in the VRCFT folder called "reset"
        // This will cause the app to reset on the next launch
        File.Create(Path.Combine(VRCFaceTracking.Core.Utils.PersistentDataDirectory, "reset"));
    }
}

