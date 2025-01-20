using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Philimon.pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Philimon.objects.UI
{
    class FormObj
    {
        public Canvas GenerateForm(Page ThisPage , string Title, string QueryViewTable, string[] viewAttr, string PassportType, string[] TableAttr, string[] AttrForSearch, string NameTableDB, string[] AllAttrFromDB, string[] AllAttrLocaliz, Page GoToPage, string[] attrTypes)
        {
            DatabaseData DB = new DatabaseData();

            DB.Connection();

            Canvas FormCanvas = new Canvas();
            FormCanvas.Height = Application.Current.MainWindow.Height;
            FormCanvas.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = Title;
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            FormCanvas.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            FormCanvas.Children.Add(line1);

            TextBox searchBox = new TextBox();
            searchBox.Width = 200;
            searchBox.Height = 40;
            searchBox.Tag = "Поиск:";
            searchBox.Visibility = Visibility.Hidden;
            searchBox.Margin = new Thickness(5, 90, 0, 0);
            FormCanvas.Children.Add(searchBox);

            Button ViewButton = new Button();
            ViewButton.Width = 200;
            ViewButton.Height = 40;
            ViewButton.Content = "Просмотр данных";
            ViewButton.Margin = new Thickness(500, 90, 0, 0);
            FormCanvas.Children.Add(ViewButton);

            Button AddButton = new Button();
            AddButton.Width = 200;
            AddButton.Height = 40;
            AddButton.Content = "Добавить данные";
            AddButton.Margin = new Thickness(500 + AddButton.Width + 30, 90, 0, 0);
            FormCanvas.Children.Add(AddButton);

            Canvas ViewCanvas = new Canvas();
            ViewCanvas.Margin = new Thickness(0, 140, 0, 0);
            ViewCanvas.Width = Application.Current.MainWindow.Width - 390;
            ViewCanvas.Height = Application.Current.MainWindow.Height - 100;
            ViewCanvas.Visibility = Visibility.Hidden;
            FormCanvas.Children.Add(ViewCanvas);

            StackPanel ViewCanvasStackPanel = new StackPanel();
            ViewCanvasStackPanel.Width = ViewCanvas.Width;
            ViewCanvasStackPanel.Height = ViewCanvas.Height;
            ViewCanvas.Children.Add(ViewCanvasStackPanel);


            Canvas AddCanvas = new Canvas();
            AddCanvas.Margin = new Thickness(0, 140, 0, 0);
            AddCanvas.Width = Application.Current.MainWindow.Width - 390;
            AddCanvas.Height = Application.Current.MainWindow.Height - 100;
            AddCanvas.Visibility = Visibility.Hidden;
            FormCanvas.Children.Add(AddCanvas);


            string[] arrtibytesFromDataBase = AllAttrFromDB;
            string[] NameAttr = AllAttrLocaliz;
            TextBox[] RedactTextBox = new TextBox[NameAttr.Length];



            int posButton = 0;

            for (int i = 0; i < NameAttr.Length; i++)
            {
                RedactTextBox[i] = new TextBox();
                RedactTextBox[i].Width = 300;
                RedactTextBox[i].Height = 38;
                RedactTextBox[i].Margin = new Thickness(10, 40 + i * 50, 0, 0);
                RedactTextBox[i].Tag = NameAttr[i];
                AddCanvas.Children.Add(RedactTextBox[i]);

                RedactTextBox[i].TextChanged += (sender, e) =>
                {
                    ((TextBox)sender).BorderBrush = Brushes.LightGray;
                };

                if (attrTypes[i] == "text")
                {

                }
                else if (attrTypes[i] == "inf")
                {
                    int currentIndex = i; // Создаем временную переменную для хранения значения i
                    RedactTextBox[i].PreviewMouseLeftButtonDown += (sender, e) => {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            RedactTextBox[currentIndex].Text = "";
                            Browser Browser = new Browser("inf", RedactTextBox[currentIndex]);
                            AddCanvas.Children.Add(Browser.Canvas);
                        }
                    };
                }
                else if (attrTypes[i] == "pos")
                {
                    int currentIndex = i; // Создаем временную переменную для хранения значения i
                    RedactTextBox[i].PreviewMouseLeftButtonDown += (sender, e) => {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            RedactTextBox[currentIndex].Text = "";
                            Browser Browser = new Browser("pos", RedactTextBox[currentIndex]);
                            AddCanvas.Children.Add(Browser.Canvas);
                        }
                    };
                }
                else if (attrTypes[i] == "teamlid")
                {
                    int currentIndex = i; // Создаем временную переменную для хранения значения i
                    RedactTextBox[i].PreviewMouseLeftButtonDown += (sender, e) => {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            RedactTextBox[currentIndex].Text = "";
                            Browser Browser = new Browser("teamlid", RedactTextBox[currentIndex]);
                            AddCanvas.Children.Add(Browser.Canvas);
                        }
                    };
                }
                else if (attrTypes[i] == "role")
                {
                    int currentIndex = i; // Создаем временную переменную для хранения значения i
                    RedactTextBox[i].PreviewMouseLeftButtonDown += (sender, e) => {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            RedactTextBox[currentIndex].Text = "";
                            Browser Browser = new Browser("role", RedactTextBox[currentIndex]);
                            AddCanvas.Children.Add(Browser.Canvas);
                        }
                    };
                }
                else if (attrTypes[i] == "dep")
                {
                    int currentIndex = i; // Создаем временную переменную для хранения значения i
                    RedactTextBox[i].PreviewMouseLeftButtonDown += (sender, e) => {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            RedactTextBox[currentIndex].Text = "";
                            Browser Browser = new Browser("dep", RedactTextBox[currentIndex]);
                            AddCanvas.Children.Add(Browser.Canvas);
                        }
                    };
                }


                
                posButton = 40 + (i + 1) * 50;
            }
            Button AddWritten = new Button();
            AddWritten.Width = 300;
            AddWritten.Height = 30;
            AddWritten.Content = "Добавить запись";
            AddWritten.Margin = new Thickness(10, posButton + 10, 0, 0);
            AddCanvas.Children.Add(AddWritten);


            
                AddWritten.Click += (sender, e) =>
                {
                    if (PassportType != "Humans") {
                        using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                        {
                            connection.Open();

                            // Получаем текущее количество строк в таблице
                            string countQuery = $"SELECT COUNT(*) FROM {NameTableDB}";
                            MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                            int rowCount = Convert.ToInt32(countCommand.ExecuteScalar()) + 1;

                            //string Query = $"INSERT INTO {NameTableDB} (id, {arrtibytesFromDataBase[0]}, {arrtibytesFromDataBase[1]}, {arrtibytesFromDataBase[2]}, {arrtibytesFromDataBase[3]}, {arrtibytesFromDataBase[4]}) VALUES (@id, @NameDepartament, @icoPatch, @Leader, @budget, @substruction)"; //

                            string Query = "";

                            for (int i = 0; i < arrtibytesFromDataBase.Length; i++)
                            {

                                Query = $"INSERT INTO {NameTableDB} (id, {string.Join(", ", arrtibytesFromDataBase)}) VALUES (@id, {string.Join(", ", arrtibytesFromDataBase.Select(attr => "@" + attr))})";


                            }


                            bool write = true;

                            for (int i = 0; i < NameAttr.Length; i++)
                            {
                                if (NameAttr[i].Contains("*") && string.IsNullOrEmpty(RedactTextBox[i].Text))
                                {
                                    RedactTextBox[i].BorderBrush = Brushes.Red;
                                    write = false;
                                }


                            }
                            //автоподбор ключей сделать
                            if (write == true)
                            {
                                using (MySqlCommand command = new MySqlCommand(Query, connection))
                                {
                                    for (int i = 0; i < RedactTextBox.Length; i++)
                                    {
                                        RedactTextBox[i].BorderBrush = Brushes.LimeGreen;
                                    }

                                    command.Parameters.AddWithValue("@id", rowCount);
                                    for (int i = 0; i < arrtibytesFromDataBase.Length; i++)
                                    {
                                        if (!string.IsNullOrEmpty(RedactTextBox[i].Text))
                                        {
                                            command.Parameters.AddWithValue("@" + arrtibytesFromDataBase[i], RedactTextBox[i].Text);
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@" + arrtibytesFromDataBase[i], "-");
                                        }
                                    }

                                    command.ExecuteNonQuery();
                                }
                            }

                        }
                    }
                    else
                    {
                        Canvas Form = new Canvas();
                        Form.Width = 500;
                        Form.Height = 300;
                        Form.Background = Brushes.White;
                        Form.Margin = new Thickness(400,200,0,0);
                        AddCanvas.Children.Add(Form);

                        Button AddUser = new Button();
                        AddUser.Width = 150;
                        AddUser.Height = 20;
                        AddUser.Content = "Добавить";
                        AddUser.Margin = new Thickness(100,270,0,0);
                        Form.Children.Add(AddUser);

                        Button Close = new Button();
                        Close.Width = 150;
                        Close.Height = 20;
                        Close.Content = "Закрыть";
                        Close.Margin = new Thickness(260, 270, 0, 0);
                        Form.Children.Add(Close);

                        Close.Click += (sender2, e2) =>
                        {
                            AddCanvas.Children.Remove(Form);
                        };

                        TextBox Login = new TextBox();
                        Login.Width = 270;
                        Login.Height = 30;
                        Login.Tag = "Логин";
                        Login.Margin = new Thickness(100,100,0,0);
                        Form.Children.Add(Login);

                        TextBox Password = new TextBox();
                        Password.Width = 270;
                        Password.Height = 30;
                        Password.Tag = "Пароль";
                        Password.Margin = new Thickness(100, 140, 0, 0);
                        Form.Children.Add(Password);

                        Button RandomLogin = new Button();
                        RandomLogin.Width = 30;
                        RandomLogin.Height = 30;
                        RandomLogin.Content = "џ";
                        RandomLogin.Margin = new Thickness(380,100,0,0);
                        Form.Children.Add(RandomLogin);

                        Button RandomPassword = new Button();
                        RandomPassword.Width = 30;
                        RandomPassword.Height = 30;
                        RandomPassword.Content = "џ";
                        RandomPassword.Margin = new Thickness(380, 140, 0, 0);
                        Form.Children.Add(RandomPassword);

                        Random random = new Random();

                        RandomLogin.Click += (sender3, e3) =>
                        {
                            string randomLogin = GenerateRandomString("", 4) + random.Next(1000, 9999).ToString();
                            Login.Text = randomLogin;
                        };

                        RandomPassword.Click += (sender4, e4) =>
                        {
                            string randomPassword = GenerateRandomString("", 4) + random.Next(1000, 9999).ToString();
                            Password.Text = randomPassword;
                        };

                        AddUser.Click += (sender1, e1) =>
                        {
                            AddUser.IsEnabled = false;
                            Close.IsEnabled = false;

                            string login = Login.Text;
                            string password = Password.Text;

                            Login.IsEnabled = false;
                            Password.IsEnabled = false;
                            
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "Rtf file (*.rtf)|*.rtf";
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                
                                File.WriteAllText(saveFileDialog.FileName, $"Внимание!\nДанные, для авторизации в системе Philimon\nвыданные вам, следует запомнить и уничтожить через 2 часа после\nпередачи их Вам на руки!\n\nЛогин: {login}\nПароль: {password}");


                                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                                {
                                    connection.Open();

                                    // Получаем текущее количество строк в таблице Humans
                                    string countQuery = "SELECT COUNT(*) FROM Humans";
                                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                                    int rowCount = Convert.ToInt32(countCommand.ExecuteScalar()) + 1;
                                    

                                    string insertHumanQuery = "";

                                    for (int i = 0; i < arrtibytesFromDataBase.Length; i++)
                                    {

                                        insertHumanQuery = $"INSERT INTO {NameTableDB} (id, {string.Join(", ", arrtibytesFromDataBase)}) VALUES (@id, {string.Join(", ", arrtibytesFromDataBase.Select(attr => "@" + attr))})";


                                    }

                                    bool write = true;

                                    for (int i = 0; i < NameAttr.Length; i++)
                                    {
                                        if (NameAttr[i].Contains("*") && string.IsNullOrEmpty(RedactTextBox[i].Text))
                                        {
                                            RedactTextBox[i].BorderBrush = Brushes.Red;
                                            write = false;
                                        }
                                    }

                                    if (write)
                                    {
                                        using (MySqlCommand command = new MySqlCommand(insertHumanQuery, connection))
                                        {
                                            for (int i = 0; i < RedactTextBox.Length; i++)
                                            {
                                                RedactTextBox[i].BorderBrush = Brushes.LimeGreen;
                                            }

                                            command.Parameters.AddWithValue("@id", rowCount);
                                            for (int i = 0; i < arrtibytesFromDataBase.Length; i++)
                                            {
                                                if (!string.IsNullOrEmpty(RedactTextBox[i].Text))
                                                {
                                                    command.Parameters.AddWithValue("@" + arrtibytesFromDataBase[i], RedactTextBox[i].Text);
                                                }
                                                else
                                                {
                                                    command.Parameters.AddWithValue("@" + arrtibytesFromDataBase[i], "-");
                                                }
                                            }

                                            command.ExecuteNonQuery();

                                        }
                                    }

                                    // Добавление данных в таблицу Users
                                    string insertUserQuery = @"INSERT INTO Users (id, human_id, tagUser, login, password) VALUES (@id, @human_id, @tagUser, @login, @password)";

                                    // Предполагается, что у вас есть переменная humanId, содержащая id человека из таблицы Humans
                                    int userRowCount = rowCount; // Используем тот же id для Users

                                    using (MySqlCommand userCommand = new MySqlCommand(insertUserQuery, connection))
                                    {
                                        userCommand.Parameters.AddWithValue("@id", userRowCount);
                                        userCommand.Parameters.AddWithValue("@human_id", userRowCount);
                                        userCommand.Parameters.AddWithValue("@tagUser", "@"+Login.Text);
                                        userCommand.Parameters.AddWithValue("@login", Login.Text); 
                                        userCommand.Parameters.AddWithValue("@password", Password.Text);

                                        userCommand.ExecuteNonQuery();
                                    }
                                }


                                AddCanvas.Children.Remove(Form);


                            }



                        };
                    }


                };
            
            


            ViewButton.Click += (sender, e) =>
            {
                FormCanvas.Height = Application.Current.MainWindow.Height;
                ViewCanvasStackPanel.Height = ViewCanvas.Height;

                ViewCanvasStackPanel.Children.Clear();

                ViewCanvas.Visibility = Visibility.Visible;
                AddCanvas.Visibility = Visibility.Hidden;
                searchBox.Visibility = Visibility.Visible;

                Canvas HeaderCanvas = new Canvas();
                HeaderCanvas.Width = 1200;
                HeaderCanvas.Height = 64;
                HeaderCanvas.Background = Brushes.LightGray;

                TextBlock Attr1 = new TextBlock();
                Attr1.Text = TableAttr[0];
                Attr1.FontSize = 20;
                Attr1.Margin = new Thickness(200, 14, 0, 0);

                TextBlock Attr2 = new TextBlock();
                Attr2.Text = TableAttr[1];
                Attr2.FontSize = 20;
                Attr2.Margin = new Thickness(900, 14, 0, 0);

                ViewCanvasStackPanel.Children.Add(HeaderCanvas);
                HeaderCanvas.Children.Add(Attr1);
                HeaderCanvas.Children.Add(Attr2);


                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();

                    string Query = QueryViewTable;

                    int i = 0;

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {



                        using (MySqlDataReader reader = command.ExecuteReader())
                        {

                            //дописать функционал так, чтоб можно было отображать разное кол-во атрибутов
                            while (reader.Read())
                            {
                                Canvas Items = new Canvas();
                                Items item = new Items();

                                Items = item.Construct(1200, 64, reader[viewAttr[0]].ToString(), reader[viewAttr[1]].ToString(), "");
                                Items.Tag = Convert.ToInt32(reader["id"]);

                                ViewCanvasStackPanel.Children.Add(Items);

                                Items.MouseDown += (sender1, e1) =>
                                {
                                    if (e1.LeftButton == MouseButtonState.Pressed)
                                    {
                                        ThisPage.NavigationService.Navigate(new PassportPage(PassportType, Convert.ToInt32(Items.Tag), NameAttr, arrtibytesFromDataBase));
                                    }
                                };

                                i++;

                                if (i > 12)
                                {
                                    ViewCanvasStackPanel.Height += 86;
                                    FormCanvas.Height = ViewCanvasStackPanel.Height;
                                }


                            }
                        }

                    }

                    if (i == 0)
                    {
                        Canvas ItemCanvas = new Canvas();
                        ItemCanvas.Width = 1200;
                        ItemCanvas.Height = 64;
                        ItemCanvas.Background = Brushes.AliceBlue;
                        ViewCanvasStackPanel.Children.Add(ItemCanvas);

                        TextBlock attr1Text = new TextBlock();
                        attr1Text.Text = "-Данные отсутствуют-";
                        attr1Text.FontSize = 19;
                        attr1Text.Margin = new Thickness(ItemCanvas.Width / 2 - attr1Text.Text.Length - attr1Text.FontSize, 14, 0, 0);
                        attr1Text.Foreground = Brushes.Black;
                        ItemCanvas.Children.Add(attr1Text);


                    }


                }

            };


            searchBox.TextChanged += (sender, e) =>
            {
                FormCanvas.Height = Application.Current.MainWindow.Height;
                ViewCanvasStackPanel.Height = ViewCanvas.Height;

                ViewCanvasStackPanel.Children.Clear();

                ViewCanvas.Visibility = Visibility.Visible;
                AddCanvas.Visibility = Visibility.Hidden;

                Canvas HeaderCanvas = new Canvas();
                HeaderCanvas.Width = 1200;
                HeaderCanvas.Height = 64;
                HeaderCanvas.Background = Brushes.LightGray;


                //сделать так чтоб было произвольное кол-во атрибутов
                TextBlock Attr1 = new TextBlock();
                Attr1.Text = TableAttr[0];
                Attr1.FontSize = 20;
                Attr1.Margin = new Thickness(200, 14, 0, 0);

                TextBlock Attr2 = new TextBlock();
                Attr2.Text = TableAttr[1];
                Attr2.FontSize = 20;
                Attr2.Margin = new Thickness(900, 14, 0, 0);

                ViewCanvasStackPanel.Children.Add(HeaderCanvas);
                HeaderCanvas.Children.Add(Attr1);
                HeaderCanvas.Children.Add(Attr2);

                int SearchItems = 0;

                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();

                    // Формируем запрос, который будет искать совпадения по введенным символам
                    string searchQuery = $"SELECT id, {AttrForSearch[0]}, {AttrForSearch[1]} FROM {NameTableDB} WHERE {AttrForSearch[0]} LIKE @searchText OR {AttrForSearch[1]} LIKE @searchText";

                    using (MySqlCommand command = new MySqlCommand(searchQuery, connection))
                    {
                        // Добавляем параметр для поиска
                        command.Parameters.AddWithValue("@searchText", "%" + searchBox.Text + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Canvas Items = new Canvas();
                                Items item = new Items();

                                Items = item.Construct(1200, 64, reader[viewAttr[0]].ToString(), reader[viewAttr[1]].ToString(), "");
                                Items.Tag = Convert.ToInt32(reader["id"]);

                                ViewCanvasStackPanel.Children.Add(Items);

                                SearchItems++;

                                if (SearchItems > 12)
                                {
                                    ViewCanvasStackPanel.Height += 86;
                                    FormCanvas.Height = ViewCanvasStackPanel.Height;
                                }

                                Items.MouseDown += (sender1, e1) =>
                                {
                                    if (e1.LeftButton == MouseButtonState.Pressed)
                                    {
                                        ThisPage.NavigationService.Navigate(new PassportPage(PassportType, Convert.ToInt32(Items.Tag), NameAttr, arrtibytesFromDataBase));
                                    }
                                };
                            }
                        }
                    }
                }

                if (SearchItems == 0)
                {
                    Canvas ItemCanvas = new Canvas();
                    ItemCanvas.Width = 1200;
                    ItemCanvas.Height = 64;
                    ItemCanvas.Background = Brushes.AliceBlue;
                    ViewCanvasStackPanel.Children.Add(ItemCanvas);

                    TextBlock attr1Text = new TextBlock();
                    attr1Text.Text = "-Данные не обнаружены-";
                    attr1Text.FontSize = 19;
                    attr1Text.Margin = new Thickness(ItemCanvas.Width / 2 - attr1Text.Text.Length - attr1Text.FontSize, 14, 0, 0);
                    attr1Text.Foreground = Brushes.Black;
                    ItemCanvas.Children.Add(attr1Text);


                }


            };

            AddButton.Click += (sender, e) =>
            {
                if (GoToPage == null) {
                    searchBox.Visibility = Visibility.Hidden;
                    ViewCanvas.Visibility = Visibility.Hidden;
                    AddCanvas.Visibility = Visibility.Visible;

                    FormCanvas.Height = Application.Current.MainWindow.Height;
                    ViewCanvasStackPanel.Height = ViewCanvas.Height;

                    ViewCanvasStackPanel.Children.Clear();

                }
                else
                {

                    ThisPage.NavigationService.Navigate(GoToPage);

                }
            };

            return FormCanvas;
        }
        Random random = new Random();
        private string GenerateRandomString(string prefix, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            builder.Append(prefix);
            for (int i = 0; i < length; i++)
            {
                builder.Append(chars[random.Next(chars.Length)]);
            }
            return builder.ToString();
        }

        class Items : Button
        {

            public Canvas Construct(double width, double height, string attr1, string attr2, string IcoPath)
            {
                Canvas ItemCanvas = new Canvas();
                ItemCanvas.Width = width;
                ItemCanvas.Height = height;
                ItemCanvas.Background = Brushes.AliceBlue;

                TextBlock attr1Text = new TextBlock();
                attr1Text.Text = attr1;
                attr1Text.FontSize = 19;
                attr1Text.Margin = new Thickness(200, 14, 0, 0);
                attr1Text.Foreground = Brushes.Black;
                ItemCanvas.Children.Add(attr1Text);

                TextBlock attr2Text = new TextBlock();
                attr2Text.Text = attr2;
                attr2Text.FontSize = 19;
                attr2Text.Foreground = Brushes.Black;
                attr2Text.Margin = new Thickness(900, 14, 0, 0);
                ItemCanvas.Children.Add(attr2Text);

                Image image = new Image();
                image.Width = 64;
                image.Height = 64;
#pragma warning disable CS0168 // Переменная объявлена, но не используется
                try
                {
                    image.Source = new BitmapImage(new Uri(IcoPath));
                }
                catch (Exception ex)
                {
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/departament.png"));
                }
#pragma warning restore CS0168 // Переменная объявлена, но не используется
                ItemCanvas.Children.Add(image);

                ItemCanvas.MouseEnter += (sender, e) =>
                {
                    ItemCanvas.Background = Brushes.White;
                };
                ItemCanvas.MouseLeave += (sender, e) =>
                {
                    ItemCanvas.Background = Brushes.AliceBlue;
                };

                return ItemCanvas;
            }


        }

        class Browser
        {
            DatabaseData DB = new DatabaseData();

            public Canvas Canvas = new Canvas();

            Canvas Window = new Canvas();

            StackPanel SP = new StackPanel();


            string Type_ = "";
            public Browser(string Type, TextBox TB)
            {
                Canvas.Width = 400;
                Canvas.Height = 400;
                Canvas.Background = Brushes.Black;
                Canvas.Margin = new Thickness(460, 150, 0, 0);


                Window.Width = 398;
                Window.Height = 398;
                Window.Background = Brushes.White;
                Window.Margin = new Thickness(1, 1, 0, 0);
                Canvas.Children.Add(Window);


                SP.Width = Window.Width;
                SP.Height = Window.Height;
                SP.Margin = new Thickness(1, 1, 0, 0);
                Canvas.Children.Add(SP);

                Type_ = Type;
                if (Type_ == "inf")
                {
                    GenerateUiInf(TB);
                }
                if (Type_ == "pos")
                {
                    GenerateUiPos(TB);
                }
                if (Type_ == "teamlid")
                {
                    GenerateUiTeamLead(TB);
                }
                if (Type_ == "role")
                {
                    GenerateUiRole(TB);
                }
                if (Type_ == "dep")
                {
                    GenerateUiDepartaments(TB);
                }
            }


            void GenerateUiInf(TextBox TB)
            {
                int rowCount = 0;

                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) AS row_count FROM departaments";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    rowCount = Convert.ToInt32(countCommand.ExecuteScalar());


                }


                string[] arrText = new string[rowCount];

                int j = 0;
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string ReadQuery = "SELECT * FROM departaments";

                    using (MySqlCommand command = new MySqlCommand(ReadQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                arrText[j] = reader["NameDepartament"].ToString();
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

                for (int i = 0; i < items.Length; i++)
                {
                    Canvas item = items[i];
                    SP.Children.Add(item);

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

                    item.MouseLeftButtonDown += (sender, e) =>
                    {
                        item.Background = Brushes.White;
                        // При нажатии на элемент Canvas, текст из Tag переносится в TextBox textBoxMain
                        TB.Text = item.Tag.ToString();

                        Close();
                    };

                }
            }

            void GenerateUiPos(TextBox TB)
            {
                int rowCount = 0;

                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) AS row_count FROM positionPeople";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    rowCount = Convert.ToInt32(countCommand.ExecuteScalar());


                }


                string[] arrText = new string[rowCount];

                int j = 0;
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string ReadQuery = "SELECT * FROM positionPeople";

                    using (MySqlCommand command = new MySqlCommand(ReadQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                arrText[j] = reader["NamePosition"].ToString();
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

                for (int i = 0; i < items.Length; i++)
                {
                    Canvas item = items[i];
                    SP.Children.Add(item);

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

                    item.MouseLeftButtonDown += (sender, e) =>
                    {
                        item.Background = Brushes.White;
                        // При нажатии на элемент Canvas, текст из Tag переносится в TextBox textBoxMain
                        TB.Text = item.Tag.ToString();

                        Close();
                    };

                }
            }

            void GenerateUiTeamLead(TextBox TB)
            {
                int rowCount = 0;

                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) AS row_count FROM Humans";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    rowCount = Convert.ToInt32(countCommand.ExecuteScalar());


                }


                string[] arrText = new string[rowCount];

                int j = 0;
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string ReadQuery = "SELECT * FROM Humans";

                    using (MySqlCommand command = new MySqlCommand(ReadQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                arrText[j] = reader["Firtsname"].ToString() + " " + reader["SecondName"].ToString() + " " + reader["ThirdName"].ToString();
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

                for (int i = 0; i < items.Length; i++)
                {
                    Canvas item = items[i];
                    SP.Children.Add(item);

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

                    item.MouseLeftButtonDown += (sender, e) =>
                    {
                        item.Background = Brushes.White;
                        // При нажатии на элемент Canvas, текст из Tag переносится в TextBox textBoxMain
                        TB.Text = item.Tag.ToString();

                        Close();
                    };

                }
            }

            void GenerateUiRole(TextBox TB)
            {
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

                for (int i = 0; i < items.Length; i++)
                {
                    Canvas item = items[i];
                    SP.Children.Add(item);

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

                    item.MouseLeftButtonDown += (sender, e) =>
                    {
                        item.Background = Brushes.White;
                        // При нажатии на элемент Canvas, текст из Tag переносится в TextBox textBoxMain
                        TB.Text = item.Tag.ToString();

                        Close();
                    };

                }
            }

            void GenerateUiDepartaments(TextBox TB)
            {
                int rowCount = 0;

                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) AS row_count FROM departaments";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    rowCount = Convert.ToInt32(countCommand.ExecuteScalar());


                }


                string[] arrText = new string[rowCount];

                int j = 0;
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string ReadQuery = "SELECT * FROM departaments";

                    using (MySqlCommand command = new MySqlCommand(ReadQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                arrText[j] = reader["NameDepartament"].ToString();
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

                for (int i = 0; i < items.Length; i++)
                {
                    Canvas item = items[i];
                    SP.Children.Add(item);

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

                    item.MouseLeftButtonDown += (sender, e) =>
                    {
                        item.Background = Brushes.White;
                        // При нажатии на элемент Canvas, текст из Tag переносится в TextBox textBoxMain
                        TB.Text = item.Tag.ToString();

                        Close();
                    };

                }
            }

            private void Close()
            {
                // Удаление всех элементов из canvas
                Canvas.Children.Clear();

                // Очистка всех ссылок
                Canvas.Opacity = 0; //сделать нормальное удаление

                Canvas = null;
                //closeButton = null;
            }

        }

    }
}
