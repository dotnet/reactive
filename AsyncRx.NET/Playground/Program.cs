using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var subject = new SequentialSimpleAsyncSubject<int>();

            var res = subject.Where(x => x % 2 == 0).Select(x => x + 1);

            await res.SubscribeAsync(
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

            for (var i = 0; i < 10; i++)
            {
                await subject.OnNextAsync(i);
            }

            await subject.OnCompletedAsync();
        }
    }
}
