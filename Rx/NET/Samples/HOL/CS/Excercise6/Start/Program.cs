using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive.Disposables;

namespace Excercise6
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
                        .Throttle(TimeSpan.FromSeconds(1))
                        .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(frm);
            }

        }
    }
}
