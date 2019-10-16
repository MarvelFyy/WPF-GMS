using System;
using System.Windows.Controls;
using System.Windows.Input;
using GreeManagmentSystem.Dao;
using GreeManagmentSystem.User.Method;

namespace GreeManagmentSystem.User.Preview
{
    /// <summary>
    /// Interaction logic for TemplateView.xaml
    /// </summary>
    public partial class TemplateView : Page
    {
        Multiply func = new Multiply();
        ArgumentsDao argumentsDao = new ArgumentsDao();
        static string ArgumentsLoad = "SELECT ID,machineTypeClass '类别',machineTypeName '名称',machineTypeNumber '型号',machineTypeRemarks '备注' " +
                                      "FROM Template";
        static string ArgumentsUpdate = ArgumentsLoad;

        string QuerySQL = "SELECT*FROM Template";

        public TemplateView()
        {
            InitializeComponent();
            //根据名称查询
            func.Fill_Combobox(Name, QuerySQL, 2);
            //根据型号查询
            func.Fill_Combobox(Type, QuerySQL, 3);

            func.QueryLoad(ArgumentsLoad, DataGrid_ArgumentsView);


        }

        //编辑
        private void DataGrid_ArgumentsView_CurrentCellChanged(object sender, EventArgs e)
        {
            func.UpdateDataBase(ArgumentsUpdate, DataGrid_ArgumentsView);
        }

        //启用编辑
        private void Edit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataGridEdit();
        }

        //删除
        private void DataGrid_ArgumentsView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            func.WhetherDelete(e, DataGrid_ArgumentsView);
        }

        private void Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Name.SelectedItem.ToString(); 
                argumentsDao.QueryName(item, DataGrid_ArgumentsView);
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
                argumentsDao.QueryType(item, DataGrid_ArgumentsView);
            }
            catch (Exception)
            {

            }
        }
    }

    //方法区
    public partial class TemplateView
    {
        bool EditState = true;//编辑按钮状态
        public void DataGridEdit()
        {

            if (EditState)
            {
                DataGrid_ArgumentsView.IsReadOnly = false;
                Edit.Content = "关闭编辑";
            }
            else
            {
                DataGrid_ArgumentsView.IsReadOnly = true;
                Edit.Content = "启用编辑";
            }
            EditState = !EditState;
        }
    }
}
