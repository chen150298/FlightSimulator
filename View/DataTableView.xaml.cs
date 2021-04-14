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
    /// Interaction logic for DataTableView.xaml
    /// </summary>
    public partial class DataTableView : UserControl
    {
        DataTableViewModel vm;

        public DataTableView()
        {
            this.vm = new DataTableViewModel(PlaneModel.GetModel());
            InitializeComponent();
            // notify on change
            vm.PropertyChanged += AltimeterChanged;
            vm.PropertyChanged += AirSpeedChanged;
            vm.PropertyChanged += HeadingChanged;
            vm.PropertyChanged += YawChanged;
            vm.PropertyChanged += PitchChanged;
            vm.PropertyChanged += RollChanged;
        }

        public void AltimeterChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Altimeter")
            {
                this.Dispatcher.Invoke(() =>
                {
                    AltimeterValue.Content = (sender as DataTableViewModel).Altimeter;
                });
            }
        }

        public void AirSpeedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AirSpeed")
            {
                this.Dispatcher.Invoke(() =>
                {
                    AirSpeedValue.Content = (sender as DataTableViewModel).AirSpeed;
                });
            }
        }

        public void HeadingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Heading")
            {
                this.Dispatcher.Invoke(() =>
                {
                    HeadingValue.Content = (sender as DataTableViewModel).Heading;
                });
            }
        }

        public void YawChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Yaw")
            {
                this.Dispatcher.Invoke(() =>
                {
                    YawValue.Content = (sender as DataTableViewModel).Yaw;
                });
            }
        }

        public void PitchChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Pitch")
            {
                this.Dispatcher.Invoke(() =>
                {
                    PitchValue.Content = (sender as DataTableViewModel).Pitch;
                });
            }
        }

        public void RollChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Roll")
            {
                this.Dispatcher.Invoke(() =>
                {
                    RollValue.Content = (sender as DataTableViewModel).Roll;
                });
            }
        }
    }
}
