using System;
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
        }

        static async Task MainAsync()
        {
            await RangeAsync();
            await ReturnAsync();
            await SubjectAsync();
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
