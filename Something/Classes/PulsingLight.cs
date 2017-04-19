using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Something.Classes
{
    
    class PulsingLight
    {
        double pulse1 = 0.02;
        double pulse2 = 0.005;

        Random rand = new Random();

        public void Pulsing(double[] d, bool[] a)
        {
            for (int i = 0; i < d.Length; i++)
            {
                if(d[i] >= 0.95)
                {
                    a[i] = false;
                    if(d[i] <= 0.05)
                    {
                        a[i] = true;
                    }
                }

            }


            for (int i = 0; i < d.Length; i++){

                if (a[i] == true)
                {
                    d[i] += pulse1;
                }
                else
                {
                    a[i] = false;
                    d[i] -= pulse2;
                    if(d[i] <= pulse2)
                    {
                        a[i] = true;
                    }
                }
                
            }
        }

        public void WinPulse(double[] d, bool[] a)
        {
            for (int i = 0; i < d.Length; i++)
            {
                if (a[i] == true && d[i] <1)
                {
                    d[i] += pulse2;
                }
                else if(d[i] > 0.05)
                {
                    d[i] -= pulse1;
                }

            }
        }



    }
}
