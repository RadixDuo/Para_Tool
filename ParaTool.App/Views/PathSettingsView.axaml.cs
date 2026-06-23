using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ParaTool.App.ViewModels;

namespace ParaTool.App.Views;

public partial class PathSettingsView : UserControl
{
    public PathSettingsView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        var browseMods = this.FindControl<Button>("BrowseModsFolderBtn");
        if (browseMods != null)
            browseMods.Click += OnBrowseModsFolder;

        var browseAmp = this.FindControl<Button>("BrowseAmpFileBtn");
        if (browseAmp != null)
            browseAmp.Click += OnBrowseAmpFile;
    }

    private async void OnBrowseModsFolder(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions
            {
                Title = "Select Mods Folder",
                AllowMultiple = false
            });

        if (folders.Count > 0 && DataContext is PathSettingsViewModel vm)
            vm.ModsFolderPath = folders[0].Path.LocalPath;
    }

    private async void OnBrowseAmpFile(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select AMP .pak File",
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("PAK files") { Patterns = ["*.pak"] }
                ]
            });

        if (files.Count > 0 && DataContext is PathSettingsViewModel vm)
            vm.AmpPakFilePath = files[0].Path.LocalPath;
    }
}
