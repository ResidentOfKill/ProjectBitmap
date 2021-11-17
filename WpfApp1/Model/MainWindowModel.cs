using System;
using System.Collections.Generic;
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

        public void ScaleImage(int width,int height)
        {
            ImageModifier.ResizeImage(ImagePath, TargetPath, width, height);
        }
    }
}
