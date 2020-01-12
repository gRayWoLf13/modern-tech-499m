using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Views
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        public BaseWindow()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);
        }

        private void BaseWindow_OnActivated(object sender, EventArgs e)
        {
            //Hide overlay if we are focused
            (DataContext as WindowViewModel).DimmableOverlayVisible = false;
        }

        private void BaseWindow_OnDeactivated(object sender, EventArgs e)
        {
            //Show overlay if we are not focused
            (DataContext as WindowViewModel).DimmableOverlayVisible = true;
        }
    }
}
