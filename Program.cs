using System.Text;
using LoginAppServer.backend;
using LoginAppServer.backend.query;
using LoginAppServer.backend.resource;
using LoginAppServer.util;
using MySql.Data.MySqlClient;

namespace LoginAppServer
{

    internal class Program
    {
        static void Menu()
        {
            MySqlConnection? connection = null;
            int option = -1;

            while (option != 3)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine(" 1. Criar banco de dados.");
                Console.WriteLine(" 2. Criar tabelas e colunas.");
                Console.WriteLine(" 3. Sair\n");

                Console.WriteLine("Insira a sua opção:");
                int.TryParse(Console.ReadLine(), out option);

                switch (option)
                {
                    case 1:
                        connection = Connection.Init();
                        break;

                    case 2:
                        if (connection != null)
                        {
                            CreateAllTables(connection);
                        } 
                        else
                        {
                            Util.OutgoingMessage("Você precisa iniciar o banco de dados antes de executar qualquer pesquisa.");

                            Console.WriteLine("Deseja iniciar o banco de dados para criar as tabelas? (s/n):");

                            if (Console.ReadLine().ToLower().Equals("s"))
                            {
                                connection = Connection.Init();
                                CreateAllTables(connection);
                            }

                        }
                        break;
                }
            }
        }

        static void CreateAllTables(MySqlConnection connection)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Criando tabelas...");

            foreach (SqlTable table in Enum.GetValues(typeof(SqlTable)))
            {
                if (!SqlQuery.TableExists(connection, "application", table.ToString().ToLower()))
                {
                    if (SqlQuery.CreateTableIfNotExists(connection, "application", table.ToString().ToLower()))
                    {
                        builder.AppendLine($"Tabela '{table.ToString().ToLower()}' criada com sucesso juntamente com as colunas.");
                    }
                    else
                    {
                        builder.AppendLine($"Não foi possível criar a tabela '{table.ToString().ToLower()}'.");
                    }
                }
                else
                {
                    builder.AppendLine($"Tabela '{table.ToString().ToLower()}' ignorada porque já existe.");
                }
            }

            builder.AppendLine("Tarefa finalizada.");

            Util.OutgoingMessage(builder.ToString());
        }

        static void Main() => Menu();
    }
}
