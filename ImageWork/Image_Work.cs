using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageWork
{
    class Image_Work
    {
        //для получения изображения из байтового массива
        public static BitmapImage byteToImg(byte[] array)
        {
            var i = new BitmapImage();
            using (var ms = new MemoryStream(array))
            {
                ms.Position = 0;
                i.BeginInit();
                i.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                i.CacheOption = BitmapCacheOption.OnLoad;
                i.StreamSource = ms;
                i.EndInit();
            }
            i.Freeze();
            return i;
        }

        //для получения байтового массива из изображения
        public static byte[] ImgToByte(string way)
        {
            byte[] ImgData = null;
            try
            {
                FileStream stream = new FileStream(way, FileMode.Open, FileAccess.Read);
                StreamReader read = new StreamReader(stream);
                ImgData = new byte[stream.Length - 1];
                stream.Read(ImgData, 0, (int)stream.Length - 1);
                return ImgData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ImgData;
        }

        //для работыи изображения с мышью
        public static void INK(FrameworkElement el, Canvas can, IInputElement t)
        {

            //для зума и вращения
            el.MouseWheel += (ss, ee) =>
            {
                Matrix mat = el.RenderTransform.Value;
                System.Windows.Point mouse = ee.GetPosition(el);
                if (ee.RightButton == MouseButtonState.Pressed)
                {
                    if (ee.Delta > 0)
                    {
                        mat.RotateAtPrepend(10, mouse.X, mouse.Y);
                    }
                    else
                    {
                        mat.RotateAtPrepend(-10, mouse.X, mouse.Y);
                    }
                }
                else
                {
                    if (ee.Delta > 0)
                    {
                        mat.ScaleAtPrepend(1.15, 1.15, mouse.X, mouse.Y);
                    }
                    else
                    {
                        mat.ScaleAtPrepend(1 / 1.15, 1 / 1.15, mouse.X, mouse.Y);

                    }
                }
                MatrixTransform mtf = new MatrixTransform(mat);
                el.RenderTransform = mtf;
            };
            System.Windows.Point first = new System.Windows.Point();

            //для удаления двойным щелчком
            el.MouseLeftButtonDown += (ss, ee) =>
            {
                first = ee.GetPosition(t);
                el.CaptureMouse();
                if (ee.ClickCount == 2)
                {
                    can.Children.Remove(el);
                }
            };

            //для отпускания мыши
            el.MouseUp += (ss, ee) =>
            {
                el.ReleaseMouseCapture();
            };

            //перемещение
            el.MouseLeftButtonDown += (ss, ee) => {
                first = ee.GetPosition(t);
                el.CaptureMouse();
            };

            el.MouseMove += (ss, ee) =>
            {
                if (ee.LeftButton == MouseButtonState.Pressed)
                {
                    System.Windows.Point point = ee.GetPosition(t);
                    System.Windows.Point res = new System.Windows.Point(first.X - point.X, first.Y - point.Y);
                    //обновления местоположения
                    Canvas.SetLeft(el, -res.X);
                    Canvas.SetTop(el, -res.Y);
                }
            };
        }
    }
}
