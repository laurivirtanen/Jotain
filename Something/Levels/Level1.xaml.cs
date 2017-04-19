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
    public partial class Level1 : Window
    {

        int retry = 0;
        DispatcherTimer timer = new DispatcherTimer();
        List<Shape> mapBlocks = new List<Shape>();
        List<SkewTransform> Lights = new List<SkewTransform>();
        EndOfLevel gameWindow = new EndOfLevel();

        Player player = new Player(new Thickness(32, 300, 0, 0), 32, 32);


        private bool trgMove = false;
        private int TargetMove = 0;
        private bool IsGrounded = false;
        private bool IsPulsing;
        private bool IsMoving;
        private bool IsPaused = true;
        private bool RedWin = false;
        private bool BlueWin = false;

        public double RotateTest { get; set; }


        public Level1()
        {
            try
            {
                InitializeComponent();
                RotateTest = 0;
                timer.Tick += new EventHandler(GameLoop);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
                InitStuff();
               // timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      
        public void InitStuff()
        {
            rctPlayer.DataContext = player;

            RedWin = false;
            BlueWin = false;
            //collision list
            mapBlocks.Add(rctBottom);
            mapBlocks.Add(rctBottomStop);
            mapBlocks.Add(rctMid);
            mapBlocks.Add(rctMid_Copy);
            mapBlocks.Add(LeftWall);
            mapBlocks.Add(RightWall);
            mapBlocks.Add(Ceiling);
            mapBlocks.Add(rctRight);
            mapBlocks.Add(rctTarget);
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
                else { player.MovePlayer(1); player.IsGrounded = true; }

            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                player.MovePlayer(1);
                if (CollisionTest(rctPlayer)) { LightTransform(Lights, 50, -50, false); }
                else { player.MovePlayer(0); player.IsGrounded = true; }
            }

            // moves the red block
            player.MovePlayer(2);
            if (CollisionTest(rctPlayer)) { }
            else { player.MovePlayer(3); }

        }

        private void RedBlock()
        {
            if (IsMoving == false && TargetMove != 0)
                switch (TargetMove)
                {
                    case 1:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left + player.moving, rctTarget.Margin.Top, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left - player.moving, rctTarget.Margin.Top, 0, 0);
                            TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 2:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left - player.moving, rctTarget.Margin.Top, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left + player.moving, rctTarget.Margin.Top, 0, 0);
                            TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 3:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top + player.moving, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top - player.moving, 0, 0);
                            TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 4:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top - player.moving, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top + player.moving, 0, 0);
                            TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    default:

                        break;
                }
        }



        private void GoalPulse()
        {
            CollisionDetect(rctPlayer, BlueGoal);
            CollisionDetect(rctTarget, rctGoal);
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
            try {
                if(blL.Offset > 0.6 && rdL.Offset > 0.6)
                {
                    gameWindow.Show();
                    this.Close();
                }
                if (IsGrounded == true) { Jumping(); }
                RedBlock();
                GoalPulse();

                //TimeTest.Content = DateTime.Now.ToLongTimeString();
                TimeTest.Content = "player x" + rctPlayer.Margin.Left + " " + rctTarget.Margin.Left + "\n" + "player y" + rctPlayer.Margin.Top;
                Player();

                
                RedBlock();
                }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

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
                        item.AngleX += 0.015;
                    }
                    else if (side == false)
                    {
                        item.AngleX -= 0.015;
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
                    if (objB.Name == "rctTarget")
                    {
                        // punasen palikan liikuttelu 
                        if (trgMove == false) {
                            trgMove = true;
                            //vasemmalle
                            if ((playerBox_rect.X+playerBox_rect.Width-4) <= objB_rect.X)
                            {
                                TargetMove = 2;
                            }
                            //ylös
                            else if (playerBox_rect.Y+playerBox_rect.Height-4 <= objB_rect.Y)
                            {
                                TargetMove = 4;
                            }
                            //alas
                            else if (playerBox_rect.Y  >= objB_rect.Y+objB_rect.Height-4)
                            {
                                TargetMove = 3;
                            }
                            else //oikealle TODO paremmi9n
                            { TargetMove = 1; }
                        }
                        

                    }

                    else if (playerBox.Name == "rctTarget")
                    {

                        if (objB.Name == "rctGoal") { RedWin = true; }
                            
                        else if (objB.Name != "rctPlayer") { trgMove = false; }
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
                    return true;
                }
            return true;

        }



        // rotates the canvas 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:
                    RotateTest += 45;
                    cnvRotate.Angle = RotateTest;
                    break;

                case Key.Space:
                    IsGrounded = true;
                    if (IsPaused == true)
                    {
                        IsPaused = false;
                        PauseScreen.Visibility = Visibility.Hidden;
                        timer.Start();
                    }
                    break;
                case Key.Escape:
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
                    break;
                case Key.R:

                    if (IsPaused == true)
                    {
                        Level1 retry = new Level1();
                        retry.Show();
                        this.Close();
                        PauseScreen.Visibility = Visibility.Hidden;
                        pauseMusic.Stop();
                        musicPlayer.Play();
                        IsPaused = false;
                        timer.Start();
                    }
                    break;
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
            // restart rctTargetng
            Level1 retry = new Level1();
            retry.Show();
            Close();
            PauseScreen.Visibility = Visibility.Hidden;
            pauseMusic.Stop();
            musicPlayer.Play();
            IsPaused = false;
            timer.Start();
        }


    }
}
