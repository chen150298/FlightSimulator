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
    /// Interaction logic for JoyStickView.xaml
    /// </summary>
    public partial class JoyStickView : UserControl
    {
        private JoyStickViewModel vm;
        public JoyStickView()
        {
            this.vm = new JoyStickViewModel(PlaneModel.GetModel());
            InitializeComponent();
            vm.PropertyChanged += AileronChanged;
            vm.PropertyChanged += ElevatorChanged;
        }

        public void AileronChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Aileron")
            {
                this.Dispatcher.Invoke(() =>
                {
                    // MiddleValue + (MaxValue - MinValue)*Aileron
                    Canvas.SetLeft(Knob, 125 + (100 * vm.Aileron));
                });
            }
        }

        public void ElevatorChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Elevator")
            {
                this.Dispatcher.Invoke(() =>
                {
                    // MiddleValue + (MaxValue - MinValue)*Aileron
                    Canvas.SetTop(Knob, 125 + (100 * vm.Elevator));
                });
            }
        }
    }
}

