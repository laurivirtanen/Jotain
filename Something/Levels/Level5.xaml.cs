using Something.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Something.Levels
{
    /// <summary>
    /// Interaction logic for PageTest.xaml
    /// </summary>
    public partial class Level5 : Page
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

        Player player1 = new Player(new Thickness(32, 256, 0, 0), 32, 32);
        public MovingBlock target = new MovingBlock(new Thickness(500, 500, 0, 0), 32, 32);

        private bool trgMove = false;
        private int TargetMove = 0;
        private bool IsGrounded = true;

        public double RotateTest { get; set; }


        public Level5()
        {
            try
            {

                InitializeComponent();
                RotateTest = 0;
                timer.Tick += new EventHandler(GameLoop);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                InitStuff();
                timer.Start();
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

            rctPlayer.DataContext = player1;
            rctTarget.DataContext = target;

            winBool[0] = false;
            winBool[1] = false;
            //collision list
            mapBlocks.Add(rctBottom);
            mapBlocks.Add(rctBottomStop);
            mapBlocks.Add(rctBottomStop1);
            mapBlocks.Add(rctBottomStop2);
            mapBlocks.Add(rctBottomStop3);
            mapBlocks.Add(rctBottomStop4);
            mapBlocks.Add(rctBottomStop5);
            mapBlocks.Add(rctBottomStop6);
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
                player1.MovePlayer(0);
                if (CollisionTest(rctPlayer)) { LightTransform(Lights, 50, -50, true); }
                else { player1.MovePlayer(1); player1.IsGrounded = true; }

            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                player1.MovePlayer(1);
                if (CollisionTest(rctPlayer)) { LightTransform(Lights, 50, -50, false); }
                else { player1.MovePlayer(0); player1.IsGrounded = true; }
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                IsGrounded = true;
            }



            // moves the red block
            player1.MovePlayer(2);
            if (CollisionTest(rctPlayer)) { }
            else { player1.MovePlayer(3); }

        }

        
        // TODO FIX THIS into a class 
        private void RedBlock()
        {
            if (TargetMove != 0)
                switch (TargetMove)
                {
                    case 1:
                        mapBlocks.RemoveAt(14);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left + player1.moving, rctTarget.Margin.Top, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left - player1.moving, rctTarget.Margin.Top, 0, 0);
                            TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 2:
                        mapBlocks.RemoveAt(14);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left - player1.moving, rctTarget.Margin.Top, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left + player1.moving, rctTarget.Margin.Top, 0, 0);
                            TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 3:
                        mapBlocks.RemoveAt(14);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top + player1.moving, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top - player1.moving, 0, 0);
                            TargetMove = 0;
                        }
                        mapBlocks.Add(rctTarget);
                        break;
                    case 4:
                        mapBlocks.RemoveAt(14);
                        if (CollisionTest(rctTarget)) { rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top - player1.moving, 0, 0); }
                        else
                        {
                            rctTarget.Margin = new Thickness(rctTarget.Margin.Left, rctTarget.Margin.Top + player1.moving, 0, 0);
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
            CollisionDetect(rctPlayer, rctBlueGoal);
            CollisionDetect(rctTarget, rctGoal);

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

            if (rctLight.Opacity > 0.5)
            {
                rctLight.Opacity -= 0.03;
            }
            else { rctLight.Opacity += 0.03; }

        }



        //DispatcherTimer tick goes through these methods
        private void GameLoop(object sender, EventArgs e)
        {
            try
            {

                EndLevel();
                if (IsGrounded == true) { Jumping(); }
                GoalPulse();
                Player();
                RedBlock();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        // TODO compares collisions with all items in mapBlocks list
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

        // Moves the "lights" just barely with the player movement
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

        //What happens when winconditions are met
        private void EndLevel()
        {
            if (blL.Offset > 0.6 && rdL.Offset > 0.6)
            {
                blL.Offset = 0.05;
                rdL.Offset = 0.05;
                player1.winCondition = false;
                target.winCondition = false;
                timer.Stop();

                // changes txtLevel
                daa.Content = new Level2();
            }
        }

        
        /// <summary>
        /// 
        /// Collision comparing. Its not workin perfectly 
        /// 
        /// TODO - Make it more precise
        /// </summary>
        private bool CollisionDetect(Shape plrBlock, Shape otherBlock)
        {

            Rect plrBlock_rect = new Rect();
            Rect otherBlock_rect = new Rect();

            plrBlock_rect.X = plrBlock.Margin.Left;
            plrBlock_rect.Y = plrBlock.Margin.Top;
            plrBlock_rect.Width = plrBlock.ActualWidth;
            plrBlock_rect.Height = plrBlock.ActualHeight;


            otherBlock_rect.X = otherBlock.Margin.Left;
            otherBlock_rect.Y = otherBlock.Margin.Top;
            otherBlock_rect.Width = otherBlock.ActualWidth;
            otherBlock_rect.Height = otherBlock.ActualHeight;

            if ((otherBlock_rect.X < (plrBlock_rect.X + plrBlock_rect.Width) &&
               (otherBlock_rect.X + otherBlock_rect.Width) > plrBlock_rect.X))
                if (
                 (otherBlock_rect.Y < (plrBlock_rect.Y + plrBlock_rect.Height)) &&
                 (otherBlock_rect.Y + otherBlock_rect.Height) > plrBlock_rect.Y)
                {
                    if (otherBlock.Name == "rctTarget")
                    {
                        // punasen palikan liikuttelu 
                        if (trgMove == false)
                        {
                            trgMove = true;
                            //vasemmalle
                            if ((plrBlock_rect.X + plrBlock_rect.Width - 4) <= otherBlock_rect.X)
                            {
                                TargetMove = 2;
                            }
                            //ylös
                            else if (plrBlock_rect.Y + plrBlock_rect.Height - 4 <= otherBlock_rect.Y)
                            {
                                TargetMove = 4;
                            }
                            //alas
                            else if (plrBlock_rect.Y >= otherBlock_rect.Y + otherBlock_rect.Height - 4)
                            {
                                TargetMove = 3;
                            }
                            else //oikealle TODO paremmi9n
                            { TargetMove = 1; }
                        }


                    }

                    else if (plrBlock.Name == "rctTarget")
                    {

                        if (otherBlock.Name == "rctGoal") { winBool[0] = true; }

                        else if (otherBlock.Name != "rctPlayer") { trgMove = false; }
                    }
                    else if (plrBlock.Name == "rctPlayer")
                    {
                        if (otherBlock.Name == "rctBlueGoal")
                        { winBool[1] = true; }
                        else { winBool[1] = false; }
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

        


        //TODO Check this out
        private void Jumping()
        {

            IsGrounded = player1.Jumping(0);
            if (player1.jumpCounter == 1) { }

            if (CollisionTest(rctPlayer)) { }
            else { IsGrounded = player1.Jumping(1); player1.jumpCounter = 45; }

        }

        



    
        
    }
}
