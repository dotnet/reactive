using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive.Disposables;

namespace Excercise8
{
    class Program
    {
        static void Main()
        {
            var txt = new TextBox();
            var lst = new ListBox { Top = txt.Height + 10 };

            var frm = new Form()
            {
                Controls = { txt, lst }
            };

            var input = (from evt in Observable.FromEventPattern(txt, "TextChanged")
                         select ((TextBox)evt.Sender).Text)
                         .Throttle(TimeSpan.FromSeconds(1))
                         .DistinctUntilChanged()
                         .Do(x => Console.WriteLine(x));

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(frm);
            }
        }
    }
}
