using MySql.Data.MySqlClient;
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

namespace Philimon.objects.UI
{
    class MissionForm
    {
        public Canvas canvas;
        private Button closeButton;

        Point startPoint;
        private bool isDragging;

        DatabaseData DB = new DatabaseData();

        public MissionForm()
        {
            Grid Header = new Grid();

            int width = 800;
            int height = 400;

            Header.Width = width;
            Header.Height = 40;
            Header.Margin = new Thickness(0, -39,0,0);
            Header.Background = Brushes.AliceBlue;

            canvas = new Canvas();

            canvas.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - width / 2, Application.Current.MainWindow.Height / 2 - height / 2, 0, 0);

            closeButton = new Button();
            closeButton.Width = 120;
            closeButton.Margin = new Thickness(-680, 0, 0, 0);
            closeButton.Content = "X";
            closeButton.Click += (sender, e) => { Close(); };

            

            StackPanel Stack = new StackPanel();
            Stack.Background = Brushes.Gray;
            Stack.Width = width;
            Stack.Height = height;

            ScrollViewer ScrollList = new ScrollViewer();
            ScrollList.Width = Stack.Width;
            ScrollList.Height = Stack.Height; //изменить под форму
            ScrollList.Content = Stack;


            canvas.Children.Add(ScrollList);

            Grid.SetColumn(Header, 0);
            Grid.SetRow(Header, 0);
            Grid.SetColumnSpan(Header, 2);

            Header.Children.Add(closeButton);

            canvas.Children.Add(Header);

            // Добавляем обработчики событий для перемещения формы
            Header.MouseLeftButtonDown += (sender, e) =>
            {
                startPoint = e.GetPosition(canvas);
                isDragging = true;
                Header.CaptureMouse();
            };

            Header.MouseMove += (sender, e) =>
            {
                if (isDragging)
                {
                    Point currentPoint = e.GetPosition(canvas);
                    double deltaX = currentPoint.X - startPoint.X;
                    double deltaY = currentPoint.Y - startPoint.Y;
                    canvas.Margin = new Thickness(canvas.Margin.Left + deltaX, canvas.Margin.Top + deltaY, 0, 0);
                    startPoint = currentPoint;
                }
            };

            Header.MouseLeftButtonUp += (sender, e) =>
            {
                isDragging = false;
                Header.ReleaseMouseCapture();
            };


            GenerateForm(Stack);

        }


        private void GenerateForm(StackPanel stackPanel)
        {
            TextBlock Title1 = new TextBlock();
            Title1.Text = "Название задачи:";
            stackPanel.Children.Add(Title1);
            // Добавляем TextBox для ввода названия задачи
            TextBox taskTitleTextBox = new TextBox();
            taskTitleTextBox.Margin = new Thickness(0, 5, 0, 5);
            taskTitleTextBox.Tag = "Название:";
            stackPanel.Children.Add(taskTitleTextBox);


            TextBlock Title2 = new TextBlock();
            Title2.Text = "Тип задачи:";
            stackPanel.Children.Add(Title2);
            // Добавляем ComboBox для выбора типа задачи
            ComboBox taskTypeComboBox = new ComboBox();
            taskTypeComboBox.Items.Add("Ошибка");
            taskTypeComboBox.Items.Add("Задача");
            //taskTypeComboBox.Items.Add("Improvement");
            stackPanel.Children.Add(taskTypeComboBox);


            TextBlock Title3 = new TextBlock();
            Title3.Text = "Отдел:";
            stackPanel.Children.Add(Title3);
            // Добавляем ComboBox для выбора Отдела
            ComboBox otdelComboBox = new ComboBox();


            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string query = "SELECT *FROM departaments";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            otdelComboBox.Items.Add(reader["NameDepartament"].ToString());
                        }

                    }
                }
            }
            stackPanel.Children.Add(otdelComboBox);

            

            TextBlock Title4 = new TextBlock();
            Title4.Text = "Исполнитель:";
            stackPanel.Children.Add(Title4);
            // Добавляем ComboBox для выбора исполнителя
            ComboBox assigneeComboBox = new ComboBox();



            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Humans";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                        //command.Parameters.AddWithValue("@otdelComboBoxItem", otdelComboBox.SelectedItem.ToString());
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                assigneeComboBox.Items.Add(reader["Firtsname"].ToString() + " " + reader["SecondName"].ToString() + " " + reader["ThirdName"].ToString());
                            }
                        }
                    
                    
                }
            }


            stackPanel.Children.Add(assigneeComboBox);


            Button buttonMissionOnMe = new Button();
            buttonMissionOnMe.Content = "Назначить меня";//тестовый код, переписать
            buttonMissionOnMe.Click += (sender, e) =>
            {
                assigneeComboBox.Items.Add("Иванов Иван Иванович");
                assigneeComboBox.SelectedItem = "Иванов Иван Иванович";
                assigneeComboBox.IsEnabled = false;
            };
            stackPanel.Children.Add(buttonMissionOnMe);


            TextBlock Title5 = new TextBlock();
            Title5.Text = "Описание:";
            stackPanel.Children.Add(Title5);
            // Добавляем TextBox для описания задачи
            TextBox taskDescriptionTextBox = new TextBox();
            taskDescriptionTextBox.AcceptsReturn = true;
            taskDescriptionTextBox.Height = 100;
            taskDescriptionTextBox.Margin = new Thickness(0, 5, 0, 5);
            taskDescriptionTextBox.Tag = "Введите описание:";
            stackPanel.Children.Add(taskDescriptionTextBox);

            Button CreateMission = new Button();
            CreateMission.Content = "Создать задачу";

            CreateMission.Click += (sender, e) =>
            {

                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO AllMissions (NameMission, TypeMission, Otdel, Person, Disc) VALUE (@NameMission,@TypeMission,@Otdel,@Person,@Disc)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NameMission", taskTitleTextBox.Text);
                        command.Parameters.AddWithValue("@TypeMission", taskTypeComboBox.SelectedValue.ToString());
                        command.Parameters.AddWithValue("@Otdel", otdelComboBox.SelectedValue.ToString());
                        command.Parameters.AddWithValue("@Person", assigneeComboBox.SelectedValue.ToString());
                        command.Parameters.AddWithValue("@Disc", taskDescriptionTextBox.Text);

                        command.ExecuteNonQuery();

                        Close();
                    }
                }

            };

            stackPanel.Children.Add(CreateMission);
        }
    


        private void Close()
        {
            // Удаление всех элементов из canvas
            canvas.Children.Clear();

            // Очистка всех ссылок
            canvas = null;
            closeButton = null;
        }



    }


}
