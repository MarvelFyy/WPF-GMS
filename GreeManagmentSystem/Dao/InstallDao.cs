using System;
using System.Windows;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreeManagementSystem.Entity;
using GreeManagmentSystem.User.Method;
using GreeManagmentSystem.User.Preview;

namespace GreeManagementSystem.Dao
{
    class InstallDao
    {
        Multiply func = new Multiply();

        //NEW 10-06
        //完成订单 修改状态
        public void FinishOrder(SalesOrder salesOrder,InstallView installView, bool state)
        {
            if (salesOrder.StateVal=="已出库")
            {
                try
                {
                    SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");

                    con.Open();
                    string sql = "UPDATE main.SalesOrder SET "
                                 + "state='已完成' "
                                 + "WHERE subunitNumber=" + salesOrder.SNVal + ";";

                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    con.Close();
                    func.MessageBox_InstallView(installView, state);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("该订单未出库或未选择行，请您核对", "错误提示", MessageBoxButton.OK);
            }

            
        }



        public void ChangeInstall(SalesOrder order)
        //修改安装订单信息 （新建安装订单，从预订订单中获取数据）
        {
            string TN = order.TNVal;
            string name = order.CNameVal;
            string tel = order.CTelVal;
            string address = order.CAddressVal;
            string MTNumber = order.MTNumberVal;
            int quantity = order.QuantityVal;
            string installDate = order.InstallDateVal;
            string installman = order.InstallmanVal;
            double installCosts = order.InstallCostsVal;
            double pipeLength = order.PipeLengthVal;
            string remarks = order.RemarksVal;

            string state = order.StateVal;
            long SN = order.SNVal;

            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");

                con.Open();
                string sql = "UPDATE main.SalesOrder SET "
                             + "totalNumber=" + "\'" + TN + "\'" + ", "
                             + "customerName=" + "\'" + name + "\'" + ", "
                             + "customerTel=" + "\'" + tel + "\'" + ", "
                             + "customerAddress=" + "\'" + address + "\'" + ", "
                             + "machineNumber=" + "\'" + MTNumber + "\'" + ", "
                             + "quantity=" + quantity + ", "
                             + "InstallDate=" + "\'" + installDate + "\'" + ", "
                             + "Installman=" + "\'" + installman + "\'" + ", "
                             + "PipeLength=" + pipeLength + ", "
                             + "InstallnCosts=" + installCosts + ", "
                             + "Remarks=" + "\'" + remarks + "\'" + ", "
                             + "State=" + state + " "
                             + "WHERE SubunitNumber=" + SN + ";";

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

        public List<SalesOrder> Query(int p)//查询安装订单数据
        {
            List<SalesOrder> orderList = new List<SalesOrder>();
            SalesOrder newOrder;
            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                con.Open();

                string sql = "SELECT SalesOrder.totalNumber,SalesOrder.subunitNumber,SalesOrder.customerName," +
                    "SalesOrder.customerTel,SalesOrder.customerAddress,MachineTypeStock.machineTypeName," +
                    "MachineTypeStock.machineTypeNumber,MachineTypeStock.machineTypeClass," +
                    "MachineTypeStock.machineTypeRemarks,SalesOrder.quantity,SalesOrder.installDate," +
                    "SalesOrder.installman,SalesOrder.installCosts,SalesOrder.pipeLength," +
                    "SalesOrder.remarks,SalesOrder.remarks,SalesOrder.state FROM SalesOrder,MachineTypeStock WHERE " +
                    "SalesOrder.machineNumber = MachineTypeStock.machineTypeNumber " +
                    "LIMIT " + p + ", 1000;";

                SQLiteCommand command = new SQLiteCommand(sql, con);
                SQLiteDataReader sr = command.ExecuteReader();
                while (sr.Read())
                {
                    newOrder = new SalesOrder
                    {
                        TNVal = sr.GetString(0),
                        SNVal = sr.GetInt32(1),
                        CNameVal = sr.GetString(2),
                        CTelVal = sr.GetString(3),
                        CAddressVal = sr.GetString(4),
                        MTNameVal = sr.GetString(5),
                        MTNumberVal = sr.GetString(6),
                        MTClassVal = sr.GetString(7),
                        MTRemarksVal = sr.GetString(8),
                        QuantityVal = sr.GetInt32(9),
                        InstallDateVal = sr.GetString(10),
                        InstallmanVal = sr.GetString(11),
                        InstallCostsVal = sr.GetDouble(12),
                        PipeLengthVal = sr.GetDouble(13),
                        RemarksVal = sr.GetString(14),
                        //StateVal = sr.GetInt32(15)
                    };

                    orderList.Add(newOrder);
                }
                command.Dispose();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return orderList;
        }

        public List<SalesOrder> InstallSearch(string keyWord) //关键词搜索客户安装订单信息（2019.09.22）
        {
            List<SalesOrder> orderList = new List<SalesOrder>();
            SalesOrder newOrder;
            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                con.Open();

                string sql = "SELECT " +
                    "SalesOrder.totalNumber," +
                    "SalesOrder.subunitNumber," +
                    "SalesOrder.customerName," +
                    "SalesOrder.customerTel," +
                    "SalesOrder.customerAddress," +
                    "MachineTypeStock.machineTypeName," +
                    "MachineTypeStock.machineTypeNumber," +
                    "MachineTypeStock.machineTypeClass," +
                    "MachineTypeStock.machineTypeRemarks," +
                    "SalesOrder.quantity," +
                    "SalesOrder.installDate," +
                    "SalesOrder.installman," +
                    "SalesOrder.installCosts," +
                    "SalesOrder.pipeLength," +
                    "SalesOrder.remarks," +
                    "SalesOrder.state " +
                    "FROM SalesOrder,MachineTypeStock WHERE " +
                    "SalesOrder.machineNumber = MachineTypeStock.machineTypeNumber AND (" +
                    "SalesOrder.totalNumber LIKE '%" + keyWord + "%' " +
                    "OR SalesOrder.subunitNumber LIKE '%" + keyWord + "%'" +
                    "OR SalesOrder.customerName LIKE '%" + keyWord + "%'" +
                    "OR SalesOrder.customerTel LIKE '%" + keyWord + "%'" +
                    "OR SalesOrder.customerAddress LIKE '%" + keyWord + "%'" +
                    "OR MachineTypeStock.machineTypeName LIKE '%" + keyWord + "%'" +
                    "OR MachineTypeStock.machineTypeNumber LIKE '%" + keyWord + "%' " +
                    "OR MachineTypeStock.machineTypeClass LIKE '%" + keyWord + "%'" +
                    "OR MachineTypeStock.machineTypeRemarks LIKE '%" + keyWord + "%'" +
                    "OR SalesOrder.installDate LIKE '%" + keyWord + "%'" +
                    "OR SalesOrder.installman LIKE '%" + keyWord + "%'" +
                    "OR SalesOrder.remarks LIKE '%" + keyWord + "%'" +
                    "OR SalesOrder.state LIKE '%" + keyWord + "%'" +
                    "); ";

                SQLiteCommand command = new SQLiteCommand(sql, con);
                SQLiteDataReader sr = command.ExecuteReader();
                while (sr.Read())
                {
                    newOrder = new SalesOrder
                    {
                        TNVal = sr.GetString(0),
                        SNVal = sr.GetInt32(1),
                        CNameVal = sr.GetString(2),
                        CTelVal = sr.GetString(3),
                        CAddressVal = sr.GetString(4),
                        MTNameVal = sr.GetString(5),
                        MTNumberVal = sr.GetString(6),
                        MTClassVal = sr.GetString(7),
                        MTRemarksVal = sr.GetString(8),
                        QuantityVal = sr.GetInt32(9),
                        InstallDateVal = sr.GetString(10),
                        InstallmanVal = sr.GetString(11),
                        InstallCostsVal = sr.GetDouble(12),
                        PipeLengthVal = sr.GetDouble(13),
                        RemarksVal = sr.GetString(14),
                        //StateVal = sr.GetInt32(15)
                    };

                    orderList.Add(newOrder);
                }
                command.Dispose();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return orderList;
        }


    }
}
