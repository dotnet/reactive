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
            
            var frm = new Form()
            {
                Controls = { txt }
            };

            Application.Run(frm);

        }
    }
}
