using System;
using System.Collections;
using System.Drawing;
using System.Reactive.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Windows.Forms;
using RxMouseService;

namespace RxMouseClient
{
    partial class Program
    {
        const string SERVICENAME = "MouseService";

        static IObservable<Point> Remoting(string srv, int port)
        {
            ConfigureRemoting();

            var ms = (IMouseService)Activator.GetObject(typeof(IMouseService), string.Format("tcp://{0}:{1}/{2}", srv, port, SERVICENAME));

            return ms.GetPoints();
        }

        private static void ConfigureRemoting()
        {
            var server = new BinaryServerFormatterSinkProvider();
            server.TypeFilterLevel = TypeFilterLevel.Full;

            var client = new BinaryClientFormatterSinkProvider();

            IDictionary props = new Hashtable();
            props["port"] = 0;
            props["name"] = SERVICENAME;
            props["typeFilterLevel"] = TypeFilterLevel.Full;

            ChannelServices.RegisterChannel(new TcpChannel(props, client, null), false);
        }
    }
}
