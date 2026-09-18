// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

internal static class NotificationExtensions
{
    /// <summary>
    /// Invokes the observer method corresponding to the notification. (AsyncRx.NET has an
    /// internal equivalent; the System.Reactive Notification type has no async accept.)
    /// </summary>
    public static ValueTask AcceptAsync<T>(this Notification<T> notification, IAsyncObserver<T> observer) => notification.Kind switch
    {
        NotificationKind.OnNext => observer.OnNextAsync(notification.Value),
        NotificationKind.OnError => observer.OnErrorAsync(notification.Exception!),
        NotificationKind.OnCompleted => observer.OnCompletedAsync(),
        _ => throw new InvalidOperationException($"Unknown notification kind '{notification.Kind}'."),
    };
}
