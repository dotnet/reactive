[assembly: System.CLSCompliant(true)]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName=".NET 6.0")]
namespace System.Linq
{
    public static class QueryableEx
    {
        public static System.Linq.IQueryProvider Provider { get; }
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Linq.IQueryable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Linq.IQueryable<TSource> source, int count) { }
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(System.Collections.Generic.IEnumerable<TSource> source, int count, int skip) { }
        public static System.Linq.IQueryable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Linq.IQueryable<TSource> source, int count, int skip) { }
        public static System.Linq.IQueryable<TResult> Case<TValue, TResult>(System.Func<TValue> selector, System.Collections.Generic.IDictionary<TValue, System.Collections.Generic.IEnumerable<TResult>> sources) { }
        public static System.Linq.IQueryable<TResult> Case<TValue, TResult>(System.Func<TValue> selector, System.Collections.Generic.IDictionary<TValue, System.Collections.Generic.IEnumerable<TResult>> sources, System.Collections.Generic.IEnumerable<TResult> defaultSource) { }
        public static System.Linq.IQueryable<TResult> Case<TValue, TResult>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<TValue>> selector, System.Collections.Generic.IDictionary<TValue, System.Collections.Generic.IEnumerable<TResult>> sources) { }
        public static System.Linq.IQueryable<TResult> Case<TValue, TResult>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<TValue>> selector, System.Collections.Generic.IDictionary<TValue, System.Collections.Generic.IEnumerable<TResult>> sources, System.Collections.Generic.IEnumerable<TResult> defaultSource) { }
        public static System.Collections.Generic.IEnumerable<TSource> Catch<TSource>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Linq.IQueryable<TSource> Catch<TSource>(params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Linq.IQueryable<TSource> Catch<TSource>(this System.Linq.IQueryable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Collections.Generic.IEnumerable<TSource> Catch<TSource>(System.Collections.Generic.IEnumerable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.Linq.IQueryable<TSource> Catch<TSource>(this System.Linq.IQueryProvider provider, params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Linq.IQueryable<TSource> Catch<TSource>(this System.Linq.IQueryable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.Collections.Generic.IEnumerable<TSource> Catch<TSource, TException>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TException, System.Collections.Generic.IEnumerable<TSource>> handler)
            where TException : System.Exception { }
        public static System.Linq.IQueryable<TSource> Catch<TSource, TException>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TException, System.Collections.Generic.IEnumerable<TSource>>> handler)
            where TException : System.Exception { }
        public static System.Collections.Generic.IEnumerable<TSource> Concat<TSource>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Linq.IQueryable<TSource> Concat<TSource>(params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Linq.IQueryable<TSource> Concat<TSource>(this System.Linq.IQueryable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Linq.IQueryable<TSource> Concat<TSource>(this System.Linq.IQueryProvider provider, params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Collections.Generic.IEnumerable<TResult> Create<TResult>(System.Func<System.Collections.Generic.IEnumerator<TResult>> getEnumerator) { }
        public static System.Linq.IQueryable<TResult> Create<TResult>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerator<TResult>>> getEnumerator) { }
        public static System.Linq.IQueryable<TResult> Defer<TResult>(System.Func<System.Collections.Generic.IEnumerable<TResult>> enumerableFactory) { }
        public static System.Linq.IQueryable<TResult> Defer<TResult>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<TResult>>> enumerableFactory) { }
        public static System.Collections.Generic.IEnumerable<TSource> Distinct<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Linq.IQueryable<TSource> Distinct<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Collections.Generic.IEnumerable<TSource> Distinct<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Linq.IQueryable<TSource> Distinct<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource>(System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Linq.IQueryable<TSource> DistinctUntilChanged<TSource>(this System.Linq.IQueryable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Linq.IQueryable<TSource> DistinctUntilChanged<TSource>(this System.Linq.IQueryable<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Linq.IQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Collections.Generic.IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Linq.IQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.IObserver<TSource> observer) { }
        public static System.Linq.IQueryable<TSource> Do<TSource>(this System.Linq.IQueryable<TSource> source, System.IObserver<TSource> observer) { }
        public static System.Linq.IQueryable<TSource> Do<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext, System.Action onCompleted) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext, System.Action<System.Exception> onError) { }
        public static System.Linq.IQueryable<TSource> Do<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext, System.Linq.Expressions.Expression<System.Action> onCompleted) { }
        public static System.Linq.IQueryable<TSource> Do<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext, System.Linq.Expressions.Expression<System.Action<System.Exception>> onError) { }
        public static System.Collections.Generic.IEnumerable<TSource> Do<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> onNext, System.Action<System.Exception> onError, System.Action onCompleted) { }
        public static System.Linq.IQueryable<TSource> Do<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext, System.Linq.Expressions.Expression<System.Action<System.Exception>> onError, System.Linq.Expressions.Expression<System.Action> onCompleted) { }
        public static System.Collections.Generic.IEnumerable<TResult> DoWhile<TResult>(System.Collections.Generic.IEnumerable<TResult> source, System.Func<bool> condition) { }
        public static System.Linq.IQueryable<TResult> DoWhile<TResult>(this System.Linq.IQueryable<TResult> source, System.Linq.Expressions.Expression<System.Func<bool>> condition) { }
        public static System.Linq.IQueryable<TResult> Empty<TResult>() { }
        public static System.Linq.IQueryable<TResult> Empty<TResult>(this System.Linq.IQueryProvider provider) { }
        public static System.Collections.Generic.IEnumerable<TSource> Expand<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, System.Collections.Generic.IEnumerable<TSource>> selector) { }
        public static System.Linq.IQueryable<TSource> Expand<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Collections.Generic.IEnumerable<TSource>>> selector) { }
        public static System.Collections.Generic.IEnumerable<TSource> Finally<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Action finallyAction) { }
        public static System.Linq.IQueryable<TSource> Finally<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Action> finallyAction) { }
        public static System.Linq.IQueryable<TResult> For<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, System.Collections.Generic.IEnumerable<TResult>> resultSelector) { }
        public static System.Linq.IQueryable<TResult> For<TSource, TResult>(this System.Linq.IQueryProvider provider, System.Collections.Generic.IEnumerable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Collections.Generic.IEnumerable<TResult>>> resultSelector) { }
        public static System.Linq.IQueryable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector) { }
        public static System.Linq.IQueryable<TResult> Generate<TState, TResult>(this System.Linq.IQueryProvider provider, TState initialState, System.Linq.Expressions.Expression<System.Func<TState, bool>> condition, System.Linq.Expressions.Expression<System.Func<TState, TState>> iterate, System.Linq.Expressions.Expression<System.Func<TState, TResult>> resultSelector) { }
        public static System.Collections.Generic.IEnumerable<TSource> Hide<TSource>(System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Linq.IQueryable<TSource> Hide<TSource>(this System.Linq.IQueryable<TSource> source) { }
        public static System.Linq.IQueryable<TResult> If<TResult>(System.Func<bool> condition, System.Collections.Generic.IEnumerable<TResult> thenSource) { }
        public static System.Linq.IQueryable<TResult> If<TResult>(System.Func<bool> condition, System.Collections.Generic.IEnumerable<TResult> thenSource, System.Collections.Generic.IEnumerable<TResult> elseSource) { }
        public static System.Linq.IQueryable<TResult> If<TResult>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<bool>> condition, System.Collections.Generic.IEnumerable<TResult> thenSource) { }
        public static System.Linq.IQueryable<TResult> If<TResult>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<bool>> condition, System.Collections.Generic.IEnumerable<TResult> thenSource, System.Collections.Generic.IEnumerable<TResult> elseSource) { }
        public static System.Collections.Generic.IEnumerable<TSource> IgnoreElements<TSource>(System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Linq.IQueryable<TSource> IgnoreElements<TSource>(this System.Linq.IQueryable<TSource> source) { }
        public static bool IsEmpty<TSource>(System.Collections.Generic.IEnumerable<TSource> source) { }
        public static bool IsEmpty<TSource>(this System.Linq.IQueryable<TSource> source) { }
        public static TSource Max<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static TSource Max<TSource>(this System.Linq.IQueryable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static System.Collections.Generic.IList<TSource> MaxBy<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Collections.Generic.IList<TSource> MaxBy<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Collections.Generic.IList<TSource> MaxBy<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IList<TSource> MaxBy<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IList<TSource> MaxByWithTies<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Collections.Generic.IList<TSource> MaxByWithTies<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Collections.Generic.IEnumerable<TResult> Memoize<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.Linq.IQueryable<TResult> Memoize<TSource, TResult>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>>> selector) { }
        public static System.Collections.Generic.IEnumerable<TResult> Memoize<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource> source, int readerCount, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.Linq.IQueryable<TResult> Memoize<TSource, TResult>(this System.Linq.IQueryable<TSource> source, int readerCount, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>>> selector) { }
        public static TSource Min<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static TSource Min<TSource>(this System.Linq.IQueryable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static System.Collections.Generic.IList<TSource> MinBy<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.Collections.Generic.IList<TSource> MinBy<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Collections.Generic.IList<TSource> MinBy<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IList<TSource> MinBy<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IList<TSource> MinByWithTies<TSource, TKey>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IList<TSource> MinByWithTies<TSource, TKey>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> OnErrorResumeNext<TSource>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Linq.IQueryable<TSource> OnErrorResumeNext<TSource>(params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Linq.IQueryable<TSource> OnErrorResumeNext<TSource>(this System.Linq.IQueryable<System.Collections.Generic.IEnumerable<TSource>> sources) { }
        public static System.Collections.Generic.IEnumerable<TSource> OnErrorResumeNext<TSource>(System.Collections.Generic.IEnumerable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.Collections.Generic.IEnumerable<TSource> OnErrorResumeNext<TSource>(this System.Linq.IQueryProvider provider, params System.Collections.Generic.IEnumerable<TSource>[] sources) { }
        public static System.Linq.IQueryable<TSource> OnErrorResumeNext<TSource>(this System.Linq.IQueryable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.Collections.Generic.IEnumerable<TResult> Publish<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.Linq.IQueryable<TResult> Publish<TSource, TResult>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>>> selector) { }
        public static System.Linq.IQueryable<int> Range(int start, int count) { }
        public static System.Linq.IQueryable<int> Range(this System.Linq.IQueryProvider provider, int start, int count) { }
        public static System.Collections.Generic.IEnumerable<TSource> Repeat<TSource>(System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Linq.IQueryable<TResult> Repeat<TResult>(TResult value) { }
        public static System.Linq.IQueryable<TSource> Repeat<TSource>(this System.Linq.IQueryable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TSource> Repeat<TSource>(System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Linq.IQueryable<TResult> Repeat<TResult>(TResult element, int count) { }
        public static System.Collections.Generic.IEnumerable<TResult> Repeat<TResult>(this System.Linq.IQueryProvider provider, TResult value) { }
        public static System.Linq.IQueryable<TSource> Repeat<TSource>(this System.Linq.IQueryable<TSource> source, int count) { }
        public static System.Linq.IQueryable<TResult> Repeat<TResult>(this System.Linq.IQueryProvider provider, TResult element, int count) { }
        public static System.Collections.Generic.IEnumerable<TSource> Retry<TSource>(System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Linq.IQueryable<TSource> Retry<TSource>(this System.Linq.IQueryable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TSource> Retry<TSource>(System.Collections.Generic.IEnumerable<TSource> source, int retryCount) { }
        public static System.Linq.IQueryable<TSource> Retry<TSource>(this System.Linq.IQueryable<TSource> source, int retryCount) { }
        public static System.Linq.IQueryable<TResult> Return<TResult>(TResult value) { }
        public static System.Linq.IQueryable<TResult> Return<TResult>(this System.Linq.IQueryProvider provider, TResult value) { }
        public static System.Collections.Generic.IEnumerable<TSource> Scan<TSource>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, TSource, TSource> accumulator) { }
        public static System.Linq.IQueryable<TSource> Scan<TSource>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TSource, TSource>> accumulator) { }
        public static System.Collections.Generic.IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(System.Collections.Generic.IEnumerable<TSource> source, TAccumulate seed, System.Func<TAccumulate, TSource, TAccumulate> accumulator) { }
        public static System.Linq.IQueryable<TAccumulate> Scan<TSource, TAccumulate>(this System.Linq.IQueryable<TSource> source, TAccumulate seed, System.Linq.Expressions.Expression<System.Func<TAccumulate, TSource, TAccumulate>> accumulator) { }
        public static System.Collections.Generic.IEnumerable<TOther> SelectMany<TSource, TOther>(System.Collections.Generic.IEnumerable<TSource> source, System.Collections.Generic.IEnumerable<TOther> other) { }
        public static System.Linq.IQueryable<TOther> SelectMany<TSource, TOther>(this System.Linq.IQueryable<TSource> source, System.Collections.Generic.IEnumerable<TOther> other) { }
        public static System.Collections.Generic.IEnumerable<TResult> Share<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.Linq.IQueryable<TResult> Share<TSource, TResult>(this System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<TSource>, System.Collections.Generic.IEnumerable<TResult>>> selector) { }
        public static System.Collections.Generic.IEnumerable<TSource> SkipLast<TSource>(System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Linq.IQueryable<TSource> SkipLast<TSource>(this System.Linq.IQueryable<TSource> source, int count) { }
        public static System.Collections.Generic.IEnumerable<TSource> StartWith<TSource>(System.Collections.Generic.IEnumerable<TSource> source, params TSource[] values) { }
        public static System.Linq.IQueryable<TSource> StartWith<TSource>(this System.Linq.IQueryable<TSource> source, params TSource[] values) { }
        public static System.Collections.Generic.IEnumerable<TSource> TakeLast<TSource>(System.Collections.Generic.IEnumerable<TSource> source, int count) { }
        public static System.Linq.IQueryable<TSource> TakeLast<TSource>(this System.Linq.IQueryable<TSource> source, int count) { }
        public static System.Linq.IQueryable<TResult> Throw<TResult>(System.Exception exception) { }
        public static System.Linq.IQueryable<TResult> Throw<TResult>(this System.Linq.IQueryProvider provider, System.Exception exception) { }
        public static System.Linq.IQueryable<TSource> Using<TSource, TResource>(System.Func<TResource> resourceFactory, System.Func<TResource, System.Collections.Generic.IEnumerable<TSource>> enumerableFactory)
            where TResource : System.IDisposable { }
        public static System.Linq.IQueryable<TSource> Using<TSource, TResource>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<TResource>> resourceFactory, System.Linq.Expressions.Expression<System.Func<TResource, System.Collections.Generic.IEnumerable<TSource>>> enumerableFactory)
            where TResource : System.IDisposable { }
        public static System.Linq.IQueryable<TResult> While<TResult>(System.Func<bool> condition, System.Collections.Generic.IEnumerable<TResult> source) { }
        public static System.Linq.IQueryable<TResult> While<TResult>(this System.Linq.IQueryProvider provider, System.Linq.Expressions.Expression<System.Func<bool>> condition, System.Collections.Generic.IEnumerable<TResult> source) { }
    }
}