using Something.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
/// <summary>
/// Song from (https://soundcloud.com/laserost)  (http://www.youtube.com/user/Manofunctional).
/// </summary>


namespace Something
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Level : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        List<Shape> mapBlocks = new List<Shape>();
        List<SkewTransform> Lights = new List<SkewTransform>();
        PulsingLight Pulser = new PulsingLight();

        //Pulsebool and pulsers are used for pulsing or changing lights
        bool[] PulseBool = new bool[4];
        double[] Pulsers = new double[4];
        // win and winBool are used to check if win conditions match
        double[] win = new double[2];
        bool[] winBool = new bool[2];

        Player player = new Player(new Thickness(32, 300, 0, 0), 32, 32);


        private bool trgMove = false;
        private int TargetMove = 0;
        private bool IsGrounded = false;
        private bool IsPaused = true;

        public double RotateTest { get; set; }


        public Level()
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
            Pulsers[0] = colorTest1.Offset;
            Pulsers[1] = colorTest1.Offset;
            Pulsers[2] = enmColor.Offset;
            Pulsers[3] = d.Offset;
            for (int i = 0; i < PulseBool.Length; i++)
            {
                PulseBool[i] = true;
            }

            rctPlayer.DataContext = player;

            winBool[0] = false;
            winBool[1] = false;
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
        // TODO change to player class or figure out a way to make it prettier
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

        // TODO FIX THIS into a class 
        private void RedBlock()
        {
            if (TargetMove != 0)
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


        // TODO Figure out how to get 
        // colortest1.offset = pulsers[0] and other similar things out of here
        private void GoalPulse()
        {
            CollisionDetect(rctPlayer, BlueGoal);
            CollisionDetect(rctTarget, rctGoal);

            //  TimeTest.Content = "1. colortest1\n" + colorTest1.Offset;
            Pulser.Pulsing(Pulsers, PulseBool);
            colorTest1.Offset = Pulsers[0];
            colorTest1.Offset = Pulsers[1];
            enmColor.Offset = Pulsers[2];
            d.Offset = Pulsers[3];


            win[0] = rdL.Offset;
            win[1] = blL.Offset;

            Pulser.WinPulse(win, winBool);

            rdL.Offset = win[0];
            blL.Offset = win[1];

            if (Light.Opacity > 0.5)
            {
                Light.Opacity -= 0.03;
            }
            else { Light.Opacity += 0.03; }

        }




        private void GameLoop(object sender, EventArgs e)
        {
            try
            {
                if (blL.Offset > 0.6 && rdL.Offset > 0.6)
                {
                    this.Close();
                }
                if (IsGrounded == true) { Jumping(); }
                
                GoalPulse();

                //TimeTest.Content = DateTime.Now.ToLongTimeString();

                Player();
                RedBlock();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        // TODO make it better
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

        // not important, 
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



        // TODO Make it prettier
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
                        if (trgMove == false)
                        {
                            trgMove = true;
                            //vasemmalle
                            if ((playerBox_rect.X + playerBox_rect.Width - 4) <= objB_rect.X)
                            {
                                TargetMove = 2;
                            }
                            //ylös
                            else if (playerBox_rect.Y + playerBox_rect.Height - 4 <= objB_rect.Y)
                            {
                                TargetMove = 4;
                            }
                            //alas
                            else if (playerBox_rect.Y >= objB_rect.Y + objB_rect.Height - 4)
                            {
                                TargetMove = 3;
                            }
                            else //oikealle TODO paremmi9n
                            { TargetMove = 1; }
                        }


                    }

                    else if (playerBox.Name == "rctTarget")
                    {

                        if (objB.Name == "rctGoal") { winBool[0] = true; }

                        else if (objB.Name != "rctPlayer") { trgMove = false; }
                    }
                    else if (playerBox.Name == "rctPlayer")
                    {


                        if (objB.Name == "BlueGoal")
                        { winBool[1] = true; }


                    }
                    else { winBool[1] = false; }
                    return false;
                }

                else
                {
                    return true;
                }
            return true;

        }



        // Keydown functions
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //TODO remove or something this
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
                        Level retry = new Level();
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


        //TODO Check this out
        private void Jumping()
        {

            IsGrounded = player.Jumping(0);
            if (player.jumpCounter == 1) { Collision.Stop(); jump.Play(); }

            if (CollisionTest(rctPlayer)) { }
            else { IsGrounded = player.Jumping(1); player.jumpCounter = 45; jump.Stop(); Collision.Play(); }

        }



        // works perfectly
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
            Level retry = new Level();
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
