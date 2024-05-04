using LoginAppServer.backend.query;
using LoginAppServer.util;
using MySql.Data.MySqlClient;

namespace LoginAppServer.backend
{
    internal class Connection
    {
        public static MySqlConnection Init(string databaseName, string user, string password)
        {
            Util.OutgoingMessage("Iniciando conexão...");

            MySqlConnection connection = new MySqlConnection($"Server=127.0.0.1;Uid={user};Pwd={password};");

            try
            {
                connection.Open();

                if (SqlQuery.DatabaseExists(connection, databaseName))
                {
                    Util.OutgoingMessage($"Conexão com o banco de dados '{databaseName}' estabelecida com sucesso!");
                }
                else
                {
                    SqlQuery.CreateDBIfNotExists(connection, databaseName);
                    Util.OutgoingMessage($"O banco de dados '{databaseName}' foi criado, porque não existia.", "Conexão estabelecida com sucesso!");
                }

            }
            catch (Exception ex)
            {
                Util.OutgoingMessage("Erro: " + ex.Message);
            }

            return connection;
        }

        public static MySqlConnection Init() => Init("application", "root", "");

    }
}
