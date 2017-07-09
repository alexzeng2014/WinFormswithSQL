---
title: WinForms 使用Entity Framework 数据绑定
tags: Entity Framework,WinForms
grammar_cjkRuby: true
---
作为一名全栈设计师，每天要面临的挑战真不少。在这个项目里面，我必须为windows服务器做一个定时批量修改MSSQL数据库里面的数据的应用，那么势必在VS 2015中新建一个Windows Forms项目，汇编成EXE在服务器中运行。

那么，下面的例子就是通过VS 2015，使用Entity Framework做数据绑定的一个实例。

**新建项目**

在VS 2015中新建一个项目，左窗格中选择Windows，右窗格中选择Windows 窗体应用程序，取名为：WinFormswithEF

![WinFormswithEF][1]

**安装Entity Framework NuGet软件包**

在解决方案资源管理器中，右键单击WinFormswithEF项目，选择管理NuGet软件包...

在“管理NuGet软件包”对话框中，选择“EntityFramework”软件包。单击安装。

![选择“EntityFramework”软件包][2]


**连接数据库**

点击视图菜单，找到服务器资源管理器，右键单击数据连接，添加连接：

![添加连接][3]

可以点击左下方的测试连接对数据库进行测试，如果显示连接成功就表示成功了。

**逆向工程师模型**

我们将使用Entity Framework Designer作为Visual Studio的一部分来创建我们的模型。
点击项目菜单 - >添加新项，选择ADO.NET 实体数据模型，取名为ProductModel

![添加新项目][4]

选择下一步：

![来自数据库的EF设计器][5]


选择从来自数据库的EF设计器，然后单击下一步

![选择数据连接][6]

选择数据连接。

![选择数据库对象和设置][7]
选择完毕之后，点击完成就可以了。


逆向工程一旦完成，新模型将被添加到您的项目中，并打开供您在实体框架设计器中查看。 App.config文件也已添加到您的项目与数据库的连接详细信息。

EF使用模板从您的模型生成代码，我们可以从代码中看到数据实体类TblProductList.cs

```c#
 using System;
    using System.Collections.Generic;
    
    public partial class TblProductList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public Nullable<decimal> price { get; set; }
    }
```

我们还可以看到，数据上下文类是被放在ProductModel.edmx里面的。

```c#
  using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class webapi2Entities : DbContext
    {
        public webapi2Entities()
            : base("name=webapi2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TblProductList> TblProductList { get; set; }
    }
```

因为我这个程序是需要批量修改的，在EF使用LINQ做UPDATE大家都会知道很难实现，那么只能再安装NuGet软件包：

> Install-Package EntityFramework.Extended

**完成代码**

打开Form1设计窗口，将工具箱的Label1,timer1移到窗口里面，在后台进行代码编写，如下

```c#
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EntityFramework.Extensions;

namespace WinFormswithEF
{
    public partial class Form1 : Form
    {
        private int counter;
        public Form1()
        {
            InitializeComponent();
            counter = 0;
            timer1.Interval = 6000;
            timer1.Enabled = true;
            
        }

       
        private void timer1_Tick(object sender, EventArgs e)
        {
            var db = new webapi2Entities();
            
             if (counter >= 10)
            {
                // Exit loop code.
                timer1.Enabled = false;
                counter = 0;
            }
            else
            {
                db.TblProductList.Where(s => s.Name == "dddsa").Update(s => new TblProductList()
                {
                    Category = "oaa"
                });
                counter = counter + 1;
                label1.Text = "运行次数： " + counter.ToString();
            }
        }

       
    }
}

```
运行程序，测试通过。

**注意**

这个例程里面，是对已经存在的数据库进行逆向处理，让VS对已经存在的数据库进行生成代码。因为我的网站已经存在，我的数据库已经存在。我只是需要一个程序，常驻在服务器后台，使用计时器对数据库进行批量操作。

对桌面应用程序来说，使用EF并不是一个多好的主意。如果使用存储过程来批量处理，反而是个更有效率的工作。

下面是操作的步骤。

**新建项目二**

在VS 2015中新建一个项目，左窗格中选择Windows，右窗格中选择Windows 窗体应用程序，取名为：WinFormswithSQL

打开文件APP.CONFIG，修改一下，按照数据库的连接进行修改：

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
    </configSections>
    <connectionStrings>
        <add name="WinFormswithSQL.Properties.Settings.webapi2ConnectionString"
            connectionString="Data Source=.;Initial Catalog=webapi2;Persist Security Info=True;User ID=sa;Password=password"
            providerName="System.Data.SqlClient" />
    </connectionStrings>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
</configuration>
```
在引用`using System.Configuration;`之前，点击解决资源管理器，右击引用，添加引用，将System.Configuration 引用进来。

新建一类，取名为conn.cs:

```c#
namespace WinFormswithSQL
{
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
}
```
然后再新建类：DataACCess.cs

```c#

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


```

现在基本的手脚架已经设置完成了,我们对数据库的表新建一个取名为edit的存储过程（存储过程是由一些SQL语句和控制语句组成的被封装起来的过程，它驻留在数据库中，可以被客户应用程序调用，也可以从另一个过程或触发器调用。它的参数可以被传递和返回。与应用程序中的函数过程类似，存储过程可以通过名字来调用，而且它们同样有输入参数和输出参数。）：

```sql
CREATE PROCEDURE [dbo].[edit]
	(@Category nvarchar(20),
	@Name nvarchar(20))
		AS
			BEGIN
				UPDATE TblProductList SET Category=@Category WHERE Name=@Name
			END
```
最后，新建一个连接的类Access.cs就可以了

```c#

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

```

打开Form1设计窗口，将工具箱的Label1,timer1移到窗口里面，在后台进行代码编写，如下

```c#
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormswithSQL
{
    public partial class Form1 : Form
    {
        private int counter;
        public Form1()
        {
            InitializeComponent();
            counter = 0;
            timer1.Interval = 6000;
            timer1.Enabled = true;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (counter >= 10)
            {
                // Exit loop code.
                timer1.Enabled = false;
                counter = 0;
            }
            else
            {
                if (Access.edit_ctegory("ccoo1", "dddsa"))
                {
                    if (Access.edit_ctegory("ccoo1", "dddsa")) {
                    counter = counter + 1;
                    label1.Text = "运行次数： " + counter.ToString();
                    }
                }
            }
        }
    }
}

```
运行结果：

![运行程序][8]

**源代码**

WinFormswithSQL 的源代码我放到了Github上面：https://github.com/alexzeng2014/WinFormswithSQL


[1]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499606872484.jpg
[2]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499606930359.jpg
[3]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499606971169.jpg
[4]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499607003958.jpg
[5]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499607059273.jpg
[6]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499607122154.jpg
[7]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499607236653.jpg
[8]: http://7xvup9.com1.z0.glb.clouddn.com/xsj/1499607259136.jpg