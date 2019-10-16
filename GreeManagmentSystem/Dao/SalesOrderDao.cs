using System;
using System.Windows;
using System.Collections.Generic;
using System.Data.SQLite;
using GreeManagementSystem.Entity;
using GreeManagementSystem.Dao;
using GreeManagmentSystem.User.Method;
using GreeManagmentSystem.User.InputInfo;
using GreeManagmentSystem.User.Preview;
using System.Windows.Controls;

namespace GreeManagementSystem.Dao
{
    class SalesOrderDao     //客户订单完全操作类
    {
        Multiply func = new Multiply();

        public void AddSales(SalesOrder order,SaleInput saleInput)
        //新建客户订单（新建订单应通过BookDao生成，该函数应用于旧数据导入等功能中）
        {
            object geted = CheckType(order);//机型比对 获取比对结果
            if (geted==null)
            {
                MessageBox.Show("未知型号，请前往库存管理核对", "错误提示", MessageBoxButton.OK);
            }
            else
            {
                try
                {
                    SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                    con.Open();
                    string sql = "INSERT INTO main.SalesOrder " +
                        "(totalNumber,subunitNumber,orderDate,customerName,customerTel," +
                        "customerAddress,machineTypeClass,machineTypeName,machineNumber," +
                        "quantity,sellingPrice,totalPrice,salesman," +
                        "earnestMoney,paymentMethod," +
                        "Remarks,state)VALUES(" +
                        "\'" + order.TNVal + "\'" + "," +
                        "\'" + order.SNVal + "\'" + "," +
                        "\'" + order.OrderDateVal + "\'" + "," +
                        "\'" + order.CNameVal + "\'" + "," +
                        "\'" + order.CTelVal + "\'" + "," +
                        "\'" + order.CAddressVal + "\'" + "," +
                        "\'" + order.MTClassVal + "\'" + "," +
                        "\'" + order.MTNameVal + "\'" + "," +
                        "\'" + order.MTNumberVal + "\'" + "," +
                        "\'" + order.QuantityVal + "\'" + "," +
                        "\'" + order.SellingPriceVal + "\'" + "," +
                        "\'" + order.TotalPriceVal + "\'" + "," +
                        "\'" + order.SalesmanVal + "\'" + "," +
                        "\'" + order.EarnestMoneyVal + "\'" + "," +
                        "\'" + order.PaymentMethodVal + "\'" + "," +
                        "\'" + order.RemarksVal + "\'" + "," +
                        "'未完成'" +
                        ");";

                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    con.Close();
                    func.MessageBox_SaleInput(saleInput, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        //NEW 10-05
        //-------------------------------------------------------------------------------------

        //删除订单 返还库存
        public void RecoverStock(SalesOrder salesOrder)
        {
            if (salesOrder.StateVal!="未完成")
            {
                try
                {
                    SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");

                    con.Open();
                    string sql = "UPDATE main.MachineTypeStock SET "
                                 + "quantity=quantity+'" + salesOrder.QuantityVal + "' "
                                 + "WHERE machineTypeNumber='" + salesOrder.MTNumberVal + "'";

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
        }

        //一键出库 减少库存
        public void ReduceStock(SalesOrder salesOrder,SaleView saleView)
        {
            object check = CheckType(salesOrder);//核对机型 查看有无
            
            if (check == null)
            {
                MessageBox.Show("未知型号或未选择行，请您核对", "错误提示", MessageBoxButton.OK);
            }
            else
            {
                int quantity = GetQuantity(salesOrder);//库存数量

                if (quantity<salesOrder.QuantityVal)//判断订单数量和库存数量
                {
                    MessageBox.Show("该机型库存不足=>库存余量 "+ quantity + " ", "错误提示", MessageBoxButton.OK);
                }
                else if(quantity>=salesOrder.QuantityVal)
                {
                    ReduceStockExcute(salesOrder); 
                    UpdateState(salesOrder); //更改状态为 已出库
                    func.MessageBox_SaleView(saleView, true);
                }
            }

        }

        //减库存执行方法
        public void ReduceStockExcute(SalesOrder salesOrder)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");

                con.Open();
                string sql = "UPDATE main.MachineTypeStock SET "
                             + "quantity=quantity-'" + salesOrder.QuantityVal + "' "
                             + "WHERE machineTypeNumber='" + salesOrder.MTNumberVal + "'";

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
        public object CheckType(SalesOrder salesOrder)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
            con.Open();
            string TypeNumberSQL = "SELECT machineTypeNumber " +
                               "FROM MachineTypeStock " +
                               "WHERE machineTypeNumber='" + salesOrder.MTNumberVal + "'";
            var command = new SQLiteCommand(TypeNumberSQL, con);
            object geted = command.ExecuteScalar();
            command.Dispose();
            con.Close();
            return geted;
        }

        //获取机型数量
        public int GetQuantity(SalesOrder salesOrder)
        {

                int result;
                SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
                con.Open();
                string TypeNumberSQL = "SELECT quantity " +
                                   "FROM MachineTypeStock " +
                                   "WHERE machineTypeNumber='" + salesOrder.MTNumberVal + "'";
                var command = new SQLiteCommand(TypeNumberSQL, con);
                int.TryParse(command.ExecuteScalar().ToString(), out result);
                command.Dispose();
                con.Close();
                return result;

        }

        //更改状态
        public void UpdateState(SalesOrder salesOrder)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");

                con.Open();
                string sql = "UPDATE main.SalesOrder SET "
                             + "state='已出库' "
                             + "WHERE subunitNumber=" + salesOrder.SNVal + ";";

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

        //-----------------------------------------------------------------------------------------
        //New 10-07
        //用于下拉框查询

        //日期查询
        public void QueryDate(string inputDate,DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量'," +
                    "sellingPrice '单价',totalPrice '总价',remarks '备注'," +
                    "salesman '销售人员',earnestMoney '定金',paymentMethod '支付方式'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM SalesOrder " +
                    "WHERE orderDate='" + inputDate + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //状态
        public void QueryState(string inputState, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量'," +
                    "sellingPrice '单价',totalPrice '总价',remarks '备注'," +
                    "salesman '销售人员',earnestMoney '定金',paymentMethod '支付方式'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM SalesOrder " +
                    "WHERE state='" + inputState + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //客户姓名
        public void QueryCustomer(string inputCustomer, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量'," +
                    "sellingPrice '单价',totalPrice '总价',remarks '备注'," +
                    "salesman '销售人员',earnestMoney '定金',paymentMethod '支付方式'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM SalesOrder " +
                    "WHERE customerName='" + inputCustomer + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
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
                    "orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量'," +
                    "sellingPrice '单价',totalPrice '总价',remarks '备注'," +
                    "salesman '销售人员',earnestMoney '定金',paymentMethod '支付方式'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM SalesOrder " +
                    "WHERE machineTypeName='" + inputName + "' " +
                    "ORDER by subunitNumber DESC ";
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
                    "orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量'," +
                    "sellingPrice '单价',totalPrice '总价',remarks '备注'," +
                    "salesman '销售人员',earnestMoney '定金',paymentMethod '支付方式'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM SalesOrder " +
                    "WHERE machineNumber='" + inputType + "' " +
                    "ORDER by subunitNumber DESC ";
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
                                "FROM SalesOrder " +
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
            string TypeNumberSQL = "SELECT machineNumber " +
                                "FROM SalesOrder " +
                                "WHERE machineNumber='" + inputType + "'";
            var command = new SQLiteCommand(TypeNumberSQL, con);
            object geted = command.ExecuteScalar();
            command.Dispose();
            con.Close();
            return geted;
        }

        //-----------------------------------------------------------------------------------------
        //InstallView 查询
        //日期查询
        public void QueryDateIns(string inputDate, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "subunitNumber '订单编号',orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量',installDate '安装日期'," +
                    "installCosts '安装费用',installman '安装师傅',pipeLength '铜管长度',remarks '备注' " +
                    "FROM SalesOrder " +
                    "WHERE orderDate='" + inputDate + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //状态
        public void QueryStateIns(string inputState, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "subunitNumber '订单编号',orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量',installDate '安装日期'," +
                    "installCosts '安装费用',installman '安装师傅',pipeLength '铜管长度',remarks '备注' " +
                    "FROM SalesOrder " +
                    "WHERE state='" + inputState + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //客户姓名
        public void QueryCustomerIns(string inputCustomer, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "subunitNumber '订单编号',orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量',installDate '安装日期'," +
                    "installCosts '安装费用',installman '安装师傅',pipeLength '铜管长度',remarks '备注' " +
                    "FROM SalesOrder " +
                    "WHERE customerName='" + inputCustomer + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //查询名称
        public void QueryNameIns(string inputName, DataGrid dataGrid)
        {
            object check = CheckName(inputName);
            if (check != null)
            {
                QueryNameInsEx(inputName, dataGrid);
            }

        }

        //查询机型
        public void QueryTypeIns(string inputType, DataGrid dataGrid)
        {
            object check = CheckType(inputType);
            if (check != null)
            {
                QueryTypeInsEx(inputType, dataGrid);
            }

        }

        //执行查询名称
        public void QueryNameInsEx(string inputName, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "subunitNumber '订单编号',orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量',installDate '安装日期'," +
                    "installCosts '安装费用',installman '安装师傅',pipeLength '铜管长度',remarks '备注' " +
                    "FROM SalesOrder " +
                    "WHERE machineTypeName='" + inputName + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        //执行查询机型
        public void QueryTypeInsEx(string inputType, DataGrid dataGrid)
        {
            try
            {
                string LoadSQL = "SELECT " +
                    "subunitNumber '订单编号',orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量',installDate '安装日期'," +
                    "installCosts '安装费用',installman '安装师傅',pipeLength '铜管长度',remarks '备注' " +
                    "FROM SalesOrder " +
                    "WHERE machineNumber='" + inputType + "' " +
                    "ORDER by subunitNumber DESC ";
                func.QueryLoad(LoadSQL, dataGrid);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        //NEW 10-13
        //根据类别查询名称
        public void QueryName(string inputCategory, ComboBox comboBox)
        {
            try
            {
                string SQL = "SELECT*FROM MachineTypeStock WHERE machineTypeClass='" + inputCategory + "'";
                func.Fill_Combobox(comboBox, SQL, 0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        //根据名称查询型号
        public void QueryType(string inputName, ComboBox comboBox)
        {
            try
            {
                string SQL = "SELECT*FROM MachineTypeStock WHERE machineTypeName='" + inputName + "'";
                func.Fill_Combobox(comboBox, SQL, 1);
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }
        //自动填充备注
        public void AutoFillRemarks(string inputType, TextBox textBox)
        {
            try
            {
                object geted = GetRemarks(inputType);
                textBox.Text = geted.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }
        //获取机型备注
        public object GetRemarks(string inputType)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=SQL/Gree.db;");
            con.Open();
            string TypeNumberSQL = "SELECT machineTypeRemarks " +
                                "FROM MachineTypeStock " +
                                "WHERE machineTypeNumber='" + inputType + "'";
            var command = new SQLiteCommand(TypeNumberSQL, con);
            object geted = command.ExecuteScalar();
            command.Dispose();
            con.Close();
            return geted;
        }


        //-----------------------------------------------------------------------------------------





        /*
                public void DelSales(int SN)    //删除客户订单
                {
                    BookDao book = new BookDao();
                    book.DelBook(SN);
                }

                public void ChangeSales(SalesOrder order)   //修改客户订单数据
                {
                    string TN = order.TNVal;
                    string name = order.CNameVal;
                    string tel = order.CTelVal;
                    string address = order.CAddressVal;
                    string MTNumber = order.MTNumberVal;
                    int quantity = order.QuantityVal;
                    string salesman = order.SalesmanVal;
                    string orderDate = order.OrderDateVal;
                    double sellingPrice = order.SellingPriceVal;
                    double earnestMoney = order.EarnestMoneyVal;
                    string installDate = order.InstallDateVal;
                    string installman = order.InstallmanVal;
                    double installCosts = order.InstallCostsVal;
                    double pipeLength = order.PipeLengthVal;
                    string paymentMethod = order.PaymentMethodVal;
                    string remarks = order.RemarksVal;
                    string state = order.StateVal;

                    long SN = order.SNVal; //NEW 10-04

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
                                     + "salesman=" + "\'" + salesman + "\'" + ", "
                                     + "orderDate=" + "\'" + orderDate + "\'" + ", "
                                     + "sellingPrice=" + sellingPrice + ", "
                                     + "earnestMoney=" + earnestMoney + ", "
                                     + "InstallDate=" + "\'" + installDate + "\'" + ", "
                                     + "Installman=" + "\'" + installman + "\'" + ", "
                                     + "PipeLength=" + pipeLength + ", "
                                     + "InstallnCosts=" + installCosts + ", "
                                     + "paymentMethod =" + "\'" + paymentMethod + "\'" + ", "
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

                public List<SalesOrder> Query(int p)    //查询客户订单全部数据
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
                                SalesmanVal = sr.GetString(10),
                                OrderDateVal = sr.GetString(11),
                                SellingPriceVal = sr.GetDouble(12),
                                EarnestMoneyVal = sr.GetDouble(13),
                                InstallDateVal = sr.GetString(14),
                                InstallmanVal = sr.GetString(15),
                                InstallCostsVal = sr.GetDouble(16),
                                PipeLengthVal = sr.GetDouble(17),
                                PaymentMethodVal = sr.GetString(18),
                                RemarksVal = sr.GetString(19),
                                //StateVal = sr.GetInt32(20)
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

                public List<SalesOrder> SalesSearch(string keyWord)     //客户订单搜索（2019.09.21）
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
                            "SalesOrder.salesman," +
                            "SalesOrder.orderDate," +
                            "SalesOrder.sellingPrice," +
                            "SalesOrder.earnestMoney," +
                            "SalesOrder.installDate," +
                            "SalesOrder.installman," +
                            "SalesOrder.installCosts," +
                            "SalesOrder.pipeLength," +
                            "SalesOrder.paymentMethod," +
                            "SalesOrder.remarks," +
                            "SalesOrder.state " +
                            "FROM SalesOrder,MachineTypeStock " +
                            "WHERE SalesOrder.machineNumber = MachineTypeStock.machineTypeNumber AND (" +
                            "SalesOrder.totalNumber LIKE '%" + keyWord + "%' " +
                            "OR SalesOrder.subunitNumber LIKE '%" + keyWord + "%'" +
                            "OR SalesOrder.customerName LIKE '%" + keyWord + "%'" +
                            "OR SalesOrder.customerTel LIKE '%" + keyWord + "%'" +
                            "OR SalesOrder.customerAddress LIKE '%" + keyWord + "%'" +
                            "OR MachineTypeStock.machineTypeName LIKE '%" + keyWord + "%'" +
                            "OR MachineTypeStock.machineTypeNumber LIKE '%" + keyWord + "%' " +
                            "OR MachineTypeStock.machineTypeClass LIKE '%" + keyWord + "%'" +
                            "OR MachineTypeStock.machineTypeRemarks LIKE '%" + keyWord + "%'" +
                            "OR SalesOrder.salesman LIKE '%" + keyWord + "%'" +
                            "OR SalesOrder.orderDate LIKE '%" + keyWord + "%'" +
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
                                SalesmanVal = sr.GetString(10),
                                OrderDateVal = sr.GetString(11),
                                SellingPriceVal = sr.GetDouble(12),
                                EarnestMoneyVal = sr.GetDouble(13),
                                InstallDateVal = sr.GetString(14),
                                InstallmanVal = sr.GetString(15),
                                InstallCostsVal = sr.GetDouble(16),
                                PipeLengthVal = sr.GetDouble(17),
                                PaymentMethodVal = sr.GetString(18),
                                RemarksVal = sr.GetString(19),
                                //StateVal = sr.GetInt32(20)
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
                }*/
    }
}
