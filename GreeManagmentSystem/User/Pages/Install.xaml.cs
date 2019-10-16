using System.Windows;
using System.Windows.Controls;
using GreeManagmentSystem.User.Preview;

namespace GreeManagmentSystem.User.Pages
{
    /// <summary>
    /// Interaction logic for Install.xaml
    /// </summary>
    public partial class Install : Page
    {
        public Install()
        {
            InitializeComponent();
            IntallFrame.Content = new InstallView();
        }
    }
}
