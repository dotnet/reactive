// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Order;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// The shared BenchmarkDotNet configuration for the whole suite. Wired once in <c>Program</c>,
    /// so individual benchmark classes do not need to repeat <c>[MemoryDiagnoser]</c>.
    /// </summary>
    /// <remarks>
    /// Memory is a first-class output: <see cref="MemoryDiagnoser"/> surfaces Gen0/1/2 and Allocated
    /// bytes/op, and the full JSON + CSV exporters emit the machine-readable artifacts the
    /// performance-improvement plan is generated from. Opt-in profilers (ETW / EventPipe / disassembly /
    /// threading) are enabled per-run via CLI switches, layered on top of this config.
    /// </remarks>
    public static class RxBenchmarkConfig
    {
        public static IConfig Create() =>
            ManualConfig.Create(DefaultConfig.Instance)
                .AddDiagnoser(MemoryDiagnoser.Default)
                // DefaultConfig already provides the GitHub markdown + CSV exporters; only add the full JSON
                // exporter, which is the machine-readable artifact the perf-improvement plan is generated from.
                .AddExporter(JsonExporter.Full)
                .AddColumn(PerElementColumn.Time, PerElementColumn.Allocated)
                .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
    }
}
