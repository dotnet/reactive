// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Globalization;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Display-only summary columns showing per-element cost: Mean/N (ns per element) and
    /// AllocatedBytes/N (bytes per element), where N is the <c>[Params]</c> element count.
    /// <c>OperationsPerInvoke</c> cannot express this because it must be a compile-time constant.
    /// </summary>
    /// <remarks>
    /// For fan-out classes (SelectMany/Switch/Merge and friends) N is the inner-subscription count
    /// with total work pinned, so there the figure reads as per-subscription rather than per-element.
    /// The columns never enter the JSON export — comparison tooling recomputes per-element values
    /// from the raw <c>Statistics.Mean</c> and <c>Memory.BytesAllocatedPerOperation</c> fields.
    /// </remarks>
    public sealed class PerElementColumn : IColumn
    {
        public static readonly IColumn Time = new PerElementColumn(
            "TimePerElement", "ns/N", "Mean time divided by the N parameter (ns per element; per-subscription for fan-out classes)", isTime: true);

        public static readonly IColumn Allocated = new PerElementColumn(
            "AllocatedPerElement", "B/N", "Allocated bytes per op divided by the N parameter (bytes per element; per-subscription for fan-out classes)", isTime: false);

        private readonly bool _isTime;

        private PerElementColumn(string id, string columnName, string legend, bool isTime)
        {
            Id = id;
            ColumnName = columnName;
            Legend = legend;
            _isTime = isTime;
        }

        public string Id { get; }

        public string ColumnName { get; }

        public string Legend { get; }

        public bool AlwaysShow => false;

        public ColumnCategory Category => ColumnCategory.Custom;

        public int PriorityInCategory => _isTime ? 0 : 1;

        public bool IsNumeric => true;

        // Dimensionless on purpose: the value is self-formatted with the unit in the column name,
        // sidestepping BenchmarkDotNet's time-unit auto-scaling.
        public UnitType UnitType => UnitType.Dimensionless;

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

        public bool IsAvailable(Summary summary)
        {
            foreach (var benchmarkCase in summary.BenchmarksCases)
            {
                if (TryGetN(benchmarkCase, out _))
                {
                    return true;
                }
            }

            return false;
        }

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase) =>
            GetValue(summary, benchmarkCase, summary.Style);

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        {
            if (!TryGetN(benchmarkCase, out var n) || n <= 0)
            {
                return "-";
            }

            var report = summary[benchmarkCase];
            if (report == null)
            {
                return "-";
            }

            double? total = _isTime
                ? report.ResultStatistics?.Mean                                  // ns
                : report.GcStats.GetBytesAllocatedPerOperation(benchmarkCase);   // null without MemoryDiagnoser

            return total is double value
                ? (value / n).ToString("N2", CultureInfo.InvariantCulture)
                : "-";
        }

        private static bool TryGetN(BenchmarkCase benchmarkCase, out int n)
        {
            // Scan Items rather than use the string indexer: the indexer's behaviour for a missing
            // name is undocumented, and classes without an N param must yield "-" rather than throw.
            foreach (var parameter in benchmarkCase.Parameters.Items)
            {
                if (parameter.Name == "N" && parameter.Value is int value)
                {
                    n = value;
                    return true;
                }
            }

            n = 0;
            return false;
        }

        public override string ToString() => ColumnName;
    }
}
