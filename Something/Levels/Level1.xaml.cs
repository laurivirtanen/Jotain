using Something.Classes;
using Something.Levels;
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
    public partial class Level1 : Window
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

        public Player player = new Player(new Thickness(32, 300, 0, 0), 32, 32);
        public BasicBlock target = new BasicBlock(new Thickness(20, 155, 0, 0), 32, 32);

        private bool IsGrounded = false;
        public bool IsPaused = true;
        public double RotateTest { get; set; }


        public Level1()
        {
            try
            {

                InitializeComponent();
                InitStuff();
                RotateTest = 0;
                timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                timer.Tick += new EventHandler(GameLoop);
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
            cnvBase.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
            rctPlayer.DataContext = player;
            rctTarget.DataContext = target;
            
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

            if (IsGrounded == true) { Jumping(); }

        }

        // TODO FIX THIS into a class 
        private void RedBlock()
        {
            if ( player.TargetMove != 0)
                switch (player.TargetMove)
                {
                    case 1:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { target.Placement = new Thickness(rctTarget.Margin.Left + player.moving, rctTarget.Margin.Top, 0, 0); }
                        else
                        {
                            target.Placement = new Thickness(rctTarget.Margin.Left - player.moving, rctTarget.Margin.Top, 0, 0);
                            player.TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 2:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { target.Placement = new Thickness(rctTarget.Margin.Left - player.moving, rctTarget.Margin.Top, 0, 0); }
                        else
                        {
                            target.Placement = new Thickness(rctTarget.Margin.Left + player.moving, rctTarget.Margin.Top, 0, 0);
                            player.TargetMove = 1;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 3:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { target.Placement = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top + player.moving, 0, 0); }
                        else
                        {
                            target.Placement = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top - player.moving, 0, 0);
                            player.TargetMove = 2;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 4:
                        mapBlocks.RemoveAt(8);
                        if (CollisionTest(rctTarget)) { target.Placement = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top - player.moving, 0, 0); }
                        else
                        {
                            target.Placement = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top + player.moving, 0, 0);
                            player.TargetMove = 3;
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
            player.CollisionDetect(rctPlayer, BlueGoal);
            target.CollisionDetect(rctTarget, rctGoal);
            
            Pulser.Pulsing(Pulsers, PulseBool);
            colorTest1.Offset = Pulsers[0];
            colorTest1.Offset = Pulsers[1];
            enmColor.Offset = Pulsers[2];
            d.Offset = Pulsers[3];
            
            win[0] = rdL.Offset;
            win[1] = blL.Offset;
            winBool[0] = target.winCondition;
            winBool[1] = player.winCondition;
            Pulser.WinPulse(win, winBool);

            target.winCondition = winBool[0];
            player.winCondition = winBool[1];

            rdL.Offset = win[0];
            blL.Offset = win[1];

            if(Light.Opacity > 0.5)
            {
                Light.Opacity -= 0.03;
            }else { Light.Opacity += 0.03; }

        }



        
        private void GameLoop(object sender, EventArgs e)
        {
            try {

                EndLevel();
                GoalPulse();
                Player();
                RedBlock();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void EndLevel()
        {
            if (blL.Offset > 0.6 && rdL.Offset > 0.6)
            {
                blL.Offset = 0.05;
                rdL.Offset = 0.05;
                player.winCondition = false;
                target.winCondition = false;
                IsGrounded = true;
                daa.Content = new Level2();
            }
        }

        // TODO make it better
        private bool CollisionTest(Shape plr)
        {
            bool dum = true;

            foreach (var item in mapBlocks)
            {
                dum = player.CollisionDetect(plr, item);
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

        



        // Keydown functions
        public void Window_KeyDown(object sender, KeyEventArgs e)
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
                        musicPlayer.Play();
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
            if (daa.Content == null)
            {
                Level1 retry = new Level1();
                retry.Show();
                Close();
            }
            
            PauseScreen.Visibility = Visibility.Hidden;
            pauseMusic.Stop();
            musicPlayer.Play();
            IsPaused = false;
            timer.Start();
        }
    }
}
