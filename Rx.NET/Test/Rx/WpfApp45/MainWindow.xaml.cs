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

namespace WpfApp45
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var clock = MyExtensions.GetClock().AsQbservable().Select(_ => _).AsObservable();

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
