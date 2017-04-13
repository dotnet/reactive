// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Threading;

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Provides access to the host's lifecycle management services.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HostLifecycleService
    {
        private static Lazy<IHostLifecycleNotifications> s_notifications = new Lazy<IHostLifecycleNotifications>(InitializeNotifications);

        private static int _refCount;

        /// <summary>
        /// Event that gets raised when the host suspends the application.
        /// </summary>
        public static event EventHandler<HostSuspendingEventArgs> Suspending;

        /// <summary>
        /// Event that gets raised when the host resumes the application.
        /// </summary>
        public static event EventHandler<HostResumingEventArgs> Resuming;

        /// <summary>
        /// Adds a reference to the host lifecycle manager, causing it to be sending notifications.
        /// </summary>
        public static void AddRef()
        {
            if (Interlocked.Increment(ref _refCount) == 1)
            {
                var notifications = s_notifications.Value;
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
                var notifications = s_notifications.Value;
                if (notifications != null)
                {
                    notifications.Suspending -= OnSuspending;
                    notifications.Resuming -= OnResuming;
                }
            }
        }

        private static void OnSuspending(object sender, HostSuspendingEventArgs e)
        {
            Suspending?.Invoke(sender, e);
        }

        private static void OnResuming(object sender, HostResumingEventArgs e)
        {
            Resuming?.Invoke(sender, e);
        }

        private static IHostLifecycleNotifications InitializeNotifications()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return PlatformEnlightenmentProvider.Current.GetService<IHostLifecycleNotifications>();
#pragma warning restore CS0618 // Type or member is obsolete
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
