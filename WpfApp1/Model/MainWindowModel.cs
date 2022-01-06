using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfApp1
{
    public class MainWindowModel
    {
        
        public MainWindowModel(string imagePath)
        {
            OriginalImagePath = imagePath;
            OriginalImage = new Bitmap(OriginalImagePath);
            CurrentImagePath = OriginalImagePath;
            CurrentImage = new Bitmap(CurrentImagePath);
        }

        public string OriginalImagePath { get; }


        private string _currentImagePath;

        public string CurrentImagePath
        {
            get => _currentImagePath;
            set
            {
                if(!string.IsNullOrWhiteSpace(value))
                {
                    _currentImagePath = value;
                }
            }
        }


        private Bitmap _currentImage;
        public Bitmap CurrentImage
        {
            get => _currentImage;
            private set
            {
                _currentImage?.Dispose();
                _currentImage = value;
            }
        }

        public Bitmap OriginalImage { get; }

        public ImageFormat Format { get; set; }


        public void ScaleImage(int width, int height)
        {
            var newImage = ImageModifier.ResizeImage(CurrentImage, width, height);
            CurrentImage = newImage;
        }

        public void UpdateQuality(int quality)
        {
            SaveAsJPG(quality);
        }

        public void SaveAsJPG(int quality)
        {

            ImageModifier.SaveAsJPG(OriginalImage, CurrentImagePath, quality);
            CurrentImage = new Bitmap(CurrentImagePath);
        }

        public void SaveAsPNG()
        {
            ImageModifier.SaveAsPNG(CurrentImage, CurrentImagePath);
        }

        public void SaveAsBMP()
        {
            ImageModifier.SaveAsBMP(CurrentImage, CurrentImagePath);
        }

        public void SaveImage(string path)
        {
            CurrentImage.Save(path, Format);
        }
    }
}
