using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace ImageWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int i1 = 1;//первое изображение
        public int i2 = 2;//второе изображение
        public int i3 = 3;//третье изображение

        public MainWindow()
        {
            InitializeComponent();

            BtnLeft.Content = @"<";
            BtnRight.Content = @">";

            //загрузка изображений
            using (var db = new Try_DB_Entities())
            {
                var pictures = db.Picture.ToList();
                var pic1 = pictures.FirstOrDefault(u => u.Picture_ID == i1);
                var pic2 = pictures.FirstOrDefault(u => u.Picture_ID == i2);
                var pic3 = pictures.FirstOrDefault(u => u.Picture_ID == i3);

                Img1.Source = Image_Work.byteToImg(pic1.Picture1);
                Img2.Source = Image_Work.byteToImg(pic2.Picture1);
                Img3.Source = Image_Work.byteToImg(pic3.Picture1);
            }
        }
        //кнопка для загрузки данных
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "Изображение *.jpg; *.png; *.jpeg| *.jpg; *.png; *.jpeg";
            if (open.ShowDialog() == true)
            {
                foreach (var item in open.FileNames)
                {
                    //загрузка изображений
                    using (var db = new Try_DB_Entities())
                    {
                        byte[] array = Image_Work.ImgToByte(item);
                        Picture picture = new Picture
                        {
                            Picture1 = array
                        };
                        db.Picture.Add(picture);
                        db.SaveChanges();
                    }
                }
            }
        }

        //скрытие изображения
        private void largeImg_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            largeImg.Visibility = Visibility.Hidden;
            gridImg.IsEnabled = true;
            BtnLoad.IsEnabled = true;
        }

        private void Img1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            largeImg.Visibility = Visibility.Visible;
            largeImg.Source = img.Source;
            gridImg.IsEnabled = false;
            BtnLoad.IsEnabled = false;
        }

        //перелистывание в левую сторону
        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            //загрузка изображений
            using (var db = new Try_DB_Entities())
            {
                var pictures = db.Picture.ToList();
                if (i1 == 1)
                {
                    i3 = i2;
                    i2 = i1;
                    i1 = pictures.LastOrDefault().Picture_ID;
                }
                else
                {
                    i3 = i2;
                    i2 = i1;
                    i1 = i1 - 1;
                }

                var pic1 = pictures.FirstOrDefault(u => u.Picture_ID == i1);
                var pic2 = pictures.FirstOrDefault(u => u.Picture_ID == i2);
                var pic3 = pictures.FirstOrDefault(u => u.Picture_ID == i3);

                Img1.Source = Image_Work.byteToImg(pic1.Picture1);
                Img2.Source = Image_Work.byteToImg(pic2.Picture1);
                Img3.Source = Image_Work.byteToImg(pic3.Picture1);
            }
        }

        //перелистывание в правую сторону
        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            //загрузка изображений
            using (var db = new Try_DB_Entities())
            {
                var pictures = db.Picture.ToList();
                int iLast = pictures.LastOrDefault().Picture_ID;
                if (i3 == iLast)
                {
                    i1 = i2;
                    i2 = i3;
                    i3 = pictures.FirstOrDefault().Picture_ID;
                }
                else
                {
                    i1 = i2;
                    i2 = i3;
                    i3 = i3 + 1;
                }

                var pic1 = pictures.FirstOrDefault(u => u.Picture_ID == i1);
                var pic2 = pictures.FirstOrDefault(u => u.Picture_ID == i2);
                var pic3 = pictures.FirstOrDefault(u => u.Picture_ID == i3);

                Img1.Source = Image_Work.byteToImg(pic1.Picture1);
                Img2.Source = Image_Work.byteToImg(pic2.Picture1);
                Img3.Source = Image_Work.byteToImg(pic3.Picture1);
            }
        }
    }
}
