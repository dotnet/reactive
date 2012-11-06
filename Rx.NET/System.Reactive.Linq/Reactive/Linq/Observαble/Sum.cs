// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class SumDouble : Producer<double>
    {
        private readonly IObservable<double> _source;

        public SumDouble(IObservable<double> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<double> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<double>, IObserver<double>
        {
            private double _sum;

            public _(IObserver<double> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0;
            }

            public void OnNext(double value)
            {
                _sum += value;
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumSingle : Producer<float>
    {
        private readonly IObservable<float> _source;

        public SumSingle(IObservable<float> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<float> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<float>, IObserver<float>
        {
            private double _sum; // This is what LINQ to Objects does!

            public _(IObserver<float> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0; // This is what LINQ to Objects does!
            }

            public void OnNext(float value)
            {
                _sum += value; // This is what LINQ to Objects does!
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext((float)_sum); // This is what LINQ to Objects does!
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumDecimal : Producer<decimal>
    {
        private readonly IObservable<decimal> _source;

        public SumDecimal(IObservable<decimal> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<decimal> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<decimal>, IObserver<decimal>
        {
            private decimal _sum;

            public _(IObserver<decimal> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0M;
            }

            public void OnNext(decimal value)
            {
                _sum += value;
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumInt32 : Producer<int>
    {
        private readonly IObservable<int> _source;

        public SumInt32(IObservable<int> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<int> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<int>, IObserver<int>
        {
            private int _sum;

            public _(IObserver<int> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0;
            }

            public void OnNext(int value)
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
                    base._observer.OnError(exception);
                    base.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumInt64 : Producer<long>
    {
        private readonly IObservable<long> _source;

        public SumInt64(IObservable<long> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<long> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<long>, IObserver<long>
        {
            private long _sum;

            public _(IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0L;
            }

            public void OnNext(long value)
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
                    base._observer.OnError(exception);
                    base.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumDoubleNullable : Producer<double?>
    {
        private readonly IObservable<double?> _source;

        public SumDoubleNullable(IObservable<double?> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<double?> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<double?>, IObserver<double?>
        {
            private double _sum;

            public _(IObserver<double?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0;
            }

            public void OnNext(double? value)
            {
                if (value != null)
                    _sum += value.Value;
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumSingleNullable : Producer<float?>
    {
        private readonly IObservable<float?> _source;

        public SumSingleNullable(IObservable<float?> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<float?> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<float?>, IObserver<float?>
        {
            private double _sum; // This is what LINQ to Objects does!

            public _(IObserver<float?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0; // This is what LINQ to Objects does!
            }

            public void OnNext(float? value)
            {
                if (value != null)
                    _sum += value.Value; // This is what LINQ to Objects does!
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext((float)_sum); // This is what LINQ to Objects does!
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumDecimalNullable : Producer<decimal?>
    {
        private readonly IObservable<decimal?> _source;

        public SumDecimalNullable(IObservable<decimal?> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<decimal?> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<decimal?>, IObserver<decimal?>
        {
            private decimal _sum;

            public _(IObserver<decimal?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0M;
            }

            public void OnNext(decimal? value)
            {
                if (value != null)
                    _sum += value.Value;
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumInt32Nullable : Producer<int?>
    {
        private readonly IObservable<int?> _source;

        public SumInt32Nullable(IObservable<int?> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<int?> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<int?>, IObserver<int?>
        {
            private int _sum;

            public _(IObserver<int?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0;
            }

            public void OnNext(int? value)
            {
                try
                {
                    checked
                    {
                        if (value != null)
                            _sum += value.Value;
                    }
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class SumInt64Nullable : Producer<long?>
    {
        private readonly IObservable<long?> _source;

        public SumInt64Nullable(IObservable<long?> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<long?> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<long?>, IObserver<long?>
        {
            private long _sum;

            public _(IObserver<long?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0L;
            }

            public void OnNext(long? value)
            {
                try
                {
                    checked
                    {
                        if (value != null)
                            _sum += value.Value;
                    }
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_sum);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif