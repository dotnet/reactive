// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Threading.Tasks;

namespace System.Reactive.Threading
{
    /// <summary>
    /// Synchronization primitive that provides <see cref="System.Threading.Monitor"/>-style
    /// exclusive access semantics, but with an asynchronous API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This enables <see cref="AsyncObservable.Synchronize{TSource}(IAsyncObservable{TSource}, IAsyncGate)"/>
    /// and <see cref="AsyncObserver.Synchronize{TSource}(IAsyncObserver{TSource}, IAsyncGate)"/>
    /// to be used to synchronize access to an observer with a custom synchronization primitive.
    /// </para>
    /// <para>
    /// These methods model the equivalents for <see cref="IObservable{T}"/> and <see cref="IObserver{T}"/>
    /// in <c>System.Reactive</c>. Those offer overloads accepting a 'gate' parameter, and if you pass
    /// the same object to multiple calls to these methods, they will all synchronize their operation
    /// through that same gate object. The <c>gate</c> parameter in those methods is of type
    /// <see cref="System.Object"/>, which works because all .NET objects have an associated monitor.
    /// (It's created on demand when you first use <c>lock</c> or something equivalent.)
    /// </para>
    /// <para>
    /// That approach is problematic in an async world, because this built-in monitor blocks the
    /// calling thread when contention occurs. The basic idea of AsyncRx.NET is to avoid such
    /// blocking. It can't always be avoided, and in cases where we can be certain that lock
    /// acquisition times will be short, the conventional .NET monitor is still a good choice.
    /// But since these <c>Synchronize</c> operators allow the caller to pass a gate which the
    /// application code itself might lock, we have no control over how long the lock might be
    /// held. So it would be inappropriate to use a monitor here.
    /// </para>
    /// <para>
    /// Since the .NET runtime does not currently offer any asynchronous direct equivalent to
    /// monitor, this interface defines the required API. The <see cref="AsyncGate"/> class
    /// provide a basic implementation. If applications require additional features, (e.g.
    /// if they want cancellation support when the application tries to acquire the lock)
    /// they can provide their own implementation.
    /// </para>
    /// </remarks>
    public interface IAsyncGate
    {
        /// <summary>
        /// Acquires the lock.
        /// </summary>
        /// <returns>
        /// A task that completes when the lock has been acquired, returning an <see cref="IAsyncGateReleaser"/>
        /// with which to release the lock.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Applications release the lock by calling <see cref="IAsyncGateReleaser.Release"/> on the object
        /// returned by this method. Typically this is done with a <c>using</c> statement or declaration by
        /// using the <see cref="AsyncGateExtensions.LockAsync(IAsyncGate)"/> extension method.
        /// </para>
        /// </remarks>
        public ValueTask<IAsyncGateReleaser> AcquireAsync();
    }
}
