﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics.CodeAnalysis;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class MostRecent<TSource> : PushToPullAdapter<TSource, TSource>
    {
        private readonly TSource _initialValue;

        public MostRecent(IObservable<TSource> source, TSource initialValue)
            : base(source)
        {
            _initialValue = initialValue;
        }

        protected override PushToPullSink<TSource, TSource> Run()
        {
            return new _(_initialValue);
        }

        private sealed class _ : PushToPullSink<TSource, TSource>
        {
            public _(TSource initialValue)
            {
                _kind = NotificationKind.OnNext;
                _value = initialValue;
            }

            private volatile NotificationKind _kind;
            private TSource _value;
            private Exception? _error;

            public override void OnNext(TSource value)
            {
                _value = value;
                _kind = NotificationKind.OnNext;       // Write last!
            }

            public override void OnError(Exception error)
            {
                Dispose();

                _error = error;
                _kind = NotificationKind.OnError;      // Write last!
            }

            public override void OnCompleted()
            {
                Dispose();

                _kind = NotificationKind.OnCompleted;  // Write last!
            }

            public override bool TryMoveNext([MaybeNullWhen(false)]out TSource current)
            {
                //
                // Notice the _kind field is marked volatile and read before the other fields.
                //
                // In case of a concurrent change, we may read a stale OnNext value, which is
                // fine because this push-to-pull adapter is about sampling.
                //
                switch (_kind)
                {
                    case NotificationKind.OnNext:
                        current = _value;
                        return true;
                    case NotificationKind.OnError:
                        _error!.Throw();
                        break;
                    case NotificationKind.OnCompleted:
                        break;
                }

                current = default;
                return false;
            }
        }
    }
}
