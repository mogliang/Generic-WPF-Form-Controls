using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FormControlLib.ControlSinks
{
	public class PlaceholderSink:ControlSinkBase
	{
		public override MatchResult MatchTest(FormItemContext context)
		{
			return MatchResult.NotRecommanded;
		}

		public override FrameworkElement CreateControl(FormItemContext context)
		{
			// create control
			var border = new Border();
			border.BorderThickness = new System.Windows.Thickness(4);
			var tb= new Label
			{
				Content="尚未实现",
				Foreground=new SolidColorBrush(Colors.Gray),
				HorizontalAlignment=HorizontalAlignment.Center,
				HorizontalContentAlignment=HorizontalAlignment.Center
			};
			border.Child = tb;

			// apply style
			StyleHelper.ApplyStyle(tb, "label_Label");
			StyleHelper.ApplyStyle(border, "placeholder_br");

			// set validation
			CustomValidation.SetValidationCallback(border, () => null);

			return border;
		}
	}
}
