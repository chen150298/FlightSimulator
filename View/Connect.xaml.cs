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
using System.Windows.Shapes;

namespace ap.View
{
    /// <summary>
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class Connect : Window
    {
        private PlaneViewModel vm;

        public Connect(PlaneViewModel vm)
        {
            InitializeComponent();
            this.vm = vm; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IClient client = new Client(ip.Text, int.Parse(port.Text));
            client.Connect();
        }
    }
}
