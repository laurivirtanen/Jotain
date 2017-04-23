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
    public partial class MainWindow : Window, ISwitchable
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

            Splash splash = new Splash();
            splash.Show();
            Level1 gameWindow = new Level1();

            MainWindow main = new MainWindow();

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(i);
            }

            gameWindow.Show();
            new Level1();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(i);
            }

            splash.Close();
            

            this.Close();
        }
        

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
