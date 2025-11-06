// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Provides access to the host's lifecycle management services.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HostLifecycleService
    {
        private static readonly Lazy<IHostLifecycleNotifications?> Notifications = new(InitializeNotifications);

        private static int _refCount;

        /// <summary>
        /// Event that gets raised when the host suspends the application.
        /// </summary>
        public static event EventHandler<HostSuspendingEventArgs>? Suspending;

        /// <summary>
        /// Event that gets raised when the host resumes the application.
        /// </summary>
        public static event EventHandler<HostResumingEventArgs>? Resuming;

        /// <summary>
        /// Adds a reference to the host lifecycle manager, causing it to be sending notifications.
        /// </summary>
        public static void AddRef()
        {
            if (Interlocked.Increment(ref _refCount) == 1)
            {
                var notifications = Notifications.Value;
                if (notifications != null)
                {
                    notifications.Suspending += OnSuspending;
                    notifications.Resuming += OnResuming;
                }
            }
        }

        /// <summary>
        /// Removes a reference to the host lifecycle manager, causing it to stop sending notifications
        /// if the removed reference was the last one.
        /// </summary>
        public static void Release()
        {
            if (Interlocked.Decrement(ref _refCount) == 0)
            {
                var notifications = Notifications.Value;
                if (notifications != null)
                {
                    notifications.Suspending -= OnSuspending;
                    notifications.Resuming -= OnResuming;
                }
            }
        }

        /// <summary>
        /// Sets the <see cref="IHostLifecycleNotifications"/> if one is not already in place.
        /// </summary>
        /// <param name="notifications">The <see cref="IHostLifecycleNotifications"/> to use.</param>
        /// <returns>
        /// True if this implementation will be used because no <see cref="IHostLifecycleNotifications"/>
        /// was already in place. False if an <see cref="IHostLifecycleNotifications"/> was already
        /// available.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This exists to support legacy UWP scenarios. Back when Rx.NET had a uap10.0.18362 target,
        /// it was able to supply a suitable <see cref="IHostLifecycleNotifications"/> implementation to
        /// UWP applications. We've removed all UI-framework-specific support from Rx.NET, and as part
        /// of this we no longer have a uap10.0.18362 target. However, that means legacy UWP apps now get
        /// the <c>netstandard2.0</c> Rx.NET. (New ones built with the UWP support added in .NET 9.0
        /// will get the <c>net8.0-windows10.0.19041</c> target, which supplies lifecycle notifications,
        /// so they don't need this.) The uap targets cause huge problems because most of the .NET SDK
        /// tooling doesn't support them property, so we really didn't want to retain a uap Rx target
        /// just to enable legacy UWP apps to get lifecycle notifications. Instead, we provide this
        /// method so that our libraries supplying legacy UWP support can supply a suitable
        /// <see cref="IHostLifecycleNotifications"/> implementation.
        /// </para>
        /// </remarks>
        public static bool TrySetHostLifecycleNotifications(IHostLifecycleNotifications notifications)
        {
            if (Notifications.Value is DelegatingHostLifecycleNotifications delegatingSource)
            {
                return delegatingSource.TrySetHostLifecycleNotifications(notifications);
            }

            return false;
        }

        private static void OnSuspending(object? sender, HostSuspendingEventArgs e)
        {
            Suspending?.Invoke(sender, e);
        }

        private static void OnResuming(object? sender, HostResumingEventArgs e)
        {
            Resuming?.Invoke(sender, e);
        }
        private static IHostLifecycleNotifications? InitializeNotifications()
        {
            return PlatformEnlightenmentProvider.Current.GetService<IHostLifecycleNotifications>()
                ?? new DelegatingHostLifecycleNotifications();
        }

        // Because we allow libraries to provide their own implementation of IHostLifecycleNotifications,
        // it's possible that something will register for notifications and call AddRef before the
        // IHostLifecycleNotifications becomes available. The parent type's static events will hang
        // onto the handlers, but we need to ensure that if an IHostLifecycleNotifications is supplied
        // once we're in this state, we get events flowing.
        private class DelegatingHostLifecycleNotifications : IHostLifecycleNotifications
        {
            private readonly object _lock = new();
            private EventHandler<HostSuspendingEventArgs>? _suspending;
            private EventHandler<HostResumingEventArgs>? _resuming;
            private IHostLifecycleNotifications? _source;

            public event EventHandler<HostSuspendingEventArgs> Suspending
            {
                add
                {
                    lock(_lock)
                    {
                        Debug.Assert(_suspending is null, "HostLifecycleService should only attach one handler to its IHostLifecycleNotifications");
                        _suspending = value;
                        if (_source is not null)
                        {
                            _source.Suspending += value;
                        }
                    }
                }
                remove
                {
                    lock (_lock)
                    {
                        Debug.Assert(_suspending is not null, "HostLifecycleService removed handler multiple times");
                        if (_source is not null)
                        {
                            _source.Suspending -= value;
                        }
                        _suspending = null; 
                    }
                }
            }
            public event EventHandler<HostResumingEventArgs> Resuming
            {
                add
                {
                    lock (_lock)
                    {
                        Debug.Assert(_resuming is null, "HostLifecycleService should only attach one handler to its IHostLifecycleNotifications");
                        if (_source is not null)
                        {
                            _source.Resuming += value;
                        }
                        _resuming = value;
                    }
                }
                remove
                {
                    lock (_lock)
                    {
                        Debug.Assert(_resuming is not null, "HostLifecycleService removed handler multiple times");
                        if (_source is not null)
                        {
                            _source.Resuming -= value;
                        }
                        _resuming = null;
                    }
                }
            }

            internal bool TrySetHostLifecycleNotifications(IHostLifecycleNotifications notifications)
            {
                lock (_lock)
                {
                    if (_source is not null)
                    {
                        return false; // Already set, cannot change.
                    }

                    _source = notifications;

                    // If handlers had already been supplied, we now need to attach them.
                    if (_suspending is not null)
                    {
                        _source.Suspending += _suspending;
                    }
                    if (_resuming is not null)
                    {
                        _source.Resuming += _resuming;
                    }
                    return true;
                }
            }
        }
    }

    /// <summary>
    /// (Infrastructure) Provides notifications about the host's lifecycle events.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHostLifecycleNotifications
    {
        /// <summary>
        /// Event that gets raised when the host suspends.
        /// </summary>
        event EventHandler<HostSuspendingEventArgs> Suspending;

        /// <summary>
        /// Event that gets raised when the host resumes.
        /// </summary>
        event EventHandler<HostResumingEventArgs> Resuming;
    }

    /// <summary>
    /// (Infrastructure) Event arguments for host suspension events.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class HostSuspendingEventArgs : EventArgs
    {
    }

    /// <summary>
    /// (Infrastructure) Event arguments for host resumption events.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class HostResumingEventArgs : EventArgs
    {
    }
}
