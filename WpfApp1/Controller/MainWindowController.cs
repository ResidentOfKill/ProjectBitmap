using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using WpfApp1.Interfaces;

namespace WpfApp1.Controller
{
    public partial class MainWindowController : Window, IImageController
    {
        private MainWindowModel _model;
        public MainWindowController()
        {
            InitializeComponent();
            _model = new MainWindowModel();
        }

        public string ImagePath
        {
            get;
            set;
        }

        public void OpenFile_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*",
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Multiselect = false,
                Title = "Waehle Bilddatei aus",
            };
            if(dialog.ShowDialog() ?? false)
            {
                ImagePath = dialog.FileName;
                CurrentImage.Source = new BitmapImage(new Uri(ImagePath));
            }
        }

        public void Save_Click(object sender, EventArgs e)
        {

        }
    }
}
