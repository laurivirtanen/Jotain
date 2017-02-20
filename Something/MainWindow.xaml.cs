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

/// <summary>
/// Song from (https://soundcloud.com/laserost)  (http://www.youtube.com/user/Manofunctional).
/// </summary>


namespace Something
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        List<Shape> mapBlocks = new List<Shape>();
        List<SkewTransform> Lights = new List<SkewTransform>();
        





        private bool IsTest = false;
        private bool IsPaused = false;

        private double jumping = 0;

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
            

            Lights.Add(rctSkew);
            Lights.Add(rctSkew1);
            Lights.Add(rctSkew2);



            musicPlayer.Play();


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

            if (jumping >= 35)
            {
                jumping = 0;
                jumpy.Width = 2;
                jumpy.Height = 2;
                jumpy.Visibility = Visibility.Hidden;

            }

            jumping += 0.3;
            jumpy.Margin = new Thickness(jumpy.Margin.Left - 0.5, jumpy.Margin.Top+0.3, 0, 0);
            jumpy.Width += 1;
            jumpy.Height += 1;


            //testi.Margin = new Thickness(testi.Margin.Left, testi.Margin.Top+1, 0, 0);
            mapBlocks.RemoveAt(8);
            if (CollisionTest(testi)) { }
            else { testi.Margin = new Thickness(testi.Margin.Left, testi.Margin.Top -1, 0, 0); }
            mapBlocks.Add(testi);

            if (Keyboard.IsKeyDown(Key.D))
            {
                
                rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left+3, rctPlayer.Margin.Top, 0, 0);
                if (CollisionTest(rctPlayer)) { LightTransform(Lights, 50, -50,true); }
                else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left - 3, rctPlayer.Margin.Top, 0, 0);}
                
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                
                rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left-3, rctPlayer.Margin.Top, 0, 0);
                if (CollisionTest(rctPlayer)) { LightTransform(Lights, 50, -50, false); }
                else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left + 3, rctPlayer.Margin.Top, 0, 0); }


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
                rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left, rctPlayer.Margin.Top-8, 0, 0);
                if (CollisionTest(rctPlayer)) { }
                else { rctPlayer.Margin = new Thickness(rctPlayer.Margin.Left, rctPlayer.Margin.Top + 8, 0, 0); }

            }
            if (Keyboard.IsKeyUp(Key.Space))
            {
                //
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


       private void LightTransform(List<SkewTransform> angle, int maxAngle, int minAngle, bool side)
        {
            foreach (var item in angle)
            {
                if(item.AngleX < maxAngle && item.AngleX > minAngle)
                {
                    if (side)
                    { 
                        item.AngleX += 0.15;
                    }
                    else if (side == false)
                    {
                        item.AngleX -= 0.15;
                    }
                }
            }
        }


        private void DrawJump()
        {
            jumpy.Height = 2;
            jumpy.Width = 5;
            jumpy.Margin = new Thickness(rctPlayer.Margin.Left+15 , rctPlayer.Margin.Top + rctPlayer.Height - 10, 0, 0) ;
            
            

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
            if(e.Key == Key.Space)
            {
                
                jumpy.Visibility = Visibility.Visible;
                DrawJump();
            }

            if(e.Key == Key.Escape)
            {
                
                if (IsPaused == false)
                {
                    musicPlayer.Pause();
                    pauseMusic.Play();
                    PauseScreen.Visibility = Visibility.Visible;
                    IsPaused = true;
                    timer.Stop();
                }
                else
                {
                    IsPaused = false;
                }

            }
        }
        

        private void Quit_Game(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Continue(object sender, RoutedEventArgs e)
        {
            PauseScreen.Visibility = Visibility.Hidden;
            pauseMusic.Stop();
            musicPlayer.Play();
            IsPaused = false;
            timer.Start();
        }
    }
}
