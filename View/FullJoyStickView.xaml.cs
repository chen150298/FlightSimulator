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
    /// Interaction logic for FullJoyStick.xaml
    /// </summary>
    public partial class FullJoyStickView : UserControl
    {
        FullJoyStickViewModel vm;
        public FullJoyStickView()
        {
            vm = new FullJoyStickViewModel(PlaneModel.GetModel());
            DataContext = vm;
            InitializeComponent();
            // notify on change
            vm.PropertyChanged += RudderChanged;
            vm.PropertyChanged += ThrottleChanged;
        }


        public void RudderChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Rudder")
            {
                this.Dispatcher.Invoke(() =>
                {
                    RudderSlider.Value = (sender as FullJoyStickViewModel).Rudder;
                });
            }
        }
        public void ThrottleChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Throttle")
            {
                this.Dispatcher.Invoke(() =>
                {
                    ThrottleSlider.Value = (sender as FullJoyStickViewModel).Throttle;
                });
            }
        }


    }
}
