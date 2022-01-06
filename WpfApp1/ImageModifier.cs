using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class ImageModifier
    {

        #region [   ImageConversion   ]

        // Save/Convert Images as/to JPG, PNG or BMP

        public static void SaveAsJPG(string sourcePath, string targetPath, int quality)
        {
            using(var bitmap = GetBitmap(sourcePath))
            {
                if(bitmap == null) return;

                ImageFormat imageFormat = ImageFormat.Jpeg;
                SaveAs(bitmap, targetPath, quality, imageFormat);
            }
        }
        public static void SaveAsJPG(string sourcePath, string targetPath)
        {
            SaveAsJPG(sourcePath, targetPath, 100);
        }

        public static void SaveAsJPG(string sourcePath, Stream targetStream, int quality)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, quality, ImageFormat.Jpeg);
        }

        public static void SaveAsJPG(string sourcePath, Stream targetStream)
        {
            SaveAsJPG(sourcePath, targetStream, 100);
        }

        public static void SaveAsJPG(Stream sourceStream, string targetPath, int quality)
        {
            Bitmap bitmap = GetBitmap(sourceStream);
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, quality, ImageFormat.Jpeg);
        }

        public static void SaveAsJPG(Stream sourceStream, string targetPath)
        {
            SaveAsJPG(sourceStream, targetPath, 100);
        }

        public static void SaveAsJPG(Stream sourceStream, Stream targetStream, int quality)
        {
            Bitmap bitmap = GetBitmap(sourceStream);
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, quality, ImageFormat.Jpeg);
        }

        public static void SaveAsJPG(Stream sourceStream, Stream targetStream)
        {
            SaveAsJPG(sourceStream, targetStream, 100);
        }

        public static void SaveAsJPG(Bitmap bitmap, string targetPath, int quality)
        {
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, quality, ImageFormat.Jpeg);
        }

        public static void SaveAsJPG(Bitmap bitmap, string targetPath)
        {
            SaveAsJPG(bitmap, targetPath, 100);
        }

        public static void SaveAsJPG(Bitmap bitmap, Stream targetStream, int quality)
        {
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, quality, ImageFormat.Jpeg);
        }

        public static void SaveAsJPG(Bitmap bitmap, Stream targetStream)
        {
            SaveAsJPG(bitmap, targetStream, 100);
        }


        public static void SaveAsPNG(string sourcePath, string targetPath)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, null, ImageFormat.Png);
        }

        public static void SaveAsPNG(string sourcePath, Stream targetStream)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, null, ImageFormat.Png);
        }

        public static void SaveAsPNG(Stream sourceStream, string targetPath)
        {
            Bitmap bitmap = GetBitmap(sourceStream);
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, null, ImageFormat.Png);
        }

        public static void SaveAsPNG(Stream sourceStream, Stream targetStream)
        {
            Bitmap bitmap = GetBitmap(sourceStream);
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, null, ImageFormat.Png);
        }

        public static void SaveAsPNG(Bitmap bitmap, string targetPath)
        {
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, null, ImageFormat.Png);
        }

        public static void SaveAsPNG(Bitmap bitmap, Stream targetStream)
        {
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, null, ImageFormat.Png);
        }


        public static void SaveAsBMP(string sourcePath, string targetPath)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, null, ImageFormat.Bmp);
        }

        public static void SaveAsBMP(string sourcePath, Stream targetStream)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, null, ImageFormat.Bmp);
        }

        public static void SaveAsBMP(Stream sourceStream, string targetPath)
        {
            Bitmap bitmap = GetBitmap(sourceStream);
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, null, ImageFormat.Bmp);
        }

        public static void SaveAsBMP(Stream sourceStream, Stream targetStream)
        {
            Bitmap bitmap = GetBitmap(sourceStream);
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, null, ImageFormat.Bmp);
        }

        public static void SaveAsBMP(Bitmap bitmap, string targetPath)
        {
            if(bitmap == null) return;

            SaveAs(bitmap, targetPath, null, ImageFormat.Bmp);
        }

        public static void SaveAsBMP(Bitmap bitmap, Stream targetStream)
        {
            if(bitmap == null) return;

            SaveAs(bitmap, targetStream, null, ImageFormat.Bmp);
        }


        private static void SaveAs(Bitmap bmp, string path, long? quality, ImageFormat imageFormat)
        {
            using(var encoderParameters = new EncoderParameters(1))
            {
                using(var parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality.Value))
                {
                    if(quality.HasValue)
                    {
                        encoderParameters.Param[0] = parameter; // 100L = 100% Quality
                    }
                    try
                    {
                        bmp.Save(path, GetEncoder(imageFormat), encoderParameters);
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
                    }
                }
            }
        }


        private static void SaveAs(Bitmap bmp, Stream stream, long? quality, ImageFormat imageFormat)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            if(quality.HasValue)
            {
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality.Value); // 100L = 100% Quality
            }
            try
            {
                bmp.Save(stream, GetEncoder(imageFormat), encoderParameters);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
            }
        }


        private static Bitmap GetBitmap(string path)
        {
            if(string.IsNullOrWhiteSpace(path)) return null;

            return new Bitmap(path);
        }

        private static Bitmap GetBitmap(Stream stream)
        {
            if(stream == null) return null;

            return new Bitmap(stream);
        }


        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach(ImageCodecInfo codec in codecs)
            {
                if(codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #endregion


        #region [   Scaling   ]

        // resize image to specified width and height

        public static void ResizeImage(string sourcePath, string targetPath, int width, int height)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            Bitmap resizedBitmap = ResizeBitmap(bitmap, width, height);
            try
            {
                resizedBitmap.Save(targetPath);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name}: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }

        public static Bitmap ResizeImage(Bitmap currentImage, int width, int height)
        {
            Bitmap resizedBitmap = ResizeBitmap(currentImage, width, height);
            return resizedBitmap;
        }

        // Scale image to certain width, height or highest Dimension
        // either save or return the image

        public static void ScaleToWidth(string sourcePath, string targetPath, int targetWidth)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            int scaledHeight = GetScaledHeight(bitmap.Width, bitmap.Height, targetWidth);
            Bitmap resizedBitmap = ResizeBitmap(bitmap, targetWidth, scaledHeight);
            try
            {
                resizedBitmap.Save(targetPath);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
            }
        }

        public static Bitmap ScaleToWidth(string sourcePath, int targetWidth)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            int scaledHeight = GetScaledHeight(bitmap.Width, bitmap.Height, targetWidth);
            Bitmap resizedBitmap = ResizeBitmap(bitmap, targetWidth, scaledHeight);
            return resizedBitmap;
        }

        public static void ScaleToHeight(string sourcePath, string targetPath, int targetHeight)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            int scaledWidth = GetScaledWidth(bitmap.Width, bitmap.Height, targetHeight);
            Bitmap resizedBitmap = ResizeBitmap(bitmap, scaledWidth, targetHeight);
            try
            {
                resizedBitmap.Save(targetPath);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
            }
        }

        public static Bitmap ScaleToHeight(string sourcePath, int targetHeight)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            int scaledWidth = GetScaledWidth(bitmap.Width, bitmap.Height, targetHeight);
            Bitmap resizedBitmap = ResizeBitmap(bitmap, scaledWidth, targetHeight);
            return resizedBitmap;
        }

        public static void ScaleToMax(string sourcePath, string targetPath, int targetMax)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            Bitmap resizedBitmap;
            int scaledMax = GetScaledMax(bitmap.Width, bitmap.Height, targetMax);
            if(bitmap.Width > bitmap.Height)
            {
                resizedBitmap = ResizeBitmap(bitmap, targetMax, scaledMax);
            }
            else
            {
                resizedBitmap = ResizeBitmap(bitmap, scaledMax, targetMax);
            }

            try
            {
                resizedBitmap.Save(targetPath);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name}: {e.Message}\n{e.StackTrace}");
            }
        }

        public static Bitmap ScaleToMax(string sourcePath, int targetMax)
        {
            Bitmap bitmap = GetBitmap(sourcePath);
            int scaledMax = GetScaledMax(bitmap.Width, bitmap.Height, targetMax);
            Bitmap resizedBitmap;
            if(bitmap.Width > bitmap.Height)
            {
                resizedBitmap = ResizeBitmap(bitmap, targetMax, scaledMax);
            }
            else
            {
                resizedBitmap = ResizeBitmap(bitmap, scaledMax, targetMax);
            }

            return resizedBitmap;
        }

        public static Bitmap ScaleToMax(Stream sourceStream, int targetMax)
        {
            Bitmap bitmap = GetBitmap(sourceStream);
            int scaledMax = GetScaledMax(bitmap.Width, bitmap.Height, targetMax);
            Bitmap resizedBitmap;
            if(bitmap.Width > bitmap.Height)
            {
                resizedBitmap = ResizeBitmap(bitmap, targetMax, scaledMax);
            }
            else
            {
                resizedBitmap = ResizeBitmap(bitmap, scaledMax, targetMax);
            }

            return resizedBitmap;
        }

        // calculations for scaling methods

        public static int GetScaledHeight(int width, int height, int scaledWidth)
        {
            if(scaledWidth == 0) return 0;

            double lambda = (double)width / scaledWidth;
            int scaledHeight = (int)(height / lambda);
            return scaledHeight;
        }

        public static int GetScaledWidth(int width, int height, int scaledHeight)
        {
            if(scaledHeight == 0) return 0;

            double lambda = (double)height / scaledHeight;
            int scaledWidth = (int)(width / lambda);
            return scaledWidth;
        }

        public static int GetScaledMax(int width, int height, int scaledMax)
        {
            if(scaledMax == 0) return 0;
            double lambda = Math.Max(width, height) / (double)scaledMax;
            int scaledWidth = (int)(width / lambda);
            int scaledHeight = (int)(height / lambda);
            return Math.Min(scaledWidth, scaledHeight);
        }

        private static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using(Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }
            return result;
        }
        #endregion


        #region [   Rotating   ]
        public static Bitmap Rotate(Bitmap bitmap, RotateFlipType rotateFlip) // Rotates bitmap via rotateFlip and returns bitmap
        {
            bitmap.RotateFlip(rotateFlip);
            return bitmap;
        }
        #endregion
    }
}
