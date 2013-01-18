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
                         .Timestamp()
                         .Do(inp => Console.WriteLine("I: " + inp.Timestamp.Millisecond + " - " + inp.Value))
                         .Select(x => x.Value)
                         .Throttle(TimeSpan.FromSeconds(1))
                         .Timestamp()
                         .Do(inp => Console.WriteLine("T: " + inp.Timestamp.Millisecond + " - " + inp.Value))
                         .Select(x => x.Value)
                         .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(frm);
            }

        }
    }
}
