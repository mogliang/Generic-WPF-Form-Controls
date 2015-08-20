using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Reflection;

namespace FormControlLib.Utilites
{
	public class EnumConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var enumtype = value.GetType();
			if (enumtype.IsEnum)
			{
				var member = enumtype.GetMember(value.ToString())[0];
				var attr = member.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
				if (attr != null)
				{
					return attr.Name;
				}
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
