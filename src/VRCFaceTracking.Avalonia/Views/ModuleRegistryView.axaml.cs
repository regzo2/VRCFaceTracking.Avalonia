using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Library;
using VRCFaceTracking.Core.Models;
using VRCFaceTracking.Core.Services;

namespace VRCFaceTracking.Avalonia.Views;

public partial class ModuleRegistryView : UserControl
{
    public static event Action<InstallableTrackingModule>? ModuleSelected;
    private ModuleInstaller ModuleInstaller { get; }
    private IModuleDataService ModuleDataService { get; }
    private ILibManager LibManager { get; }

    private static FilePickerFileType ZIP { get; } = new("Zip Files")
    {
        Patterns = [ "*.zip" ]
    };

    public ModuleRegistryView()
    {
        InitializeComponent();

        ModuleDataService = Ioc.Default.GetService<IModuleDataService>()!;
        ModuleInstaller = Ioc.Default.GetService<ModuleInstaller>()!;
        LibManager = Ioc.Default.GetService<UnifiedLibManager>()!;

        this.Get<Button>("BrowseLocal")!.Click += async delegate
        {
            var topLevel = TopLevel.GetTopLevel(this)!;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select a .zip.",
                AllowMultiple = false,
                FileTypeFilter = [ZIP]
            });

            if (files.Count == 0) return;

            string? path = null;
            try
            {
                path = await ModuleInstaller.InstallLocalModule(files.First().Path.AbsolutePath);
            }
            finally
            {
                if (path != null)
                {
                    // CustomInstallStatus.Text = "Successfully installed module.";
                    Dispatcher.UIThread.Invoke(() => LibManager.Initialize());
                }
                else
                {
                    // CustomInstallStatus.Text = "Failed to install module. Check logs for more information.";
                }
            }
        };
    }

    private void OnModuleSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem is InstallableTrackingModule selectedModule)
        {
            ModuleSelected?.Invoke(selectedModule);
        }
    }

    public InstallableTrackingModule[] GetModules()
    {
        IEnumerable<InstallableTrackingModule> data = [];
        Task.Run(async () =>
        {
            data = await ModuleDataService.GetRemoteModules();
        }).Wait();

        // Now comes the tricky bit, we get all locally installed modules and add them to the list.
        // If any of the IDs match a remote module and the other data contained within does not match,
        // then we need to set the local module install state to outdated. If everything matches then we need to set the install state to installed.
        var installedModules = ModuleDataService.GetInstalledModules().Concat(ModuleDataService.GetLegacyModules());
        var localModules = new List<InstallableTrackingModule>();    // dw about it
        foreach (var installedModule in installedModules)
        {
            installedModule.InstallationState = InstallState.Installed;
            var remoteModule = data.FirstOrDefault(x => x.ModuleId == installedModule.ModuleId);
            if (remoteModule == null)   // If this module is completely missing from the remote list, then we need to add it to the list.
            {
                // This module is installed but not in the remote list, so we need to add it to the list.
                localModules.Add(installedModule);
            }
            else
            {
                // This module is installed and in the remote list, so we need to update the remote module's install state.
                remoteModule.InstallationState = remoteModule.Version != installedModule.Version ? InstallState.Outdated : InstallState.Installed;
            }
        }

        // Sort our data by name, then place any modules with the author name VRCFT Team at the top of the list. (unbiased)
        data = data.OrderByDescending(x => x.InstallationState == InstallState.Installed)
            .ThenByDescending(x => x.AuthorName == "VRCFT Team")
            .ThenBy(x => x.ModuleName);

        // Then prepend the local modules to the list.
        data = localModules.Concat(data);
        ModuleSelected?.Invoke(data.First());

        return data.ToArray();
    }
}

