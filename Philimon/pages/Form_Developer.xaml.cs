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

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для Form_Developer.xaml
    /// </summary>
    public partial class Form_Developer : Page
    {
        public struct Cells
        {
            public string[] NameFuch;
            public string[] AdressFuch;
            public string[] IcoAdress;
        }

        Cells CellsItems = new Cells();

        public Form_Developer()
        {
            InitializeComponent();

            CellsItems.NameFuch = new string[] { "Технические задания"};

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
            Logo.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/infract.png"));
            Logo.Margin = new Thickness(0, 0, 0, 0);
            MainCanvas.Children.Add(Logo);

            TextBlock Title = new TextBlock();
            Title.Text = "Проектирование\nтехнического задания";
            Title.FontSize = 22;
            Title.Foreground = Brushes.Black;
            Title.Margin = new Thickness(82, 10, 0, 0);
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
                            ContentScroll.Content = TechnicalMissions();
                            ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        /*
                        if (canvasIndex == 1)
                        {
                            ContentScroll.Content = Diagrams();
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

        public Canvas TechnicalMissions()
        {

            FormObj HumansFormObj = new FormObj();

            string[] viewAttr = { "NameTM", "Project" };
            string[] tableAttr = { "Название", "Проект" };

            string[] arrtibytesFromDataBase = { "Firtsname", "SecondName", "ThirdName", "icoPatch", "Data_of_employment", "departaments", "positionPeople", "gender", "Data_of_born", "numberPhone", "Email" };
            string[] arrtibytesLocaliz = { "Фамилия*", "Имя*", "Отчество*", "Фотография", "Дата трудоустройства*", "Отдел*", "Должность*", "Пол*", "Дата рождения*", "Номер телефона*", "Электронная почта" };
            string[] attrTypes = { "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text" };

            Canvas departmentsCanvas = HumansFormObj.GenerateForm(this, "Технические задания:", "SELECT id, NameTM, Project FROM AllTechnicalMissions", viewAttr, "AllTechnicalMissions", tableAttr, viewAttr, "AllTechnicalMissions", arrtibytesFromDataBase, arrtibytesLocaliz, new NotePad(this), attrTypes);

            return departmentsCanvas;
        }

        public Canvas Diagrams()
        {

            FormObj departmentsFormObj = new FormObj();

            string[] viewAttr = { "NameD", "Project" };
            string[] tableAttr = { "Название", "Проект" };
            

            string[] arrtibytesFromDataBase = { "NameDepartament", "Abbreviated", "icoPatch", "Leader", "budget", "substruction" };
            string[] arrtibytesLocaliz = { "Название отдела*", "Сокращённое наименование отдела*", "Изображение*", "Руководитель*", "Ежемесячный бюджет*", "Описание*" };
            string[] attrTypes = { "text", "text", "text", "text", "text", "text" };

            Canvas departmentsCanvas = departmentsFormObj.GenerateForm(this, "Диаграммы:", "SELECT id, NameD, Project FROM AllDiagrams", viewAttr, "AllDiagrams", tableAttr, viewAttr, "AllDiagrams", arrtibytesFromDataBase, arrtibytesLocaliz, new NotePad(this), attrTypes);

            return departmentsCanvas;
        }


    }
}
