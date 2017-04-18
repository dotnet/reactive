// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Collect<TSource, TResult> : PushToPullAdapter<TSource, TResult>
    {
        private readonly Func<TResult> _getInitialCollector;
        private readonly Func<TResult, TSource, TResult> _merge;
        private readonly Func<TResult, TResult> _getNewCollector;

        public Collect(IObservable<TSource> source, Func<TResult> getInitialCollector, Func<TResult, TSource, TResult> merge, Func<TResult, TResult> getNewCollector)
            : base(source)
        {
            _getInitialCollector = getInitialCollector;
            _merge = merge;
            _getNewCollector = getNewCollector;
        }

        protected override PushToPullSink<TSource, TResult> Run(IDisposable subscription)
        {
            var sink = new _(this, subscription);
            sink.Initialize();
            return sink;
        }

        private sealed class _ : PushToPullSink<TSource, TResult>
        {
            // CONSIDER: This sink has a parent reference that can be considered for removal.

            private readonly Collect<TSource, TResult> _parent;

            public _(Collect<TSource, TResult> parent, IDisposable subscription)
                : base(subscription)
            {
                _parent = parent;
            }

            private object _gate;
            private TResult _collector;
            private bool _hasFailed;
            private Exception _error;
            private bool _hasCompleted;
            private bool _done;

            public void Initialize()
            {
                _gate = new object();
                _collector = _parent._getInitialCollector();
            }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    try
                    {
                        _collector = _parent._merge(_collector, value);
                    }
                    catch (Exception ex)
                    {
                        _error = ex;
                        _hasFailed = true;

                        base.Dispose();
                    }
                }
            }

            public override void OnError(Exception error)
            {
                base.Dispose();

                lock (_gate)
                {
                    _error = error;
                    _hasFailed = true;
                }
            }

            public override void OnCompleted()
            {
                base.Dispose();

                lock (_gate)
                {
                    _hasCompleted = true;
                }
            }

            public override bool TryMoveNext(out TResult current)
            {
                lock (_gate)
                {
                    if (_hasFailed)
                    {
                        current = default(TResult);
                        _error.Throw();
                    }
                    else
                    {
                        if (_hasCompleted)
                        {
                            if (_done)
                            {
                                current = default(TResult);
                                return false;
                            }

                            current = _collector;
                            _done = true;
                        }
                        else
                        {
                            current = _collector;

                            try
                            {
                                _collector = _parent._getNewCollector(current);
                            }
                            catch
                            {
                                base.Dispose();
                                throw;
                            }
                        }
                    }

                    return true;
                }
            }
        }
    }
}
