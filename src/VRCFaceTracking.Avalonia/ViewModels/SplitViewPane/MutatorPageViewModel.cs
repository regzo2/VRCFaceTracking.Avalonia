using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class MutatorPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isCalibrationEnabled = false;

    [ObservableProperty]
    private bool _isParameterAdjustmentEnabled = false;

    [ObservableProperty]
    private bool _isUnifiedCorrectorsEnabled = false;

    [ObservableProperty]
    private bool _isDataFilterEnabled = false;

    [ObservableProperty]
    private float _calibrationDeviation = 0f;

    [ObservableProperty] private bool _isClampConnectedExpressionsEnabled = false;

    [ObservableProperty] private int _debugDataPoints = 0;

    [ObservableProperty] private float _debugStepDelta = 0f;

    [ObservableProperty] private float _debugCalibrationDelta = 0f;

    public ObservableCollection<TrackingParameter> TrackingParameters { get; private set; }

    public MutatorPageViewModel()
    {
        TrackingParameters = new ObservableCollection<TrackingParameter>
        {
            new TrackingParameter("Eyebrow Raiser"),
            new TrackingParameter("Eyebrow Lowerer"),
            new TrackingParameter("Eye Squint"),
            new TrackingParameter("Eye Wide"),
            new TrackingParameter("Cheek"),
            new TrackingParameter("Cheek Squint"),
            new TrackingParameter("Jaw"),
            new TrackingParameter("Mouth Closed"),
            new TrackingParameter("Jaw Sideways")
        };
    }
}

public partial class TrackingParameter : ObservableObject
{
    [ObservableProperty] private string _name;

    [ObservableProperty] private float _lowerValue;

    [ObservableProperty] private float _upperValue;

    [ObservableProperty] private float _minValue;

    [ObservableProperty] private float _maxValue;

    public TrackingParameter(string name, float lowerInitialValue = 0, float upperInitialValue = 1, float minValue = 0, float maxValue = 1)
    {
        Name = name;
        LowerValue = lowerInitialValue;
        UpperValue = upperInitialValue;
        MinValue = minValue;
        MaxValue = maxValue;
    }
}
