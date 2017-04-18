using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Something
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        bool Lights;
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (btnPlaycolor.Offset < 0.365)
            {
                Lights = false;

            }else if(btnPlaycolor.Offset > 0.795)
            {
                Lights = true;
            }

            if(Lights == true)
            {
                btnPlaycolor.Offset -= 0.005;
                playRotate.Angle += rnd.NextDouble();
            }
            else
            {
                btnPlaycolor.Offset += 0.005;
                playRotate.Angle -= rnd.NextDouble();
            }
            

        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Level1 gameWindow = new Level1();
            gameWindow.Show();
            this.Close();
        }
    }
}
