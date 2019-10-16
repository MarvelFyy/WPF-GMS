using Lierda.WPFHelper;
using System.Windows;

namespace GreeManagmentSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        LierdaCracker cracker = new LierdaCracker();

        protected override void OnStartup(StartupEventArgs e)
        {
            cracker.Cracker(30);//垃圾回收间隔时间
            base.OnStartup(e);
        }
    }
}
