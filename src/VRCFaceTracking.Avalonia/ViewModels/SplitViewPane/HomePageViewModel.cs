using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jeek.Avalonia.Localization;
using Microsoft.Extensions.DependencyInjection;
using VRCFaceTracking.Core.Contracts;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.OSC;
using VRCFaceTracking.Core.Params;
using VRCFaceTracking.Core.Services;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class HomePageViewModel : ViewModelBase
{
    public ILibManager LibManager { get; }
    public IModuleDataService ModuleDataService { get; }
    public OscRecvService OscRecvService { get; }
    public OscSendService OscSendService { get; }
    public IOscTarget OscTarget { get; }

    [ObservableProperty] private bool _noModulesInstalled;

    [ObservableProperty] private bool _oscWasDisabled;

    private int _messagesRecvd;
    [ObservableProperty] private string _messagesInPerSecCount;

    private int _messagesSent;
    [ObservableProperty] private string _messagesOutPerSecCount;

    [ObservableProperty] private OscQueryService parameterOutputService;

    public int CurrentParametersCount => ParameterOutputService.AvatarParameters?.Count ?? 0;
    public int LegacyParametersCount => ParameterOutputService.AvatarParameters?.Count(p => p.Deprecated) ?? 0;
    public bool IsLegacyAvatar => LegacyParametersCount > 0;
    public bool IsTestAvatar => ParameterOutputService.AvatarInfo?.Id.StartsWith("local:") ?? false;

    private DispatcherTimer msgCounterTimer;

    public HomePageViewModel()
    {
        // Services
        LibManager = Ioc.Default.GetService<ILibManager>()!;
        ParameterOutputService = Ioc.Default.GetService<OscQueryService>()!;
        ModuleDataService = Ioc.Default.GetService<IModuleDataService>()!;
        OscTarget = Ioc.Default.GetService<IOscTarget>()!;
        OscRecvService = Ioc.Default.GetService<OscRecvService>()!;
        OscSendService = Ioc.Default.GetService<OscSendService>()!;

        // Modules
        var installedNewModules = ModuleDataService.GetInstalledModules();
        var installedLegacyModules = ModuleDataService.GetLegacyModules().Count();
        NoModulesInstalled = !installedNewModules.Any() && installedLegacyModules == 0;

        // Message Timer
        OscRecvService.OnMessageReceived += MessageReceived;
        OscSendService.OnMessagesDispatched += MessageDispatched;
        msgCounterTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        msgCounterTimer.Tick += (_, _) =>
        {
            MessagesInPerSecCount = _messagesRecvd.ToString();
            _messagesRecvd = 0;

            MessagesOutPerSecCount = _messagesSent.ToString();
            _messagesSent = 0;
        };
        msgCounterTimer.Start();
    }

    partial void OnParameterOutputServiceChanged(OscQueryService value)
    {
        OnPropertyChanged(nameof(IsTestAvatar));
        OnPropertyChanged(nameof(CurrentParametersCount));
        OnPropertyChanged(nameof(LegacyParametersCount));
        OnPropertyChanged(nameof(IsLegacyAvatar));
    }

    private void MessageReceived(OscMessage msg) => _messagesRecvd++;
    private void MessageDispatched(int msgCount) => _messagesSent += msgCount;

    ~HomePageViewModel()
    {
        OscRecvService.OnMessageReceived -= MessageReceived;
        OscSendService.OnMessagesDispatched -= MessageDispatched;

        msgCounterTimer.Stop();
    }
}
