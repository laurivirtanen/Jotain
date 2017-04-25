using Something.Classes;
using Something.Levels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Navigation;
/// <summary>
/// Song from (https://soundcloud.com/laserost)  (http://www.youtube.com/user/Manofunctional).
/// Harjoitustyö koulun kurssille, toivottavasti herra opettaja arvostaa spagettikoodia 
/// </summary>


namespace Something
{
    /// <summary>
    /// Level1
    /// </summary>
    public partial class Level1 : Window
    {
        // dispatcherpriority.Render should make the game run smoother
        public DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);

        //list of shapes what we want to compare against in collisiontesting
        List<Shape> mapBlocks = new List<Shape>();
        // lights list
        List<SkewTransform> Lights = new List<SkewTransform>();
        PulsingLight Pulser = new PulsingLight();

        //Pulsebool and pulsers are used for pulsing or changing lights
        bool[] PulseBool = new bool[4];
        double[] Pulsers = new double[4];
        // win and winBool are used to check if win conditions match
        double[] win = new double[2];
        bool[] winBool = new bool[2];
        
        
        //create player, assing it coordinates 32,300   and size 32,32
        public Player player = new Player(new Thickness(32, 300, 0, 0), 32, 32);

        //create player, assing it coordinates 20,155  and size 32,32
        public MovingBlock target = new MovingBlock(new Thickness(20, 155, 0, 0), 32, 32);

        private bool IsGrounded = false;
        //this was just for laughs, to test if we can rotate the whole canvas with Q -- and i thought it might be rolling physics with it
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
            mapBlocks.Add(rctLeftWall);
            mapBlocks.Add(rctRightWall);
            mapBlocks.Add(rctCeiling);
            mapBlocks.Add(rctRight);
            mapBlocks.Add(rctTarget);
            //"lights"
            Lights.Add(rctSkew);
            Lights.Add(rctSkew1);
            Lights.Add(rctSkew2);

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
        // Moves the target block when player hits it
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
            player.CollisionDetect(rctPlayer, rctBlueGoal);
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

            if(rctLight.Opacity > 0.5)
            {
                rctLight.Opacity -= 0.03;
            }else { rctLight.Opacity += 0.03; }

        }



        // gameloop as in what happens within every tick
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
                /*if(txtLevel > kaikki.Count)
                {
                    txtLevel = 0;
                }
                frmLevel.Content = kaikki[txtLevel+1];
                txtLevel++;
                */
                // frmLevel.Content = new Level2();
                frmLevel.Content = new Level2();
                Config.txtLevel = 1;
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
                //this was just for laughs, to test if we can rotate the whole canvas with Q 
                case Key.Q:
                    RotateTest += 45;
                    cnvRotate.Angle = RotateTest;
                    break;

                case Key.Space:

                    IsGrounded = true;

                    if (Config.Paused == true)
                    {
                        this.Cursor = Cursors.None;
                        Config.Paused = false;
                        cnvPause.Visibility = Visibility.Hidden;
                        timer.Start();
                    }

                    break;

                case Key.Escape:

                    if (Config.Paused == false)
                    {

                        this.Cursor = Cursors.Arrow;
                        mdePause.Play();
                        cnvPause.Visibility = Visibility.Visible;
                        Config.Paused = true;
                        timer.Stop();
                    }
                    else
                    {
                        Config.Paused = false;
                    }
                    break;

                case Key.R:

                    if (Config.Paused == true)
                    {
                        Level1 retry = new Level1();
                        retry.Show();
                        this.Close();
                        cnvPause.Visibility = Visibility.Hidden;
                        mdePause.Stop();
                        Config.Paused = false;
                        timer.Start();
                    }
                    break;
            }
        }


        //TODO MAKE IT BETTER - - Player Jump 
        private void Jumping()
        {

            IsGrounded = player.Jumping(0);
            if (player.jumpCounter == 1) { }
            if (CollisionTest(rctPlayer)) { }
            else { IsGrounded = player.Jumping(1); player.jumpCounter = 45; }

        }



        // works perfectly
        public void Quit_Game(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        // Continue from menu
        public void Continue(object sender, RoutedEventArgs e)
        {
            cnvPause.Visibility = Visibility.Hidden;
            mdePause.Stop();
            Config.Paused = false;
            timer.Start();
        }

        //retry button
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            // restart rctTargetng
            if (frmLevel.Content == null)
            {
                Level1 retry = new Level1();
                frmLevel.Content = null;
                retry.Show();
                Close();
            }
            else
            {//resets the current level
                switch (Config.txtLevel)
                {
                    case 1:
                        frmLevel.Content = new Level2();
                        break;
                    case 2:
                        frmLevel.Content = new Level3();
                        break;
                    case 3:
                        frmLevel.Content = new Level4();
                        break;
                    case 4:
                        frmLevel.Content = new Level5();
                        break;
                    default:
                        break;
                }
            }
            
            cnvPause.Visibility = Visibility.Hidden;
            mdePause.Stop();
            Config.Paused = false;
            timer.Start();
        }
        // keeps the same song rolling
        private void mdeMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            mdeMusic.Position = TimeSpan.FromMilliseconds(0);
            
        }
    }
}
