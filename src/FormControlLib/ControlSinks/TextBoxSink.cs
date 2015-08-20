using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Data;
using System.Windows;

namespace FormControlLib.ControlSinks
{
	public class TextBoxSink : ControlSinkBase
	{
		public override MatchResult MatchTest(FormItemContext context)
		{
			if (context.PropertyInfo.PropertyType == typeof(string))
				return MatchResult.Yes;
			else
				return MatchResult.No;
		}
		public override FrameworkElement CreateControl(FormItemContext context)
		{
			TextBox tb = new TextBox();

			// apply [MaxLength(x)] attribute
			var attr2 = context.PropertyInfo.GetCustomAttribute(typeof(MaxLengthAttribute)) as MaxLengthAttribute;
			var attr3 = context.PropertyInfo.GetCustomAttribute(typeof(StringLengthAttribute)) as StringLengthAttribute;
			int maxlength = 0;
			if (attr2 != null)
			{
				maxlength = attr2.Length;
			}
			else if (attr3 != null)
			{
				maxlength = attr3.MaximumLength;
			}

			if(maxlength>0)
			{
				tb.MaxLength = maxlength;
			}
			if (maxlength > 100) //TODO multiline mode
			{
				tb.TextWrapping = System.Windows.TextWrapping.Wrap;
				tb.AcceptsReturn = true;
				tb.Height = 50;
			}

			var binding = new Binding(context.PropertyInfo.Name)
			{
				Mode = BindingMode.TwoWay,
			};
			binding.ValidationRules.Add(new AnnotationValidationRule(context.PropertyInfo));
			tb.SetBinding(TextBox.TextProperty, binding);
			CustomValidation.SetValidationProperty(tb, TextBox.TextProperty);
			tb.GotFocus += (s, e) =>
			{
				tb.SelectAll();
			};
			StyleHelper.ApplyStyle(tb, FormControlConstrants.EDIT_TEXTBOX_STYLE);
			return tb;
		}
	}
}
