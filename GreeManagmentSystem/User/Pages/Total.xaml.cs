using GreeManagmentSystem.Dao;
using GreeManagmentSystem.User.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreeManagmentSystem.User.Pages
{
    /// <summary>
    /// Interaction logic for Total.xaml
    /// </summary>
    public partial class Total : Page
    {
        Multiply func = new Multiply();


        ///DataGrid数据装载
        static string LoadSQL = "SELECT " +
                    "machineTypeClass '类别',machineTypeName '名称'," +
                    "machineTypeNumber '型号',quantity '数量'," +
                    "machineTypeRemarks '备注' " +
                    "FROM MachineTypeStock " +
                    "GROUP by machineTypeClass,machineTypeName " +
                    "ORDER by quantity DESC";

        string QuerySQL = "SELECT*FROM MachineTypeStock";

        public Total()
        {
            InitializeComponent();

            //根据名称查询
            func.Fill_Combobox(Name, QuerySQL, 0);
            //根据型号查询
            func.Fill_Combobox(Type, QuerySQL, 1);

            func.QueryLoad(LoadSQL,DataGrid_TotalView);

        }

        private void Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Name.SelectedItem.ToString();
                TotalDao totalDao = new TotalDao();
                totalDao.QueryName(item, DataGrid_TotalView);
            }
            catch (Exception)
            {

            }    

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Type.SelectedItem.ToString();
                TotalDao totalDao = new TotalDao();
                totalDao.QueryType(item, DataGrid_TotalView);
            }
            catch (Exception)
            {

            }

        }
    }
}
