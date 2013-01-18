using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PortableLibraryProfile7;

namespace WpfXbapApp45_NuGet
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //
            // Note: Local Qbservable compilation doesn't work in partial trust XBAP! (Cut for Rx v2.0 RTM.)
            //
            // var clock = MyExtensions.GetClock().AsQbservable().Select(_ => _).AsObservable();
            //

            var clock = MyExtensions.GetClock();

            var input = Observable.FromEventPattern<TextChangedEventArgs>(textBox1, "TextChanged").Select(evt => ((TextBox)evt.Sender).Text).Throttle(TimeSpan.FromSeconds(.5)).DistinctUntilChanged();

            var xs = from word in input.StartWith("")
                     from length in Task.Run(async () => { await Task.Delay(250); return word.Length; })
                     select length;

            var res = xs.CombineLatest(clock, (len, now) => now.ToString() + " - Word length = " + len);

            res.ObserveOnDispatcher().Subscribe(s =>
            {
                label1.Text = s.ToString();
            });
        }
    }
}
