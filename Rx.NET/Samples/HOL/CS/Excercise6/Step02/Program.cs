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
            var lbl = new Label { Left = txt.Width + 20 };

            var frm = new Form()
            {
                Controls = { txt, lbl }
            };

            var input = (from evt in Observable.FromEventPattern(txt, "TextChanged")
                         select ((TextBox)evt.Sender).Text)
                        .Throttle(TimeSpan.FromSeconds(1))
                        .DistinctUntilChanged();

            using (input.Subscribe(inp => lbl.Text = inp))
            {
                Application.Run(frm);
            }

        }
    }
}
