using Something.Classes;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Something.Levels
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public BasicBlock LoadingScreen = new BasicBlock();

        public Splash()
        {
            InitializeComponent();
            LoadingScreen.Amount = 0;
            Sup.DataContext = LoadingScreen;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
