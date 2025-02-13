using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Avalonia.Models;
using VRCFaceTracking.Avalonia.Views;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Models;
using VRCFaceTracking.Core.Services;

namespace VRCFaceTracking.Avalonia.ViewModels.SplitViewPane;

public partial class ModuleRegistryViewModel : ViewModelBase
{
    [ObservableProperty] private InstallableTrackingModule _module;

    [ObservableProperty] private string _searchText;
    public ObservableCollection<InstallableTrackingModule> FilteredModuleInfos { get; private set; } = [];

    private InstallableTrackingModule[] _moduleInfos = [];
    private ModuleRegistryView _moduleRegistryView { get; }
    private IModuleDataService _moduleDataService { get; }
    private ModuleInstaller _moduleInstaller { get; }
    private ILibManager _libManager { get; }

    public ModuleRegistryViewModel()
    {
        _moduleRegistryView = Ioc.Default.GetService<ModuleRegistryView>()!;
        _moduleDataService = Ioc.Default.GetService<IModuleDataService>()!;
        _moduleInstaller = Ioc.Default.GetService<ModuleInstaller>()!;
        _libManager = Ioc.Default.GetService<ILibManager>()!;
        ModuleRegistryView.ModuleSelected += ModuleSelected;
        ModuleRegistryView.LocalModuleInstalled += LocalModuleInstalled;
        ModuleRegistryView.RemoteModuleInstalled += RemoteModuleInstalled;

        _moduleInfos = _moduleRegistryView.GetModules();
        foreach (var module in _moduleInfos)
        {
            FilteredModuleInfos.Add(module);
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        UpdateFilteredModules();
    }

    private void UpdateFilteredModules()
    {
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _moduleInfos
            : _moduleInfos.Where(m =>
                m.ModuleName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                m.DllFileName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                m.AuthorName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                m.ModuleDescription.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase));

        FilteredModuleInfos.Clear();
        foreach (var module in filtered)
        {
            FilteredModuleInfos.Add(module);
        }
    }

    private void ModuleSelected(InstallableTrackingModule module)
    {
        Module = module;
    }

    private void LocalModuleInstalled()
    {
        _moduleInfos = _moduleRegistryView.GetModules();
    }

    private async void RemoteModuleInstalled(InstallableTrackingModule module)
    {
        switch (module.InstallationState)
        {
            case InstallState.NotInstalled or InstallState.Outdated:
            {
                _libManager.TeardownAllAndResetAsync();
                var path = await _moduleInstaller.InstallRemoteModule(module);
                if (string.IsNullOrEmpty(path))
                {
                    module!.InstallationState = InstallState.Installed;
                    await _moduleDataService.IncrementDownloadsAsync(module);
                    module!.Downloads++;
                    _libManager.Initialize();
                }
                break;
            }
            case InstallState.Installed:
            {
                _libManager.TeardownAllAndResetAsync();
                _moduleInstaller.MarkModuleForDeletion(module);
                _libManager.Initialize();
                break;
            }
        }
    }

    public void OpenModuleUrl()
    {
        OpenUrl(_module.ModulePageUrl);
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
