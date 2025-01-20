using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Philimon.objects.UI.menu
{
    class MenuItems
    {

        public Canvas MenuItemConstructor(double width, double height, string imageSource, string Title)
        {
            Canvas MenuItemCanvas = new Canvas();
            MenuItemCanvas.Width = width;
            MenuItemCanvas.Height = height;
            MenuItemCanvas.Background = Brushes.Gray;

            Image image = new Image();
            image.Width = 64;
            image.Height = height;
            image.Stretch = Stretch.Fill;
            image.Source = new BitmapImage(new Uri(imageSource));
            MenuItemCanvas.Children.Add(image);


            TextBlock TitleText = new TextBlock();
            TitleText.Text = Title;
            TitleText.FontSize = 19;
            TitleText.Foreground = Brushes.White;
            TitleText.Margin = new System.Windows.Thickness(68, height / 2 - 12, 0, 0);
            MenuItemCanvas.Children.Add(TitleText);



            return MenuItemCanvas;

        }



    }
}
