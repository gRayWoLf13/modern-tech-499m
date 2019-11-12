using System;
using System.Globalization;
using System.Windows.Data;
using modern_tech_499m.Logic;

namespace modern_tech_499m.Converters
{
    class GameControllerToCellValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            GameController controller = (GameController)values[0];
            IPlayer player = (IPlayer)values[1];
            int cellIndex = System.Convert.ToInt32(values[2]);
            return controller.GameLogic.GetCellValue(player, cellIndex);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
