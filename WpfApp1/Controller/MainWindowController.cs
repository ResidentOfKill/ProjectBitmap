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
using Microsoft.Win32;
using System.IO;
using WpfApp1.Helpers;
using WpfApp1.Interfaces;
using System.Security.AccessControl;
using System.Threading;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace WpfApp1.Controller
{
    public partial class MainWindowController : Window
    {
        private MainWindowModel _model;
        private int _imageCount = 0;
        private int _variantCount = 0;
        private readonly string _directoryPath;
        private readonly string _tempFolderRemoverPath;

        public MainWindowController()
        {
            InitializeComponent();
            _directoryPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "temp");
            _tempFolderRemoverPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName, "TempFolderRemover", "bin", "Debug", "TempFolderRemover.exe");
            Directory.CreateDirectory(_directoryPath);
            Closing += DeleteTemporaryData_Closing;
        }

        public string OriginalImagePath { get; set; }

        private void DeleteTemporaryData_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Task.Run(() => Process.Start($"{_tempFolderRemoverPath}", $"{_directoryPath}"));
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

            };

            if(saveFileDialog.ShowDialog() ?? false)
            {
                _model.SaveImage(Path.ChangeExtension(saveFileDialog.FileName,_model.Format.ToString()));
            }
        }

        public void QualitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(SliderValueTextBox != null)
            {
                if(!Equals(CurrentImage, null) && !Equals(_model, null) && !Equals(_model.CurrentImagePath, null))
                {
                    var newPath = System.IO.Path.Combine(_directoryPath, $"tempImage{Guid.NewGuid()}_qualityChanged_{++_variantCount}.{ImageFormat.Jpeg.ToString().ToLower()}");
                    _model.CurrentImagePath = System.IO.Path.ChangeExtension(newPath, ".jpg");
                    Directory.CreateDirectory(_directoryPath);

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
        public void Open_Tooltipp(object sender, EventArgs e)
        {
            TooltippWindowController window = new TooltippWindowController();
            window.Show();
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
            if(_model == null)
            {
                JPGRadioButton.IsChecked = false;
                PNGRadioButton.IsChecked = false;
                BMPRadioButton.IsChecked = false;
                return;
            }

            QualitySlider.IsEnabled = true;
            SaveButton.IsEnabled = true;
            ResetButton.IsEnabled = true;
            AlignButton.IsEnabled = true;
            ResizeButton.IsEnabled = true;

            if(BMPRadioButton.IsChecked ?? false)
            {
                _model.Format = ImageFormat.Bmp;
            }
            else if(JPGRadioButton.IsChecked ?? false)
            {
                _model.Format = ImageFormat.Jpeg;
            }
            else if(PNGRadioButton.IsChecked ?? false)
            {
                _model.Format = ImageFormat.Png;
            }

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

        public void ResizeButton_Click(object sender, EventArgs e)
        {
            if(int.TryParse(ImageHeightTextBox.Text, out var imageHeight) && int.TryParse(ImageWidthTextBox.Text, out var imageWidth))
            {
                var directoryName = _directoryPath;
                Directory.CreateDirectory(_directoryPath);
                var fileName = System.IO.Path.GetFileNameWithoutExtension(_model.CurrentImagePath);
                var extension = System.IO.Path.GetExtension(_model.CurrentImagePath);

                var newFileName = "";
                if(!_model.CurrentImagePath.Contains("_resized_"))
                {
                    newFileName = $"{fileName}_resized_{++_variantCount}{extension}";
                }
                else
                {
                    newFileName = $"{fileName}{++_variantCount}{extension}";
                }

                int resizeIndex = newFileName.IndexOf("_resized_");

                var startIndex = newFileName.IndexOf("_resized_") + "_resized_".Length;
                var endIndex = newFileName.IndexOf($"{System.IO.Path.GetExtension(newFileName)}");

                newFileName = newFileName.Remove(startIndex, endIndex - startIndex);

                newFileName.Insert(resizeIndex, $"{++_variantCount}");
                newFileName = $"{System.IO.Path.GetFileNameWithoutExtension(newFileName)}{++_variantCount}{extension}";

                var savePath = System.IO.Path.Combine(directoryName, newFileName);
                _model.CurrentImagePath = savePath;
                _model.ScaleImage(imageWidth, imageHeight);
                _model.CurrentImagePath = savePath;

                _model.CurrentImage.Save(savePath);
                CurrentImage.Source = new BitmapImage(new Uri(savePath));
            }
        }

        public void AlignButton_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(_model.CurrentImagePath);
            if(long.TryParse(FileSizeTextBox.Text, out var destinedSize))
            {
                destinedSize *= 1_000;
                var width = _model.CurrentImage.Width;
                var height = _model.CurrentImage.Height;
                bool isFirstCycle = true;
                while(destinedSize != fileInfo.Length)
                {
                    if(isFirstCycle)
                    {
                        isFirstCycle = false;
                    }
                    else
                    {
                        if(destinedSize < fileInfo.Length)
                        {
                            width -= 100;
                            height -= 100;
                        }
                        else if(destinedSize > fileInfo.Length)
                        {
                            width += 100;
                            height += 100;
                        }

                        ImageWidthTextBox.Text = width.ToString();
                        ImageHeightTextBox.Text = height.ToString();
                        ResizeButton_Click(sender, e);
                    }
                }
                ImageWidthTextBox.Text = width.ToString();
                ImageHeightTextBox.Text = height.ToString();
                MessageBox.Show("Fertig");
            }
        }

        public void ResetButton_Click(object sender, EventArgs e)
        {
            var oldImagePath = new string(_model.OriginalImagePath.ToCharArray());
            _model = new MainWindowModel(oldImagePath);
            CurrentImage.Source = new BitmapImage(new Uri(oldImagePath));
        }
    }
}
