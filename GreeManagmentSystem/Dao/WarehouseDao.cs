using System;
using System.Windows;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreeManagementSystem.Entity;

namespace GreeManagementSystem.Dao
{
    class WarehouseDao
    {
        public void PutInStorage(PurchaseOrder order, int realQuantity)
        {
            long SN = order.SNVal;
            string MTNumber = order.MTNumberVal;
            int orderQuantity = order.QuantityVal;
            int quantityReceived = order.QuantityReceivedVal;
            int state;

            if (realQuantity + quantityReceived == orderQuantity)
            {
                //判断该订单是否全部到货
                quantityReceived = orderQuantity;
                state = 2;
            }
            else
            {
                quantityReceived = realQuantity + quantityReceived;
                state = 1;
            }

            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");

                con.Open();
                string sql = "UPDATE main.PurchaseOrder SET "
                             + "quantityReceived=" + "\'" + quantityReceived + "\'" + ", "
                             + "state=" + state + " "
                             + "WHERE SubunitNumber=" + SN + ";";

                SQLiteCommand command0 = new SQLiteCommand(sql, con);
                command0.ExecuteNonQuery();
                command0.Dispose();

                MachineTypeStock stock;
                sql = "SELECT quantity " +
                    "FROM main. MachineTypeStock " +
                    "WHERE machineTypeNumber="
                    + "\'" + MTNumber + "\'" + ";";
                SQLiteCommand command1 = new SQLiteCommand(sql, con);
                SQLiteDataReader sr = command1.ExecuteReader();
                if (sr.Read())
                {
                    stock = new MachineTypeStock
                    {
                        MTQuantityVal = sr.GetInt32(0) + realQuantity
                    };

                    sql = "UPDATE main.MachineTypeStock SET "
                        + "quantity=" + stock.MTQuantityVal + " "
                        + "WHERE machineTypeNumber=" + "\'" + MTNumber + "\'" + ";";
                    SQLiteCommand command2 = new SQLiteCommand(sql, con);
                    command2.ExecuteNonQuery();
                    command2.Dispose();

                }
                else
                {
                    sql = "INSERT INTO main.MachineTypeStock "
                             + "(machineTypeName,machineTypeNumber,"
                             + "machineTypeClass,machineTypeRemarks,quantity)"
                             + "VALUES("
                             + "\'" + "未命名的机型" + "\'" + ","
                             + "\'" + MTNumber + "\'" + ","
                             + "\'" + "未知机型分类" + "\'" + ","
                             + "\'" + "无机型备注信息" + "\'" + ","
                             + realQuantity
                             + ");";

                    SQLiteCommand command2 = new SQLiteCommand(sql, con);
                    command2.ExecuteNonQuery();
                    command2.Dispose();

                }
                command1.Dispose();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int OutGoing(SalesOrder order)
        {
            /*
             * 函数解释
             * 生成订单时，不出库
             * 生成安装订单时，出库
             * 
             * 返回值解释
             * 0：成功出库并生成订单
             * 1：查找不到该机型的数据
             * 2:该机型库存不足
             * 4：未知错误
             */

            string MTNumber = order.MTNumberVal;
            int quantity = order.QuantityVal;

            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                con.Open();

                string sql = "SELECT quantity "
                           + "FROM main.MachineTypeStock "
                           + "WHERE machineTypeNumber=" + "\'" + MTNumber + "\'" + ";";

                SQLiteCommand command0 = new SQLiteCommand(sql, con);
                SQLiteDataReader sr = command0.ExecuteReader();
                if (sr.Read())
                {
                    int quantityForGet = sr.GetInt32(0);
                    if (quantity > quantityForGet)
                    {
                        command0.Dispose();
                        con.Close();
                        return 2;
                    }
                    else
                    {

                        sql = "UPDATE main.MachineTypeStock SET "
                            + "Quantity=" + (quantityForGet - quantity) + " "
                            + "WHERE machineTypeNumber=" + "\'" + MTNumber + "\'" + ";";

                        SQLiteCommand command1 = new SQLiteCommand(sql, con);
                        command1.ExecuteNonQuery();
                        command1.Dispose();

                        InstallDao installDao = new InstallDao();
                        //order.StateVal = 2;
                        installDao.ChangeInstall(order);

                        command0.Dispose();
                        con.Close();
                        return 0;
                    }
                }
                else
                {
                    command0.Dispose();
                    con.Close();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 4;
            }
        }
    }
}
