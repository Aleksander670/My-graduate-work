using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Philimon.objects
{
    class SoundPlay
    {

        MediaPlayer player = new MediaPlayer();

        public void SoundWelcome()
        {
            // Получаем текущую директорию, где находится исполняемый файл
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string iniFilePath = System.IO.Path.Combine(currentDirectory, "settings.ini");

            try
            {
                if (File.Exists(iniFilePath))
                {
                    string[] lines = File.ReadAllLines(iniFilePath);

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("startupSound_effect="))
                        {
                            string soundName = line.Substring("startupSound_effect=".Length);
                            string filePath = System.IO.Path.Combine(currentDirectory, "data/sounds/" + soundName + ".wav"); //сделать больше форматов

                            player.Open(new Uri(filePath));
                            player.Volume = 0.50;
                            player.Play();

                            return; // Выходим из цикла после проигрывания первого звука
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Файл settings.ini не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }

            

        }
        

    }
}
