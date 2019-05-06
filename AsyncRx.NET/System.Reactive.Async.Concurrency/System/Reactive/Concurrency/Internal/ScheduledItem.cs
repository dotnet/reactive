// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents a work item that has been scheduled.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    internal sealed class ScheduledItem<TAbsolute> : IComparable<ScheduledItem<TAbsolute>>, IAsyncCancelable
        where TAbsolute : IComparable<TAbsolute>
    {
        private readonly Func<Task<IAsyncDisposable>> _action;
        private readonly IComparer<TAbsolute> _comparer;
        private readonly SingleAssignmentAsyncDisposable _disposable = new SingleAssignmentAsyncDisposable();

        public ScheduledItem(Func<Task<IAsyncDisposable>> action, TAbsolute dueTime, IComparer<TAbsolute> comparer)
        {
            _action = action;
            DueTime = dueTime;
            _comparer = comparer;
        }

        public ScheduledItem(Func<Task<IAsyncDisposable>> action, TAbsolute dueTime)
            : this(action, dueTime, Comparer<TAbsolute>.Default) { }

        /// <summary>
        /// Gets the absolute time at which the item is due for invocation.
        /// </summary>
        public TAbsolute DueTime { get; }

        /// <summary>
        /// Invokes the work item.
        /// </summary>
        public async Task Invoke()
        {
            if (_disposable.IsDisposed) return;
            var disposable = await _action().ConfigureAwait(false);
            await _disposable.AssignAsync(disposable).ConfigureAwait(false);
        }

        public Task DisposeAsync() => _disposable.DisposeAsync();

        public bool IsDisposed { get => _disposable.IsDisposed; }


        #region Inequality

        /// <summary>
        /// Compares the work item with another work item based on absolute time values.
        /// </summary>
        /// <param name="other">Work item to compare the current work item to.</param>
        /// <returns>Relative ordering between this and the specified work item.</returns>
        /// <remarks>The inequality operators are overloaded to provide results consistent with the <see cref="IComparable"/> implementation. Equality operators implement traditional reference equality semantics.</remarks>
        public int CompareTo(ScheduledItem<TAbsolute> other)
        {
            // MSDN: By definition, any object compares greater than null, and two null references compare equal to each other.
            if (other is null)
            {
                return 1;
            }

            return _comparer.Compare(DueTime, other.DueTime);
        }

        /// <summary>
        /// Determines whether one specified <see cref="ScheduledItem{TAbsolute}" /> object is due before a second specified <see cref="ScheduledItem{TAbsolute}" /> object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if the <see cref="DueTime"/> value of left is earlier than the <see cref="DueTime"/> value of right; otherwise, <c>false</c>.</returns>
        /// <remarks>This operator provides results consistent with the <see cref="IComparable"/> implementation.</remarks>
        public static bool operator <(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) < 0;

        /// <summary>
        /// Determines whether one specified <see cref="ScheduledItem{TAbsolute}" /> object is due before or at the same of a second specified <see cref="ScheduledItem{TAbsolute}" /> object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if the <see cref="DueTime"/> value of left is earlier than or simultaneous with the <see cref="DueTime"/> value of right; otherwise, <c>false</c>.</returns>
        /// <remarks>This operator provides results consistent with the <see cref="IComparable"/> implementation.</remarks>
        public static bool operator <=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) <= 0;

        /// <summary>
        /// Determines whether one specified <see cref="ScheduledItem{TAbsolute}" /> object is due after a second specified <see cref="ScheduledItem{TAbsolute}" /> object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if the <see cref="DueTime"/> value of left is later than the <see cref="DueTime"/> value of right; otherwise, <c>false</c>.</returns>
        /// <remarks>This operator provides results consistent with the <see cref="IComparable"/> implementation.</remarks>
        public static bool operator >(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) > 0;

        /// <summary>
        /// Determines whether one specified <see cref="ScheduledItem{TAbsolute}" /> object is due after or at the same time of a second specified <see cref="ScheduledItem{TAbsolute}" /> object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if the <see cref="DueTime"/> value of left is later than or simultaneous with the <see cref="DueTime"/> value of right; otherwise, <c>false</c>.</returns>
        /// <remarks>This operator provides results consistent with the <see cref="IComparable"/> implementation.</remarks>
        public static bool operator >=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) >= 0;

        #endregion

        #region Equality

        /// <summary>
        /// Determines whether two specified <see cref="ScheduledItem{TAbsolute, TValue}" /> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if both <see cref="ScheduledItem{TAbsolute, TValue}" /> are equal; otherwise, <c>false</c>.</returns>
        /// <remarks>This operator does not provide results consistent with the IComparable implementation. Instead, it implements reference equality.</remarks>
        public static bool operator ==(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => ReferenceEquals(left, right);

        /// <summary>
        /// Determines whether two specified <see cref="ScheduledItem{TAbsolute, TValue}" /> objects are inequal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if both <see cref="ScheduledItem{TAbsolute, TValue}" /> are inequal; otherwise, <c>false</c>.</returns>
        /// <remarks>This operator does not provide results consistent with the IComparable implementation. Instead, it implements reference equality.</remarks>
        public static bool operator !=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => !(left == right);

        /// <summary>
        /// Determines whether a <see cref="ScheduledItem{TAbsolute}" /> object is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to the current <see cref="ScheduledItem{TAbsolute}" /> object.</param>
        /// <returns><c>true</c> if the obj parameter is a <see cref="ScheduledItem{TAbsolute}" /> object and is equal to the current <see cref="ScheduledItem{TAbsolute}" /> object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => ReferenceEquals(this, obj);

        /// <summary>
        /// Returns the hash code for the current <see cref="ScheduledItem{TAbsolute}" /> object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => base.GetHashCode();

        #endregion
    }
}
