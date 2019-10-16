using System;
using System.Windows;
using System.Windows.Controls;
using GreeManagmentSystem.User.Method;
using GreeManagmentSystem.User.InputInfo;
using GreeManagmentSystem.User.Preview;


namespace GreeManagmentSystem.User.Pages
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class Stock : Page
    {

        public Stock()
        {
            InitializeComponent();
            StockFrame.Content = new StockStorage();
        }
        //入库
        private void StorageBtn_Click(object sender, RoutedEventArgs e)
        {
            
            StockFrame.Content = new StockStorage();
        }
        //查看
        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            
            StockFrame.Content = new StockView(); ;
        }


    }

    //或许有用
    public partial class Stock
    {
/*        Multiply func = new Multiply();

        public Stock()
        {
            InitializeComponent();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            func.MessageBox_self(message, true);
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(baigei, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void Sure_ActionClick(object sender, RoutedEventArgs e)
        {
            func.MessageBox_self(message, false);
        }

        private void Decrease(object sender, RoutedEventArgs e)
        {
            Int32 Num = Convert.ToInt32(valueText.Text);

            valueText.Text = ((Num - 1).ToString());
        }

        private void Increase(object sender, RoutedEventArgs e)
        {
            Int32 Num = Convert.ToInt32(valueText.Text);

            valueText.Text = ((Num + 1).ToString());
        }*/
    }
}
