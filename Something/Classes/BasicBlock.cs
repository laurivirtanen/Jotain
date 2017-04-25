using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Something.Classes
{
    public abstract class BasicBlock : INotifyPropertyChanged
    {
        public double moving = 5;
        public bool trgMove = false;
        public int TargetMove = 0;
        public double Height { get; set; }
        public double Width { get; set; }
        public bool winCondition = false;

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

        public virtual bool CollisionDetect(Shape playerBox, Shape objB)
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
                    // punasen palikan liikuttelu 
                    if (objB.Name == "rctPlayer" && trgMove == false)
                    {
                        trgMove = true;
                        //vasemmalle
                        if ((playerBox_rect.X + playerBox_rect.Width) <= objB_rect.X)
                        {
                            TargetMove = 2;
                        }
                        //ylös
                        else if (playerBox_rect.Y + playerBox_rect.Height <= objB_rect.Y)
                        {
                            TargetMove = 4;
                        }
                        //alas
                        else if (playerBox_rect.Y >= objB_rect.Y + objB_rect.Height )
                        {
                            TargetMove = 3;
                        }
                        else //oikealle TODO paremmin
                        { TargetMove = 1; }

                    }
                    else if (objB.Name == "rctGoal") { winCondition = true; }


                    return false;
                }

                else
                {
                    return true;
                }
            return true;

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
}
