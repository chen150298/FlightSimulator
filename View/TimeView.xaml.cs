using ap.Model;
using ap.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ap.View
{
    /// <summary>
    /// Interaction logic for TimeView.xaml
    /// </summary>
    public partial class TimeView : UserControl
    {
        private TimeViewModel vm;

        public TimeView()
        {
            this.vm = new TimeViewModel(PlaneModel.GetModel());
            InitializeComponent();           
            DataContext = vm;
            // notify on change
            vm.PropertyChanged += TimeChanged;
            vm.PropertyChanged += UpdateTotalTime;
        }

        //private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    // forward
        //    if (Slider.Value > Slider.Maximum)
        //    {
        //        if (vm.Indicator + 100 > vm.TotalTime)
        //        {
        //            vm.Indicator = vm.TotalTime;
        //        }
        //        else
        //        {
        //            vm.Indicator += 100;
        //        }
        //    }
        //    // backwards
        //    if (Slider.Value < Slider.Minimum)
        //    {
        //        if (vm.Indicator - 100 < 0)
        //        {
        //            vm.Indicator = 0;
        //        }
        //        else
        //        {
        //            vm.Indicator -= 100;
        //        }
        //    }

        //}

        public void TimeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Slider")
            {
                this.Dispatcher.Invoke(() =>
                {
                    Slider.Value = (sender as TimeViewModel).Indicator;
                });               
            }
        }

        public void UpdateTotalTime(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalTime")
            {
                Slider.Maximum = (sender as TimeViewModel).TotalTime;
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            vm.Stop();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            vm.Play();
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            vm.Pause();
        }

        private void Option0_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 0.25;
        }

        private void Option1_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 0.5;
        }

        private void Option2_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 0.75;
        }

        private void Option3_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 1;
        }

        private void Option4_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 1.25;
        }

        private void Option5_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 1.5;
        }

        private void Option6_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 1.75;
        }

        private void Option7_Selected(object sender, RoutedEventArgs e)
        {
            vm.Frequency = 2;
        }
    }
}
