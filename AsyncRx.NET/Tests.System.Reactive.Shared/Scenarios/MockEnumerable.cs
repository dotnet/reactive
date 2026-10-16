// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>An enumerable that records when each of its enumerations began and ended.</summary>
/// <typeparam name="T">The type of the elements.</typeparam>
/// <param name="scheduler">The scheduler whose clock stamps each enumeration.</param>
/// <param name="underlyingEnumerable">The sequence to enumerate.</param>
/// <remarks>
/// Rx.NET's <c>MockEnumerable</c>, which several of its test classes share, so it is a top-level
/// type here too. Each <c>GetEnumerator</c> adds an <see cref="Enumeration"/> stamped with the
/// scheduler's clock, completed when that enumerator is disposed, so a scenario asserts on how
/// an operator enumerated what the selector returned (<c>new Enumeration(210, 210)</c> for one
/// that began and ended at tick 210). An enumerable is the same type on both targets, so unlike
/// the other doubles this one needs no per-target object.
/// </remarks>
public sealed class MockEnumerable<T>(
    TestSchedulerRef scheduler,
    IEnumerable<T> underlyingEnumerable) : IEnumerable<T>
{
    /// <summary>Each enumeration so far, as the ticks it began and ended at.</summary>
    public List<Enumeration> Enumerations { get; } = [];

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() =>
        new MockEnumerator(scheduler, Enumerations, underlyingEnumerable.GetEnumerator());

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class MockEnumerator : IEnumerator<T>
    {
        private readonly TestSchedulerRef _scheduler;
        private readonly List<Enumeration> _enumerations;
        private readonly IEnumerator<T> _enumerator;
        private readonly int _index;
        private bool _disposed;

        public MockEnumerator(
            TestSchedulerRef scheduler,
            List<Enumeration> enumerations,
            IEnumerator<T> enumerator)
        {
            _scheduler = scheduler;
            _enumerations = enumerations;
            _enumerator = enumerator;
            _index = enumerations.Count;
            enumerations.Add(new Enumeration(scheduler.Clock));
        }

        public T Current
        {
            get
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return _enumerator.Current;
            }
        }

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _enumerator.Dispose();
                _enumerations[_index] = _enumerations[_index] with { End = _scheduler.Clock };
            }
        }

        public bool MoveNext()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            _enumerator.Reset();
        }
    }
}
