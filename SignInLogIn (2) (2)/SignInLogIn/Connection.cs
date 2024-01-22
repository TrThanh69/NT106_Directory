using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TuDien
{
    class Connection
    {
        private static string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\UIT - Studying\2021-2022\HK2\Lap Trinh Mang Can Ban\Đồ án\[NT106.M21.ATCL]-Group11\SignInLogIn (2) (2)\SignInLogIn\Database1.mdf;Integrated Security=True";
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(stringConnection);
        }
    }
}
