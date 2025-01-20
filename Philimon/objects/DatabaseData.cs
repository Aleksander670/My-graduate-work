using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Philimon.objects
{
    class DatabaseData
    {

        public string ConnectionString { get; set; }

        public DatabaseData()
        {
            // Задаем имя файла
            string iniFileName = "settings.ini";
            // Получаем путь к папке с исполняемым файлом приложения
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Формируем полный путь к INI-файлу
            string iniFilePath = Path.Combine(appDirectory, iniFileName);

            try
            {
                // Считываем все строки из INI-файла
                string[] iniLines = File.ReadAllLines(iniFilePath);
                // Ищем строку, начинающуюся с "ConnectionString:"
                foreach (var line in iniLines)
                {
                    if (line.StartsWith("ConnectionString:"))
                    {
                        // Извлекаем строку подключения, следующую за "ConnectionString:"
                        ConnectionString = line.Substring("ConnectionString:".Length).Trim();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки выводим сообщение
                Console.WriteLine("Ошибка при чтении файла: " + ex.Message);
            }

            //server=localhost;user=root;password=12345;database=PhilimonDB
        }

        public bool Connection()
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        


    }
}
