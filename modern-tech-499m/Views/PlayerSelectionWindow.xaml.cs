using System.Windows;
using modern_tech_499m.Logic;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Views
{
    /// <summary>
    /// Interaction logic for PlayerSelectionWindow.xaml
    /// </summary>
    public partial class PlayerSelectionWindow : Window
    {
        public PlayerSelectionWindow()
        {
            InitializeComponent();
            DataContext = BootStrapper.Resolve<IEntitySelectionViewModel<IPlayer>>();
        }
    }
}
