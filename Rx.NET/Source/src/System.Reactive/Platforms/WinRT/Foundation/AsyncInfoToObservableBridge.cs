﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if HAS_WINRT
using System.Reactive.Subjects;
using Windows.Foundation;

namespace System.Reactive.Windows.Foundation
{
    internal class AsyncInfoToObservableBridge<TResult, TProgress> : ObservableBase<TResult>
    {
        private readonly Action<IAsyncInfo, Action<IAsyncInfo, AsyncStatus>> _onCompleted;
        private readonly Func<IAsyncInfo, TResult> _getResult;
        private readonly AsyncSubject<TResult> _subject;

        public AsyncInfoToObservableBridge(IAsyncInfo info, Action<IAsyncInfo, Action<IAsyncInfo, AsyncStatus>> onCompleted, Func<IAsyncInfo, TResult> getResult, Action<IAsyncInfo, Action<IAsyncInfo, TProgress>>? onProgress, IProgress<TProgress>? progress, bool multiValue)
        {
            _onCompleted = onCompleted;
            _getResult = getResult;

            _subject = new AsyncSubject<TResult>();

            onProgress?.Invoke(info, (iai, p) =>
            {
                if (multiValue && getResult != null)
                {
                    _subject.OnNext(getResult(iai));
                }

                progress?.Report(p);
            });

            Done(info, info.Status, true);
        }

        private void Done(IAsyncInfo info, AsyncStatus status, bool initial)
        {
            var error = default(Exception);
            var result = default(TResult);

            //
            // Initial interactions with the IAsyncInfo object. Those could fail, which indicates
            // a rogue implementation. Failure is just propagated out.
            //
            switch (status)
            {
                case AsyncStatus.Error:
                    error = info.ErrorCode;
                    if (error == null)
                    {
                        throw new InvalidOperationException("The asynchronous operation failed with a null error code.");
                    }

                    break;
                case AsyncStatus.Canceled:
                    error = new OperationCanceledException();
                    break;
                case AsyncStatus.Completed:
                    if (_getResult != null)
                    {
                        result = _getResult(info);
                    }

                    break;
                default:
                    if (!initial)
                    {
                        throw new InvalidOperationException("The asynchronous operation completed unexpectedly.");
                    }

                    _onCompleted(info, (iai, s) => Done(iai, s, false));
                    return;
            }

            //
            // Close as early as possible, before running continuations which could fail. In case of
            // failure above, we don't close out the object in order to allow for debugging of the
            // rogue implementation without losing state prematurely. Notice _getResult is merely
            // an indirect call to the appropriate GetResults method, which is not supposed to throw.
            // Instead, an Error status should be returned.
            //
            info.Close();

            //
            // Now we run the continuations, which could take a long time. Failure here is catastrophic
            // and under control of the upstream subscriber.
            //
            if (error != null)
            {
                _subject.OnError(error);
            }
            else
            {
                if (_getResult != null)
                {
                    _subject.OnNext(result!); // NB: Has been assigned in switch statement above.
                }

                _subject.OnCompleted();
            }
        }

        protected override IDisposable SubscribeCore(IObserver<TResult> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
#endif
