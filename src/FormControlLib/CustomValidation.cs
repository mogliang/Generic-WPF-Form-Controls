using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FormControlLib
{
	public static class CustomValidation
	{
		public static string GetValidationError(DependencyObject obj)
		{
			return (string)obj.GetValue(ValidationErrorProperty);
		}

		public static void SetValidationError(DependencyObject obj, string value)
		{
			obj.SetValue(ValidationErrorProperty, value);
		}

		public static readonly DependencyProperty ValidationErrorProperty =
			DependencyProperty.RegisterAttached("ValidationError", typeof(string), typeof(FrameworkElement), new PropertyMetadata(null));

		public static Func<string> GetValidationCallback(DependencyObject obj)
		{
			return (Func<string>)obj.GetValue(ValidationCallbackProperty);
		}

		public static void SetValidationCallback(DependencyObject obj, Func<string> value)
		{
			obj.SetValue(ValidationCallbackProperty, value);
		}

		public static void SetValidationOptOut(DependencyObject obj)
		{
			SetValidationCallback(obj, () => null);
		}

		public static readonly DependencyProperty ValidationCallbackProperty =
			DependencyProperty.RegisterAttached("ValidationCallback", typeof(Func<string>), typeof(FrameworkElement), new PropertyMetadata(null));

		public static DependencyProperty GetValidationProperty(DependencyObject obj)
		{
			return (DependencyProperty)obj.GetValue(ValidationPropertyProperty);
		}

		public static void SetValidationProperty(DependencyObject obj, DependencyProperty value)
		{
			obj.SetValue(ValidationPropertyProperty, value);
		}

		public static readonly DependencyProperty ValidationPropertyProperty =
			DependencyProperty.RegisterAttached("ValidationProperty", typeof(DependencyProperty), typeof(FrameworkElement), new PropertyMetadata(null));

		public static string ForceValidation(FrameworkElement ele)
		{
			string errMsg = "";
			var validationprop = GetValidationProperty(ele);
			if (validationprop != null)
			{
				ele.GetBindingExpression(validationprop).UpdateSource();
				if (Validation.GetHasError(ele))
				{
					var errs = Validation.GetErrors(ele);
					foreach (var err in errs)
					{
						string errstr = null;
						if (err.ErrorContent != null)
							errstr = err.ErrorContent.ToString();
						else if (err.Exception != null)
							errstr = err.Exception.Message;
						else
							errstr = "Unknown Error";
						errMsg += errstr + "\n";
					}
				}
				return errMsg;
			}

			var validationcb = GetValidationCallback(ele);
			if (validationcb != null)
			{
				return validationcb();
			}

			throw new InvalidOperationException("Neither ValidationProperty nor ValidationCallback is set.");
		}

		public static PropertyPath GetValidationMsgBindingPath(FrameworkElement ele)
		{
			if (GetValidationProperty(ele) != null)
				return new PropertyPath("(Validation.Errors)[0].ErrorContent");
			else if (GetValidationCallback(ele) != null)
				return new PropertyPath(CustomValidation.ValidationErrorProperty);
			else
				throw new InvalidOperationException("Neither ValidationProperty nor ValidationCallback is set.");
		}
	}
}
