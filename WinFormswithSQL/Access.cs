
    using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;


    public class Access
    {
        static Access()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 直接运行存储过程文件返回DATATABLE
        /// </summary>
        /// <param name="commtext">存储过程文件名</param>
        /// <returns></returns>
        public static DataTable GETdaleis(string commtext)
        {
            DbCommand comm = DataACCess.CreateComand();
            comm.CommandText = commtext;
            return DataACCess.ExcuteselectCommmand(comm);
        }


        /// <summary>
        /// 修改类别
        /// </summary>
        /// <param name="CategoryID">ID</param>
        /// <param name="NAME">需要修改的名称</param>
        /// <returns></returns>
        public static int edit_ctegory(string Category, string Name)
        {
            DbCommand comm = DataACCess.CreateComand();
            comm.CommandText = "edit";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@Name";
            param.Value = Name;  
            param.Size = 20;
            comm.Parameters.Add(param);

            param = comm.CreateParameter();
            param.ParameterName = "@Category";
            param.Value = Category;
            param.DbType = DbType.String;
            param.Size = 20;
            comm.Parameters.Add(param);

            int result = -1;
            try
            {
                //执行存储过程
                result = DataACCess.ExecuteNonQuery(comm);
            }
            catch { }
            //result = 1即执行成功
            return (result);
        }

    }

