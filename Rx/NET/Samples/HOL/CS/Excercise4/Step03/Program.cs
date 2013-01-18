using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive.Disposables;

namespace Excercise4
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

            var moves = from evt in Observable.FromEventPattern<MouseEventArgs>(frm, "MouseMove")
                        select evt.EventArgs.Location;

            var input = from evt in Observable.FromEventPattern(txt, "TextChanged")
                        select ((TextBox)evt.Sender).Text;

            var movesSubscription = moves.Subscribe(pos => Console.WriteLine("Mouse at: "   + pos));
            var inputSubscription = input.Subscribe(inp => Console.WriteLine("User wrote: " + inp));

            using (new CompositeDisposable(movesSubscription, inputSubscription))
            {
                Application.Run(frm);
            }
        }
    }
}
