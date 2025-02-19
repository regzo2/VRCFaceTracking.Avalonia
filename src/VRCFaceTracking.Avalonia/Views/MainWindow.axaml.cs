using Avalonia;
using Avalonia.Layout;
using Avalonia.Controls;
using System.Runtime.InteropServices;
using VRCFaceTracking.Avalonia.ViewModels;
using System;

namespace VRCFaceTracking.Avalonia.Views;

public partial class MainWindow : Window
{
    // constructor with 1 parameter is needed to stop the DI to instantly create the window (when declared as singleton)
    // during the startup phase and crashing the whole android app
    // with "Specified method is not supported window" error
    public MainWindow(MainViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        AdjustTitleBarForPlatform();
    }

    private void AdjustTitleBarForPlatform()
    {
#if OS_MOBILE
        // elements are already disabled.
#elif MACOS
        ApplicationTitleBar.IsVisible = true;
        TitleBarContent.HorizontalAlignment = HorizontalAlignment.Center;
#elif WINDOWS
        ApplicationTitleBar.IsVisible = true;
        TitleBarContent.HorizontalAlignment = HorizontalAlignment.Left;
#elif LINUX
        // Linux has so many edge cases, we will just let the OS handle titlebars.
        SystemDecorations = SystemDecorations.Full;
        ExtendClientAreaToDecorationsHint = false;
#else
        SystemDecorations = SystemDecorations.Full;
        ExtendClientAreaToDecorationsHint = false;
#endif
    }

    public MainWindow() : this(new MainViewModel()) { }
}
