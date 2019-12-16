using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    public class GamePageViewModel : BaseViewModel
    {
        public ICommand TestCommand { get; set; }       
        public GamePageViewModel()
        {
            TestCommand = new RelayParameterizedCommand((obj) => MessageBox.Show(obj.ToString()));
        }
    }
}
