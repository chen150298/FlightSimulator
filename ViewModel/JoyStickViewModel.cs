using ap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.ViewModel
{
    public class JoyStickViewModel : INotifyPropertyChanged
    {
        private IPlaneModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public JoyStickViewModel(IPlaneModel model)
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

        public double Aileron
        {
            get
            {
                return model.Aileron;
            }
        }

        public double Elevator
        {
            get
            {
                return model.Elevator;
            }
        }


    }
}
