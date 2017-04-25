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

        public virtual bool CollisionDetect(Shape plrBlock, Shape otherBlock)
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
                    // punasen palikan liikuttelu 
                    if (otherBlock.Name == "rctPlayer" && trgMove == false)
                    {
                        trgMove = true;
                        //vasemmalle
                        if ((plrBlock_rect.X + plrBlock_rect.Width) <= otherBlock_rect.X)
                        {
                            TargetMove = 2;
                        }
                        //ylös
                        else if (plrBlock_rect.Y + plrBlock_rect.Height <= otherBlock_rect.Y)
                        {
                            TargetMove = 4;
                        }
                        //alas
                        else if (plrBlock_rect.Y >= otherBlock_rect.Y + otherBlock_rect.Height )
                        {
                            TargetMove = 3;
                        }
                        else //oikealle TODO paremmin
                        { TargetMove = 1; }

                    }
                    else if (otherBlock.Name == "rctGoal") { winCondition = true; }


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
