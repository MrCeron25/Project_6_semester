using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Project.Models
{
    public class Tools
    {
        public static byte[] GetImageBytes(string filePath)
        {
            byte[] photo = null;
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        photo = reader.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return photo;
        }

        public static BitmapImage BytesToImage(byte[] bytes)
        {
            BitmapImage image = null;
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show($"Ошибка загрузки фотографии.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return image;
        }

        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            try
            {
                using (MemoryStream memory = new MemoryStream() { Position = 0 })
                {
                    bitmap.Save(memory, ImageFormat.Png);
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return bitmapImage;
        }
    }
}
