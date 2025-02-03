using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Library;
using VRCFaceTracking.Core.Services;

namespace VRCFaceTracking.Avalonia.Views;

public partial class ModuleRegistryView : UserControl
{
    private ModuleInstaller ModuleInstaller { get; }

    private ILibManager LibManager { get; }

    private static FilePickerFileType ZIP { get; } = new("Zip Files")
    {
        Patterns = [ "*.zip" ]
    };

    public ModuleRegistryView()
    {
        InitializeComponent();

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

    ~ModuleRegistryView()
    {
        // Dispose above button event?
    }
}

