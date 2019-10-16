using System;
using System.Windows;
using System.Windows.Controls;
using GreeManagmentSystem.User.InputInfo;
using GreeManagmentSystem.User.Preview;


namespace GreeManagmentSystem.User.Pages
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public  partial  class Order : Page
    {
        //Instance of OrderInput
        OrderInput orderInput = new OrderInput();
        ArgumentsInput arguments = new ArgumentsInput();

        public Order()
        {
            InitializeComponent();
            OrderFrame.Content = orderInput;
            
        }

        //填写
        private void InputBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderFrame.Content = new OrderInput();
            //this.OrderFrame.Source = new Uri("/User/InputInfo/OrderInput.xaml", UriKind.RelativeOrAbsolute);
        }

        //查看
        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderFrame.Content = new OrderView();
            //this.OrderFrame.Source = new Uri("/User/Preview/OrderView.xaml", UriKind.RelativeOrAbsolute);
        }

        //参数
        private void ArgumentBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderFrame.Content = arguments;
            //this.OrderFrame.Source = new Uri("/User/InputInfo/ArgumentsInput.xaml", UriKind.RelativeOrAbsolute);
        }

        //模板
        private void TemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderFrame.Content = new TemplateView();
            //this.OrderFrame.Source = new Uri("/User/Preview/TemplateView.xaml", UriKind.RelativeOrAbsolute);
        }

    }
}
