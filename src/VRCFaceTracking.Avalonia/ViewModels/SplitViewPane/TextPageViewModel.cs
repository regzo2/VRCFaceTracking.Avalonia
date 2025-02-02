using CommunityToolkit.Mvvm.ComponentModel;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class TextPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isTextEnabled = true;
}
