// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace System.Reactive
{
    abstract class ConcatSink<TSource> : TailRecursiveSink<TSource>
    {
        public ConcatSink(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
        {
        }

        protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
        {
            var concat = source as IConcatenatable<TSource>;
            if (concat != null)
                return concat.GetSources();

            return null;
        }

        public override void OnCompleted()
        {
            _recurse();
        }
    }
}
