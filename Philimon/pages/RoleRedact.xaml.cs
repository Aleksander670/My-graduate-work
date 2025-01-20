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
    /// Логика взаимодействия для RoleRedact.xaml
    /// </summary>
    public partial class RoleRedact : Page
    {

        DatabaseData DB = new DatabaseData();

        StackPanel stack = new StackPanel();



        public RoleRedact()
        {
            InitializeComponent();

            GenerateUI();
        }




        void GenerateUI()
        {
            Canvas MainCanvas = new Canvas();
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


            Canvas MainHeader = new Canvas();
            MainHeader.Width = Application.Current.MainWindow.Width;
            MainHeader.Height = 40;
            MainHeader.Background = Brushes.AliceBlue;
            MainCanvas.Children.Add(MainHeader);

            Button back = new Button();
            back.Width = 200;
            back.Height = 38;
            back.Content = "Назад";

            back.Click += (sender, e) =>
            {
                this.NavigationService.Navigate(new DesktopPage());
            };

            MainHeader.Children.Add(back);

            // Создание объекта TableRoles
            TableRoles tableRoles = new TableRoles();

            tableRoles.Table_.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - tableRoles.Table_.Width / 2, Application.Current.MainWindow.Height / 2 - tableRoles.Table_.Height / 2, 0, 0);

            int rowCount = 0;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string countQuery = "SELECT COUNT(*) AS row_count FROM AllRoles";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                rowCount = Convert.ToInt32(countCommand.ExecuteScalar());


            }


            string[] arrText = new string[rowCount];

            int j = 0;
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string ReadQuery = "SELECT * FROM AllRoles";

                using (MySqlCommand command = new MySqlCommand(ReadQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            arrText[j] = reader["NameRole"].ToString();
                            j++;
                        }
                    }
                }
            }

            Canvas[] items = new Canvas[rowCount];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Canvas();
                items[i].Width = 300;
                items[i].Height = 40;
                items[i].Tag = arrText[i];
                items[i].Background = Brushes.LightGray;

            }

            tableRoles.AddItemsToStack(items);

            Button SaveChanges = new Button();
            SaveChanges.Content = "Добавить данные";
            SaveChanges.Width = 300;
            SaveChanges.Height = 30;
            tableRoles.AddButton(SaveChanges);

            SaveChanges.Click += (sender, e) =>
            {
                Canvas background = new Canvas();
                background.Background = Brushes.Black;
                background.Width = Application.Current.MainWindow.Width;
                background.Height = Application.Current.MainWindow.Height;
                background.Opacity = 0.9;
                MainCanvas.Children.Add(background);

                Canvas Form = new Canvas();
                Form.Width = 600;
                Form.Height = 800;
                Form.Background = Brushes.WhiteSmoke;
                Form.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - Form.Width / 2, Application.Current.MainWindow.Height / 2 - Form.Height / 2, 0, 0);

                TextBox name = new TextBox();

                TextBox disc = new TextBox();

                CheckBox funcView_Admin = new CheckBox();

                CheckBox funcView_Dev = new CheckBox();

                CheckBox funcView_Test = new CheckBox();

                CheckBox funcMadeRedact_Admin = new CheckBox();

                CheckBox funcMadeRedact_Dev = new CheckBox();

                CheckBox funcMadeRedact_Test = new CheckBox();

                MainCanvas.Children.Add(Form);

                Canvas Header = new Canvas();
                Header.Width = Form.Width;
                Header.Height = 40;
                Header.Background = Brushes.AliceBlue;
                Form.Children.Add(Header);

                TextBlock Title = new TextBlock();
                Title.Text = "Добавить роль" + ":";
                Title.Margin = new Thickness(5, 12, 0, 0);
                Header.Children.Add(Title);

                StackPanel stack = new StackPanel();
                stack.Width = Form.Width;
                stack.Height = Form.Height - Header.Height;
                stack.Margin = new Thickness(0, 60, 0, 0);
                Form.Children.Add(stack);



                name.Width = 400;
                name.Height = 40;
                name.Tag = "Название:";
                stack.Children.Add(name);

                disc.Width = 400;
                disc.Height = 100;
                disc.Tag = "Описание:";
                stack.Children.Add(disc);

                funcView_Admin.Width = 400;
                funcView_Admin.Height = 90;
                funcView_Admin.Content = "Просмотр функций Администратора";
                stack.Children.Add(funcView_Admin);

                funcView_Dev.Width = 400;
                funcView_Dev.Height = 90;
                funcView_Dev.Content = "Просмотр функций Разработчика";
                stack.Children.Add(funcView_Dev);

                funcView_Test.Width = 400;
                funcView_Test.Height = 90;
                funcView_Test.Content = "Просмотр функций Тестировщика";
                stack.Children.Add(funcView_Test);

                funcMadeRedact_Admin.Width = 400;
                funcMadeRedact_Admin.Height = 90;
                funcMadeRedact_Admin.Content = "Создание/Редактирование записей администратора";
                stack.Children.Add(funcMadeRedact_Admin);

                funcMadeRedact_Dev.Width = 400;
                funcMadeRedact_Dev.Height = 90;
                funcMadeRedact_Dev.Content = "Создание/Редактирование записей тестировщика";
                stack.Children.Add(funcMadeRedact_Dev);

                funcMadeRedact_Test.Width = 400;
                funcMadeRedact_Test.Height = 90;
                funcMadeRedact_Test.Content = "Создание/Редактирование записей разработчика";
                stack.Children.Add(funcMadeRedact_Test);


                Button WriteButton = new Button();
                WriteButton.Width = 400;
                WriteButton.Height = 40;
                WriteButton.Content = "Записать данные";
                stack.Children.Add(WriteButton);

                WriteButton.Click += (sender1, e1) =>
                {
                    if (!string.IsNullOrEmpty(name.Text) || !string.IsNullOrEmpty(disc.Text))
                    {
                        using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                        {
                            connection.Open();

                            string query = "INSERT INTO AllRoles (NameRole, DiscRole, funcView_Admin, funcView_Dev, funcView_Test, funcMadeRedact_Admin, funcMadeRedact_Dev, funcMadeRedact_Test) VALUES (@NameRole,@DiscRole,@funcView_Admin,@funcView_Dev,@funcView_Test,@funcMadeRedact_Admin,@funcMadeRedact_Dev,@funcMadeRedact_Test)";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@NameRole", name.Text);
                                command.Parameters.AddWithValue("@DiscRole", disc.Text);
                                command.Parameters.AddWithValue("@funcView_Admin", funcView_Admin.IsChecked);
                                command.Parameters.AddWithValue("@funcView_Dev", funcView_Dev.IsChecked);
                                command.Parameters.AddWithValue("@funcView_Test", funcView_Test.IsChecked);
                                command.Parameters.AddWithValue("@funcMadeRedact_Admin", funcMadeRedact_Admin.IsChecked);
                                command.Parameters.AddWithValue("@funcMadeRedact_Dev", funcMadeRedact_Dev.IsChecked);
                                command.Parameters.AddWithValue("@funcMadeRedact_Test", funcMadeRedact_Test.IsChecked);

                                command.ExecuteNonQuery();

                                name.BorderBrush = Brushes.Lime;
                                disc.BorderBrush = Brushes.Lime;

                            }
                        }
                    }
                    else
                    {
                        name.BorderBrush = Brushes.Red;
                        disc.BorderBrush = Brushes.Red;
                    }
                };



                Button Close = new Button();
                Close.Width = 38;
                Close.Height = 38;
                Close.Margin = new Thickness(560,0,0,0);
                Close.Content = "X";
                Form.Children.Add(Close);
                Close.Click += (sender2, e2) =>
                {
                    if (MainCanvas.Children.Contains(background))
                    {
                        MainCanvas.Children.Remove(background);
                        background = null;
                    }
                    if (MainCanvas.Children.Contains(Form))
                    {
                        MainCanvas.Children.Remove(Form);
                        Form = null;
                    }

                    RoleRedact RR = new RoleRedact();
                    this.NavigationService.Navigate(RR);
                };


            };

            MainCanvas.Children.Add(tableRoles.Table_);


        }


    }

        


    class TableRoles : Canvas
    {
        public Canvas Table_ = new Canvas();
        Point startPoint;
        private bool isDragging;
        StackPanel Stack = new StackPanel();

        public TableRoles()
        {
            Table_ = new Canvas();
            Table_.Width = 300;
            Table_.Height = 400;
            Table_.Background = Brushes.White;

            Canvas Header = new Canvas();
            Header.Width = Table_.Width;
            Header.Height = 40;
            Header.Background = Brushes.AliceBlue;
            Table_.Children.Add(Header);

            TextBlock Title = new TextBlock();
            Title.Text = "Роли" + ":";
            Title.Margin = new Thickness(5, 12, 0, 0);
            Header.Children.Add(Title);
            Header.MouseLeftButtonDown += MouseLeftButtonDown;
            Header.MouseLeftButtonUp += MouseLeftButtonUp;
            Header.MouseMove += MouseMove;


            Stack.Width = Table_.Width;
            Stack.Height = Table_.Height - Header.Height;
            Stack.Margin = new Thickness(0, 40, 0, 0);
            Table_.Children.Add(Stack);
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDragging)
            {
                Point currentPoint = e.GetPosition(null);
                double deltaX = currentPoint.X - startPoint.X;
                double deltaY = currentPoint.Y - startPoint.Y;
                Table_.Margin = new Thickness(Table_.Margin.Left + deltaX, Table_.Margin.Top + deltaY, 0, 0);
                startPoint = currentPoint;
            }
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            isDragging = true;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        public void AddItemsToStack(Canvas[] items)
        {
            foreach (Canvas item in items)
            {
                Stack.Children.Add(item);

                TextBlock text = new TextBlock();
                text.Text = item.Tag.ToString();
                text.Margin = new Thickness(5, 12, 0, 0);
                item.Children.Add(text);



                item.MouseEnter += (sender, e) =>
                {
                    item.Background = Brushes.White;
                };
                item.MouseLeave += (sender, e) =>
                {
                    item.Background = Brushes.LightGray;
                };

                item.MouseDown += (sender, e) =>
                {
                    Line line = new Line();
                    TableRules Rules = new TableRules(text.Text);
                    Table_.Children.Add(Rules.Table_);

                    Rules.ParentCanvas = item;

                    item.Background = Brushes.DarkGray;
                    item.IsEnabled = false;

                    Rules.CloseEvent += (sender1, e1) =>
                    {
                        item.IsEnabled = true;
                    };



                };



            }






        }


            public void AddButton(Button button){

                Stack.Children.Add(button);
            }


    }



    class TableRules
    {
        public event EventHandler CloseEvent;

        public Canvas Table_ = new Canvas();
        Point startPoint;
        private bool isDragging;
        StackPanel Stack = new StackPanel();

        public Canvas ParentCanvas { get; set; }

        Button Close = new Button();

        DatabaseData DB = new DatabaseData();

        public TableRules(string nameTable)
        {
            Table_ = new Canvas();
            Table_.Width = 300;
            Table_.Height = 400;
            Table_.Background = Brushes.White;

            Canvas Header = new Canvas();
            Header.Width = Table_.Width;
            Header.Height = 40;
            Header.Background = Brushes.AliceBlue;
            Table_.Children.Add(Header);

            TextBlock Title = new TextBlock();
            Title.Text = "Правила для" + " " + nameTable + ":";
            Title.Margin = new Thickness(5, 12, 0, 0);
            Header.Children.Add(Title);
            Header.MouseLeftButtonDown += MouseLeftButtonDown;
            Header.MouseMove += MouseMove;
            Header.MouseLeftButtonUp += MouseLeftButtonUp;

            Close.Width = 40;
            Close.Height = 40;
            Close.Content = "X";
            Close.Margin = new Thickness(260, 0, 0, 0);

            Close.Click += (sender, e) =>
            {
                ParentCanvas.IsEnabled = true;
                ParentCanvas.Children.Remove(Table_);
                Table_.Opacity = 0; // сделать нормальноу уничтожение 
            };

            Header.Children.Add(Close);

            Stack.Width = Table_.Width;
            Stack.Height = Table_.Height - Header.Height;
            Stack.Margin = new Thickness(0, 40, 0, 0);
            Table_.Children.Add(Stack);



            GenerateFunc(nameTable);
        }


        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDragging)
            {
                Point currentPoint = e.GetPosition(null);
                double deltaX = currentPoint.X - startPoint.X;
                double deltaY = currentPoint.Y - startPoint.Y;
                Table_.Margin = new Thickness(Table_.Margin.Left + deltaX, Table_.Margin.Top + deltaY, 0, 0);
                startPoint = currentPoint;
            }
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            isDragging = true;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }


        public void AddItemsToStack(Canvas[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Canvas item = items[i];
                Stack.Children.Add(item);

                item.MouseEnter += (sender, e) =>
                {
                    item.Background = Brushes.White;
                };

                item.MouseLeave += (sender, e) =>
                {
                    item.Background = Brushes.LightGray;
                };
            }



        }





        void GenerateFunc(string NameRole)
        {
            string[] Function = { "Просмотр функций Администратора", "Просмотр функций Разработчика", "Просмотр функций Тестировщика", "Создание/Ред. записей администратора", "Создание/Ред. записей тестировщика", "Создание/Ред. записей разработчика" };
            List<Canvas> items = new List<Canvas>(); // Используем List вместо массива
            List<CheckBox> Func = new List<CheckBox>(); // Используем List вместо массива

            bool[] bools = new bool[Function.Length];

            string[] attrName = { "funcView_Admin", "funcView_Dev", "funcView_Test", "funcMadeRedact_Admin", "funcMadeRedact_Dev", "funcMadeRedact_Test" };

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM AllRoles WHERE NameRole = @NameRole";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NameRole", NameRole);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < Function.Length; i++)
                            {
                                bools[i] = (bool)reader[attrName[i]];
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < Function.Length; i++) // Проходим по массиву Function
            {
                Canvas canvas = new Canvas(); // Создаем новый Canvas
                canvas.Width = 300;
                canvas.Height = 40;
                canvas.Background = Brushes.LightGray;

                CheckBox checkBox = new CheckBox(); // Создаем новый CheckBox
                checkBox.Margin = new Thickness(4, 12, 0, 0);
                checkBox.Content = Function[i]; // Устанавливаем текст из массива Function
                checkBox.IsChecked = bools[i];
                Func.Add(checkBox); // Добавляем CheckBox в список Func

                canvas.Children.Add(checkBox); // Добавляем CheckBox в Canvas
                items.Add(canvas); // Добавляем Canvas в список items
            }

            AddItemsToStack(items.ToArray()); // Преобразуем список items в массив и передаем его в метод AddItemsToStack

            Button SaveChanges = new Button();
            SaveChanges.Width = 300;
            SaveChanges.Height = 30;
            SaveChanges.Content = "Сохранить изменения";
            Stack.Children.Add(SaveChanges);

            SaveChanges.Click += (sender, e) =>
            {
                try {
                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();
                        string updateQuery = $"UPDATE AllRoles SET funcView_Admin = {Func[0].IsChecked}, funcView_Dev = {Func[1].IsChecked}, funcView_Test = {Func[2].IsChecked}, funcMadeRedact_Admin = {Func[3].IsChecked}, funcMadeRedact_Dev = {Func[4].IsChecked}, funcMadeRedact_Test = {Func[5].IsChecked} WHERE NameRole = @NameRole";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@NameRole", NameRole);

                            for (int i = 0; i < Function.Length; i++)
                            {

                                if (Func[i].IsChecked != bools[i])
                                {
                                    Func[i].BorderBrush = Brushes.Lime;
                                }
                            }

                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    for (int i = 0; i < Function.Length; i++)
                    {
                        Func[i].BorderBrush = Brushes.Red;
                    }
                }
                                
            };

        }


        

    }



}
