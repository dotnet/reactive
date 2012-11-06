// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Concurrency;
using System.Threading;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region FromAsyncPattern

        #region Func

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<IObservable<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, IObservable<TResult>> FromAsyncPattern<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TResult>(begin, end);
        }

#if !NO_LARGEARITY
        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TResult>(Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the end delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(begin, end);
        }
#endif

        #endregion

        #region Action

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<IObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, IObservable<Unit>> FromAsyncPattern<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2>(begin, end);
        }

#if !NO_LARGEARITY
        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4>(Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(begin, end);
        }

        /// <summary>
        /// Converts a Begin/End invoke function pair into an asynchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the begin delegate.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the begin delegate.</typeparam>
        /// <param name="begin">The delegate that begins the asynchronous operation.</param>
        /// <param name="end">The delegate that ends the asynchronous operation.</param>
        /// <returns>Function that can be used to start the asynchronous operation and retrieve the result (represented as a Unit value) as an observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="begin"/> or <paramref name="end"/> is null.</exception>
        /// <remarks>Each invocation of the resulting function will cause the asynchronous operation to be started. Subscription to the resulting sequence has no observable side-effect, and each subscription will produce the asynchronous operation's result.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]
#endif
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            if (begin == null)
                throw new ArgumentNullException("begin");
            if (end == null)
                throw new ArgumentNullException("end");

            return s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(begin, end);
        }
#endif

        #endregion

        #endregion

        #region Start[Async]

        #region Func

        /// <summary>
        /// Invokes the specified function asynchronously, surfacing the result through an observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to run asynchronously.</param>
        /// <returns>An observable sequence exposing the function's result value, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The function is called immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the function's result.</description></item>
        /// </list>
        /// </remarks>
        public static IObservable<TResult> Start<TResult>(Func<TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.Start<TResult>(function);
        }

        /// <summary>
        /// Invokes the specified function asynchronously on the specified scheduler, surfacing the result through an observable sequence
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to run asynchronously.</param>
        /// <param name="scheduler">Scheduler to run the function on.</param>
        /// <returns>An observable sequence exposing the function's result value, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The function is called immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the function's result.</description></item>
        /// </list>
        /// </remarks>
        public static IObservable<TResult> Start<TResult>(Func<TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.Start<TResult>(function, scheduler);
        }

#if !NO_TPL
        /// <summary>
        /// Invokes the asynchronous function, surfacing the result through an observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the asynchronous function.</typeparam>
        /// <param name="functionAsync">Asynchronous function to run.</param>
        /// <returns>An observable sequence exposing the function's result value, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="functionAsync"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The function is started immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the function's result.</description></item>
        /// </list>
        /// </remarks>
        public static IObservable<TResult> StartAsync<TResult>(Func<Task<TResult>> functionAsync)
        {
            if (functionAsync == null)
                throw new ArgumentNullException("functionAsync");

            return s_impl.StartAsync<TResult>(functionAsync);
        }

        /// <summary>
        /// Invokes the asynchronous function, surfacing the result through an observable sequence.
        /// The CancellationToken is shared by all subscriptions on the resulting observable sequence. See the remarks section for more information.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the asynchronous function.</typeparam>
        /// <param name="functionAsync">Asynchronous function to run.</param>
        /// <returns>An observable sequence exposing the function's result value, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="functionAsync"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The function is started immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the function's result.</description></item>
        /// <item><description>
        /// If any subscription to the resulting sequence is disposed, the CancellationToken is set. The observer associated to the disposed
        /// subscription won't see the TaskCanceledException, but other observers will. You can protect against this using the Catch operator.
        /// Be careful when handing out the resulting sequence because of this behavior. The most common use is to have a single subscription
        /// to the resulting sequence, which controls the CancellationToken state. Alternatively, you can control subscription behavior using
        /// multicast operators.
        /// </description></item>
        /// </list>
        /// </remarks>
        public static IObservable<TResult> StartAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync)
        {
            if (functionAsync == null)
                throw new ArgumentNullException("functionAsync");

            return s_impl.StartAsync<TResult>(functionAsync);
        }
#endif

        #endregion

        #region Action

        /// <summary>
        /// Invokes the action asynchronously, surfacing the result through an observable sequence.
        /// </summary>
        /// <param name="action">Action to run asynchronously.</param>
        /// <returns>An observable sequence exposing a Unit value upon completion of the action, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The action is called immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the action's outcome.</description></item>
        /// </list>
        /// </remarks>
        public static IObservable<Unit> Start(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.Start(action);
        }

        /// <summary>
        /// Invokes the action asynchronously on the specified scheduler, surfacing the result through an observable sequence.
        /// </summary>
        /// <param name="action">Action to run asynchronously.</param>
        /// <param name="scheduler">Scheduler to run the action on.</param>
        /// <returns>An observable sequence exposing a Unit value upon completion of the action, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The action is called immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the action's outcome.</description></item>
        /// </list>
        /// </remarks>
        public static IObservable<Unit> Start(Action action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.Start(action, scheduler);
        }

#if !NO_TPL
        /// <summary>
        /// Invokes the asynchronous action, surfacing the result through an observable sequence.
        /// </summary>
        /// <param name="actionAsync">Asynchronous action to run.</param>
        /// <returns>An observable sequence exposing a Unit value upon completion of the action, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="actionAsync"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The action is started immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the action's outcome.</description></item>
        /// </list>
        /// </remarks>
        public static IObservable<Unit> StartAsync(Func<Task> actionAsync)
        {
            if (actionAsync == null)
                throw new ArgumentNullException("actionAsync");

            return s_impl.StartAsync(actionAsync);
        }

        /// <summary>
        /// Invokes the asynchronous action, surfacing the result through an observable sequence.
        /// The CancellationToken is shared by all subscriptions on the resulting observable sequence. See the remarks section for more information.
        /// </summary>
        /// <param name="actionAsync">Asynchronous action to run.</param>
        /// <returns>An observable sequence exposing a Unit value upon completion of the action, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="actionAsync"/> is null.</exception>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The action is started immediately, not during the subscription of the resulting sequence.</description></item>
        /// <item><description>Multiple subscriptions to the resulting sequence can observe the action's outcome.</description></item>
        /// <item><description>
        /// If any subscription to the resulting sequence is disposed, the CancellationToken is set. The observer associated to the disposed
        /// subscription won't see the TaskCanceledException, but other observers will. You can protect against this using the Catch operator.
        /// Be careful when handing out the resulting sequence because of this behavior. The most common use is to have a single subscription
        /// to the resulting sequence, which controls the CancellationToken state. Alternatively, you can control subscription behavior using
        /// multicast operators.
        /// </description></item>
        /// </list>
        /// </remarks>
        public static IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync)
        {
            if (actionAsync == null)
                throw new ArgumentNullException("actionAsync");

            return s_impl.StartAsync(actionAsync);
        }
#endif

        #endregion

        #endregion

        #region FromAsync

#if !NO_TPL

        #region Func

        /// <summary>
        /// Converts to asynchronous function into an observable sequence. Each subscription to the resulting sequence causes the function to be started.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the asynchronous function.</typeparam>
        /// <param name="functionAsync">Asynchronous function to convert.</param>
        /// <returns>An observable sequence exposing the result of invoking the function, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="functionAsync"/> is null.</exception>
        public static IObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync)
        {
            return s_impl.FromAsync<TResult>(functionAsync);
        }

        /// <summary>
        /// Converts to asynchronous function into an observable sequence. Each subscription to the resulting sequence causes the function to be started.
        /// The CancellationToken passed to the asynchronous function is tied to the observable sequence's subscription that triggered the function's invocation and can be used for best-effort cancellation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the asynchronous function.</typeparam>
        /// <param name="functionAsync">Asynchronous function to convert.</param>
        /// <returns>An observable sequence exposing the result of invoking the function, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="functionAsync"/> is null.</exception>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous function will be signaled.</remarks>
        public static IObservable<TResult> FromAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync)
        {
            return s_impl.FromAsync<TResult>(functionAsync);
        }

        #endregion

        #region Action

        /// <summary>
        /// Converts to asynchronous action into an observable sequence. Each subscription to the resulting sequence causes the action to be started.
        /// </summary>
        /// <param name="actionAsync">Asynchronous action to convert.</param>
        /// <returns>An observable sequence exposing a Unit value upon completion of the action, or an exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="actionAsync"/> is null.</exception>
        public static IObservable<Unit> FromAsync(Func<Task> actionAsync)
        {
            return s_impl.FromAsync(actionAsync);
        }

        /// <summary>
        /// Converts to asynchronous action into an observable sequence. Each subscription to the resulting sequence causes the action to be started.
        /// The CancellationToken passed to the asynchronous action is tied to the observable sequence's subscription that triggered the action's invocation and can be used for best-effort cancellation.
        /// </summary>
        /// <param name="actionAsync">Asynchronous action to convert.</param>
        /// <returns>An observable sequence exposing a Unit value upon completion of the action, or an exception.</returns>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous function will be signaled.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="actionAsync"/> is null.</exception>
        public static IObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync)
        {
            return s_impl.FromAsync(actionAsync);
        }

        #endregion

#endif

        #endregion

        #region ToAsync

        #region Func

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<IObservable<TResult>> ToAsync<TResult>(this Func<TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<IObservable<TResult>> ToAsync<TResult>(this Func<TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, IObservable<TResult>> ToAsync<TArg1, TResult>(this Func<TArg1, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, IObservable<TResult>> ToAsync<TArg1, TResult>(this Func<TArg1, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, IObservable<TResult>> ToAsync<TArg1, TArg2, TResult>(this Func<TArg1, TArg2, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, IObservable<TResult>> ToAsync<TArg1, TArg2, TResult>(this Func<TArg1, TArg2, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(this Func<TArg1, TArg2, TArg3, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(this Func<TArg1, TArg2, TArg3, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(function, scheduler);
        }

#if !NO_LARGEARITY
        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(function, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg16">The type of the sixteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(function);
        }

        /// <summary>
        /// Converts the function into an asynchronous function. Each invocation of the resulting asynchronous function causes an invocation of the original synchronous function on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the function.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the function.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the function.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the function.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the function.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the function.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the function.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the function.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the function.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the function.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the function.</typeparam>
        /// <typeparam name="TArg16">The type of the sixteenth argument passed to the function.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">Function to convert to an asynchronous function.</param>
        /// <param name="scheduler">Scheduler to invoke the original function on.</param>
        /// <returns>Asynchronous function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult> function, IScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(function, scheduler);
        }
#endif

        #endregion

        #region Action

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<IObservable<Unit>> ToAsync(this Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<IObservable<Unit>> ToAsync(this Action action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, IObservable<Unit>> ToAsync<TArg1>(this Action<TArg1> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, IObservable<Unit>> ToAsync<TArg1>(this Action<TArg1> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, IObservable<Unit>> ToAsync<TArg1, TArg2>(this Action<TArg1, TArg2> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, IObservable<Unit>> ToAsync<TArg1, TArg2>(this Action<TArg1, TArg2> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3>(this Action<TArg1, TArg2, TArg3> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3>(this Action<TArg1, TArg2, TArg3> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(this Action<TArg1, TArg2, TArg3, TArg4> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(this Action<TArg1, TArg2, TArg3, TArg4> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4>(action, scheduler);
        }

#if !NO_LARGEARITY

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(action, scheduler);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the default scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg16">The type of the sixteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(action);
        }

        /// <summary>
        /// Converts the function into an asynchronous action. Each invocation of the resulting asynchronous action causes an invocation of the original synchronous action on the specified scheduler.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument passed to the action.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument passed to the action.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument passed to the action.</typeparam>
        /// <typeparam name="TArg4">The type of the fourth argument passed to the action.</typeparam>
        /// <typeparam name="TArg5">The type of the fifth argument passed to the action.</typeparam>
        /// <typeparam name="TArg6">The type of the sixth argument passed to the action.</typeparam>
        /// <typeparam name="TArg7">The type of the seventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg8">The type of the eighth argument passed to the action.</typeparam>
        /// <typeparam name="TArg9">The type of the ninth argument passed to the action.</typeparam>
        /// <typeparam name="TArg10">The type of the tenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg11">The type of the eleventh argument passed to the action.</typeparam>
        /// <typeparam name="TArg12">The type of the twelfth argument passed to the action.</typeparam>
        /// <typeparam name="TArg13">The type of the thirteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg14">The type of the fourteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg15">The type of the fifteenth argument passed to the action.</typeparam>
        /// <typeparam name="TArg16">The type of the sixteenth argument passed to the action.</typeparam>
        /// <param name="action">Action to convert to an asynchronous action.</param>
        /// <param name="scheduler">Scheduler to invoke the original action on.</param>
        /// <returns>Asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> or <paramref name="scheduler"/> is null.</exception>
        public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> action, IScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(action, scheduler);
        }
#endif

        #endregion

        #endregion
    }
}
