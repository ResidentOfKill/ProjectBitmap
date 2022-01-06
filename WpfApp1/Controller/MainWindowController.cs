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
using System.Threading;

namespace WpfApp1.Controller
{
    public partial class MainWindowController : Window
    {
        private MainWindowModel _model;
        private int _imageCount = 0;

        public MainWindowController()
        {
            InitializeComponent();
            Closing += DeleteTemporaryData_Closing;
        }

        public string OriginalImagePath { get; set; }

        private void DeleteTemporaryData_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            CurrentImage = null;
            _model.CurrentImage.Dispose();
            _model.OriginalImage.Dispose();
            _model.CurrentImagePath = null;
            Directory.Delete(System.IO.Path.GetDirectoryName(_model.CurrentImagePath), true);
        }

        private void SetTargetPath(string fileExtension)
        {
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"temp", $"BitmapTempImage{++_imageCount}.bmp");
            System.IO.Path.ChangeExtension(path, fileExtension);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            if(!Equals(_model, null))
            {
                _model.CurrentImagePath = path;
            }
        }


        public void OpenFile_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Image Files(*.PNG;*.JPG;*.GIF)|*.PNG;*.JPG;*.GIF|All files (*.*)|*.*",
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Multiselect = false,
                Title = "Waehle Bilddatei aus",
            };
            if(dialog.ShowDialog() ?? false)
            {
                OriginalImagePath = dialog.FileName;
                _model = new MainWindowModel(OriginalImagePath);
                var currentImage = new BitmapImage(new Uri(_model.OriginalImagePath));
                CurrentImage.Source = currentImage;
            }
            else
            {
                SetTargetPath(".BMP");
            }
        }

        public void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {

            }
        }

        public void QualitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(SliderValueTextBox != null)
            {
                if(!Equals(CurrentImage, null) && !Equals(_model, null) && !Equals(_model.CurrentImagePath, null))
                {
                    _model.UpdateQuality((int)QualitySlider.Value);     //Quality can not be updated upwards
                    SliderValueTextBox.Text = QualitySlider.Value.ToString("N2");

                    CurrentImage.Source = new BitmapImage(new Uri(_model.CurrentImagePath));
                }
                else
                {
                    MessageBox.Show(this, "Bitte wählen Sie zuerst ein Bild aus", "Error 404: Image not found", MessageBoxButton.OK);
                    QualitySlider.ValueChanged -= QualitySlider_ValueChanged;
                    QualitySlider.Value = e.OldValue;
                    QualitySlider.ValueChanged += QualitySlider_ValueChanged;

                }
            }
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
            QualitySlider.IsEnabled = true;

            //var radioButtonContent = sender is RadioButton isRadioButton ? isRadioButton.Content as string : throw new Exception();
            //if(radioButtonContent == "JPG")
            //{
            //    SliderStackPanel.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    SliderStackPanel.Visibility = Visibility.Collapsed;
            //}
        }
    }
}
