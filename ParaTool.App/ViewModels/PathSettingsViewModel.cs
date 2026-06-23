using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParaTool.App.Services;

namespace ParaTool.App.ViewModels;

public partial class PathSettingsViewModel : ViewModelBase
{
    [ObservableProperty] private string? _modsFolderPath;
    [ObservableProperty] private string? _ampPakFilePath;
    [ObservableProperty] private string? _validationError;

    /// <summary>
    /// Fired when the user clicks Apply &amp; Rescan.
    /// Args: (modsFolder, ampPakPath) — ampPakPath is null when auto-detect is requested.
    /// </summary>
    public event Action<string, string?>? Applied;

    /// <summary>Fired when the user clicks Cancel.</summary>
    public event Action? Cancelled;

    public PathSettingsViewModel(UiSettings settings)
    {
        _modsFolderPath = settings.ModsFolderPath;
        _ampPakFilePath  = settings.AmpPakFilePath;
    }

    [RelayCommand]
    private void Apply()
    {
        if (string.IsNullOrWhiteSpace(ModsFolderPath))
        {
            ValidationError = "Please select a Mods folder to scan.";
            return;
        }

        ValidationError = null;

        // Persist to settings
        var settings = UiSettingsService.Load();
        settings.ModsFolderPath = ModsFolderPath!.Trim();
        settings.AmpPakFilePath = string.IsNullOrWhiteSpace(AmpPakFilePath)
            ? null
            : AmpPakFilePath!.Trim();
        UiSettingsService.Save(settings);

        Applied?.Invoke(settings.ModsFolderPath, settings.AmpPakFilePath);
    }

    [RelayCommand]
    private void Cancel() => Cancelled?.Invoke();

    [RelayCommand]
    private void ClearAmpPath()
    {
        AmpPakFilePath  = null;
        ValidationError = null;
    }
}
