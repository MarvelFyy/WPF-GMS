using System;
using System.Windows;
using System.Collections.Generic;
using GreeManagementSystem.Entity;
using System.Data.SQLite;
using GreeManagmentSystem.User.InputInfo;
using GreeManagmentSystem.User.Method;

namespace GreeManagementSystem.Dao
{
    class StockDao
    {
        Multiply func = new Multiply();

        public void AddStock(MachineTypeStock stock,PurchaseOrder purchaseOrder)
        {


            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
                con.Open();
                string sql = "INSERT INTO main.MachineTypeStock " +
                    "(machineTypeName,machineTypeNumber," +
                    "machineTypeClass,machineTypeRemarks,quantity)" +
                    "VALUES(" +
                    "\'" + stock.MTNameVal + "\'" + "," +
                    "\'" + stock.MTNumberVal + "\'" + "," +
                    "\'" + stock.MTClassVal + "\'" + "," +
                    "\'" + stock.MTRemarksVal + "\'" + "," +
                    "\'" + purchaseOrder.QuantityReceivedVal + "\');";

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

        //NEW 2019 10-03 ------------------------------------------------------------------
        //判断标志位
        public void CheckFlag(PurchaseOrder purchaseOrder, MachineTypeStock machineTypeStock,StockStorage stockStorage)
        {
            try
            {

                int flag = GetFlag(purchaseOrder);
                int already = GetAlready(purchaseOrder);
                object check = GetTypeNumber(purchaseOrder);

                if (check == null)//机型库不存在此机型
                {
                    if (flag == 0)
                    {
                        AddStock(machineTypeStock, purchaseOrder);
                        flag++;
                        UpdateFlag(flag, purchaseOrder);
                        UpdateAlready(purchaseOrder);

                        //订货数量和到货数量相等 
                        if (purchaseOrder.QuantityReceivedVal == purchaseOrder.QuantityVal)
                        {
                            UpdateState(purchaseOrder);//更改状态为"已入库"
                        }

                        func.MessageBox_StockStorage(stockStorage, true);
                    
                    }
                    else if (flag > 0)
                    {

                        //订货数量和到货数量不等
                        if (purchaseOrder.QuantityReceivedVal != purchaseOrder.QuantityVal)
                        {
                            int result = purchaseOrder.QuantityReceivedVal - already;//到货数量减去已入库数量
                            UpdateStock(result, purchaseOrder);
                            UpdateAlready(result, purchaseOrder);
                            func.MessageBox_StockStorage(stockStorage, true);
                        }
                        else if (purchaseOrder.QuantityReceivedVal == purchaseOrder.QuantityVal) //订货数量和到货数量相等 
                        {
                            int result = purchaseOrder.QuantityReceivedVal - already;//到货数量减去已入库数量
                            UpdateStock(result, purchaseOrder);
                            UpdateAlready(result, purchaseOrder);
                            UpdateState(purchaseOrder);//更改状态为"已入库"
                            func.MessageBox_StockStorage(stockStorage, true);
                        }
                        flag++;
                        UpdateFlag(flag, purchaseOrder);//更新flag

                    }
                }
                else
                {
                    if (flag == 0)
                    {
                        UpdateStock(purchaseOrder.QuantityReceivedVal, purchaseOrder);//更改数量
                        flag++;
                        UpdateFlag(flag, purchaseOrder);
                        UpdateAlready(purchaseOrder);

                        //订货数量和到货数量相等 
                        if (purchaseOrder.QuantityReceivedVal == purchaseOrder.QuantityVal)
                        {
                            UpdateState(purchaseOrder);//更改状态为"已入库"
                        }
                        func.MessageBox_StockStorage(stockStorage, true);

                    }
                    else if (flag > 0)
                    {

                        //订货数量和到货数量不等
                        if (purchaseOrder.QuantityReceivedVal != purchaseOrder.QuantityVal)
                        {
                            int result = purchaseOrder.QuantityReceivedVal - already;//到货数量减去已入库数量
                            UpdateStock(result, purchaseOrder);
                            UpdateAlready(result, purchaseOrder);
                            func.MessageBox_StockStorage(stockStorage, true);
                        }
                        else if (purchaseOrder.QuantityReceivedVal == purchaseOrder.QuantityVal) //订货数量和到货数量相等 
                        {
                            int result = purchaseOrder.QuantityReceivedVal - already;//到货数量减去已入库数量
                            UpdateStock(result, purchaseOrder);
                            UpdateAlready(result, purchaseOrder);
                            UpdateState(purchaseOrder);//更改状态为"已入库"
                            func.MessageBox_StockStorage(stockStorage, true);
                        }
                        flag++;
                        UpdateFlag(flag, purchaseOrder);//更新flag

                    }
                }

            }
            catch (Exception)
            {

                MessageBox.Show("未知型号或未选择行，请您核对", "错误提示", MessageBoxButton.OK);
            }


        }

        //查询flag
        public int GetFlag(PurchaseOrder purchaseOrder)
        {

             int result;
             SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
             con.Open();
             string FlagSQL = "SELECT flag " +
                              "FROM PurchaseOrder " +
                              "WHERE subunitNumber='" + purchaseOrder.SNVal + "'";
             var command = new SQLiteCommand(FlagSQL, con);
             int.TryParse(command.ExecuteScalar().ToString(), out result);
             command.Dispose();
             con.Close();
             return result;

        }

        //查询already
        public int GetAlready(PurchaseOrder purchaseOrder)
        {
            int result;
            SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
            con.Open();
            string AlreadySQL= "SELECT already " +
                               "FROM PurchaseOrder " +
                               "WHERE subunitNumber='" + purchaseOrder.SNVal + "'";
            var command = new SQLiteCommand(AlreadySQL, con);
            int.TryParse(command.ExecuteScalar().ToString(),out result);
            command.Dispose();
            con.Close();
            return result;
        }

        //更新flag
        public void UpdateFlag(int flag, PurchaseOrder purchaseOrder)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");

                con.Open();
                string sql = "UPDATE main.PurchaseOrder SET " +
                    "flag='" + flag + "' "+
                    "WHERE subunitNumber='" + purchaseOrder.SNVal + "';";

                SQLiteCommand command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
                command.Dispose();
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //更新already 第一次
        public void UpdateAlready(PurchaseOrder purchaseOrder)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");

                con.Open();
                string sql = "UPDATE main.PurchaseOrder SET " +
                    "already='" + purchaseOrder.QuantityReceivedVal + "' " +
                    "WHERE subunitNumber='" + purchaseOrder.SNVal + "';";

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

        //更新already 第一次之后
        public void UpdateAlready(int result,PurchaseOrder purchaseOrder)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");

                con.Open();
                string sql = "UPDATE main.PurchaseOrder SET " +
                    "already=already+'" + result + "' " +
                    "WHERE subunitNumber='" + purchaseOrder.SNVal + "';";

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

        //更改库存数量
        public void UpdateStock(int result,PurchaseOrder purchaseOrder)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");

                con.Open();
                string sql = "UPDATE main.MachineTypeStock SET " +
                    "quantity=quantity+'" + result + "' " +
                    "WHERE machineTypeNumber='" + purchaseOrder.MTNumberVal + "';";

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

        //更改状态为已入库
        public void UpdateState(PurchaseOrder purchaseOrder)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");

                con.Open();
                string sql = "UPDATE main.PurchaseOrder SET " +
                    "state='已入库' " +
                    "WHERE subunitNumber=" + purchaseOrder.SNVal + ";";

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

        //机型比对 查看机型有无
        public object GetTypeNumber(PurchaseOrder purchaseOrder)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
            con.Open();
            string TypeNumberSQL = "SELECT machineTypeNumber " +
                               "FROM MachineTypeStock " +
                               "WHERE machineTypeNumber='" + purchaseOrder.MTNumberVal + "'";
            var command = new SQLiteCommand(TypeNumberSQL, con);
            object geted= command.ExecuteScalar();
            command.Dispose();
            con.Close();
            return geted;
        }

        //NEW 2019 10-03 ------------------------------------------------------------------


        public void DelStock(String Number) //删除库存
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");

                con.Open();
                string sql = "DELETE FROM main.MachineTypeStock " +
                    "WHERE machineTypeNumber=" + Number + ";";
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

        //修改库存
        public void ChangeStock(MachineTypeStock stock)
        {
            string name = stock.MTNameVal;
            string mtClass = stock.MTClassVal;
            string remarks = stock.MTRemarksVal;
            double quantity = stock.MTQuantityVal;

            string number = stock.MTNumberVal;

            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");

                con.Open();
                string sql = "UPDATE main.MachineTypeStock SET " +
                    "machineTypeName=" + name + ", " +
                    "machineTypeClass=" + mtClass + ", " +
                    "machineTypeRemarks=" + remarks + ", " +
                    "quantity=" + quantity + " " +
                    "WHERE machineTypeNumber=" + number + ";";

                SQLiteCommand command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
                command.Dispose();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           /* List<MachineTypeStock> query(int p)
            {
                List<MachineTypeStock> stockList = new List<MachineTypeStock>();
                MachineTypeStock newStock = null;
                try
                {
                    SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
                    con.Open();

                    string sql = "SELECT * FROM main.MachineTypeStock " +
                        "LIMIT " + p + ", 1000;";

                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    SQLiteDataReader sr = command.ExecuteReader();
                    while (sr.Read())
                    {
                        newStock = new MachineTypeStock();
                        newStock.MTNameVal = sr.GetString(0);
                        newStock.MTNumberVal = sr.GetString(1);
                        newStock.MTClassVal = sr.GetString(2);
                        newStock.MTRemarksVal = sr.GetString(3);
                        newStock.MTQuantityVal = sr.GetInt32(4);

                        stockList.Add(newStock);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return stockList;
            }*/

        }

    }
}