using System.Windows.Controls;
using System.Data.SQLite;
using System.Data;
using System;
using System.Windows;
using System.Windows.Input;
using GreeManagmentSystem.User.Method;
using GreeManagementSystem.Dao;
using GreeManagementSystem.Entity;

namespace GreeManagmentSystem.User.Preview
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : Page
    {

        Multiply func = new Multiply();

        static PurchaseOrder purchaseOrder=new PurchaseOrder();

        //DataGrid数据装载SQL
        static string LoadSQL= "SELECT " +
                    "orderDate '订单日期',state '状态',machineTypeClass '类别'," +
                    "machineTypeName '名称',machineTypeNumber '型号'," +
                    "quantity '数量',quantityReceived '到货',already '已入库'," +
                    "purchasePrice '进价',totalPrice '总价',remarks '备注'," +
                    "totalNumber '总订单编号',subunitNumber '分订单编号' " +
                    "FROM PurchaseOrder ORDER by subunitNumber DESC";
        //更新数据库SQL
        static string UpdateSQL = LoadSQL;

        string QuerySQL = "SELECT*FROM PurchaseOrder";

        string[] arr = { "待入库", "已入库" };

        public OrderView()
        {
            InitializeComponent();

            //装载数据库
            func.QueryLoad(LoadSQL, DataGrid_OrderView);
            //根据状态查询
            func.Fill_ComboboxState(State, arr);
            //根据名称查询
            func.Fill_Combobox(Name, QuerySQL, 3);
            //根据型号查询
            func.Fill_Combobox(Type, QuerySQL, 4);
        }

        //编辑单元格
        private void DataGrid_OrderView_CurrentCellChanged(object sender, EventArgs e)
        {
            func.UpdateDataBase(UpdateSQL, DataGrid_OrderView);
        }

        //删除
        private void DataGrid_OrderView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //型号
                purchaseOrder.MTNumberVal= (DataGrid_OrderView.Columns[4].GetCellContent(DataGrid_OrderView.CurrentCell.Item) as TextBlock).Text;
                //已入库数量
                purchaseOrder.QuantityVal= func.IntConvertor((DataGrid_OrderView.Columns[7].GetCellContent(DataGrid_OrderView.CurrentCell.Item) as TextBlock).Text);
                //子订单号
                purchaseOrder.SNVal= func.LongConvertor((DataGrid_OrderView.Columns[12].GetCellContent(DataGrid_OrderView.CurrentCell.Item) as TextBlock).Text);

                WhetherDelete(e, DataGrid_OrderView);
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
                PurchaseDao purchaseDao = new PurchaseDao();
                purchaseDao.QueryDate(item, DataGrid_OrderView);
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
                purchaseDao.QueryState(item, DataGrid_OrderView);
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
                purchaseDao.QueryName(item, DataGrid_OrderView);
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
                PurchaseDao purchaseDao= new PurchaseDao();
                purchaseDao.QueryType(item, DataGrid_OrderView);
            }
            catch (Exception)
            {

            }
        }


    }

    //删除方法
    public partial class OrderView 
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
                    PurchaseDao purchaseDao = new PurchaseDao();
                    purchaseDao.RecoverStock(purchaseOrder);  //返还数量
                }


            }
        }
    }

}
