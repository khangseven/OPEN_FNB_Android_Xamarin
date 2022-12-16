using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using MySqlConnector;

namespace OPEN_FNB_Android.Client
{
    public class MySQLConnection
    {

        public static MySQLConnection conn { get; set; }

        public static MySqlConnection getMySQLConnection(string server, string port, string database, string uid, string pwd)
        {
            string connString = "server="+server+";port="+port+";database="+database+";uid="+uid+"; pwd="+pwd+ ";";

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;



        }
    }
}
