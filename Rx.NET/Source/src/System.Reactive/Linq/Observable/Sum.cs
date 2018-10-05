// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class SumDouble : Producer<double, SumDouble._>
    {
        private readonly IObservable<double> _source;

        public SumDouble(IObservable<double> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<double>
        {
            private double _sum;

            public _(IObserver<double> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumSingle : Producer<float, SumSingle._>
    {
        private readonly IObservable<float> _source;

        public SumSingle(IObservable<float> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<float>
        {
            private double _sum; // This is what LINQ to Objects does (accumulates into double that is)!

            public _(IObserver<float> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumDecimal : Producer<decimal, SumDecimal._>
    {
        private readonly IObservable<decimal> _source;

        public SumDecimal(IObservable<decimal> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<decimal>
        {
            private decimal _sum;

            public _(IObserver<decimal> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumInt32 : Producer<int, SumInt32._>
    {
        private readonly IObservable<int> _source;

        public SumInt32(IObservable<int> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<int> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<int>
        {
            private int _sum;

            public _(IObserver<int> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumInt64 : Producer<long, SumInt64._>
    {
        private readonly IObservable<long> _source;

        public SumInt64(IObservable<long> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<long> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<long>
        {
            private long _sum;

            public _(IObserver<long> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumDoubleNullable : Producer<double?, SumDoubleNullable._>
    {
        private readonly IObservable<double?> _source;

        public SumDoubleNullable(IObservable<double?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double?> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<double?>
        {
            private double _sum;

            public _(IObserver<double?> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumSingleNullable : Producer<float?, SumSingleNullable._>
    {
        private readonly IObservable<float?> _source;

        public SumSingleNullable(IObservable<float?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float?> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<float?>
        {
            private double _sum; // This is what LINQ to Objects does (accumulates into double that is)!

            public _(IObserver<float?> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumDecimalNullable : Producer<decimal?, SumDecimalNullable._>
    {
        private readonly IObservable<decimal?> _source;

        public SumDecimalNullable(IObservable<decimal?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal?> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<decimal?>
        {
            private decimal _sum;

            public _(IObserver<decimal?> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumInt32Nullable : Producer<int?, SumInt32Nullable._>
    {
        private readonly IObservable<int?> _source;

        public SumInt32Nullable(IObservable<int?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<int?> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<int?>
        {
            private int _sum;

            public _(IObserver<int?> observer)
                : base(observer)
            {
            }

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
    }

    internal sealed class SumInt64Nullable : Producer<long?, SumInt64Nullable._>
    {
        private readonly IObservable<long?> _source;

        public SumInt64Nullable(IObservable<long?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<long?> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<long?>
        {
            private long _sum;

            public _(IObserver<long?> observer)
                : base(observer)
            {
            }

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
}
