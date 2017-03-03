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
    /// Interaction logic for Level1.xaml
    /// </summary>
    public partial class Level1 : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        List<Shape> mapBlocks = new List<Shape>();
        List<SkewTransform> Lights = new List<SkewTransform>();
        Random rand = new Random();
        
        Player player = new Player(new Thickness(32, 300, 0, 0), 32, 32);

        int aliveLight = 0;
        private bool IsGrounded = false;
        private bool IsPulsing;
        private bool IsRedClicked = false;
        private bool HasCollided = false;

        private bool IsPaused = false;

        private bool RedWin = false;
        private bool BlueWin = false;

        public double RotateTest { get; set; }


        public Level1()
        {
            InitializeComponent();
            RotateTest = 0;
            timer.Tick += new EventHandler(GameLoop);
            InitStuff();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Start();
        }

        public void InitStuff()
        {

            rctPlayer.DataContext = player;

            //collision list
            mapBlocks.Add(rctBottom);
            mapBlocks.Add(rctBottomStop);
            mapBlocks.Add(rctMid);
            mapBlocks.Add(rctMid_Copy);
            mapBlocks.Add(LeftWall);
            mapBlocks.Add(RightWall);
            mapBlocks.Add(Ceiling);
            mapBlocks.Add(rctRight);
            mapBlocks.Add(testi);

            //"lights"
            Lights.Add(rctSkew);
            Lights.Add(rctSkew1);
            Lights.Add(rctSkew2);


            jump.Stop();
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

        private void Player()
        {
            if (Keyboard.IsKeyDown(Key.D))
            {
                player.MovePlayer(0);
                if (CollisionTest(rctPlayer)) { LightTransform(Lights, 50, -50, true); }
                else { player.MovePlayer(1); }

            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                player.MovePlayer(1);
                if (CollisionTest(rctPlayer)) { LightTransform(Lights, 50, -50, false); }
                else { player.MovePlayer(0); }
            }

            // moves the red block
            if (Keyboard.IsKeyDown(Key.E))
            {
                if (HasCollided == true)
                {
                    player.MovePlayer(0);
                    IsRedClicked = true;
                    RedMove.Play();
                    if (CollisionTest(rctPlayer)) { IsRedClicked = true; }
                    else
                    {

                        player.MovePlayer(1);
                    }
                }
            }

            player.MovePlayer(2);
            if (CollisionTest(rctPlayer)) { }
            else { player.MovePlayer(3); }

        }

        private void RedBlock()
        {
            if (IsRedClicked == true)
            {
                mapBlocks.RemoveAt(8);
                if (CollisionTest(testi)) { testi.Margin = new Thickness(testi.Margin.Left + 2, testi.Margin.Top, 0, 0); }
                else
                {
                    IsRedClicked = false;
                }
                mapBlocks.Add(testi);
            }
        }



        private void GoalPulse()
        {
            CollisionDetect(rctPlayer, BlueGoal);
            CollisionDetect(testi, rctGoal);
            double pulse1 = 0.02;
            double pulse2 = 0.005;
            Random rand = new Random();
            pulse2 = rand.NextDouble() / 100;

            if (d.Offset < 0.95 && IsPulsing == true)
            {
                colorTest1.Offset += pulse1;
                plrColor.Offset += pulse1;
                enmColor.Offset += pulse1;

                d.Offset += pulse1;
            }
            else
            {
                IsPulsing = false;
                colorTest1.Offset -= pulse2;
                plrColor.Offset -= pulse2;
                enmColor.Offset -= pulse2;

                d.Offset -= pulse2;


                if (d.Offset <= 0.05)
                {
                    IsPulsing = true;
                }
            }

            if (RedWin == true)
            {
                if (rdL.Offset < 1)
                    rdL.Offset += pulse2;
            }
            else
            {
                if (rdL.Offset > 0.05)
                {
                    rdL.Offset -= pulse2;
                }
            }

            if (BlueWin == true)
            {
                if (blL.Offset < 1)
                    blL.Offset += pulse2;
            }
            else {
                if (blL.Offset > 0.05)
                {
                    blL.Offset -= pulse2;
                }
            }
        }




        private void GameLoop(object sender, EventArgs e)
        {

            if (IsGrounded == true) { Jumping(); }

            

            if (aliveLight < 10)
            {
                clrLive.Offset += 0.0025;
                aliveLight++;
            } else if (aliveLight < 20)
            {
                clrLive.Offset -= 0.0025;
                aliveLight++;
            } else { aliveLight = 0; }

            RedBlock();
            GoalPulse();


            TimeTest.Content = DateTime.Now.ToLongTimeString();
            Player();


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
                if (item.AngleX < maxAngle && item.AngleX > minAngle)
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
                    if (objB.Name == "testi")
                    {
                        HasCollided = true;

                    }

                    else if (playerBox.Name == "testi")
                    {

                        if (objB.Name == "rctGoal")

                            RedWin = true;
                    }
                    else if (playerBox.Name == "rctPlayer")
                    {


                        if (objB.Name == "BlueGoal")
                        { BlueWin = true; }


                    }
                    else { BlueWin = false; }
                    return false;
                }

                else
                {
                    HasCollided = false;
                    return true;
                }
            HasCollided = false;
            return true;

        }



        // rotates the canvas 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Q)
            {
                RotateTest += 45;
                cnvRotate.Angle = RotateTest;
            }
            if (e.Key == Key.Space)
            {
                IsGrounded = true;
            }


            if (e.Key == Key.Escape)
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



        private void Jumping()
        {

            IsGrounded = player.Jumping(0);
            if (player.jumpCounter == 1) { Collision.Stop(); jump.Play(); }

            if (CollisionTest(rctPlayer)) { }
            else { IsGrounded = player.Jumping(1); player.jumpCounter = 45; jump.Stop(); Collision.Play(); }

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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // restart testing

            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();

            PauseScreen.Visibility = Visibility.Hidden;
            pauseMusic.Stop();
            musicPlayer.Play();
            IsPaused = false;
            timer.Start();
        }
    }
}
