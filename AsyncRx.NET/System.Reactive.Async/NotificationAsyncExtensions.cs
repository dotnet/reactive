// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive
{
    internal static class NotificationAsyncExtensions
    {
        /// <summary>
        /// Invokes the observer's method corresponding to the notification.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="notification">The notification.</param>
        /// <param name="observer">Observer to invoke the notification on.</param>
        /// <returns>A task that completes when the observer is done.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>
        /// The implementation of <see cref="Notification{T}"/> we get from System.Reactive is
        /// missing the built-in support for this method that the version in System.Reactive.Shared
        /// used to have (but is otherwise identical). This reproduces that as an extension method.
        /// </remarks>
        public static async ValueTask AcceptAsync<T>(this Notification<T> notification, IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var adapter = new NotificationAdapter<T>(observer);
            notification.Accept(adapter);
            await adapter.Wait().ConfigureAwait(false);
        }

        private class NotificationAdapter<T> : IObserver<T>
        {
            private readonly IAsyncObserver<T> _asyncObserver;
            private ValueTask? _valueTask;

            public NotificationAdapter(IAsyncObserver<T> asyncObserver)
            {
                _asyncObserver = asyncObserver;
            }

            public async ValueTask Wait()
            {
                if (!_valueTask.HasValue)
                {
                    throw new InvalidOperationException("Accept did not call any IObserver<T> method");
                }

                await _valueTask.Value.ConfigureAwait(false);
            }

            public void OnCompleted()
            {
                if (_valueTask.HasValue)
                {
                    throw new InvalidOperationException("Accept should have called only one IObserver<T> method");
                }

                _valueTask = _asyncObserver.OnCompletedAsync();
            }

            public void OnError(Exception error)
            {
                if (_valueTask.HasValue)
                {
                    throw new InvalidOperationException("Accept should have called only one IObserver<T> method");
                }

                _valueTask = _asyncObserver.OnErrorAsync(error);
            }

            public void OnNext(T value)
            {
                if (_valueTask.HasValue)
                {
                    throw new InvalidOperationException("Accept should have called only one IObserver<T> method");
                }

                _valueTask = _asyncObserver.OnNextAsync(value);
            }
        }
    }
}
