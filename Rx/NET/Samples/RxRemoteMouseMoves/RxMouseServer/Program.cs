using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace RxMouseServer
{
    partial class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Server");

            int port;
            ParseArgs(args, out port);

            var observer = Remoting(port);

            var frm = new Form();

            frm.Load += (o, e) =>
            {
                var g = frm.CreateGraphics();

                var mme = (from mm in Observable.FromEventPattern<MouseEventArgs>(frm, "MouseMove")
                           select mm.EventArgs.Location)
                          .DistinctUntilChanged()
                          .Do(pt =>
                          {
                              g.DrawEllipse(Pens.Red, pt.X, pt.Y, 1, 1);
                          });

                mme.Subscribe(observer);
            };

            Application.Run(frm);
        }

        static void ParseArgs(string[] args, out int port)
        {
            port = 9090;

            if (args.Length == 1)
            {
                port = int.Parse(args[0]);
            }
        }
    }
}
