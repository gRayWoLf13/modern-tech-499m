using System.Windows;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Views
{
    /// <summary>
    /// Interaction logic for UsersDatabaseView.xaml
    /// </summary>
    public partial class UsersDatabaseView : Window
    {
        //public UsersDatabaseView()
        //{
        //    InitializeComponent();
        //    DataContext = new UsersDatabaseViewModel(null, null);
        //}

        public UsersDatabaseView(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            InitializeComponent();
            DataContext = new UsersDatabaseViewModel(unitOfWork, userRepository);
        }

        public UsersDatabaseView()
        {
            InitializeComponent();
            DataContext = BootStrapper.Resolve<UsersDatabaseViewModel>();
        }
    }
}
