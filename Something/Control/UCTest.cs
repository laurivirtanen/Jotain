using Something.Classes;
using Something.Levels;
using System;
using System.Windows.Controls;

namespace Something.Control
{
    public partial class UCTest : UserControl, ISwitchable
    {

        public UCTest()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new PageTest());
        }

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
