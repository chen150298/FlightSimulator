using ap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.ViewModel
{
    public class ConnectViewModel : INotifyPropertyChanged
    {
        private IPlaneModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectViewModel(IPlaneModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged(e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void Connect(string ip, int port)
        {
            model.Connect(ip, port);

        }

        public void Disconnect()
        {
            model.Disconnect();
        }

        public void SetFile(string file_name)
        {
            model.SetFile(file_name);
        }

    }
}
