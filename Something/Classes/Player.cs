using System;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Shapes;
using Something.Classes;

namespace Something
{
    public class Player : MovingBlock, INotifyPropertyChanged
    {
        public double gravity = 5;
        public int jumpCounter = 0;
        double jump;
        public bool IsGrounded;
        public int TargetMove = 0;
        public bool trgMove;
        

        public Player(Thickness plc, double hgt, double wdt)
        {
            Placement = plc;
            Height = hgt;
            Width = wdt;
        }



        public void MovePlayer(int side)
        {
            switch (side)
            {
                case 0: // right
                    Placement = new Thickness(Placement.Left + moving, Placement.Top, 0, 0);
                    break;
                case 1: //left
                    Placement = new Thickness(Placement.Left - moving, Placement.Top, 0, 0);
                    break;
                case 2: // gravity
                    Placement = new Thickness(Placement.Left, Placement.Top + gravity, 0, 0);
                    IsGrounded = false;
                    break;
                case 3: // gravity collision
                    Placement = new Thickness(Placement.Left, Placement.Top - gravity, 0, 0);
                    IsGrounded = true;
                    break;
                default:
                    Placement = new Thickness(Placement.Left, Placement.Top, 0, 0);
                    break;
            }

        }


        public bool Jumping(int side)
        {
            if ((jumpCounter == 0 && IsGrounded == true) || jumpCounter != 0)
            {
                jump = gravity * 2.2 - (jumpCounter / 5);

                if (jumpCounter < 50)
                {
                    jumpCounter++;
                    switch (side)
                    {
                        case 0:
                            Placement = new Thickness(Placement.Left, Placement.Top - jump, 0, 0);
                            return true;
                        case 1:
                            Placement = new Thickness(Placement.Left, Placement.Top + jump, 0, 0);
                            return true;
                        default:
                            return false;
                    }
                }

                else { jumpCounter = 0; return false; }
            }
            else { return false; }
        }



        public bool CollisionDetect(Shape playerBox, Shape objB)
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
                    
                    else if (objB.Name == "BlueGoal")
                    
                        {  winCondition = true; }
                    
                    return false;
                }

                else
                {
                    return true;
                }
            return true;
        }

        



        // all Things Blocky baseclass to be implemented

    }
}
