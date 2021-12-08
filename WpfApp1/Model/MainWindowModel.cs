using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WpfApp1
{
    public class MainWindowModel
    {
        public MainWindowModel()
        {
            
        }

        public string ImagePath { get; set; }
        public string TargetPath { get; set; }

        public Bitmap CurrentImage { get; set; }

        public void ScaleImage(int width,int height)
        {
            CurrentImage = ImageModifier.ResizeImage(CurrentImage, width, height);
        }

        public void SaveAsJPG(int quality)
		{
            ImageModifier.SaveAsJPG(CurrentImage, TargetPath , quality);
		}

        public void SaveAsPNG()
		{
            ImageModifier.SaveAsPNG(CurrentImage,TargetPath);
		}

        public void SaveAsBMP ()
        {
            ImageModifier.SaveAsBMP(CurrentImage, TargetPath);
        }

    }
}
