using System.Threading.Tasks;
using Avalonia.Styling;

namespace VRCFaceTracking.Contracts.Services;

public interface ILanguageSelectorService
{
    string Language
    {
        get;
    }

    Task InitializeAsync();

    Task SetLanguageAsync(string language);

    Task SetRequestedLanguageAsync();
}
