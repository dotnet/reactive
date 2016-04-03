// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for subscribing delegates to observables.
    /// </summary>
    public static class WeakObservableExtensions
    {
        #region ToWeakObservable

        /// <summary>
        /// To weak observable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IObservable<T> ToWeakObservable<T>(this IObservable<T> source)
        {
            var result = new WeakObservable<T>(source);
            return result;
        }

        #endregion ToWeakObservable
    }
}
