using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;




namespace GreeManagmentSystem.User.ListWindow
{
    /// <summary>
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {
        ListModel listModel = new ListModel();
        static MainWindow mainWindow = new MainWindow();

        

        public OrderList()
        {
            InitializeComponent();
            //订单编号
            OrderNumber.Text = listModel.timer;
            //当前日期
            CurrentDate.Text = listModel.currentDate;
            //分订单号
            SubNumber.Text = listModel.subNumber;

            

        }

        //窗体状态
        bool windowState = true;

        //单击拖动窗体
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        //自定义关闭
        private void CloseThis_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //自定义最大化
        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (windowState)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
            windowState = !windowState;

        }
        //自定义最小化
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(OrderListWindow, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }


    }

}
