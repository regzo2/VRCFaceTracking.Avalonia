using System.Threading.Tasks;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Threading;
using VRCFaceTracking.Contracts.Services;
using VRCFaceTracking.Core.Contracts.Services;

namespace VRCFaceTracking.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    public ThemeVariant Theme { get; set; } = ThemeVariant.Default;

    private readonly ILocalSettingsService _localSettingsService;

    public ThemeSelectorService(ILocalSettingsService localSettingsService) =>_localSettingsService = localSettingsService;

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await SetRequestedThemeAsync();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ThemeVariant theme)
    {
        Theme = theme;
        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public Task SetRequestedThemeAsync()
    {
        Dispatcher.UIThread.Invoke(() => Application.Current!.RequestedThemeVariant = Theme);
        return Task.CompletedTask;
    }

    private async Task<ThemeVariant> LoadThemeFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<ThemeVariant>(SettingsKey);;
    }

    private async Task SaveThemeInSettingsAsync(ThemeVariant theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
}
