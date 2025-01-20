using MySql.Data.MySqlClient;
using Philimon.objects;
using Philimon.objects.UI;
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
    /// Логика взаимодействия для settings_page.xaml
    /// </summary>
    public partial class settings_page : Page
    {
        Canvas MainCanvas = new Canvas();

        DatabaseData DB = new DatabaseData();

        string[] settingsTitles = { "Профиль","Экран","Стили","Звуки","Безопасность","Хранилища данных","Справочные материалы","Особые функции", "О системе"};
        string[] icoNamePatch = { "profile_ico.png", "display_ico.png", "styles_ico.png", "sounds_ico.png", "security_ico.png", "data_ico.png", "guide_ico.jpg", "special_func_ico.png", "about_system_ico.png" };

        public settings_page()
        {
            InitializeComponent();

            GenerateInterfaceUi();
        }

        void GenerateInterfaceUi()
        {
            
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

            Canvas SettingsCanvas = new Canvas();
            SettingsCanvas.Width = Application.Current.MainWindow.Width;
            SettingsCanvas.Height = Application.Current.MainWindow.Height;
            SettingsCanvas.Opacity = 0.4;
            SettingsCanvas.Background = Brushes.Black;
            MainCanvas.Children.Add(SettingsCanvas);

            Canvas SettingsCellsCanvas = new Canvas();
            SettingsCellsCanvas.Width = 365;
            SettingsCellsCanvas.Height = Application.Current.MainWindow.Height;
            SettingsCellsCanvas.Opacity = 0.6;
            SettingsCellsCanvas.Background = Brushes.Black;
            SettingsCellsCanvas.Margin = new Thickness(0,0,0,0);
            MainCanvas.Children.Add(SettingsCellsCanvas);

            BlurEffect BlurLogo = new BlurEffect();
            BlurLogo.Radius = 4;

            Canvas TitleCanvas = new Canvas();
            TitleCanvas.Width = 365;
            TitleCanvas.Height = 80;
            TitleCanvas.Background = Brushes.White;
            TitleCanvas.Opacity = 0.8;
            TitleCanvas.Effect = BlurLogo;
            TitleCanvas.Margin = new Thickness(0,0,0,0);
            MainCanvas.Children.Add(TitleCanvas);

            

            Image Logo = new Image();
            Logo.Width = 365;
            Logo.Height = 80;
            Logo.Stretch = Stretch.Fill;
            Logo.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/title_settings.png"));
            Logo.Margin = new Thickness(0, 0, 0, 0);
            MainCanvas.Children.Add(Logo);

            StackPanel stackPanel = new StackPanel();
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            ScrollViewer ScrollList = new ScrollViewer();
            ScrollList.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollList.Width = 365;
            ScrollList.Height = 520;
            ScrollList.Margin = new Thickness(0, 140,0,0);
            ScrollList.Content = stackPanel;
            SettingsCellsCanvas.Children.Add(ScrollList);


            Canvas[] ButtonSetting = new Canvas[settingsTitles.Length];
            Image[] icons = new Image[icoNamePatch.Length];
            TextBlock[] TextButton = new TextBlock[settingsTitles.Length];



            Canvas ContentSettingPanel = new Canvas();
            ContentSettingPanel.Visibility = Visibility.Hidden;
            ContentSettingPanel.Width = Application.Current.MainWindow.Width - 375;
            ContentSettingPanel.Height = Application.Current.MainWindow.Height;
            ContentSettingPanel.Margin = new Thickness(360, 0, 0, 0);
            MainCanvas.Children.Add(ContentSettingPanel);


            ScrollViewer ContentScroll = new ScrollViewer();
            ContentScroll.Width = ContentSettingPanel.Width;
            ContentScroll.Height = ContentSettingPanel.Height;
            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ContentScroll.Margin = new Thickness(360, 0, 0, 0);
            MainCanvas.Children.Add(ContentScroll);


            for (int i = 0; i < settingsTitles.Length; i++)
            {
                ButtonSetting[i] = new Canvas();
                ButtonSetting[i].Width = 360;
                ButtonSetting[i].Height = 60;
                ButtonSetting[i].Opacity = 0.8;
                
                ButtonSetting[i].Background = Brushes.AliceBlue;
                stackPanel.Children.Add(ButtonSetting[i]);

                icons[i] = new Image();
                icons[i].Width = 60;
                icons[i].Height = 60;
                icons[i].Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/"+ icoNamePatch[i]));
                ButtonSetting[i].Children.Add(icons[i]);
                icons[i].Margin = new Thickness(0,0,0,0);

                TextButton[i] = new TextBlock();
                TextButton[i].Text = settingsTitles[i];
                TextButton[i].FontSize = 16;
                ButtonSetting[i].Children.Add(TextButton[i]);
                TextButton[i].Margin = new Thickness(64,60/2- (TextButton[i].FontSize-2), 0 ,0);


                ButtonSetting[i].Tag = i;

                ButtonSetting[i].MouseEnter += (sender, e) =>
                {
                    ((Canvas)sender).Background = Brushes.White;
                    ((Canvas)sender).Opacity = 1;
                };
                ButtonSetting[i].MouseLeave += (sender, e) =>
                {
                    ((Canvas)sender).Background = Brushes.AliceBlue;
                    ((Canvas)sender).Opacity = 0.8;
                };

                

                ButtonSetting[i].MouseDown += (sender, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        ContentSettingPanel.Visibility = Visibility.Visible;
                        int canvasIndex = (int)((Canvas)sender).Tag;

                        ((Canvas)sender).Background = Brushes.Gray;
                        if (canvasIndex == 0)
                        {
                            ContentScroll.Content = Account();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 1)
                        {
                            ContentScroll.Content = WindowSettings();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 2)
                        {
                            ContentScroll.Content = StyleSettingsCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 3)
                        {
                            ContentScroll.Content = SoundSettingsCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 4)
                        {
                            ContentScroll.Content = SecuritySettingsCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 5)
                        {
                            ContentScroll.Content = dataSettingsCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 6)
                        {
                            ContentScroll.Content = GuideSettingsCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 7)
                        {
                            ContentScroll.Content = SpecialFuchSettingsCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 8)
                        {
                            ContentScroll.Content = AboutSystemCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }

                    }
                };


            }

            Canvas ExitButton = new Canvas();
            ExitButton.Width = 365;
            ExitButton.Height = 60;
            ExitButton.Background = Brushes.AliceBlue;
            ExitButton.Opacity = 0.8;
            ExitButton.Margin = new Thickness(0,Application.Current.MainWindow.Height - ((ExitButton.Height * 2) - 30), 0, 0);
            SettingsCellsCanvas.Children.Add(ExitButton);

            Image IcoExit = new Image();
            IcoExit.Width = 60;
            IcoExit.Height = 60;
            IcoExit.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/back.png"));
            IcoExit.Margin = new Thickness(0,0,0,0);
            ExitButton.Children.Add(IcoExit);

            TextBlock exitText = new TextBlock();
            exitText.Text = "Назад...";
            exitText.FontSize = 16;
            ExitButton.Children.Add(exitText);
            exitText.Margin = new Thickness(64, 60 / 2 - (exitText.FontSize - 2), 0, 0);

            ExitButton.MouseEnter += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.Red;
                ((Canvas)sender).Opacity = 1;

                
            };
            ExitButton.MouseLeave += (sender, e) =>
            {
                ((Canvas)sender).Background = Brushes.AliceBlue;
                ((Canvas)sender).Opacity = 0.8;
            };

            ExitButton.MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.NavigationService.Navigate(new DesktopPage());
                }
            };
            

            animationOpenPage(Blur, SettingsCanvas, SettingsCellsCanvas, TitleCanvas, Logo, ScrollList);


            

        }

        

        void animationOpenPage(BlurEffect effect, Canvas MainCanvas, Canvas SettingsCellsCanvas, Canvas TitleCanvas, Image Logo, ScrollViewer ScrollList)
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
                if (SettingsCellsCanvas.Opacity < 0.6)
                {
                    SettingsCellsCanvas.Opacity += 0.01;
                }
                if (TitleCanvas.Opacity < 0.8)
                {
                    TitleCanvas.Opacity += 0.01;
                }
                if (Logo.Opacity < 1)
                {
                    Logo.Opacity += 0.1;
                }
                if (ScrollList.Opacity < 1)
                {
                    ScrollList.Opacity += 0.01;
                }

                if (effect.Radius >= 50 && MainCanvas.Opacity >= 0.4 && SettingsCellsCanvas.Opacity >= 0.6 && TitleCanvas.Opacity >= 0.8 && Logo.Opacity >= 0.9 && ScrollList.Opacity >= 0.9)
                {
                    timer.Stop();
                }

            };


        }


        private Canvas Account()
        {
            string Family = "";
            string Name = "";
            string ThirdName = "";

            string nameUser = "";

            string tagUser = "";

            string DateBeginJob = "";

            string Departament = "";

            string position = "";

            string gender = "";

            string Data_of_born = "";

            string PhoneNumberText = "";

            string mail = "";

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
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
                            Family = reader["Firtsname"].ToString();
                            Name = reader["SecondName"].ToString();
                            ThirdName = reader["ThirdName"].ToString();
                            nameUser = Family + " " + Name + " " + ThirdName + " ";

                            gender = reader["gender"].ToString();

                            DateBeginJob = reader["Data_of_employment"].ToString();
                            Departament = reader["departaments"].ToString();
                            position = reader["positionPeople"].ToString();

                            Data_of_born = reader["Data_of_born"].ToString();

                            PhoneNumberText = reader["numberPhone"].ToString();

                            mail = reader["Email"].ToString();
                        }
                    }
                }
            }
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tagUser = reader["tagUser"].ToString();
                        }
                    }
                }
            }

            string Nickname = nameUser + "\n" + tagUser;

            Canvas accountCanvas = new Canvas();

            
            accountCanvas.Margin = new Thickness(10, 0, 0, 0);


            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "Общая информация:";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(0, -70, 0, 0);
            accountCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 10,
                Y1 = -20,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = -20,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            accountCanvas.Children.Add(line1);


            Ellipse ellipseAcc = new Ellipse()
            {
                Width = 200,
                Height = 200,
                Fill = Brushes.White,
                StrokeThickness = 5,
                StrokeDashArray = new DoubleCollection() { 4, 2 },
                Stroke = Brushes.Black,
                StrokeDashCap = PenLineCap.Round
            };


            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/sources/images/ico_dammy.png"));
            ellipseAcc.Fill = imageBrush;

            accountCanvas.Children.Add(ellipseAcc);
            accountCanvas.Margin = new Thickness(40, 95, 0, 0);


            TextBlock TextBoxNikname = new TextBlock();
            TextBoxNikname.Text = Nickname;
            TextBoxNikname.Width = TextBoxNikname.Text.Length * TextBoxNikname.FontSize + 30;
            TextBoxNikname.Foreground = Brushes.White;
            TextBoxNikname.FontSize = 22;
            accountCanvas.Children.Add(TextBoxNikname);
            TextBoxNikname.Margin = new Thickness(220, 95 - TextBoxNikname.FontSize, 0, 0);
            Button redactButton = new Button();
            redactButton.Width = 30;
            redactButton.Height = 30;
            redactButton.Background = Brushes.Transparent;
            redactButton.BorderBrush = Brushes.Transparent;
            Image IcoRedact = new Image();
            IcoRedact.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображению                                                                                           // Устанавливаем изображение как содержимое кнопки
            redactButton.Content = IcoRedact;
            accountCanvas.Children.Add(redactButton);
            redactButton.Margin = new Thickness(220 + TextBoxNikname.Width + 30, 95 - TextBoxNikname.FontSize, 0, 0);



            TextBlock TextBoxBeginJob = new TextBlock();
            TextBoxBeginJob.Text = "Дата трудоустройства" + ":\n" + DateBeginJob;
            TextBoxBeginJob.Width = TextBoxNikname.Text.Length * TextBoxNikname.FontSize + 30;
            TextBoxBeginJob.Foreground = Brushes.White;
            TextBoxBeginJob.FontSize = 22;
            accountCanvas.Children.Add(TextBoxBeginJob);
            TextBoxBeginJob.Margin = new Thickness(40, 300 - TextBoxBeginJob.FontSize, 0, 0);
            Button BeginJobredactButton = new Button();
            BeginJobredactButton.Width = 30;
            BeginJobredactButton.Height = 30;
            BeginJobredactButton.Background = Brushes.Transparent;
            BeginJobredactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact2 = new Image();
            BeginJobredactButton.Content = IcoRedact2;
            IcoRedact2.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(BeginJobredactButton);
            BeginJobredactButton.Margin = new Thickness(40 + TextBoxBeginJob.Width + 30, 300 - TextBoxBeginJob.FontSize + 4, 0, 0);

            BeginJobredactButton.Click += (sender, e) =>
            {
                Canvas Form = new Canvas();
                Form.Width = 500;
                Form.Height = 300;
                Form.Background = Brushes.White;
                Form.Margin = new Thickness(500, 280, 0, 0);
                MainCanvas.Children.Add(Form);

                Button Save = new Button();
                Save.Width = 150;
                Save.Height = 20;
                Save.Content = "Сохранить";
                Save.Margin = new Thickness(100, 270, 0, 0);
                Form.Children.Add(Save);

                Button Close = new Button();
                Close.Width = 150;
                Close.Height = 20;
                Close.Content = "Закрыть";
                Close.Margin = new Thickness(260, 270, 0, 0);
                Form.Children.Add(Close);

                Close.Click += (sender2, e2) =>
                {
                    MainCanvas.Children.Remove(Form);
                };

                TextBox NewDate = new TextBox();
                NewDate.Width = 270;
                NewDate.Height = 38;
                NewDate.Tag = "Новые данные:";
                NewDate.Margin = new Thickness(100, 100, 0, 0);
                Form.Children.Add(NewDate);

                NewDate.Text = DateBeginJob;

                Save.Click += (sender1, e1) =>
                {
                    DateBeginJob = NewDate.Text;
                    TextBoxBeginJob.Text = "Дата трудоустройства" + ":\n" + DateBeginJob;

                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();


                        string query = "UPDATE Humans SET Data_of_employment = @Data_of_employment WHERE id = @id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            
                            command.Parameters.AddWithValue("@Data_of_employment", DateBeginJob);
                            command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);
                            
                            command.ExecuteNonQuery();
                            
                        }
                    }

                    MainCanvas.Children.Remove(Form);
                };

            };

            TextBlock TextBoxOtdel= new TextBlock();
            TextBoxOtdel.Text = "Отдел" + ":" + Departament;
            TextBoxOtdel.Width = TextBoxNikname.Text.Length * TextBoxNikname.FontSize + 30;
            TextBoxOtdel.Foreground = Brushes.White;
            TextBoxOtdel.FontSize = 22;
            accountCanvas.Children.Add(TextBoxOtdel);
            TextBoxOtdel.Margin = new Thickness(40, 505 - TextBoxOtdel.FontSize, 0, 0);
            Button OtdelredactButton = new Button();
            OtdelredactButton.Width = 30;
            OtdelredactButton.Height = 30;
            OtdelredactButton.Background = Brushes.Transparent;
            OtdelredactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact3 = new Image();
            OtdelredactButton.Content = IcoRedact3;
            IcoRedact3.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(OtdelredactButton);
            OtdelredactButton.Margin = new Thickness(40 + TextBoxOtdel.Width + 30, 505 - TextBoxOtdel.FontSize + 4, 0, 0);

            OtdelredactButton.Click += (sender, e) =>
            {
                Canvas Form = new Canvas();
                Form.Width = 500;
                Form.Height = 300;
                Form.Background = Brushes.White;
                Form.Margin = new Thickness(500, 280, 0, 0);
                MainCanvas.Children.Add(Form);

                Button Save = new Button();
                Save.Width = 150;
                Save.Height = 20;
                Save.Content = "Сохранить";
                Save.Margin = new Thickness(100, 270, 0, 0);
                Form.Children.Add(Save);

                Button Close = new Button();
                Close.Width = 150;
                Close.Height = 20;
                Close.Content = "Закрыть";
                Close.Margin = new Thickness(260, 270, 0, 0);
                Form.Children.Add(Close);

                Close.Click += (sender2, e2) =>
                {
                    MainCanvas.Children.Remove(Form);
                };

                TextBox NewDate = new TextBox();
                NewDate.Width = 270;
                NewDate.Height = 38;
                NewDate.Tag = "Новые данные:";
                NewDate.Margin = new Thickness(100, 100, 0, 0);
                Form.Children.Add(NewDate);

                NewDate.Text = Departament;

                Save.Click += (sender1, e1) =>
                {
                    Departament = NewDate.Text;
                    TextBoxOtdel.Text = "Отдел" + ":" + Departament;

                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();


                        string query = "UPDATE Humans SET departaments = @departaments WHERE id = @id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@departaments", Departament);
                            command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);

                            command.ExecuteNonQuery();

                        }
                    }

                    MainCanvas.Children.Remove(Form);
                };

            };

            TextBlock TextBoxWorkPosition = new TextBlock();
            TextBoxWorkPosition.Text = "Должность" + ":" + position;
            TextBoxWorkPosition.Width = TextBoxWorkPosition.Text.Length * TextBoxWorkPosition.FontSize + 30;
            TextBoxWorkPosition.Foreground = Brushes.White;
            TextBoxWorkPosition.FontSize = 22;
            accountCanvas.Children.Add(TextBoxWorkPosition);
            TextBoxWorkPosition.Margin = new Thickness(40, 710 - TextBoxWorkPosition.FontSize, 0, 0);
            Button WorkPositionredactButton = new Button();
            WorkPositionredactButton.Width = 30;
            WorkPositionredactButton.Height = 30;
            WorkPositionredactButton.Background = Brushes.Transparent;
            WorkPositionredactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact4 = new Image();
            WorkPositionredactButton.Content = IcoRedact4;
            IcoRedact4.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(WorkPositionredactButton);
            WorkPositionredactButton.Margin = new Thickness(40 + TextBoxWorkPosition.Width + 30, 710 - TextBoxWorkPosition.FontSize + 4, 0, 0);

            WorkPositionredactButton.Click += (sender, e) =>
            {
                Canvas Form = new Canvas();
                Form.Width = 500;
                Form.Height = 300;
                Form.Background = Brushes.White;
                Form.Margin = new Thickness(500, 280, 0, 0);
                MainCanvas.Children.Add(Form);

                Button Save = new Button();
                Save.Width = 150;
                Save.Height = 20;
                Save.Content = "Сохранить";
                Save.Margin = new Thickness(100, 270, 0, 0);
                Form.Children.Add(Save);

                Button Close = new Button();
                Close.Width = 150;
                Close.Height = 20;
                Close.Content = "Закрыть";
                Close.Margin = new Thickness(260, 270, 0, 0);
                Form.Children.Add(Close);

                Close.Click += (sender2, e2) =>
                {
                    MainCanvas.Children.Remove(Form);
                };

                TextBox NewDate = new TextBox();
                NewDate.Width = 270;
                NewDate.Height = 38;
                NewDate.Tag = "Новые данные:";
                NewDate.Margin = new Thickness(100, 100, 0, 0);
                Form.Children.Add(NewDate);

                NewDate.Text = position;

                Save.Click += (sender1, e1) =>
                {
                    position = NewDate.Text;
                    TextBoxWorkPosition.Text = "Должность" + ":" + position;

                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();


                        string query = "UPDATE Humans SET positionPeople = @positionPeople WHERE id = @id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@positionPeople", position);
                            command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);

                            command.ExecuteNonQuery();

                        }
                    }

                    MainCanvas.Children.Remove(Form);
                };

            };


            TextBlock TitleSetting_2 = new TextBlock();
            TitleSetting_2.Text = "Личная информация:";
            TitleSetting_2.FontSize = 26;
            TitleSetting_2.Foreground = Brushes.White;
            TitleSetting_2.Margin = new Thickness(0, 915, 0, 0);
            accountCanvas.Children.Add(TitleSetting_2);
            Line line2 = new Line()
            {
                X1 = 10,
                Y1 = 965,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 965,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            accountCanvas.Children.Add(line2);


            TextBlock TextBoxFirstname= new TextBlock();
            TextBoxFirstname.Height = 30;
            TextBoxFirstname.Text = "Имя: " + Name;
            TextBoxFirstname.Width = TextBoxFirstname.Text.Length * TextBoxFirstname.FontSize + 30;
            TextBoxFirstname.Foreground = Brushes.White;
            TextBoxFirstname.FontSize = 22;
            accountCanvas.Children.Add(TextBoxFirstname);
            TextBoxFirstname.Margin = new Thickness(40, 1120 - TextBoxFirstname.FontSize, 0, 0);
            Button redactButtonFirstname = new Button();
            redactButtonFirstname.Width = 30;
            redactButtonFirstname.Height = 30;
            redactButtonFirstname.Background = Brushes.Transparent;
            redactButtonFirstname.BorderBrush = Brushes.Transparent;
            Image IcoRedact5 = new Image();
            IcoRedact5.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображению                                                                                           // Устанавливаем изображение как содержимое кнопки
            redactButtonFirstname.Content = IcoRedact5;
            accountCanvas.Children.Add(redactButtonFirstname);
            redactButtonFirstname.Margin = new Thickness(40 + TextBoxFirstname.Width + 30, 1120 - TextBoxFirstname.FontSize + 4, 0, 0);

            redactButtonFirstname.Click += (sender, e) =>
            {
                Canvas Form = new Canvas();
                Form.Width = 500;
                Form.Height = 300;
                Form.Background = Brushes.White;
                Form.Margin = new Thickness(500, 280, 0, 0);
                MainCanvas.Children.Add(Form);

                Button Save = new Button();
                Save.Width = 150;
                Save.Height = 20;
                Save.Content = "Сохранить";
                Save.Margin = new Thickness(100, 270, 0, 0);
                Form.Children.Add(Save);

                Button Close = new Button();
                Close.Width = 150;
                Close.Height = 20;
                Close.Content = "Закрыть";
                Close.Margin = new Thickness(260, 270, 0, 0);
                Form.Children.Add(Close);

                Close.Click += (sender2, e2) =>
                {
                    MainCanvas.Children.Remove(Form);
                };

                TextBox NewDate = new TextBox();
                NewDate.Width = 270;
                NewDate.Height = 38;
                NewDate.Tag = "Новые данные:";
                NewDate.Margin = new Thickness(100, 100, 0, 0);
                Form.Children.Add(NewDate);

                NewDate.Text = Name;

                Save.Click += (sender1, e1) =>
                {
                    Name = NewDate.Text;
                    TextBoxFirstname.Text = "Имя: " + Name;

                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();


                        string query = "UPDATE Humans SET SecondName = @SecondName WHERE id = @id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@SecondName", Name);
                            command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);

                            command.ExecuteNonQuery();

                        }
                    }

                    MainCanvas.Children.Remove(Form);
                };

            };


            TextBlock TextBoxSenondname = new TextBlock();
            TextBoxSenondname.Text = "Фамилия:" + Family;
            TextBoxSenondname.Width = TextBoxSenondname.Text.Length * TextBoxSenondname.FontSize + 30;
            TextBoxSenondname.Foreground = Brushes.White;
            TextBoxSenondname.FontSize = 22;
            accountCanvas.Children.Add(TextBoxSenondname);
            TextBoxSenondname.Margin = new Thickness(40, 1325 - TextBoxSenondname.FontSize, 0, 0);
            Button SenondnameredactButton = new Button();
            SenondnameredactButton.Width = 30;
            SenondnameredactButton.Height = 30;
            SenondnameredactButton.Background = Brushes.Transparent;
            SenondnameredactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact6 = new Image();
            SenondnameredactButton.Content = IcoRedact6;
            IcoRedact6.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(SenondnameredactButton);
            SenondnameredactButton.Margin = new Thickness(40 + TextBoxSenondname.Width + 30, 1325 - TextBoxSenondname.FontSize + 4, 0, 0);

            SenondnameredactButton.Click += (sender, e) =>
            {
                Canvas Form = new Canvas();
                Form.Width = 500;
                Form.Height = 300;
                Form.Background = Brushes.White;
                Form.Margin = new Thickness(500, 280, 0, 0);
                MainCanvas.Children.Add(Form);

                Button Save = new Button();
                Save.Width = 150;
                Save.Height = 20;
                Save.Content = "Сохранить";
                Save.Margin = new Thickness(100, 270, 0, 0);
                Form.Children.Add(Save);

                Button Close = new Button();
                Close.Width = 150;
                Close.Height = 20;
                Close.Content = "Закрыть";
                Close.Margin = new Thickness(260, 270, 0, 0);
                Form.Children.Add(Close);

                Close.Click += (sender2, e2) =>
                {
                    MainCanvas.Children.Remove(Form);
                };

                TextBox NewDate = new TextBox();
                NewDate.Width = 270;
                NewDate.Height = 38;
                NewDate.Tag = "Новые данные:";
                NewDate.Margin = new Thickness(100, 100, 0, 0);
                Form.Children.Add(NewDate);

                NewDate.Text = Family;

                Save.Click += (sender1, e1) =>
                {
                    Family = NewDate.Text;
                    TextBoxSenondname.Text = "Фамилия:" + Family;

                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();


                        string query = "UPDATE Humans SET Firtsname = @Firtsname WHERE id = @id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@Firtsname", Family);
                            command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);

                            command.ExecuteNonQuery();

                        }
                    }

                    MainCanvas.Children.Remove(Form);
                };

            };

            TextBlock TextBoxThirdname = new TextBlock();
            TextBoxThirdname.Text = "Отчество:" + ThirdName;
            TextBoxThirdname.Width = TextBoxThirdname.Text.Length * TextBoxThirdname.FontSize + 30;
            TextBoxThirdname.Foreground = Brushes.White;
            TextBoxThirdname.FontSize = 22;
            accountCanvas.Children.Add(TextBoxThirdname);
            TextBoxThirdname.Margin = new Thickness(40, 1530 - TextBoxThirdname.FontSize, 0, 0);
            Button ThirdnameredactButton = new Button();
            ThirdnameredactButton.Width = 30;
            ThirdnameredactButton.Height = 30;
            ThirdnameredactButton.Background = Brushes.Transparent;
            ThirdnameredactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact7 = new Image();
            ThirdnameredactButton.Content = IcoRedact7;
            IcoRedact7.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(ThirdnameredactButton);
            ThirdnameredactButton.Margin = new Thickness(40 + TextBoxThirdname.Width + 30, 1530 - TextBoxThirdname.FontSize + 4, 0, 0);

            ThirdnameredactButton.Click += (sender, e) =>
            {
                Canvas Form = new Canvas();
                Form.Width = 500;
                Form.Height = 300;
                Form.Background = Brushes.White;
                Form.Margin = new Thickness(500, 280, 0, 0);
                MainCanvas.Children.Add(Form);

                Button Save = new Button();
                Save.Width = 150;
                Save.Height = 20;
                Save.Content = "Сохранить";
                Save.Margin = new Thickness(100, 270, 0, 0);
                Form.Children.Add(Save);

                Button Close = new Button();
                Close.Width = 150;
                Close.Height = 20;
                Close.Content = "Закрыть";
                Close.Margin = new Thickness(260, 270, 0, 0);
                Form.Children.Add(Close);

                Close.Click += (sender2, e2) =>
                {
                    MainCanvas.Children.Remove(Form);
                };

                TextBox NewDate = new TextBox();
                NewDate.Width = 270;
                NewDate.Height = 38;
                NewDate.Tag = "Новые данные:";
                NewDate.Margin = new Thickness(100, 100, 0, 0);
                Form.Children.Add(NewDate);

                NewDate.Text = ThirdName;

                Save.Click += (sender1, e1) =>
                {
                    ThirdName = NewDate.Text;
                    TextBoxThirdname.Text = "Отчество:" + ThirdName;

                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();


                        string query = "UPDATE Humans SET ThirdName = @ThirdName WHERE id = @id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@ThirdName", ThirdName);
                            command.Parameters.AddWithValue("@id", ((Main)Application.Current.MainWindow).PersonID);

                            command.ExecuteNonQuery();

                        }
                    }

                    MainCanvas.Children.Remove(Form);
                };

            };


            TextBlock SexTitle = new TextBlock();
            SexTitle.Text = "Ваш пол:";
            SexTitle.Foreground = Brushes.White;
            SexTitle.FontSize = 22;
            accountCanvas.Children.Add(SexTitle);
            SexTitle.Margin = new Thickness(40, 1735 - TextBoxThirdname.FontSize, 0, 0);
            ComboBox SexItems = new ComboBox();
            SexItems.IsEnabled = false;
            SexItems.Items.Add(gender);
            SexItems.SelectedIndex = 0;
            SexItems.Width = 200;
            SexItems.Height = 25;
            SexItems.Margin = new Thickness(40, 1800 - TextBoxThirdname.FontSize + SexTitle.FontSize/2, 0, 0);
            accountCanvas.Children.Add(SexItems);



            TextBlock TextBoxDataBorn = new TextBlock();
            TextBoxDataBorn.Text = "Дата рождения" + ":\n" + Data_of_born;
            TextBoxDataBorn.Width = TextBoxDataBorn.Text.Length * TextBoxDataBorn.FontSize + 30;
            TextBoxDataBorn.Foreground = Brushes.White;
            TextBoxDataBorn.FontSize = 22;
            accountCanvas.Children.Add(TextBoxDataBorn);
            TextBoxDataBorn.Margin = new Thickness(40, 2005 - TextBoxDataBorn.FontSize, 0, 0);
            Button DataBornredactButton = new Button();
            DataBornredactButton.Width = 30;
            DataBornredactButton.Height = 30;
            DataBornredactButton.Background = Brushes.Transparent;
            DataBornredactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact8 = new Image();
            DataBornredactButton.Content = IcoRedact8;
            IcoRedact8.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(DataBornredactButton);
            DataBornredactButton.Margin = new Thickness(40 + TextBoxDataBorn.Width + 30, 2005 - TextBoxDataBorn.FontSize + 4, 0, 0);


            TextBlock TitleSetting_3 = new TextBlock();
            TitleSetting_3.Text = "Связь со мной:";
            TitleSetting_3.FontSize = 26;
            TitleSetting_3.Foreground = Brushes.White;
            TitleSetting_3.Margin = new Thickness(0, 2210, 0, 0);
            accountCanvas.Children.Add(TitleSetting_3);
            Line line3 = new Line()
            {
                X1 = 10,
                Y1 = 2260,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 2260,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            accountCanvas.Children.Add(line3);


            TextBlock PhoneTitle = new TextBlock();
            PhoneTitle.Text = "Мой телефон:";
            PhoneTitle.Width = PhoneTitle.Text.Length * PhoneTitle.FontSize + 30;
            PhoneTitle.Foreground = Brushes.White;
            PhoneTitle.FontSize = 22;
            accountCanvas.Children.Add(PhoneTitle);
            PhoneTitle.Margin = new Thickness(0, 2365, 0, 0);
            Image telefonIco = new Image();
            telefonIco.Width = 30;
            telefonIco.Height = 30;
            telefonIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/telefon_ico.png"));
            accountCanvas.Children.Add(telefonIco);
            telefonIco.Margin = new Thickness(0, 2400, 0, 0);
            TextBlock PhoneNumber = new TextBlock();
            PhoneNumber.Text = PhoneNumberText;
            PhoneNumber.Width = PhoneNumber.Text.Length * PhoneNumber.FontSize + 30;
            PhoneNumber.Foreground = Brushes.White;
            PhoneNumber.FontSize = 22;
            accountCanvas.Children.Add(PhoneNumber);
            PhoneNumber.Margin = new Thickness(40, 2400, 0, 0);
            Button PhoneNumberRedactButton = new Button();
            PhoneNumberRedactButton.Width = 30;
            PhoneNumberRedactButton.Height = 30;
            PhoneNumberRedactButton.Background = Brushes.Transparent;
            PhoneNumberRedactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact9 = new Image();
            PhoneNumberRedactButton.Content = IcoRedact9;
            IcoRedact9.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(PhoneNumberRedactButton);
            PhoneNumberRedactButton.Margin = new Thickness(40 + PhoneTitle.Width + 30, 2400 - PhoneTitle.FontSize, 0, 0);
            StackPanel Stack1 = new StackPanel();
            Stack1.Width = 300;
            Stack1.Height = 50;
            Stack1.HorizontalAlignment = HorizontalAlignment.Left;
            accountCanvas.Children.Add(Stack1);
            Stack1.Margin = new Thickness(10, 2450, 0, 0);

            RadioButton HidePhone = new RadioButton();
            HidePhone.HorizontalAlignment = HorizontalAlignment.Left;
            HidePhone.Content = "Скрыть от всех";
            HidePhone.Foreground = Brushes.White;
            HidePhone.Width = HidePhone.Content.ToString().Length * HidePhone.FontSize;
            Stack1.Children.Add(HidePhone);
            RadioButton ShowPone = new RadioButton();
            ShowPone.HorizontalAlignment = HorizontalAlignment.Left;
            ShowPone.Content = "Показать всем";
            ShowPone.Foreground = Brushes.White;
            ShowPone.Width = ShowPone.Content.ToString().Length * ShowPone.FontSize;
            Stack1.Children.Add(ShowPone);
            RadioButton OnlyFriends = new RadioButton();
            OnlyFriends.HorizontalAlignment = HorizontalAlignment.Left;
            OnlyFriends.Content = "Показать только отделу";
            OnlyFriends.Foreground = Brushes.White;
            OnlyFriends.Width = OnlyFriends.Content.ToString().Length * OnlyFriends.FontSize;
            Stack1.Children.Add(OnlyFriends);



            TextBlock EmailTitle = new TextBlock();
            EmailTitle.Text = "Моя электронная почта:";
            EmailTitle.Width = EmailTitle.Text.Length * EmailTitle.FontSize + 30;
            EmailTitle.Foreground = Brushes.White;
            EmailTitle.FontSize = 22;
            accountCanvas.Children.Add(EmailTitle);
            EmailTitle.Margin = new Thickness(0, 2565, 0, 0);
            Image EmailIco = new Image();
            EmailIco.Width = 30;
            EmailIco.Height = 30;
            EmailIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/email_ico.png"));
            accountCanvas.Children.Add(EmailIco);
            EmailIco.Margin = new Thickness(0, 2600, 0, 0);
            TextBlock EmailAdress = new TextBlock();
            EmailAdress.Text = mail;
            EmailAdress.Width = EmailAdress.Text.Length * EmailAdress.FontSize + 30;
            EmailAdress.Foreground = Brushes.White;
            EmailAdress.FontSize = 22;
            accountCanvas.Children.Add(EmailAdress);
            EmailAdress.Margin = new Thickness(40, 2600, 0, 0);
            Button EmailAdressRedactButton = new Button();
            EmailAdressRedactButton.Width = 30;
            EmailAdressRedactButton.Height = 30;
            EmailAdressRedactButton.Background = Brushes.Transparent;
            EmailAdressRedactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact10 = new Image();
            EmailAdressRedactButton.Content = IcoRedact10;
            IcoRedact10.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(EmailAdressRedactButton);
            EmailAdressRedactButton.Margin = new Thickness(40 + EmailTitle.Width + 30, 2600 - EmailTitle.FontSize, 0, 0);
            StackPanel Stack2 = new StackPanel();
            Stack2.Width = 300;
            Stack2.Height = 50;
            Stack2.HorizontalAlignment = HorizontalAlignment.Left;
            accountCanvas.Children.Add(Stack2);
            Stack2.Margin = new Thickness(10, 2650, 0, 0);

            RadioButton HideEmail = new RadioButton();
            HideEmail.HorizontalAlignment = HorizontalAlignment.Left;
            HideEmail.Content = "Скрыть от всех";
            HideEmail.Foreground = Brushes.White;
            HideEmail.Width = HideEmail.Content.ToString().Length * HideEmail.FontSize;
            Stack2.Children.Add(HideEmail);
            RadioButton ShowEmail = new RadioButton();
            ShowEmail.HorizontalAlignment = HorizontalAlignment.Left;
            ShowEmail.Content = "Показать всем";
            ShowEmail.Foreground = Brushes.White;
            ShowEmail.Width = ShowEmail.Content.ToString().Length * ShowEmail.FontSize;
            Stack2.Children.Add(ShowEmail);
            RadioButton OnlyEmail = new RadioButton();
            OnlyEmail.HorizontalAlignment = HorizontalAlignment.Left;
            OnlyEmail.Content = "Показать только отделу";
            OnlyEmail.Foreground = Brushes.White;
            OnlyEmail.Width = OnlyEmail.Content.ToString().Length * OnlyEmail.FontSize;
            Stack2.Children.Add(OnlyEmail);


            TextBlock VK_title = new TextBlock();
            VK_title.Text = "Мой VK:";
            VK_title.Width = VK_title.Text.Length * VK_title.FontSize + 30;
            VK_title.Foreground = Brushes.White;
            VK_title.FontSize = 22;
            accountCanvas.Children.Add(VK_title);
            VK_title.Margin = new Thickness(0, 2765, 0, 0);
            Image VKIco = new Image();
            VKIco.Width = 30;
            VKIco.Height = 30;
            VKIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/vk_ico.png"));
            accountCanvas.Children.Add(VKIco);
            VKIco.Margin = new Thickness(0, 2800, 0, 0);
            TextBlock VKAdress = new TextBlock();
            VKAdress.Text = "Не указан";
            VKAdress.Width = VKAdress.Text.Length * VKAdress.FontSize + 30;
            VKAdress.Foreground = Brushes.White;
            VKAdress.FontSize = 22;
            accountCanvas.Children.Add(VKAdress);
            VKAdress.Margin = new Thickness(40, 2800, 0, 0);
            Button VKAdressRedactButton = new Button();
            VKAdressRedactButton.Width = 30;
            VKAdressRedactButton.Height = 30;
            VKAdressRedactButton.Background = Brushes.Transparent;
            VKAdressRedactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact11 = new Image();
            VKAdressRedactButton.Content = IcoRedact11;
            IcoRedact11.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(VKAdressRedactButton);
            VKAdressRedactButton.Margin = new Thickness(40 + VK_title.Width + 30, 2800 - VK_title.FontSize, 0, 0);
            StackPanel Stack3 = new StackPanel();
            Stack3.Width = 300;
            Stack3.Height = 50;
            Stack3.HorizontalAlignment = HorizontalAlignment.Left;
            accountCanvas.Children.Add(Stack3);
            Stack3.Margin = new Thickness(10, 2850, 0, 0);

            RadioButton HideVK = new RadioButton();
            HideVK.HorizontalAlignment = HorizontalAlignment.Left;
            HideVK.Content = "Скрыть от всех";
            HideVK.Foreground = Brushes.White;
            HideVK.Width = HideVK.Content.ToString().Length * HideVK.FontSize;
            Stack3.Children.Add(HideVK);
            RadioButton ShowVK = new RadioButton();
            ShowVK.HorizontalAlignment = HorizontalAlignment.Left;
            ShowVK.Content = "Показать всем";
            ShowVK.Foreground = Brushes.White;
            ShowVK.Width = ShowVK.Content.ToString().Length * ShowVK.FontSize;
            Stack3.Children.Add(ShowVK);
            RadioButton OnlyVK = new RadioButton();
            OnlyVK.HorizontalAlignment = HorizontalAlignment.Left;
            OnlyVK.Content = "Показать только отделу";
            OnlyVK.Foreground = Brushes.White;
            OnlyVK.Width = OnlyVK.Content.ToString().Length * OnlyVK.FontSize;
            Stack3.Children.Add(OnlyVK);




            TextBlock Telegram_title = new TextBlock();
            Telegram_title.Text = "Мой Telegram:";
            Telegram_title.Width = Telegram_title.Text.Length * Telegram_title.FontSize + 30;
            Telegram_title.Foreground = Brushes.White;
            Telegram_title.FontSize = 22;
            accountCanvas.Children.Add(Telegram_title);
            Telegram_title.Margin = new Thickness(0, 2965, 0, 0);
            Image TelegramIco = new Image();
            TelegramIco.Width = 30;
            TelegramIco.Height = 30;
            TelegramIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/telegram_ico.png"));
            accountCanvas.Children.Add(TelegramIco);
            TelegramIco.Margin = new Thickness(0, 3000, 0, 0);
            TextBlock TelegramAdress = new TextBlock();
            TelegramAdress.Text = "Не указан";
            TelegramAdress.Width = TelegramAdress.Text.Length * TelegramAdress.FontSize + 30;
            TelegramAdress.Foreground = Brushes.White;
            TelegramAdress.FontSize = 22;
            accountCanvas.Children.Add(TelegramAdress);
            TelegramAdress.Margin = new Thickness(40, 3000, 0, 0);
            Button TelegramAdressRedactButton = new Button();
            TelegramAdressRedactButton.Width = 30;
            TelegramAdressRedactButton.Height = 30;
            TelegramAdressRedactButton.Background = Brushes.Transparent;
            TelegramAdressRedactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact12 = new Image();
            TelegramAdressRedactButton.Content = IcoRedact12;
            IcoRedact12.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(TelegramAdressRedactButton);
            TelegramAdressRedactButton.Margin = new Thickness(40 + Telegram_title.Width + 30, 3000 - Telegram_title.FontSize, 0, 0);
            StackPanel Stack4 = new StackPanel();
            Stack4.Width = 300;
            Stack4.Height = 50;
            Stack4.HorizontalAlignment = HorizontalAlignment.Left;
            accountCanvas.Children.Add(Stack4);
            Stack4.Margin = new Thickness(10, 3050, 0, 0);

            RadioButton HideTelegram = new RadioButton();
            HideTelegram.HorizontalAlignment = HorizontalAlignment.Left;
            HideTelegram.Content = "Скрыть от всех";
            HideTelegram.Foreground = Brushes.White;
            HideTelegram.Width = HideTelegram.Content.ToString().Length * HideTelegram.FontSize;
            Stack4.Children.Add(HideTelegram);
            RadioButton ShowTelegram = new RadioButton();
            ShowTelegram.HorizontalAlignment = HorizontalAlignment.Left;
            ShowTelegram.Content = "Показать всем";
            ShowTelegram.Foreground = Brushes.White;
            ShowTelegram.Width = ShowTelegram.Content.ToString().Length * ShowTelegram.FontSize;
            Stack4.Children.Add(ShowTelegram);
            RadioButton OnlyTelegram = new RadioButton();
            OnlyTelegram.HorizontalAlignment = HorizontalAlignment.Left;
            OnlyTelegram.Content = "Показать только отделу";
            OnlyTelegram.Foreground = Brushes.White;
            OnlyTelegram.Width = OnlyTelegram.Content.ToString().Length * OnlyTelegram.FontSize;
            Stack4.Children.Add(OnlyTelegram);





            TextBlock WhatsApp_title = new TextBlock();
            WhatsApp_title.Text = "Мой WhatsApp:";
            WhatsApp_title.Width = WhatsApp_title.Text.Length * WhatsApp_title.FontSize + 30;
            WhatsApp_title.Foreground = Brushes.White;
            WhatsApp_title.FontSize = 22;
            accountCanvas.Children.Add(WhatsApp_title);
            WhatsApp_title.Margin = new Thickness(0, 3150, 0, 0);
            Image WhatsAppIco = new Image();
            WhatsAppIco.Width = 30;
            WhatsAppIco.Height = 30;
            WhatsAppIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/whatsApp_ico.png"));
            accountCanvas.Children.Add(WhatsAppIco);
            WhatsAppIco.Margin = new Thickness(0, 3185, 0, 0);
            TextBlock WhatsAppAdress = new TextBlock();
            WhatsAppAdress.Text = "Не указан";
            WhatsAppAdress.Width = WhatsAppAdress.Text.Length * WhatsAppAdress.FontSize + 30;
            WhatsAppAdress.Foreground = Brushes.White;
            WhatsAppAdress.FontSize = 22;
            accountCanvas.Children.Add(WhatsAppAdress);
            WhatsAppAdress.Margin = new Thickness(40, 3185, 0, 0);
            Button WhatsAppAdressRedactButton = new Button();
            WhatsAppAdressRedactButton.Width = 30;
            WhatsAppAdressRedactButton.Height = 30;
            WhatsAppAdressRedactButton.Background = Brushes.Transparent;
            WhatsAppAdressRedactButton.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact13 = new Image();
            WhatsAppAdressRedactButton.Content = IcoRedact13;
            IcoRedact13.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            accountCanvas.Children.Add(WhatsAppAdressRedactButton);
            WhatsAppAdressRedactButton.Margin = new Thickness(40 + WhatsApp_title.Width + 30, 3200 - WhatsApp_title.FontSize, 0, 0);
            StackPanel Stack5 = new StackPanel();
            Stack5.Width = 300;
            Stack5.Height = 50;
            Stack5.HorizontalAlignment = HorizontalAlignment.Left;
            accountCanvas.Children.Add(Stack5);
            Stack5.Margin = new Thickness(10, 3230, 0, 0);

            RadioButton HideWhatsApp = new RadioButton();
            HideWhatsApp.HorizontalAlignment = HorizontalAlignment.Left;
            HideWhatsApp.Content = "Скрыть от всех";
            HideWhatsApp.Foreground = Brushes.White;
            HideWhatsApp.Width = HideWhatsApp.Content.ToString().Length * HideWhatsApp.FontSize;
            Stack5.Children.Add(HideWhatsApp);
            RadioButton ShowWhatsApp = new RadioButton();
            ShowWhatsApp.HorizontalAlignment = HorizontalAlignment.Left;
            ShowWhatsApp.Content = "Показать всем";
            ShowWhatsApp.Foreground = Brushes.White;
            ShowWhatsApp.Width = ShowWhatsApp.Content.ToString().Length * ShowWhatsApp.FontSize;
            Stack5.Children.Add(ShowWhatsApp);
            RadioButton OnlyWhatsApp = new RadioButton();
            OnlyWhatsApp.HorizontalAlignment = HorizontalAlignment.Left;
            OnlyWhatsApp.Content = "Показать только отделу";
            OnlyWhatsApp.Foreground = Brushes.White;
            OnlyWhatsApp.Width = OnlyWhatsApp.Content.ToString().Length * OnlyWhatsApp.FontSize;
            Stack5.Children.Add(OnlyWhatsApp);



            accountCanvas.Height = 3450;

            return accountCanvas;
        }



        private Canvas WindowSettings()
        {
            Canvas WindowSettings = new Canvas();
            WindowSettings.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "Настройки отображения на экране:";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            WindowSettings.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            WindowSettings.Children.Add(line1);

            TextBlock SelectMonitorTitle = new TextBlock();
            SelectMonitorTitle.Text = "Выбрать основной монитор:";
            SelectMonitorTitle.Margin = new Thickness(10,100,0,0);
            SelectMonitorTitle.Foreground = Brushes.White;
            SelectMonitorTitle.FontSize = 22;
            WindowSettings.Children.Add(SelectMonitorTitle);
            ComboBox SelectMonitor = new ComboBox();
            SelectMonitor.Width = 300;
            SelectMonitor.Height = 40;
            SelectMonitor.Margin = new Thickness(10, 145, 0,0);
            SelectMonitor.Items.Add("Выбрать монитор");
            SelectMonitor.SelectedIndex = 0;
            WindowSettings.Children.Add(SelectMonitor);


            TextBlock HeaderWindowTitle = new TextBlock();
            HeaderWindowTitle.Text = "Выбрать стиль отображаемого окна:";
            HeaderWindowTitle.Margin = new Thickness(10, 200, 0, 0);
            HeaderWindowTitle.Foreground = Brushes.White;
            HeaderWindowTitle.FontSize = 22;
            WindowSettings.Children.Add(HeaderWindowTitle);
            ComboBox HeaderWindowStyle = new ComboBox();
            HeaderWindowStyle.Width = 300;
            HeaderWindowStyle.Height = 40;
            HeaderWindowStyle.Margin = new Thickness(10, 240, 0, 0);
            HeaderWindowStyle.Items.Add("Системный");
            HeaderWindowStyle.SelectedIndex = 0;
            WindowSettings.Children.Add(HeaderWindowStyle);



            TextBlock WindowSizeTitle = new TextBlock();
            WindowSizeTitle.Text = "Выбрать стиль отображаемого окна:";
            WindowSizeTitle.Margin = new Thickness(10, 300, 0, 0);
            WindowSizeTitle.Foreground = Brushes.White;
            WindowSizeTitle.FontSize = 22;
            WindowSettings.Children.Add(WindowSizeTitle);
            ComboBox WindowSize = new ComboBox();
            WindowSize.Width = 300;
            WindowSize.Height = 40;
            WindowSize.Margin = new Thickness(10, 340, 0, 0);
            WindowSize.Items.Add("Стандарт");
            WindowSize.SelectedIndex = 0;
            WindowSettings.Children.Add(WindowSize);


            TextBlock WindowImageTitle = new TextBlock();
            WindowImageTitle.Text = "Выбрать изображение для рабочего стола:";
            WindowImageTitle.Margin = new Thickness(10, 400, 0, 0);
            WindowImageTitle.Foreground = Brushes.White;
            WindowImageTitle.FontSize = 22;
            WindowSettings.Children.Add(WindowImageTitle);
            Image WindowImage = new Image();
            WindowImage.Width = 500;
            WindowImage.Height = 400;
            WindowImage.Stretch = Stretch.Fill;

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
                                WindowImage.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
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
                WindowImage.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/desktopTest.jpg"));
            }
            
            WindowImage.Margin = new Thickness(10,450,0,0);
            WindowSettings.Children.Add(WindowImage);
            Button ImageRedact = new Button();
            ImageRedact.Width = 30;
            ImageRedact.Height = 30;
            ImageRedact.Background = Brushes.Transparent;
            ImageRedact.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            ImageRedact.Click += (sender, e) =>
            {
                string directoryPath = AppDomain.CurrentDomain.BaseDirectory + @"data\images\"; // Получаем путь к каталогу относительно запускаемого файла

                DataBrowser dataBrowser = new DataBrowser(directoryPath, "picture");
                dataBrowser.GetImageUiElement(WindowImage);
                MainCanvas.Children.Add(dataBrowser.canvas);

            };

            Image IcoRedact = new Image();
            ImageRedact.Content = IcoRedact;
            IcoRedact.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            WindowSettings.Children.Add(ImageRedact);
            ImageRedact.Margin = new Thickness(40 + WindowImage.Width + 30, 400 + WindowImage.Height/2, 0, 0);

            TextBlock IcoSizeTitle = new TextBlock();
            IcoSizeTitle.Text = "Выбрать размер отображаемых иконок:";
            IcoSizeTitle.Margin = new Thickness(10, 900, 0, 0);
            IcoSizeTitle.Foreground = Brushes.White;
            IcoSizeTitle.FontSize = 22;
            WindowSettings.Children.Add(IcoSizeTitle);
            ComboBox IcoSize = new ComboBox();
            IcoSize.Width = 300;
            IcoSize.Height = 40;
            IcoSize.Margin = new Thickness(10, 940, 0, 0);
            IcoSize.Items.Add("Стандарт");
            IcoSize.SelectedIndex = 0;
            WindowSettings.Children.Add(IcoSize);

            WindowSettings.Height = 1200;

            return WindowSettings;
        }


        private Canvas StyleSettingsCanvas()
        {
            Canvas StyleCanvas = new Canvas();
            StyleCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "Настройки стиля эементов в приложении: (в разработке)";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            StyleCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            StyleCanvas.Children.Add(line1);

            return StyleCanvas;
        }


        private Canvas SoundSettingsCanvas()
        {
            Canvas SoundCanvas = new Canvas();
            SoundCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "Настройки звукового оформления:";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            SoundCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            SoundCanvas.Children.Add(line1);


            TextBlock SelectStartSound = new TextBlock();
            SelectStartSound.Text = "Выбрать звук при запуске:";
            SelectStartSound.Margin = new Thickness(10, 100, 0, 0);
            SelectStartSound.Foreground = Brushes.White;
            SelectStartSound.FontSize = 22;
            SoundCanvas.Children.Add(SelectStartSound);

            TextBlock StartSound = new TextBlock();
            StartSound.Text = "...";
            StartSound.Margin = new Thickness(10, 140, 0, 0);
            StartSound.Foreground = Brushes.White;
            StartSound.FontSize = 20;
            SoundCanvas.Children.Add(StartSound);

            Button StartSoundRedact = new Button();
            StartSoundRedact.Width = 30;
            StartSoundRedact.Height = 30;
            StartSoundRedact.Background = Brushes.Transparent;
            StartSoundRedact.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact1 = new Image();
            StartSoundRedact.Content = IcoRedact1;
            IcoRedact1.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            SoundCanvas.Children.Add(StartSoundRedact);
            StartSoundRedact.Margin = new Thickness(10 + StartSoundRedact.Content.ToString().Length + 30, 140, 0, 0);

            StartSoundRedact.Click += (sender, e) =>
            {
                string directoryPath = AppDomain.CurrentDomain.BaseDirectory + @"data\sounds\"; // Получаем путь к каталогу относительно запускаемого файла

                DataBrowser dataBrowser = new DataBrowser(directoryPath, "sound");

                

                MainCanvas.Children.Add(dataBrowser.canvas);
            };


            Slider StartSoundVolume = new Slider();
            StartSoundVolume.Width = 300;
            StartSoundVolume.Height = 20;
            StartSoundVolume.Value = 0;
            StartSoundVolume.Margin = new Thickness(10, 180, 0, 0);
            SoundCanvas.Children.Add(StartSoundVolume);


            TextBlock SelectSoundT = new TextBlock();
            SelectSoundT.Text = "Выбрать звук при открытии функции:";
            SelectSoundT.Margin = new Thickness(10, 200, 0, 0);
            SelectSoundT.Foreground = Brushes.White;
            SelectSoundT.FontSize = 22;
            SoundCanvas.Children.Add(SelectSoundT);

            TextBlock SelectSound = new TextBlock();
            SelectSound.Text = "...";
            SelectSound.Margin = new Thickness(10, 240, 0, 0);
            SelectSound.Foreground = Brushes.White;
            SelectSound.FontSize = 20;
            SoundCanvas.Children.Add(SelectSound);

            Button SelectSoundRedact = new Button();
            SelectSoundRedact.Width = 30;
            SelectSoundRedact.Height = 30;
            SelectSoundRedact.Background = Brushes.Transparent;
            SelectSoundRedact.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact2 = new Image();
            SelectSoundRedact.Content = IcoRedact2;
            IcoRedact2.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            SoundCanvas.Children.Add(SelectSoundRedact);
            SelectSoundRedact.Margin = new Thickness(10 + SelectSoundRedact.Content.ToString().Length + 30, 240, 0, 0);

            Slider SelectSoundVolume = new Slider();
            SelectSoundVolume.Width = 300;
            SelectSoundVolume.Height = 20;
            SelectSoundVolume.Value = 0;
            SelectSoundVolume.Margin = new Thickness(10, 280, 0, 0);
            SoundCanvas.Children.Add(SelectSoundVolume);



            TextBlock SelectNotificationSound = new TextBlock();
            SelectNotificationSound.Text = "Выбрать звук уведомления:";
            SelectNotificationSound.Margin = new Thickness(10, 300, 0, 0);
            SelectNotificationSound.Foreground = Brushes.White;
            SelectNotificationSound.FontSize = 22;
            SoundCanvas.Children.Add(SelectNotificationSound);

            TextBlock SelectNotification = new TextBlock();
            SelectNotification.Text = "...";
            SelectNotification.Margin = new Thickness(10, 340, 0, 0);
            SelectNotification.Foreground = Brushes.White;
            SelectNotification.FontSize = 20;
            SoundCanvas.Children.Add(SelectNotification);

            Button SelectNotificationRedact = new Button();
            SelectNotificationRedact.Width = 30;
            SelectNotificationRedact.Height = 30;
            SelectNotificationRedact.Background = Brushes.Transparent;
            SelectNotificationRedact.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact3 = new Image();
            SelectNotificationRedact.Content = IcoRedact3;
            IcoRedact3.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png")); // Укажите путь к изображен
            SoundCanvas.Children.Add(SelectNotificationRedact);
            SelectNotificationRedact.Margin = new Thickness(10 + SelectNotificationRedact.Content.ToString().Length + 30, 340, 0, 0);

            Slider SelectNotificationVolume = new Slider();
            SelectNotificationVolume.Width = 300;
            SelectNotificationVolume.Height = 20;
            SelectNotificationVolume.Value = 0;
            SelectNotificationVolume.Margin = new Thickness(10, 380, 0, 0);
            SoundCanvas.Children.Add(SelectNotificationVolume);
            


            return SoundCanvas;
        }

        private Canvas SecuritySettingsCanvas()
        {
            Canvas SecurityCanvas = new Canvas();
            SecurityCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "Настройки безопасности: (в разработке)";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            SecurityCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            SecurityCanvas.Children.Add(line1);

            return SecurityCanvas;
        }

        private Canvas dataSettingsCanvas()
        {
            Canvas dataCanvas = new Canvas();
            dataCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "настройка хранилищ данных: (в разработке)";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            dataCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            dataCanvas.Children.Add(line1);

            return dataCanvas;
        }

        private Canvas GuideSettingsCanvas()
        {
            Canvas GuideCanvas = new Canvas();
            GuideCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "Справочные материалы: (в разработке)";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            GuideCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            GuideCanvas.Children.Add(line1);

            return GuideCanvas;
        }

        private Canvas SpecialFuchSettingsCanvas()
        {
            Canvas SpecialFuncCanvas = new Canvas();
            SpecialFuncCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "Специальные функции: (в разработке)";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            SpecialFuncCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            SpecialFuncCanvas.Children.Add(line1);

            return SpecialFuncCanvas;
        }

        private Canvas AboutSystemCanvas()
        {
            Canvas AboutCanvas = new Canvas();
            AboutCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = "О системе:";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            AboutCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            AboutCanvas.Children.Add(line1);


            TextBlock TextInfo = new TextBlock();
            TextInfo.Text = "Philimon представляет собой инновационный и не дорогой инструментарий,\nспроектированный для автоматизации процессов создания, тестирования и управления\nпрограммным обеспечением, а также для эффективного взаимодействия\nмежду членами команды. Этот инструментарий предлагает\nновые возможности для решения проблем, с\nкоторыми сталкиваются компании в условиях санкций и высоких цен на ПО,\nявляясь достойным и гибким в использовании аналогом многих зарубежных программных\nпродуктов в данной сфере.";
            TextInfo.Margin = new Thickness(10, 100, 0, 0);
            TextInfo.Foreground = Brushes.White;
            TextInfo.FontSize = 22;
            AboutCanvas.Children.Add(TextInfo);

            return AboutCanvas;
        }


    }
}
