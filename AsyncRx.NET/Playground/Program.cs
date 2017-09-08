using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Playground
{
    static class Program
    {
        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();

            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            await BufferTimeHoppingAsync();
            await BufferTimeSlidingAsync();
            await MergeAsync();
            await RangeAsync();
            await ReturnAsync();
            await SubjectAsync();
            await TakeUntilAsync();
            await TimerAsync();
        }

        static async Task BufferTimeHoppingAsync()
        {
            await
                AsyncObservable
                    .Interval(TimeSpan.FromMilliseconds(300))
                    .Buffer(TimeSpan.FromSeconds(1))
                    .Select(xs => string.Join(", ", xs))
                    .SubscribeAsync(Print<string>()); // TODO: Use ForEachAsync.
        }

        static async Task BufferTimeSlidingAsync()
        {
            await
                AsyncObservable
                    .Interval(TimeSpan.FromMilliseconds(100))
                    .Timestamp(TaskPoolAsyncScheduler.Default)
                    .Buffer(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(300))
                    .Select(xs => $"[{xs.First().Timestamp}, {xs.Last().Timestamp}] = {(xs.Last().Timestamp - xs.First().Timestamp).TotalMilliseconds}")
                    .SubscribeAsync(Print<string>()); // TODO: Use ForEachAsync.
        }

        static async Task MergeAsync()
        {
            var subject = new SequentialSimpleAsyncSubject<IAsyncObservable<int>>();

            var res = subject.Merge();

            await res.SubscribeAsync(Print<int>());

            for (var i = 1; i <= 10; i++)
            {
                await subject.OnNextAsync(AsyncObservable.Range(0, i));
            }

            await subject.OnCompletedAsync();
        }

        static async Task RangeAsync()
        {
            await AsyncObservable.Range(0, 10).SubscribeAsync(Print<int>()); // TODO: Use ForEachAsync.
        }

        static async Task ReturnAsync()
        {
            await AsyncObservable.Return(42).SubscribeAsync(Print<int>());
        }

        static async Task SubjectAsync()
        {
            var subject = new SequentialSimpleAsyncSubject<int>();

            var res = subject.Where(x => x % 2 == 0).Select(x => x + 1);

            await res.SubscribeAsync(Print<int>());

            for (var i = 0; i < 10; i++)
            {
                await subject.OnNextAsync(i);
            }

            await subject.OnCompletedAsync();
        }

        static async Task TakeUntilAsync()
        {
            await AsyncObservable.Range(0, int.MaxValue).TakeUntil(DateTimeOffset.Now.AddSeconds(5)).SubscribeAsync(Print<int>()); // TODO: Use ForEachAsync.
        }

        static async Task TimerAsync()
        {
            await AsyncObservable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)).Take(5).Select(_ => DateTimeOffset.Now).SubscribeAsync(Print<DateTimeOffset>()); // TODO: Use ForEachAsync.
        }

        static IAsyncObserver<T> Print<T>()
        {
            return AsyncObserver.Create<T>(
                x =>
                {
                    Console.WriteLine(x);
                    return Task.CompletedTask;
                },
                ex =>
                {
                    Console.WriteLine("Error: " + ex);
                    return Task.CompletedTask;
                },
                () =>
                {
                    Console.WriteLine("Completed");
                    return Task.CompletedTask;
                }
            );
        }
    }
}
