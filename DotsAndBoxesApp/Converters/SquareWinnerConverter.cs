using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DotsAndBoxesApp.Converters
{
	class SquareWinnerConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var wonsq = value as DotsAndBoxes.Types.WonSquare;   

			if (wonsq != null)
			{
				return wonsq.winner.IsP1 ? "1" : "2";
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
