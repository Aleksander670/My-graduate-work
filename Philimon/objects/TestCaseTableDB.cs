using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Philimon.objects
{
    class TestCaseTableDB
    {
        DatabaseData DB = new DatabaseData();
        public void CreateTable(int id, string Name, string PreCode, int Priority, string Labels, string Components, string Maker, string Project)
        {

            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                string NameTable = Name + ":" + PreCode + "-" + id.ToString();

                string createUsersTableQuery = $"CREATE TABLE {NameTable} (id INT, name VARCHAR(60), priority INT, labels VARCHAR(100), components VARCHAR(100), maker VARCHAR(60), Project VARCHAR(60))";
                MySqlCommand createUsersTableCommand = new MySqlCommand(createUsersTableQuery, connection);
                createUsersTableCommand.ExecuteNonQuery();

                string query = $"INSERT INTO {NameTable} VALUES ({id},{Name},{Priority},{Labels},{Components},{Maker},{Project})";

                MySqlCommand AddData = new MySqlCommand(query, connection);
                AddData.ExecuteNonQuery();

            }
        

            
        }




    }
}
