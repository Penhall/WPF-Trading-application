using Righthand.RealtimeGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CommonFrontEnd.View.DigitalClock
{
    /// <summary>
    /// Interaction logic for SensexChart.xaml
    /// </summary>
    public partial class SensexChart : Window
    {
        BindingList<RealtimeGraphItem> items = new BindingList<RealtimeGraphItem>();
        private DispatcherTimer timer;
        private DateTime start;
        private DateTime last;

        public SensexChart()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;

            float angle = 135;
            // bind items
            Graph.SeriesSource = items;

            timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5)
            };
            timer.Tick += timer_Tick;
            last = start = DateTime.Now;
            timer.IsEnabled = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // add new items each tick
            TimeSpan span = DateTime.Now - last;
            TimeSpan totalSpan = DateTime.Now - start;
            int previousTime = items.Count > 0 ? items[items.Count - 1].Time : 0;
            RealtimeGraphItem newItem = new RealtimeGraphItem
            {
                Time = (int)(previousTime + span.TotalMilliseconds),
                Value = Math.Sin(totalSpan.TotalSeconds / 10f) * 90 + 150
            };

            items.Add(newItem);
            last = DateTime.Now;
        }
        public class RealtimeGraphItem : IGraphItem
        {
            public int Time { get; set; }
            public double Value { get; set; }
        }

    }
}
