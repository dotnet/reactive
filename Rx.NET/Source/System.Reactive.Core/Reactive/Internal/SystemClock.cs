// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections;
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
        private static Lazy<ISystemClock> s_serviceSystemClock = new Lazy<ISystemClock>(InitializeSystemClock);
        private static Lazy<INotifySystemClockChanged> s_serviceSystemClockChanged = new Lazy<INotifySystemClockChanged>(InitializeSystemClockChanged);
#if NO_WEAKREFOFT
        private static readonly HashSet<WeakReference> s_systemClockChanged = new HashSet<WeakReference>();
#else
        private static readonly HashSet<WeakReference<LocalScheduler>> s_systemClockChanged = new HashSet<WeakReference<LocalScheduler>>();
#endif
        private static IDisposable s_systemClockChangedHandlerCollector;

        private static int _refCount;

        /// <summary>
        /// Gets the local system clock time.
        /// </summary>
        public static DateTimeOffset UtcNow
        {
            get { return s_serviceSystemClock.Value.UtcNow; }
        }

        /// <summary>
        /// Adds a reference to the system clock monitor, causing it to be sending notifications.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the system doesn't support sending clock change notifications.</exception>
        public static void AddRef()
        {
            if (Interlocked.Increment(ref _refCount) == 1)
            {
                s_serviceSystemClockChanged.Value.SystemClockChanged += OnSystemClockChanged;
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
                s_serviceSystemClockChanged.Value.SystemClockChanged -= OnSystemClockChanged;
            }
        }

        private static void OnSystemClockChanged(object sender, SystemClockChangedEventArgs e)
        {
            lock (s_systemClockChanged)
            {
                foreach (var entry in s_systemClockChanged)
                {
#if NO_WEAKREFOFT
                    var scheduler = entry.Target as LocalScheduler;
                    if (scheduler != null)
#else
                    var scheduler = default(LocalScheduler);
                    if (entry.TryGetTarget(out scheduler))
#endif
                    {
                        scheduler.SystemClockChanged(sender, e);
                    }
                }
            }
        }

        private static ISystemClock InitializeSystemClock()
        {
            return PlatformEnlightenmentProvider.Current.GetService<ISystemClock>() ?? new DefaultSystemClock();
        }

        private static INotifySystemClockChanged InitializeSystemClockChanged()
        {
            return PlatformEnlightenmentProvider.Current.GetService<INotifySystemClockChanged>() ?? new DefaultSystemClockMonitor();
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
            lock (s_systemClockChanged)
            {
#if NO_WEAKREFOFT
                s_systemClockChanged.Add(new WeakReference(scheduler, false));
#else
                s_systemClockChanged.Add(new WeakReference<LocalScheduler>(scheduler));
#endif

                if (s_systemClockChanged.Count == 1)
                {
                    s_systemClockChangedHandlerCollector = ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(CollectHandlers, TimeSpan.FromSeconds(30));
                }
                else if (s_systemClockChanged.Count % 64 == 0)
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
            lock (s_systemClockChanged)
            {
#if NO_WEAKREFOFT
                var remove = default(HashSet<WeakReference>);
#else
                var remove = default(HashSet<WeakReference<LocalScheduler>>);
#endif

                foreach (var handler in s_systemClockChanged)
                {
#if NO_WEAKREFOFT
                    var scheduler = handler.Target as LocalScheduler;
                    if (scheduler == null)
#else
                    var scheduler = default(LocalScheduler);
                    if (!handler.TryGetTarget(out scheduler))
#endif
                    {
                        if (remove == null)
                        {
#if NO_WEAKREFOFT
                            remove = new HashSet<WeakReference>();
#else
                            remove = new HashSet<WeakReference<LocalScheduler>>();
#endif
                        }

                        remove.Add(handler);
                    }
                }

                if (remove != null)
                {
                    foreach (var handler in remove)
                    {
                        s_systemClockChanged.Remove(handler);
                    }
                }

                if (s_systemClockChanged.Count == 0)
                {
                    s_systemClockChangedHandlerCollector.Dispose();
                    s_systemClockChangedHandlerCollector = null;
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
        public DateTimeOffset OldTime { get; private set; }

        /// <summary>
        /// Gets the time after the system clock changed, or DateTimeOffset.MaxValue if not known.
        /// </summary>
        public DateTimeOffset NewTime { get; private set; }
    }

#if NO_WEAKREFOFT
    class WeakReference<T>
        where T : class
    {
        private readonly WeakReference _weakReference;

        public WeakReference(T value)
        {
            _weakReference = new WeakReference(value);
        }

        public bool TryGetTarget(out T value)
        {
            value = (T)_weakReference.Target;
            return value != null;
        }
    }
#endif

#if NO_HASHSET
    class HashSet<T> : IEnumerable<T>
    {
        private readonly Dictionary<T, object> _dictionary = new Dictionary<T, object>();

        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dictionary.Keys.GetEnumerator();
        }

        public void Add(T value)
        {
            _dictionary.Add(value, null);
        }

        public void Remove(T value)
        {
            _dictionary.Remove(value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
#endif
}
