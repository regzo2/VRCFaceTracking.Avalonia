using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using VRCFaceTracking.Avalonia.Models;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class ModuleRegistryViewModel : ViewModelBase
{
    [ObservableProperty] private int _version = 1;
    [ObservableProperty] private string _author = "Sample Author";
    [ObservableProperty] private int _downloadCount = 1000;
    [ObservableProperty] private double _rating = 4.5;
    [ObservableProperty] private string _modulePageUrl = "https://example.com/module";
    [ObservableProperty] private DateTime _lastUpdated = DateTime.Now;
    [ObservableProperty] private string _description = "Module description goes here";
    [ObservableProperty] private string _usageInstructions = "How to use the module";

    public IEnumerable<Module> ModuleList =>
    [
        new Module("Module 1", "Bob"),
        new Module("Module 2", "Joe"),
    ];

    public void InstallLocalModule()
    {

    }

    public void InstallRemoteModule()
    {

    }

    public void UninstallModule()
    {

    }

    public void OpenModuleUrl()
    {
        OpenUrl(_modulePageUrl);
    }

    private void OpenUrl(string URL)
    {
        try
        {
            Process.Start(URL);
        }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var url = URL.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", URL);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", URL);
            }
            else
            {
                throw;
            }
        }
    }
}
