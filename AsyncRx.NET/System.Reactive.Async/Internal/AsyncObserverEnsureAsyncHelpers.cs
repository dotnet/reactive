using System.Threading.Tasks;

namespace System.Reactive.Internal;

// Helpers methods that ensure that calls to IAsyncObserver methods don't throw synchronously.
// Those methods will always return a ValueTask, and any exception will be propagated through that ValueTask.
internal static class AsyncObserverEnsureAsyncHelpers
{
    public static ValueTask OnNextAsync_EnsureAsync<T>(this IAsyncObserver<T> source, T value)
    {
        try
        {
            return source.OnNextAsync(value);
        }
        catch (Exception e)
        {
            return new ValueTask(Task.FromException(e));
        }
    }

    public static ValueTask OnErrorAsync_EnsureAsync<T>(this IAsyncObserver<T> source, Exception error)
    {
        try
        {
            return source.OnErrorAsync(error);
        }
        catch (Exception e)
        {
            return new ValueTask(Task.FromException(e));
        }
    }

    public static ValueTask OnCompletedAsync_EnsureAsync<T>(this IAsyncObserver<T> source)
    {
        try
        {
            return source.OnCompletedAsync();
        }
        catch (Exception e)
        {
            return new ValueTask(Task.FromException(e));
        }
    }
}
