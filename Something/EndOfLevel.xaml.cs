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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Something
{
    /// <summary>
    /// Interaction logic for EndOfLevel.xaml
    /// </summary>
    public partial class EndOfLevel : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        Player player = new Player(new Thickness(32, 300, 0, 0), 32, 32);
        public EndOfLevel()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

        }

        private void Quit_Game(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void Continue(object sender, RoutedEventArgs e)
        {
            //PauseScreen.Visibility = Visibility.Hidden;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {// restart rctTargetng

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
