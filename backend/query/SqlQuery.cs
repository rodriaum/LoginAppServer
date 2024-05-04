using LoginAppServer.backend.resource;
using LoginAppServer.util;
using MySql.Data.MySqlClient;
using System.Text;

namespace LoginAppServer.backend.query
{
    internal class SqlQuery
    {

        public static bool DatabaseExists(MySqlConnection connection, string databaseName)
        {
            string query = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @DatabaseName";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DatabaseName", databaseName);

                object? result = null;

                try
                {
                    result = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Util.OutgoingMessage($"Não foi possível verificar se o banco de dados '{databaseName}' existe.", "Erro: " + ex.Message);
                }

                return result != null && result.ToString() == databaseName;
            }
        }

        public static bool TableExists(MySqlConnection connection, string databaseName, string tableName)
        {
            string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @DatabaseName AND TABLE_NAME = @TableName";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DatabaseName", databaseName);
                command.Parameters.AddWithValue("@TableName", tableName);

                object? result = null;

                try
                {
                    result = command.ExecuteScalar();
                } 
                catch (Exception ex)
                {
                    Util.OutgoingMessage($"Não foi possível verificar se a tabela '{tableName}' existe.", "Erro: " + ex.Message);
                }

                return result != null && result.ToString() == tableName;
            }
        }

        public static bool CreateDBIfNotExists(MySqlConnection connection, string databaseName)
        {
            string query = $"CREATE DATABASE IF NOT EXISTS `{databaseName}`";

            return BasicQuery(connection, query);
        }

        public static bool CreateTableIfNotExists(MySqlConnection connection, string databaseName, string tableName)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine($"CREATE TABLE IF NOT EXISTS `{databaseName}`.`{tableName}` (");
            query.AppendLine("`id` INT AUTO_INCREMENT PRIMARY KEY,");

            List<SqlColumn> columns = SqlColumn.AllColumns().ToList();

            foreach (SqlColumn column in columns)
            {
                query.Append($"`{column.GetColumnName()}` {column.GetDataType()} NOT NULL");

                if (column != columns.Last())
                {
                    query.AppendLine(",");
                }
            }

            query.AppendLine(");");

            return BasicQuery(connection, query.ToString());
        }

        static bool BasicQuery(MySqlConnection connection, string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Util.OutgoingMessage(ex.Message);
                    return false;
                }
            }
        }
    }
}
