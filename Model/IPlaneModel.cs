using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.Model
{
    public interface IPlaneModel : INotifyPropertyChanged
    {
        // connection to the plane
        Task<bool> Connect(string ip, int port);
        void Disconnect();
        void SetFile(string file_name);
        
        // slide bar
        int Indicator { set; get; }
        int TotalTime { set; get; }
        double Frequency { set; get; }
        void StopVideo();
        void PlayVideo();
        void PauseVideo();
        void SetSlider(int value);

        // joystick
        double Aileron { set; get; }
        double Elevator { set; get; }
        double Rudder { set; get; }
        double Throttle { set; get; }

        // data table
        double Altimeter { set; get; }
        double AirSpeed { set; get; }
        double Heading { set; get; }
        double Yaw { set; get; }
        double Pitch { set; get; }
        double Roll { set; get; }

        // DataGraph
        List<double[]> GetData();
        Dictionary<int, CorrelateFeatur> GetCorrelative();

    }
}
