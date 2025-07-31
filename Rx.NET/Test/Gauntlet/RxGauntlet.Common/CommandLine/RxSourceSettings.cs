// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.Build;

using Spectre.Console;
using Spectre.Console.Cli;

using System.ComponentModel;
using System.Diagnostics;

namespace RxGauntlet.CommandLine;

public class RxSourceSettings : CommandSettings
{
    private PackageIdAndVersion? _parsedRxMainPackage;
    private PackageIdAndVersion? _parsedRxLegacyPackage;
    private PackageIdAndVersion[]? _parsedRxUiFrameworkPackages;

    [Description("The URL of an additional NuGet package source, or the file path of a local package store. (The public NuGet feed will remain available.)")]
    [CommandOption("--package-source")]
    public string? PackageSource { get; init; }

    [Description("Package (as PackageId,Version, e.g. System.Reactive.Net,7.0.0-preview.17.g58342773bd) to use as the main Rx package (replacing the existing System.Reactive PackageReference where appropriate)")]
    [CommandOption("--rx-main-package")]
    public required string RxMainPackage { get; init; }

    [Description("Package (as PackageId,Version, e.g. System.Reactive,7.0.0-preview.17.g58342773bd) to use as a compatibility facade replacing the legacy System.Reactive (typically used only when a dependency is using an old System.Reactive, and the app wants to use a newer one). In cases where System.Reactive remains as the main package, this will not be set.")]
    [CommandOption("--rx-legacy-package")]
    public string? RxLegacyPackage { get; init; }

    [Description("Package (as PackageId,Version, e.g. System.Reactive.For.Wpf,7.0.0-preview.17.g58342773bd) to reference when UI-framework-specific Rx functionatliy is required")]
    [CommandOption("--rx-ui-package")]
    public string[] RxUiFrameworkPackages { get; init; } = [];

    public PackageIdAndVersion RxMainPackageParsed => GetParsedPackage(ref _parsedRxMainPackage);
    public PackageIdAndVersion? RxLegacyPackageParsed => RxLegacyPackage is null
        ? null
        : GetParsedPackage(ref _parsedRxLegacyPackage);

    public PackageIdAndVersion[] RxUiFrameworkPackagesParsed
    {
        get
        {
            if (_parsedRxUiFrameworkPackages is not null)
            {
                return _parsedRxUiFrameworkPackages;
            }

            if (RxUiFrameworkPackages.Length == 0)
            {
                _parsedRxUiFrameworkPackages = [];
            }
            else
            {
                var rxPackagesValidationResult = Validate();
                if (!rxPackagesValidationResult.Successful)
                {
                    throw new InvalidOperationException($"Settings are invalid: {rxPackagesValidationResult.Message}");
                }

                Debug.Assert(_parsedRxUiFrameworkPackages is not null, "RxPackagesParsed should have been set by ValidateRxPackages.");
            }

            return _parsedRxUiFrameworkPackages;
        }
    }

    /// <summary>
    /// Gets all of the Rx packages, starting with the one in <see cref="RxMainPackageParsed"/>, and then,
    /// if present, <see cref="RxLegacyPackageParsed"/>, followed by <see cref="RxUiFrameworkPackagesParsed"/>.
    /// </summary>
    /// <returns></returns>
    public PackageIdAndVersion[] GetAllParsedPackages() =>
        [
            RxMainPackageParsed,

            ..RxLegacyPackageParsed is PackageIdAndVersion legacy
                ? [legacy] : Array.Empty<PackageIdAndVersion>(),

            ..RxUiFrameworkPackagesParsed
        ];

    /// <summary>
    /// Validates the command line argument(s).
    /// </summary>
    /// <returns>A value indicating whether the settings are valid.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this object's properties are set in a way that we do not expect from Spectre Console CLI.
    /// </exception>
    public override ValidationResult Validate()
    {
        // Spectre Console will never make the following properties null, so this can only mean that we're being used
        // in an unsupported way (probably not via Spectre.Console) so we throw instead of reporting a validation
        // failure.

        if (RxMainPackage is not string rxMainPackage)
        {
            throw new InvalidOperationException($"{nameof(RxMainPackage)} must not be null.");
        }

        if (RxUiFrameworkPackages is not string[] rxUiPackages)
        {
            throw new InvalidOperationException($"{nameof(RxUiFrameworkPackages)}  must not be null.");
        }

        if (!PackageIdAndVersion.TryParse(rxMainPackage, out _parsedRxMainPackage))
        {
            return ValidationResult.Error($"Invalid package specification: {RxMainPackage}. Must be <PackageId>,<Version>");
        }

        if (RxLegacyPackage is string rxLegacyPackage &&
            !PackageIdAndVersion.TryParse(rxLegacyPackage, out _parsedRxLegacyPackage))
        {
            return ValidationResult.Error($"Invalid package specification: {rxLegacyPackage}. Must be <PackageId>,<Version>");
        }

        HashSet<string> uiPackageIdsSeen = new(capacity: rxUiPackages.Length);
        var uiPackageIdsAndVersions = new PackageIdAndVersion[rxUiPackages.Length];
        for (var i = 0; i < rxUiPackages.Length; i++)
        {
            if (!PackageIdAndVersion.TryParse(rxUiPackages[i], out var packageIdAndVersion))
            {
                return ValidationResult.Error($"Invalid package specification: {rxUiPackages[i]}. Must be <PackageId>,<Version>");
            }
            if (!uiPackageIdsSeen.Add(packageIdAndVersion.PackageId))
            {
                return ValidationResult.Error($"Duplicate package id: {packageIdAndVersion.PackageId}.");
            }
            uiPackageIdsAndVersions[i] = packageIdAndVersion;
        }

        _parsedRxUiFrameworkPackages = uiPackageIdsAndVersions;
        return ValidationResult.Success();
    }

    private PackageIdAndVersion GetParsedPackage(ref PackageIdAndVersion? field)
    {
        if (field is null)
        {
            var rxPackagesValidationResult = Validate();

            if (field is null && !rxPackagesValidationResult.Successful)
            {
                throw new InvalidOperationException($"Settings are invalid: {rxPackagesValidationResult.Message}");
            }

            Debug.Assert(field is not null);
        }

        return field;
    }
}
