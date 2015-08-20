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

namespace FormControlLib.ControlSinks
{
	public class PasswordSink : ControlSinkBase
	{
		public override MatchResult MatchTest(FormItemContext context)
		{
			var attr = context.PropertyInfo.GetCustomAttribute(typeof(UIHintAttribute)) as UIHintAttribute;
			if (context.PropertyInfo.PropertyType==typeof(string)
				&& attr != null
				&& string.Equals(attr.UIHint, "password", StringComparison.OrdinalIgnoreCase))
			{
				return MatchResult.Recommanded;
			}
			else
				return MatchResult.No;
		}
		public override FrameworkElement CreateControl(FormItemContext context)
		{
			PasswordBox tb = new PasswordBox();

			CustomValidation.SetValidationCallback(tb, () =>
			{
				string errMsg = null;
				if (tb.Password == null ||
					tb.Password.Length < 6)
				{
					errMsg = "密码强度不够";
				}
				CustomValidation.SetValidationError(tb, errMsg);
				return errMsg;
			});

			tb.PasswordChanged += (s, e) =>
			{
				CustomValidation.ForceValidation(tb);
				context.SetValueToBindedProperty(tb.Password);
			};

			StyleHelper.ApplyStyle(tb, "editctl_pwd");
			return tb;
		}
	}
}
