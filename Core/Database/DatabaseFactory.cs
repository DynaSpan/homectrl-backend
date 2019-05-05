using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using HomeCTRL.Backend.Core.Database.Handlers;
using MySql.Data.MySqlClient;

namespace HomeCTRL.Backend.Core.Database
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly string connectionString;

        public DatabaseFactory(DatabaseSettings dbSettings)
        {
            this.connectionString = this.ConstructConnectionString(dbSettings);

            SqlMapper.AddTypeHandler(typeof(Dictionary<int, object>), new JsonObjectTypeHandler());
	        SqlMapper.AddTypeHandler(typeof(string[]),                new JsonObjectTypeHandler());
        }

        /// <summary>
        /// Gets a DB connection
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDatabase()
        {
            return new MySqlConnection(this.connectionString);
        }

        /// <summary>
        /// Constructs the connection string based on the
        /// DatabaseSettings
        /// </summary>
        /// <param name="dbSettings"></param>
        /// <returns></returns>
        private string ConstructConnectionString(DatabaseSettings dbSettings)
        {
            StringBuilder connStringBuilder = new StringBuilder();

            connStringBuilder.AppendFormat("Server={0};", dbSettings.Host, dbSettings.Port);

            if (dbSettings.Port != 3306)
                connStringBuilder.AppendFormat("Port={0};", dbSettings.Port);
            
            connStringBuilder.AppendFormat("Database={0};", dbSettings.Database);
            connStringBuilder.AppendFormat("User Id={0};", dbSettings.User);
            connStringBuilder.AppendFormat("Password={0};", dbSettings.Password);

            return connStringBuilder.ToString();
        }
    }
}