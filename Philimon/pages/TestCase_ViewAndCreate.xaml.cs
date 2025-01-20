using MySql.Data.MySqlClient;
using Philimon.objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для TestCase_ViewAndCreate.xaml
    /// </summary>
    public partial class TestCase_ViewAndCreate : Page
    {
        DatabaseData DB = new DatabaseData();

#pragma warning disable CS0169 // Поле "TestCase_ViewAndCreate.SteepTitle" никогда не используется.
        RichTextBox SteepTitle;
#pragma warning restore CS0169 // Поле "TestCase_ViewAndCreate.SteepTitle" никогда не используется.
#pragma warning disable CS0169 // Поле "TestCase_ViewAndCreate.DataTest" никогда не используется.
        RichTextBox DataTest;
#pragma warning restore CS0169 // Поле "TestCase_ViewAndCreate.DataTest" никогда не используется.
#pragma warning disable CS0169 // Поле "TestCase_ViewAndCreate.Result" никогда не используется.
        RichTextBox Result;
#pragma warning restore CS0169 // Поле "TestCase_ViewAndCreate.Result" никогда не используется.

        public TestCase_ViewAndCreate(string Use, int id)
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                LoadPage(Use, id);
            };
        }

        void LoadPage(string Use, int id)
        {
            GenerateUI(Use, id);
        }

        void GenerateUI(string Use, int id)
        {
            ScrollViewer Viewer = new ScrollViewer();
            Viewer.Width = Application.Current.MainWindow.Width;
            Viewer.Height = Application.Current.MainWindow.Height;
            Grid.Children.Add(Viewer);

            if (Use == "view")
            {
                Viewer.Content = ViewTK(id);
            }
            if (Use == "create")
            {
                Viewer.Content = MadeTK(id);
            }
            if (Use == "redact")
            {
                Viewer.Content = RedactTK(id);
            }

        }



        public Canvas ViewTK(int id)
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



            string NameTK = "";
            string Creater = "";
            string Project = "";
            int indexStatus = 0;

            int countRows = 0;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string query = "SELECT *FROM AllTestCases WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id",id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NameTK = reader["NameTK"].ToString();
                            Creater = reader["Creater"].ToString();
                            Project = reader["Project"].ToString();
                            indexStatus = Convert.ToInt32(reader["StatusCase"]);
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
                string query = "SELECT *FROM  tsz_" + id.ToString();
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
            TitleTextBox.Text = "Название:" + NameTK;
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




            //таблица
            Canvas TableHeader = new Canvas();
            TableHeader.Width = 1500;
            TableHeader.Height = 40;
            TableHeader.Background = Brushes.LightGray;
            TableHeader.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - TableHeader.Width / 2, 430, 0, 0);
            Background.Children.Add(TableHeader);

            StackPanel Stack = new StackPanel();
            Stack.Width = TableHeader.Width;
            Stack.Height = 400;

            ScrollViewer ScrollList = new ScrollViewer();
            ScrollList.Width = TableHeader.Width;
            ScrollList.Height = 400;
            ScrollList.Background = Brushes.White;
            ScrollList.Margin = new Thickness(0, 40, 0, 0);
            ScrollList.Content = Stack;
            TableHeader.Children.Add(ScrollList);

            TextBlock Attr = new TextBlock();
            Attr.Text = "Шаг №";
            Attr.FontSize = 14;
            Attr.Margin = new Thickness(5, 6, 0, 0);
            TableHeader.Children.Add(Attr);

            TextBlock Attr2 = new TextBlock();
            Attr2.Text = "Шаг теста";
            Attr2.FontSize = 14;
            Attr2.Margin = new Thickness(100, 6, 0, 0);
            TableHeader.Children.Add(Attr2);

            TextBlock Attr3 = new TextBlock();
            Attr3.Text = "Данные теста";
            Attr3.FontSize = 14;
            Attr3.Margin = new Thickness(320, 6, 0, 0);
            TableHeader.Children.Add(Attr3);

            TextBlock Attr4 = new TextBlock();
            Attr4.Text = "Ожидаемый результат";
            Attr4.FontSize = 14;
            Attr4.Margin = new Thickness(610, 6, 0, 0);
            TableHeader.Children.Add(Attr4);

            TextBlock Attr5 = new TextBlock();
            Attr5.Text = "Статус теста:";
            Attr5.FontSize = 14;
            Attr5.Margin = new Thickness(900, 6, 0, 0);
            TableHeader.Children.Add(Attr5);

            TextBlock Attr6 = new TextBlock();
            Attr6.Text = "Комментарий:";
            Attr6.FontSize = 14;
            Attr6.Margin = new Thickness(1190, 6, 0, 0);
            TableHeader.Children.Add(Attr6);



            int CountRows = 0;
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM tsz_" + id.ToString() + "_testdata";

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
            TextBlock[] TextData = new TextBlock[CountRows];
            TextBlock[] TextResult = new TextBlock[CountRows];

            ComboBox[] comboboxes = new ComboBox[CountRows];
            TextBlock[] indexesBlock = new TextBlock[CountRows];
            RichTextBox[] comment = new RichTextBox[CountRows];

            int[] comboboxesIndex = new int[CountRows];
            string[] commentText = new string[CountRows];

            int i = 0;
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tsz_" + id.ToString() + "_testdata";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Item[i] = new Canvas();
                            Item[i].Width = 1500;
                            Item[i].Height = 100;
                            Item[i].Background = Brushes.LightGray;
                            Item[i].MouseEnter += (sender, e) =>
                            {
                                ((Canvas)sender).Background = Brushes.White;
                            };
                            Item[i].MouseLeave += (sender, e) =>
                            {
                                ((Canvas)sender).Background = Brushes.LightGray;
                            };
                            Stack.Children.Add(Item[i]);

                            TextSteep[i] = new TextBlock();
                            TextSteep[i].Text = reader["StepTest"].ToString();
                            TextData[i] = new TextBlock();
                            TextData[i].Text = reader["TestData"].ToString();
                            TextResult[i] = new TextBlock();
                            TextResult[i].Text = reader["Result"].ToString();

                            TextSteep[i].Margin = new Thickness(100, 6, 0, 0);
                            TextData[i].Margin = new Thickness(320, 6, 0, 0);
                            TextResult[i].Margin = new Thickness(610, 6, 0, 0);


                            Item[i].Children.Add(TextSteep[i]);
                            Item[i].Children.Add(TextData[i]);
                            Item[i].Children.Add(TextResult[i]);

                            comboboxes[i] = new ComboBox();
                            comboboxes[i].Tag = i+1.ToString();
                            comboboxes[i] = GenerateSetStatus(260, 24, 900, 24);
                            comboboxes[i].SelectedIndex = Convert.ToInt32(reader["ResultIndex"]);
                            Item[i].Children.Add(comboboxes[i]);



                            indexesBlock[i] = new TextBlock();
                            indexesBlock[i].Text = (i + 1).ToString();
                            indexesBlock[i].FontSize = 16;
                            indexesBlock[i].Margin = new Thickness(2, 36, 0, 0);
                            Item[i].Children.Add(indexesBlock[i]);



                            comment[i] = new RichTextBox();
                            comment[i].Width = 200;
                            comment[i].Height = 80;
                            comment[i].Margin = new Thickness(1190, 6, 1, 0);
                            comment[i].Tag = i.ToString();
                            Item[i].Children.Add(comment[i]);


                            if (indexStatus != 4)
                            {
                                comboboxes[i].IsEnabled = false;
                                comment[i].IsEnabled = false;
                            }
                            else
                            {
                                comboboxes[i].IsEnabled = true;
                                comment[i].IsEnabled = true;
                            }



                            Stack.Height += 100;

                            i++;
                        }

                    }
                }

            }

            return Background;

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

        public ComboBox GenerateSetStatus(double width, double height, double X, double Y)
        {
            string[] statusText = { "Не проверялось", "Пройдено", "Провалено", "Заблокировано", "В процессе" };

            ComboBox Status = new ComboBox();

            Status.Width = width;
            Status.Height = height;

            Status.Margin = new Thickness(X, Y, 0, 0);

            for (int i = 0; i < statusText.Length; i++)
            {
                Status.Items.Add(statusText[i]);
            }

            Status.SelectedIndex = 0;

            return Status;
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
                if (redactpage == false) {
                    
                    

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
                        
                        ChangeStatus(4, id);
                    };
                
            }



        }



        




        private int CalculateButtonWidth(string buttonText, double fontSize)
        {
            return (int)(buttonText.Length * (fontSize / 2)) + 16;
        }

        public void ChangeStatus(int status, int id)
        {
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string query = "UPDATE AllTestCases SET StatusCase = @status WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();

                }

            }

            if (status > -1)
            {
                this.NavigationService.Navigate(new TestCase_ViewAndCreate("view", id));
            }
            else if (status == -1)
            {
                this.NavigationService.Navigate(new TestCase_ViewAndCreate("redact", id));
            }
        }



        public Canvas MadeTK(int id)
        {
            id++;
            string NameTK = "Тесткейс";
            string PreCode = "TSZ";//сделать аббревиатуру для отделов тестирования
            string postcode = id.ToString();
#pragma warning disable CS0219 // Переменная назначена, но ее значение не используется
            string makerLabel = "";
#pragma warning restore CS0219 // Переменная назначена, но ее значение не используется
#pragma warning disable CS0219 // Переменная назначена, но ее значение не используется
            string projectLabel = "";
#pragma warning restore CS0219 // Переменная назначена, но ее значение не используется

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
            TitleTextBox.Text = "Название:" + NameTK + ":" + PreCode + "-" + postcode;
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


            ImageRedact.Click += (sender, e) =>
            {
                Canvas backgroundWindow = new Canvas();
                backgroundWindow.Width = Application.Current.MainWindow.Width;
                backgroundWindow.Height = Application.Current.MainWindow.Height;
                backgroundWindow.Opacity = 0.9;
                backgroundWindow.Background = Brushes.Black;
                Background.Children.Add(backgroundWindow);

                Canvas redactTitleWind = new Canvas();
                redactTitleWind.Width = 600;
                redactTitleWind.Height = 300;
                redactTitleWind.Background = Brushes.White;
                redactTitleWind.Opacity = 1;
                redactTitleWind.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - redactTitleWind.Width / 2, backgroundWindow.Height / 2 - redactTitleWind.Height / 2, 0, 0);
                Background.Children.Add(redactTitleWind);


                TextBlock Title = new TextBlock();
                Title.Text = "Редактировать название:";
                Title.Margin = new Thickness(5,5,0,0);
                Title.FontSize = 16;
                redactTitleWind.Children.Add(Title);

                TextBox newName = new TextBox();
                newName.Tag = "Имя: " + NameTK;
                newName.Width = 400;
                newName.Height = 40;
                newName.Margin = new Thickness(100,120,0,0);
                redactTitleWind.Children.Add(newName);

                Button redactName = new Button();
                redactName.Width = 300;
                redactName.Content = "Сохранить";
                redactName.Margin = new Thickness(redactTitleWind.Width/2 - redactName.Width, redactTitleWind.Height - 30, 0, 0);
                redactTitleWind.Children.Add(redactName);

                redactName.Click += (sender1, e1) =>
                {
                    if (!string.IsNullOrEmpty(newName.Text))
                    {
                        NameTK = newName.Text;
                        TitleTextBox.Text = "Название:" + NameTK + ":" + PreCode + "-" + postcode;
                        Background.Children.Remove(backgroundWindow);
                        Background.Children.Remove(redactTitleWind);
                    }
                };

                Button Back = new Button();
                Back.Width = 300;
                Back.Content = "Назад";
                Back.Background = Brushes.Red;
                Back.Margin = new Thickness(redactTitleWind.Width / 2, redactTitleWind.Height - 30, 0, 0);
                redactTitleWind.Children.Add(Back);

                Back.Click += (sender1, e1) =>
                {
                    Background.Children.Remove(backgroundWindow);
                    Background.Children.Remove(redactTitleWind);
                };

            };


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

            //таблица
            Canvas TableHeader = new Canvas();
            TableHeader.Width = 900;
            TableHeader.Height = 40;
            TableHeader.Background = Brushes.LightGray;
            TableHeader.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - TableHeader.Width / 2, 430, 0, 0);
            Background.Children.Add(TableHeader);

            StackPanel Stack = new StackPanel();
            Stack.Width = TableHeader.Width;
            Stack.Height = 400;

            ScrollViewer ScrollList = new ScrollViewer();
            ScrollList.Width = 900;
            ScrollList.Height = 400;
            ScrollList.Background = Brushes.White;
            ScrollList.Margin = new Thickness(0, 40, 0, 0);
            ScrollList.Content = Stack;
            TableHeader.Children.Add(ScrollList);

            TextBlock Attr = new TextBlock();
            Attr.Text = "Шаг №";
            Attr.FontSize = 14;
            Attr.Margin = new Thickness(5, 6, 0, 0);
            TableHeader.Children.Add(Attr);

            TextBlock Attr2 = new TextBlock();
            Attr2.Text = "Шаг теста";
            Attr2.FontSize = 14;
            Attr2.Margin = new Thickness(100, 6, 0, 0);
            TableHeader.Children.Add(Attr2);

            TextBlock Attr3 = new TextBlock();
            Attr3.Text = "Данные теста";
            Attr3.FontSize = 14;
            Attr3.Margin = new Thickness(320, 6, 0, 0);
            TableHeader.Children.Add(Attr3);

            TextBlock Attr4 = new TextBlock();
            Attr4.Text = "Ожидаемый результат";
            Attr4.FontSize = 14;
            Attr4.Margin = new Thickness(610, 6, 0, 0);
            TableHeader.Children.Add(Attr4);

            int CountRows = 1;
            Canvas[] Item = new Canvas[CountRows];
            Canvas AddCanvas = new Canvas();
            AddCanvas.Width = 900;
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

            
            List<TestItem> tableItemsList = new List<TestItem>();

            AddCanvas.MouseLeftButtonDown += (sender, e) =>
            {

                // Сохраняем ссылку на AddCanvas
                UIElement addCanvasReference = AddCanvas;

                // Создаем новый объект TableItem
                TestItem tableItem = new TestItem();

                // Добавляем tableItem в список
                tableItemsList.Add(tableItem);

                // Увеличиваем высоту StackPanel
                Stack.Height += 100;
                CountRows++;

                // Удаляем AddCanvas из StackPanel
                Stack.Children.Remove(AddCanvas);

                // Добавляем новый объект TableItem в StackPanel
                Stack.Children.Add(tableItem.newItem);

                // Добавляем AddCanvas обратно в StackPanel, чтобы он был последним элементом
                Stack.Children.Add(addCanvasReference);

                tableItem.RequestDelete += (senderDelete, eDelete) =>
                {
                    Stack.Height -= 100;
                    CountRows++;
                    Stack.Children.Remove(tableItem.newItem);
                    tableItemsList.Remove(tableItem);
                };

            };

            GenerateButton(CanvasMainInformation, -1, id, true, CountRows);

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

                    string NameTable = $"{PreCode}_{id}";

                    string query1 = $"INSERT INTO AllTestCases VALUES ({id}, '{NameTK}', '{Maker.SelectedItem.ToString()}', '{Projects.SelectedItem.ToString()}', 0)";

                    using (MySqlCommand AddTK = new MySqlCommand(query1, connection))
                    {
                        AddTK.ExecuteNonQuery();
                    }



                    string createUsersTableQuery = $"CREATE TABLE {NameTable} (id INT, name VARCHAR(60), priority INT, labels VARCHAR(100), components VARCHAR(100), maker VARCHAR(60), Project VARCHAR(60))";

                    using (MySqlCommand createUsersTableCommand = new MySqlCommand(createUsersTableQuery, connection))
                    {
                        createUsersTableCommand.ExecuteNonQuery();
                    }


                    string query2 = $"INSERT INTO {NameTable} (id, name, priority, labels, components, maker, Project) VALUES ({id}, '{NameTable}', '{Priotity.SelectedIndex}', '{Labels.Text}', '{Components.Text}', '{Maker.SelectedItem.ToString()}', '{Projects.SelectedItem.ToString()}')";

                    using (MySqlCommand AddData = new MySqlCommand(query2, connection))
                    {
                        AddData.ExecuteNonQuery();
                    }


                    string createTestDataTableQuery = $"CREATE TABLE {NameTable}_TestData (id INT, StepTest TEXT, TestData TEXT, Result TEXT, ResultIndex INT, Comment TEXT)";


                    using (MySqlCommand createTestDataTableCommand = new MySqlCommand(createTestDataTableQuery, connection))
                    {
                        createTestDataTableCommand.ExecuteNonQuery();
                    }


                    //richText1[i] = new TextRange(tableItemsList[i].SteepTitle.Document.ContentStart, tableItemsList[i].SteepTitle.Document.ContentEnd).Text;
                    //richText2[i] = new TextRange(tableItemsList[i].DataTest.Document.ContentStart, tableItemsList[i].DataTest.Document.ContentEnd).Text;
                    //richText3[i] = new TextRange(tableItemsList[i].Result.Document.ContentStart, tableItemsList[i].Result.Document.ContentEnd).Text;

                    string insertTestDataQuery = $"INSERT INTO {NameTable}_TestData (id, StepTest, TestData, Result, ResultIndex) VALUES (@id, @column1, @column2, @column3, @column4)";

                    int i = 0;
                    foreach (TestItem tableItem in tableItemsList)
                    {
                        using (MySqlCommand insertTestDataCommand = new MySqlCommand(insertTestDataQuery, connection))
                        {
                            insertTestDataCommand.Parameters.AddWithValue("@id", i + 1);
                            insertTestDataCommand.Parameters.AddWithValue("@column1", new TextRange(tableItemsList[i].SteepTitle.Document.ContentStart, tableItemsList[i].SteepTitle.Document.ContentEnd).Text);
                            insertTestDataCommand.Parameters.AddWithValue("@column2", new TextRange(tableItemsList[i].DataTest.Document.ContentStart, tableItemsList[i].DataTest.Document.ContentEnd).Text);
                            insertTestDataCommand.Parameters.AddWithValue("@column3", new TextRange(tableItemsList[i].Result.Document.ContentStart, tableItemsList[i].Result.Document.ContentEnd).Text);
                            insertTestDataCommand.Parameters.AddWithValue("@column4", 0);

                            insertTestDataCommand.ExecuteNonQuery();
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

            return Background;
        }


        public Canvas RedactTK(int id)
        {

            string NameTK = "";
            string PreCode = "TSZ";//сделать аббревиатуру для отделов тестирования
            string labels = "";
            string components = "";
            string postcode = id.ToString();
            string makerLabel = "";
            string projectLabel = "";

            int priority = 0;

            string NameTable = $"{PreCode}_{id}";

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
                            NameTK = reader["NameTK"].ToString();
                            NameTable = $"{PreCode}_{id}";
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

            string lastnameTK = NameTK;

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
            TitleTextBox.Text = "Название:" + NameTK + ":" + PreCode + "-" + postcode;
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


            ImageRedact.Click += (sender, e) =>
            {
                Canvas backgroundWindow = new Canvas();
                backgroundWindow.Width = Application.Current.MainWindow.Width;
                backgroundWindow.Height = Application.Current.MainWindow.Height;
                backgroundWindow.Opacity = 0.9;
                backgroundWindow.Background = Brushes.Black;
                Background.Children.Add(backgroundWindow);

                Canvas redactTitleWind = new Canvas();
                redactTitleWind.Width = 600;
                redactTitleWind.Height = 300;
                redactTitleWind.Background = Brushes.White;
                redactTitleWind.Opacity = 1;
                redactTitleWind.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - redactTitleWind.Width / 2, backgroundWindow.Height / 2 - redactTitleWind.Height / 2, 0, 0);
                Background.Children.Add(redactTitleWind);


                TextBlock Title = new TextBlock();
                Title.Text = "Редактировать название:";
                Title.Margin = new Thickness(5, 5, 0, 0);
                Title.FontSize = 16;
                redactTitleWind.Children.Add(Title);

                TextBox newName = new TextBox();
                newName.Tag = "Имя: " + NameTK;
                newName.Width = 400;
                newName.Height = 40;
                newName.Margin = new Thickness(100, 120, 0, 0);
                redactTitleWind.Children.Add(newName);

                Button redactName = new Button();
                redactName.Width = 300;
                redactName.Content = "Сохранить";
                redactName.Margin = new Thickness(redactTitleWind.Width / 2 - redactName.Width, redactTitleWind.Height - 30, 0, 0);
                redactTitleWind.Children.Add(redactName);

                redactName.Click += (sender1, e1) =>
                {
                    if (!string.IsNullOrEmpty(newName.Text))
                    {
                        NameTK = newName.Text;
                        TitleTextBox.Text = "Название:" + NameTK + ":" + PreCode + "-" + postcode;
                        Background.Children.Remove(backgroundWindow);
                        Background.Children.Remove(redactTitleWind);
                    }
                };

                Button Back = new Button();
                Back.Width = 300;
                Back.Content = "Назад";
                Back.Background = Brushes.Red;
                Back.Margin = new Thickness(redactTitleWind.Width / 2, redactTitleWind.Height - 30, 0, 0);
                redactTitleWind.Children.Add(Back);

                Back.Click += (sender1, e1) =>
                {
                    Background.Children.Remove(backgroundWindow);
                    Background.Children.Remove(redactTitleWind);
                };

            };


            for (int i = 0; i < Attributes.Length; i++)
            {
                Parametrs[i] = new TextBlock();
                Parametrs[i].Text = Attributes[i];
                Parametrs[i].FontSize = 15;
                Parametrs[i].Margin = new Thickness(10, 50 + (i * 32), 0, 0);
                CanvasMainInformation.Children.Add(Parametrs[i]);
            }

            ComboBox Priority = new ComboBox();
            Priority.Width = 200;
            Priority.Height = 24;
            Priority.Items.Add("Низкий");
            Priority.Items.Add("Средний");
            Priority.Items.Add("Высокий");
            Priority.Items.Add("КРИТИЧЕСКИЙ");
            Priority.SelectedIndex = priority;
            Priority.Margin = new Thickness(96, 52, 0, 0);
            CanvasMainInformation.Children.Add(Priority);

            TextBox Labels = new TextBox();
            Labels.Text = labels;
            Labels.Width = 200;
            Labels.Height = 24;
            Labels.Margin = new Thickness(96, 79, 0, 0);
            CanvasMainInformation.Children.Add(Labels);

            TextBox Components = new TextBox();
            Components.Text = components;
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

            //таблица
            Canvas TableHeader = new Canvas();
            TableHeader.Width = 900;
            TableHeader.Height = 40;
            TableHeader.Background = Brushes.LightGray;
            TableHeader.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - TableHeader.Width / 2, 430, 0, 0);
            Background.Children.Add(TableHeader);

            StackPanel Stack = new StackPanel();
            Stack.Width = TableHeader.Width;
            Stack.Height = 400;

            ScrollViewer ScrollList = new ScrollViewer();
            ScrollList.Width = 900;
            ScrollList.Height = 400;
            ScrollList.Background = Brushes.White;
            ScrollList.Margin = new Thickness(0, 40, 0, 0);
            ScrollList.Content = Stack;
            TableHeader.Children.Add(ScrollList);

            TextBlock Attr = new TextBlock();
            Attr.Text = "Шаг №";
            Attr.FontSize = 14;
            Attr.Margin = new Thickness(5, 6, 0, 0);
            TableHeader.Children.Add(Attr);

            TextBlock Attr2 = new TextBlock();
            Attr2.Text = "Шаг теста";
            Attr2.FontSize = 14;
            Attr2.Margin = new Thickness(100, 6, 0, 0);
            TableHeader.Children.Add(Attr2);

            TextBlock Attr3 = new TextBlock();
            Attr3.Text = "Данные теста";
            Attr3.FontSize = 14;
            Attr3.Margin = new Thickness(320, 6, 0, 0);
            TableHeader.Children.Add(Attr3);

            TextBlock Attr4 = new TextBlock();
            Attr4.Text = "Ожидаемый результат";
            Attr4.FontSize = 14;
            Attr4.Margin = new Thickness(610, 6, 0, 0);
            TableHeader.Children.Add(Attr4);

            int CountRows = 1;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open(); // Убедитесь, что соединение открыто перед выполнением команды

                string countQuery = "SELECT COUNT(*) FROM " + NameTable + "_testdata";

                // Создание команды с использованием SQL-запроса и соединения
                using (MySqlCommand command = new MySqlCommand(countQuery, connection))
                {
                    object result = command.ExecuteScalar(); // Теперь это должно работать, так как соединение открыто
                    CountRows = Convert.ToInt32(result);
                }
            }


            Canvas[] Item = new Canvas[CountRows];
            Canvas AddCanvas = new Canvas();
            AddCanvas.Width = 900;
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

            

            List<TestItem> tableItemsList = new List<TestItem>();

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string testDataQuery = $"SELECT StepTest, TestData, Result FROM {NameTable}_testdata";

                using (MySqlCommand command = new MySqlCommand(testDataQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Чтение всех записей из таблицы
                        {
                            // Создаем новый объект TableItem
                            TestItem tableItem = new TestItem();
                            tableItem.SteepTitle.Document.Blocks.Clear();
                            tableItem.SteepTitle.Document.Blocks.Add(new Paragraph(new Run(reader["StepTest"].ToString())));

                            tableItem.DataTest.Document.Blocks.Clear();
                            tableItem.DataTest.Document.Blocks.Add(new Paragraph(new Run(reader["TestData"].ToString())));

                            tableItem.Result.Document.Blocks.Clear();
                            tableItem.Result.Document.Blocks.Add(new Paragraph(new Run(reader["Result"].ToString())));

                            // Добавляем tableItem в список
                            tableItemsList.Add(tableItem);

                            Stack.Children.Add(tableItem.newItem);

                            Stack.Height += 100;
                        }
                    }
                }
            }
            Stack.Children.Add(AddCanvas);
            

            AddCanvas.MouseLeftButtonDown += (sender, e) =>
            {

                // Сохраняем ссылку на AddCanvas
                UIElement addCanvasReference = AddCanvas;

                // Создаем новый объект TableItem
                TestItem tableItem = new TestItem();

                // Добавляем tableItem в список
                tableItemsList.Add(tableItem);

                // Увеличиваем высоту StackPanel
                Stack.Height += 100;
                CountRows++;

                // Удаляем AddCanvas из StackPanel
                Stack.Children.Remove(AddCanvas);

                // Добавляем новый объект TableItem в StackPanel
                Stack.Children.Add(tableItem.newItem);

                // Добавляем AddCanvas обратно в StackPanel, чтобы он был последним элементом
                Stack.Children.Add(addCanvasReference);

                tableItem.RequestDelete += (senderDelete, eDelete) =>
                {
                    Stack.Height -= 100;
                    CountRows++;
                    Stack.Children.Remove(tableItem.newItem);
                    tableItemsList.Remove(tableItem);
                };

            };

            Button Preview = new Button();
            Preview.Width = 210;
            Preview.Height = Header.Height;
            Preview.Content = "  Предпросмотр";
            Preview.Background = Brushes.AliceBlue;
            Header.Children.Add(Preview);

            GenerateButton(CanvasMainInformation, -1, id, true, CountRows);

            TestCaseTableDB table = new TestCaseTableDB();

            Preview.MouseEnter += (sender, e) =>
            {
                Preview.Background = Brushes.White;
            };

            Preview.Click += (sender, e) =>
            {
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();

                    

                    // Предполагаем, что таблица AllTestCases уже существует и содержит запись с данным id.
                    string updateAllTestCasesQuery = $"UPDATE AllTestCases SET NameTK = '{NameTK}', Creater = '{Maker.SelectedItem.ToString()}', Project = '{Projects.SelectedItem.ToString()}' WHERE id = {id}";

                    using (MySqlCommand updateCommand = new MySqlCommand(updateAllTestCasesQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }

                    // Предполагаем, что таблица с именем NameTable уже существует.
                    string updateNameTableQuery = $"UPDATE {NameTable} SET name = '{NameTable}', priority = '{Priority.SelectedIndex}', labels = '{Labels.Text}', components = '{Components.Text}', maker = '{Maker.SelectedItem.ToString()}', Project = '{Projects.SelectedItem.ToString()}' WHERE id = {id}";

                    using (MySqlCommand updateNameTableCommand = new MySqlCommand(updateNameTableQuery, connection))
                    {
                        updateNameTableCommand.ExecuteNonQuery();
                    }

                    // Предполагаем, что таблица с именем NameTable_TestData уже существует.
                    string updateTestDataQuery = $"UPDATE {NameTable}_TestData SET StepTest = @StepTest, TestData = @TestData, Result = @Result, ResultIndex = @ResultIndex WHERE id = @id";

                    int i = 0;
                    foreach (TestItem tableItem in tableItemsList)
                    {
                        using (MySqlCommand updateTestDataCommand = new MySqlCommand(updateTestDataQuery, connection))
                        {
                            updateTestDataCommand.Parameters.AddWithValue("@id", i + 1);
                            updateTestDataCommand.Parameters.AddWithValue("@StepTest", new TextRange(tableItemsList[i].SteepTitle.Document.ContentStart, tableItemsList[i].SteepTitle.Document.ContentEnd).Text);
                            updateTestDataCommand.Parameters.AddWithValue("@TestData", new TextRange(tableItemsList[i].DataTest.Document.ContentStart, tableItemsList[i].DataTest.Document.ContentEnd).Text);
                            updateTestDataCommand.Parameters.AddWithValue("@Result", new TextRange(tableItemsList[i].Result.Document.ContentStart, tableItemsList[i].Result.Document.ContentEnd).Text);
                            updateTestDataCommand.Parameters.AddWithValue("@ResultIndex", 0);

                            updateTestDataCommand.ExecuteNonQuery();
                        }

                        i++;
                    }
                }

                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();

                    string countRowsQuery = $"SELECT COUNT(*) FROM {NameTable}_TestData;";
                    MySqlCommand countRowsCommand = new MySqlCommand(countRowsQuery, connection);
                    int currentCountRows = Convert.ToInt32(countRowsCommand.ExecuteScalar());

                    // Если в tableItemsList больше элементов, чем строк в таблице, добавляем новые строки
                    if (tableItemsList.Count > currentCountRows)
                    {
                        for (int i = currentCountRows; i < tableItemsList.Count; i++)
                        {
                            // Запрос на добавление новой строки
                            string insertQuery = $"INSERT INTO {NameTable}_TestData (id, StepTest, TestData, Result, ResultIndex) VALUES (@id, @StepTest, @TestData, @Result, @ResultIndex);";
                            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@id", (i+1).ToString());
                                insertCommand.Parameters.AddWithValue("@StepTest", new TextRange(tableItemsList[i].SteepTitle.Document.ContentStart, tableItemsList[i].SteepTitle.Document.ContentEnd).Text);
                                insertCommand.Parameters.AddWithValue("@TestData", new TextRange(tableItemsList[i].DataTest.Document.ContentStart, tableItemsList[i].DataTest.Document.ContentEnd).Text);
                                insertCommand.Parameters.AddWithValue("@Result", new TextRange(tableItemsList[i].Result.Document.ContentStart, tableItemsList[i].Result.Document.ContentEnd).Text);
                                insertCommand.Parameters.AddWithValue("@ResultIndex", 0);

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                ChangeStatus(0, id);
            };
            Preview.MouseLeave += (sender, e) =>
            {
                Preview.Background = Brushes.AliceBlue;
            };

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

            return Background;
        }




    }







        public class TestItem : UIElement
        {
            public int count { get; set; }
            public Canvas newItem { get; set; }

            public RichTextBox SteepTitle { get; set; }
            public RichTextBox DataTest { get; set; }
            public RichTextBox Result { get; set; }

            // Событие для удаления экземпляра TestItem
            public event EventHandler RequestDelete;

            public TestItem()
            {
                newItem = new Canvas();
                newItem.Width = 900;
                newItem.Height = 100;
                newItem.Background = Brushes.White;

                newItem.MouseEnter += (sender, e) =>
                {
                    newItem.Background = Brushes.LightGray;
                };
                newItem.MouseLeave += (sender, e) =>
                {
                    newItem.Background = Brushes.White;
                };

                Image icoDelete = new Image();
                icoDelete.Width = 30;
                icoDelete.Height = 30;
                icoDelete.Margin = new Thickness(newItem.Width - 60, 34, 0, 0);
                icoDelete.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/delete.png"));
                newItem.Children.Add(icoDelete);

                icoDelete.MouseDown += (sender, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        // Вызов события RequestDelete для удаления экземпляра
                        RequestDelete?.Invoke(this, EventArgs.Empty);
                    }
                };


                SteepTitle = new RichTextBox();
                SteepTitle.Width = 200;
                SteepTitle.Height = 80;
                SteepTitle.Margin = new Thickness(100, 6, 0, 0);
                newItem.Children.Add(SteepTitle);

                DataTest = new RichTextBox();
                DataTest.Width = 260;
                DataTest.Height = 80;
                DataTest.Margin = new Thickness(320, 6, 0, 0);
                newItem.Children.Add(DataTest);

                Result = new RichTextBox();
                Result.Width = 200;
                Result.Height = 80;
                Result.Margin = new Thickness(610, 6, 0, 0);
                newItem.Children.Add(Result);


            }




        }


    

}



