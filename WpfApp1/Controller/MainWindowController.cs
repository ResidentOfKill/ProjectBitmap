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
using System.IO;
using WpfApp1.Helpers;
using WpfApp1.Interfaces;
using System.Security.AccessControl;

namespace WpfApp1.Controller
{
    public partial class MainWindowController : Window
    {
        private MainWindowModel _model;
        private int _imageCount = 0;

        public MainWindowController()
        {
            InitializeComponent();
            SliderStackPanel.Visibility = Visibility.Collapsed;
            Closing += DeleteTemporaryData_Closing;
        }

        public string OriginalImagePath { get; set; }

        private void DeleteTemporaryData_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //File.Delete(_model.TargetPath);
        }

        private void SetTargetPath(string fileExtension)
        {
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"temp", $"BitmapTempImage{++_imageCount}.bmp");
            System.IO.Path.ChangeExtension(path, fileExtension);
            _ = Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            _model.TargetPath = path;
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
                OriginalImagePath = dialog.FileName;
                var currentImage = new BitmapImage(new Uri(OriginalImagePath));
                CurrentImage.Source = currentImage;
                _model = new MainWindowModel(OriginalImagePath);
                _model.TargetPath = currentImage.UriSource.AbsolutePath;
            }
            else
            {
                SetTargetPath(".BMP");
            }
        }

        public void Save_Click(object sender, EventArgs e)
        {
            _model.SaveImage();
        }

        public void QualitySlider_ValueChanged(object sender, EventArgs e)
        {
            if(SliderValueTextBox != null)
            {
                SliderValueTextBox.Text = QualitySlider.Value.ToString("N2");
                _model.UpdateQuality((int)QualitySlider.Value);     //Quality can not be updated upwards
                if(!Equals(CurrentImage, null) && !Equals(_model.TargetPath, null))
                {
                    CurrentImage.Source = new BitmapImage(new Uri(_model.TargetPath));
                }
            }
        }
        public void Open_Tooltipp(object sender, EventArgs e)
        {
            Window1 window = new Window1 ();
            window.Show ();
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
