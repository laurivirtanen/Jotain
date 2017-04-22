using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Something.Classes
{
    public abstract class MovingBlock : INotifyPropertyChanged
    {
        public double moving = 5;
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
