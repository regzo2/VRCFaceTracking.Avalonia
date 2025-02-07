using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;

using VRCFaceTracking.Avalonia;
using VRCFaceTracking.Contracts.Services;
using VRCFaceTracking.Core.Contracts.Services;
using VRCFaceTracking.Core.Models;
using VRCFaceTracking.Core.Services;

namespace VRCFaceTracking.Services;

public class ActivationService : IActivationService
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly OscQueryService _parameterOutputService;
    private readonly IMainService _mainService;
    private readonly IModuleDataService _moduleDataService;
    private readonly ModuleInstaller _moduleInstaller;
    private readonly ILibManager _libManager;
    private readonly ILogger<ActivationService> _logger;

    public ActivationService(
        IThemeSelectorService themeSelectorService,
        OscQueryService parameterOutputService,
        IMainService mainService,
        IModuleDataService moduleDataService,
        ModuleInstaller moduleInstaller,
        ILibManager libManager,
        ILogger<ActivationService> logger)
    {
        _themeSelectorService = themeSelectorService;
        _parameterOutputService = parameterOutputService;
        _mainService = mainService;
        _moduleDataService = moduleDataService;
        _moduleInstaller = moduleInstaller;
        _libManager = libManager;
        _logger = logger;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Execute tasks after activation.
        await StartupAsync();

        await _themeSelectorService.SetRequestedThemeAsync();
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);

        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        _logger.LogInformation("VRCFT Version {version} initializing...", Assembly.GetExecutingAssembly().GetName().Version);

        _logger.LogInformation("Initializing OSC...");
        await _parameterOutputService.InitializeAsync().ConfigureAwait(false);

        _logger.LogInformation("Initializing main service...");
        await _mainService.InitializeAsync().ConfigureAwait(false);

        // Before we initialize, we need to delete pending restart modules and check for updates for all our installed modules
        _logger.LogDebug("Checking for deletion requests for installed modules...");
        var needsDeleting = _moduleDataService.GetInstalledModules().Concat(_moduleDataService.GetLegacyModules())
            .Where(m => m.InstallationState == InstallState.AwaitingRestart);
        foreach (var deleteModule in needsDeleting)
        {
            _moduleInstaller.UninstallModule(deleteModule);
        }

        _logger.LogInformation("Checking for updates for installed modules...");
        var localModules = _moduleDataService.GetInstalledModules().Where(m => m.ModuleId != Guid.Empty);
        var remoteModules = await _moduleDataService.GetRemoteModules();
        var outdatedModules = remoteModules.Where(rm => localModules.Any(lm =>
        {
            if (rm.ModuleId != lm.ModuleId)
                return false;

            var remoteVersion = new Version(rm.Version);
            var localVersion = new Version(lm.Version);

            return remoteVersion.CompareTo(localVersion) > 0;
        }));
        foreach (var outdatedModule in outdatedModules)
        {
            _logger.LogInformation($"Updating {outdatedModule.ModuleName} from {localModules.First(rm => rm.ModuleId == outdatedModule.ModuleId).Version} to {outdatedModule.Version}");
            await _moduleInstaller.InstallRemoteModule(outdatedModule);
        }

        _logger.LogInformation("Initializing modules...");
        Dispatcher.UIThread.Invoke(() => _libManager.Initialize());

        await Task.CompletedTask;
    }
}
