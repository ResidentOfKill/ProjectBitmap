using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WpfApp1
{
    public class MainWindowModel
    {

        public MainWindowModel(string imagePath)
        {
            OriginalImagePath = imagePath;
            OriginalImage = new Bitmap(OriginalImagePath);
        }

        public string OriginalImagePath { get; }


        private string _targetPath;
        public string TargetPath
        {
            get => _targetPath;
            set
            {
                if(!string.IsNullOrWhiteSpace(value))
                {
                    CurrentImage = new Bitmap(value);
                    _targetPath = value;
                }
            }
        }

        public Bitmap CurrentImage { get; private set; }

        public Bitmap OriginalImage { get; }

        public Bitmap ScaleImage(int width, int height)
        {
            var newImage = ImageModifier.ResizeImage(CurrentImage, width, height);
            CurrentImage = newImage;
            return newImage;
        }

        public void UpdateQuality(int quality)
        {
            SaveAsJPG(quality);
            CurrentImage = !string.IsNullOrWhiteSpace(TargetPath) ? new Bitmap(TargetPath) : null;
        }

        public void SaveAsJPG(int quality)
        {
            ImageModifier.SaveAsJPG(OriginalImagePath, TargetPath, quality);
        }

        public void SaveAsPNG()
        {
            ImageModifier.SaveAsPNG(CurrentImage, TargetPath);
        }

        public void SaveAsBMP()
        {
            ImageModifier.SaveAsBMP(CurrentImage, TargetPath);
        }

        public void SaveImage()
        {
            CurrentImage.Save("CurrentImage", ImageFormat.Jpeg);
        }
    }
}
