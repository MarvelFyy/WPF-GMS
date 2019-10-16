
using System.Windows;
using System.Windows.Input;
using GreeManagmentSystem.User.Pages;
using System.Windows.Media.Animation;
using GreeManagmentSystem.User.InputInfo;

namespace GreeManagmentSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    //主窗体事件区
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //导航栏状态
        bool StateClosed = true;
        //窗体状态
        bool windowState = true;

        //单击拖动窗体
        private void AcrylicWindow_MouseDown(object sender, MouseButtonEventArgs e)
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

        //导航栏伸展按钮


        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (StateClosed)
            {

                Storyboard storyboard = this.FindResource("CloseMenu") as Storyboard;
                storyboard.Begin();

                Storyboard shrink = this.FindResource("ShrinkBanner") as Storyboard;
                shrink.Begin();

                Storyboard change = this.FindResource("ChangeColor") as Storyboard;
                change.Begin();




            }
            else
            {

                Storyboard storyboard = this.FindResource("OpenMenu") as Storyboard;
                storyboard.Begin();

                Storyboard enlarge = this.FindResource("EnlargeBanner") as Storyboard;
                enlarge.Begin();

                Storyboard restore = this.FindResource("RestoreColor") as Storyboard;
                restore.Begin();
            }

            StateClosed = !StateClosed;
        }

    }

    ////导航栏关联窗体
    public partial class MainWindow : Window
    {

        //仓储总览
        private void Total_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Total();
        }

        //订货中心
        private void Order_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Order();
        }

        //库存管理
        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Stock();
        }

        //销售订单
        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Sale();
        }

        //售后服务
        private void Install_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Install();
        }

    }
 
}
