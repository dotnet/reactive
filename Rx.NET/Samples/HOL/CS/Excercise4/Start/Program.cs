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

            var moves = Observable.FromEventPattern<MouseEventArgs>(frm, "MouseMove");
            var input = Observable.FromEventPattern<EventArgs>(txt, "TextChanged");

            var moveSubscription = moves.Subscribe(evt =>
                {
                    Console.WriteLine("Mouse at: " + evt.EventArgs.Location);
                });

            var inputSubscription = input.Subscribe(evt =>
                {
                    Console.WriteLine("User wrote: " + ((TextBox)evt.Sender).Text);
                });

            using (new CompositeDisposable(moveSubscription, inputSubscription))
            {
                Application.Run(frm);
            }
        }
    }
}
