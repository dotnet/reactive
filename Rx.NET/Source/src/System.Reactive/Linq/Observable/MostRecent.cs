// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

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
            if (IntPtr.Size < Unsafe.SizeOf<TSource>())
            {
                return new NonAtomic(_initialValue);
            }
            else
            {
                return new Atomic(_initialValue);
            }
        }

        private abstract class Shared : PushToPullSink<TSource, TSource>
        {
            protected Shared(in TSource initialValue)
            {
                _kind = NotificationKind.OnNext;
                _value = initialValue;
            }

            private volatile NotificationKind _kind;
            private TSource _value;
            private Exception _error;

            protected virtual void GetValue(out TSource value)
            {
                value = _value;
            }

            protected void SetValue(in TSource value)
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

            public override bool TryMoveNext(out TSource current)
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
                        GetValue(out current);
                        return true;
                    case NotificationKind.OnError:
                        _error.Throw();
                        break;
                    case NotificationKind.OnCompleted:
                        break;
                }

                current = default;
                return false;
            }
        }

        private sealed class Atomic : Shared
        {
            public Atomic(TSource initialValue)
                : base(initialValue)
            { }

            public override void OnNext(TSource value)
            {
                SetValue(value);
            }
        }

        private sealed class NonAtomic : Shared
        {
            private readonly object _gate = new object();

            public NonAtomic(in TSource initialValue)
                : base(initialValue)
            { }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    SetValue(value);
                }
            }

            protected override void GetValue(out TSource value)
            {
                lock (_gate)
                {
                    base.GetValue(out value);
                }
            }
        }
    }
}
