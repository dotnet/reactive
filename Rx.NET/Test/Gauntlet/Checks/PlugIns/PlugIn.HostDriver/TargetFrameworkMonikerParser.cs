// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;

namespace PlugIn.HostDriver;

/// <summary>
/// Decodes target framework monikers (TFMs) for .NET Framework and .NET.
/// </summary>
public partial class TargetFrameworkMonikerParser
{
    private static readonly Regex DotnetTfmRegex = DotnetTfmRegexGen();

    /// <summary>
    /// Extracs the major and minor version numbers from a .NET Framework target framework moniker (TFM).
    /// </summary>
    /// <param name="targetFrameworkMoniker">
    /// The TFM, e.g. <c>net48</c> or <c>net462</c>.
    /// </param>
    /// <param name="majorVersion">
    /// Returns the major version number.
    /// </param>
    /// <param name="minorVersionAsTwoDigitNumber">
    /// Returns the minor version number as a two-digit number. For TFMs with a two-digit minor version, this
    /// is straightforward, e.g. <c>net462</c> becomes 62. For TFMs with a single-digit minor version, it
    /// multiplies the value by 10, so <c>net46</c> becomes 60. This is to ensure that the minor version
    /// numbers sort correctly. So <c>net46</c>, c>net461</c>, and c>net462</c> are reported as 60, 61, and 62
    /// respectively. (If <c>net46</c> returned the more obvious value of 6, then that would make <c>net46</c> look
    /// like a higher version than <c>net462</c>, which is not the case.)
    /// </param>
    /// <returns></returns>
    public static bool TryParseNetFxMoniker(
        string targetFrameworkMoniker, out int majorVersion, out int minorVersionAsTwoDigitNumber)
    {
        ReadOnlySpan<char> tfm = targetFrameworkMoniker;
        var length = targetFrameworkMoniker.Length;
        if (!tfm.StartsWith("net") ||
            (length < 5) || (length > 6) ||
            !char.IsDigit(tfm[3]) ||
            !int.TryParse(tfm[4..], out var minorVersion))
        {
            majorVersion = minorVersionAsTwoDigitNumber = 0;
            return false;
        }

        majorVersion = tfm[3] - '0';
        minorVersionAsTwoDigitNumber = length == 5
            ? minorVersion * 10
            : minorVersion;

        return true;
    }

    /// <summary>
    /// Extracs the major and minor version numbers from a target framework moniker (TFM).
    /// </summary>
    /// <param name="targetFrameworkMoniker">
    /// The TFM, e.g. <c>net48</c> or <c>net6.0</c> or <c>net8.0-windows10.0.19041</c>.
    /// </param>
    /// <param name="majorVersion">
    /// Returns the major version number.
    /// </param>
    /// <param name="minorVersionAsTwoDigitNumber">
    /// Returns the minor version number as a two-digit number. Typically this is only of interest for .NET Framework
    /// TFMs, because all others seem to have a zero minor version in practice. See the documentation for
    /// <see cref="TryParseNetFxMoniker(string, out int, out int)"/> for an explanation of why this is a 'two-digit'
    /// number.
    /// </param>
    /// <param name="os">
    /// Returns the OS name if specified in the TFM. Set to <see langword="null"/> the TFM does not specify an OS.
    /// </param>
    /// <param name="osVersion">
    /// Returns the OS name if specified in the TFM. Set to <see langword="null"/> the TFM does not specify an OS,
    /// or specifies an OS but without a version.
    /// </param>
    /// <returns></returns>
    public static bool TryParseNetMoniker(
        string targetFrameworkMoniker, out int majorVersion, out int minorVersionAsTwoDigitNumber, out string? os, out string? osVersion)
    {
        os = osVersion = null;
        var match = DotnetTfmRegex.Match(targetFrameworkMoniker);
        if (!match.Success ||
            !int.TryParse(match.Groups["major"].Value, out majorVersion) ||
            !int.TryParse(match.Groups["minor"].Value, out var minorVersion))
        {
            majorVersion = minorVersionAsTwoDigitNumber = 0;
            return false;
        }

        if (match.Groups["os"].Success)
        {
            os = match.Groups["osname"].Value;
            osVersion = match.Groups["osversion"].Value;
            if (osVersion.Length == 0)
            {
                osVersion = null; // If the OS version is empty, we treat it as not specified.
            }
        }


        // Currently this is for consistency with TryParseNetFxMoniker, but if there is ever, say, a net12.11 then a
        // net12.2 or similar, this ensures that the second comes out looking higher than the first (20 > 11, instead
        // of 2 < 11).
        // In practice, everything recent has a zero minor version, so the fact that we reserve two digits makes no
        // difference.
        minorVersionAsTwoDigitNumber = minorVersion * 10;
        return true;
    }

    [GeneratedRegex(@"^net(?<major>\d+)\.(?<minor>\d+)(?<os>-(?<osname>[^\d]+)(?<osversion>[\d.]*))?$")]
    private static partial Regex DotnetTfmRegexGen();
}
