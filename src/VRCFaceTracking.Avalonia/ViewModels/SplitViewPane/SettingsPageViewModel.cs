using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Avalonia.Views;
using VRCFaceTracking.Core.Contracts;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Services;
using VRCFaceTracking.Models;
using VRCFaceTracking.Services;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class SettingsPageViewModel : ViewModelBase
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
    private bool _isAutoStartEnabled;

    [ObservableProperty]
    private bool _isRiskySettingsEnabled;

    public bool AllRelevantDebug
    {
        get => ParameterSenderService.AllParametersRelevant;
        set => ParameterSenderService.AllParametersRelevant = value;
    }

    public IOscTarget OscTarget { get; private set;}
    public IMainService MainStandalone { get; private set;}
    public ILocalSettingsService SettingsService { get; private set;}
    public SettingsPageView View { get; private set; }
    public GithubService GithubService { get; private set;}
    public ParameterSenderService ParameterSenderService { get; private set;}

    public SettingsPageViewModel()
    {
        // General/Calibration Settings
        OscTarget = Ioc.Default.GetService<IOscTarget>()!;
        GithubService = Ioc.Default.GetService<GithubService>()!;
        View = Ioc.Default.GetService<SettingsPageView>()!;
        SettingsService = Ioc.Default.GetService<ILocalSettingsService>()!;
        SettingsService.Load(this);

        // Risky Settings
        MainStandalone = Ioc.Default.GetService<IMainService>()!;
        ParameterSenderService = Ioc.Default.GetService<ParameterSenderService>()!;

        PropertyChanged += (_, _) =>
        {
            SettingsService.Save(this);
        };
    }

    private async Task LoadContributors()
    {
        Contributors = await GithubService.GetContributors("benaclejames/VRCFaceTracking");
    }
}
