using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Zeroconf;

namespace ZeroconfTest.NetFx
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        async void Button_Click(object sender, RoutedEventArgs e)
        {

            //Action<IZeroconfRecord> onMessage = record => Console.WriteLogLine("On Message: {0}", record);


            var domains = await ZeroconfResolver.BrowseDomainsAsync();
            
            var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));
            // var responses = await ZeroconfResolver.ResolveAsync("_http._tcp.local.");
            
            foreach (var resp in responses)
                WriteLogLine(resp.ToString());
        }

        async void Browse_Click(object sender, RoutedEventArgs e)
        {
            var responses = await ZeroconfResolver.BrowseDomainsAsync();
            
            foreach (var service in responses)
            {
                WriteLogLine(service.Key);

                foreach (var host in service)
                    WriteLogLine("\tIP: " + host);

            }
        }

        private void WriteLogLine(string text, params object[] args)
        {
            if (Log.Dispatcher.CheckAccess())
            {
                Log.AppendText(string.Format(text, args) + "\r\n");
                Log.ScrollToEnd();
            }
            else
            {
                Log.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => WriteLogLine(text, args)));
            }
        }

        private void OnAnnouncement(ServiceAnnouncement sa)
        {
            Log.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                WriteLogLine("---- Announced on {0} ({1}) ----", sa.AdapterInformation.Name, sa.AdapterInformation.Address);
                WriteLogLine(sa.Host.ToString());
            }));
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (listenSubscription != null)
            {
                listenSubscription.Dispose();
                listenSubscription = null;
            }
        }

        IDisposable listenSubscription;
        IObservable<ServiceAnnouncement> subscription;
            
        
        void StartStopListener_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListenButton.IsChecked = false;

                if (listenSubscription != null)
                {
                    listenSubscription.Dispose();
                    listenSubscription = null;
                }
                else
                {
                    subscription = ZeroconfResolver.ListenForAnnouncementsAsync();
                    listenSubscription = subscription.Subscribe(OnAnnouncement);
                }

            }
            finally
            {
                ListenButton.IsChecked = true;
            }
        }

        async void ResolveContinuous_OnClickListener_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResolveContinuous.IsChecked = false;

                if (listenSubscription != null)
                {
                    listenSubscription.Dispose();
                    listenSubscription = null;
                }
                else
                {
                    var domains = await ZeroconfResolver.BrowseDomainsAsync();
                    var sub = ZeroconfResolver.ResolveContinuous(domains.Select(g => g.Key));
                    listenSubscription = sub.Subscribe(resp => WriteLogLine(resp.ToString()));
                }

            }
            finally
            {
                ResolveContinuous.IsChecked = true;
            }
        }
    }
}
