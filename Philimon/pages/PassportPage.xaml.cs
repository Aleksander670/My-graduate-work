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
    /// Логика взаимодействия для PassportPage.xaml
    /// </summary>
    public partial class PassportPage : Page
    {

        DatabaseData DB = new DatabaseData();

        int id_;
        string NameTable_;
        string[] attributes_;
        string[] DB_attr_;

        public PassportPage(string NameTable, int id, string[] attributes, string[] DB_attr)
        {
            InitializeComponent();

            id_ = id;
            NameTable_ = NameTable;
            attributes_ = attributes;
            DB_attr_ = DB_attr;

            string newValue1 = id.ToString(); // Новое значение для добавления

            string[] newDBAttr = new string[DB_attr.Length + 1];
            newDBAttr[0] = newValue1;

            for (int i = 0; i < DB_attr.Length; i++)
            {
                newDBAttr[i + 1] = DB_attr[i];
            }


            string newValue = id.ToString(); // Новое значение для добавления

            string[] newAttr = new string[attributes.Length + 1];
            newAttr[0] = newValue;

            for (int i = 0; i < attributes.Length; i++)
            {
                newAttr[i + 1] = attributes[i];
            }


            GenerateUI(NameTable, newAttr, id, newDBAttr);
        }

        void GenerateUI(string NameTable, string[] attributes, int id, string[] DB_attr)
        {
            Canvas pageCanvas = new Canvas();

            pageCanvas.Background = Brushes.Gray;
            pageCanvas.Width = Application.Current.MainWindow.Width;
            pageCanvas.Height = Application.Current.MainWindow.Height;
            Grid.Children.Add(pageCanvas);

            TextBlock TitlePassport = new TextBlock();
            TitlePassport.Text = "Паспорт записи:";
            TitlePassport.FontSize = 26;
            TitlePassport.Foreground = Brushes.White;
            TitlePassport.Margin = new Thickness(25, 25, 0, 0);
            pageCanvas.Children.Add(TitlePassport);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            pageCanvas.Children.Add(line1);

            StackPanel stackPanel = new StackPanel();
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Left;

            ScrollViewer ScrollList = new ScrollViewer();
            ScrollList.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollList.Width = 465;
            ScrollList.Height = 520;
            ScrollList.Margin = new Thickness(100, 140, 0, 0);
            ScrollList.Content = stackPanel;
            pageCanvas.Children.Add(ScrollList);

            

            int CollAttr = 0;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string Query = "SELECT * FROM " + NameTable;

                using (MySqlCommand command = new MySqlCommand(Query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Получаем количество атрибутов
                            CollAttr = reader.FieldCount-1;

                                /*
                                Canvas ItemCanvas = new Canvas();
                                Items item = new Items();
                                ItemCanvas.Margin = new Thickness(0, 5, 0, 5);

                                string TextToItem = "";

                                // Формируем команду чтения атрибута
                                

                                ItemCanvas = item.Construct(400, 40, TextToItem);

                                stackPanel.Children.Add(ItemCanvas);
                                */

                        }
                    }
                }


            }


            string commandRead = "SELECT * FROM " + NameTable + " WHERE id = " + id;

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(commandRead, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Перемещаем указатель на следующую запись
                        {
                            for (int i = 0; i < CollAttr; i++)
                            {
                                string TextToItem = attributes[i] + ":" + reader[i].ToString();
                                string nameAttr = attributes[i];
                                string values = reader[i].ToString();
                                string DB_attr_ = DB_attr[i];

                                Canvas ItemCanvas = new Canvas();
                                Items item = new Items();
                                ItemCanvas.Margin = new Thickness(0, 5, 0, 5);
                                ItemCanvas = item.Construct(400, 40, TextToItem);
                                stackPanel.Children.Add(ItemCanvas);

                                if (i == 0)
                                {
                                    ItemCanvas.Visibility = Visibility.Hidden;
                                }

                                int index = i; // сохраняем значение i для использования внутри замыкания
                                ItemCanvas.MouseDown += (sender, e) =>
                                {
                                    if (e.LeftButton == MouseButtonState.Pressed)
                                    {
                                        RedactData(id, nameAttr.ToString(), values.ToString(), NameTable, DB_attr_, TextToItem, item);
                                    }
                                };

                            }
                        }
                    }
                }
            }


            Button exit = new Button();
            exit.Width = 400;
            exit.Height = 50;
            exit.Content = "Назад";
            exit.Margin = new Thickness(100,630,0,0);
            pageCanvas.Children.Add(exit);

            exit.Click += (sender, e) =>
            {
                this.NavigationService.Navigate(new Form_infrastructure());
            };


            Button Delete = new Button();
            Delete.Width = 400;
            Delete.Height = 50;
            Delete.Content = "Удалить";
            Delete.Background = Brushes.Red;
            Delete.Margin = new Thickness(100, 630 + exit.Height + 10, 0, 0);
            pageCanvas.Children.Add(Delete);

            Delete.Click += (sender, e) =>
            {
                if (NameTable_ != "Humans") {
                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString)) {
                        connection.Open();

                        string Query = "DELETE FROM " + NameTable + " WHERE id =" + id;

                        using (MySqlCommand command = new MySqlCommand(Query, connection))
                        {
                            command.ExecuteNonQuery();
                            this.NavigationService.Navigate(new Form_infrastructure());
                        }

                    }
                }
                else
                {

                    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                    {
                        connection.Open();
                        // Удаление из таблицы Humans
                        string queryHumans = "DELETE FROM Humans WHERE id = @id";
                        using (MySqlCommand command = new MySqlCommand(queryHumans, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        // Удаление из таблицы Users
                        string queryUsers = "DELETE FROM Users WHERE id = @id";
                        using (MySqlCommand command = new MySqlCommand(queryUsers, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        this.NavigationService.Navigate(new Form_infrastructure());
                    }

                }
            };

        }

        void RedactData(int id, string attr, string value, string table, string DB_attr, string text, Items item)
        {
            Canvas ParentCanvas = new Canvas();
            ParentCanvas.Background = Brushes.Black;
            ParentCanvas.Opacity = 0.7;
            ParentCanvas.Width = Application.Current.MainWindow.Width;
            ParentCanvas.Height = Application.Current.MainWindow.Height;
            Grid.Children.Add(ParentCanvas);

            Canvas RedactCanvas = new Canvas();
            RedactCanvas.Width = 400;
            RedactCanvas.Height = 200;
            RedactCanvas.Opacity = 1;
            RedactCanvas.Background = Brushes.White;
            RedactCanvas.Margin = new Thickness(Application.Current.MainWindow.Width/2- RedactCanvas.Width/2, Application.Current.MainWindow.Height/2 - RedactCanvas.Height/2, 0, 0);
            ParentCanvas.Children.Add(RedactCanvas);

            TextBlock TitleRedact = new TextBlock();
            TitleRedact.Text = "Редактировать значение:";
            TitleRedact.FontSize = 19;
            TitleRedact.Opacity = 1;
            RedactCanvas.Children.Add(TitleRedact);

            TextBox RedactBox = new TextBox();
            RedactBox.Width = 340;
            RedactBox.Height = 35;
            RedactBox.Margin = new Thickness(RedactCanvas.Width/2 - RedactBox.Width/2, RedactCanvas.Height/2 - RedactBox.Height, 0, 0);
            RedactBox.Tag = attr;
            RedactBox.Text = value;
            RedactCanvas.Children.Add(RedactBox);


            Button Save = new Button();
            Save.Width = RedactBox.Width / 2;
            Save.Height = 20;
            Save.Content = "Сохранить";
            Save.Margin = new Thickness(RedactBox.Width/2 - Save.Width, RedactCanvas.Height - Save.Height - 8, 0, 0);
            RedactCanvas.Children.Add(Save);

            Button Back = new Button();
            Back.Width = RedactBox.Width / 2;
            Back.Height = 20;
            Back.Content = "Закрыть";
            Back.Margin = new Thickness(((RedactBox.Width / 2 + Save.Width) - Back.Width/2) - 25, RedactCanvas.Height - Back.Height - 8, 0, 0);
            RedactCanvas.Children.Add(Back);


            Save.Click += (sender, e) =>
            {
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE `" + table + "` SET `" + DB_attr + "` = @NewValue WHERE id = @ID";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewValue", RedactBox.Text);
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                }

                text = attr + ":" + RedactBox.Text;

                

                // Очистка родительского Canvas и удаление его из Grid
                ParentCanvas.Children.Clear();
                Grid.Children.Remove(ParentCanvas);

                RefreshPage(NameTable_, id_, attributes_, DB_attr_);
            };

            Back.Click += (sender, e) =>
            {
                ParentCanvas.Children.Clear();
                Grid.Children.Remove(ParentCanvas);

                RefreshPage(NameTable_, id_, attributes_, DB_attr_);
            };



        }


        public void RefreshPage(string NameTable, int id, string[] attributes, string[] DB_attr)
        {
            this.NavigationService.Navigate(new PassportPage(NameTable, id, attributes, DB_attr));
        }

        class Items : Button
        {
#pragma warning disable CS0067 // Событие "PassportPage.Items.SingleClickEvent" никогда не используется.
            public event EventHandler SingleClickEvent;
#pragma warning restore CS0067 // Событие "PassportPage.Items.SingleClickEvent" никогда не используется.
            public Canvas Construct(double width, double height, string text)
            {
                Canvas ItemCanvas = new Canvas();
                ItemCanvas.Width = width;
                ItemCanvas.Height = height;
                ItemCanvas.Background = Brushes.AliceBlue;

                TextBlock Attribute = new TextBlock();
                Attribute.Text = text;
                Attribute.FontSize = 16;
                ItemCanvas.Children.Add(Attribute);

                Canvas RedactCanvas = new Canvas();
                RedactCanvas.Width = 40;
                RedactCanvas.Height = 40;
                RedactCanvas.Background = Brushes.AliceBlue;
                RedactCanvas.Margin = new Thickness(ItemCanvas.Width - RedactCanvas.Width,0 ,0, 0);
                ItemCanvas.Children.Add(RedactCanvas);

                Image redactIco = new Image();
                redactIco.Width = RedactCanvas.Width;
                redactIco.Height = RedactCanvas.Height;
                redactIco.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/redact.png"));
                RedactCanvas.Children.Add(redactIco);


                RedactCanvas.MouseEnter += (sender, e) =>
                {
                    ((Canvas)sender).Background = Brushes.LimeGreen;
                };
                RedactCanvas.MouseLeave += (sender, e) =>
                {
                    ((Canvas)sender).Background = Brushes.AliceBlue;
                };

                return ItemCanvas;
            }


        }

    }
}
