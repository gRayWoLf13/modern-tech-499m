using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace modern_tech_499m.UserControls
{
    /// <summary>
    /// Interaction logic for GameCellControl.xaml
    /// </summary>
    public partial class GameCellControl : UserControl
    {
        public static readonly DependencyProperty CellValueProperty = DependencyProperty.Register(
            "CellValue", typeof(string), typeof(GameCellControl), new PropertyMetadata(string.Empty));

        public string CellValue
        {
            get => (string)GetValue(CellValueProperty);
            set => SetValue(CellValueProperty, value);
        }

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            "ClickCommand", typeof(ICommand), typeof(GameCellControl), new PropertyMetadata(default(ICommand)));

        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        public static readonly DependencyProperty IsActiveCellProperty = DependencyProperty.Register(
            "IsActiveCell", typeof(bool), typeof(GameCellControl), new PropertyMetadata(default(bool)));

        public bool IsActiveCell
        {
            get => (bool)GetValue(IsActiveCellProperty);
            set => SetValue(IsActiveCellProperty, value);
        }

        public static readonly DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register(
            "ClickCommandParameter", typeof(object), typeof(GameCellControl), new PropertyMetadata(default(object)));

        public object ClickCommandParameter
        {
            get => GetValue(ClickCommandParameterProperty);
            set => SetValue(ClickCommandParameterProperty, value);
        }

        public GameCellControl()
        {
            InitializeComponent();
        }
    }
}
