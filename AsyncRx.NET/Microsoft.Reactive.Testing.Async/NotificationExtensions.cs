// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

internal static class NotificationExtensions
{
    /// <summary>
    /// Invokes the observer method corresponding to the notification.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This provides the asynchronous version of <see cref="Notification{T}.Accept(IObserver{T})"/>.
    /// Since <see cref="Notification{T}"/> is part of System.Reactive, it doesn't know anything
    /// about asynchronous observers, so we add this capability as an extension method.
    /// </para>
    /// <para>
    /// TODO: AsyncRx.NET has an internal equivalent. Should we just make that public?
    /// </para>
    /// </remarks>
    public static ValueTask AcceptAsync<T>(this Notification<T> notification, IAsyncObserver<T> observer) => notification.Kind switch
    {
        NotificationKind.OnNext => observer.OnNextAsync(notification.Value),
        NotificationKind.OnError => observer.OnErrorAsync(notification.Exception!),
        NotificationKind.OnCompleted => observer.OnCompletedAsync(),
        _ => throw new InvalidOperationException($"Unknown notification kind '{notification.Kind}'."),
    };
}
