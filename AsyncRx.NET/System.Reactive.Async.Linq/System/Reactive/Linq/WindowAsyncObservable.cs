// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    internal sealed class WindowAsyncObservable<TSource> : AsyncObservableBase<TSource>
    {
        private readonly IAsyncSubject<TSource> _subject;
        private readonly RefCountAsyncDisposable _disposable;

        public WindowAsyncObservable(IAsyncSubject<TSource> subject, RefCountAsyncDisposable disposable)
        {
            _subject = subject;
            _disposable = disposable;
        }

        protected override async Task<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            if (_disposable != null)
            {
                var d = await _disposable.GetDisposableAsync().ConfigureAwait(false);
                var s = await _subject.SubscribeAsync(observer).ConfigureAwait(false);
                return StableCompositeAsyncDisposable.Create(d, s);
            }

            return await _subject.SubscribeAsync(observer).ConfigureAwait(false);
        }
    }
}
