using System.Reflection;

namespace LoginAppServer.backend.resource
{
    public class SqlColumn
    {
        public static readonly SqlColumn LOGIN_EMAIL = new SqlColumn(SqlTable.LOGIN, "email", "VARCHAR(255)");
        public static readonly SqlColumn LOGIN_PASSWORD = new SqlColumn(SqlTable.LOGIN, "password", "VARCHAR(255)");

        private SqlTable table;
        private string columnName;
        private string dataType;

        private SqlColumn(SqlTable table, string columnName, string dataType)
        {
            this.table = table;
            this.columnName = columnName;
            this.dataType = dataType;
        }

        public SqlTable GetTable() => table;
        public string GetColumnName() => columnName;
        public string GetDataType() => dataType;

        public static IEnumerable<SqlColumn> AllColumns()
        {
            foreach (FieldInfo field in typeof(SqlColumn).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(SqlColumn))
                {
                    yield return (SqlColumn)field.GetValue(null);
                }
            }
        }
    }
}
