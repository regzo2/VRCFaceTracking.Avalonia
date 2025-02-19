using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;
using VRCFaceTracking.Core.Params.Data;

namespace VRCFaceTracking.Avalonia.Views;

public partial class MutatorPageView : UserControl
{
    public MutatorPageViewModel ViewModel
    {
        get;
    }

    public MutatorPageView()
    {
        ViewModel = Ioc.Default.GetService<MutatorPageViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
}

