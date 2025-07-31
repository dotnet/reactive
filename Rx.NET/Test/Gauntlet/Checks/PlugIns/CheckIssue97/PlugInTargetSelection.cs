// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

using PlugIn.HostDriver;

using RxGauntlet.Build;

namespace CheckIssue97;

/// <summary>
/// Determines which pairs of plug-in TFMs to test.
/// </summary>
/// <remarks>
/// There are many TFMs a plug-in could target, but for any particular version of Rx.NET, most of these will
/// resolve to the same target assembly in Rx.NET. (E.g., with Rx 6.0.1, it is not useful building both
/// a net472 and a net481 plug-in because both end up resolving to the net472 Rx target). So this determines
/// a minimal set of TFMs where each produces a plug-in that resolves to a different Rx.NET target. It then
/// reports a series of pairings in which the first and second plug-in will use different Rx.NET targets.
/// Note that order is significant, so for any pair of TFMs this produces, it will also produce the
/// same pair in reverse order.
/// </remarks>
internal class PlugInTargetSelection
{
    private static readonly string[] HostRuntimes =
    [
        "net462",
        "net472",
        "net481",
        "net8.0",
        "net9.0"
    ];

    private static readonly string[] EveryTfmWeAreConsidering =
        [
            "net11",
            "net20",
            "net35",
            "net40",
            "net403",
            "net45",
            "net451",
            "net452",
            "net46",
            //"net461",
            "net462",
            "net47",
            //"net471",
            "net472",
            "net48",
            "net481",
            //"netcoreapp1.0",
            //"netcoreapp1.1",
            //"netcoreapp2.0",
            //"netcoreapp2.1",
            //"netcoreapp2.2",
            "netcoreapp3.0",
            "netcoreapp3.1",
            "net5.0",
            "net6.0",
            "net7.0",
            "net8.0",
            "net9.0",
            "net5.0-windows10.0.19041",
            "net6.0-windows10.0.19041",
            "net7.0-windows10.0.19041",
            "net8.0-windows10.0.19041",
            "net9.0-windows10.0.19041",
        ];

    public static async Task<List<Scenario>> GetPlugInTfmPairingsAsync(
        PackageIdAndVersion[] packages, string? packageSource)
    {
        // For the selected Rx version, we want to determine a list of TFMs of interest. The goal here is to
        // get a list of plug-ins all built against the same Rx version, but where each plug-in ends up selecting
        // a different target from the Rx NuGet Package.
        //
        // For example the Rx3.0 NuGet package has netstandard1.0, netstandard1.1, netstandard1.3, net45, and
        // net46 targets. If we build plug-ins against Rx3.0, where one targets net45, and another targets net46,
        // then that has covered every possible .NET Framework TFM for which Rx3.0 offers a distinct target.
        // As for the .NET Standard targets, the only runtimes that do not support .NET Standard 1.3 are the
        // ancient UAP8.x runtimes (which we don't have a way of testing, and which are very ancient, so we
        // will ignore them) and .NET Framework 4.5.2 and earlier. However, for any .NET Framework version that
        // can use Rx 3.0, the build will select a .NET FX target of Rx. The only situation in which we can make
        // a plug-in choose a .NET Standard target is to use .NET (non-FX), and in all cases, this will always select
        // the netstandard1.3 target. So in practice, a single plug-in targeting .net 8.0 will cover all possibilities.
        //
        // So in the case of Rx3.0, if our host TFM is .NET Framework, then since plug-ins with net45 and net46
        // TFMs will cover all availabile options when it comes to selecting the available targets, we need just
        // (net45, net46) and (net46, net45) as the pairs of plug-in TFMs to test. If the host TFM is .NET, then
        // there are no pairings because we needed only the net8.0 plug-in, and since we never pair a plug-in with
        // itself, there are no pairings.

        var mainPackageId = packages[0].PackageId;
        var mainPackageVersion = packages[0].Version;

        var source = packageSource ?? "https://api.nuget.org/v3/index.json";
        var logger = NullLogger.Instance;
        var cache = new SourceCacheContext();

        var repository = Repository.Factory.GetCoreV3(source);
        var resource = await repository.GetResourceAsync<FindPackageByIdResource>();

        // We're now going to work out which target frameworks we could build the plug-in for, given
        // the frameworks supported by the Rx version we've got. In cases where there's a single package,
        // it's straightforward, but when we have multiple packages, typically the UI packages support
        // a narrower set.
        Dictionary<PackageIdAndVersion, List<NuGetFramework>> frameworksByPackage = [];
        foreach (var package in packages)
        {
            using var packageStream = new MemoryStream();
            if (!await resource.CopyNupkgToStreamAsync(
                package.PackageId, new NuGetVersion(package.Version), packageStream, cache, logger, CancellationToken.None))
            {
                throw new InvalidOperationException($"Could not download {mainPackageId} {mainPackageVersion} from {source}");
            }

            packageStream.Position = 0;
            using var reader = new PackageArchiveReader(packageStream);

            // Some packages (e.g. System.Reactive.Linq 3.0.0) report file entries for what should be folders.
            // For example, we get lib/netstandard1.0/, which is a zero-length file. I think something went
            // wrong when the package was created back in 2016, because these should not be files. (There are files
            // inside these folders, such as lib/netstandard1.0/System.Reactive.Linq.dll, but the parent folder
            // itself really shouldn't be reported as a file.) It does not report any such bogus entries for
            // the other target frameworks - this just seems to afflict the netstandard targets.
            // These cause PackageArchiveReader to report these as belonging to an 'Any' framework, which
            // does not accurately represent what the package actually offers, so we strip these out.
            var libItemsExcludingBogusFolderEntries = reader.GetLibItems()
                .Where(x => x.Items.All(item => item.Split('/') is [.., string final] && final.Length > 0));
            var packageFrameworks = libItemsExcludingBogusFolderEntries
                .Select(x => x.TargetFramework)
                .Where(fx => fx is not null)
                .Distinct()
                .ToList();

            frameworksByPackage.Add(package, packageFrameworks);
        }

        var reducer = new FrameworkReducer();

        // At this point, as Carmel has pointed out, if we have a netstandard target, we know
        // we've got a potentially tricky case, because it might not be obvious which target
        // a plug-in needs to specify to get the netstandardX.X target. But for everything
        // else, maybe it is as straightforward as just using the TFM in this list. (So if the
        // list includes `net45`, we build a `net45`-targetting plug-in.)
        //
        // If it is a netstandard target, we only need to consider versions of .NET older
        // than the oldest .NET Framework target offered by the version of Rx we're testing.
        //
        // However the brute-force approach, in which we just try every TFM we are considering
        // and ask the NuGet library which Rx target it would select, seems to work well enough.

        List<Scenario> results = [];
        List<Scenario> fallbackResults = [];
        foreach (var hostRuntimeTfm in HostRuntimes)
        {
            // Assuming we're running on a supported version of Windows 11 or later. This ensures that when we get to
            // plugins with OS-specific TFMs, the host runtime version comes out as higher than or equal to the plugin TFM
            // in cases where they match on major and minor versions.
            var effectiveHostTfm = TargetFrameworkMonikerParser.TryParseNetFxMoniker(hostRuntimeTfm, out _, out _)
                ? hostRuntimeTfm
                : $"{hostRuntimeTfm}-windows10.0.22631";

            var hostFramework = NuGetFramework.Parse(effectiveHostTfm);

            // This filters out Rx targets where none of the TFMs we could use in the plug-ins to select
            // that target are compatible with the host runtime.
            var plugInTfmsCompatibleWithHostRuntime = EveryTfmWeAreConsidering
                .Select(NuGetFramework.Parse)
                .Where(item => reducer.GetNearest(hostFramework, [item]) is not null);
            var plugInTfmsWithNearestRxMatch = plugInTfmsCompatibleWithHostRuntime
                .SelectMany(plugInFramework =>
                {
                    // The code below seems to do what we want, but I think that's partly by luck.
                    // I think the correct logic at this point is to ask:
                    //  for each package, what are all the targets that this plugInFramework could consume?
                    //  disregard any of those targets with which not all packages are compatible
                    //  of the remaining targets, pick 'nearest' for the plugInFramework

                    var nearestAvailableFrameworksAcrossAllPackages = frameworksByPackage.Values
                        .Select(fs => reducer.GetNearest(plugInFramework, fs))
                        .OfType<NuGetFramework>()
                        .Distinct();
                    // If the plug-in TFM is compatible with the host runtime, we can determine which Rx target it will select.
                    (NuGetFramework PluginTfm, string RxTfm)[] result =
                        (reducer.GetNearest(plugInFramework, nearestAvailableFrameworksAcrossAllPackages) is NuGetFramework selectedRxTarget &&
                         frameworksByPackage.Values.All(
                                frameworks => reducer.GetNearest(plugInFramework, frameworks) is not null))
                            ? [(plugInFramework, selectedRxTarget.GetShortFolderName())]
                            : [];

                    return result;
                });

            var plugInTfmsBySelectedRxTarget = plugInTfmsWithNearestRxMatch
                .GroupBy(item => item.RxTfm)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p => p.PluginTfm).OrderBy(x => x.Version).ToList());

            List<string> selectedPlugInTfms = [];
            var isFallback = false;
            if (plugInTfmsBySelectedRxTarget.Count == 1)
            {
                // No matter which TFM we choose for the plug-in, it will always select the same Rx target. This
                // happens with Rx 4.4 on .NET FX, for example. (And this is the reason that Rx 4.4 does not exhibit
                // the plug-in issue 97 bug. It offers just one .NET FX target: net46. So when we build the plug-in for
                // net46, or any later version of .NET FX, it will always select the net46 version of System.Reactive.
                // And although Rx supplies a netstandard2.0 target, there is no .NET FX TFM we can build for that will
                // cause that copy of Rx to be selected. .NET FX didn't offer netstandard2.0 support before net462, and
                // the NuGet resolution rules deem net46 to be a better match than netstandard2.0 for all versions of
                // .NET FX that support netstandard2.0.)
                //
                // The effect of this is that our normal logic below will decide that it doesn't need to run any test
                // combinations at all, because normally we select exactly one plug-in TFM for each reachable Rx target.
                // Since there's only one reachable Rx target in this case that means we would select just one plug-in TFM,
                // meaning there are no available pairings. (We never pair a plug-in with itself.)
                //
                // However, it's useful to ensure we have at least one test run in cases like this because otherwise,
                // versions that have this characteristic may become invisible when we load the results into analytics
                // tooling. Using the absence of data to signify that a problem isn't possible can be tricky to deal
                // with when it comes to visualizing the results. So in cases like this, we pick two TFMs, even
                // though we know that they will correctly resolve to the same Rx target. (There might not actually be
                // two such TFM - with the latest versions of Rx running on a net472 host, net472 is likely the only
                // TFM. But we should still pick up two options when running on a net481 host.)
                selectedPlugInTfms.AddRange(plugInTfmsBySelectedRxTarget.Values.Single()
                    .Take(2)
                    .Select(f => f.GetShortFolderName()));

                isFallback = true;
            }
            else
            {
                foreach ((var key, var plugInTfms) in plugInTfmsBySelectedRxTarget)
                {
                    // We could just do this to let NuGet pick which it thinks is the best of the available TFMs for
                    // the host runtime:
                    // NuGetFramework? candidate = reducer.GetNearest(hostFramework, plugInTfms);
                    // However, that tends to pick the highest possible version. E.g., for Rx 6.0.1 in the net472 host,
                    // its pick for the netstandard2.0 NuGet target is a plug-in TFM of net471.
                    // Back when we did all this by hand, we chose net462 as the plug-in TFM that resolved to netstandard2.0.
                    // More generally, we prefer the oldest TFM that works. (This whole test scenario is essentially recreating
                    // legacy setups so the older TFMs usually better reflect the real-life scenarios these tests represent.)
                    var candidate = plugInTfms
                        .Select(plugInTfm => reducer.GetNearest(hostFramework, [plugInTfm]))
                        .FirstOrDefault(framework => framework is not null);
                    if (candidate is not null)
                    {
                        selectedPlugInTfms.Add(candidate.GetShortFolderName());
                    }
                    else
                    {
                        // If we cannot find a compatible TFM, then we cannot test this Rx target.
                        Console.WriteLine($"No compatible plug-in TFM found for Rx target {key} with host runtime {hostRuntimeTfm}");
                    }
                } 
            }

            (isFallback ? fallbackResults : results).AddRange(
                from firstPlugIn in selectedPlugInTfms
                from secondPlugIn in selectedPlugInTfms
                where firstPlugIn != secondPlugIn
                select new Scenario(hostRuntimeTfm, new PlugInDescriptor(firstPlugIn, packages, packageSource), new PlugInDescriptor(secondPlugIn, packages, packageSource)));
        }

        return results.Count > 0 ? results : fallbackResults.Take(1).ToList();
    }
}
