
using System;
using System.Data;
using System.Data.Common;

    public class DataACCess
    {
        static DataACCess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static DbCommand CreateComand()
        {
            string dataProviderName = Conn.DbProviderName;
            string connectionString = Conn.DbConnectionString;
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = connectionString;
            DbCommand comm = conn.CreateCommand();
            comm.CommandType = CommandType.StoredProcedure;
            return comm;
        }

        public static DataTable ExcuteselectCommmand(DbCommand command)
        {
            DataTable table;

            try
            {
                command.Connection.Open();
                DbDataReader reader = command.ExecuteReader();
                table = new DataTable();
                table.Load(reader);
                reader.Close();
            }
          
            finally
            {
                command.Connection.Close();
            }
            return table;

        }

    public static int ExecuteNonQuery(DbCommand command)
    {
        // 受影响行数
        int affectedRows = -1;
        // 执行该命令后确保关闭数据库
        try
        {
            // 打开数据库
            command.Connection.Open();
            // 执行命令并返回行数
            affectedRows = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            // 记录错误并且抛出他们。
            Utilities.LogError(ex);
            throw;
        }
        finally
        {
            // 关闭数据库
            command.Connection.Close();
        }
        // 返回结果
        return affectedRows;
    }
}

