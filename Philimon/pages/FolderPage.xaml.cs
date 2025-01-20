using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для FolderPage.xaml
    /// </summary>
    public partial class FolderPage : Page
    {
        public struct Fuch
        {
            public string[] NameFuch;
            public string[] AdressFuch;
            public string[] IcoAdress;
        }

        Fuch function = new Fuch();

        string ConnectionString = "";

        public FolderPage(string ItemsStatus, string ConnectionString_)
        {
            InitializeComponent();

            ConnectionString = ConnectionString_;

            GenerateUI(ItemsStatus);
        }

        void LoadFuch()
        {
            function.NameFuch = new string[4]; // Инициализация массива с одним элементом
            function.AdressFuch = new string[4]; // Инициализация массива с одним элементом
            function.IcoAdress = new string[4]; // Инициализация массива с одним элементом

            function.NameFuch[0] = "Инфраструктура";
            function.IcoAdress[0] = "pack://application:,,,/sources/images/company_test.png";

            function.NameFuch[1] = "Статус сервера";
            function.IcoAdress[1] = "pack://application:,,,/sources/images/company_test.png";

            function.NameFuch[2] = "Роли";
            function.IcoAdress[2] = "pack://application:,,,/sources/images/company_test.png";

            function.NameFuch[3] = "Потерянные пароль";
            function.IcoAdress[3] = "pack://application:,,,/sources/images/company_test.png";
        }

        void LoadFuncTest()
        {
            function.NameFuch = new string[1]; // Инициализация массива с одним элементом
            function.AdressFuch = new string[1]; // Инициализация массива с одним элементом
            function.IcoAdress = new string[1]; // Инициализация массива с одним элементом

            function.NameFuch[0] = "Тестирование и отладка";
            function.IcoAdress[0] = "pack://application:,,,/sources/images/company_test.png";
        }

        void LoadFuncDevelop()
        {
            function.NameFuch = new string[1]; // Инициализация массива с одним элементом
            function.AdressFuch = new string[1]; // Инициализация массива с одним элементом
            function.IcoAdress = new string[1]; // Инициализация массива с одним элементом

            function.NameFuch[0] = "Проектирование технического задания";
            function.IcoAdress[0] = "pack://application:,,,/sources/images/company_test.png";
        }

        void GenerateUI(string ItemsStatus)
        {

            Canvas MainCanvas = new Canvas();
            MainCanvas.Width = Application.Current.MainWindow.Width;
            MainCanvas.Height = Application.Current.MainWindow.Height;
            Grid.Children.Add(MainCanvas);

            BlurEffect Blur = new BlurEffect();
            Blur.Radius = 0;

            Image BackgroundDesktopPicture = new Image();
            BackgroundDesktopPicture.Width = MainCanvas.Width;
            BackgroundDesktopPicture.Height = MainCanvas.Height;
            BackgroundDesktopPicture.Stretch = Stretch.Fill;
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string iniFilePath = System.IO.Path.Combine(currentDirectory, "settings.ini");

            try
            {
                if (File.Exists(iniFilePath))
                {
                    string[] lines = File.ReadAllLines(iniFilePath);

                    foreach (string line in lines)
                    {
                        // Исправлено на правильный ключ для поиска
                        if (line.StartsWith("DesktopImage="))
                        {
                            // Убедитесь, что длина строки больше, чем длина ключа
                            if (line.Length > "DesktopImage=".Length)
                            {
                                string FileName = line.Substring("DesktopImage=".Length).Trim();
                                // Проверка наличия расширения файла
                                if (!FileName.EndsWith(".png"))
                                {
                                    FileName += ".png"; // Добавляем расширение, если его нет
                                }
                                string filePath = System.IO.Path.Combine(currentDirectory, "data/images", FileName);
                                BackgroundDesktopPicture.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Файл settings.ini не найден.");
                }
            }
            catch (Exception ex)
            {
                // Используйте корректный путь к ресурсу
                BackgroundDesktopPicture.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/desktopTest.jpg", UriKind.Absolute));
            }
            BackgroundDesktopPicture.Effect = Blur;
            MainCanvas.Children.Add(BackgroundDesktopPicture);


            animationOpenPage(Blur, MainCanvas);


            Canvas windowCanvas = new Canvas();
            windowCanvas.Width = 600;
            windowCanvas.Height = 400;
            windowCanvas.Background = Brushes.LightGray;

            Canvas header = new Canvas();
            header.Width = windowCanvas.Width;
            header.Height = 30;
            header.Background = Brushes.Gray;
            header.Margin = new System.Windows.Thickness(0, -30, 0, 0);
            windowCanvas.Children.Add(header);


            Button Close = new Button();
            Close.Width = 30;
            Close.Height = 30;
            Close.Content = "X";
            Close.Background = Brushes.Red;
            Close.Foreground = Brushes.White;
            header.Children.Add(Close);

            Close.Click += (sender, e) =>
            {
                this.NavigationService.Navigate(new DesktopPage());
            };

            ScrollViewer Scroll = new ScrollViewer();
            Scroll.Width = windowCanvas.Width;
            Scroll.Height = windowCanvas.Height;
            Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Scroll.Background = Brushes.LightGray;
            windowCanvas.Children.Add(Scroll);

            StackPanel Stack = new StackPanel();
            Stack.Width = Scroll.Width;
            Stack.Height = Scroll.Height;

            Scroll.Content = Stack;

            windowCanvas.Margin = new Thickness(MainCanvas.Width/2 - windowCanvas.Width/2, MainCanvas.Height/2 - windowCanvas.Height/2, 0, 0);
            MainCanvas.Children.Add(windowCanvas);


            GenerateFuch(Stack, ItemsStatus);
        }


        void animationOpenPage(BlurEffect effect, Canvas MainCanvas)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Start();

            timer.Tick += (sender, e) =>
            {
                if (effect.Radius < 50)
                {
                    effect.Radius += 1;
                }
                if (MainCanvas.Opacity < 0.4)
                {
                    MainCanvas.Opacity += 0.01;
                }
                

                if (effect.Radius >= 50 && MainCanvas.Opacity >= 0.4)
                {
                    timer.Stop();
                }

            };


        }


        void GenerateFuch(StackPanel stack, string type)
        {

            Page[] pagesLinks = null;

            if (type == "admin")
            {
                LoadFuch();

                Page Form_infrastructure = new Form_infrastructure();
                Page ServerStatus = new ServerStatus();
                Page RolesRedact = new RoleRedact();
                Page LostPasswords = new LostPasswords();

                pagesLinks = new Page[] { Form_infrastructure, ServerStatus, RolesRedact, LostPasswords };
            }

            if (type == "tester")
            {
                LoadFuncTest();

                Page Form_Tester = new Form_Tester();

                pagesLinks = new Page[] { Form_Tester };
            }

            if (type == "developer")
            {
                LoadFuncDevelop();

                Page DeveloperPage = new Form_Developer();

                pagesLinks = new Page[] { DeveloperPage };
            }

            Canvas[] ItemCanvas = new Canvas[function.NameFuch.Length];
            TextBlock[] Text = new TextBlock[function.NameFuch.Length];
            Image[] Image = new Image[function.NameFuch.Length];

            for (int i = 0; i < function.NameFuch.Length; i++)
            {
                ItemCanvas[i] = new Canvas();
                ItemCanvas[i].Width = 600;
                ItemCanvas[i].Height = 60;
                ItemCanvas[i].Background = Brushes.Gray;

                Text[i] = new TextBlock();
                Text[i].Text = function.NameFuch[i];
                Text[i].FontSize = 18;
                Text[i].Margin = new Thickness(68, ItemCanvas[i].Height / 2 - 12, 0, 0);
                ItemCanvas[i].Children.Add(Text[i]);

                Image[i] = new Image();
                Image[i].Width = 64;
                Image[i].Height = 64;
                Image[i].Source = new BitmapImage(new Uri(function.IcoAdress[i]));
                ItemCanvas[i].Children.Add(Image[i]);

                int index = i; // сохраняем значение i для использования в замыкании

                ItemCanvas[i].MouseDown += (sender, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed && pagesLinks != null && index < pagesLinks.Length)
                    {
                        this.NavigationService.Navigate(pagesLinks[index]);
                    }
                };

                stack.Children.Add(ItemCanvas[i]);
            }

        }


    }
}
