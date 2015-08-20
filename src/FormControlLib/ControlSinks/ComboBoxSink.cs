using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Windows.Data;
using System.Windows;
using System.Collections;
using FormControlLib.Utilites;

namespace FormControlLib.ControlSinks
{
	public class ComboBoxSink : ControlSinkBase
	{
		public override MatchResult MatchTest(FormItemContext context)
		{
			if (context.PropertyInfo.PropertyType.IsEnum)
			{
				return MatchResult.Yes;
			}
			else
				return MatchResult.No;

		}

		public FrameworkElement CreateControlForLookup(FormItemContext context, List<NameValuePair> options)
		{
			ComboBox cb = new ComboBox();
			cb.ItemsSource = options;
			cb.SelectedValuePath = "Value";

			var binding = new Binding(context.PropertyInfo.Name)
			{
				Mode = BindingMode.TwoWay,
			};
			binding.ValidationRules.Add(new AnnotationValidationRule(context.PropertyInfo));
			cb.SetBinding(ComboBox.SelectedValueProperty, binding);
			CustomValidation.SetValidationProperty(cb, ComboBox.SelectedValueProperty);
			StyleHelper.ApplyStyle(cb, FormControlConstrants.EDIT_COMBO_STYLE);
			return cb;
		}

		public override FrameworkElement CreateControl(FormItemContext context)
		{
			ComboBox cb = new ComboBox();
			var enumtype = context.PropertyInfo.PropertyType;
			var source = EnumNameValuePair.EnumToList(enumtype);

			cb.ItemsSource = source;
			cb.SelectedValuePath = "Value";

			var binding = new Binding(context.PropertyInfo.Name)
			{
				Mode = BindingMode.TwoWay,
			};
			binding.ValidationRules.Add(new AnnotationValidationRule(context.PropertyInfo));
			cb.SetBinding(ComboBox.SelectedValueProperty, binding);
			CustomValidation.SetValidationProperty(cb, ComboBox.SelectedValueProperty);
			StyleHelper.ApplyStyle(cb, FormControlConstrants.EDIT_COMBO_STYLE);
			return cb;
		}

		void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var cc= e.AddedItems[0];
		}
	}
}
