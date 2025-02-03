using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace VRCFaceTracking.Avalonia.Views;

public partial class OutputPageView : UserControl
{
    private static readonly string[] _fileOptions = ["*.txt"];
    private readonly TextBlock _logs;

    public OutputPageView()
    {
        InitializeComponent();

        _logs = this.FindControl<TextBlock>("LogTextBlock")!;
    }

    private void CopyRequested(object? sender, RoutedEventArgs e)
    {
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard is null) return;

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await clipboard.SetTextAsync(_logs.Text);
        });
    }

    private void SaveRequested(object? sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var file = await TopLevel.GetTopLevel(this)?.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save VRCFaceTracking.Avalonia Log File",
                SuggestedFileName = "log.txt",
                DefaultExtension = "txt",
                FileTypeChoices =
                [
                    new FilePickerFileType("Text File")
                    {
                        Patterns = _fileOptions
                    }
                ]
            })!;

            if (file is not null)
            {
                await using var stream = await file.OpenWriteAsync();
                await using var streamWriter = new StreamWriter(stream);
                await streamWriter.WriteLineAsync(_logs.Text);
            }
        });
    }
}
