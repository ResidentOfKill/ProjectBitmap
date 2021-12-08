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
using WpfApp1.Helpers;
using WpfApp1.Interfaces;

namespace WpfApp1.Controller
{
    public partial class MainWindowController : Window
    {
        private MainWindowModel _model;
        public MainWindowController()
        {
            InitializeComponent();
            _model = new MainWindowModel();
            SliderStackPanel.Visibility = Visibility.Collapsed;
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
                var currentImage = new BitmapImage(new Uri(ImagePath));
                CurrentImage.Source = currentImage;
                _model.CurrentImage = new Bitmap(currentImage.UriSource.AbsolutePath);
            }
        }

        public void Save_Click(object sender, EventArgs e)
        {

        }

        public void QualitySlider_ValueChanged(object sender, EventArgs e)
        {
            SliderValueTextBox.Text = QualitySlider.Value.ToString("N2");
        }

        public void Resize_Click(object sender, EventArgs e)
        {
            if(int.TryParse(ImageWidthTextBox.Text, out var width) && int.TryParse(ImageHeightTextBox.Text, out var height))
            {
                _model.ScaleImage(width, height);
            }
            else
            {
                throw new ArgumentException($"{ImageWidthTextBox} und {ImageHeightTextBox} enthalten keine oder inkompatible Numerischen Werte.");
            }
        }

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radioButtonContent = sender is RadioButton isRadioButton ? isRadioButton.Content as string : throw new Exception();
            if(radioButtonContent == "JPG")
            {
                SliderStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SliderStackPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
