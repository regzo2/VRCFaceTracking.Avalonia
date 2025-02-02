using CommunityToolkit.Mvvm.ComponentModel;
using VRCFaceTracking.Avalonia.Services;

namespace VRCFaceTracking.Avalonia.ViewModels;

public partial class SecretViewModel : ViewModelBase
{
    [ObservableProperty] private string _token;

    public SecretViewModel(AuthenticationResult authResult)
    {
        Token = authResult.Token;
    }
}
