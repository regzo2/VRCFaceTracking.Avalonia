using System;
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
using VRCFaceTracking.Core.Contracts;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Services;
using VRCFaceTracking.Models;
using VRCFaceTracking.Services;
using Vector = Avalonia.Vector;
using VrcftImage = VRCFaceTracking.Core.Types.Image;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class SettingsPageViewModel : ViewModelBase, IDisposable
{
    [ObservableProperty]
    [property: SavedSetting("CalibrationEnabled", false)]
    private bool _isCalibrationEnabled;

    [ObservableProperty]
    [property: SavedSetting("ContCalibrationEnabled", false)]
    private bool _isContCalibrationEnabled;

    [ObservableProperty]
    [property: SavedSetting("CalibrationSliderValue", 0.5f)]
    private float _sliderValue;

    [ObservableProperty]
    private List<GithubContributor> _contributors;

    [ObservableProperty]
    [property: SavedSetting("AutoStartEnabled", false)]
    private bool _isAutoStartEnabled;

    [ObservableProperty]
    private bool _isRiskySettingsEnabled;

    public bool AllRelevantDebug
    {
        get => ParameterSenderService.AllParametersRelevant;
        set => ParameterSenderService.AllParametersRelevant = value;
    }

    public IOscTarget OscTarget { get; private set;}
    public ILibManager LibManager { get; private set;}
    public ILocalSettingsService SettingsService { get; private set;}
    public GithubService GithubService { get; private set;}
    public ParameterSenderService ParameterSenderService { get; private set;}

    public bool HasUpperImage => UpperImageSource is not null;
    public bool HasLowerImage => LowerImageSource is not null;
    public WriteableBitmap UpperImageSource { get; private set; }
    public WriteableBitmap LowerImageSource { get; private set; }

    public SettingsPageViewModel()
    {
        // General/Calibration Settings
        OscTarget = Ioc.Default.GetService<IOscTarget>()!;
        GithubService = Ioc.Default.GetService<GithubService>()!;
        LibManager = Ioc.Default.GetService<ILibManager>()!;
        SettingsService = Ioc.Default.GetService<ILocalSettingsService>()!;
        SettingsService.Load(this);

        // Risky Settings
        ParameterSenderService = Ioc.Default.GetService<ParameterSenderService>()!;

        // Initialize hardware debug streams for upper and lower face tracking
        UpperImageSource = InitializeHardwareDebugStream(UnifiedTracking.EyeImageData);
        LowerImageSource = InitializeHardwareDebugStream(UnifiedTracking.LipImageData);

        UnifiedTracking.OnUnifiedDataUpdated += _ => Dispatcher.UIThread.Invoke(OnTrackingDataUpdated);

        PropertyChanged += (_, _) =>
        {
            SettingsService.Save(this);
        };
    }

    ~SettingsPageViewModel()
    {
        UnifiedTracking.OnUnifiedDataUpdated -= _ => Dispatcher.UIThread.Invoke(OnTrackingDataUpdated);
    }

    private WriteableBitmap InitializeHardwareDebugStream(VrcftImage image)
    {
        var imageSize = image.ImageSize;

        if ( imageSize is { x: > 0, y: > 0 } )
        {
            return new WriteableBitmap(
                new PixelSize(imageSize.x, imageSize.y),
                new Vector(96, 96),
                PixelFormat.Rgba8888,
                AlphaFormat.Opaque);
        }

        return null;
    }

    private async void OnTrackingDataUpdated()
    {
        // Handle eye tracking
        var upperData = UnifiedTracking.EyeImageData.ImageData;
        if ( upperData != null )
        {
            UpperImageSource ??= InitializeHardwareDebugStream(UnifiedTracking.EyeImageData);

            using var frameBuffer = UpperImageSource.Lock();
            {
                Marshal.Copy(upperData, 0, frameBuffer.Address, upperData.Length);
            }
        }
        else
        {
            // Handle device getting unplugged / destroyed / disabled
            // Device is connected
            if ( UpperImageSource != null )
            {
                UpperImageSource.Dispose();
                UpperImageSource = null;
            }
        }

        // Handle lip tracking
        var lowerData = UnifiedTracking.LipImageData.ImageData;
        if ( lowerData != null )
        {
            LowerImageSource ??= InitializeHardwareDebugStream(UnifiedTracking.LipImageData);

            using var frameBuffer = LowerImageSource.Lock();
            {
                Marshal.Copy(lowerData, 0, frameBuffer.Address, lowerData.Length);
            }
        }
        else
        {
            // Handle device getting unplugged / destroyed / disabled
            // Device is connected
            if ( LowerImageSource != null )
            {
                LowerImageSource.Dispose();
                LowerImageSource = null;
            }
        }
    }

    private async Task LoadContributors()
    {
        Contributors = await GithubService.GetContributors("benaclejames/VRCFaceTracking");
    }

    public void Dispose()
    {
        UpperImageSource.Dispose();
        LowerImageSource.Dispose();
    }
}
