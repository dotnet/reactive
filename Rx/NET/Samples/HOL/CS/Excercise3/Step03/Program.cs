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

            frm.MouseMove += (sender, args) =>
            {
                lbl.Text = args.Location.ToString(); // This has become a position-tracking label.
            };

            Application.Run(frm);
        }
    }
}
