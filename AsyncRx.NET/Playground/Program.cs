// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
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
            //await AggregateAsync();
            //await AllAsync();
            //await AnyAsync();
            //await AppendAsync();
            //await AwaitAsync();
            //await BufferTimeHoppingAsync();
            //await BufferTimeSlidingAsync();
            //await CombineLatestAsync();
            //await ConcatAsync();
            //await DelayAsync();
            //await GroupByAsync();
            //await GroupBySelectManyAsync();
            //await MergeAsync();
            //await PrependAsync();
            //await RangeAsync();
            //await ReplaySubjectAsync();
            //await ReturnAsync();
            //await SelectManyAsync();
            //await SubjectAsync();
            //await TakeUntilAsync();
            //await TimerAsync();
            //await WhileAsync();
        }

        static async Task AggregateAsync()
        {
            await AsyncObservable.Range(0, 10).Aggregate(0, (sum, x) => sum + x).SubscribeAsync(Print<int>());
        }

        static async Task AllAsync()
        {
            await AsyncObservable.Range(0, 10).All(x => x < 10).SubscribeAsync(Print<bool>());
        }

        static async Task AnyAsync()
        {
            await AsyncObservable.Range(0, 10).Any(x => x == 5).SubscribeAsync(Print<bool>());
        }

        static async Task AppendAsync()
        {
            await AsyncObservable.Range(0, 10).Append(42).SubscribeAsync(Print<int>());
        }

        static async Task AwaitAsync()
        {
            Console.WriteLine(await AsyncObservable.Range(0, 10));
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

        static async Task CombineLatestAsync()
        {
            await
                AsyncObservable.CombineLatest(
                    AsyncObservable.Interval(TimeSpan.FromMilliseconds(250)).Take(10).Timestamp(),
                    AsyncObservable.Interval(TimeSpan.FromMilliseconds(333)).Take(10).Timestamp(),
                    (x, y) => x.ToString() + ", " + y.ToString()
                )
                .SubscribeAsync(Print<string>()); // TODO: Use ForEachAsync.
        }

        static async Task ConcatAsync()
        {
            await
                AsyncObservable.Concat(
                    AsyncObservable.Range(0, 5),
                    AsyncObservable.Range(5, 5),
                    AsyncObservable.Range(10, 5),
                    AsyncObservable.Range(15, 5)
                )
                .SubscribeAsync(Print<int>()); // TODO: Use ForEachAsync.
        }

        static async Task DelayAsync()
        {
            await
                AsyncObservable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                    .Timestamp()
                    .Delay(TimeSpan.FromMilliseconds(2500))
                    .Timestamp()
                    .Select(x => new TimeInterval<long>(x.Value.Value, x.Timestamp - x.Value.Timestamp).ToString())
                    .SubscribeAsync(Print<string>()); // TODO: Use ForEachAsync.
        }

        static async Task GroupByAsync()
        {
            await
                AsyncObservable.Interval(TimeSpan.FromMilliseconds(250))
                    .Timestamp()
                    .Take(20)
                    .GroupBy(t => t.Timestamp.Millisecond / 100)
                    .SubscribeAsync(async g =>
                    {
                        await g.Select(x => g.Key + " - " + x).SubscribeAsync(Print<string>());
                    });
        }

        static async Task GroupBySelectManyAsync()
        {
            await
                AsyncObservable.Interval(TimeSpan.FromMilliseconds(250))
                    .Timestamp()
                    .Take(20)
                    .GroupBy(t => t.Timestamp.Millisecond / 100)
                    .SelectMany(g => g, (g, x) => g.Key + " - " + x)
                    .SubscribeAsync(Print<string>());
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

        static async Task PrependAsync()
        {
            await AsyncObservable.Range(0, 10).Prepend(42).SubscribeAsync(Print<int>());
        }

        static async Task RangeAsync()
        {
            await AsyncObservable.Range(0, 10).SubscribeAsync(PrintAsync<int>()); // TODO: Use ForEachAsync.
        }

        static async Task ReplaySubjectAsync()
        {
            var sub = new SequentialReplayAsyncSubject<int>(5);

            var d1 = await sub.SubscribeAsync(x => Console.WriteLine("1> " + x));

            await sub.OnNextAsync(40);
            await sub.OnNextAsync(41);

            var d2 = await sub.SubscribeAsync(x => Console.WriteLine("2> " + x));

            await sub.OnNextAsync(42);

            await d1.DisposeAsync();

            await sub.OnNextAsync(43);

            var d3 = await sub.SubscribeAsync(x => Console.WriteLine("3> " + x));

            await sub.OnNextAsync(44);
            await sub.OnNextAsync(45);

            await d3.DisposeAsync();

            await sub.OnNextAsync(46);

            await d2.DisposeAsync();

            await sub.OnNextAsync(47);
        }

        static async Task ReturnAsync()
        {
            await AsyncObservable.Return(42).SubscribeAsync(Print<int>());
        }

        static async Task SelectManyAsync()
        {
            var res = from i in AsyncObservable.Range(0, 10)
                      from j in AsyncObservable.Range(i * 10, 10)
                      select i + " -> " + j;

            await res.SubscribeAsync(Print<string>());
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

        static async Task WhileAsync()
        {
            var i = 0;

            await AsyncObservable.While(() => ++i < 5, AsyncObservable.Range(0, 5)).SubscribeAsync(Print<int>()); // TODO: Use ForEachAsync.
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

        static IAsyncObserver<T> PrintAsync<T>()
        {
            return AsyncObserver.Create<T>(
                async x =>
                {
                    await Task.Yield();
                    Console.WriteLine(x);
                },
                async ex =>
                {
                    await Task.Yield();
                    Console.WriteLine("Error: " + ex);
                },
                async () =>
                {
                    await Task.Yield();
                    Console.WriteLine("Completed");
                }
            );
        }
    }
}
