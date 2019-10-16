using GreeManagmentSystem.User.Method;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace GreeManagmentSystem.Dao
{
    class TotalDao
    {
        Multiply func = new Multiply();

        //查询名称
        public void QueryName(string inputName,DataGrid dataGrid)
        {
            object check= CheckName(inputName);
            if (check!=null)
            {
                QueryNameEx(inputName, dataGrid);   
            }

        }

        //查询机型
        public void QueryType(string inputType,DataGrid dataGrid)
        {
            object check = CheckType(inputType);
            if (check!=null)
            {
                QueryTypeEx(inputType, dataGrid);
            }

        }

        //执行查询名称
        public void QueryNameEx(string inputName,DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                "machineTypeClass '类别',machineTypeName '名称'," +
                "machineTypeNumber '型号',quantity '数量'," +
                "machineTypeRemarks '备注' " +
                "FROM MachineTypeStock " +
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
                "machineTypeNumber '型号',quantity '数量'," +
                "machineTypeRemarks '备注' " +
                "FROM MachineTypeStock " +
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
                                "FROM MachineTypeStock " +
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
                                "FROM MachineTypeStock " +
                                "WHERE machineTypeNumber='" + inputType + "'";
            var command = new SQLiteCommand(TypeNumberSQL, con);
            object geted = command.ExecuteScalar();
            command.Dispose();
            con.Close();
            return geted;
        }
    }
}
