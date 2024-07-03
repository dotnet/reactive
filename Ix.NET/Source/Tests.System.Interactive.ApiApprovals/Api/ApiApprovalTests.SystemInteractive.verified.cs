[assembly: System.CLSCompliant(true)]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName=".NET 6.0")]
namespace System.Linq
{
    public static class EnumerableEx
    {
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int count, int skip) { }
        public static System.Collections.Generic.IEnumerable<TResult> Case<TValue, TResult>(System.Func<TValue> selector, System.Collections.Generic.IDictionary<TValue, System.Collections.Generic.IEnumerable<TResult>> sources) { }
        public static System.Collections.Generic.IEnumerable<TResult> Case<TValue, TResult>(System.Func<TValue> selector, System.Collections.Generic.IDictionary<TValue, System.Collections.Generic.IEnumerable<TResult>> sources, System.Collections.Generic.IEnumerable<TResult> defaultSource) { }
        public static System.Collections.Generic.IEnumerable<TSource> Catch<TSource>(params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Collections.Generic.IEnumerable<TSource> Catch<TSource>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Collections.Generic.IEnumerable<TSource> Catch<TSource>(this System.Collections.Generic.IEnumerable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.Collections.Generic.IEnumerable<TSource> Catch<TSource, TException>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TException, System.Collections.Generic.IEnumerable<TSource>> handler)
            where TException : System.Exception { }
        public static System.Collections.Generic.IEnumerable<TSource> Concat<TSource>(params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Collections.Generic.IEnumerable<TSource> Concat<TSource>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Collections.Generic.IEnumerable<T> Create<T>(System.Action<System.Linq.IYielder<T>> create) { }
        public static System.Collections.Generic.IEnumerable<TResult> Create<TResult>(System.Func<System.Collections.Generic.IEnumerator<TResult>> getEnumerator) { }
        public static System.Collections.Generic.IEnumerable<TResult> Defer<TResult>(System.Func<System.Collections.Generic.IEnumerable<TResult>> enumerableFactory) { }
        public static System.Collections.Generic.IEnumerable<TSource> Distinct<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Collections.Generic.IEnumerable<TSource> Distinct<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.IObserver<TSource> observer) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext, System.Action onCompleted) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext, System.Action<System.Exception> onError) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext, System.Action<System.Exception> onError, System.Action onCompleted) { }
        public static System.Collections.Generic.IEnumerable<TResult> DoWhile<TResult>(this System.Collections.Generic.IEnumerable<TResult> source, System.Func<bool> condition) { }
        public static System.Collections.Generic.IEnumerable<TSource> Expand<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, System.Collections.Generic.IEnumerable<TSource>> selector) { }
        public static System.Collections.Generic.IEnumerable<TSource> Finally<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Action finallyAction) { }
        public static System.Collections.Generic.IEnumerable<TResult> For<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, System.Collections.Generic.IEnumerable<TResult>> resultSelector) { }
        public static void ForEach<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext) { }
        public static void ForEach<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource, int> onNext) { }
        public static System.Collections.Generic.IEnumerable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector) { }
        public static System.Collections.Generic.IEnumerable<TSource> Hide<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TResult> If<TResult>(System.Func<bool> condition, System.Collections.Generic.IEnumerable<TResult> thenSource) { }
        public static System.Collections.Generic.IEnumerable<TResult> If<TResult>(System.Func<bool> condition, System.Collections.Generic.IEnumerable<TResult> thenSource, System.Collections.Generic.IEnumerable<TResult> elseSource) { }
        public static System.Collections.Generic.IEnumerable<TSource> IgnoreElements<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static bool IsEmpty<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static TSource Max<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        [System.Obsolete("Use MaxByWithTies to maintain same behavior with .NET 6 and later", false)]
        public static System.Collections.Generic.IList<TSource> MaxBy<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        [System.Obsolete("Use MaxByWithTies to maintain same behavior with .NET 6 and later", false)]
        public static System.Collections.Generic.IList<TSource> MaxBy<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IList<TSource> MaxByWithTies<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Collections.Generic.IList<TSource> MaxByWithTies<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Linq.IBuffer<TSource> Memoize<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Linq.IBuffer<TSource> Memoize<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int readerCount) { }
        public static System.Collections.Generic.IEnumerable<TResult> Memoize<TSource, TResult>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.Collections.Generic.IEnumerable<TResult> Memoize<TSource, TResult>(this System.Collections.Generic.IEnumerable<TSource> source, int readerCount, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static TSource Min<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        [System.Obsolete("Use MinByWithTies to maintain same behavior with .NET 6 and later", false)]
        public static System.Collections.Generic.IList<TSource> MinBy<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        [System.Obsolete("Use MinByWithTies to maintain same behavior with .NET 6 and later", false)]
        public static System.Collections.Generic.IList<TSource> MinBy<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IList<TSource> MinByWithTies<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Collections.Generic.IList<TSource> MinByWithTies<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> OnErrorResumeNext<TSource>(params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Collections.Generic.IEnumerable<TSource> OnErrorResumeNext<TSource>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Collections.Generic.IEnumerable<TSource> OnErrorResumeNext<TSource>(this System.Collections.Generic.IEnumerable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.Linq.IBuffer<TSource> Publish<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TResult> Publish<TSource, TResult>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.Collections.Generic.IEnumerable<TResult> Repeat<TResult>(TResult value) { }
        public static System.Collections.Generic.IEnumerable<TSource> Repeat<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TResult> Repeat<TResult>(TResult element, int count) { }
        public static System.Collections.Generic.IEnumerable<TSource> Repeat<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Collections.Generic.IEnumerable<TSource> Retry<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TSource> Retry<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int retryCount) { }
        public static System.Collections.Generic.IEnumerable<TResult> Return<TResult>(TResult value) { }
        public static System.Collections.Generic.IEnumerable<TSource> Scan<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TSource, TSource> accumulator) { }
        public static System.Collections.Generic.IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this System.Collections.Generic.IEnumerable<TSource> source, TAccumulate seed, System.Func<TAccumulate, TSource, TAccumulate> accumulator) { }
        public static System.Collections.Generic.IEnumerable<TOther> SelectMany<TSource, TOther>(this System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IEnumerable<TOther> other) { }
        public static System.Linq.IBuffer<TSource> Share<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TResult> Share<TSource, TResult>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.Collections.Generic.IEnumerable<TSource> SkipLast<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Collections.Generic.IEnumerable<TSource> StartWith<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, params TSource[] values) { }
        public static System.Collections.Generic.IEnumerable<TSource> TakeLast<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Collections.Generic.IEnumerable<TResult> Throw<TResult>(System.Exception exception) { }
        public static System.Collections.Generic.IEnumerable<TSource> Using<TSource, TResource>(System.Func<TResource> resourceFactory, System.Func<TResource, System.Collections.Generic.IEnumerable<TSource>> enumerableFactory)
            where TResource : System.IDisposable { }
        public static System.Collections.Generic.IEnumerable<TResult> While<TResult>(System.Func<bool> condition, System.Collections.Generic.IEnumerable<TResult> source) { }
    }
    public interface IAwaitable
    {
        System.Linq.IAwaiter GetAwaiter();
    }
    public interface IAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
    {
        bool IsCompleted { get; }
        void GetResult();
    }
    public interface IBuffer<out T> : System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable, System.IDisposable { }
    public interface IYielder<in T>
    {
        System.Linq.IAwaitable Break();
        System.Linq.IAwaitable Return(T value);
    }
}
