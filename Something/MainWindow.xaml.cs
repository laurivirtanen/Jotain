using Something.Levels;
using System;
using System.Threading;
using System.Windows;
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

            Splash splash = new Splash();
            splash.Show();

            MainWindow main = new MainWindow();


            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(i);
            }
            splash.Close();

            gameWindow.Show();

            this.Close();
        }
    }
}
