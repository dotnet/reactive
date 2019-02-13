// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Aggregate<TSource> : Pipe<TSource>
    {
        private readonly Func<TSource, TSource, TSource> _accumulator;

        private TSource _accumulation;
        private bool _hasAccumulation;

        public Aggregate(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator) : base(source)
        {
            _accumulator = accumulator;
        }

        protected override Pipe<TSource, TSource> Clone() => new Aggregate<TSource>(_source, _accumulator);

        public override void OnNext(TSource value)
        {
            if (!_hasAccumulation)
            {
                _accumulation = value;
                _hasAccumulation = true;
            }
            else
            {
                try
                {
                    _accumulation = _accumulator(_accumulation, value);
                }
                catch (Exception exception)
                {
                    _accumulation = default;
                    ForwardOnError(exception);
                }
            }
        }

        public override void OnError(Exception error)
        {
            _accumulation = default;
            ForwardOnError(error);
        }

        public override void OnCompleted()
        {
            if (!_hasAccumulation)
            {
                ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
            }
            else
            {
                var accumulation = _accumulation;
                _accumulation = default;
                ForwardOnNext(accumulation);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class Aggregate<TSource, TAccumulate> : Pipe<TSource, TAccumulate>
    {
        private readonly TAccumulate _seed;
        private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;

        private TAccumulate _accumulation;

        public Aggregate(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
            : base(source)
        {
            _seed = seed;
            _accumulator = accumulator;
            _accumulation = seed;
        }

        protected override Pipe<TSource, TAccumulate> Clone() => new Aggregate<TSource, TAccumulate>(_source, _seed, _accumulator);

        public override void OnNext(TSource value)
        {
            try
            {
                _accumulation = _accumulator(_accumulation, value);
            }
            catch (Exception exception)
            {
                _accumulation = default;
                ForwardOnError(exception);
            }
        }

        public override void OnError(Exception error)
        {
            _accumulation = default;
            ForwardOnError(error);
        }

        public override void OnCompleted()
        {
            var accumulation = _accumulation;
            _accumulation = default;
            ForwardOnNext(accumulation);
            ForwardOnCompleted();
        }
    }

    internal sealed class Aggregate<TSource, TAccumulate, TResult> : Pipe<TSource, TResult>
    {
        private readonly TAccumulate _seed;
        private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
        private readonly Func<TAccumulate, TResult> _resultSelector;

        private TAccumulate _accumulation;

        public Aggregate(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
            : base(source)
        {
            _seed = seed;
            _accumulator = accumulator;
            _resultSelector = resultSelector;
            _accumulation = seed;
        }

        protected override Pipe<TSource, TResult> Clone() => new Aggregate<TSource, TAccumulate, TResult>(_source, _seed, _accumulator, _resultSelector);

        public override void OnNext(TSource value)
        {
            try
            {
                _accumulation = _accumulator(_accumulation, value);
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);
            }
        }

        public override void OnError(Exception error)
        {
            ForwardOnError(error);
        }

        public override void OnCompleted()
        {
            var result = default(TResult);
            try
            {
                result = _resultSelector(_accumulation);
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);
                return;
            }

            ForwardOnNext(result);
            ForwardOnCompleted();
        }
    }
}
