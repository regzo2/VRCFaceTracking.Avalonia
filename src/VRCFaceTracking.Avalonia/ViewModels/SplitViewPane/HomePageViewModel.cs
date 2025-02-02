using CommunityToolkit.Mvvm.ComponentModel;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class HomePageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isButtonEnabled = true;
}
