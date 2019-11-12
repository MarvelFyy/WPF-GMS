using System;
using System.Windows;
using System.Windows.Controls;
using GreeManagmentSystem.User.Method;
using GreeManagementSystem.Dao;
using GreeManagementSystem.Entity;
using GreeManagmentSystem.User.ListWindow;
using System.Windows.Threading;

namespace GreeManagmentSystem.User.InputInfo
{
    /// <summary>
    /// Interaction logic for SaleInput.xaml
    /// </summary>
    public partial class SaleInput : Page
    {
        Multiply func = new Multiply();
        static SalesOrderDao salesOrderDao = new SalesOrderDao();
        //SQL of Type
        string SQL_Fill = "SELECT*FROM Template";

        public SaleInput()
        {
            InitializeComponent();
            //自动填充类型
            func.Fill_Combobox(Category, SQL_Fill, 1);

        }

        private void sure_ActionClick(object sender, RoutedEventArgs e)
        {
            func.MessageBox_self(message, false);
        }

        //数量
        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            Total.Text = func.getTotal(Number.Text, Price.Text);
        }
        //价格
        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            Total.Text = func.getTotal(Number.Text, Price.Text);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Multiply func = new Multiply();
            ListModel listModel = new ListModel();
            SalesOrderDao sales = new SalesOrderDao();
            SalesOrder salesOrder = new SalesOrder()
            {
                //总订单
                TNVal = listModel.timer,
                //分订单
                SNVal = func.LongConvertor(listModel.subNumber),
                //下单日期
                OrderDateVal = Date.Text,
                //客户姓名
                CNameVal=CustomerName.Text,
                //客户电话
                CTelVal=Tel.Text,
                //客户地址
                CAddressVal=Address.Text,
                //类别
                MTClassVal = Category.Text,
                //名称
                MTNameVal = Name.Text,
                //型号
                MTNumberVal = Type.Text,
                //数量
                QuantityVal = func.IntConvertor(Number.Text),
                //售价
                SellingPriceVal=func.DoubleConvertor(Price.Text),
                //总价
                TotalPriceVal = func.DoubleConvertor(Total.Text),
                //销售人员
                SalesmanVal=SalesMan.Text,
                //定金
                EarnestMoneyVal=func.DoubleConvertor(OrderPayment.Text),
                //支付方式
                PaymentMethodVal=Payment.Text,
                //备注
                RemarksVal = Remarks.Text
            };

            sales.AddSales(salesOrder,this);
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

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                func.ClearItems(Name);
                string item = Category.SelectedItem.ToString();
                salesOrderDao.QueryName(item, Name);
            }
            catch (Exception)
            {

            }
        }

        private void Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                func.ClearItems(Type);
                string item = Name.SelectedItem.ToString();
                salesOrderDao.QueryType(item, Type);

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
                salesOrderDao.AutoFillRemarks(item, Remarks);

            }
            catch (Exception)
            {

            }
        }
    }

}
