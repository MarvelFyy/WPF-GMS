using System;
using System.Windows;
using System.Windows.Controls;
using GreeManagementSystem.Entity;
using GreeManagmentSystem.User.Method;
using GreeManagementSystem.Dao;
using System.Windows.Threading;

namespace GreeManagmentSystem.User.InputInfo
{
    /// <summary>
    /// Interaction logic for StockStorage.xaml
    /// </summary>
    public partial class StockStorage : Page
    {
        static Multiply func = new Multiply();
        static MachineTypeStock machineTypeStock = new MachineTypeStock();
        
        static PurchaseOrder purchaseOrder = new PurchaseOrder();
        

        //DataGrid数据装载SQL
        static string LoadSQL= "SELECT " +
                    "orderDate '订货日期',state '状态',machineTypeClass '类别'," +
                    "machineTypeName '名称',machineTypeNumber '型号'," +
                    "quantity '数量',quantityReceived '到货',already '已入库'," +
                    "purchasePrice '进价',totalPrice '总价',remarks '备注'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM PurchaseOrder ORDER by totalNumber DESC";
        //更新数据库SQL
        static string UpdateSQL = LoadSQL;

        string QuerySQL = "SELECT*FROM PurchaseOrder";

        string[] arr = { "待入库", "已入库" };

        public StockStorage()
        {
            InitializeComponent();
            func.QueryLoad(LoadSQL, DataGrid_StockStorage);

            //根据状态查询
            func.Fill_ComboboxState(State, arr);
            //根据名称查询
            func.Fill_Combobox(Name, QuerySQL, 3);
            //根据型号查询
            func.Fill_Combobox(Type, QuerySQL, 4);
        }

        private void DataGrid_StockStorage_CurrentCellChanged(object sender, EventArgs e)
        {
            func.UpdateDataBase(UpdateSQL, DataGrid_StockStorage);
        }
        
        //获取DataGrid的值
        private void DataGrid_StockStorage_GotFocus(object sender, RoutedEventArgs e)
        {
            //类别
            machineTypeStock.MTClassVal = (DataGrid_StockStorage.Columns[2].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text;
            //名称
            machineTypeStock.MTNameVal = (DataGrid_StockStorage.Columns[3].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text;
            //型号
            machineTypeStock.MTNumberVal = (DataGrid_StockStorage.Columns[4].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text;
            //数量
            machineTypeStock.MTQuantityVal = func.IntConvertor((DataGrid_StockStorage.Columns[5].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text);
            //备注
            machineTypeStock.MTRemarksVal = (DataGrid_StockStorage.Columns[10].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text;
            //分订单号
            purchaseOrder.SNVal= func.LongConvertor((DataGrid_StockStorage.Columns[12].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text);
            //到货
            purchaseOrder.QuantityReceivedVal= func.IntConvertor((DataGrid_StockStorage.Columns[6].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text);
            //purchase型号
            purchaseOrder.MTNumberVal= (DataGrid_StockStorage.Columns[4].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text;
            //purchase quantity
            purchaseOrder.QuantityVal = func.IntConvertor((DataGrid_StockStorage.Columns[5].GetCellContent(DataGrid_StockStorage.CurrentCell.Item) as TextBlock).Text);
        }
        //载入库存
        private void Storage_Click(object sender, RoutedEventArgs e)
        {
            StockDao stockDao = new StockDao();
            stockDao.CheckFlag(purchaseOrder, machineTypeStock,this);
            func.QueryLoad(LoadSQL, DataGrid_StockStorage);
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
            func.MessageBox_self(message,false);
        }

        //日期
        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Convert.ToDateTime(Date.SelectedDate.ToString()).ToString("yyyy/M/d");
                PurchaseDao purchaseDao = new PurchaseDao();
                purchaseDao.QueryDate(item, DataGrid_StockStorage);
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
                PurchaseDao purchaseDao = new PurchaseDao();
                purchaseDao.QueryState(item, DataGrid_StockStorage);
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
                PurchaseDao purchaseDao = new PurchaseDao();
                purchaseDao.QueryName(item, DataGrid_StockStorage);
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
                PurchaseDao purchaseDao = new PurchaseDao();
                purchaseDao.QueryType(item, DataGrid_StockStorage);
            }
            catch (Exception)
            {

            }
        }


    }
}
