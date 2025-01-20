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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;


namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для ServerStatus.xaml
    /// </summary>
    public partial class ServerStatus : Page
    {

        private Polyline polyline;
#pragma warning disable CS0414 // Полю "ServerStatus.xOffset" присвоено значение, но оно ни разу не использовано.
        private double xOffset = 0;
#pragma warning restore CS0414 // Полю "ServerStatus.xOffset" присвоено значение, но оно ни разу не использовано.
#pragma warning disable CS0414 // Полю "ServerStatus.direction" присвоено значение, но оно ни разу не использовано.
        private double direction = 1;
#pragma warning restore CS0414 // Полю "ServerStatus.direction" присвоено значение, но оно ни разу не использовано.

        int N = 0;
        int LengthGraph = 0;

        Canvas GraphCanvas = new Canvas();

        bool Connect = true;

        DatabaseData DB = new DatabaseData();

        public ServerStatus()
        {
            InitializeComponent();

            

            Loaded += (sender, e) =>
            {
                GenerateUI();

                DrawGraph();

                GenerateButtons();

#pragma warning disable CS0168 // Переменная объявлена, но не используется
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        Connect = true;
                    }
                }
                catch (Exception ex)
                {
                    Connect = false;
                }
#pragma warning restore CS0168 // Переменная объявлена, но не используется

            };

        }


        void GenerateUI()
        {

            Grid.Background = Brushes.Black;
            Canvas BackgroundGraph = new Canvas();
            BackgroundGraph.Background = Brushes.LimeGreen;
            BackgroundGraph.Width = 1100;
            BackgroundGraph.Height = 400;
            BackgroundGraph.HorizontalAlignment = HorizontalAlignment.Center;
            BackgroundGraph.VerticalAlignment = VerticalAlignment.Bottom;
            BackgroundGraph.ClipToBounds = true; // Обрезаем содержимое до границ родительского Canvas

            GraphCanvas.Background = Brushes.Black; // Используем поле класса GraphCanvas
            GraphCanvas.Width = 1098;
            GraphCanvas.Height = 398;
            GraphCanvas.Margin = new Thickness(1, 1, 0, 0);

            BackgroundGraph.Children.Add(GraphCanvas);
            Grid.Children.Add(BackgroundGraph);


            // Оранжевая горизонтальная линия
            Line orangeLine = new Line();
            orangeLine.Stroke = Brushes.Orange;
            orangeLine.X1 = 0; // начало линии слева
            orangeLine.Y1 = BackgroundGraph.Height * 0.5; // середина по вертикали
            orangeLine.X2 = BackgroundGraph.Width; // конец линии справа
            orangeLine.Y2 = BackgroundGraph.Height * 0.5; // середина по вертикали
            BackgroundGraph.Children.Add(orangeLine);

            // Красная горизонтальная линия
            Line redLine = new Line();
            redLine.Stroke = Brushes.Red;
            redLine.X1 = 0; // начало линии слева
            redLine.Y1 = BackgroundGraph.Height * 0.8; // ближе к низу
            redLine.X2 = BackgroundGraph.Width; // конец линии справа
            redLine.Y2 = BackgroundGraph.Height * 0.8; // ближе к низу
            BackgroundGraph.Children.Add(redLine);

        }

        private void DrawGraph()
        {
            
            polyline = new Polyline();
            polyline.Stroke = Brushes.Green;
            polyline.StrokeThickness = 2;


            if (Connect == true) {
                StartAnimation();
                for (int i = 0; i < LengthGraph; i++)
                {
                    double x = i * 10;
                    double y = 0;
                    polyline.Points.Add(new Point(x, y));
                }

                GraphCanvas.Children.Add(polyline);
            }
            else
            {
                TextBlock Text = new TextBlock();
                Text.Text = "Ошибка!\nПодключение к серверу отсутсвует!";
                Text.Foreground = Brushes.Red;
                Text.HorizontalAlignment = HorizontalAlignment.Center;
                Text.VerticalAlignment = VerticalAlignment.Center;

                GraphCanvas.Children.Add(Text);
            }
            

       }
    
        void GenerateButtons()
        {
            Canvas BackgroundStackButtons = new Canvas();
            BackgroundStackButtons.Width = Application.Current.MainWindow.Width;
            BackgroundStackButtons.Height = 68;
            BackgroundStackButtons.Margin = new Thickness(0, -800, 0, 0);
            BackgroundStackButtons.Background = Brushes.LimeGreen;
            Grid.Children.Add(BackgroundStackButtons);

            Canvas Stack = new Canvas();
            Stack.Width = BackgroundStackButtons.Width - 2;
            Stack.Height = BackgroundStackButtons.Height - 2;
            Stack.Margin = new Thickness(0, -1, 1, 0);
            Stack.Background = Brushes.Black;
            BackgroundStackButtons.Children.Add(Stack);

            Button BackButton = new Button();
            BackButton.Width = 100;
            BackButton.Height = 40; // Устанавливаем высоту кнопки
            BackButton.Content = "Назад";
            BackButton.Margin = new Thickness(0,20,0,0);
            BackButton.Background = Brushes.Black;
            BackButton.Foreground = Brushes.LimeGreen;
            BackButton.HorizontalAlignment = HorizontalAlignment.Left; // Выравнивание кнопки
            BackButton.VerticalAlignment = VerticalAlignment.Bottom; // Выравнивание кнопки
            Stack.Children.Add(BackButton);

            BackButton.Click += (sender, e) =>
            {
                this.NavigationService.Navigate(new DesktopPage());
            };

            ComboBox TimeLeft = new ComboBox();
            TimeLeft.Width = 200;
            TimeLeft.Height = 20;
            TimeLeft.Margin = new Thickness(120, 30, 0, 0);
            TimeLeft.HorizontalAlignment = HorizontalAlignment.Left; // Выравнивание кнопки
            TimeLeft.VerticalAlignment = VerticalAlignment.Bottom; // Выравнивание кнопки
            TimeLeft.Items.Add("-Время обновления-");
            TimeLeft.Items.Add("Быстро");
            TimeLeft.Items.Add("Стандартно");
            TimeLeft.Items.Add("Медленно");
            TimeLeft.SelectedIndex = 0;
            Stack.Children.Add(TimeLeft);

            TimeLeft.SelectionChanged += (sender, e) =>
            {
                StartAnimation();
                N = TimeLeft.SelectedIndex;
                

            };


            ComboBox GraphSize = new ComboBox();
            GraphSize.Width = 200;
            GraphSize.Height = 20;
            GraphSize.Margin = new Thickness(420, 30, 0, 0);
            GraphSize.HorizontalAlignment = HorizontalAlignment.Left; // Выравнивание кнопки
            GraphSize.VerticalAlignment = VerticalAlignment.Bottom; // Выравнивание кнопки
            GraphSize.Items.Add("-Длина графика-");
            GraphSize.Items.Add("Большой");
            GraphSize.Items.Add("Средний");
            GraphSize.Items.Add("Малый");
            GraphSize.SelectedIndex = 0;
            Stack.Children.Add(GraphSize);

            GraphSize.SelectionChanged += (sender, e) =>
            {
                if (GraphSize.SelectedIndex == 0)
                {
                    LengthGraph = 0;
                }
                else if (GraphSize.SelectedIndex == 1)
                {
                    LengthGraph = 200;
                }
                else if (GraphSize.SelectedIndex == 2)
                {
                    LengthGraph = 100;
                }
                else if (GraphSize.SelectedIndex == 3)
                {
                    LengthGraph = 50;
                }

            };


            Button CheckTestButton = new Button();
            CheckTestButton.Width = 300;
            CheckTestButton.Height = 40; // Устанавливаем высоту кнопки
            CheckTestButton.Content = "Проверить доступ....";
            CheckTestButton.Margin = new Thickness(700, 20, 0, 0);
            CheckTestButton.Background = Brushes.Black;
            CheckTestButton.Foreground = Brushes.LimeGreen;
            CheckTestButton.HorizontalAlignment = HorizontalAlignment.Left; // Выравнивание кнопки
            CheckTestButton.VerticalAlignment = VerticalAlignment.Bottom; // Выравнивание кнопки
            Stack.Children.Add(CheckTestButton);

            CheckTestButton.Click += async (sender, e) =>
            {
                string connectionString = DB.ConnectionString;

                for (int i = 0; i < 1000; i++)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = "SELECT * FROM Humans";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                           
                        }
                    }
                }

            };

        }


        private void StartAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = -GraphCanvas.ActualWidth; // Двигаем график на всю длину GraphCanvas
            animation.Duration = TimeSpan.FromSeconds(N * 0.1); // Скорость анимации зависит от переменной N
            animation.Completed += AnimationCompleted;

            TranslateTransform translateTransform = new TranslateTransform();
            polyline.RenderTransform = translateTransform;
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private async void AnimationCompleted(object sender, EventArgs e)
        {
            polyline.Points.Clear();

            Stopwatch stopwatch = new Stopwatch();
            List<double> connectionSpeedData = new List<double>();

            for (int i = 0; i < LengthGraph; i++)
            {
                stopwatch.Start();

                await ConnectToDatabaseAsync();

                stopwatch.Stop();
                connectionSpeedData.Add(stopwatch.Elapsed.TotalMilliseconds);
                stopwatch.Reset();
            }

            for (int i = 0; i < connectionSpeedData.Count; i++)
            {
                double x = i * (GraphCanvas.ActualWidth / connectionSpeedData.Count);
                double y = connectionSpeedData[i];
                polyline.Points.Add(new Point(x, y));
            }

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = -GraphCanvas.ActualWidth;
            animation.Duration = TimeSpan.FromSeconds(N);
            animation.Completed += AnimationCompleted;

            TranslateTransform translateTransform = new TranslateTransform();
            polyline.RenderTransform = translateTransform;
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private async Task ConnectToDatabaseAsync()
        {
            string connectionString = DB.ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    // Успешное подключение к базе данных
                }
                catch (Exception ex)
                {
                    // Обработка ошибки подключения
                    MessageBox.Show("Error connecting to the database: " + ex.Message);
                }
            }
        }

    }
}

    




