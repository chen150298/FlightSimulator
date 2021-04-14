using ap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.ViewModel
{
    class FullJoyStickViewModel : INotifyPropertyChanged
    {
        private IPlaneModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public FullJoyStickViewModel(IPlaneModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (object sendr, PropertyChangedEventArgs e)
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
        //rudder
        public double Rudder
        {
            get
            {
                return model.Rudder;
            }
        }
        //thorttle
        public double Throttle
        {
            get
            {
                return model.Throttle;
            }
        }

    }
}
