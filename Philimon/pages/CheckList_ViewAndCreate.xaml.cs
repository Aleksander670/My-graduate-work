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

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для CheclList_ViewAndCreate.xaml
    /// </summary>
    public partial class CheckList_ViewAndCreate : Page
    {
        DatabaseData DB = new DatabaseData();

#pragma warning disable CS0414 // Полю "CheckList_ViewAndCreate.testing" присвоено значение, но оно ни разу не использовано.
        bool testing = false;
#pragma warning restore CS0414 // Полю "CheckList_ViewAndCreate.testing" присвоено значение, но оно ни разу не использовано.

        public CheckList_ViewAndCreate(string status, int id)
        {
            InitializeComponent();

            GenerateUI(status, id);
        }

        

        void GenerateUI(string status, int id)
        {
            ScrollViewer Viewer = new ScrollViewer();
            Viewer.Width = Application.Current.MainWindow.Width;
            Viewer.Height = Application.Current.MainWindow.Height;
            Grid.Children.Add(Viewer);

            if (status == "view")
            {
                Viewer.Content = GenerateView(id);
            }
            if (status == "create")
            {
                Viewer.Content = GenerateCreate(id);
            }
            if (status == "redact")
            {
                Viewer.Content = GenerateRedact(id);
            }

        }

        Canvas GenerateView(int id)
        {
            Canvas Background = new Canvas();
            Background.Width = Application.Current.MainWindow.Width;
            Background.Height = Application.Current.MainWindow.Height + 100;
            Background.Background = Brushes.Gray;

            Canvas Header = new Canvas();
            Header.Width = Background.Width;
            Header.Height = 64;
            Header.Background = Brushes.LightGray;
            Background.Children.Add(Header);

            Canvas CanvasMainInformation = new Canvas();
            CanvasMainInformation.Width = Application.Current.MainWindow.Width - 200;
            CanvasMainInformation.Height = 240;
            CanvasMainInformation.Background = Brushes.White;
            CanvasMainInformation.Margin = new Thickness(100, 150, 1, 100);
            Background.Children.Add(CanvasMainInformation);



            string NameCL = "";
            string Creater = "";
            string Project = "";
            int indexStatus = 0;

            int countRows = 0;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string query = "SELECT *FROM AllCheckLists";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NameCL = reader["NameCL"].ToString();
                            Creater = reader["Creater"].ToString();
                            Project = reader["Project"].ToString();
                            indexStatus = Convert.ToInt32(reader["StatusCheckList"]);

                            countRows++;
                        }

                    }
                }

            }

            GenerateButton(CanvasMainInformation, indexStatus, id, false, countRows);

            TextBlock TitleStatus = new TextBlock();
            TitleStatus.Text = generateStatusCase(indexStatus);
            TitleStatus.Margin = new Thickness(4, 8, 0, 0);
            TitleStatus.FontSize = 22;
            Header.Children.Add(TitleStatus);

            string PriotityText = "";
            string LabelsText = "";
            string ComponentsText = "";

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string query = "SELECT *FROM _tsz_Check_" + id.ToString();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PriotityText = reader["priority"].ToString();
                            LabelsText = reader["labels"].ToString();
                            ComponentsText = reader["components"].ToString();
                        }

                    }
                }

            }

            TextBlock TitleTextBox = new TextBlock();
            TitleTextBox.FontSize = 19;
            TitleTextBox.Text = "Название:" + NameCL;
            TitleTextBox.Margin = new Thickness(5, 5, 0, 0);
            CanvasMainInformation.Children.Add(TitleTextBox);

            string[] Attributes = { "Приоритет:", "Метки:", "Компоненты:", "Кто создал:", "Проекты:" };

            TextBlock[] Parametrs = new TextBlock[Attributes.Length];

            for (int j = 0; j < Attributes.Length; j++)
            {
                Parametrs[j] = new TextBlock();
                Parametrs[j].Text = Attributes[j];
                Parametrs[j].FontSize = 15;
                Parametrs[j].Margin = new Thickness(10, 50 + (j * 32), 0, 0);
                CanvasMainInformation.Children.Add(Parametrs[j]);
            }

            TextBlock Priotity = new TextBlock();
            Priotity.Width = 200;
            Priotity.Height = 24;
            Priotity.Text = PriotityText;
            Priotity.Margin = new Thickness(96, 52, 0, 0);
            CanvasMainInformation.Children.Add(Priotity);

            TextBlock Labels = new TextBlock();
            Labels.Width = 200;
            Labels.Height = 24;
            Labels.Text = LabelsText;
            Labels.Margin = new Thickness(96, 79, 0, 0);
            CanvasMainInformation.Children.Add(Labels);

            TextBlock Components = new TextBlock();
            Components.Width = 200;
            Components.Height = 24;
            Components.Text = ComponentsText;
            Components.Margin = new Thickness(120, 112, 0, 0);
            CanvasMainInformation.Children.Add(Components);

            TextBlock Maker = new TextBlock();
            Maker.Width = 200;
            Maker.Height = 24;
            Maker.Text = Creater;
            Maker.Margin = new Thickness(120, 145, 0, 0);
            CanvasMainInformation.Children.Add(Maker);

            TextBlock Projects = new TextBlock();
            Projects.Width = 200;
            Projects.Height = 24;
            Projects.Text = Project;
            Projects.Margin = new Thickness(96, 178, 0, 0);
            CanvasMainInformation.Children.Add(Projects);

            Button Close = new Button();
            Close.Width = 64;
            Close.Height = 64;
            Close.Content = "X";
            Close.FontSize = 22;
            Close.Background = Brushes.Red;
            Close.Foreground = Brushes.White;
            Close.Margin = new Thickness(Header.Width - 78, 0, 0, 0);

            Close.Click += (sender, e) =>
            {
                this.NavigationService.Navigate(new Form_Tester());
            };


            Header.Children.Add(Close);


            ScrollViewer List = new ScrollViewer();
            List.Width = CanvasMainInformation.Width;
            List.Height = 400;
            List.Background = Brushes.LightGray;
            List.Margin = new Thickness(100, 410, 1, 100);
            Background.Children.Add(List);

            StackPanel Stack = new StackPanel();
            Stack.Width = List.Width;
            Stack.Height = 100;
            List.Content = Stack;

            int CountRows = 0;
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM {"_tsz_Check_" + id.ToString() + "_checkdata"}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        CountRows = Convert.ToInt32(result);
                    }
                }
            }

            Canvas[] Item = new Canvas[CountRows];
            TextBlock[] TextSteep = new TextBlock[CountRows];

            int i = 0;
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM _tsz_Check_" + id.ToString() + "_checkdata";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Item[i] = new Canvas();
                            Item[i].Width = List.Width;
                            Item[i].Height = 100;
                            Item[i].Background = Brushes.LightGray;
                            Item[i].Tag = i;
                            Item[i].MouseEnter += (sender, e) =>
                            {
                                ((Canvas)sender).Background = Brushes.White;
                            };
                            Item[i].MouseLeave += (sender, e) =>
                            {
                                ((Canvas)sender).Background = Brushes.LightGray;
                            };
                            // Добавляем обработчик события нажатия мыши
                            
                                Item[i].MouseDown += (sender, e) =>
                                {
                                    var canvas = (Canvas)sender;
                                    int index = (int)canvas.Tag;
                                    var textBlock = TextSteep[index];
                                    // Проверяем, зачёркнут ли текст

                                    if (textBlock.TextDecorations == TextDecorations.Strikethrough)
                                    {
                                        // Если текст зачёркнут, убираем зачёркивание
                                        textBlock.TextDecorations = null;
                                    }
                                    else
                                    {
                                        // Если текст не зачёркнут, добавляем зачёркивание
                                        textBlock.TextDecorations = TextDecorations.Strikethrough;
                                    }
                                };
                            
                            Stack.Children.Add(Item[i]);

                            TextSteep[i] = new TextBlock();
                            TextSteep[i].Tag = i;
                            TextSteep[i].FontSize = 16;
                            TextSteep[i].Text = reader["StepTest"].ToString();
                            TextSteep[i].Margin = new Thickness(20, 6, 0, 0);
                            // Устанавливаем начальное форматирование текста, если необходимо
                            // TextSteep[i].TextDecorations = TextDecorations.Strikethrough;

                            Stack.Height += 100;

                            Item[i].Children.Add(TextSteep[i]);

                            i++;
                        }
                    }
                }
            }



            return Background;

        }

        Canvas GenerateCreate(int id)
        {
            id++;

            Canvas Background = new Canvas();
            Background.Width = Application.Current.MainWindow.Width;
            Background.Height = Application.Current.MainWindow.Height + 100;
            Background.Background = Brushes.Gray;

            Canvas Header = new Canvas();
            Header.Width = Background.Width;
            Header.Height = 64;
            Header.Background = Brushes.LightGray;
            Background.Children.Add(Header);

            Canvas CanvasMainInformation = new Canvas();
            CanvasMainInformation.Width = Application.Current.MainWindow.Width - 200;
            CanvasMainInformation.Height = 240;
            CanvasMainInformation.Background = Brushes.White;
            CanvasMainInformation.Margin = new Thickness(100, 100, 1, 100);
            Background.Children.Add(CanvasMainInformation);

            string NameCL = "ЧекЛист";
            string PreCode = "_tsz_Check_";//сделать аббревиатуру для отделов тестирования
            string postcode = id.ToString();

            TextBlock TitleTextBox = new TextBlock();
            TitleTextBox.FontSize = 19;
            TitleTextBox.Text = "Название:" + NameCL + ":" + PreCode + "-" + postcode;
            TitleTextBox.Margin = new Thickness(42, 5, 0, 0);
            CanvasMainInformation.Children.Add(TitleTextBox);

            string[] Attributes = { "Приоритет:", "Метки:", "Компоненты:", "Кто создал:", "Проекты:" };

            TextBlock[] Parametrs = new TextBlock[Attributes.Length];

            Button ImageRedact = new Button();
            ImageRedact.Width = 30;
            ImageRedact.Height = 30;
            ImageRedact.Background = Brushes.Transparent;
            ImageRedact.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact = new Image();
            ImageRedact.Content = IcoRedact;
            IcoRedact.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png"));
            CanvasMainInformation.Children.Add(ImageRedact);
            ImageRedact.Margin = new Thickness(10, 3, 0, 0);



            for (int i = 0; i < Attributes.Length; i++)
            {
                Parametrs[i] = new TextBlock();
                Parametrs[i].Text = Attributes[i];
                Parametrs[i].FontSize = 15;
                Parametrs[i].Margin = new Thickness(10, 50 + (i * 32), 0, 0);
                CanvasMainInformation.Children.Add(Parametrs[i]);
            }

            ComboBox Priotity = new ComboBox();
            Priotity.Width = 200;
            Priotity.Height = 24;
            Priotity.Items.Add("Низкий");
            Priotity.Items.Add("Средний");
            Priotity.Items.Add("Высокий");
            Priotity.Items.Add("КРИТИЧЕСКИЙ");
            Priotity.SelectedIndex = 1;
            Priotity.Margin = new Thickness(96, 52, 0, 0);
            CanvasMainInformation.Children.Add(Priotity);

            TextBox Labels = new TextBox();
            Labels.Width = 200;
            Labels.Height = 24;
            Labels.Margin = new Thickness(96, 79, 0, 0);
            CanvasMainInformation.Children.Add(Labels);

            TextBox Components = new TextBox();
            Components.Width = 200;
            Components.Height = 24;
            Components.Margin = new Thickness(120, 112, 0, 0);
            CanvasMainInformation.Children.Add(Components);

            ComboBox Maker = new ComboBox();
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Запрос к базе данных для выборки данных из таблицы Projects
                string query = "SELECT * FROM Humans";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string MakerName = reader["Firtsname"].ToString() + " " + reader["SecondName"].ToString() + " " + reader["ThirdName"].ToString();
                        Maker.Items.Add(MakerName); // Добавляем каждое название проекта в ComboBox
                    }
                }
            }
            Maker.Width = 200;
            Maker.Height = 24;
            Maker.Margin = new Thickness(120, 145, 0, 0);
            CanvasMainInformation.Children.Add(Maker);



            ComboBox Projects = new ComboBox();
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Запрос к базе данных для выборки данных из таблицы Projects
                string query = "SELECT NameProject FROM Projects";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string projectName = reader["NameProject"].ToString();
                        Projects.Items.Add(projectName); // Добавляем каждое название проекта в ComboBox
                    }
                }
            }
            Projects.Width = 200;
            Projects.Height = 24;
            Projects.Margin = new Thickness(96, 178, 0, 0);
            CanvasMainInformation.Children.Add(Projects);


            ScrollViewer List = new ScrollViewer();
            List.Width = CanvasMainInformation.Width;
            List.Height = 400;
            List.Background = Brushes.LightGray;
            List.Margin = new Thickness(100, 360, 1, 100);
            Background.Children.Add(List);

            StackPanel Stack = new StackPanel();
            Stack.Width = List.Width;
            Stack.Height = 400;
            List.Content = Stack;


            int CountRows = 1;
            Canvas[] Item = new Canvas[CountRows];
            Canvas AddCanvas = new Canvas();
            AddCanvas.Width = Stack.Width;
            AddCanvas.Height = 100;
            AddCanvas.Background = Brushes.Green; AddCanvas.MouseEnter += (sender, e) =>
            {
                AddCanvas.Background = Brushes.Lime;
            };
            AddCanvas.MouseLeave += (sender, e) =>
            {
                AddCanvas.Background = Brushes.Green;
            };
            Stack.Children.Add(AddCanvas);
            TextBlock AddCanvasText = new TextBlock();
            AddCanvasText.FontSize = 50;
            AddCanvasText.Text = "+";
            AddCanvasText.Margin = new Thickness(AddCanvas.Width / 2 - AddCanvasText.FontSize / 2, 18, 0, 0);
            AddCanvas.Children.Add(AddCanvasText);


            Image icoPrew = new Image();
            icoPrew.Width = 64;
            icoPrew.Height = 64;
            icoPrew.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/preview.png"));
            Header.Children.Add(icoPrew);

            Button Close = new Button();
            Close.Width = 64;
            Close.Height = 64;
            Close.Content = "X";
            Close.FontSize = 22;
            Close.Background = Brushes.Red;
            Close.Foreground = Brushes.White;
            Close.Margin = new Thickness(Header.Width - 78, 0, 0, 0);
            Header.Children.Add(Close);

            Close.Click += (sender, e) =>
            {
                this.NavigationService.Navigate(new Form_Tester());
            };



            List<TextBox> tableItemsList = new List<TextBox>();

            AddCanvas.MouseLeftButtonDown += (sender, e) =>
            {

                // Сохраняем ссылку на AddCanvas
                UIElement addCanvasReference = AddCanvas;

                // Создаем новый объект TableItem
                TextBox tableItem = new TextBox();
                tableItem.Tag = "Шаг " + CountRows.ToString();

                // Добавляем tableItem в список
                tableItemsList.Add(tableItem);

                // Увеличиваем высоту StackPanel
                Stack.Height += 30;
                CountRows++;

                // Удаляем AddCanvas из StackPanel
                Stack.Children.Remove(AddCanvas);

                // Добавляем новый объект TableItem в StackPanel
                Stack.Children.Add(tableItem);

                // Добавляем AddCanvas обратно в StackPanel, чтобы он был последним элементом
                Stack.Children.Add(addCanvasReference);

                /*
                tableItem.RequestDelete += (senderDelete, eDelete) =>
                {
                    Stack.Height -= 100;
                    CountRows++;
                    Stack.Children.Remove(tableItem.newItem);
                    tableItemsList.Remove(tableItem);
                };
                */

            };



            Button Prewiew = new Button();
            Prewiew.Width = 210;
            Prewiew.Height = Header.Height;
            Prewiew.Content = "  Предпросмотр";
            Prewiew.Background = Brushes.AliceBlue;
            Header.Children.Add(Prewiew);

            TestCaseTableDB table = new TestCaseTableDB();

            Prewiew.MouseEnter += (sender, e) =>
            {
                Prewiew.Background = Brushes.White;
            };

            Prewiew.Click += (sender, e) =>
            {
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string NameTable = $"{PreCode}{id}";
                    string query1 = $"INSERT INTO AllCheckLists VALUES (@id, @NameCL, @Maker, @Projects, 0)";
                    using (MySqlCommand AddTK = new MySqlCommand(query1, connection))
                    {
                        AddTK.Parameters.AddWithValue("@id", id);
                        AddTK.Parameters.AddWithValue("@NameCL", NameCL);
                        AddTK.Parameters.AddWithValue("@Maker", Maker.SelectedItem.ToString());
                        AddTK.Parameters.AddWithValue("@Projects", Projects.SelectedItem.ToString());
                        AddTK.ExecuteNonQuery();
                    }

                    string createUsersTableQuery = $"CREATE TABLE {NameTable} (id INT, name VARCHAR(60), priority INT, labels VARCHAR(100), components VARCHAR(100), maker VARCHAR(60), Project VARCHAR(60))";
                    using (MySqlCommand createUsersTableCommand = new MySqlCommand(createUsersTableQuery, connection))
                    {
                        createUsersTableCommand.ExecuteNonQuery();
                    }

                    string query2 = $"INSERT INTO {NameTable} (id, name, priority, labels, components, maker, Project) VALUES (@id, @NameTable, @Priority, @Labels, @Components, @Maker, @Projects)";
                    using (MySqlCommand AddData = new MySqlCommand(query2, connection))
                    {
                        AddData.Parameters.AddWithValue("@id", id);
                        AddData.Parameters.AddWithValue("@NameTable", NameTable);
                        AddData.Parameters.AddWithValue("@Priority", Priotity.SelectedIndex);
                        AddData.Parameters.AddWithValue("@Labels", Labels.Text);
                        AddData.Parameters.AddWithValue("@Components", Components.Text);
                        AddData.Parameters.AddWithValue("@Maker", Maker.SelectedItem.ToString());
                        AddData.Parameters.AddWithValue("@Projects", Projects.SelectedItem.ToString());
                        AddData.ExecuteNonQuery();
                    }

                    string createCheckDataTableQuery = $"CREATE TABLE {NameTable}_CheckData (id INT, StepTest TEXT)";
                    using (MySqlCommand createTestDataTableCommand = new MySqlCommand(createCheckDataTableQuery, connection))
                    {
                        createTestDataTableCommand.ExecuteNonQuery();
                    }

                    string AddDataCheckDataTableQuery = $"INSERT INTO {NameTable}_CheckData (id, StepTest) VALUES (@id, @CheckItem)";
                    int i = 0;
                    foreach (TextBox tableItem in tableItemsList)
                    {
                        using (MySqlCommand AddDataTableCommand = new MySqlCommand(AddDataCheckDataTableQuery, connection))
                        {
                            AddDataTableCommand.Parameters.AddWithValue("@id", i + 1);
                            AddDataTableCommand.Parameters.AddWithValue("@CheckItem", tableItem.Text);
                            AddDataTableCommand.ExecuteNonQuery();
                        }
                        i++;
                    }
                }
                ChangeStatus(0, id);
            };

            Prewiew.MouseLeave += (sender, e) =>
            {
                Prewiew.Background = Brushes.AliceBlue;
            };

            

            return Background;
        }

        Canvas GenerateRedact(int id)
        {

            string NameCL = "";
            string PreCode = "tsz";//сделать аббревиатуру для отделов тестирования
            string labels = "";
            string components = "";
            string postcode = id.ToString();
            string makerLabel = "";
            string projectLabel = "";

            int priority = 0;

            string NameTable = $"_{PreCode}_check_{id}";

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM AllTestCases WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Добавлен вызов метода Read()
                        {
                            NameCL = reader["NameTK"].ToString();
                            NameTable = $"_{PreCode}_check_{id}";
                        }
                        else
                        {
                            //ошибка
                        }
                    }


                }

                

                string queryData = $"SELECT * FROM {NameTable}";

                using (MySqlCommand command = new MySqlCommand(queryData, connection))
                {
                    using (MySqlDataReader readerDataTK = command.ExecuteReader())
                    {
                        if (readerDataTK.Read()) // Добавлен вызов метода Read()
                        {
                            makerLabel = readerDataTK["maker"].ToString();
                            projectLabel = readerDataTK["Project"].ToString();
                            labels = readerDataTK["labels"].ToString();
                            components = readerDataTK["components"].ToString();
                            priority = Convert.ToInt32(readerDataTK["priority"]);
                        }
                        else
                        {
                            // Обработка ситуации, когда запись не найдена
                        }
                    }
                }

            }

            string lastnameCl = NameCL;

            Canvas Background = new Canvas();
            Background.Width = Application.Current.MainWindow.Width;
            Background.Height = Application.Current.MainWindow.Height + 100;
            Background.Background = Brushes.Gray;

            Canvas Header = new Canvas();
            Header.Width = Background.Width;
            Header.Height = 64;
            Header.Background = Brushes.LightGray;
            Background.Children.Add(Header);

            Canvas CanvasMainInformation = new Canvas();
            CanvasMainInformation.Width = Application.Current.MainWindow.Width - 200;
            CanvasMainInformation.Height = 240;
            CanvasMainInformation.Background = Brushes.White;
            CanvasMainInformation.Margin = new Thickness(100, 100, 1, 100);
            Background.Children.Add(CanvasMainInformation);

            TextBlock TitleTextBox = new TextBlock();
            TitleTextBox.FontSize = 19;
            TitleTextBox.Text = "Название:" + NameCL + ":" + PreCode + "-" + postcode;
            TitleTextBox.Margin = new Thickness(42, 5, 0, 0);
            CanvasMainInformation.Children.Add(TitleTextBox);

            string[] Attributes = { "Приоритет:", "Метки:", "Компоненты:", "Кто создал:", "Проекты:" };

            TextBlock[] Parametrs = new TextBlock[Attributes.Length];

            Button ImageRedact = new Button();
            ImageRedact.Width = 30;
            ImageRedact.Height = 30;
            ImageRedact.Background = Brushes.Transparent;
            ImageRedact.BorderBrush = Brushes.Transparent;  // Устанавливаем изображение как содержимое кнопки
            Image IcoRedact = new Image();
            ImageRedact.Content = IcoRedact;
            IcoRedact.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png"));
            CanvasMainInformation.Children.Add(ImageRedact);
            ImageRedact.Margin = new Thickness(10, 3, 0, 0);



            for (int i = 0; i < Attributes.Length; i++)
            {
                Parametrs[i] = new TextBlock();
                Parametrs[i].Text = Attributes[i];
                Parametrs[i].FontSize = 15;
                Parametrs[i].Margin = new Thickness(10, 50 + (i * 32), 0, 0);
                CanvasMainInformation.Children.Add(Parametrs[i]);
            }


            ComboBox Priotity = new ComboBox();
            Priotity.Width = 200;
            Priotity.Height = 24;
            Priotity.Items.Add("Низкий");
            Priotity.Items.Add("Средний");
            Priotity.Items.Add("Высокий");
            Priotity.Items.Add("КРИТИЧЕСКИЙ");
            Priotity.SelectedIndex = priority;
            Priotity.Margin = new Thickness(96, 52, 0, 0);
            CanvasMainInformation.Children.Add(Priotity);

            TextBox Labels = new TextBox();
            Labels.Width = 200;
            Labels.Height = 24;
            Labels.Text = labels;
            Labels.Margin = new Thickness(96, 79, 0, 0);
            CanvasMainInformation.Children.Add(Labels);

            TextBox Components = new TextBox();
            Components.Width = 200;
            Components.Height = 24;
            Components.Text = components;
            Components.Margin = new Thickness(120, 112, 0, 0);
            CanvasMainInformation.Children.Add(Components);

            ComboBox Maker = new ComboBox();
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Запрос к базе данных для выборки данных из таблицы Projects
                string query = "SELECT * FROM Humans";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string MakerName = reader["Firtsname"].ToString() + " " + reader["SecondName"].ToString() + " " + reader["ThirdName"].ToString();
                        Maker.Items.Add(MakerName); // Добавляем каждое название проекта в ComboBox
                    }
                }
            }
            Maker.Text = makerLabel;
            Maker.Width = 200;
            Maker.Height = 24;
            Maker.Margin = new Thickness(120, 145, 0, 0);
            CanvasMainInformation.Children.Add(Maker);



            ComboBox Projects = new ComboBox();
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Запрос к базе данных для выборки данных из таблицы Projects
                string query = "SELECT NameProject FROM Projects";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string projectName = reader["NameProject"].ToString();
                        Projects.Items.Add(projectName); // Добавляем каждое название проекта в ComboBox
                    }
                }
            }
            Projects.Width = 200;
            Projects.Height = 24;
            Projects.Text = projectLabel;
            Projects.Margin = new Thickness(96, 178, 0, 0);
            CanvasMainInformation.Children.Add(Projects);


            ScrollViewer List = new ScrollViewer();
            List.Width = CanvasMainInformation.Width;
            List.Height = 400;
            List.Background = Brushes.LightGray;
            List.Margin = new Thickness(100, 360, 1, 100);
            Background.Children.Add(List);

            StackPanel Stack = new StackPanel();
            Stack.Width = List.Width;
            Stack.Height = 400;
            List.Content = Stack;


            int CountRows = 1;
            Canvas[] Item = new Canvas[CountRows];
            Canvas AddCanvas = new Canvas();
            AddCanvas.Width = Stack.Width;
            AddCanvas.Height = 100;
            AddCanvas.Background = Brushes.Green; AddCanvas.MouseEnter += (sender, e) =>
            {
                AddCanvas.Background = Brushes.Lime;
            };
            AddCanvas.MouseLeave += (sender, e) =>
            {
                AddCanvas.Background = Brushes.Green;
            };
            
            TextBlock AddCanvasText = new TextBlock();
            AddCanvasText.FontSize = 50;
            AddCanvasText.Text = "+";
            AddCanvasText.Margin = new Thickness(AddCanvas.Width / 2 - AddCanvasText.FontSize / 2, 18, 0, 0);
            AddCanvas.Children.Add(AddCanvasText);


            Image icoPrew = new Image();
            icoPrew.Width = 64;
            icoPrew.Height = 64;
            icoPrew.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/preview.png"));
            Header.Children.Add(icoPrew);

            Button Close = new Button();
            Close.Width = 64;
            Close.Height = 64;
            Close.Content = "X";
            Close.FontSize = 22;
            Close.Background = Brushes.Red;
            Close.Foreground = Brushes.White;
            Close.Margin = new Thickness(Header.Width - 78, 0, 0, 0);

            Close.Click += (sender, e) =>
            {
                this.NavigationService.Navigate(new Form_Tester());
            };

            Header.Children.Add(Close);

            List<TextBox> tableItemsList = new List<TextBox>();

            int cRowsPre = 0;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Запрос к базе данных для выборки данных из таблицы Projects
                string query = $"SELECT StepTest FROM {NameTable}_checkdata";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        TextBox tableItem = new TextBox();
                        tableItem.Tag = "Шаг " + CountRows.ToString();
                        tableItem.Text = reader["StepTest"].ToString();
                        Stack.Height += 30;
                        CountRows++;
                        tableItemsList.Add(tableItem);
                        Stack.Children.Add(tableItem);
                    }
                }
            }

            cRowsPre = CountRows;

            Stack.Children.Add(AddCanvas);


            AddCanvas.MouseLeftButtonDown += (sender, e) =>
                {

                // Сохраняем ссылку на AddCanvas
                UIElement addCanvasReference = AddCanvas;

                // Создаем новый объект TableItem
                TextBox tableItem = new TextBox();
                tableItem.Tag = "Шаг " + CountRows.ToString();

                // Добавляем tableItem в список
                tableItemsList.Add(tableItem);

                // Увеличиваем высоту StackPanel
                Stack.Height += 30;
                CountRows++;

                // Удаляем AddCanvas из StackPanel
                Stack.Children.Remove(AddCanvas);

                // Добавляем новый объект TableItem в StackPanel
                Stack.Children.Add(tableItem);

                // Добавляем AddCanvas обратно в StackPanel, чтобы он был последним элементом
                Stack.Children.Add(addCanvasReference);

                /*
                tableItem.RequestDelete += (senderDelete, eDelete) =>
                {
                    Stack.Height -= 100;
                    CountRows++;
                    Stack.Children.Remove(tableItem.newItem);
                    tableItemsList.Remove(tableItem);
                };
                */

            };



            Button Prewiew = new Button();
            Prewiew.Width = 210;
            Prewiew.Height = Header.Height;
            Prewiew.Content = "  Предпросмотр";
            Prewiew.Background = Brushes.AliceBlue;
            Header.Children.Add(Prewiew);

            TestCaseTableDB table = new TestCaseTableDB();

            Prewiew.MouseEnter += (sender, e) =>
            {
                Prewiew.Background = Brushes.White;
            };

            Prewiew.Click += (sender, e) =>
            {
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();

                    string NameTable_ = NameTable;

                    // Удаление всех записей из таблицы
                    string DeleteAllDataQuery = $"DELETE FROM {NameTable_}_CheckData";
                    using (MySqlCommand DeleteAllDataCommand = new MySqlCommand(DeleteAllDataQuery, connection))
                    {
                        DeleteAllDataCommand.ExecuteNonQuery();
                    }

                    for (int i = 0; i < CountRows-1; i++)
                    {
                        string AddDataCheckDataTableQuery = $"INSERT INTO {NameTable_}_CheckData (id, StepTest) VALUES (@id, @CheckItem)";
                        using (MySqlCommand AddDataTableCommand = new MySqlCommand(AddDataCheckDataTableQuery, connection))
                        {
                            AddDataTableCommand.Parameters.AddWithValue("@id", i + 1);
                            AddDataTableCommand.Parameters.AddWithValue("@CheckItem", tableItemsList[i].Text);
                            AddDataTableCommand.ExecuteNonQuery();
                        }
                    }

                    ChangeStatus(0, id);
                }
            };

            Prewiew.MouseLeave += (sender, e) =>
            {
                Prewiew.Background = Brushes.AliceBlue;
            };

            


            return Background;
        }


        public void ChangeStatus(int status, int id)
        {
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "UPDATE AllCheckLists SET StatusCheckList = @status WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();

                }

            }

            if (status > -1)
            {
                this.NavigationService.Navigate(new CheckList_ViewAndCreate("view", id));
            }
            else if (status == -1)
            {
                this.NavigationService.Navigate(new CheckList_ViewAndCreate("redact", id));
            }
        }




        public void GenerateButton(Canvas parentCanvas, int IndexStatus, int id, bool redactpage, int CountRows)
        {
            if (IndexStatus == 0)
            {
                Button ToCheck = new Button
                {
                    FontSize = 12,
                    Content = "Отправить на проверку",
                    Margin = new Thickness(2, parentCanvas.Height - 28, 0, 0)
                };
                ToCheck.Width = CalculateButtonWidth(ToCheck.Content.ToString(), ToCheck.FontSize);
                parentCanvas.Children.Add(ToCheck);

                //добавить проверку что Вы и есть тот самыый пользователь который создавал ТК

                Button ToRedact = new Button
                {
                    Content = "Редактировать",
                    FontSize = 12,
                    Margin = new Thickness(2 + ToCheck.Width + 4, parentCanvas.Height - 28, 0, 0)
                };

                ToRedact.Click += (sender, e) =>
                {
                    ChangeStatus(-1, id);
                };

                ToRedact.Width = CalculateButtonWidth(ToRedact.Content.ToString(), ToRedact.FontSize);
                parentCanvas.Children.Add(ToRedact);


                ToCheck.Click += (sender, e) =>
                {
                    ChangeStatus(1, id);
                };



            }
            if (IndexStatus == 1)
            {
                Button ToDevelop = new Button
                {
                    FontSize = 12,
                    Content = "Отправить на доработку",
                    Background = Brushes.Red,
                    Margin = new Thickness(2, parentCanvas.Height - 28, 0, 0)
                };
                ToDevelop.Width = CalculateButtonWidth(ToDevelop.Content.ToString(), ToDevelop.FontSize);
                parentCanvas.Children.Add(ToDevelop);

                Button ToConfirm = new Button
                {
                    FontSize = 12,
                    Content = "Подтвердить",
                    Margin = new Thickness(2 + ToDevelop.Width + 4, parentCanvas.Height - 28, 0, 0)
                };
                ToConfirm.Width = CalculateButtonWidth(ToConfirm.Content.ToString(), ToConfirm.FontSize);
                parentCanvas.Children.Add(ToConfirm);

                ToDevelop.Click += (sender, e) =>
                {
                    ChangeStatus(2, id);
                };
                ToConfirm.Click += (sender, e) =>
                {
                    ChangeStatus(3, id);
                };
            }
            if (IndexStatus == -1 || IndexStatus == 2 || IndexStatus == 3)
            {

                Button ToRedact = new Button
                {
                    Content = "Редактировать",
                    FontSize = 12,
                    Margin = new Thickness(2, parentCanvas.Height - 28, 0, 0)
                };
                if (redactpage == false)
                {



                    ToRedact.Click += (sender, e) =>
                    {
                        ChangeStatus(-1, id);
                    };

                    ToRedact.Width = CalculateButtonWidth(ToRedact.Content.ToString(), ToRedact.FontSize);
                    parentCanvas.Children.Add(ToRedact);
                }

                if (IndexStatus == 3)
                {
                    Button ToBegin = new Button
                    {
                        Content = "Начать тестирование",
                        FontSize = 12,
                        Margin = new Thickness(2 + ToRedact.Width + 4, parentCanvas.Height - 28, 0, 0)
                    };
                    ToBegin.Width = CalculateButtonWidth(ToBegin.Content.ToString(), ToBegin.FontSize);
                    parentCanvas.Children.Add(ToBegin);

                    ToBegin.Click += (sender, e) =>
                    {
                        ChangeStatus(4, id);
                    };
                }

            }
            if (IndexStatus == 4)
            {
                Button Complete = new Button
                {
                    FontSize = 12,
                    Content = "Завершить тестирование",
                    Background = Brushes.LimeGreen,
                    Margin = new Thickness(2, parentCanvas.Height - 28, 0, 0)
                };
                Complete.Width = CalculateButtonWidth(Complete.Content.ToString(), Complete.FontSize);
                parentCanvas.Children.Add(Complete);

                ComboBox comboStatusTest = new ComboBox();
                comboStatusTest.Items.Add("Не выполнялся");
                comboStatusTest.Items.Add("Выполнен");
                comboStatusTest.Items.Add("Провален");
                comboStatusTest.Items.Add("В процессе");
                comboStatusTest.Items.Add("Заблокирован");
                comboStatusTest.SelectedIndex = 3;
                comboStatusTest.Width = 200;
                comboStatusTest.Height = 24;
                comboStatusTest.Margin = new Thickness(2 + Complete.Width + 6, parentCanvas.Height - 26, 0, 0);
                parentCanvas.Children.Add(comboStatusTest);

                Complete.Click += (sender, e) =>
                {
                    //записывать данные прохождения 
                    testing = false;
                    ChangeStatus(10 + comboStatusTest.SelectedIndex, id);
                };

            }
            if (IndexStatus >= 10)
            {



                Button ToRedact = new Button
                {
                    Content = "Редактировать",
                    FontSize = 12,
                    Margin = new Thickness(2, parentCanvas.Height - 28, 0, 0)
                };

                ToRedact.Click += (sender, e) =>
                {
                    ChangeStatus(-1, id);
                };

                ToRedact.Width = CalculateButtonWidth(ToRedact.Content.ToString(), ToRedact.FontSize);
                parentCanvas.Children.Add(ToRedact);

                Button ToBegin = new Button
                {
                    Content = "Начать тестирование",
                    FontSize = 12,
                    Margin = new Thickness(2 + ToRedact.Width + 4, parentCanvas.Height - 28, 0, 0)
                };
                ToBegin.Width = CalculateButtonWidth(ToBegin.Content.ToString(), ToBegin.FontSize);
                parentCanvas.Children.Add(ToBegin);

                ToBegin.Click += (sender, e) =>
                {
                    testing = true;
                    ChangeStatus(4, id);
                };

            }



        }



        private int CalculateButtonWidth(string buttonText, double fontSize)
        {
            return (int)(buttonText.Length * (fontSize / 2)) + 16;
        }

        public string generateStatusCase(int statusIndex)
        {
            string text = "Статус" + ":";
            string status = "";

            switch (statusIndex)
            {
                case -1:
                    status = "Редактирование";
                    text += status;
                    break;
                case 0:
                    status = "Черновик";
                    text += status;
                    break;
                case 1:
                    status = "На проверке";
                    text += status;
                    break;
                case 2:
                    status = "На доработке";
                    text += status;
                    break;
                case 3:
                    status = "Подтверждён";
                    text += status;
                    break;
                case 4:
                    status = "В процессе прохождения";
                    text += status;
                    break;
                case 10:
                    status = "Тестировался. Не проверялся";
                    text += status;
                    break;
                case 11:
                    status = "Тестировался. Выполнен";
                    text += status;
                    break;
                case 12:
                    status = "Тестировался. Провален";
                    text += status;
                    break;
                case 13:
                    status = "Тестировался. В процессе";
                    text += status;
                    break;
                case 14:
                    status = "Тестировался. Заблокирован";
                    text += status;
                    break;

            }




            return text;
        }



    }
}
