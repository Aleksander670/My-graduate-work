using MySql.Data.MySqlClient;
using Philimon.objects;
using System;
using System.Collections.Generic;
using System.Data;
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

    public partial class redact_database : Page
    {

        DatabaseData DB = new DatabaseData();

        public redact_database()
        {
            InitializeComponent();

            GenerateUI();
        

        }

        void GenerateUI()
        {
            // Создание заднего фона
            Canvas canvas = new Canvas();
            canvas.Background = Brushes.Purple;
            canvas.Width = Application.Current.MainWindow.Width;
            canvas.Height = Application.Current.MainWindow.Height;

            // Добавление ScrollViewer для прокрутки
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            

           // Создание Grid для размещения квадратных объектов
            Grid grid = new Grid();
            grid.Margin = new Thickness(40,40,0,0);
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

            Brush[] BrushesColor = {
                Brushes.LimeGreen, Brushes.Red,
                Brushes.Blue, Brushes.Yellow,
                Brushes.Orange, Brushes.Cyan,
                Brushes.Magenta, Brushes.Brown,
                Brushes.LightBlue, Brushes.LightGreen,
                Brushes.Silver, Brushes.Gray
            };

            Random r = new Random();

            int tablesCount = GetTablesCountFromDatabase(); // Получение количества таблиц из базы данных 
            int columns, rows;

            if (tablesCount % 2 == 0) // Если tablesCount четное число
            {
                columns = tablesCount / 2;
                rows = 2;
            }
            else // Если tablesCount нечетное число
            {
                columns = tablesCount / 3 + 1;
                rows = 3;
            }

            // Добавление квадратных объектов в Grid      
            double squareSize = 200; // Размер квадрата      
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Border square = new Border();
                    square = TableInformation();
                    square.Background = BrushesColor[r.Next(0, BrushesColor.Length)];
                    square.Width = squareSize;
                    square.Height = squareSize;
                    square.Margin = new Thickness(10);
                    Canvas.SetLeft(square, column * (squareSize + 10));
                    Canvas.SetTop(square, row * (squareSize + 10));
                    canvas.Children.Add(square);
                }
            }

            scrollViewer.Content = grid;

            // Добавление Grid в ScrollViewer
            canvas.Children.Add(scrollViewer);
            Grid.Children.Add(canvas);


            Button back = new Button();
            back.Content = "Назад";
            back.Width = 40;
            back.Height = 20;
            Grid.Children.Add(back);
        }


        private Border TableInformation()
        {
            
            Border TableIco = new Border();
            TableIco.Width = 200;
            TableIco.Height = 200;

            Image image = new Image();
            image.Width = 100;
            image.Height = 100;
            image.Margin = new Thickness(5,5,0,0);
            image.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/tableIco.png"));
            TableIco.Child = image;

            


            return TableIco;
        }



        private int GetTablesCountFromDatabase()
        {

            int coll = 0;
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Получаем список таблиц в базе данных
                DataTable schema = connection.GetSchema("Tables");

                // Создаем объекты Table на основе списка таблиц
                List<Table> tables = new List<Table>();
                foreach (DataRow row in schema.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    coll++;
                }

                
            }
            return coll;
        }

        private string GetTablesNameFromDatabase()
        {
            string name = "";
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Получаем список таблиц в базе данных
                DataTable schema = connection.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    name = row["TABLE_NAME"].ToString();
                }


            }
            return name;
        }




    }





    public class Table : Canvas
    {

        public Canvas canvas;
        private Button closeButton;
#pragma warning disable CS0169 // Поле "Table.directoryPath" никогда не используется.
        private string directoryPath;
#pragma warning restore CS0169 // Поле "Table.directoryPath" никогда не используется.

        public Table(string NameTable)
        {
            int width = 800;
            int height = 400;

            canvas = new Canvas();
            canvas.Width = width;
            canvas.Height = height;

            // Создание Header и установка его расположения внутри родительского Canvas
            Canvas header = new Canvas();
            header.Width = width;
            header.Height = 32;
            header.Background = Brushes.WhiteSmoke;
            canvas.Children.Add(header);

            TextBlock Title = new TextBlock();
            Title.Margin = new Thickness(56, 7, 0, 0);
            Title.Text = NameTable;
            header.Children.Add(Title);

            // Установка расположения Header
            Canvas.SetTop(header, 0);

            // Установка расположения StackPanel
            StackPanel stack = new StackPanel();
            stack.Background = Brushes.Gray;
            stack.Width = width;
            stack.Height = height - 32; // Вычитаем высоту Header, чтобы StackPanel занимал оставшееся пространство
            canvas.Children.Add(stack);

            // Установка расположения StackPanel
            Canvas.SetTop(stack, 32); // Располагаем StackPanel под Header

            closeButton = new Button();
            closeButton.Background = Brushes.White;
            closeButton.Content = "   🡃   ";
            closeButton.Click += (sender, e) => { Close(); };
            header.Children.Add(closeButton); // Добавляем кнопку в Header
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
