using ap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.ViewModel
{
    class DataTableViewModel : INotifyPropertyChanged
    {

        private IPlaneModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public DataTableViewModel(IPlaneModel model)
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

        public double Altimeter
        {
            get
            {
                return model.Altimeter;
            }
        }
        public double AirSpeed
        {
            get
            {
                return model.AirSpeed;
            }
        }
        public double Heading
        {
            get
            {
                return model.Heading;
            }
        }
        public double Yaw
        {
            get
            {
                return model.Yaw;
            }
        }
        public double Pitch
        {
            get
            {
                return model.Pitch;
            }
        }
        public double Roll
        {
            get
            {
                return model.Roll;
            }
        }

    }
}
