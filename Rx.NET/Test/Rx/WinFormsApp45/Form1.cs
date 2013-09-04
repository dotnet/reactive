using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PortableLibraryProfile7;

namespace WinFormsApp45
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var clock = MyExtensions.GetClock();

            var input = Observable.FromEventPattern(textBox1, "TextChanged").Select(evt => ((TextBox)evt.Sender).Text).Throttle(TimeSpan.FromSeconds(.5)).DistinctUntilChanged();

            var xs = from word in input.StartWith("")
                     from length in Task.Run(async () => { await Task.Delay(250); return word.Length; })
                     select length;

            var res = xs.CombineLatest(clock, (len, now) => now.ToString() + " - Word length = " + len);

            res.ObserveOn(this).Subscribe(s =>
            {
                label1.Text = s.ToString();
            });
        }
    }
}
