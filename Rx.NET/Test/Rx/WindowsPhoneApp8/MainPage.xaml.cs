using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhoneApp8.Resources;
using PortableLibraryProfile7;
using System.Reactive.Linq;
using WindowsPhoneAgent8;

namespace WindowsPhoneApp8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            new ScheduledAgent();

            var clock = MyExtensions.GetClock().AsQbservable().Select(_ => _).AsObservable();

            var input = Observable.FromEventPattern<TextChangedEventArgs>(textBox1, "TextChanged").Select(evt => ((TextBox)evt.Sender).Text).Throttle(TimeSpan.FromSeconds(.5)).DistinctUntilChanged();

            var xs = from word in input.StartWith("")
                     from length in Observable.Return(word.Length).Delay(TimeSpan.FromSeconds(.5))
                     select length;

            var res = xs.CombineLatest(clock, (len, now) => now.ToString() + " - Word length = " + len);

            res.ObserveOnDispatcher().Subscribe(s =>
            {
                label1.Text = s.ToString();
            });
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}