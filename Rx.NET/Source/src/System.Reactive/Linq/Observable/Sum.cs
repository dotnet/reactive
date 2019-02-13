// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class SumDouble : Pipe<double>
    {
        private double _sum;

        public SumDouble(IObservable<double> source) : base(source)
        {
        }

        protected override Pipe<double, double> Clone() => new SumDouble(_source);

        public override void OnNext(double value)
        {
            _sum += value;
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
    }

    internal sealed class SumSingle : Pipe<float>
    {
        private double _sum; // This is what LINQ to Objects does (accumulates into double that is)!

        public SumSingle(IObservable<float> source) : base(source)
        {
        }

        protected override Pipe<float, float> Clone() => new SumSingle(_source);

        public override void OnNext(float value)
        {
            _sum += value; // This is what LINQ to Objects does!
        }

        public override void OnCompleted()
        {
            ForwardOnNext((float)_sum); // This is what LINQ to Objects does!
            ForwardOnCompleted();
        }
    }

    internal sealed class SumDecimal : Pipe<decimal>
    {
        private decimal _sum;

        public SumDecimal(IObservable<decimal> source) : base(source)
        {
        }

        protected override Pipe<decimal, decimal> Clone() => new SumDecimal(_source);

        public override void OnNext(decimal value)
        {
            _sum += value;
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
    }

    internal sealed class SumInt32 : Pipe<int>
    {
        private int _sum;

        public SumInt32(IObservable<int> source) : base(source)
        {
        }

        protected override Pipe<int, int> Clone() => new SumInt32(_source);

        public override void OnNext(int value)
        {
            try
            {
                checked
                {
                    _sum += value;
                }
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
    }

    internal sealed class SumInt64 : Pipe<long>
    {
        private long _sum;

        public SumInt64(IObservable<long> source) : base(source)
        {
        }

        protected override Pipe<long, long> Clone() => new SumInt64(_source);

        public override void OnNext(long value)
        {
            try
            {
                checked
                {
                    _sum += value;
                }
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
    }

    internal sealed class SumDoubleNullable : Pipe<double?>
    {
        private double _sum;

        public SumDoubleNullable(IObservable<double?> source) : base(source)
        {
        }

        protected override Pipe<double?, double?> Clone() => new SumDoubleNullable(_source);

        public override void OnNext(double? value)
        {
            if (value != null)
            {
                _sum += value.Value;
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
        
    }

    internal sealed class SumSingleNullable : Pipe<float?>
    {
        private double _sum; // This is what LINQ to Objects does (accumulates into double that is)!

        public SumSingleNullable(IObservable<float?> source) : base(source)
        {
        }

        protected override Pipe<float?, float?> Clone() => new SumSingleNullable(_source);

        public override void OnNext(float? value)
        {
            if (value != null)
            {
                _sum += value.Value; // This is what LINQ to Objects does!
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext((float)_sum); // This is what LINQ to Objects does!
            ForwardOnCompleted();
        }
    }

    internal sealed class SumDecimalNullable : Pipe<decimal?>
    {
        private decimal _sum;

        public SumDecimalNullable(IObservable<decimal?> source) : base(source)
        {
        }

        protected override Pipe<decimal?, decimal?> Clone() => new SumDecimalNullable(_source);

        public override void OnNext(decimal? value)
        {
            if (value != null)
            {
                _sum += value.Value;
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
    }

    internal sealed class SumInt32Nullable : Pipe<int?>
    {
        private int _sum;

        public SumInt32Nullable(IObservable<int?> source) : base(source)
        {
        }

        protected override Pipe<int?, int?> Clone() => new SumInt32Nullable(_source);

        public override void OnNext(int? value)
        {
            try
            {
                checked
                {
                    if (value != null)
                    {
                        _sum += value.Value;
                    }
                }
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
    }

    internal sealed class SumInt64Nullable : Pipe<long?>
    {
        private long _sum;

        public SumInt64Nullable(IObservable<long?> source) : base(source)
        {
        }

        protected override Pipe<long?, long?> Clone() => new SumInt64Nullable(_source);
        
        public override void OnNext(long? value)
        {
            try
            {
                checked
                {
                    if (value != null)
                    {
                        _sum += value.Value;
                    }
                }
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);
            }
        }

        public override void OnCompleted()
        {
            ForwardOnNext(_sum);
            ForwardOnCompleted();
        }
    }
}
