

    using System.Configuration;


    public class Conn
    {
        private static string dbconnedtionString;//缓存连接字符串
        private static string dbProviderName;//缓存数据提供器名称

        static Conn()
        {
            dbconnedtionString = ConfigurationManager.ConnectionStrings["WinFormswithSQL.Properties.Settings.webapi2ConnectionString"].ConnectionString;
            dbProviderName = ConfigurationManager.ConnectionStrings["WinFormswithSQL.Properties.Settings.webapi2ConnectionString"].ProviderName;
        }

        public static string DbConnectionString
        {
            get
            {
                return dbconnedtionString;
            }
        }

        /// <summary>
        /// 连接程序名和属性
        /// </summary>
        public static string DbProviderName
        {
            get
            {
                return dbProviderName;
            }
        }
    }



