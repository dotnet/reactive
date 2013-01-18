using System;
using System.Linq;
using System.Reactive.Linq;

namespace Excercise2
{
    class Program
    {
        static void Main(string[] args)
        {
            IObservable<int> source = Observable.Range(0, 10);

            source.ForEach(
                x  => Console.WriteLine("OnNext:  {0}", x),
                ex => Console.WriteLine("OnError: {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")
            );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();
           
        }
    }
}
