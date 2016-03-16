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
using WindowsColorizerGui.Utils;

namespace WindowsColorizerGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush backBrush = new SolidColorBrush();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Background = backBrush;
        }
        private string rgbInfo;
        public string RGBInfo
        {
            get { return rgbInfo; }
            set
            {
                if (value != rgbInfo)
                {
                    rgbInfo = value;
                    RaisePropertyChanged("RGBInfo");
                }
            }
        }

        private void win1_MouseMove_1(object sender, MouseEventArgs e)
        {
            double radius = (canv1.ActualWidth / 2);

            Color c = ColorFromMousePosition(e.GetPosition(canv1), radius);
            WindowsColors.SetTitleBarColor(c);
            WindowsColors.SetExplorerColors(c);
            backBrush.Color = c;
            RGBInfo = string.Format("R={0}, G={1}, B={2}", c.R, c.G, c.B);
        }
        Color ColorFromMousePosition(Point mousePos, double radius)
        {
            // Position relative to center of canvas
            double xRel = mousePos.X - radius;
            double yRel = mousePos.Y - radius;

            // Hue is angle in deg, 0-360
            double angleRadians = Math.Atan2(yRel, xRel);
            double hue = angleRadians * (180 / Math.PI);
            if (hue < 0)
                hue = 360 + hue;

            // Saturation is distance from center
            double saturation = Math.Min(Math.Sqrt(xRel * xRel + yRel * yRel) / radius, 1.0);

            byte r, g, b;
            ColorUtil.HsvToRgb(hue, saturation, 1.0, out r, out g, out b);
            return Color.FromRgb(r, g, b);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
