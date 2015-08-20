using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FormControlLib.ControlSinks
{
	public class AnnotationValidationRule:ValidationRule
	{
		PropertyInfo _propInfo;
		IEnumerable<ValidationAttribute> _vlist;
		public AnnotationValidationRule(PropertyInfo propInfo)
		{
			_propInfo = propInfo;
			_vlist = propInfo.GetCustomAttributes(true).OfType<ValidationAttribute>();
		}

		public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			List<string> errormsgs=new List<string>();
			foreach (var va in _vlist)
			{
				if (!va.IsValid(value))
				{
					errormsgs.Add(va.ErrorMessage);
				}
			}
			if (errormsgs.Count == 0)
				return System.Windows.Controls.ValidationResult.ValidResult;
			else
				return new System.Windows.Controls.ValidationResult(false, errormsgs[0]);
		}
	}
}
