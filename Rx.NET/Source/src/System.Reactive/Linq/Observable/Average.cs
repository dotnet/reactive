// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class AverageDouble : Producer<double, AverageDouble._>
    {
        private readonly IObservable<double> _source;

        public AverageDouble(IObservable<double> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double>, IObserver<double>
        {
            private double _sum;
            private long _count;

            public _(IObserver<double> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0;
                _count = 0L;
            }

            public void OnNext(double value)
            {
                try
                {
                    checked
                    {
                        _sum += value;
                        _count++;
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext(_sum / _count);
                    base._observer.OnCompleted();
                }
                else
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }

                base.Dispose();
            }
        }
    }

    internal sealed class AverageSingle : Producer<float, AverageSingle._>
    {
        private readonly IObservable<float> _source;

        public AverageSingle(IObservable<float> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<float>, IObserver<float>
        {
            private double _sum; // NOTE: Uses a different accumulator type (double), conform LINQ to Objects.
            private long _count;

            public _(IObserver<float> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0;
                _count = 0L;
            }

            public void OnNext(float value)
            {
                try
                {
                    checked
                    {
                        _sum += value;
                        _count++;
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext((float)(_sum / _count));
                    base._observer.OnCompleted();
                }
                else
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }

                base.Dispose();
            }
        }
    }

    internal sealed class AverageDecimal : Producer<decimal, AverageDecimal._>
    {
        private readonly IObservable<decimal> _source;

        public AverageDecimal(IObservable<decimal> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<decimal>, IObserver<decimal>
        {
            private decimal _sum;
            private long _count;

            public _(IObserver<decimal> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0M;
                _count = 0L;
            }

            public void OnNext(decimal value)
            {
                try
                {
                    checked
                    {
                        _sum += value;
                        _count++;
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext(_sum / _count);
                    base._observer.OnCompleted();
                }
                else
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }

                base.Dispose();
            }
        }
    }

    internal sealed class AverageInt32 : Producer<double, AverageInt32._>
    {
        private readonly IObservable<int> _source;

        public AverageInt32(IObservable<int> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double>, IObserver<int>
        {
            private long _sum;
            private long _count;

            public _(IObserver<double> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0L;
                _count = 0L;
            }

            public void OnNext(int value)
            {
                try
                {
                    checked
                    {
                        _sum += value;
                        _count++;
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext((double)_sum / _count);
                    base._observer.OnCompleted();
                }
                else
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }

                base.Dispose();
            }
        }
    }

    internal sealed class AverageInt64 : Producer<double, AverageInt64._>
    {
        private readonly IObservable<long> _source;

        public AverageInt64(IObservable<long> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double>, IObserver<long>
        {
            private long _sum;
            private long _count;

            public _(IObserver<double> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0L;
                _count = 0L;
            }

            public void OnNext(long value)
            {
                try
                {
                    checked
                    {
                        _sum += value;
                        _count++;
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext((double)_sum / _count);
                    base._observer.OnCompleted();
                }
                else
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }

                base.Dispose();
            }
        }
    }

    internal sealed class AverageDoubleNullable : Producer<double?, AverageDoubleNullable._>
    {
        private readonly IObservable<double?> _source;

        public AverageDoubleNullable(IObservable<double?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double?>, IObserver<double?>
        {
            private double _sum;
            private long _count;

            public _(IObserver<double?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0;
                _count = 0L;
            }

            public void OnNext(double? value)
            {
                try
                {
                    checked
                    {
                        if (value != null)
                        {
                            _sum += value.Value;
                            _count++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext(_sum / _count);
                }
                else
                {
                    base._observer.OnNext(null);
                }

                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class AverageSingleNullable : Producer<float?, AverageSingleNullable._>
    {
        private readonly IObservable<float?> _source;

        public AverageSingleNullable(IObservable<float?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<float?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<float?>, IObserver<float?>
        {
            private double _sum; // NOTE: Uses a different accumulator type (double), conform LINQ to Objects.
            private long _count;

            public _(IObserver<float?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0.0;
                _count = 0L;
            }

            public void OnNext(float? value)
            {
                try
                {
                    checked
                    {
                        if (value != null)
                        {
                            _sum += value.Value;
                            _count++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext((float)(_sum / _count));
                }
                else
                {
                    base._observer.OnNext(null);
                }

                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class AverageDecimalNullable : Producer<decimal?, AverageDecimalNullable._>
    {
        private readonly IObservable<decimal?> _source;

        public AverageDecimalNullable(IObservable<decimal?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<decimal?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<decimal?>, IObserver<decimal?>
        {
            private decimal _sum;
            private long _count;

            public _(IObserver<decimal?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0M;
                _count = 0L;
            }

            public void OnNext(decimal? value)
            {
                try
                {
                    checked
                    {
                        if (value != null)
                        {
                            _sum += value.Value;
                            _count++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext(_sum / _count);
                }
                else
                {
                    base._observer.OnNext(null);
                }

                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class AverageInt32Nullable : Producer<double?, AverageInt32Nullable._>
    {
        private readonly IObservable<int?> _source;

        public AverageInt32Nullable(IObservable<int?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double?>, IObserver<int?>
        {
            private long _sum;
            private long _count;

            public _(IObserver<double?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0L;
                _count = 0L;
            }

            public void OnNext(int? value)
            {
                try
                {
                    checked
                    {
                        if (value != null)
                        {
                            _sum += value.Value;
                            _count++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext((double)_sum / _count);
                }
                else
                {
                    base._observer.OnNext(null);
                }

                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    internal sealed class AverageInt64Nullable : Producer<double?, AverageInt64Nullable._>
    {
        private readonly IObservable<long?> _source;

        public AverageInt64Nullable(IObservable<long?> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<double?> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<double?>, IObserver<long?>
        {
            private long _sum;
            private long _count;

            public _(IObserver<double?> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _sum = 0L;
                _count = 0L;
            }

            public void OnNext(long? value)
            {
                try
                {
                    checked
                    {
                        if (value != null)
                        {
                            _sum += value.Value;
                            _count++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                if (_count > 0)
                {
                    base._observer.OnNext((double)_sum / _count);
                }
                else
                {
                    base._observer.OnNext(null);
                }

                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
