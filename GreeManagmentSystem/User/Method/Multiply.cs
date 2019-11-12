using System;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Input;
using GreeManagementSystem.Entity;
using GreeManagementSystem.Dao;
using GreeManagmentSystem.User.Preview;
using GreeManagmentSystem.User.InputInfo;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;

namespace GreeManagmentSystem.User.Method
{
    public partial class Multiply
    {

        //Combobox数据绑定方法 不要更改
        public void Fill_combobox(ComboBox comboBox,string SQL)
        {
            //创建连接对象
            SQLiteConnection sqliteconn = new SQLiteConnection(@"Data Source=SQL\GREE.db");

            SQLiteCommand cmd;
            try
            {

                sqliteconn.Open();
                cmd = new SQLiteCommand(SQL, sqliteconn);
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //机型
                    string type = dr.GetString(1);
                    comboBox.Items.Add(type);

                }
                //关闭资源
                sqliteconn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //Combobox数据去重绑定方法
        public void Fill_Combobox(ComboBox comboBox, string SQL,int num)
        {
            //创建连接对象
            SQLiteConnection sqliteconn = new SQLiteConnection(@"Data Source=SQL\GREE.db");

            SQLiteCommand cmd;
            try
            {

                sqliteconn.Open();
                cmd = new SQLiteCommand(SQL, sqliteconn);
                SQLiteDataReader dr = cmd.ExecuteReader();
                List<string> list = new List<string>();

                while (dr.Read())
                {
                    list.Add(dr.GetString(num));
                }
                list.Distinct().ToList().ForEach(s => comboBox.Items.Add(s));
                //关闭资源
                sqliteconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ClearItems(ComboBox comboBox)
        {
            try
            {
                comboBox.Items.Clear();
            }
            catch (Exception)
            {

            }
        }


        //状态定制方法
        public void Fill_ComboboxState(ComboBox comboBox,string[] arr)
        {
            try
            {
 
                foreach (var item in arr)
                {
                    comboBox.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //获取时间作为总订单编号
        public string getNumber()
        {
            string str= DateTime.Now.ToString();
            string result = null;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = str;
                }
            }
            return result + "0000";
        }

        //获取当前时间
        public string getCurrentDate()
        {
            return DateTime.Now.ToString("yyyy/MM/dd");
        }

        //获取分订单号
        public string getSubNumber()
        {
            string str = DateTime.Now.ToString();
            string result = null;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = str;
                }
            }

            return result + "1111";
        }

        //总价计算方法
        public string getTotal(string numbers, string prices)
        {
            double.TryParse(numbers, out double price);
            double.TryParse(prices, out double number);
            return (price * number).ToString();
        }

        //long转换器
        public long LongConvertor(string StringType)
        {
                long result;
                long.TryParse(StringType, out result);
                return result;
        }

        //int转换器
        public int IntConvertor(string StringType)
        {
            int result;
            int.TryParse(StringType, out result);
            return result;
        }
        //double转换器
        public double DoubleConvertor(string StringType)
        {
            double result;
            double.TryParse(StringType, out result);
            return result;
        }

        //展示机型名称
        public string getName(string Category,string Series,string Work,string Level,string Cold,string Color)
        {
            return Category + Series + Work + Level + Cold + "("+Color+")";
        }

        //自定义MessageBox
        public void MessageBox_self(MaterialDesignThemes.Wpf.Snackbar message, bool state)
        {
            message.IsActive = state;

        }
        //InstallView 消息框
        public void MessageBox_InstallView(InstallView install, bool state)
        {
            install.message.IsActive = state;
        }


        //OrderInput 消息框
        public void MessageBox_OrderInput(OrderInput order,bool state)
        {
            order.message.IsActive = state;
        }
        //ArgumentsInput 消息框
        public void MessageBox_ArgumentsInput(ArgumentsInput arguments, bool state)
        {
            arguments.message.IsActive = state;
        }
        //StockStorage
        public void MessageBox_StockStorage(StockStorage stock, bool state)
        {
            stock.message.IsActive = state;
        }
        //SaleInput
        public void MessageBox_SaleInput(SaleInput sale, bool state)
        {
            sale.message.IsActive = state;
        }
        //SaleInput
        public void MessageBox_SaleView(SaleView saleView, bool state)
        {
            saleView.message.IsActive = state;
        }


        //启用编辑
        bool EditState = true;//编辑按钮状态
        public void DataGridEdit(DataGrid dataGrid,Button Edit)
        {
          
            if (EditState)
            {
                dataGrid.IsReadOnly = false;
                Edit.Content = "关闭编辑";
            }
            else
            {
                dataGrid.IsReadOnly = true;
                Edit.Content = "启用编辑";
            }
            EditState = !EditState;
        }

    }

    //数据库双向绑定方法区
    public partial class Multiply
    {
        //DataGrid数据装载
        public void QueryLoad(string sql,DataGrid gridName)     //查询入库订单
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                con.Open();
                var cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridName.ItemsSource = dt.DefaultView;
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //更新数据库
        public void UpdateDataBase(string sql,DataGrid gridName)
        {
            try
            {
                DataTable dt = (gridName.ItemsSource as DataView).Table;
                SQLiteConnection con = new SQLiteConnection(@"Data Source=SQL\GREE.db");
                var cmd = new SQLiteCommand(sql, con);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);
                cb.RefreshSchema();
                da.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       //询问删除
       public void WhetherDelete(KeyEventArgs e,DataGrid dataGrid)
        {
            if (e.Key == Key.Delete)
            {

                MessageBoxResult result = MessageBox.Show("确定要删除吗？ ", "提示框", MessageBoxButton.YesNo, MessageBoxImage.Question);

                dataGrid.CanUserDeleteRows = true;

                if (result == MessageBoxResult.No)
                {
                    dataGrid.CanUserDeleteRows = false;
                }
               
            }
        }

    }

}
