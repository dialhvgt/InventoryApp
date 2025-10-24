using MySql.Data.MySqlClient;

namespace InventoryApp.Infrastructure
{
    public sealed class DbConnectionFactory
    {
        private static readonly Lazy<DbConnectionFactory> _instance =
            new(() => new DbConnectionFactory());

        public static DbConnectionFactory Instance => _instance.Value;

        private readonly string _connString = "Server=127.0.0.1;Port=3306;Database=inventario_db;Uid=root;Pwd=Diego2010-;SslMode=Required;TlsVersion=Tls12,Tls13;";

        private DbConnectionFactory() { }

        public MySqlConnection CreateOpen()
        {
            var conn = new MySqlConnection(_connString);
            conn.Open();
            return conn;
        }
    }
}