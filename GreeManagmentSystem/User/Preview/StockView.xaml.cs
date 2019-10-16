using System;
using System.Windows.Controls;
using GreeManagmentSystem.Dao;
using GreeManagmentSystem.User.Method;


namespace GreeManagmentSystem.User.Preview
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : Page
    {
        static Multiply func = new Multiply();

        ///DataGrid数据装载
        static string LoadSQL = "SELECT " +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineTypeNumber '型号',quantity '数量'," +
                    "machineTypeRemarks '备注' " +
                    "FROM MachineTypeStock " +
                    "GROUP by machineTypeClass,machineTypeName "+
                    "ORDER by quantity DESC";
        //DataGrid数据装载SQL
        static string UpdateSQL = LoadSQL;

        string QuerySQL = "SELECT*FROM MachineTypeStock";

        public StockView()
        {
            InitializeComponent();

            //根据名称查询
            func.Fill_Combobox(Name, QuerySQL, 0);
            //根据型号查询
            func.Fill_Combobox(Type, QuerySQL, 1);

            func.QueryLoad(LoadSQL, DataGrid_StockView);
        }



        private void Edit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            func.DataGridEdit(DataGrid_StockView, Edit);
        }

        private void DataGrid_StockView_CurrentCellChanged(object sender, EventArgs e)
        {
            func.UpdateDataBase(UpdateSQL, DataGrid_StockView);
        }

        private void DataGrid_StockView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            func.WhetherDelete(e, DataGrid_StockView);
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Type.SelectedItem.ToString();
                TotalDao totalDao = new TotalDao();
                totalDao.QueryType(item, DataGrid_StockView);
            }
            catch (Exception)
            {

            }

        }

        private void Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Name.SelectedItem.ToString();
                TotalDao totalDao = new TotalDao();
                totalDao.QueryName(item, DataGrid_StockView);
            }
            catch (Exception)
            {

            }

        }
    }

}
