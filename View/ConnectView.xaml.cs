using ap.Model;
using ap.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ConnectView.xaml
    /// </summary>
    public partial class ConnectView : UserControl
    {
        private ConnectViewModel vm;
        private bool is_connected;

        public ConnectView()
        {
            InitializeComponent();
            this.vm = new ConnectViewModel(PlaneModel.GetModel());
            is_connected = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (is_connected == false)
            {
                connection.Content = "Disconnect";
                vm.Connect(ip.Text, int.Parse(port.Text));
                vm.SetFile(path.Text);
                is_connected = true;
            }
            else
            {
                connection.Content = "Connect";
                vm.Disconnect();
                is_connected = false;
            }
        }
    }
}
