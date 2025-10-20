using MySql.Data.MySqlClient;

namespace Project1
{
    public class Connect
    {
        public MySqlConnection connection;
        private string _host;
        private string _db;
        private string _user;
        private string _password;
        private string connectionString;

        public Connect()
        {
            _host = "localhost";
            _db = "auto";
            _user = "root";
            _password = "";

            connectionString = $"SERVER={_host};DATABASE={_db};UID={_user};PASSWORD={_password};SslMode=None";

            connection = new MySqlConnection(connectionString);
        }
    }
}
