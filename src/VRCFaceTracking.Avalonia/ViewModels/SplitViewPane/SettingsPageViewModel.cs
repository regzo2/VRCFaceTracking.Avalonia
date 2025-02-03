using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using VRCFaceTracking.Avalonia.Views;
using VRCFaceTracking.Core.Contracts;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Services;
using VRCFaceTracking.Core.Types;
using VRCFaceTracking.Models;
using VRCFaceTracking.Services;
using PixelFormat = Avalonia.Remote.Protocol.Viewport.PixelFormat;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class SettingsPageViewModel : ViewModelBase
{
    public SettingsPageView View { get; }
    public IOscTarget OscTarget { get; }
    public GithubService GithubService { get; }
    public IMainService MainStandalone { get; }
    public ILogger Logger { get; }
    public ParameterSenderService ParameterSenderService { get; }

    [ObservableProperty] public bool _isAutoStartEnabled = true;

    [ObservableProperty] public bool _isRiskySettingsEnabled = true;

    [ObservableProperty] public float _sliderValue = 0.5f;

    [ObservableProperty] private List<GithubContributor> _contributors;

    // private WriteableBitmap _upperImageStream, _lowerImageStream;
    // private MemoryStream _upperStream, _lowerStream;

    public SettingsPageViewModel()
    {
        // General Settings
        View = Ioc.Default.GetService<SettingsPageView>()!;
        OscTarget = Ioc.Default.GetService<IOscTarget>()!;
        GithubService = Ioc.Default.GetService<GithubService>()!;
        // InitializeHardwareDebugStream(UnifiedTracking.EyeImageData, ref _upperImageStream);
        // InitializeHardwareDebugStream(UnifiedTracking.LipImageData, ref _lowerImageStream);
        // UnifiedTracking.OnUnifiedDataUpdated += _ => Dispatcher.UIThread.Invoke(OnTrackingDataUpdated);
        // LoadContributors();

        // Calibration Settings
        // TODO

        // Risky Settings
        // MainStandalone = Ioc.Default.GetService<IMainService>()!;
        // Logger = Ioc.Default.GetService<ILogger>()!;
        // ParameterSenderService = Ioc.Default.GetService<ParameterSenderService>()!;
    }

    private async void LoadContributors()
    {
        Contributors = await GithubService.GetContributors("benaclejames/VRCFaceTracking");
    }
}
