using System.Windows;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Views
{
    /// <summary>
    /// Interaction logic for GameInfoSelectionWindow.xaml
    /// </summary>
    public partial class GameInfoSelectionWindow : Window
    {
        public GameInfoSelectionWindow()
        {
            InitializeComponent();
            DataContext = BootStrapper.Resolve<IEntitySelectionViewModel<GameInfo>>();
        }
    }
}
