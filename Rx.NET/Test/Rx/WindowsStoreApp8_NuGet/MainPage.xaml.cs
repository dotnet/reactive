using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PortableLibraryProfile7;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsStoreApp8_NuGet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var clock = MyExtensions.GetClock().AsQbservable().Select(_ => _).AsObservable();

            var txt = Observable.FromEventPattern<TextChangedEventArgs>(textBox1, "TextChanged").Select(evt => ((TextBox)evt.Sender).Text).Throttle(TimeSpan.FromSeconds(.5)).DistinctUntilChanged();

            var input = from t in Observable.Timer(TimeSpan.FromMilliseconds(100))
                        from _ in txt
                        select _;

            var xs = from word in input.StartWith("")
                     from length in Task.Run(async () => { await Task.Delay(250); return word.Length; }).AsAsyncOperation()
                     select length;

            var res = xs.CombineLatest(clock, (len, now) => now.ToString() + " - Word length = " + len);

            res.ObserveOnDispatcher().Subscribe(s =>
            {
                label1.Text = s.ToString();
            });
        }
    }
}
