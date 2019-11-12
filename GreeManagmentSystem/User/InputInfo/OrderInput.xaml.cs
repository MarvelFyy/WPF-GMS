using System.Windows;
using System;
using System.Windows.Controls;
using GreeManagmentSystem.User.Method;
using GreeManagementSystem.Dao;
using GreeManagementSystem.Entity;
using GreeManagmentSystem.User.ListWindow;
using System.Windows.Threading;

namespace GreeManagmentSystem.User.InputInfo
{
    /// <summary>
    /// Interaction logic for OrderInput.xaml
    /// </summary>
    public partial class OrderInput : Page
    {

        Multiply func = new Multiply();
        static PurchaseDao purchaseDao = new PurchaseDao();

        //SQL of Fill
        string SQL_Fill = "SELECT*FROM Template";

        public OrderInput()
        {
            InitializeComponent();

            //自动填充类别
            func.Fill_Combobox(Category,SQL_Fill,1);
            //自动填充机型名称
            //func.Fill_Combobox(Name, SQL_Name, 2);
            //自动填充机型数据
            //func.Fill_Combobox(Type, SQL_Fill, 3);

        }

        //价格
        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            Total.Text = func.getTotal(Number.Text,Price.Text);
        }
        //数量
        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            Total.Text = func.getTotal(Number.Text, Price.Text);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Multiply func = new Multiply();
            ListModel listModel = new ListModel();
            PurchaseDao purchase = new PurchaseDao();
            PurchaseOrder purchaseOrder = new PurchaseOrder()
            {
            //总订单
            TNVal = listModel.timer,
            //分订单
            SNVal = func.LongConvertor(listModel.subNumber),
            //名称
            MTNameVal = Name.Text,
            //型号
            MTNumberVal = Type.Text,
            //类别
            MTClassVal=Category.Text,
            //下单日期
            OrderDateVal = Date.Text,
            //数量
            QuantityVal = func.IntConvertor(Number.Text),
            //进价
            PurchasePriceVal = func.DoubleConvertor(Price.Text),
            //总价
            TotalPriceVal = func.DoubleConvertor(Total.Text),
            //备注
            RemarksVal = Remarks.Text

            };
            //新建入库订单 
            purchase.AddPrchase(purchaseOrder, this);
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

        //一键清除
        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            CleanContent();
        }

        //确定
        private void sure_ActionClick(object sender, RoutedEventArgs e)
        {
            func.MessageBox_self(message, false);
        }

        //选择类别
        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                func.ClearItems(Name);
                string item = Category.SelectedItem.ToString();             
                purchaseDao.QueryName(item,Name);
            }
            catch (Exception)
            {

            }
        }

        //选择名称
        private void Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                func.ClearItems(Type);
                string item = Name.SelectedItem.ToString();          
                purchaseDao.QueryType(item, Type);

            }
            catch (Exception)
            {

            }
        }

        //选择型号 自动添加备注
        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = Type.SelectedItem.ToString(); 
                purchaseDao.AutoFillRemarks(item, Remarks);
                

            }
            catch (Exception)
            {

            }
        }
    }

    //方法区
    public partial class OrderInput
    {
        //清除内容
        public void CleanContent()
        {
            Date.Text = "";
            Type.Text = "";
            Name.Text = "";
            Category.Text = "";
            Number.Text = "";
            Price.Text = "";
            Total.Text = "";
        }
    }

 
}
