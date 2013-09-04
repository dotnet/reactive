using System;
using System.Collections;
using System.Drawing;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using RxMouseService;

namespace RxMouseServer
{
    partial class Program
    {
        const string SERVICENAME = "MouseService";

        static IObserver<Point> Remoting(int port)
        {
            ConfigureRemoting(port);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(MouseService), SERVICENAME, WellKnownObjectMode.Singleton);

            var ms = (IMouseService)Activator.GetObject(typeof(IMouseService), string.Format("tcp://{0}:{1}/{2}", "localhost", port, SERVICENAME));

            return (IObserver<Point>)ms;
        }

        private static void ConfigureRemoting(int port)
        {
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            var clientProvider = new BinaryClientFormatterSinkProvider();

            IDictionary props = new Hashtable();
            props["port"] = port;
            props["name"] = SERVICENAME;
            props["typeFilterLevel"] = TypeFilterLevel.Full;

            ChannelServices.RegisterChannel(new TcpChannel(props, clientProvider, serverProvider), false);
        }
    }
}
