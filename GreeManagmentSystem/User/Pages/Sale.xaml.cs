using System.Windows;
using System.Windows.Controls;
using GreeManagmentSystem.User.InputInfo;
using GreeManagmentSystem.User.Preview;

namespace GreeManagmentSystem.User.Pages
{
    /// <summary>
    /// Interaction logic for Sale.xaml
    /// </summary>
    public partial class Sale : Page
    {
        public Sale()
        {
            InitializeComponent();
            SaleFrame.Content = new SaleInput();
        }

        private void InputBtn_Click(object sender, RoutedEventArgs e)
        {
            SaleFrame.Content = new SaleInput();
        }

        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            SaleFrame.Content = new SaleView();
        }
    }
}
