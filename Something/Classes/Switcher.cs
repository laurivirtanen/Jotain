using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Something.Levels;

namespace Something.Classes
{
    public static class Switcher
    {
        public static WPFPageSwitch pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            pageSwitcher.Navigate(newPage);
        }

        public static void Switch(UserControl newPage,object state)
        {
            pageSwitcher.Navigate(newPage, state);
        }

        internal static void Switch(Level1 level)
        {
            throw new NotImplementedException();
        }
    }
}
