namespace Hexa.Core.Data
{
    using System.Runtime.Serialization;

    #region Enumerations

    public enum DbProvider
    {
        [EnumMember(Value = "MySql.Data.MySQLClient")] MySqlProvider,

        [EnumMember(Value = "System.Data.SQLite")] SQLiteProvider,

        [EnumMember(Value = "System.Data.SqlClient")] MsSqlProvider,

        [EnumMember(Value = "Oracle.DataAccess.Client")] OracleDataProvider,

        [EnumMember(Value = "Npgsql")] PostgreSQLProvider,

        [EnumMember(Value = "System.Data.SqlServerCe.3.5")] SqlCe,

        [EnumMember(Value = "FirebirdSql.Data.FirebirdClient")] Firebird
    }

    #endregion Enumerations
}