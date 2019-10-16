using GreeManagmentSystem.User.Method;
using System.Windows.Controls;
using GreeManagmentSystem.Dao;
using System.Windows;
using System.Windows.Threading;
using System;

namespace GreeManagmentSystem.User.InputInfo
{
    /// <summary>
    /// Interaction logic for ArgumentsInput.xaml
    /// </summary>
    public partial class ArgumentsInput : Page
    {

        static Multiply func = new Multiply();

        //SQL of Series
        string SQL_Series = "SELECT*FROM tb_seriesTemplate";

        public ArgumentsInput()
        {
            InitializeComponent();
            //系列
            func.Fill_Combobox(Series, SQL_Series, 1);

        }

        //保存参数
        private void Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ArgumentsDao arguments = new ArgumentsDao();
            string TypeClass = Category.Text;
            string ProductName = Series.Text + Work.Text + Level.Text + Cold.Text + "(" + Color.Text + ")";
            string ProductType = ProCode.Text + Struct.Text + Function.Text + "-" + Specification.Text + CategoryCode.Text + External.Text 
                                + "/" + "(" + Multiple.Text + ")" + WorkCode.Text + ColdCode.Text + FaceCode.Text + "-" + LevelCode.Text + "(" + RemarkCode.Text + ")";
            string ProductRemarks = Remarks.Text;
            arguments.AddArguments(TypeClass, ProductName, ProductType,ProductRemarks,this);
            arguments.AddArgumentsSeries(Series.Text);
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

        //点击确定关闭消息框
        private void sure_ActionClick(object sender, RoutedEventArgs e)
        {
            func.MessageBox_self(message, false);
        }


    }

    //非关键方法区
    public partial class ArgumentsInput
    {
        //清除内容
        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            Category.Text = "";
            Series.Text = "";
            Work.Text = "";
            Level.Text = "";
            Cold.Text = "";
            Color.Text = "";
            ProCode.Text = "";
            Struct.Text = "";
            Function.Text = "";
            Specification.Text = "";
            CategoryCode.Text = "";
            External.Text = "";
            Multiple.Text = "";
            WorkCode.Text = "";
            ColdCode.Text = "";
            FaceCode.Text = "";
            LevelCode.Text = "";
            RemarkCode.Text = "";

        }
    }
}
