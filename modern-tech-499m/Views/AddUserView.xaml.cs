using System.Windows;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Views
{
    /// <summary>
    /// Interaction logic for AddUserView.xaml
    /// </summary>
    public partial class AddUserView : Window
    {
        public AddUserView()
        {
            InitializeComponent();
            DataContext = BootStrapper.Resolve<AddUserViewModel>();
        }
    }
}
