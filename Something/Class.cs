using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace Something
{
    class Player
    {
        public double positionX = 0;
        public double positionY = 0;
        public double height;
        public double width;

        public double MovePlayer(double x, double y)
        {
            return x;
        }

    }

    public class MediaPlayer
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        

        private void OpenAudioFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files(*.mp3) | *.mp3 | All files(*.*) | *.* ";
            if(openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.OpenAudioFile();
                
            }
        }
        

    }
}
