using Philimon.objects.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using Philimon.objects;
using Philimon.objects.UI.menu;
using MySql.Data.MySqlClient;

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для DesktopPage.xaml
    /// </summary>
    public partial class DesktopPage : Page
    {
        DatabaseData databaseData = new DatabaseData();

        string ConnectionString_ = "";

        public DesktopPage()
        {
            InitializeComponent();
            //Application.Current.MainWindow.Width = 1200;
            //Application.Current.MainWindow.Height = 800;

            

            Application.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized; // Maximize the default

            //Application.Current.MainWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth; // Set the width as the width of the screen
            //this.height = system.windows.systemparameters.primaryScreenHeight; // Set height to the height of the screen

            // Установка минимального размера окна равным текущему разрешению экрана
            Application.Current.MainWindow.MinWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Application.Current.MainWindow.MinHeight = System.Windows.SystemParameters.PrimaryScreenHeight;


            ConnectionString_ = databaseData.ConnectionString;

            GenerateInterface();
        }

        

        void GenerateInterface()
        {

            Canvas MainCanvas = new Canvas();
            MainCanvas.Width = Application.Current.MainWindow.Width;
            MainCanvas.Height = Application.Current.MainWindow.Height;
            MainCanvas.Background = Brushes.White;
            Grid.Children.Add(MainCanvas);

            
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

            MainCanvas.Children.Add(BackgroundDesktopPicture);

            Canvas PanelMissions = new Canvas();
            PanelMissions.Width = Application.Current.MainWindow.Width- 120;
            PanelMissions.Height = 40;
            PanelMissions.Background = Brushes.LightBlue;
            MainCanvas.Children.Add(PanelMissions);
            PanelMissions.Margin = new Thickness(0,0,0,0);
            //Иконка компании
            Canvas PanelCompany = new Canvas();
            PanelCompany.Width = Application.Current.MainWindow.Width - PanelMissions.Width;
            PanelCompany.Height = PanelMissions.Height;
            PanelCompany.Background = Brushes.LightBlue;
            MainCanvas.Children.Add(PanelCompany);
            PanelCompany.Margin = new Thickness(PanelMissions.Width, 0, 0, 0);

            Image CompanyIco = new Image();
            CompanyIco.Width = PanelCompany.Width;
            CompanyIco.Height = PanelCompany.Height;
            CompanyIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/company_test.png"));
            MainCanvas.Children.Add(CompanyIco);
            CompanyIco.Margin = new Thickness(PanelMissions.Width, 0, 0, 0);

            //кнопка Пуск
            Button startButton = new Button();
            startButton.Width = 40;
            startButton.Height = 40;
            startButton.Margin = new Thickness(0,0,0,0);
            PanelMissions.Children.Add(startButton);
                // Создаем изображение
                Image startButtonIcon = new Image();
                startButtonIcon.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/logo.png")); // Укажите путь к изображению
                // Устанавливаем изображение как содержимое кнопки
                startButton.Content = startButtonIcon;

            

            


            //Поиск
            Canvas panelSearch = new Canvas();
            panelSearch.Width = 280;
            panelSearch.Height = 30;
            panelSearch.Background = Brushes.Aquamarine;
            //MainCanvas.Children.Add(panelSearch);
            panelSearch.Margin = new Thickness(0, MainCanvas.Height - (panelSearch.Height * 2) - 8,0,0);

            TextBox SearchFunc = new TextBox();
            SearchFunc.Width = 300;
            SearchFunc.Height = 30;
            //MainCanvas.Children.Add(SearchFunc);
            SearchFunc.Margin = new Thickness(0, MainCanvas.Height - (SearchFunc.Height*2) - 8, 0, 0);

            DispatcherTimer TimerSearchAnim = new DispatcherTimer();
            TimerSearchAnim.Interval = TimeSpan.FromSeconds(0.01);

            Image SearchIco = new Image();
            SearchIco.Width = 28;
            SearchIco.Visibility = Visibility.Hidden;
            SearchIco.Height = 28;
            SearchIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/Loading_ico.png"));
            MainCanvas.Children.Add(SearchIco);
            SearchIco.Margin = new Thickness((SearchFunc.Width + SearchIco.Width) - 18 , (MainCanvas.Height - SearchIco.Height * 2) - 11, 0, 0);

            // Создаем RotateTransform
            RotateTransform rotateTransform = new RotateTransform();
            SearchIco.RenderTransformOrigin = new Point(0.5, 0.5); // Устанавливаем центр вращения в центр объекта
            SearchIco.RenderTransform = rotateTransform;

            // Вращаем объект SearchIco
            DoubleAnimation rotateAnimation = new DoubleAnimation();
            rotateAnimation.From = 0;
            rotateAnimation.To = 360;
            rotateAnimation.Duration = new Duration(TimeSpan.FromSeconds(5)); // Увеличиваем время анимации до 5 секунд
            rotateAnimation.RepeatBehavior = RepeatBehavior.Forever;

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

            SearchFunc.TextChanged += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(SearchFunc.Text))
                {
                    TimerSearchAnim.Start();
                }
            };

            TimerSearchAnim.Tick += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(SearchFunc.Text)) {
                    if (panelSearch.Width < 1200)
                    {
                        panelSearch.Width += 20;
                    }
                    if (panelSearch.Width >= 1200)
                    {
                        SearchIco.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (panelSearch.Width > 280)
                    {
                        SearchIco.Visibility = Visibility.Hidden;
                        panelSearch.Width -= 30;
                    }
                    if (panelSearch.Width <= 280)
                    {
                        SearchIco.Visibility = Visibility.Hidden;
                        TimerSearchAnim.Stop();
                    }
                }

            };


            Icon[] ico = new Icon[3];
            Canvas[] iconCanvas = new Canvas[3];
            string[] texts = { "Настройки", "Текстовый редактор", "О нас" };
            string[] funch = { "settings_func", "NotePad_func", "settings_func" };

            for (int i = 0; i < 3; i++)
            {
                int currentIndex = i; // Создание временной переменной

                ico[i] = new Icon();
                ico[i].Tag = funch[i];
                iconCanvas[i] = ico[i].Icons(60, 60, "pack://application:,,,/sources/images/settings.png", texts[i], funch[i]);
                iconCanvas[i].Margin = new Thickness(0, 60 + i * 68, 0, 0);
                MainCanvas.Children.Add(iconCanvas[i]);

                iconCanvas[i].MouseLeftButtonDown += (sender, e) =>
                {
                    if (e.ClickCount == 2)
                    {
                        if (ico[currentIndex].Tag == "settings_func")
                        {
                            this.NavigationService.Navigate(new settings_page());
                        }
                        if (ico[currentIndex].Tag == "NotePad_func")
                        {
                            this.NavigationService.Navigate(new NotePad(this));
                        }
                    }
                };
            }



            ScrollViewer StartMenuScroll = new ScrollViewer();
            StartMenuScroll.Width = 400;
            StartMenuScroll.Height = 400;
            StartMenuScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            StartMenuScroll.Visibility = Visibility.Hidden;
            StartMenuScroll.Margin = new Thickness(0, 40, 0, 0);
            StartMenuScroll.Background = Brushes.LightGray;
            MainCanvas.Children.Add(StartMenuScroll);

            StackPanel StartMenuItemStack = new StackPanel();
            StartMenuItemStack.Width = StartMenuScroll.Width;
            StartMenuItemStack.Height = StartMenuScroll.Height;
            StartMenuScroll.Content = StartMenuItemStack;



            bool adminF = false, testerF = false, developF = false;
            string positionPeople = "";

            using (MySqlConnection connection = new MySqlConnection(databaseData.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Humans WHERE id = @id"; //получение индекса positionPeople

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            positionPeople = reader["positionPeople"].ToString();
                        }
                    }
                }
            }

            

            //В positionPeople получаю роль
            string role = "";
            using (MySqlConnection connection = new MySqlConnection(databaseData.ConnectionString))
            {
                connection.Open();

                // Предполагаем, что у вас есть переменная positionPeople, содержащая роль для поиска
                string query = "SELECT * FROM positionPeople WHERE NamePosition = @NamePosition"; // Получение индекса positionPeople

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NamePosition", positionPeople);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) {
                            role = reader["role"].ToString();
                        }
                    }
                }
            }

            


            using (MySqlConnection connection = new MySqlConnection(databaseData.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM AllRoles WHERE NameRole = @NameRole"; // Получение индекса positionPeople

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NameRole", role);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Используйте if, если ожидается только одна запись
                        {
                           
                                adminF = (bool)reader["funcView_Admin"];
                                testerF = (bool)reader["funcView_Test"];
                                developF = (bool)reader["funcView_Dev"];
                           
                        }
                    }
                }
            }

            //получаю данные из роли
            if (adminF == true) {
                AdministartorFunc_generate(MainCanvas, StartMenuItemStack);
            }
            if (testerF == true)
            {
                TesterFunc_generate(MainCanvas, StartMenuItemStack);
            }
            if (developF == true)
            {
                DeveloperFunc_generate(MainCanvas, StartMenuItemStack);
            }

            ItemsMissions(MainCanvas, StartMenuItemStack);
            ItemsMenu(MainCanvas, StartMenuItemStack);
            

            startButton.Click += (sender, e) =>
            {
                StartMenuScroll.Visibility = StartMenuScroll.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            };

            Button Exit = new Button();
            Exit.Width = 160;
            Exit.Height = 40;
            Exit.Content = "Выйти из системы";
            MainCanvas.Children.Add(Exit);
            Exit.Margin = new Thickness(0, Application.Current.MainWindow.Height - 120, 0, 0);

            Exit.Click += (sender, e) =>
            {
                Application.Current.MainWindow.MinWidth = 800;
                Application.Current.MainWindow.MinHeight = 500;

                Application.Current.MainWindow.Width = 800;
                Application.Current.MainWindow.Height = 500;

                ((Main)Application.Current.MainWindow).PersonID = -1;
                this.NavigationService.Navigate(new AutrizPage());
            };


        }


        private void AdministartorFunc_generate(Canvas MainCanvas, StackPanel StartMenuItemStack)
        {
            StartMenu_folder folderStartMenu = new StartMenu_folder();
            Canvas folder1 = folderStartMenu.folder(400, 60, "Администрирование", MainCanvas);
            StartMenuItemStack.Children.Add(folder1);


            folder1.MouseDown += (sender, e) =>
            {
                if (e.ClickCount == 1)
                {
                    this.NavigationService.Navigate(new FolderPage("admin", ConnectionString_));
                }

                
            };


            folder1.MouseEnter += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.White;
            };
            folder1.MouseLeave += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.AliceBlue;
            };

        }
        private void TesterFunc_generate(Canvas MainCanvas, StackPanel StartMenuItemStack)
        {
            StartMenu_folder folderStartMenu = new StartMenu_folder();
            Canvas folder1 = folderStartMenu.folder(400, 60, "Тестирование", MainCanvas);
            StartMenuItemStack.Children.Add(folder1);


            folder1.MouseDown += (sender, e) =>
            {
                if (e.ClickCount == 1)
                {
                    this.NavigationService.Navigate(new FolderPage("tester", ConnectionString_));
                }


            };

            folder1.MouseEnter += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.White;
            };
            folder1.MouseLeave += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.AliceBlue;
            };

        }

        private void DeveloperFunc_generate(Canvas MainCanvas, StackPanel StartMenuItemStack)
        {
            StartMenu_folder folderStartMenu = new StartMenu_folder();
            Canvas folder1 = folderStartMenu.folder(400, 60, "Разработка", MainCanvas);
            StartMenuItemStack.Children.Add(folder1);


            folder1.MouseDown += (sender, e) =>
            {
                if (e.ClickCount == 1)
                {
                    this.NavigationService.Navigate(new FolderPage("developer", ConnectionString_));
                }


            };

            folder1.MouseEnter += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.White;
            };
            folder1.MouseLeave += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.AliceBlue;
            };

        }


        private void ItemsMenu(Canvas MainCanvas, StackPanel StartMenuItemStack)
        {
            StartMenu_item StartMenu_item = new StartMenu_item();
            Canvas Item1 = StartMenu_item.Item(400, 60, "Создание задач", MainCanvas, "pack://application:,,,/sources/images/preview.png");
            StartMenuItemStack.Children.Add(Item1);


            Item1.MouseDown += (sender, e) =>
            {
                if (e.ClickCount == 1)
                {
                    MissionForm Form = new MissionForm();

                    MainCanvas.Children.Add(Form.canvas);

                }


            };

            Item1.MouseEnter += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.White;
            };
            Item1.MouseLeave += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.AliceBlue;
            };



        }


        private void ItemsMissions(Canvas MainCanvas, StackPanel StartMenuItemStack)
        {
            StartMenu_item StartMenu_item = new StartMenu_item();
            Canvas Item1 = StartMenu_item.Item(400, 60, "Просмотр задач", MainCanvas, "pack://application:,,,/sources/images/missionIco.png");
            StartMenuItemStack.Children.Add(Item1);


            Item1.MouseDown += (sender, e) =>
            {
                if (e.ClickCount == 1)
                {

                    this.NavigationService.Navigate(new MissionPage());

                }


            };

            Item1.MouseEnter += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.White;
            };
            Item1.MouseLeave += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.AliceBlue;
            };



        }




    }
}
