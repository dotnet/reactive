// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class AverageDouble : Producer<double>
    {
        private readonly IObservable<double> _source;

        public AverageDouble(IObservable<double> source)
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

    class AverageSingle : Producer<float>
    {
        private readonly IObservable<float> _source;

        public AverageSingle(IObservable<float> source)
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

    class AverageDecimal : Producer<decimal>
    {
        private readonly IObservable<decimal> _source;

        public AverageDecimal(IObservable<decimal> source)
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

    class AverageInt32 : Producer<double>
    {
        private readonly IObservable<int> _source;

        public AverageInt32(IObservable<int> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<double> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<double>, IObserver<int>
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

    class AverageInt64 : Producer<double>
    {
        private readonly IObservable<long> _source;

        public AverageInt64(IObservable<long> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<double> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<double>, IObserver<long>
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

    class AverageDoubleNullable : Producer<double?>
    {
        private readonly IObservable<double?> _source;

        public AverageDoubleNullable(IObservable<double?> source)
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

    class AverageSingleNullable : Producer<float?>
    {
        private readonly IObservable<float?> _source;

        public AverageSingleNullable(IObservable<float?> source)
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

    class AverageDecimalNullable : Producer<decimal?>
    {
        private readonly IObservable<decimal?> _source;

        public AverageDecimalNullable(IObservable<decimal?> source)
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

    class AverageInt32Nullable : Producer<double?>
    {
        private readonly IObservable<int?> _source;

        public AverageInt32Nullable(IObservable<int?> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<double?> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<double?>, IObserver<int?>
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

    class AverageInt64Nullable : Producer<double?>
    {
        private readonly IObservable<long?> _source;

        public AverageInt64Nullable(IObservable<long?> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<double?> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<double?>, IObserver<long?>
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
#endif