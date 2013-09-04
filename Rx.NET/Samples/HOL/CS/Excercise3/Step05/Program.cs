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
            var txt = new TextBox();
            
            var frm = new Form()
            {
                Controls = { lbl}
            };

            frm.Controls.Add(txt);

            Application.Run(frm);
        }
    }
}
