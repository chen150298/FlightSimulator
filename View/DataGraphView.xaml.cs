using ap.Model;
using ap.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// Interaction logic for DataGraphView.xaml
    /// </summary>
    public partial class DataGraphView : UserControl
    {
        private const int RowsToDisplay = (1000 / 25) * 30;

        private DataGraphViewModel vm;
        private List<double[]> data;
        private Dictionary<int, CorrelateFeatur> Correlative;

        private double step;
        private const double margin = 10;
        private double xmin;
        private double xmax;
        private double ymin;
        private double ymax;
        private double xaxis;

        private int propertyToShow;

        public DataGraphView()
        {
            this.vm = new DataGraphViewModel(PlaneModel.GetModel());
            data = vm.Data;
            Correlative = vm.Correlative;
            InitializeComponent();
            propertyToShow = 0;
            vm.PropertyChanged += ShowDataGraph;
        }

        // ********* Data Graph ******** //

        private void DataGraph_Initialized(object sender, EventArgs e)
        {
            xmin = margin;
            xmax = DataGraph.Width - margin;
            ymin = margin;
            ymax = DataGraph.Height - margin;
            step = (DataGraph.Width - margin) / RowsToDisplay;
        }

        // no correlative

        private void DrawXAxis(double minVal, double maxVal)
        {
            // find xaxis
            if (minVal > maxVal)
            {
                double temp = minVal;
                minVal = maxVal;
                maxVal = temp;
            }

            if (minVal > 0) xaxis = ymin;
            else if (maxVal < 0) xaxis = ymax;
            else
            {
                double valRatio;
                if ((maxVal - minVal) == 0) valRatio = 0 - minVal;
                else valRatio = Math.Abs(minVal / (maxVal - minVal));

                xaxis = valRatio * (ymax - ymin);
            }

            // make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax - xaxis), new Point(DataGraph.Width, ymax - xaxis)));

            Path xaxis_path = new Path();
            // thickness
            xaxis_path.StrokeThickness = 1;
            // color
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            DataGraph.Children.Add(xaxis_path);
        }

        private void DrawYAyis()
        {
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, DataGraph.Height)));

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            DataGraph.Children.Add(yaxis_path);
        }

        private void Draw(double minVal, double maxVal)
        {
            PointCollection points = new PointCollection();

            int startIndex = Math.Max(vm.Indicator - RowsToDisplay, 0);

            // add points to graph - 30 recent points
            for (int i = startIndex; i < vm.Indicator; i += 1)
            {
                double curVal = data[i][propertyToShow];
                double valRatio;
                // if stays the same 
                if ((maxVal - minVal) == 0) valRatio = 0;
                else valRatio = (curVal - minVal) / (maxVal - minVal);
                double yval = valRatio * (ymax - ymin);
                points.Add(new Point((i - startIndex) * step + margin, ymax - yval));
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 2;
            polyline.Stroke = Brushes.Blue;
            polyline.Points = points;

            DataGraph.Children.Add(polyline);
        }

        private void FindMinMax(ref double minVal, ref double maxVal)
        {
            int startIndex = Math.Max(vm.Indicator - RowsToDisplay, 0);

            // find min and max val for 30 recent points
            minVal = data[startIndex][propertyToShow];
            maxVal = data[startIndex][propertyToShow];
            for (int i = startIndex + 1; i < vm.Indicator; i += 1)
            {
                double curVal = data[i][propertyToShow];
                if (curVal < minVal) minVal = curVal;
                if (curVal > maxVal) maxVal = curVal;
            }
        }

        // corellative

        private void DrawXAxiscorellative(double minVal, double maxVal)
        {
            int correlativeIndex = Correlative[propertyToShow].id;

            // find xaxis
            if (minVal > maxVal)
            {
                double temp = minVal;
                minVal = maxVal;
                maxVal = temp;
            }

            if (minVal > 0) xaxis = ymin;
            else if (maxVal < 0) xaxis = ymax;
            else
            {
                double valRatio;
                if ((maxVal - minVal) == 0) valRatio = 0 - minVal;
                else valRatio = Math.Abs(minVal / (maxVal - minVal));

                xaxis = valRatio * (ymax - ymin);
            }

            // make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax - xaxis), new Point(DataGraph.Width, ymax - xaxis)));

            Path xaxis_path = new Path();
            // thickness
            xaxis_path.StrokeThickness = 1;
            // color
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            CorrelativeGraph.Children.Add(xaxis_path);
        }

        private void DrawYAyiscorellative()
        {
            int correlativeIndex = Correlative[propertyToShow].id;

            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, DataGraph.Height)));

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            CorrelativeGraph.Children.Add(yaxis_path);
        }

        private void Drawcorellative(double minVal, double maxVal)
        {
            int correlativeIndex = Correlative[propertyToShow].id;

            PointCollection points = new PointCollection();

            int startIndex = Math.Max(vm.Indicator - RowsToDisplay, 0);

            // add points to graph - 30 recent points
            for (int i = startIndex; i < vm.Indicator; i += 1)
            {
                double curVal = data[i][correlativeIndex];
                double valRatio;
                // if stays the same 
                if ((maxVal - minVal) == 0) valRatio = 0;
                else valRatio = (curVal - minVal) / (maxVal - minVal);
                double yval = valRatio * (ymax - ymin);
                points.Add(new Point((i - startIndex) * step + margin, ymax - yval));
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 2;
            polyline.Stroke = Brushes.Blue;
            polyline.Points = points;

            CorrelativeGraph.Children.Add(polyline);
        }

        private void FindMinMaxcorellative(ref double minVal, ref double maxVal)
        {
            int correlativeIndex = Correlative[propertyToShow].id;

            int startIndex = Math.Max(vm.Indicator - RowsToDisplay, 0);

            // find min and max val for 30 recent points
            minVal = data[startIndex][correlativeIndex];
            maxVal = data[startIndex][correlativeIndex];
            for (int i = startIndex + 1; i < vm.Indicator; i += 1)
            {
                double curVal = data[i][correlativeIndex];
                if (curVal < minVal) minVal = curVal;
                if (curVal > maxVal) maxVal = curVal;
            }
        }

        // linear reg

        private void DrawLinearReg(double minVal, double maxVal)
        {
           
        }
        
        // delete

        //private void DrawXAxisTwoFeatures(double minVal, double maxVal)
        //{
        //    // find xaxis
        //    if (minVal > maxVal)
        //    {
        //        double temp = minVal;
        //        minVal = maxVal;
        //        maxVal = temp;
        //    }

        //    if (minVal > 0) xaxis = ymin;
        //    else if (maxVal < 0) xaxis = ymax;
        //    else
        //    {
        //        double valRatio;
        //        if ((maxVal - minVal) == 0) valRatio = 0 - minVal;
        //        else valRatio = Math.Abs(minVal / (maxVal - minVal));

        //        xaxis = valRatio * (ymax - ymin);
        //    }

        //    // make the X axis.
        //    GeometryGroup xaxis_geom = new GeometryGroup();
        //    xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax - xaxis), new Point(DataGraph.Width, ymax - xaxis)));

        //    Path xaxis_path = new Path();
        //    // thickness
        //    xaxis_path.StrokeThickness = 1;
        //    // color
        //    xaxis_path.Stroke = Brushes.Black;
        //    xaxis_path.Data = xaxis_geom;
        //    // add
        //    DataGraph.Children.Add(xaxis_path);

        //    Path xaxis_correlative_path = new Path();
        //    // thickness
        //    xaxis_correlative_path.StrokeThickness = 1;
        //    // color
        //    xaxis_correlative_path.Stroke = Brushes.Black;
        //    xaxis_correlative_path.Data = xaxis_geom; 
        //    // add
        //    CorrelativeGraph.Children.Add(xaxis_correlative_path);
        //}

        //private void DrawYAyisTwoFeatures()
        //{
        //    GeometryGroup yaxis_geom = new GeometryGroup();
        //    yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, DataGraph.Height)));

        //    Path yaxis_path = new Path();
        //    // thickness
        //    yaxis_path.StrokeThickness = 1;
        //    // color
        //    yaxis_path.Stroke = Brushes.Black;
        //    yaxis_path.Data = yaxis_geom;
        //    // add
        //    DataGraph.Children.Add(yaxis_path);

        //    Path yaxis_corellative_path = new Path();
        //    // thickness
        //    yaxis_corellative_path.StrokeThickness = 1;
        //    // color
        //    yaxis_corellative_path.Stroke = Brushes.Black;
        //    yaxis_corellative_path.Data = yaxis_geom;
        //    // add
        //    CorrelativeGraph.Children.Add(yaxis_corellative_path);
        //}

        //private void DrawTwoFeatures(double minVal, double maxVal)
        //{
        //    PointCollection points = new PointCollection();

        //    int startIndex = Math.Max(vm.Indicator - RowsToDisplay, 0);

        //    if (startIndex > 0)
        //    {
        //        Console.WriteLine("hi");
        //    }

        //    // add points to graph - 30 recent points
        //    for (int i = startIndex; i < vm.Indicator; i += 1)
        //    {
        //        double curVal = data[i][propertyToShow];
        //        double valRatio;
        //        // if stays the same 
        //        if ((maxVal - minVal) == 0) valRatio = 0;
        //        else valRatio = (curVal - minVal) / (maxVal - minVal);
        //        double yval = valRatio * (ymax - ymin);
        //        points.Add(new Point((i - startIndex) * step + margin, ymax - yval));
        //    }

        //    Polyline polyline = new Polyline();
        //    polyline.StrokeThickness = 2;
        //    polyline.Stroke = Brushes.Blue;
        //    polyline.Points = points;

        //    DataGraph.Children.Add(polyline);

        //    // corrlative graph

        //    PointCollection correlative_points = new PointCollection();
        //    int correlativeIndex = Correlative[propertyToShow].id;

        //    // add points to graph - 30 recent points
        //    for (int i = startIndex; i < vm.Indicator; i += 1)
        //    {
        //        double curVal = data[i][correlativeIndex];
        //        double valRatio;
        //        // if stays the same 
        //        if ((maxVal - minVal) == 0) valRatio = 0;
        //        else valRatio = (curVal - minVal) / (maxVal - minVal);
        //        double yval = valRatio * (ymax - ymin);
        //        correlative_points.Add(new Point((i - startIndex) * step + margin, ymax - yval));
        //    }

        //    Polyline corrlative_polyline = new Polyline();
        //    corrlative_polyline.StrokeThickness = 2;
        //    corrlative_polyline.Stroke = Brushes.Blue;
        //    corrlative_polyline.Points = correlative_points;

        //    CorrelativeGraph.Children.Add(corrlative_polyline);

        //}

        //private void FindMinMaxTwoFeatures(ref double minVal, ref double maxVal)
        //{
        //    int correlativeIndex = Correlative[propertyToShow].id;
        //    int startIndex = Math.Max(vm.Indicator - RowsToDisplay, 0);

        //    minVal = Math.Min(data[startIndex][propertyToShow], data[startIndex][correlativeIndex]);
        //    maxVal = Math.Max(data[startIndex][propertyToShow], data[startIndex][correlativeIndex]);
        //    // find min and max val for 30 recent points
        //    for (int i = startIndex + 1; i < vm.Indicator; i += 1)
        //    {
        //        double curVal1 = data[i][propertyToShow];
        //        double curVal2 = data[i][correlativeIndex];
        //        if (curVal1 < minVal) minVal = curVal1;
        //        if (curVal2 < minVal) minVal = curVal2;
        //        if (curVal1 > maxVal) maxVal = curVal1;
        //        if (curVal2 > maxVal) maxVal = curVal2;
        //    }
        //}


        // display

        public void ShowDataGraph(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Slider")
            {
                this.Dispatcher.Invoke(() =>
                {
                    DataGraph.Children.Clear();
                    CorrelativeGraph.Children.Clear();
                    LinearRegGraph.Children.Clear();

                    double minVal = 0, maxVal = 0;
                    if (!Correlative.ContainsKey(propertyToShow))
                    {
                        Text.Content = "No Correlative Features Found";
                        FindMinMax(ref minVal, ref maxVal);
                        // draw current
                        DrawYAyis();
                        DrawXAxis(minVal, maxVal);
                        Draw(minVal, maxVal);
                    }
                    else
                    {
                        Text.Content = Correlative[propertyToShow].name;
                        FindMinMax(ref minVal, ref maxVal);
                        // draw current
                        DrawYAyis();
                        DrawXAxis(minVal, maxVal);
                        Draw(minVal, maxVal);

                        // corelation
                        FindMinMaxcorellative(ref minVal, ref maxVal);
                        // draw current
                        DrawYAyiscorellative();
                        DrawXAxiscorellative(minVal, maxVal);
                        Drawcorellative(minVal, maxVal);
                        DrawLinearReg(minVal, maxVal);
                    }
                   
                });

            }
        }

        // ****** List ********* // 

        private void List_Initialized(object sender, EventArgs e)
        {
            List.Items.Add(new Property { Id = 0, Name = "aileron" });
            List.Items.Add(new Property { Id = 1, Name = "elevator" });
            List.Items.Add(new Property { Id = 2, Name = "rudder" });
            List.Items.Add(new Property { Id = 3, Name = "flaps" });
            List.Items.Add(new Property { Id = 4, Name = "slats" });
            List.Items.Add(new Property { Id = 5, Name = "speedbrake" });
            List.Items.Add(new Property { Id = 6, Name = "throttle" });
            List.Items.Add(new Property { Id = 7, Name = "throttle" });
            List.Items.Add(new Property { Id = 8, Name = "engine-pump" });
            List.Items.Add(new Property { Id = 9, Name = "engine-pump" });
            List.Items.Add(new Property { Id = 10, Name = "electric-pump" });
            List.Items.Add(new Property { Id = 11, Name = "electric-pump" });
            List.Items.Add(new Property { Id = 12, Name = "external-power" });
            List.Items.Add(new Property { Id = 13, Name = "APU-generator" });
            List.Items.Add(new Property { Id = 14, Name = "latitude-deg" });
            List.Items.Add(new Property { Id = 15, Name = "longitude-deg" });
            List.Items.Add(new Property { Id = 16, Name = "altitude-ft" });
            List.Items.Add(new Property { Id = 17, Name = "roll-deg" });
            List.Items.Add(new Property { Id = 18, Name = "pitch-deg" });
            List.Items.Add(new Property { Id = 19, Name = "heading-deg" });
            List.Items.Add(new Property { Id = 20, Name = "side-slip-deg" });
            List.Items.Add(new Property { Id = 21, Name = "airspeed-kt" });
            List.Items.Add(new Property { Id = 22, Name = "glideslope" });
            List.Items.Add(new Property { Id = 23, Name = "vertical-speed-fps" });
            List.Items.Add(new Property { Id = 24, Name = "airspeed-indicator_indicated-speed-kt" });
            List.Items.Add(new Property { Id = 25, Name = "altimeter_indicated-altitude-ft" });
            List.Items.Add(new Property { Id = 26, Name = "altimeter_pressure-alt-ft" });
            List.Items.Add(new Property { Id = 27, Name = "attitude-indicator_indicated-pitch-deg" });
            List.Items.Add(new Property { Id = 28, Name = "attitude-indicator_indicated-roll-deg" });
            List.Items.Add(new Property { Id = 29, Name = "attitude-indicator_internal-pitch-deg" });
            List.Items.Add(new Property { Id = 30, Name = "attitude-indicator_internal-roll-deg" });
            List.Items.Add(new Property { Id = 31, Name = "encoder_indicated-altitude-ft" });
            List.Items.Add(new Property { Id = 32, Name = "encoder_pressure-alt-ft" });
            List.Items.Add(new Property { Id = 33, Name = "gps_indicated-altitude-ft" });
            List.Items.Add(new Property { Id = 34, Name = "gps_indicated-ground-speed-kt" });
            List.Items.Add(new Property { Id = 35, Name = "gps_indicated-vertical-speed" });
            List.Items.Add(new Property { Id = 36, Name = "indicated-heading-deg" });
            List.Items.Add(new Property { Id = 37, Name = "magnetic-compass_indicated-heading-deg" });
            List.Items.Add(new Property { Id = 38, Name = "slip-skid-ball_indicated-slip-skid" });
            List.Items.Add(new Property { Id = 39, Name = "turn-indicator_indicated-turn-rate" });
            List.Items.Add(new Property { Id = 40, Name = "vertical-speed-indicator_indicated-speed-fpm" });
            List.Items.Add(new Property { Id = 41, Name = "engine_rpm" });
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List.SelectedItem == null) return;

            propertyToShow = (List.SelectedItem as Property).Id;
        }
    }
}
