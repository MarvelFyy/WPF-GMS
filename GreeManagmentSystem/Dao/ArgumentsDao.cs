using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GreeManagmentSystem.Entity;
using GreeManagmentSystem.User.InputInfo;
using GreeManagmentSystem.User.Method;

namespace GreeManagmentSystem.Dao
{
    class ArgumentsDao
    {
        Multiply func = new Multiply();
        //名称
        public void AddArguments(string Category,string ProductName,string  ProductType,string ProductRemarks,ArgumentsInput arguments)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                con.Open();
                string sql = "INSERT INTO Template " +
                    "(machineTypeClass,machineTypeName,machineTypeNumber,machineTypeRemarks)" +
                    "VALUES('" + Category + "','" + ProductName + "','" + ProductType + "','" + ProductRemarks + "');";
                SQLiteCommand command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
                command.Dispose();
                con.Close();
                func.MessageBox_ArgumentsInput(arguments, true);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        //系列
        public void AddArgumentsSeries(string Series)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                con.Open();
                string sql = "INSERT INTO main.tb_seriesTemplate " +
                    "(series)VALUES('" + Series + "');";
                SQLiteCommand command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
                command.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //查询名称
        public void QueryName(string inputName, DataGrid dataGrid)
        {
            object check = CheckName(inputName);
            if (check != null)
            {
                QueryNameEx(inputName, dataGrid);
            }

        }

        //查询机型
        public void QueryType(string inputType, DataGrid dataGrid)
        {
            object check = CheckType(inputType);
            if (check != null)
            {
                QueryTypeEx(inputType, dataGrid);
            }

        }

        //执行查询名称
        public void QueryNameEx(string inputName, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                "machineTypeClass '类别',machineTypeName '名称'," +
                "machineTypeNumber '型号'," +
                "machineTypeRemarks '备注' " +
                "FROM Template " +
                "WHERE machineTypeName='" + inputName + "'";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        //执行查询机型
        public void QueryTypeEx(string inputType, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                "machineTypeClass '类别',machineTypeName '名称'," +
                "machineTypeNumber '型号'," +
                "machineTypeRemarks '备注' " +
                "FROM Template " +
                "WHERE machineTypeNumber='" + inputType + "'";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        //名称比对 查看名称有无
        public object CheckName(string inputName)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
            con.Open();
            string TypeNumberSQL = "SELECT machineTypeName " +
                                "FROM Template " +
                                "WHERE machineTypeName='" + inputName + "'";
            var command = new SQLiteCommand(TypeNumberSQL, con);
            object geted = command.ExecuteScalar();
            command.Dispose();
            con.Close();
            return geted;
        }

        //机型比对 查看机型有无
        public object CheckType(string inputType)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
            con.Open();
            string TypeNumberSQL = "SELECT machineTypeNumber " +
                                "FROM Template " +
                                "WHERE machineTypeNumber='" + inputType + "'";
            var command = new SQLiteCommand(TypeNumberSQL, con);
            object geted = command.ExecuteScalar();
            command.Dispose();
            con.Close();
            return geted;
        }
    }
}
