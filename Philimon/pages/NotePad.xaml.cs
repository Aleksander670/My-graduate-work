using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Controls;

namespace Philimon.pages
{
    /// <summary>
    /// Логика взаимодействия для NotePad.xaml
    /// </summary>
    public partial class NotePad : Page
    {
        public Page back_;

        

        public NotePad(Page Back)
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbFontSize.SelectedIndex = 0;

            back_ = Back;
        }


        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (rtbEditor.Selection != null)
            {
                if (rtbEditor.Selection.Start.Parent is InlineUIContainer || rtbEditor.Selection.End.Parent is InlineUIContainer)
                {
                    // Обработка случая, когда выбрана картинка
                    // Здесь можно добавить необходимую логику для работы с картинкой
                }
                else
                {
                    object temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                    btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline) && temp != null);

                    temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
                    btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

                    temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
                    cmbFontFamily.SelectedItem = temp;

                    temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);

                    if (temp != null && temp != DependencyProperty.UnsetValue && temp is double)
                    {
                        double fontSize = (double)temp;
                        // Дополнительная логика для работы с размером шрифта
                    }
                }
            }
        }



        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            double fontSize;
            if (double.TryParse(cmbFontSize.Text, out fontSize))
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Page Previous = new Page();
            Previous = back_;
            this.NavigationService.Navigate(Previous);
        }

        private void Open_Executed(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas Window = new Canvas();
            Window.Width = Application.Current.MainWindow.Width;
            Window.Height = Application.Current.MainWindow.Height;
            Window.Margin = new Thickness(Application.Current.MainWindow.Width / 2 - Window.Width / 2, Application.Current.MainWindow.Height / 2 - Window.Height / 2, 0, 0);

            Canvas Menu = new Canvas();
            Menu.Width = 400;
            Menu.Height = Window.Height;
            Menu.Margin = new Thickness(0,0,0,0);
            Menu.Background = Brushes.AliceBlue;
            Window.Children.Add(Menu);

            Button saveToDiskButton = new Button();
            saveToDiskButton.Width = 300;
            saveToDiskButton.Height = 100;
            saveToDiskButton.Content = "Сохранить на жёсткий диск";
            saveToDiskButton.Click += SaveToDiskButton_Click;

            Button saveToSystemButton = new Button();
            saveToSystemButton.Width = 300;
            saveToSystemButton.Height = 100;
            saveToSystemButton.Content = "Сохранить в систему";

            Button BackSystemButton = new Button();
            BackSystemButton.Width = 300;
            BackSystemButton.Height = 100;
            BackSystemButton.Content = "Закрыть";
            BackSystemButton.Click += (sender1, e1) =>
            {
                Window = null;
            };

            StackPanel buttonPanel = new StackPanel();
            buttonPanel.Children.Add(BackSystemButton);
            buttonPanel.Children.Add(saveToDiskButton);
            buttonPanel.Children.Add(saveToSystemButton);

            Canvas.SetTop(buttonPanel, 400);
            Canvas.SetLeft(buttonPanel, 20);

            Canvas.Children.Add(Window);
            Window.Children.Add(buttonPanel);


           

        }



        private void SaveToDiskButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|PDF (*.pdf)|*.pdf|Word Document (*.docx)|*.docx|All files (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                string fileName = dlg.FileName;
                string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();

                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }
    }
}
