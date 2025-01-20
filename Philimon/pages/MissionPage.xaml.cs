using MySql.Data.MySqlClient;
using Philimon.objects;
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

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для MissionPage.xaml
    /// </summary>
    public partial class MissionPage : Page
    {
        Canvas MainCanvas = new Canvas();

        DatabaseData DB = new DatabaseData();

        public MissionPage()
        {
            InitializeComponent();

            GenerateUI();
        }


        void GenerateUI()
        {
            MainCanvas.Width = Application.Current.MainWindow.Width;
            MainCanvas.Height = Application.Current.MainWindow.Height;
            Grid.Children.Add(MainCanvas);

            BlurEffect Blur = new BlurEffect();
            Blur.Radius = 8;

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
            SettingsCellsCanvas.Margin = new Thickness(0, 0, 0, 0);
            MainCanvas.Children.Add(SettingsCellsCanvas);

            BlurEffect BlurLogo = new BlurEffect();
            BlurLogo.Radius = 4;

            Canvas TitleCanvas = new Canvas();
            TitleCanvas.Width = 365;
            TitleCanvas.Height = 80;
            TitleCanvas.Background = Brushes.White;
            TitleCanvas.Opacity = 0.8;
            TitleCanvas.Effect = BlurLogo;
            TitleCanvas.Margin = new Thickness(0, 0, 0, 0);
            //MainCanvas.Children.Add(TitleCanvas);


            StackPanel stackPanel = new StackPanel();
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            ScrollViewer ScrollList = new ScrollViewer();
            ScrollList.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollList.Width = 365;
            ScrollList.Height = 520;
            ScrollList.Margin = new Thickness(0, 140, 0, 0);
            ScrollList.Content = stackPanel;
            SettingsCellsCanvas.Children.Add(ScrollList);


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

           

            int count = 0;
            string[] arrText;
            int[] arrId;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM AllMissions";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

                arrText = new string[count];
                arrId = new int[count];

                string query1 = "SELECT * FROM AllMissions"; 
                int i = 0;
                using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                {
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            arrText[i] = reader["NameMission"].ToString();
                            arrId[i] = (int)reader["id"];
                            i++;
                        }
                    }
                }
            }



            Canvas[] ButtonSetting = new Canvas[count];
            Image[] icons = new Image[count];
            TextBlock[] TextButton = new TextBlock[count];



            for (int i = 0; i < count; i++)
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
                icons[i].Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/MissionCellIco.png"));
                ButtonSetting[i].Children.Add(icons[i]);
                icons[i].Margin = new Thickness(0, 0, 0, 0);

                TextButton[i] = new TextBlock();
                TextButton[i].Text = arrText[i];
                TextButton[i].FontSize = 16;
                ButtonSetting[i].Children.Add(TextButton[i]);
                TextButton[i].Margin = new Thickness(64, 60 / 2 - (TextButton[i].FontSize - 2), 0, 0);


                ButtonSetting[i].Tag = arrId[i].ToString();

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


                ButtonSetting[i].MouseDown += (sender, e) => {
                    Canvas button = (Canvas)sender;
                    int missionId = Convert.ToInt32(button.Tag); // Получаем значение Tag кнопки как ID миссии

                    string connectionString = DB.ConnectionString;
                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();

                        string query = "SELECT * FROM AllMissions WHERE id = @missionId";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@missionId", missionId);

                        MySqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string name = reader["NameMission"].ToString();
                            string type = reader["TypeMission"].ToString();
                            string otdel = reader["Otdel"].ToString();
                            string person = reader["Person"].ToString();
                            string disc = reader["Disc"].ToString();

                            ContentScroll.Content = MissionCanvas(missionId, name, type, otdel, person, disc);
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                            
                        }

                        reader.Close();
                    }

                };





            }

            Canvas ExitButton = new Canvas();
            ExitButton.Width = 365;
            ExitButton.Height = 60;
            ExitButton.Background = Brushes.AliceBlue;
            ExitButton.Opacity = 0.8;
            ExitButton.Margin = new Thickness(0, Application.Current.MainWindow.Height - ((ExitButton.Height * 2) - 30), 0, 0);
            SettingsCellsCanvas.Children.Add(ExitButton);

            Image IcoExit = new Image();
            IcoExit.Width = 60;
            IcoExit.Height = 60;
            IcoExit.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/settings/back.png"));
            IcoExit.Margin = new Thickness(0, 0, 0, 0);
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






        }


        private Canvas MissionCanvas(int id, string Name, string Type, string Otdel, string Person, string Disc)
        {
            Canvas Canvas = new Canvas();
            Canvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = Name + ":";
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            Canvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            Canvas.Children.Add(line1);


            TextBlock TextInfo = new TextBlock();
            TextInfo.Text = Type + "\n" + Person + ":" + Otdel + "\n" + "Описание" + ":" + Disc;
            TextInfo.Margin = new Thickness(10, 100, 0, 0);
            TextInfo.Foreground = Brushes.White;
            TextInfo.FontSize = 22;
            Canvas.Children.Add(TextInfo);

            Button CompliteMission = new Button();
            CompliteMission.Width = 400;
            CompliteMission.Height = 40;
            CompliteMission.Content = "Завершить";
            Canvas.SetLeft(CompliteMission, 30); // Устанавливаем отступ слева
            Canvas.SetTop(CompliteMission, 300); // Устанавливаем отступ сверху
            Canvas.Children.Add(CompliteMission);

            CompliteMission.Click += (sender, e) =>
            {
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM AllMissions WHERE id = @id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();

                        this.NavigationService.Navigate(new DesktopPage());
                    }

                }
            };


            return Canvas;
        }


    }
}
