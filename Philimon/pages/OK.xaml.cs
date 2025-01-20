using MySql.Data.MySqlClient;
using Philimon.objects;
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
using System.Windows.Threading;

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для OK.xaml
    /// </summary>
    public partial class OK : Page
    {
        double posY = 0;

        int id_ = ((Main)Application.Current.MainWindow).PersonID;

#pragma warning disable CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово
        string Name = "";
#pragma warning restore CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово

        int next = 0;

        DatabaseData DB = new DatabaseData();

        public OK()
        {
            InitializeComponent();

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Humans WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id",id_);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            Name = reader["Firtsname"].ToString() + "\n" + reader["SecondName"].ToString() + " " + reader["ThirdName"].ToString();
                            
                        }

                    }

                }

            }


            CompleteUi();
        }

        void CompleteUi()
        {
            Canvas Canvas = new Canvas();
            Canvas.Width = Application.Current.MainWindow.Width;
            Canvas.Height = Application.Current.MainWindow.Height;
            Canvas.Margin = new Thickness(0, 0, 0, 0);
            Canvas.Background = Brushes.White;
            Grid.Children.Add(Canvas);

            Image Ico = new Image();
            Ico.Width = 128;
            Ico.Height = 128;
            Ico.Opacity = 0;
            Ico.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/complete.png"));
            Ico.Margin = new Thickness(Application.Current.MainWindow.Width/2-Ico.Width/2,Application.Current.MainWindow.Height/2-Ico.Height - 50, 0 ,0);
            Canvas.Children.Add(Ico);

            TextBlock CompliteBlock = new TextBlock();
            CompliteBlock.Text = "Добро пожаловать " + Name + "!";
            CompliteBlock.HorizontalAlignment = HorizontalAlignment.Center;
            CompliteBlock.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - Ico.Width / 2, Application.Current.MainWindow.Height / 2 - Ico.Height + 150, 0, 0);
            Canvas.Children.Add(CompliteBlock);

            bool fadeWind = false;

            DispatcherTimer AnimTimer = new DispatcherTimer();
            AnimTimer.Interval = TimeSpan.FromSeconds(0.01);
            AnimTimer.Start();
            AnimTimer.Tick += (sender, e) =>
            {
                if (Ico.Opacity < 1)
                {
                    posY += 0.5;
                    Ico.Opacity += 0.01;
                    Ico.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - Ico.Width / 2, (Application.Current.MainWindow.Height / 2 - Ico.Height - 50)+ posY, 0, 0);
                }
                if (Ico.Opacity > 0.9 && CompliteBlock.Opacity < 1)
                {
                    
                    CompliteBlock.Opacity += 0.1;
                    CompliteBlock.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - CompliteBlock.Width / 2, (Application.Current.MainWindow.Height / 2 - CompliteBlock.Height + 150) + posY, 0, 0);
                }

                if (Ico.Opacity >= 1)
                {
                    next++;
                }

                if (next >= 60)
                {
                    fadeWind = true;
                }

                if (fadeWind == true)
                {
                    Canvas.Opacity -= 0.1;
                }
                if (next >= 200)
                {

                    AnimTimer.Stop();

                    SoundPlay Welcome = new SoundPlay();
                    Welcome.SoundWelcome();

                    this.NavigationService.Navigate(new DesktopPage());
                }

            };



        }


    }
}
