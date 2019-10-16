using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GreeManagmentSystem.User.Method;
using GreeManagementSystem.Entity;
using GreeManagementSystem.Dao;
using System.Windows.Threading;

namespace GreeManagmentSystem.User.Preview
{
    /// <summary>
    /// Interaction logic for SaleView.xaml
    /// </summary>
    public partial class SaleView : Page
    {
        Multiply func = new Multiply();
        static SalesOrder salesOrder = new SalesOrder();
        //DataGrid数据装载SQL
        static string LoadSQL = "SELECT " +
                    "orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量'," +
                    "sellingPrice '单价',totalPrice '总价',remarks '备注'," +
                    "salesman '销售人员',earnestMoney '定金',paymentMethod '支付方式'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM SalesOrder ORDER by subunitNumber DESC";
        //更新数据库SQL
        static string UpdateSQL = LoadSQL;

        string QuerySQL = "SELECT*FROM SalesOrder";

        string[] arr = { "未完成", "已出库", "已完成" };

        public SaleView()
        {
            InitializeComponent();

            //装载数据库
            func.QueryLoad(LoadSQL, DataGrid_SalesView);
            //根据状态查询
            func.Fill_ComboboxState(State,arr);
            //根据客户姓名查询
            func.Fill_Combobox(CustomerName,QuerySQL,4);
            //根据名称查询
            func.Fill_Combobox(Name, QuerySQL, 8);
            //根据型号查询
            func.Fill_Combobox(Type, QuerySQL,9);
            
        }

        //编辑单元格
        private void DataGrid_SalesView_CurrentCellChanged(object sender, EventArgs e)
        {
            func.UpdateDataBase(UpdateSQL, DataGrid_SalesView);
        }

        //删除
        private void DataGrid_SalesView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //状态
                salesOrder.StateVal = (DataGrid_SalesView.Columns[1].GetCellContent(DataGrid_SalesView.CurrentCell.Item) as TextBlock).Text;
                //型号
                salesOrder.MTNumberVal = (DataGrid_SalesView.Columns[7].GetCellContent(DataGrid_SalesView.CurrentCell.Item) as TextBlock).Text;
                //数量
                salesOrder.QuantityVal = func.IntConvertor((DataGrid_SalesView.Columns[8].GetCellContent(DataGrid_SalesView.CurrentCell.Item) as TextBlock).Text);

                WhetherDelete(e, DataGrid_SalesView);
            }
            catch (Exception)
            {

            }

        }


        //一键出库
        private void Out_Click(object sender, RoutedEventArgs e)
        {
            SalesOrderDao sales = new SalesOrderDao();
            sales.ReduceStock(salesOrder,this);
            func.QueryLoad(LoadSQL, DataGrid_SalesView);
            StartTimer();
        }

        //定时器
        public void StartTimer()
        {
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 3);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        //3秒后自动关闭
        private void Timer_Tick(object sender, EventArgs e)
        {
            message.IsActive = false;
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= Timer_Tick; //取消注册
        }

        private void sure_ActionClick(object sender, RoutedEventArgs e)
        {
            func.MessageBox_self(message, false);
        }

        private void DataGrid_SalesView_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //子订单号
                salesOrder.SNVal = func.LongConvertor((DataGrid_SalesView.Columns[16].GetCellContent(DataGrid_SalesView.CurrentCell.Item) as TextBlock).Text);
                //型号
                salesOrder.MTNumberVal = (DataGrid_SalesView.Columns[7].GetCellContent(DataGrid_SalesView.CurrentCell.Item) as TextBlock).Text;
                //数量
                salesOrder.QuantityVal = func.IntConvertor((DataGrid_SalesView.Columns[8].GetCellContent(DataGrid_SalesView.CurrentCell.Item) as TextBlock).Text);
            }
            catch (Exception)
            {

            }


        }

        //日期
        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Convert.ToDateTime(Date.SelectedDate.ToString()).ToString("yyyy/M/d");
                SalesOrderDao salesOrderDao = new SalesOrderDao();
                salesOrderDao.QueryDate(item, DataGrid_SalesView);
            }
            catch (Exception)
            {

            }
        }

        //状态
        private void State_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = State.SelectedItem.ToString();
                SalesOrderDao salesOrderDao = new SalesOrderDao();
                salesOrderDao.QueryState(item, DataGrid_SalesView);
            }
            catch (Exception)
            {

            }
        }
        //客户姓名
        private void CustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = CustomerName.SelectedItem.ToString();
                SalesOrderDao salesOrderDao = new SalesOrderDao();
                salesOrderDao.QueryCustomer(item, DataGrid_SalesView);
            }
            catch (Exception)
            {

            }
        }
        //名称
        private void Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Name.SelectedItem.ToString();
                SalesOrderDao salesOrderDao = new SalesOrderDao();
                salesOrderDao.QueryName(item, DataGrid_SalesView);
            }
            catch (Exception)
            {

            }
        }
        //型号
        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Type.SelectedItem.ToString();
                SalesOrderDao salesOrderDao = new SalesOrderDao();
                salesOrderDao.QueryType(item, DataGrid_SalesView);
            }
            catch (Exception)
            {

            }
        }

    }

    public partial class SaleView
    {
        //是否删除
        public void WhetherDelete(KeyEventArgs e, DataGrid dataGrid)
        {
            if (e.Key == Key.Delete)
            {

                MessageBoxResult result = MessageBox.Show("确定要删除吗？ ", "提示框", MessageBoxButton.YesNo, MessageBoxImage.Question);

                dataGrid.CanUserDeleteRows = true;

                if (result == MessageBoxResult.No)
                {
                    dataGrid.CanUserDeleteRows = false;
                }
                else
                {
                    SalesOrderDao sales = new SalesOrderDao();
                    sales.RecoverStock(salesOrder);  //返还数量
                }


            }
        }
    }

}
