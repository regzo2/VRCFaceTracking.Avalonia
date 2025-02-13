using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Avalonia.Views;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public class OutputPageViewModel : ViewModelBase
{
    public OutputPageView View { get; } = Ioc.Default.GetService<OutputPageView>();
}
