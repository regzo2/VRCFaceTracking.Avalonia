using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
    [ObservableProperty] private bool _enabled;

    public bool AllRelevantDebug
    {
        get => ParameterSenderService.AllParametersRelevant;
        set => ParameterSenderService.AllParametersRelevant = value;
    }

    public SettingsPageView View { get; }
    public IOscTarget OscTarget { get; }
    public IMainService MainStandalone { get; }
    public ILocalSettingsService SettingsService { get; }
    public GithubService GithubService { get; }
    public ParameterSenderService ParameterSenderService { get; }

    [ObservableProperty] public float _sliderValue = 0.5f;

    [ObservableProperty] private List<GithubContributor> _contributors;

    public SettingsPageViewModel()
    {
        // Calibration Settings
        // TODO

        // General Settings
        View = Ioc.Default.GetService<SettingsPageView>()!;
        OscTarget = Ioc.Default.GetService<IOscTarget>()!;
        GithubService = Ioc.Default.GetService<GithubService>()!;
        SettingsService = Ioc.Default.GetService<ILocalSettingsService>()!;

        // Risky Settings
        MainStandalone = Ioc.Default.GetService<IMainService>()!;
        ParameterSenderService = Ioc.Default.GetService<ParameterSenderService>()!;
    }

    private async Task LoadContributors()
    {
        Contributors = await GithubService.GetContributors("benaclejames/VRCFaceTracking");
    }
}
