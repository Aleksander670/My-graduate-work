using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Philimon.objects.UI
{
    

    class DataBrowser : UIElement
    {
        public Canvas canvas;
        private Button closeButton;
#pragma warning disable CS0169 // Поле "DataBrowser.directoryPath" никогда не используется.
        private string directoryPath;
#pragma warning restore CS0169 // Поле "DataBrowser.directoryPath" никогда не используется.

        Image ImageLink;

        string selectedFileName;

        string format = "";
        public string DesktopImage { get; set; }

        string type_ = "";

        public DataBrowser(string path, string type)
        {
            int width = 800;
            int height = 400;

            type_ = type;

            canvas = new Canvas();

            canvas.Margin = new Thickness(Application.Current.MainWindow.Width/2 - width / 2, Application.Current.MainWindow.Height/2 - height/2, 0, 0);

            closeButton = new Button();
            closeButton.Content = "X";
            closeButton.Click += (sender, e) => { Close(); };

            StackPanel Stack = new StackPanel();
            Stack.Background = Brushes.Gray;
            Stack.Width = width;
            Stack.Height = height;
            canvas.Children.Add(Stack);

            List<string> filesList = new List<string>();

            string WriteAttr = "";
            

            if (type == "sound")
            {
                filesList = Directory.GetFiles(path, "*.wav").ToList();
                format = "." + "wav";
                WriteAttr = "startupSound_effect";
            }
            if (type == "picture")
            {
                filesList = Directory.GetFiles(path, "*.png").ToList();
                format = "." + "png";
                WriteAttr = "DesktopImage";
            }

            string[] files = filesList.ToArray();

            foreach (string file in files)
            {
                Canvas canvasItem = new Canvas();
                canvasItem.Width = 800;
                canvasItem.Height = 40;
                canvasItem.Background = Brushes.LightGray;
                canvasItem.Margin = new Thickness(0, 40, 0, 0);

                canvasItem.MouseLeftButtonDown += (sender, e) =>
                {
                    ((Canvas)sender).Background = Brushes.White;

                    selectedFileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    string iniFilePath = "settings.ini";

                    // Проверяем существование файла
                    if (!File.Exists(iniFilePath))
                    {
                        using (StreamWriter sw = File.CreateText(iniFilePath))
                        {
                            sw.WriteLine(WriteAttr + "=" + selectedFileName);
                            DesktopImage = selectedFileName;
                        }
                    }
                    else
                    {
                        string[] lines = File.ReadAllLines(iniFilePath);
                        bool found = false;

                        // Проверяем, был ли уже записан startupSound_effect
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].StartsWith(WriteAttr + "="))
                            {
                                lines[i] = WriteAttr + "=" + selectedFileName;
                                DesktopImage = selectedFileName;
                                found = true;
                                break;
                            }
                        }

                        // Если не было найдено, добавляем новую строку
                        if (!found)
                        {
                            Array.Resize(ref lines, lines.Length + 1);
                            lines[lines.Length - 1] = WriteAttr + "=" + selectedFileName;
                        }


                        // Перезаписываем файл с обновленными данными
                        File.WriteAllLines(iniFilePath, lines);
                    }
                };

                canvasItem.MouseLeave += (sender, e) =>
                {
                    ((Canvas)sender).Background = Brushes.LightGray;
                };

                TextBlock textBlock = new TextBlock();
                textBlock.Text = System.IO.Path.GetFileNameWithoutExtension(file);
                textBlock.Margin = new Thickness(10, 0, 0, 0); // Изменил отступы для TextBlock

                canvasItem.Children.Add(textBlock); // Добавляем TextBlock в Children элемента Canvas
                Stack.Children.Add(canvasItem);
            }

            canvas.Children.Add(closeButton);

        }

        public void GetImageUiElement(Image Image)
        {
            ImageLink = Image;
        }
        public void ChangeImage(Image image, string path, string desktopImage)
        {
            if (type_ == "picture")
            {
                string directoryPath = AppDomain.CurrentDomain.BaseDirectory + @"data\images\" + desktopImage;
                try
                {
                    // Установка источника изображения
                    image.Source = new BitmapImage(new Uri(directoryPath + format, UriKind.Absolute));
                }
                catch (UriFormatException ex)
                {
                    // Обработка исключения, если URI неверен
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

        private void Close()
        {
            ChangeImage(ImageLink, "DesktopImage=", selectedFileName);
            // Удаление всех элементов из canvas
            canvas.Children.Clear();

            // Очистка всех ссылок
            canvas = null;
            closeButton = null;
        }

       


    }




}
