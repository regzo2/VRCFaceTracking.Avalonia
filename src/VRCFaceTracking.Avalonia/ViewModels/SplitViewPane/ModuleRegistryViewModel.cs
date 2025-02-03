using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Avalonia.Models;
using VRCFaceTracking.Avalonia.Views;
using VRCFaceTracking.Core.Models;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class ModuleRegistryViewModel : ViewModelBase
{
    public InstallableTrackingModule[] ModuleInfos { get; private set; } = [];
    private ModuleRegistryView _moduleRegistryView { get; }

    [ObservableProperty] public string version = "1";
    [ObservableProperty] public string author = "Sample Author";
    [ObservableProperty] public int downloadCount = 1000;
    [ObservableProperty] public double rating = 4.5;
    [ObservableProperty] public string modulePageUrl = "https://example.com/module";
    [ObservableProperty] public DateTime lastUpdated = DateTime.Now;
    [ObservableProperty] public string description = "Module description goes here";
    [ObservableProperty] public string usageInstructions = "How to use the module";

    public ModuleRegistryViewModel()
    {
        _moduleRegistryView = Ioc.Default.GetService<ModuleRegistryView>()!;
        ModuleRegistryView.ModuleSelected += ModuleSelected;
        ModuleInfos = _moduleRegistryView.GetModules();
    }

    private void ModuleSelected(InstallableTrackingModule module)
    {
        Version = module.Version;
        Author = module.AuthorName;
        DownloadCount = module.Downloads;
        Rating = module.Rating;
        ModulePageUrl = module.ModulePageUrl;
        LastUpdated = module.LastUpdated;
        Description = module.ModuleDescription;
        UsageInstructions = module.UsageInstructions;
    }

    public void OpenModuleUrl()
    {
        OpenUrl(modulePageUrl);
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

    ~ModuleRegistryViewModel()
    {
        ModuleRegistryView.ModuleSelected -= ModuleSelected;
    }
}
