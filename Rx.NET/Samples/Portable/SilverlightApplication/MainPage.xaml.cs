using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reactive;
using System.Reactive.Linq;

namespace SilverlightApplication
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var portableClass = new PortableClassLibrary.PortableClass();

            // Create timer and route output to list1
            portableClass.CreateTimer(10, TimeSpan.FromSeconds(1.5))
                .Buffer(2)
                .ObserveOnDispatcher()
                .Subscribe(items =>
                {
                    foreach (var item in items)
                        list1.Items.Add(item);
                }, onCompleted: () =>
                {
                    list1.Items.Add("Finished");
                });

            // Create list observer and route output to list1, but specify scheduler instead of using SubscribeOnDispatcher
            var scheduler = System.Reactive.Concurrency.DispatcherScheduler.Current;
            portableClass.CreateList(scheduler)
                .Delay(TimeSpan.FromSeconds(1))
                .ObserveOn(scheduler)
                .Subscribe(item =>
                {
                    list2.Items.Add(item);
                }, onCompleted: () =>
                {
                    list2.Items.Add("Finished");
                });


        }
    }
}
