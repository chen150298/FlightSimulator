using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ap.Model
{
    public class CorrelateFeatur
    {
        public string name { set; get; }
        public int id { set; get; }
        public double pearson { set; get; }
        public double[] linear_reg { set; get; }
    }

    public class PlaneModel
    {
        static IPlaneModel myPlaneModel;

        private PlaneModel() { }

        public static IPlaneModel GetModel()
        {
            if (myPlaneModel == null)
            {
                myPlaneModel = new MyPlaneModel();
            }
            return myPlaneModel;
        }

        public class MyPlaneModel : IPlaneModel
        {

            //******* members ****** //

            enum Name { aileron = 0, elevator = 1, rudder = 2, throttle = 6, altimeter = 16, roll = 17, pitch = 18, heading = 19, yaw = 20, airspeed = 21 }

            private List<double[]> Data;
            private string[] Names = { "aileron", "elevator", "rudder", "flaps" , "slats", "speedbrake", "throttle", "throttle",
            "engine-pump", "engine-pump", "electric-pump", "electric-pump", "external-power", "APU-generator" , "latitude-deg",
            "longitude-deg", "altitude-ft", "roll-deg", "pitch-deg", "heading-deg" , "side-slip-deg", "airspeed-kt", "glide-slope",
            "vertical-speed-fps", "airspeed-indicator_indicated-speed-Kt", "altimeter-_indicated-altitude-ft", "altimeter_pressure-alt-ft",
            "attitude-indicator_indicated-pitch-deg", "attitude-indicator_indicated-roll-deg", "attitude-indicator_internal-pitch-deg",
            "attitude-indicator_internal-roll-deg", "encoder_indicated-altitude-ft", "encoder_pressure-alt-ft", "gps_indicated-altitude-ft",
            "gps_indicated-ground-speed-kt", "gps_indicated-vertical-speed", "indicated-heading-deg", "magnetic-compass_indicated-heading-deg",
            "slip-skid-ball_indicated-slip-skid", "turn-indicator_indicated-turn-rate", "vertical-speed-indicator_indicated-speed-fpm" , "engine_rpm" };
            private int DataRowsNumber;
            private int DataColumnsNumber;

            // connection members
            private IClient client;
            private Mutex mutex = new Mutex();
            public event PropertyChangedEventHandler PropertyChanged;
            string file_name;

            // slider members
            private int indicator;
            public int Indicator
            {
                get
                {
                    return this.indicator;
                }
                set
                {
                    this.indicator = value;
                    this.NotifyPropertyChanged("Slider");
                }
            }
            private int totaltime;
            public int TotalTime
            {
                get
                {
                    return this.totaltime;
                }
                set
                {
                    this.totaltime = value;
                    this.NotifyPropertyChanged("TotalTime");
                }
            }
            private bool stop;
            private double frequency;
            public double Frequency
            {
                get
                {
                    return this.frequency;
                }
                set
                {
                    this.frequency = value;
                    this.NotifyPropertyChanged("Frequency");
                }
            }

            // joystick members
            private double aileron;
            public double Aileron
            {
                get
                {
                    return this.aileron;
                }
                set
                {
                    this.aileron = value;
                    this.NotifyPropertyChanged("Aileron");
                }
            }
            private double elevator;
            public double Elevator
            {
                get
                {
                    return this.elevator;
                }
                set
                {
                    this.elevator = value;
                    this.NotifyPropertyChanged("Elevator");
                }
            }
            private double rudder;
            public double Rudder
            {
                get
                {
                    return this.rudder;
                }
                set
                {
                    this.rudder = value;
                    this.NotifyPropertyChanged("Rudder");
                }
            }
            private double throttle;
            public double Throttle
            {
                get
                {
                    return this.throttle;
                }
                set
                {
                    this.throttle = value;
                    this.NotifyPropertyChanged("Throttle");
                }
            }

            // data table members
            private double altimeter;
            public double Altimeter
            {
                get
                {
                    return this.altimeter;
                }
                set
                {
                    this.altimeter = value;
                    this.NotifyPropertyChanged("Altimeter");
                }
            }
            private double airSpeed;
            public double AirSpeed
            {
                get
                {
                    return this.airSpeed;
                }
                set
                {
                    this.airSpeed = value;
                    this.NotifyPropertyChanged("AirSpeed");
                }
            }
            private double heading;
            public double Heading
            {
                get { return this.heading; }
                set
                {
                    this.heading = value;
                    this.NotifyPropertyChanged("Heading");
                }
            }
            private double yaw;
            public double Yaw
            {
                get { return this.yaw; }
                set
                {
                    this.yaw = value;
                    this.NotifyPropertyChanged("Yaw");
                }
            }
            private double pitch;
            public double Pitch
            {
                get { return this.pitch; }
                set
                {
                    this.pitch = value;
                    this.NotifyPropertyChanged("Pitch");
                }
            }
            private double roll;
            public double Roll
            {
                get { return this.roll; }
                set
                {
                    this.roll = value;
                    this.NotifyPropertyChanged("Roll");
                }
            }

            // graph
            Dictionary<int, CorrelateFeatur> correlative; // map id to a correlated id
            private const double THRESHOLD = 0.9;

            public MyPlaneModel()
            {
                this.client = ClientSingelton.GetClient();
                stop = false;
                Data = new List<double[]>();
                DataColumnsNumber = 0;
                DataRowsNumber = 0;
                indicator = 0;
                totaltime = 0;
                frequency = 1;
                aileron = 0;
                elevator = 0;
                rudder = 0;
                throttle = 0;
                altimeter = 0;
                airSpeed = 0;
                heading = 0;
                pitch = 0;
                roll = 0;
                yaw = 0;
                correlative = new Dictionary<int, CorrelateFeatur>();
            }

            //******* connect functions ****** //

            public async Task<bool> Connect(string ip, int port)
            {
                return await client.Connect(ip, port);
            }

            private bool CheckConnection()
            {
                return client.CheckConnection();
            }

            public void Disconnect()
            {
                client.Disconnect();
                stop = true;
            }

            public void SetFile(string file_name)
            {
                this.file_name = file_name;
                LoadCSVFile();
                // update total time
                TotalTime = DataRowsNumber;
                FindCorallatedFeatures();
            }

            // the actual program
            private void Start()
            {               

                new Thread(delegate ()
                {
                    mutex.WaitOne();

                    while (Indicator < TotalTime && !stop && CheckConnection())
                    {
                        int i = Indicator;

                        // aileron changed
                        Aileron = Data[i][(int)Name.aileron];
                        // elevator changed
                        Elevator = Data[i][(int)Name.elevator];
                        // Rudder changed
                        Rudder = Data[i][(int)Name.rudder];
                        // Throttle changed
                        Throttle = Data[i][(int)Name.throttle];
                        // Altimeter changed
                        Altimeter = Data[i][(int)Name.altimeter];
                        // AirSpeed changed
                        AirSpeed = Data[i][(int)Name.airspeed];
                        // Heading changed
                        Heading = Data[i][(int)Name.heading];
                        // Yaw Changed
                        Yaw = Data[i][(int)Name.yaw];
                        // Pitch changed
                        Pitch = Data[i][(int)Name.pitch];
                        // Roll changed
                        Roll = Data[i][(int)Name.roll];

                        // set send
                        string send = "";
                        for (int j = 0; j < DataColumnsNumber - 1; j++)
                        {
                            send += Data[i][j] + ",";
                        }
                        send += Data[i][DataColumnsNumber - 1];

                        Indicator++;

                        // send
                        client.Write(send);
                        Thread.Sleep((int)(25 / Frequency));
                    }

                    mutex.ReleaseMutex();

                }).Start();
            }

            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                }
            }

            //help functions

            private void LoadCSVFile()
            {
                try
                {
                    string line;
                    StreamReader streamReader = new StreamReader(file_name);
                    
                    string[] DataAsString = null;
                    int j;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        j = 0;
                        DataAsString = line.Split(',');
                        double[] DataAsDouble = new double[DataAsString.Length];
                        foreach (string s in DataAsString)
                        {
                            DataAsDouble[j] = double.Parse(s);
                            j++;
                        }
                        Data.Add(DataAsDouble);
                        DataRowsNumber++;
                    }
                    DataColumnsNumber = DataAsString.Length;
                    streamReader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("failed to load file: " + e.ToString());
                }

            }

            // ******* Time ********* //

            public void StopVideo()
            {
                stop = true;
                Indicator = 0;
            }

            public void PlayVideo()
            {
                stop = false;
                Start();
            }

            public void PauseVideo()
            {
                stop = true;
            }

            public void SetSlider(int value)
            {
                if(value != Indicator)
                {
                    Indicator = value;
                }               
            }

            // ******** DataGraph ********* // 

            public List<double[]> GetData()
            {
                return this.Data;
            }

            private double Var(double[] x)
            {
                double element1 = 0;
                double element2 = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    element1 += Math.Pow(x[i], 2);
                }
                element1 /= x.Length;
                element2 = Math.Pow(Avg(x), 2);
                return element1 - element2;
            }

            private double Avg(double[] x)
            {
                double sum = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    sum += x[i];
                }
                return sum / x.Length;
            }

            private double Cov(double[] x, double[] y)
            {
                double avgX = Avg(x);
                double avgY = Avg(y);
                double multXY = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    multXY += x[i] * y[i];
                }
                multXY /= x.Length;
                return (multXY - (avgX * avgY));
            }

            private double Pearson(double[] x, double[] y)
            {
                double covXY = Cov(x, y);
                double deviationX = Math.Sqrt(Var(x));
                double deviationY = Math.Sqrt(Var(y));
                if (deviationX == 0 || deviationY == 0) return 0;
                return (covXY / (deviationX * deviationY));
            }

            private double[] LinearReg(double[] x, double[] y)
            {
                double a = Cov(x, y) / Var(x);
                double b = Avg(y) - a * Avg(x);

                double[] regression = new double[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    regression[i] = a * x[i] + b;
                }
                return regression;
            }

            private void FindCorallatedFeatures()
            {
                double max, temp;
                string name = "";
                int id1 = 0, id2 = 0;
                double[] saveX = null, saveY = null;
                for (int i = 0; i < DataColumnsNumber; i++)
                {
                    max = 0;
                    for (int j = 0; j < DataColumnsNumber; j++)
                    {
                        if (i == j) continue;
                        double[] x = GetCulomn(Data, i);
                        double[] y = GetCulomn(Data, j);
                        temp = Pearson(x, y);
                        if (max < temp)
                        {
                            max = temp;
                            id1 = i;
                            id2 = j;
                            name = Names[j];
                            saveX = x;
                            saveY = y;
                        }
                    }
                    if (max >= THRESHOLD) correlative[id1] = new CorrelateFeatur { name = name, id = id2, pearson = max, linear_reg = LinearReg(saveX,saveY) };
                }
            }

            public Dictionary<int, CorrelateFeatur> GetCorrelative()
            {
                return this.correlative;
            }

            private double[] GetCulomn(List<double[]> a, int i)
            {
                double[] culomn = new double[a.Count];
                for (int j = 0; j < a.Count; j++)
                {
                    culomn[j] = a[j][i];
                }
                return culomn;
            }



        }

    }
}
