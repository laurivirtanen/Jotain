using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Something;

namespace Something
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        List<Shape> mapBlocks = new List<Shape>();
        private bool IsTest = false;
        public double RotateTest { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            RotateTest = 0;
            timer.Tick += new EventHandler(MovePlayer);
            InitStuff();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.IsEnabled = true;
            timer.Start();
        }

        public void InitStuff()
        {
            Player player = new Player();
            player.positionX = rctPlayer.Margin.Left;
            player.positionY = rctPlayer.Margin.Top;
            mapBlocks.Add(rctBottom);
            mapBlocks.Add(rctBottomStop);
            mapBlocks.Add(rctMid);
            mapBlocks.Add(rctMid_Copy);
            mapBlocks.Add(LeftWall);
            mapBlocks.Add(RightWall);
            mapBlocks.Add(Ceiling);
            mapBlocks.Add(rctRight);
            mapBlocks.Add(testi);


        }
        /*
        ¨*
        public static DependencyObject FindVisualTreeDown(DependencyObject obj, Type type)
        {
            if(obj != null)
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                    if (child.GetType() == type)
                    {
                        return child;
                    }

                    DependencyObject childReturn = FindVisualTreeDown(child, type);
                    if(childReturn != null)
                    {
                        return childReturn;
                    }
                }
            return null;
        }*/

        private void MovePlayer(object sender, EventArgs e)
        {

            TimeTest.Content = DateTime.Now.ToLongTimeString();
            //testi.Margin = new Thickness(testi.Margin.Left, testi.Margin.Top+1, 0, 0);
            mapBlocks.RemoveAt(8);
            if (CollisionTest(testi)) { }
            else { testi.Margin = new Thickness(testi.Margin.Left, testi.Margin.Top -1, 0, 0); }
            mapBlocks.Add(testi);

            if (Keyboard.IsKeyDown(Key.D))
            {
                
                rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left+3, rctPlayer.Margin.Top, 0, 0);
                if (CollisionTest(rctPlayer)) { Console.WriteLine(IsTest); rctSkew.AngleX += 0.15; rctSkew2.AngleX += 0.15; } // shadow skew movement test
                else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left - 3, rctPlayer.Margin.Top, 0, 0);}
                
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                
                rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left-3, rctPlayer.Margin.Top, 0, 0);
                if (CollisionTest(rctPlayer)) { Console.WriteLine(IsTest  ); rctSkew.AngleX -= 0.15; rctSkew2.AngleX -= 0.15; }// shadow skew movement test
                else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left +3, rctPlayer.Margin.Top, 0, 0);  }


            }


            //moves the red block
            if (Keyboard.IsKeyDown(Key.E))
            {
                if (IsTest == true)
                {
                    rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left + 5, rctPlayer.Margin.Top, 0, 0);
                    testi.Margin = new Thickness(testi.Margin.Left + 355, testi.Margin.Top, 0, 0);
                    if (CollisionTest(rctPlayer)) { Console.WriteLine("sup"); }
                    else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left -5, rctPlayer.Margin.Top, 0, 0); }
                }
            }
            
            if (Keyboard.IsKeyDown(Key.Space))
            {
                rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left, rctPlayer.Margin.Top-3, 0, 0);
                if (CollisionTest(rctPlayer)) { }
                else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left, rctPlayer.Margin.Top + 3, 0, 0); }

            }
            if (Keyboard.IsKeyUp(Key.Space))
            {
                rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left, rctPlayer.Margin.Top + 3, 0, 0);
                if (CollisionTest(rctPlayer)) {  }
                else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left, rctPlayer.Margin.Top -3, 0, 0); }
            }



        }

        private bool CollisionTest(Shape player)
        {
            bool dum = true;

            foreach (var item in mapBlocks)
            {
               dum = CollisionDetect(player, item);
                if (dum == false) { return dum; }

            }
            return dum;
        }

        private bool CollisionDetect(Shape playerBox, Shape objB)
        {

            Rect playerBox_rect = new Rect();
            Rect objB_rect = new Rect();

            playerBox_rect.X = playerBox.Margin.Left;
            playerBox_rect.Y = playerBox.Margin.Top;
            playerBox_rect.Width = playerBox.ActualWidth;
            playerBox_rect.Height = playerBox.ActualHeight;


            objB_rect.X = objB.Margin.Left;
            objB_rect.Y = objB.Margin.Top;

            objB_rect.Width = objB.ActualWidth;
            objB_rect.Height = objB.ActualHeight;

            if ((objB_rect.X < (playerBox_rect.X + playerBox_rect.Width) &&
               (objB_rect.X + objB_rect.Width) > playerBox_rect.X))
               if (
                (objB_rect.Y < (playerBox_rect.Y + playerBox_rect.Height)) &&
               (objB_rect.Y + objB_rect.Height) > playerBox_rect.Y)
                {
                    if(objB.Name == "testi")
                    {
                        IsTest = true;
                    }
                    return false;
                }

                else
                {
                    IsTest = false;
                    return true;
                }
            IsTest = false;
            return true;

        }

        // rotates the canvas 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Q)
            {
                RotateTest += 45;
                cnvRotate.Angle = RotateTest;
            }
        }
    }
}
