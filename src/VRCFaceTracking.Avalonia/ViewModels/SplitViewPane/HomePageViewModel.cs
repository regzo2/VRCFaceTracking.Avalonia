using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Core.Contracts;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Library;
using VRCFaceTracking.Core.Services;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class HomePageViewModel : ViewModelBase
{
    public ILibManager LibManager { get; }
    public OscQueryService ParameterOutputService { get; }
    public OscRecvService OscRecvService { get; }
    public OscSendService OscSendService { get; }
    public IOscTarget OscTarget { get; }

    [ObservableProperty] private int _messagesInPerSec;

    [ObservableProperty] private int _messagesOutPerSec;

    [ObservableProperty] private bool _noModulesInstalled;

    [ObservableProperty] private bool _oscWasDisabled;

    public HomePageViewModel()
    {
        LibManager = Ioc.Default.GetService<ILibManager>()!;
        ParameterOutputService = Ioc.Default.GetService<OscQueryService>()!;
        OscRecvService = Ioc.Default.GetService<OscRecvService>()!;
        OscSendService = Ioc.Default.GetService<OscSendService>()!;
        OscTarget = Ioc.Default.GetService<IOscTarget>()!;
    }
}
