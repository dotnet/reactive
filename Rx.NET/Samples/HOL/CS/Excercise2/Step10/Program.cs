using System;
using System.Linq;
using System.Reactive.Linq;

namespace Excercise2
{
    class Program
    {
        static void Main(string[] args)
        {
            IObservable<int> source = Observable.Generate(
                0, i => i < 5,
                i => i + 1,
                i => i * i, 
                i => TimeSpan.FromSeconds(i)
            );

            using (source.Subscribe(
                x => Console.WriteLine("OnNext:  {0}", x),
                ex => Console.WriteLine("OnError: {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")
            ))
            {
                Console.WriteLine("Press ENTER to unsubscribe...");
                Console.ReadLine();
            }
        }
    }
}
