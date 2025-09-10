using System.Diagnostics.CodeAnalysis;

namespace RxGauntlet.Build;

public record PackageIdAndVersion(
    string PackageId,
    string Version)
{
    public static bool TryParse(string input, [NotNullWhen(true)] out PackageIdAndVersion? packageIdAndVersion)
    {
        if (string.IsNullOrWhiteSpace(input) ||
            input.Split(',') is not [string id, string version])
        {
            packageIdAndVersion = null;
            return false;
        }

        packageIdAndVersion = new PackageIdAndVersion(id.Trim(), version.Trim());
        return true;
    }
}
