using FormControlLib.ControlSinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;
using FormControlLib;

namespace WarehouseMgr.Controls
{
	public class AddressSink : ControlSinkBase
	{
		public override MatchResult MatchTest(FormControlLib.FormItemContext context)
		{
			var attr = context.PropertyInfo.GetCustomAttribute(typeof(UIHintAttribute)) as UIHintAttribute;
			if (context.PropertyInfo.PropertyType == typeof(string)
				&& attr != null
				&& string.Equals(attr.UIHint, "address", StringComparison.OrdinalIgnoreCase))
			{
				return MatchResult.Recommanded;
			}
			else
				return MatchResult.No;
		}

		public override System.Windows.FrameworkElement CreateControl(FormControlLib.FormItemContext context)
		{
			// create control
			var inputctl = new AddressCombobox();
			
			// set style
			inputctl.Style = Application.Current.Resources["edit_controlbase"] as Style;
			
			// set binding
			var binding = new Binding(context.PropertyInfo.Name)
			{
				Mode = BindingMode.TwoWay,
			};
			inputctl.SetBinding(WatermarkTextBox.TextProperty, binding);

			// set validation
			CustomValidation.SetValidationOptOut(inputctl);

			return inputctl;
		}
	}
}
