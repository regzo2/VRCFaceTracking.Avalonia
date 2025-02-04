using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Avalonia.Views;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public class OutputPageViewModel : ViewModelBase
{
    public OutputPageView View { get; }

    public OutputPageViewModel()
    {
        View = Ioc.Default.GetService<OutputPageView>();
    }
}
