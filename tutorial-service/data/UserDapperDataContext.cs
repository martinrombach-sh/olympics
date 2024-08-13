using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace OlympicsAPI.Data
{
    public class DataContextDapper
    {
        //Config object defines the connection settings etc
        private readonly IConfiguration _config;

        //Data context = central entity framework / dapper class, contains all methods/classes
        public DataContextDapper(IConfiguration config)
        {
            _config = config;

        }
        public IEnumerable<T> LoadData<T>(string sql)
        {
            //IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TestConnection"));
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            //IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TestConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            //An execute sql call returns the rows affected as a result 
            //IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TestConnection"));
            return dbConnection.Execute(sql);
        }

        public bool ExecuteSql(string sql)
        {
            //This query returns a bool based on the response (200 true, 404 false) which can 
            //be useful if you don't need the rows
            //IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TestConnection"));
            return dbConnection.Execute(sql) > 0;
        }
    }
}