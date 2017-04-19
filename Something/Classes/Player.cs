using System;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Shapes;

namespace Something
{
    class Player : INotifyPropertyChanged
    {
        public double moving = 5;
        public double gravity = 5;
        public int jumpCounter = 0;
        double jump;
        public bool IsGrounded;
        public double Height { get; set; }
        public double Width { get; set; }
        private Thickness placement;
        public Thickness Placement
        {
            get
            {
                return placement;
            }
            set
            {
                placement = value;
                RaisePropertyChanged();
            }
        }

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


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    // all Things Blocky baseclass to be implemented
    

}
