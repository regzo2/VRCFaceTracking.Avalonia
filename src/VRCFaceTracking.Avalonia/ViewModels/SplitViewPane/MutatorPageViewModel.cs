using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class MutatorPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isCalibrationEnabled;

    [ObservableProperty]
    private bool _isParameterAdjustmentEnabled;

    [ObservableProperty]
    private bool _isUnifiedCorrectorsEnabled;

    [ObservableProperty]
    private bool _isDataFilterEnabled;

    [ObservableProperty]
    private float _calibrationDeviation;

    [ObservableProperty] private bool _isClampConnectedExpressionsEnabled;

    [ObservableProperty] private int _debugDataPoints;

    [ObservableProperty] private float _debugStepDelta;

    [ObservableProperty] private float _debugCalibrationDelta;

    public ObservableCollection<TrackingParameter> TrackingParameters { get; private set; } = new()
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
