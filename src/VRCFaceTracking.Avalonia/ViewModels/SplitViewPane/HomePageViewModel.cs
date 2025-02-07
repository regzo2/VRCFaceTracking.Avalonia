using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jeek.Avalonia.Localization;
using Microsoft.Extensions.DependencyInjection;
using VRCFaceTracking.Core.Contracts;
using VRCFaceTracking.Core.Contracts.Services;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class HomePageViewModel : ViewModelBase
{
    public ILibManager LibManager { get; }
    public IOscTarget OscTarget { get; }
    public IAvatarInfo AvatarInfo { get; }

    public string AvatarName
    {
        get
        {
            if (AvatarInfo is null) return "Loading...";
            return string.IsNullOrEmpty(AvatarInfo.Name) ? "Loading..." : AvatarInfo.Name;
        }
    }

    // Below: TODO!
    [ObservableProperty] public bool _isLegacy;

    [ObservableProperty] public int _legacyParametersCount;

    public string AvatarID
    {
        get
        {
            if (AvatarInfo is null) return "Loading...";
            return string.IsNullOrEmpty(AvatarInfo.Id) ? "Loading..." : AvatarInfo.Id;
        }
    }

    public int AvatarParametersCount
    {
        get
        {
            if (AvatarInfo is null) return 0;
            return AvatarInfo.Parameters.Length;
        }
    }


    [ObservableProperty] private int _messagesInPerSecCount;

    [ObservableProperty] private string _messagesInPerSec;

    [ObservableProperty] private int _messagesOutPerSecCount;

    [ObservableProperty] private string _messagesOutPerSec;

    [ObservableProperty] private bool _noModulesInstalled;

    [ObservableProperty] private bool _oscWasDisabled;

    public HomePageViewModel()
    {
        LibManager = Ioc.Default.GetService<ILibManager>()!;
        OscTarget = Ioc.Default.GetService<IOscTarget>()!;
        AvatarInfo = Ioc.Default.GetService<IAvatarInfo>()!;

        _messagesInPerSec = Localizer.Get("msIncoming.Text");
        _messagesOutPerSec = Localizer.Get("msOutgoing.Text");
    }
}
