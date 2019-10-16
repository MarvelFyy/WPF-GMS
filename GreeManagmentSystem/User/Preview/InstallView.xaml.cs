using GreeManagementSystem.Dao;
using GreeManagementSystem.Entity;
using GreeManagmentSystem.User.Method;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GreeManagmentSystem.User.Preview
{
    /// <summary>
    /// Interaction logic for InstallView.xaml
    /// </summary>
    public partial class InstallView : Page
    {
        Multiply func = new Multiply();
        
        static SalesOrder salesOrder = new SalesOrder();

        static string LoadSQL = "SELECT " +
                    "subunitNumber '订单编号',orderDate '订单日期',state '状态',customerName '客户姓名'," +
                    "customerTel '客户电话',customerAddress '客户地址'," +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineNumber '型号',quantity '数量',installDate '安装日期'," +
                    "installCosts '安装费用',installman '安装师傅',pipeLength '铜管长度',remarks '备注' " +
                    "FROM SalesOrder ORDER by subunitNumber DESC";

        //更新数据库SQL
        static string UpdateSQL = LoadSQL;

        string QuerySQL = "SELECT*FROM SalesOrder";

        string[] arr = { "未完成", "已出库", "已完成" };

        public InstallView()
        {
            InitializeComponent();
            //装载数据库
            func.QueryLoad(LoadSQL, DataGrid_InstallView);
            //根据状态查询
            func.Fill_ComboboxState(State,arr);
            //根据客户姓名查询
            func.Fill_Combobox(CustomerName, QuerySQL, 4);
            //根据名称查询
            func.Fill_Combobox(Name, QuerySQL, 8);
            //根据型号查询
            func.Fill_Combobox(Type, QuerySQL, 9);
        }

        private void DataGrid_InstallView_CurrentCellChanged(object sender, EventArgs e)
        {
            func.UpdateDataBase(UpdateSQL, DataGrid_InstallView);
        }

        private void DataGrid_InstallView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //状态
                salesOrder.StateVal = (DataGrid_InstallView.Columns[2].GetCellContent(DataGrid_InstallView.CurrentCell.Item) as TextBlock).Text;
                //型号
                salesOrder.MTNumberVal = (DataGrid_InstallView.Columns[8].GetCellContent(DataGrid_InstallView.CurrentCell.Item) as TextBlock).Text;
                //数量
                salesOrder.QuantityVal = func.IntConvertor((DataGrid_InstallView.Columns[9].GetCellContent(DataGrid_InstallView.CurrentCell.Item) as TextBlock).Text);

                WhetherDelete(e, DataGrid_InstallView);
            }
            catch (Exception)
            {

            }
        }

        private void DataGrid_InstallView_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                salesOrder.SNVal = func.LongConvertor((DataGrid_InstallView.Columns[0].GetCellContent(DataGrid_InstallView.CurrentCell.Item) as TextBlock).Text);
                salesOrder.StateVal = (DataGrid_InstallView.Columns[2].GetCellContent(DataGrid_InstallView.CurrentCell.Item) as TextBlock).Text;
            }
            catch (Exception)
            {

            }

        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            InstallDao installDao = new InstallDao();
            installDao.FinishOrder(salesOrder,this, true);
            func.QueryLoad(LoadSQL, DataGrid_InstallView);
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

        //日期
        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Convert.ToDateTime(Date.SelectedDate.ToString()).ToString("yyyy/M/d");
                SalesOrderDao salesOrderDao = new SalesOrderDao();
                salesOrderDao.QueryDateIns(item, DataGrid_InstallView);
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
                string item = State.SelectedItem.ToString() as string;
                SalesOrderDao salesOrderDao = new SalesOrderDao();
                salesOrderDao.QueryStateIns(item, DataGrid_InstallView);
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
                salesOrderDao.QueryCustomerIns(item, DataGrid_InstallView);
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
                salesOrderDao.QueryNameIns(item, DataGrid_InstallView);
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
                salesOrderDao.QueryTypeIns(item, DataGrid_InstallView);
            }
            catch (Exception)
            {

            }
        }

    }

    public partial class InstallView
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
