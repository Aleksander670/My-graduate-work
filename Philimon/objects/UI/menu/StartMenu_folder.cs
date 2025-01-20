using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Philimon.objects.UI.menu
{
    class StartMenu_folder : Button
    {

        public Canvas DauterCanvas { get; set; }

#pragma warning disable CS0067 // Событие "StartMenu_folder.MouseDown" никогда не используется.
#pragma warning disable CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово
        public event EventHandler MouseDown;
#pragma warning restore CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово
#pragma warning restore CS0067 // Событие "StartMenu_folder.MouseDown" никогда не используется.

        public Canvas folder(double width, double height, string Title, Canvas MainCanvas)
        {
            Canvas FolderCanvas = new Canvas();
            FolderCanvas.Width = width;
            FolderCanvas.Height = height;
            FolderCanvas.Background = Brushes.WhiteSmoke;

            Image ico = new Image();
            ico.Width = 64;
            ico.Height = 64;
            ico.Source = new BitmapImage(new Uri("pack://application:,,,/sources/images/folder_ico.jpg"));
            FolderCanvas.Children.Add(ico);

            TextBlock TitleBlock = new TextBlock();
            TitleBlock.Text = Title;
            TitleBlock.FontSize = 18;
            TitleBlock.Margin = new System.Windows.Thickness(68, FolderCanvas.Height/2 - 10, 0, 0);
            FolderCanvas.Children.Add(TitleBlock);

            TextBlock Label = new TextBlock();
            Label.Text = "→";
            Label.FontSize = 18;
            Label.Margin = new System.Windows.Thickness(FolderCanvas.Width - Label.FontSize - 2, FolderCanvas.Height / 2 - 10, 0, 0);
            FolderCanvas.Children.Add(Label);

            FolderCanvas.MouseDown += (sender, e) =>
            {
                if (DauterCanvas != null && !MainCanvas.Children.Contains(DauterCanvas))
                {
                    DauterCanvas.Margin = new Thickness(MainCanvas.Width / 2 - DauterCanvas.Width, MainCanvas.Height / 2, 0, 0);
                    MainCanvas.Children.Add(DauterCanvas);
                }
            };

            return FolderCanvas;
        }



    }
}
