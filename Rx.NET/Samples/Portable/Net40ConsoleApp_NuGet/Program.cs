using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;

namespace Net40ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            var portableClass = new PortableClassLibrary.PortableClass();

            var scheduler = System.Reactive.Concurrency.CurrentThreadScheduler.Instance;

            // Create timer and route output to console
            portableClass.CreateTimer(10, TimeSpan.FromSeconds(1.5))
                .Buffer(2)
                .ObserveOn(scheduler)
                .Subscribe(items =>
                {
                    Console.WriteLine(" 1: Received items {0}", string.Join(", ", items));
                }, onCompleted: () =>
                {
                    Console.WriteLine(" 1: Finished ");
                });

            // Create list observer and route output to console, but specify scheduler instead of using SubscribeOnDispatcher            
            portableClass.CreateList(scheduler)
                .Delay(TimeSpan.FromSeconds(1))
                .Subscribe(item =>
                {
                    Console.WriteLine(" 2: Received item {0}", item);
                }, onCompleted: () =>
                {
                    Console.WriteLine(" 2: Finished ");
                });

            
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
