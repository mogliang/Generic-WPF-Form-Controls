using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FormControlLib.ControlSinks
{
	public class DatePickerSink:ControlSinkBase
	{
		public override MatchResult MatchTest(FormItemContext context)
		{
			if (context.PropertyInfo.PropertyType == typeof(DateTime) ||
				context.PropertyInfo.PropertyType == typeof(Nullable<DateTime>))
				return MatchResult.Yes;
			else
				return MatchResult.No;
		}

		public override FrameworkElement CreateControl(FormItemContext context)
		{
			DatePicker picker = new DatePicker();

			var binding = new Binding(context.PropertyInfo.Name)
			{
				Mode = BindingMode.TwoWay,
			};
			binding.ValidationRules.Add(new AnnotationValidationRule(context.PropertyInfo));
			picker.SetBinding(DatePicker.SelectedDateProperty, binding);
			CustomValidation.SetValidationProperty(picker, DatePicker.SelectedDateProperty);
			StyleHelper.ApplyStyle(picker, FormControlConstrants.EDIT_DATE_STYLE);
			return picker;
		}
	}
}
