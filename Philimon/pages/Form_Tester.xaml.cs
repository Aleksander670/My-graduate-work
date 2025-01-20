using Philimon.objects.UI;
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
using MySql.Data.MySqlClient;
using Philimon.objects;

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для Form_Tester.xaml
    /// </summary>
    public partial class Form_Tester : Page
    {
        public struct Cells
        {
            public string[] NameFuch;
            public string[] AdressFuch;
            public string[] IcoAdress;
        }

        Cells CellsItems = new Cells();

        public Form_Tester()
        {
            InitializeComponent();

            CellsItems.NameFuch = new string[] { "Чек-листы", "Тест-кейсы"};

            GenerateUI();
        }

        void GenerateUI()
        {
            Canvas MainCanvas = new Canvas();
            MainCanvas.Width = Application.Current.MainWindow.Width;
            MainCanvas.Height = Application.Current.MainWindow.Height;
            Grid.Children.Add(MainCanvas);


            Canvas BackgroundDesktopCanvas = new Canvas();
            BackgroundDesktopCanvas.Width = MainCanvas.Width;
            BackgroundDesktopCanvas.Height = MainCanvas.Height;
            BackgroundDesktopCanvas.Background = Brushes.White;
            MainCanvas.Children.Add(BackgroundDesktopCanvas);

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

            Canvas TitleCanvas = new Canvas();
            TitleCanvas.Width = 365;
            TitleCanvas.Height = 80;
            TitleCanvas.Background = Brushes.White;
            TitleCanvas.Opacity = 0.8;
            TitleCanvas.Margin = new Thickness(0, 0, 0, 0);
            MainCanvas.Children.Add(TitleCanvas);



            Image Logo = new Image();
            Logo.Width = 80;
            Logo.Height = 80;
            Logo.Stretch = Stretch.Fill;
            Logo.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/tester_ico.png"));
            Logo.Margin = new Thickness(0, 0, 0, 0);
            MainCanvas.Children.Add(Logo);

            TextBlock Title = new TextBlock();
            Title.Text = "Тестирование и отладка";
            Title.FontSize = 22;
            Title.Foreground = Brushes.Black;
            Title.Margin = new Thickness(82, 22, 0, 0);
            MainCanvas.Children.Add(Title);

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


            Canvas[] ButtonSetting = new Canvas[CellsItems.NameFuch.Length];
            Image[] icons = new Image[CellsItems.NameFuch.Length];
            TextBlock[] TextButton = new TextBlock[CellsItems.NameFuch.Length];



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


            for (int i = 0; i < CellsItems.NameFuch.Length; i++)
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
                icons[i].Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/folder_ico.jpg"));
                ButtonSetting[i].Children.Add(icons[i]);
                icons[i].Margin = new Thickness(0, 0, 0, 0);

                TextButton[i] = new TextBlock();
                TextButton[i].Text = CellsItems.NameFuch[i];
                TextButton[i].FontSize = 16;
                ButtonSetting[i].Children.Add(TextButton[i]);
                TextButton[i].Margin = new Thickness(64, 60 / 2 - (TextButton[i].FontSize - 2), 0, 0);


                ButtonSetting[i].Tag = i;

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



                ButtonSetting[i].MouseDown += (sender, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        ContentSettingPanel.Visibility = Visibility.Visible;
                        int canvasIndex = (int)((Canvas)sender).Tag;

                        ((Canvas)sender).Background = Brushes.Gray;

                        if (canvasIndex == 0)
                        {
                            ContentScroll.Content = CheckListCanvas();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 1)
                        {
                            ContentScroll.Content = Test_Cases();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        /*
                        if (canvasIndex == 2)
                        {
                            ContentScroll.Content = positionPeople();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 3)
                        {
                            ContentScroll.Content = Projects();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        if (canvasIndex == 4)
                        {
                            ContentScroll.Content = Sources();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        */
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

        public Canvas CheckListCanvas()
        {

            DatabaseData DB = new DatabaseData();

            Canvas Test_CheckListFormObj = new Canvas();
            Test_CheckListFormObj.Height = Application.Current.MainWindow.Height;
            Test_CheckListFormObj.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = Title;
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Text = "Чек-листы";
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            Test_CheckListFormObj.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            Test_CheckListFormObj.Children.Add(line1);

            TextBox searchBox = new TextBox();
            searchBox.Width = 200;
            searchBox.Height = 40;
            searchBox.Tag = "Поиск:";
            searchBox.Visibility = Visibility.Hidden;
            searchBox.Margin = new Thickness(5, 90, 0, 0);
            Test_CheckListFormObj.Children.Add(searchBox);

            Button ViewButton = new Button();
            ViewButton.Width = 200;
            ViewButton.Height = 40;
            ViewButton.Content = "Просмотр данных";
            ViewButton.Margin = new Thickness(500, 90, 0, 0);
            Test_CheckListFormObj.Children.Add(ViewButton);

            Button AddButton = new Button();
            AddButton.Width = 200;
            AddButton.Height = 40;
            AddButton.Content = "Добавить данные";
            AddButton.Margin = new Thickness(500 + AddButton.Width + 30, 90, 0, 0);
            Test_CheckListFormObj.Children.Add(AddButton);

            Canvas ViewCanvas = new Canvas();
            ViewCanvas.Margin = new Thickness(0, 140, 0, 0);
            ViewCanvas.Width = Application.Current.MainWindow.Width - 390;
            ViewCanvas.Height = Application.Current.MainWindow.Height - 100;
            ViewCanvas.Visibility = Visibility.Hidden;
            Test_CheckListFormObj.Children.Add(ViewCanvas);

            StackPanel ViewCanvasStackPanel = new StackPanel();
            ViewCanvasStackPanel.Width = ViewCanvas.Width;
            ViewCanvasStackPanel.Height = ViewCanvas.Height;
            ViewCanvas.Children.Add(ViewCanvasStackPanel);


            Canvas AddCanvas = new Canvas();
            AddCanvas.Margin = new Thickness(0, 140, 0, 0);
            AddCanvas.Width = Application.Current.MainWindow.Width - 390;
            AddCanvas.Height = Application.Current.MainWindow.Height - 100;
            AddCanvas.Visibility = Visibility.Hidden;
            Test_CheckListFormObj.Children.Add(AddCanvas);

            AddButton.Click += (sender, e) =>
            {
                int id = 0;
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT MAX(id) FROM AllCheckLists";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            id = Convert.ToInt32(result);
                        }
                    }
                }
                this.NavigationService.Navigate(new CheckList_ViewAndCreate("create", id));
            };

            ViewButton.Click += (sender, e) =>
            {

                Test_CheckListFormObj.Height = Application.Current.MainWindow.Height;
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
                Attr1.Text = "Название";
                Attr1.FontSize = 20;
                Attr1.Margin = new Thickness(200, 14, 0, 0);

                TextBlock Attr2 = new TextBlock();
                Attr2.Text = "Проект";
                Attr2.FontSize = 20;
                Attr2.Margin = new Thickness(900, 14, 0, 0);

                ViewCanvasStackPanel.Children.Add(HeaderCanvas);
                HeaderCanvas.Children.Add(Attr1);
                HeaderCanvas.Children.Add(Attr2);

                string QueryViewTable = "SELECT *FROM AllCheckLists";

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

                                Items = item.Construct(1200, 64, reader["NameCL"].ToString(), reader["Project"].ToString(), "");
                                Items.Tag = Convert.ToInt32(reader["id"]);

                                ViewCanvasStackPanel.Children.Add(Items);

                                Items.MouseDown += (sender1, e1) =>
                                {
                                    if (e1.LeftButton == MouseButtonState.Pressed)
                                    {
                                        this.NavigationService.Navigate(new CheckList_ViewAndCreate("view", Convert.ToInt32(Items.Tag)));
                                    }
                                };

                                i++;

                                if (i > 12)
                                {
                                    ViewCanvasStackPanel.Height += 86;
                                    Test_CheckListFormObj.Height = ViewCanvasStackPanel.Height;
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


            return Test_CheckListFormObj;
        }

        public Canvas Test_Cases()
        {
            DatabaseData DB = new DatabaseData();

            Canvas Test_CasesFormObj = new Canvas();
            Test_CasesFormObj.Height = Application.Current.MainWindow.Height;
            Test_CasesFormObj.Margin = new Thickness(10, 0, 0, 0);

            TextBlock TitleSetting_1 = new TextBlock();
            TitleSetting_1.Text = Title;
            TitleSetting_1.FontSize = 26;
            TitleSetting_1.Foreground = Brushes.White;
            TitleSetting_1.Text = "Тест-кейсы";
            TitleSetting_1.Margin = new Thickness(25, 25, 0, 0);
            Test_CasesFormObj.Children.Add(TitleSetting_1);
            Line line1 = new Line()
            {
                X1 = 40,
                Y1 = 75,
                X2 = Application.Current.MainWindow.Width - 60,
                Y2 = 75,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            Test_CasesFormObj.Children.Add(line1);

            TextBox searchBox = new TextBox();
            searchBox.Width = 200;
            searchBox.Height = 40;
            searchBox.Tag = "Поиск:";
            searchBox.Visibility = Visibility.Hidden;
            searchBox.Margin = new Thickness(5, 90, 0, 0);
            Test_CasesFormObj.Children.Add(searchBox);

            Button ViewButton = new Button();
            ViewButton.Width = 200;
            ViewButton.Height = 40;
            ViewButton.Content = "Просмотр данных";
            ViewButton.Margin = new Thickness(500, 90, 0, 0);
            Test_CasesFormObj.Children.Add(ViewButton);

            Button AddButton = new Button();
            AddButton.Width = 200;
            AddButton.Height = 40;
            AddButton.Content = "Добавить данные";
            AddButton.Margin = new Thickness(500 + AddButton.Width + 30, 90, 0, 0);
            Test_CasesFormObj.Children.Add(AddButton);

            Canvas ViewCanvas = new Canvas();
            ViewCanvas.Margin = new Thickness(0, 140, 0, 0);
            ViewCanvas.Width = Application.Current.MainWindow.Width - 390;
            ViewCanvas.Height = Application.Current.MainWindow.Height - 100;
            ViewCanvas.Visibility = Visibility.Hidden;
            Test_CasesFormObj.Children.Add(ViewCanvas);

            StackPanel ViewCanvasStackPanel = new StackPanel();
            ViewCanvasStackPanel.Width = ViewCanvas.Width;
            ViewCanvasStackPanel.Height = ViewCanvas.Height;
            ViewCanvas.Children.Add(ViewCanvasStackPanel);


            Canvas AddCanvas = new Canvas();
            AddCanvas.Margin = new Thickness(0, 140, 0, 0);
            AddCanvas.Width = Application.Current.MainWindow.Width - 390;
            AddCanvas.Height = Application.Current.MainWindow.Height - 100;
            AddCanvas.Visibility = Visibility.Hidden;
            Test_CasesFormObj.Children.Add(AddCanvas);

            AddButton.Click += (sender, e) =>
            {
                int id = 0;
                using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT MAX(id) FROM AllTestCases";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            id = Convert.ToInt32(result);
                        }
                    }
                }
                this.NavigationService.Navigate(new TestCase_ViewAndCreate("create", id));
            };

            ViewButton.Click += (sender, e) =>
            {

                Test_CasesFormObj.Height = Application.Current.MainWindow.Height;
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
                Attr1.Text = "Название";
                Attr1.FontSize = 20;
                Attr1.Margin = new Thickness(200, 14, 0, 0);

                TextBlock Attr2 = new TextBlock();
                Attr2.Text = "Проект";
                Attr2.FontSize = 20;
                Attr2.Margin = new Thickness(900, 14, 0, 0);

                ViewCanvasStackPanel.Children.Add(HeaderCanvas);
                HeaderCanvas.Children.Add(Attr1);
                HeaderCanvas.Children.Add(Attr2);

                string QueryViewTable = "SELECT *FROM AllTestCases";

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

                                Items = item.Construct(1200, 64, reader["NameTK"].ToString(), reader["Project"].ToString(), "");
                                Items.Tag = Convert.ToInt32(reader["id"]);

                                ViewCanvasStackPanel.Children.Add(Items);

                                Items.MouseDown += (sender1, e1) =>
                                {
                                    if (e1.LeftButton == MouseButtonState.Pressed)
                                    {
                                        this.NavigationService.Navigate(new TestCase_ViewAndCreate("view", Convert.ToInt32(Items.Tag)));
                                    }
                                };

                                i++;

                                if (i > 12)
                                {
                                    ViewCanvasStackPanel.Height += 86;
                                    Test_CasesFormObj.Height = ViewCanvasStackPanel.Height;
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


            return Test_CasesFormObj;
        }

        public Canvas positionPeople()
        {

            FormObj positionPeopleFormObj = new FormObj();

            string[] viewAttr = { "NamePosition", "short_description" };
            string[] tableAttr = { "Название должности", "Краткое описание" };

            string[] arrtibytesFromDataBase = { "NamePosition", "icoPatch", "Payment", "short_description", "substruction" };
            string[] arrtibytesLocaliz = { "Наименование должности*", "Изображение*", "Оплата*", "Краткое описание", "Описание (полное)*" };
            string[] attrTypes = { };

            Canvas departmentsCanvas = positionPeopleFormObj.GenerateForm(this, "Должности:", "SELECT id, NamePosition, short_description FROM positionPeople", viewAttr, "positionPeople", tableAttr, viewAttr, "positionPeople", arrtibytesFromDataBase, arrtibytesLocaliz, null, attrTypes);

            return departmentsCanvas;
        }

        public Canvas Projects()
        {

            FormObj ProjectsFormObj = new FormObj();

            string[] viewAttr = { "NameProject", "departaments" };
            string[] tableAttr = { "Название проекта", "Отдел разработки" };
            string[] attrTypes = { };

            string[] arrtibytesFromDataBase = { "NameProject", "icoPatch", "budget", "departaments", "Leader", "substruction" };
            string[] arrtibytesLocaliz = { "Название проекта*", "Изображение*", "Бюджет*", "Отдел разработки", "Глава разработки*", "Описание" };

            Canvas departmentsCanvas = ProjectsFormObj.GenerateForm(this, "Проекты:", "SELECT id, NameProject, departaments FROM Projects", viewAttr, "Projects", tableAttr, viewAttr, "Projects", arrtibytesFromDataBase, arrtibytesLocaliz, null, attrTypes);

            return departmentsCanvas;
        }

        public Canvas Sources()
        {

            FormObj ProjectsFormObj = new FormObj();

            string[] viewAttr = { "NameSource", "amount" };
            string[] tableAttr = { "Наименование", "Количество" };
            string[] attrTypes = { };

            string[] arrtibytesFromDataBase = { "NameSource", "icoPatch", "price", "amount", "manufacturer", "serialNumber", "substruction" };
            string[] arrtibytesLocaliz = { "Название ресурса*", "Изображение*", "Цена*", "Количество", "Производитель*", "Серийный номер", "Описание" };

            Canvas departmentsCanvas = ProjectsFormObj.GenerateForm(this, "Ресурсы:", "SELECT id, NameSource, amount FROM Sources", viewAttr, "Sources", tableAttr, viewAttr, "Sources", arrtibytesFromDataBase, arrtibytesLocaliz, null, attrTypes);

            return departmentsCanvas;
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


    }


}

