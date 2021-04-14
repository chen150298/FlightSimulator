using ap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.ViewModel
{
    public class TimeViewModel : INotifyPropertyChanged
    {
        private IPlaneModel model;
        private int slider_bar;
        public event PropertyChangedEventHandler PropertyChanged;

        public TimeViewModel(IPlaneModel model)
        {
            this.model = model;
            slider_bar = 0;
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

        public int Indicator
        {
            get
            {
                return model.Indicator;
            }
        }

        public int TotalTime
        {
            get
            {
                return model.TotalTime;
            }
        }

        public void Stop()
        {
            model.StopVideo();
        }

        public void Play()
        {
            model.PlayVideo();
        }

        public void Pause()
        {
            model.PauseVideo();
        }

        public double Frequency
        {
            get
            {
                return model.Frequency;
            }
            set
            {
                model.Frequency = value;
            }
        }

        public int SliderBar
        {
            get
            {
                return this.slider_bar;
            }
            set
            {
                this.slider_bar = value;
                model.SetSlider(value);
            }
        }
        
    }
}