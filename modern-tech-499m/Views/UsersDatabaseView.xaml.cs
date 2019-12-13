using System.Windows;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Views
{
    /// <summary>
    /// Interaction logic for UsersDatabaseView.xaml
    /// </summary>
    public partial class UsersDatabaseView : Window
    {
        public UsersDatabaseView()
        {
            InitializeComponent();
            DataContext = BootStrapper.Resolve<UsersDatabaseViewModel>();
        }
    }
}
