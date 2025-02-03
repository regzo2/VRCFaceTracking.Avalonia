using CommunityToolkit.Mvvm.ComponentModel;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class SettingsPageViewModel : ViewModelBase
{
    [ObservableProperty] public int? _receivePort = 9000;

    [ObservableProperty] public string? _ipAddress = "127.0.0.1";

    [ObservableProperty] public int? _sendPort = 9001;

    [ObservableProperty] public bool _isAutoStartEnabled = true;

    [ObservableProperty] public bool _isRiskySettingsEnabled = true;

    [ObservableProperty] public float _sliderValue = 0.5f;
}
