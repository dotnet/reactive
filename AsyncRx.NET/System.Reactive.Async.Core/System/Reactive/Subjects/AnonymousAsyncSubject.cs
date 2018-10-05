// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    internal sealed class AnonymousAsyncSubject<T> : IAsyncSubject<T>
    {
        private readonly IAsyncObserver<T> _observer;
        private readonly IAsyncObservable<T> _observable;

        public AnonymousAsyncSubject(IAsyncObserver<T> observer, IAsyncObservable<T> observable)
        {
            _observer = observer;
            _observable = observable;
        }

        public Task OnCompletedAsync() => _observer.OnCompletedAsync();

        public Task OnErrorAsync(Exception error) => _observer.OnErrorAsync(error ?? throw new ArgumentNullException(nameof(error)));

        public Task OnNextAsync(T value) => _observer.OnNextAsync(value);

        public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer) => _observable.SubscribeAsync(observer ?? throw new ArgumentNullException(nameof(observer)));
    }
}
