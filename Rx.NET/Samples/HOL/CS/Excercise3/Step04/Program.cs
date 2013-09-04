using System;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Excercise3
{
    class Program
    {
        static void Main()
        {
            var lbl = new Label();
            var frm = new Form()
            {
                Controls = { lbl }
            };

            var moves = Observable.FromEventPattern<MouseEventArgs>(frm, "MouseMove");

            using (moves.Subscribe(evt => 
            {
                lbl.Text = evt.EventArgs.Location.ToString();
            }))
            {
                Application.Run(frm);
            }
        }
    }
}
