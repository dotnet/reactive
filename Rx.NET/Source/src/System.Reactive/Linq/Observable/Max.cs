// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Max<TSource> : Pipe<TSource>
    {
        private readonly IComparer<TSource> _comparer;

        private bool _hasValue;
        private TSource _lastValue;

        public Max(IObservable<TSource> source, IComparer<TSource> comparer) : base(source)
        {
            _comparer = comparer;
        }

        protected override Pipe<TSource, TSource> Clone() => new Max<TSource>(_source, _comparer);

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
                    ForwardOnError(ex);
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

        public override void OnCompleted()
        {
            if (!_hasValue)
            {
                ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
            }
            else
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MaxNullable<TSource> : Pipe<TSource>
    {
        private readonly IComparer<TSource> _comparer;

        private TSource _lastValue;

        public MaxNullable(IObservable<TSource> source, IComparer<TSource> comparer) : base(source)
        {
            _comparer = comparer;
        }

        protected override Pipe<TSource, TSource> Clone() => new Max<TSource>(_source, _comparer);

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
                        ForwardOnError(ex);
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
            ForwardOnError(error);
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_lastValue);
            ForwardOnCompleted();
        }
    }

    internal sealed class MaxDouble : Pipe<double>
    {
        private bool _hasValue;
        private double _lastValue;

        public MaxDouble(IObservable<double> source) : base(source)
        {
        }

        protected override Pipe<double, double> Clone() => new MaxDouble(_source);

        public override void OnNext(double value)
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

        public override void OnCompleted()
        {
            if (!_hasValue)
            {
                ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
            }
            else
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MaxSingle : Pipe<float>
    {
        private bool _hasValue;
        private float _lastValue;

        public MaxSingle(IObservable<float> source) : base(source)
        {
        }

        protected override Pipe<float, float> Clone() => new MaxSingle(_source);

        public override void OnNext(float value)
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

        public override void OnCompleted()
        {
            if (!_hasValue)
            {
                ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
            }
            else
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MaxDecimal : Pipe<decimal>
    {
        private bool _hasValue;
        private decimal _lastValue;

        public MaxDecimal(IObservable<decimal> source) : base(source)
        {
        }

        protected override Pipe<decimal, decimal> Clone() => new MaxDecimal(_source);

        public override void OnNext(decimal value)
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

        public override void OnCompleted()
        {
            if (!_hasValue)
            {
                ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
            }
            else
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MaxInt32 : Pipe<int>
    {
        private bool _hasValue;
        private int _lastValue;

        public MaxInt32(IObservable<int> source) : base(source)
        {
        }

        protected override Pipe<int, int> Clone() => new MaxInt32(_source);

        public override void OnNext(int value)
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

        public override void OnCompleted()
        {
            if (!_hasValue)
            {
                ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
            }
            else
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MaxInt64 : Pipe<long>
    {
        private bool _hasValue;
        private long _lastValue;

        public MaxInt64(IObservable<long> source) : base(source)
        {
        }

        protected override Pipe<long, long> Clone() => new MaxInt64(_source);

        public override void OnNext(long value)
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

        public override void OnCompleted()
        {
            if (!_hasValue)
            {
                ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
            }
            else
            {
                ForwardOnNext(_lastValue);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class MaxDoubleNullable : Pipe<double?>
    {
        private double? _lastValue;

        public MaxDoubleNullable(IObservable<double?> source) : base(source)
        {
        }

        protected override Pipe<double?, double?> Clone() => new MaxDoubleNullable(_source);

        public override void OnNext(double? value)
        {
            if (!value.HasValue)
            {
                return;
            }

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

        public override void OnCompleted()
        {
            ForwardOnNext(_lastValue);
            ForwardOnCompleted();
        }
    }

    internal sealed class MaxSingleNullable : Pipe<float?>
    {
        private float? _lastValue;

        public MaxSingleNullable(IObservable<float?> source) : base(source)
        {
        }

        protected override Pipe<float?, float?> Clone() => new MaxSingleNullable(_source);

        public override void OnNext(float? value)
        {
            if (!value.HasValue)
            {
                return;
            }

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

        public override void OnCompleted()
        {
            ForwardOnNext(_lastValue);
            ForwardOnCompleted();
        }
    }

    internal sealed class MaxDecimalNullable : Pipe<decimal?>
    {
        private decimal? _lastValue;

        public MaxDecimalNullable(IObservable<decimal?> source) : base(source)
        {
        }

        protected override Pipe<decimal?, decimal?> Clone() => new MaxDecimalNullable(_source);

        public override void OnNext(decimal? value)
        {
            if (!value.HasValue)
            {
                return;
            }

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

        public override void OnCompleted()
        {
            ForwardOnNext(_lastValue);
            ForwardOnCompleted();
        }
    }

    internal sealed class MaxInt32Nullable : Pipe<int?>
    {
        private int? _lastValue;

        public MaxInt32Nullable(IObservable<int?> source) : base(source)
        {
        }

        protected override Pipe<int?, int?> Clone() => new MaxInt32Nullable(_source);

        public override void OnNext(int? value)
        {
            if (!value.HasValue)
            {
                return;
            }

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

        public override void OnCompleted()
        {
            ForwardOnNext(_lastValue);
            ForwardOnCompleted();
        }
    }

    internal sealed class MaxInt64Nullable : Pipe<long?>
    {
        private long? _lastValue;

        public MaxInt64Nullable(IObservable<long?> source) : base(source)
        {
        }

        protected override Pipe<long?, long?> Clone() => new MaxInt64Nullable(_source);

        public override void OnNext(long? value)
        {
            if (!value.HasValue)
            {
                return;
            }

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

        public override void OnCompleted()
        {
            ForwardOnNext(_lastValue);
            ForwardOnCompleted();
        }
    }
}

