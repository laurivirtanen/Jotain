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
    public class MovingBlock : BasicBlock, INotifyPropertyChanged
    {

        public int Amount;

        public MovingBlock(Thickness plc, double hgt, double wdt)
        {
            Placement = plc;
            Height = hgt;
            Width = wdt;
        }
       





        
    }
}
