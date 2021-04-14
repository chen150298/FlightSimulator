using ap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.ViewModel
{
    public class DataGraphViewModel : INotifyPropertyChanged
    {
        private IPlaneModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public DataGraphViewModel(IPlaneModel model)
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

        public int Indicator
        {
            get
            {
                return model.Indicator;
            }
        }

        public List<double[]> Data
        {
            get
            {
                return model.GetData();
            }
        }

        public Dictionary<int, CorrelateFeatur> Correlative
        {
            get
            {
                return model.GetCorrelative();
            }
        }
    }

}
