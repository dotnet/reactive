using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Excercise5
{
    class Program
    {
        static void Main()
        {
            var txt = new TextBox();
            
            var frm = new Form()
            {
                Controls = { txt }
            };

            var input = (from evt in Observable.FromEventPattern(txt, "TextChanged")
                         select ((TextBox)evt.Sender).Text)
                         .LogTimestampedValues(x => Console.WriteLine("I: " + x.Timestamp.Millisecond + " - " + x.Value))
                         .Throttle(TimeSpan.FromSeconds(1))
                         .LogTimestampedValues(x => Console.WriteLine("T: " + x.Timestamp.Millisecond + " - " + x.Value))
                         .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(frm);
            }

        }
    }
    
    public static class MyExtensions
    {
        public static IObservable<T> LogTimestampedValues<T>(this IObservable<T> source, Action<Timestamped<T>> onNext)
        {
            return source.Timestamp().Do(onNext).Select(x => x.Value);
        }
    }
}
