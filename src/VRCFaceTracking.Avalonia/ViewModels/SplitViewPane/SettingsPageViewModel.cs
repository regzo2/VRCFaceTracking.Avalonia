using CommunityToolkit.Mvvm.ComponentModel;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class SettingsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isAutoStartEnabled = true;

    [ObservableProperty]
    private bool _isRiskySettingsEnabled = true;
}
