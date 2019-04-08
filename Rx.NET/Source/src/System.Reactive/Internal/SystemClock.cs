// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Provides access to local system clock services.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SystemClock
    {
        private static readonly Lazy<ISystemClock> ServiceSystemClock = new Lazy<ISystemClock>(InitializeSystemClock);
        private static readonly Lazy<INotifySystemClockChanged> ServiceSystemClockChanged = new Lazy<INotifySystemClockChanged>(InitializeSystemClockChanged);
        internal static readonly HashSet<WeakReference<LocalScheduler>> SystemClockChanged = new HashSet<WeakReference<LocalScheduler>>();
        private static IDisposable _systemClockChangedHandlerCollector;

        private static int _refCount;

        /// <summary>
        /// Gets the local system clock time.
        /// </summary>
        public static DateTimeOffset UtcNow => ServiceSystemClock.Value.UtcNow;

        /// <summary>
        /// Adds a reference to the system clock monitor, causing it to be sending notifications.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the system doesn't support sending clock change notifications.</exception>
        public static void AddRef()
        {
            if (Interlocked.Increment(ref _refCount) == 1)
            {
                ServiceSystemClockChanged.Value.SystemClockChanged += OnSystemClockChanged;
            }
        }

        /// <summary>
        /// Removes a reference to the system clock monitor, causing it to stop sending notifications
        /// if the removed reference was the last one.
        /// </summary>
        public static void Release()
        {
            if (Interlocked.Decrement(ref _refCount) == 0)
            {
                ServiceSystemClockChanged.Value.SystemClockChanged -= OnSystemClockChanged;
            }
        }

        internal static void OnSystemClockChanged(object sender, SystemClockChangedEventArgs e)
        {
            lock (SystemClockChanged)
            {
                // create a defensive copy as the callbacks may change the hashset
                var copySystemClockChanged = new List<WeakReference<LocalScheduler>>(SystemClockChanged);
                foreach (var entry in copySystemClockChanged)
                {
                    if (entry.TryGetTarget(out var scheduler))
                    {
                        scheduler.SystemClockChanged(sender, e);
                    }
                }
            }
        }

        private static ISystemClock InitializeSystemClock()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return PlatformEnlightenmentProvider.Current.GetService<ISystemClock>() ?? new DefaultSystemClock();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private static INotifySystemClockChanged InitializeSystemClockChanged()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return PlatformEnlightenmentProvider.Current.GetService<INotifySystemClockChanged>() ?? new DefaultSystemClockMonitor();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        internal static void Register(LocalScheduler scheduler)
        {
            //
            // LocalScheduler maintains per-instance work queues that need revisiting
            // upon system clock changes. We need to be careful to avoid keeping those
            // scheduler instances alive by the system clock monitor, so we use weak
            // references here. In particular, AsyncLockScheduler in ImmediateScheduler
            // can have a lot of instances, so we need to collect spurious handlers
            // at regular times.
            //
            lock (SystemClockChanged)
            {
                SystemClockChanged.Add(new WeakReference<LocalScheduler>(scheduler));

                if (SystemClockChanged.Count == 1)
                {
                    _systemClockChangedHandlerCollector = ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(CollectHandlers, TimeSpan.FromSeconds(30));
                }
                else if (SystemClockChanged.Count % 64 == 0)
                {
                    CollectHandlers();
                }
            }
        }

        private static void CollectHandlers()
        {
            //
            // The handler collector merely collects the WeakReference<T> instances
            // that are kept in the hash set. The underlying scheduler itself will
            // be collected due to the weak reference. Unfortunately, we can't use
            // the ConditionalWeakTable<TKey, TValue> type here because we need to
            // be able to enumerate the keys.
            //
            lock (SystemClockChanged)
            {
                var remove = default(HashSet<WeakReference<LocalScheduler>>);

                foreach (var handler in SystemClockChanged)
                {
                    if (!handler.TryGetTarget(out _))
                    {
                        if (remove == null)
                        {
                            remove = new HashSet<WeakReference<LocalScheduler>>();
                        }

                        remove.Add(handler);
                    }
                }

                if (remove != null)
                {
                    foreach (var handler in remove)
                    {
                        SystemClockChanged.Remove(handler);
                    }
                }

                if (SystemClockChanged.Count == 0)
                {
                    _systemClockChangedHandlerCollector.Dispose();
                    _systemClockChangedHandlerCollector = null;
                }
            }
        }
    }

    /// <summary>
    /// (Infrastructure) Provides access to the local system clock.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISystemClock
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }

    /// <summary>
    /// (Infrastructure) Provides a mechanism to notify local schedulers about system clock changes.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface INotifySystemClockChanged
    {
        /// <summary>
        /// Event that gets raised when a system clock change is detected.
        /// </summary>
        event EventHandler<SystemClockChangedEventArgs> SystemClockChanged;
    }

    /// <summary>
    /// (Infrastructure) Event arguments for system clock change notifications.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SystemClockChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new system clock notification object with unknown old and new times.
        /// </summary>
        public SystemClockChangedEventArgs()
            : this(DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
        {
        }

        /// <summary>
        /// Creates a new system clock notification object with the specified old and new times.
        /// </summary>
        /// <param name="oldTime">Time before the system clock changed, or DateTimeOffset.MinValue if not known.</param>
        /// <param name="newTime">Time after the system clock changed, or DateTimeOffset.MaxValue if not known.</param>
        public SystemClockChangedEventArgs(DateTimeOffset oldTime, DateTimeOffset newTime)
        {
            OldTime = oldTime;
            NewTime = newTime;
        }

        /// <summary>
        /// Gets the time before the system clock changed, or DateTimeOffset.MinValue if not known.
        /// </summary>
        public DateTimeOffset OldTime { get; }

        /// <summary>
        /// Gets the time after the system clock changed, or DateTimeOffset.MaxValue if not known.
        /// </summary>
        public DateTimeOffset NewTime { get; }
    }
}
