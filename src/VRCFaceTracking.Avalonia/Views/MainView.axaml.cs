using Avalonia;
using Avalonia.Controls;
using System;


namespace VRCFaceTracking.Avalonia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        SetupViewContainer();
    }

    private void SetupViewContainer()
    {
#if OS_MOBILE
        ApplicationSidePanelLogo.IsVisible = true;
        ViewContainer.CornerRadius = new CornerRadius(0, 0, 0, 0);
#elif OS_DESKTOP
        ApplicationSidePanelLogo.IsVisible = false;

        // We will just default to the existing xaml to make it more straight-forward to adjust later.
        //ViewContainer.CornerRadius = new CornerRadius(16, 0, 0, 0);
#else
        ApplicationSidePanelLogo.IsVisible = false;

        // We will just default to the existing xaml to make it more straight-forward to adjust later.
        //ViewContainer.CornerRadius = new CornerRadius(16, 0, 0, 0);
#endif
    }
}
