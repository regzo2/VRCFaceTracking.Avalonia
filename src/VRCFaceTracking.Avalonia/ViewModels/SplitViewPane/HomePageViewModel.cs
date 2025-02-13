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
    public IOscTarget OscTarget { get; }
    private IModuleDataService ModuleDataService { get; }
    private OscRecvService OscRecvService { get; }
    private OscSendService OscSendService { get; }

    [ObservableProperty] private OscQueryService parameterOutputService;

    private int _messagesRecvd;
    [ObservableProperty] private string _messagesInPerSecCount;

    private int _messagesSent;
    [ObservableProperty] private string _messagesOutPerSecCount;

    [ObservableProperty] private bool _noModulesInstalled;

    [ObservableProperty] private bool _oscWasDisabled;

    [ObservableProperty] private int _currentParametersCount;

    [ObservableProperty] private int _legacyParametersCount;

    [ObservableProperty] private bool _isLegacyAvatar;

    [ObservableProperty] private bool _isTestAvatar;

    private readonly DispatcherTimer _msgCounterTimer;

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
        MessagesInPerSecCount = "0";
        MessagesOutPerSecCount = "0";
        OscRecvService.OnMessageReceived += MessageReceived;
        OscSendService.OnMessagesDispatched += MessageDispatched;
        _msgCounterTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _msgCounterTimer.Tick += (_, _) =>
        {
            MessagesInPerSecCount = _messagesRecvd.ToString();
            _messagesRecvd = 0;

            MessagesOutPerSecCount = _messagesSent.ToString();
            _messagesSent = 0;

            CurrentParametersCount = ParameterOutputService.AvatarParameters?.Count ?? 0;
            LegacyParametersCount = ParameterOutputService.AvatarParameters?.Count(p => p.Deprecated) ?? 0;
            IsLegacyAvatar = LegacyParametersCount > 0;
            IsTestAvatar = ParameterOutputService.AvatarInfo?.Id.StartsWith("local:") ?? false;
        };
        _msgCounterTimer.Start();
    }

    private void MessageReceived(OscMessage msg) => _messagesRecvd++;
    private void MessageDispatched(int msgCount) => _messagesSent += msgCount;

    ~HomePageViewModel()
    {
        OscRecvService.OnMessageReceived -= MessageReceived;
        OscSendService.OnMessagesDispatched -= MessageDispatched;

        _msgCounterTimer.Stop();
    }
}
