using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Avalonia.ViewModels;
using VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

namespace VRCFaceTracking.Avalonia.Views;

public partial class HomePageView : UserControl
{
    private readonly MainViewModel _mainViewModel;
    private readonly HomePageViewModel _homePageViewModel;

    public HomePageView()
    {
        InitializeComponent();
        _mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();
        _homePageViewModel = Ioc.Default.GetRequiredService<HomePageViewModel>();
    }

    public void GoToModulesPage(object sender, RoutedEventArgs routedEventArgs)
    {
        _mainViewModel.SelectedListItem = _mainViewModel.Items[2]; // Module Page
    }

    public void DismissModuleWarning(object sender, RoutedEventArgs routedEventArgs)
    {
        _homePageViewModel.NoModulesInstalled = false;
    }
}
