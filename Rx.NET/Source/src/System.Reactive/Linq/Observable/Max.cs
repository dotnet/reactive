// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Max<TSource> : Producer<TSource, Max<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IComparer<TSource> _comparer;

        public Max(IObservable<TSource> source, IComparer<TSource> comparer)
        {
            _source = source;
            _comparer = comparer;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => default(TSource) == null ? (_)new Null(_comparer, observer, cancel) : new NonNull(_comparer, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal abstract class _ : Sink<TSource>, IObserver<TSource>
        {
            protected readonly IComparer<TSource> _comparer;

            public _(IComparer<TSource> comparer, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _comparer = comparer;
            }

            public abstract void OnCompleted();
            public abstract void OnError(Exception error);
            public abstract void OnNext(TSource value);
        }

        private sealed class NonNull : _
        {
            private bool _hasValue;
            private TSource _lastValue;

            public NonNull(IComparer<TSource> comparer, IObserver<TSource> observer, IDisposable cancel)
                : base(comparer, observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(TSource);
            }

            public override void OnNext(TSource value)
            {
                if (_hasValue)
                {
                    var comparison = 0;

                    try
                    {
                        comparison = _comparer.Compare(value, _lastValue);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    if (comparison > 0)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _hasValue = true;
                    _lastValue = value;
                }
            }

            public override void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public override void OnCompleted()
            {
                if (!_hasValue)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_lastValue);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }

        private sealed class Null : _
        {
            private TSource _lastValue;

            public Null(IComparer<TSource> comparer, IObserver<TSource> observer, IDisposable cancel)
                : base(comparer, observer, cancel)
            {
                _lastValue = default(TSource);
            }

            public override void OnNext(TSource value)
            {
                if (value != null)
                {
                    if (_lastValue == null)
                    {
                        _lastValue = value;
                    }
                    else
                    {
                        var comparison = 0;

                        try
                        {
                            comparison = _comparer.Compare(value, _lastValue);
                        }
                        catch (Exception ex)
                        {
                            base._observer.OnError(ex);
                            base.Dispose();
                            return;
                        }

                        if (comparison > 0)
                        {
                            _lastValue = value;
                        }
                    }
                }
            }

            public override void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public override void OnCompleted()
            {
                base._observer.OnNext(_lastValue);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class MaxDouble : Producer<double, MaxDouble._>
    {
        private readonly IObservable<double> _source;

        public MaxDouble(IObservable<double> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double>, IObserver<double>
        {
            private bool _hasValue;
            private double _lastValue;

            public _(IObserver<double> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(double);
            }

            public void OnNext(double value)
            {
                if (_hasValue)
                {
                    if (value > _lastValue || double.IsNaN(value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_lastValue);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }
    }

    internal sealed class MaxSingle : Producer<float, MaxSingle._>
    {
        private readonly IObservable<float> _source;

        public MaxSingle(IObservable<float> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<float>, IObserver<float>
        {
            private bool _hasValue;
            private float _lastValue;

            public _(IObserver<float> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(float);
            }

            public void OnNext(float value)
            {
                if (_hasValue)
                {
                    if (value > _lastValue || float.IsNaN(value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_lastValue);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }
    }

    internal sealed class MaxDecimal : Producer<decimal, MaxDecimal._>
    {
        private readonly IObservable<decimal> _source;

        public MaxDecimal(IObservable<decimal> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<decimal>, IObserver<decimal>
        {
            private bool _hasValue;
            private decimal _lastValue;

            public _(IObserver<decimal> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(decimal);
            }

            public void OnNext(decimal value)
            {
                if (_hasValue)
                {
                    if (value > _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_lastValue);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }
    }

    internal sealed class MaxInt32 : Producer<int, MaxInt32._>
    {
        private readonly IObservable<int> _source;

        public MaxInt32(IObservable<int> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<int> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<int>, IObserver<int>
        {
            private bool _hasValue;
            private int _lastValue;

            public _(IObserver<int> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(int);
            }

            public void OnNext(int value)
            {
                if (_hasValue)
                {
                    if (value > _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_lastValue);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }
    }

    internal sealed class MaxInt64 : Producer<long, MaxInt64._>
    {
        private readonly IObservable<long> _source;

        public MaxInt64(IObservable<long> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<long> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<long>, IObserver<long>
        {
            private bool _hasValue;
            private long _lastValue;

            public _(IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _hasValue = false;
                _lastValue = default(long);
            }

            public void OnNext(long value)
            {
                if (_hasValue)
                {
                    if (value > _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                    _hasValue = true;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_lastValue);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }
    }

    internal sealed class MaxDoubleNullable : Producer<double?, MaxDoubleNullable._>
    {
        private readonly IObservable<double?> _source;

        public MaxDoubleNullable(IObservable<double?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double?>, IObserver<double?>
        {
            private double? _lastValue;

            public _(IObserver<double?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(double?);
            }

            public void OnNext(double? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value > _lastValue || double.IsNaN((double)value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_lastValue);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class MaxSingleNullable : Producer<float?, MaxSingleNullable._>
    {
        private readonly IObservable<float?> _source;

        public MaxSingleNullable(IObservable<float?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<float?>, IObserver<float?>
        {
            private float? _lastValue;

            public _(IObserver<float?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(float?);
            }

            public void OnNext(float? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value > _lastValue || float.IsNaN((float)value))
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_lastValue);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class MaxDecimalNullable : Producer<decimal?, MaxDecimalNullable._>
    {
        private readonly IObservable<decimal?> _source;

        public MaxDecimalNullable(IObservable<decimal?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<decimal?>, IObserver<decimal?>
        {
            private decimal? _lastValue;

            public _(IObserver<decimal?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(decimal?);
            }

            public void OnNext(decimal? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value > _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_lastValue);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class MaxInt32Nullable : Producer<int?, MaxInt32Nullable._>
    {
        private readonly IObservable<int?> _source;

        public MaxInt32Nullable(IObservable<int?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<int?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<int?>, IObserver<int?>
        {
            private int? _lastValue;

            public _(IObserver<int?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(int?);
            }

            public void OnNext(int? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value > _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_lastValue);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class MaxInt64Nullable : Producer<long?, MaxInt64Nullable._>
    {
        private readonly IObservable<long?> _source;

        public MaxInt64Nullable(IObservable<long?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<long?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<long?>, IObserver<long?>
        {
            private long? _lastValue;

            public _(IObserver<long?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _lastValue = default(long?);
            }

            public void OnNext(long? value)
            {
                if (!value.HasValue)
                    return;

                if (_lastValue.HasValue)
                {
                    if (value > _lastValue)
                    {
                        _lastValue = value;
                    }
                }
                else
                {
                    _lastValue = value;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_lastValue);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
