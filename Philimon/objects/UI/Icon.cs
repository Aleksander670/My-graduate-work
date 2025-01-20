using System;
using System.Collections.Generic;
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
    public class Icon : Button
    {
        Point startPoint;
        private bool isDragging;

        Canvas IcoCanvas = new Canvas();

        public event EventHandler DoubleClickEvent;

        public Canvas Icons(double width, double height, string imageSource, string text, string Func)
        {
            
            IcoCanvas.Width = width;
            IcoCanvas.Height = height;

            Rectangle rectBack = new Rectangle();
            rectBack.Width = width + 22;
            rectBack.Height = height + 22;
            rectBack.RadiusX = 4;
            rectBack.RadiusY = 4;
            IcoCanvas.Children.Add(rectBack);
            rectBack.Margin = new Thickness(-10,-8,0,0);

            Image ImageIcon = new Image();
            ImageIcon.Source = new BitmapImage(new Uri(imageSource));
            ImageIcon.Width = width;
            ImageIcon.Height = height - 4;
            ImageIcon.Stretch = Stretch.Fill;
            IcoCanvas.Children.Add(ImageIcon);
            ImageIcon.Margin = new Thickness(0,0,0,0);

            TextBlock TB = new TextBlock();
            TB.Text = text;
            TB.HorizontalAlignment = HorizontalAlignment.Center;
            IcoCanvas.Children.Add(TB);
            TB.Margin = new Thickness(((ImageIcon.Width/2 - TB.Text.Length) - TB.Text.Length * 2)-2, ImageIcon.Height - 2,0,0);


            IcoCanvas.MouseLeftButtonDown += MissionsData_MouseLeftButtonDown;

            // Обработка событий на уровне всего окна
            Application.Current.MainWindow.MouseMove += MainWindow_MouseMove;
            Application.Current.MainWindow.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;

            IcoCanvas.MouseEnter += (sender, e) =>
            {
                rectBack.Fill = Brushes.AliceBlue;
                //TB.Foreground = Brushes.White;
            };
            IcoCanvas.MouseLeave += (sender, e) =>
            {
                rectBack.Fill = Brushes.Transparent;
                TB.Foreground = Brushes.Black;
            };
            IcoCanvas.MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    rectBack.Fill = Brushes.LightBlue;
                }
                if (e.ClickCount == 2)
                {
                    rectBack.Fill = Brushes.LightSkyBlue;
                    //запускать функционал
                    DoubleClickEvent?.Invoke(this, EventArgs.Empty);
                }
            };

            IcoCanvas.MouseUp += (sender, e) =>
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    rectBack.Fill = Brushes.AliceBlue;
                }
            };
            

            return IcoCanvas;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDragging)
            {
                Point currentPoint = e.GetPosition(null);
                double deltaX = currentPoint.X - startPoint.X;
                double deltaY = currentPoint.Y - startPoint.Y;
                IcoCanvas.Margin = new Thickness(IcoCanvas.Margin.Left + deltaX, IcoCanvas.Margin.Top + deltaY, 0, 0);
                startPoint = currentPoint;
            }
        }

        private void MissionsData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            isDragging = true;
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

    }
}
