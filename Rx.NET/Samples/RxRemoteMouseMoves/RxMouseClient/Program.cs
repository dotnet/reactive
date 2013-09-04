using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace RxMouseClient
{
    partial class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Client");

            string server;
            int port;
            ParseArgs(args, out server, out port);

            var points = Remoting(server, port);

            var frm = new Form();

            var closed = Observable.FromEventPattern(frm, "FormClosed");

            frm.Load += (o, e) =>
            {
                var g = frm.CreateGraphics();

                points.TakeUntil(closed).ObserveOn(frm).Subscribe(pt =>
                {
                    g.DrawEllipse(Pens.Red, pt.X, pt.Y, 1, 1);
                });
            };

            Application.Run(frm);
        }

        static void ParseArgs(string[] args, out string server, out int port)
        {
            port = 9090;
            server = "localhost";

            if (args.Length >= 1)
            {
                server = args[0];

                if (args.Length == 2)
                {
                    port = int.Parse(args[1]);
                }
            }
        }
    }
}
