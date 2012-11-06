// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel;
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

        private static int _refCount;

        /// <summary>
        /// Gets the local system clock time.
        /// </summary>
        public static DateTimeOffset UtcNow
        {
            get { return s_serviceSystemClock.Value.UtcNow; }
        }

        /// <summary>
        /// Event that gets raised when a system clock change is detected, if there's any interest as indicated by AddRef calls.
        /// </summary>
        public static event EventHandler<SystemClockChangedEventArgs> SystemClockChanged;

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
            var scc = SystemClockChanged;
            if (scc != null)
                scc(sender, e);
        }

        private static ISystemClock InitializeSystemClock()
        {
            return PlatformEnlightenmentProvider.Current.GetService<ISystemClock>() ?? new DefaultSystemClock();
        }

        private static INotifySystemClockChanged InitializeSystemClockChanged()
        {
            return PlatformEnlightenmentProvider.Current.GetService<INotifySystemClockChanged>() ?? new DefaultSystemClockMonitor();
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
}
