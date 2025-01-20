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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.Data;
using Philimon.objects;
using System.Diagnostics;
using System.IO;

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для AutrizPage.xaml
    /// </summary>
    public partial class AutrizPage : Page
    {
        DatabaseData DB = new DatabaseData();

        string ConnectionString = "";
        int codeErrorInt = 0;
        public AutrizPage()
        {
            ConnectionString = DB.ConnectionString;

            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                Load();
            };

        }

        void Load()
        {

            

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                    try
                    {
                        connection.Open();
                        GenerateUiMain();
                    }
                    catch (MySqlException ex)
                    {

                            // Логирование ошибки или другие действия при неудачной попытке
                            if (ex.Number == 1042)
                            {
                                codeErrorInt = 1042;
                            }
                            if (ex.Number == 1045)
                            {
                                codeErrorInt = 1045;
                            }

                            GenerateWindowErrorConnect();
                        
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                
            }


        }

        void GenerateUiMain()
        {
            
            Canvas MainCanvas = new Canvas();
            MainCanvas.Width = Application.Current.MainWindow.Width;
            MainCanvas.Height = Application.Current.MainWindow.Height;
            MainCanvas.Margin = new Thickness(0,0,0,0);
            MainCanvas.Background = Brushes.LightGray;
            Grid.Children.Add(MainCanvas);

            Image Ico = new Image();
            Ico.Width = 128;
            Ico.Height = 128;
            Ico.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/logo.png"));
            Ico.Margin = new Thickness(Application.Current.MainWindow.Width/2- Ico.Width / 2, Application.Current.MainWindow.Height/2 - Ico.Height, 0,0);
            MainCanvas.Children.Add(Ico);

            ProgressBar Loading = new ProgressBar();
            Loading.Width = 360;
            Loading.Height = 26;
            Loading.Margin = new Thickness(Application.Current.MainWindow.Width/2- Loading.Width/2,280,0,0);
            MainCanvas.Children.Add(Loading);

            Canvas footer = new Canvas();
            footer.Width = Application.Current.MainWindow.Width;
            footer.Height = 90;
            footer.Margin = new Thickness(0,Application.Current.MainWindow.Height - footer.Height,0,0);
            footer.Background = Brushes.Gray;
            MainCanvas.Children.Add(footer);

            Image CompanyLogo = new Image();
            CompanyLogo.Width = 96;
            CompanyLogo.Height = 108;
            CompanyLogo.Stretch = Stretch.Uniform;
            CompanyLogo.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - CompanyLogo.Width / 2, 490 - CompanyLogo.Height, 0, 0);
            CompanyLogo.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/company_test.png"));
            MainCanvas.Children.Add(CompanyLogo);

            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 0;

            DispatcherTimer Timer1 = new DispatcherTimer();
            Timer1.Interval = TimeSpan.FromSeconds(0.01);
            Timer1.Start();
            Timer1.Tick += (sender, e) =>
            {
                if (Loading.Value < 100) {
                    Loading.Value++;
                }
                if (Loading.Value >= 100)
                {
                    if(blurEffect.Radius < 5)
                    {
                        blurEffect.Radius += 0.1;
                        Ico.Effect = blurEffect;
                        Loading.Effect = blurEffect;
                        footer.Effect = blurEffect;
                        CompanyLogo.Effect = blurEffect;
                    }
                    if (blurEffect.Radius >= 3)
                    {
                        GenerateFormLogin(MainCanvas);
                    }
                    if (blurEffect.Radius >= 5)
                    {
                        Timer1.Stop();
                    }
                }
            };

            Button button = new Button();
            button.Content = "☼";
            button.Width = 30;
            button.Height = 30;
            button.Margin = new Thickness(0, 0, 0, 0);
            MainCanvas.Children.Add(button);

            button.Click += (sender, e) =>
            {
                // Получаем путь к папке с исполняемым файлом приложения
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                // Формируем полный путь к файлу Connector.exe
                string connectorPath = Path.Combine(appDirectory, "Connector.exe");

                try
                {
                    // Создаем новый процесс для запуска приложения Connector.exe
                    Process connectorProcess = new Process();
                    connectorProcess.StartInfo.FileName = connectorPath;
                    connectorProcess.StartInfo.UseShellExecute = true;

                    // Запускаем процесс и ждем его завершения
                    connectorProcess.Start();
                    connectorProcess.WaitForExit(); // Основное приложение будет неактивно, пока Connector.exe запущен
                }
                catch (Exception ex)
                {
                    // В случае ошибки выводим сообщение
                    MessageBox.Show("Ошибка при запуске Connector.exe: " + ex.Message);
                }
            };

        }

        void GenerateFormLogin(Canvas MainCanvas)
        {
            Canvas FormLogin = new Canvas();
            FormLogin.Width = 550;
            FormLogin.Height = 280;
            FormLogin.Background = Brushes.White;
            FormLogin.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - FormLogin.Width / 2, Application.Current.MainWindow.Height / 2 - FormLogin.Height / 2, 0, 0);
            FormLogin.Opacity = 0;
            MainCanvas.Children.Add(FormLogin);

            TextBlock Title = new TextBlock();
            Title.Text = "Авторизация:";
            Title.Margin = new Thickness(10, 10, 0, 0);
            FormLogin.Children.Add(Title);

            TextBox LoginBox = new TextBox();
            LoginBox.Width = 300;
            LoginBox.Height = 35;
            LoginBox.Tag = "Логин:";
            LoginBox.Margin = new Thickness(FormLogin.Width/2- LoginBox.Width/2, 100,0,0);
            FormLogin.Children.Add(LoginBox);

            TextBox PasswordBox = new TextBox();
            PasswordBox.Width = 300;
            PasswordBox.Height = 35;
            PasswordBox.Tag = "Пароль:";
            PasswordBox.Margin = new Thickness(FormLogin.Width / 2 - PasswordBox.Width / 2, 100 + LoginBox.Height + 25, 0, 0);
            FormLogin.Children.Add(PasswordBox);

            CheckBox RememberMe = new CheckBox();
            RememberMe.Content = "Запомнить меня";
            RememberMe.Width = RememberMe.Content.ToString().Length * RememberMe.FontSize;
            RememberMe.Height = 2 * RememberMe.FontSize;
            RememberMe.Margin = new Thickness((FormLogin.Width / 2 - RememberMe.Width)+18, 130 + PasswordBox.Height + 34, 0, 0);
            FormLogin.Children.Add(RememberMe);

            Button LoginEnterButton = new Button();
            LoginEnterButton.Width = 150;
            LoginEnterButton.Height = 30;
            LoginEnterButton.Content = "Войти";
            LoginEnterButton.Margin = new Thickness(FormLogin.Width / 2 - LoginEnterButton.Width, (100 + LoginBox.Height + PasswordBox.Height) + 60, 0, 0);
            FormLogin.Children.Add(LoginEnterButton);
            
            Button LostPasswordButton = new Button();
            LostPasswordButton.Width = 150;
            LostPasswordButton.Height = 30;
            LostPasswordButton.Content = "Забыл пароль";
            LostPasswordButton.Margin = new Thickness((FormLogin.Width / 2 + LoginEnterButton.Width) - LostPasswordButton.Width, (100 + LoginBox.Height + PasswordBox.Height) + 60, 0, 0);
            FormLogin.Children.Add(LostPasswordButton);


            LostPasswordButton.Click += (sender, e) =>
            {
                Canvas background = new Canvas();
                background.Background = Brushes.Black;
                background.Width = Application.Current.MainWindow.Width;
                background.Height = Application.Current.MainWindow.Height;
                background.Opacity = 0.9;
                MainCanvas.Children.Add(background);

                Canvas Form = new Canvas();
                Form.Width = 400;
                Form.Height = 400;
                Form.Background = Brushes.WhiteSmoke;
                Form.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - Form.Width / 2, Application.Current.MainWindow.Height / 2 - Form.Height / 2, 0, 0);
                MainCanvas.Children.Add(Form);

                TextBox name = new TextBox();
                name.Tag = "Ваше имя*:";

                TextBox disc = new TextBox();
                disc.Tag = "Напишите информацию о Вас,\nвашем рабочем месте.\nУкажите, как потеряли данные\nавторизации*:";

                Canvas Header = new Canvas();
                Header.Width = Form.Width;
                Header.Height = 40;
                Header.Background = Brushes.AliceBlue;
                Form.Children.Add(Header);

                StackPanel stack = new StackPanel();
                stack.Width = Form.Width;
                stack.Height = Form.Height - Header.Height;
                stack.Margin = new Thickness(0, 60, 0, 0);
                Form.Children.Add(stack);

                name.Width = 400;
                name.Height = 40;
                stack.Children.Add(name);

                disc.Width = 400;
                disc.Height = 100;
                stack.Children.Add(disc);

                TextBox Feedback = new TextBox();
                Feedback.Tag = "Данные для связи с вами (email, тел. и т.д)*:";
                Feedback.Width = 400;
                Feedback.Height = 50;
                stack.Children.Add(Feedback);


                Button WriteButton = new Button();
                WriteButton.Width = 400;
                WriteButton.Height = 40;
                WriteButton.Content = "Записать данные";
                stack.Children.Add(WriteButton);

                WriteButton.Click += (sender1, e1) =>
                {
                    if (!string.IsNullOrEmpty(name.Text) && !string.IsNullOrEmpty(disc.Text) && !string.IsNullOrEmpty(Feedback.Text))
                    {
                        using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                        {
                            connection.Open();

                            string query = "INSERT INTO Requests_password (NameRequest, disc, connectionText) VALUES (@NameRequest, @disc, @connectionText)";

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@NameRequest", name.Text);
                                command.Parameters.AddWithValue("@disc", disc.Text);
                                command.Parameters.AddWithValue("@connectionText", Feedback.Text);

                                command.ExecuteNonQuery();

                                name.BorderBrush = Brushes.Lime;
                                disc.BorderBrush = Brushes.Lime;
                                Feedback.BorderBrush = Brushes.Lime;
                                WriteButton.BorderBrush = Brushes.Lime;
                            }
                        }
                    }
                    else
                    {
                        name.BorderBrush = Brushes.Red;
                        disc.BorderBrush = Brushes.Red;
                        Feedback.BorderBrush = Brushes.Red;
                        WriteButton.BorderBrush = Brushes.Red;
                    }
                };




                Button Close = new Button();
                Close.Width = 38;
                Close.Height = 38;
                Close.Margin = new Thickness(360, 0, 0, 0);
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

                    this.NavigationService.Navigate(this);
                };
            };

            Image Ico = new Image();
            Ico.Width = 128;
            Ico.Height = 128;
            Ico.Opacity = 0;
            Ico.Visibility = Visibility.Collapsed;
            Ico.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/logo.png"));
            Ico.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - Ico.Width / 2, Application.Current.MainWindow.Height / 2 - Ico.Height + 70, 0, 0);
            MainCanvas.Children.Add(Ico);

            

            int opacityAnim = 0;
            bool startanim = false;
            int secondSearchProfile = 0;

            DispatcherTimer TimerLogin = new DispatcherTimer();
            TimerLogin.Interval = TimeSpan.FromSeconds(0.01);

            BlurEffect BlurEffLogin = new BlurEffect();
            BlurEffLogin.Radius = 0;

            TimerLogin.Tick += (sender, e) =>
            {
                if (Title.Opacity > 0 && startanim == false)
                {
                    Title.Opacity -= 0.1;
                    LoginBox.Opacity -= 0.1;
                    PasswordBox.Opacity -= 0.1;
                    RememberMe.Opacity -= 0.1;
                    LoginEnterButton.Opacity -= 0.1;
                    LostPasswordButton.Opacity -= 0.1;
                    
                }

                if (LoginEnterButton.Opacity <= 0)
                {
                    RememberMe.Visibility = Visibility.Collapsed;
                    LoginEnterButton.Visibility = Visibility.Collapsed;
                    LostPasswordButton.Visibility = Visibility.Collapsed;
                }

                if (opacityAnim == 1)
                {
                    Ico.Opacity -= 0.1;
                }
                if (opacityAnim == 0)
                {
                    Ico.Opacity += 0.1;
                }

                if (Ico.Opacity >= 1)
                {
                    opacityAnim = 1;
                }
                if (Ico.Opacity <= 0)
                {
                    opacityAnim = 0;
                }

                if (secondSearchProfile < 60)
                {
                    secondSearchProfile++;
                }

                if (secondSearchProfile >= 60)
                {
                    startanim = true;
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();

                        int id = 0;

                        string query = "SELECT *FROM users";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                bool userFound = false; // Флаг, указывающий, был ли найден пользователь

                                while (reader.Read()){

                                    string login = reader["login"].ToString();
                                    string password = reader["password"].ToString();

                                    id++;

                                    if (LoginBox.Text == login && PasswordBox.Text == password)
                                    {
                                        // Если пользователь найден, выполняем соответствующие действия
                                        userFound = true;

                                        if (Ico.Opacity > 0)
                                        {
                                            Ico.Opacity -= 0.1;
                                        }
                                        FormLogin.Width += 8;
                                        FormLogin.Height += 8;
                                        FormLogin.Effect = BlurEffLogin;
                                        BlurEffLogin.Radius += 1;
                                        FormLogin.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - FormLogin.Width / 2, Application.Current.MainWindow.Height / 2 - FormLogin.Height / 2, 0, 0);
                                        if (FormLogin.Width > 1400)
                                        {
                                            TimerLogin.Stop();
                                            this.NavigationService.Navigate(new OK());
                                        }

                                        ((Main)Application.Current.MainWindow).PersonID = id;

                                    }
                                }

                                if (!userFound)
                                {
                                    LoginBox.BorderBrush = Brushes.Red;
                                    PasswordBox.BorderBrush = Brushes.Red;

                                    LoginBox.Foreground = Brushes.Red;
                                    PasswordBox.Foreground = Brushes.Red;
                                    if (Ico.Opacity > 0)
                                    {
                                        Ico.Opacity -= 0.1;
                                    }
                                    if (Ico.Opacity <= 0)
                                    {
                                        Ico.Visibility = Visibility.Hidden;
                                    }
                                    if (Title.Opacity < 1 && startanim == true)
                                    {
                                        Title.Opacity += 0.1;
                                        LoginBox.Opacity += 0.1;
                                        PasswordBox.Opacity += 0.1;
                                        RememberMe.Opacity += 0.1;
                                        LoginEnterButton.Opacity += 0.1;
                                        LostPasswordButton.Opacity += 0.1;
                                    }
                                    if (LoginEnterButton.Opacity >= 0.1)
                                    {
                                        RememberMe.Visibility = Visibility.Visible;
                                        LoginEnterButton.Visibility = Visibility.Visible;
                                        LostPasswordButton.Visibility = Visibility.Visible;
                                    }
                                    if (Title.Opacity >= 1)
                                    {
                                        opacityAnim = 0;
                                        startanim = false;
                                        secondSearchProfile = 0;
                                        TimerLogin.Stop();
                                    }
                                }

                            

                            }

                        }

                        

                    }


                }

            };

            

            LoginEnterButton.Click += (sender, e) =>
            {
                //Авторизация...
                TimerLogin.Start();
                Ico.Visibility = Visibility.Visible;

            };


            DispatcherTimer Timer2 = new DispatcherTimer();
            Timer2.Interval = TimeSpan.FromSeconds(0.01);
            Timer2.Start();
            Timer2.Tick += (sender, e) =>
            {
                if (FormLogin.Opacity < 1)
                {
                    FormLogin.Opacity += 0.1;
                }
                else
                {
                    Timer2.Stop();
                }
            };

            


        }

        void GenerateWindowErrorConnect()
        {
            Canvas MainCanvas = new Canvas();
            MainCanvas.Width = Application.Current.MainWindow.Width;
            MainCanvas.Height = Application.Current.MainWindow.Height;
            MainCanvas.Margin = new Thickness(0, 0, 0, 0);
            MainCanvas.Background = Brushes.LightGray;
            Grid.Children.Add(MainCanvas);

            Canvas ErrorPanel = new Canvas();
            ErrorPanel.Width = 550;
            ErrorPanel.Height = 280;
            ErrorPanel.Background = Brushes.White;
            ErrorPanel.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - ErrorPanel.Width / 2, Application.Current.MainWindow.Height / 2 - ErrorPanel.Height / 2, 0, 0);
            ErrorPanel.Opacity = 0;
            MainCanvas.Children.Add(ErrorPanel);

            TextBlock Warring = new TextBlock();
            Warring.Text = "-ОШИБКА-";
            Warring.Margin = new Thickness((ErrorPanel.Width/2 - Warring.Text.Length) - Warring.FontSize, 40, 0, 0);
            ErrorPanel.Children.Add(Warring);

            TextBlock CodeError = new TextBlock();
            CodeError.Text = "Код:" + codeErrorInt.ToString();
            CodeError.Margin = new Thickness((ErrorPanel.Width / 2 - CodeError.Text.Length) - CodeError.FontSize, 80, 0, 0);
            ErrorPanel.Children.Add(CodeError);

            TextBlock CodeErrorText = new TextBlock();
            if (codeErrorInt == 1042)
            {
                CodeErrorText.Text = "Ошибка при подключении к серверу!";
            }
            else
            {
                CodeErrorText.Text = "Неправильные данные для подключения к серверу!";
            }
            CodeErrorText.HorizontalAlignment = HorizontalAlignment.Center;
            CodeErrorText.Margin = new Thickness((ErrorPanel.Width / 2), 160, 0, 0);
            ErrorPanel.Children.Add(CodeErrorText);

            Button ExitEnterButton = new Button();
            ExitEnterButton.Width = 240;
            ExitEnterButton.Height = 30;
            ExitEnterButton.Content = "Закрыть Philimon";
            ExitEnterButton.Margin = new Thickness(ErrorPanel.Width / 2 - ExitEnterButton.Width/2, (ErrorPanel.Height - 50), 0, 0);
            ErrorPanel.Children.Add(ExitEnterButton);

            ExitEnterButton.Click += (sender, e) =>
            {
                Application.Current.Shutdown();
            };

            DispatcherTimer TimerError = new DispatcherTimer();
            TimerError.Interval = TimeSpan.FromSeconds(0.01);
            TimerError.Start();
            TimerError.Tick += (sender, e) =>
            {
                if (ErrorPanel.Opacity < 1)
                {
                    ErrorPanel.Opacity += 0.1;
                }
                else
                {
                    TimerError.Stop();
                }
            };

        }


    }
}
