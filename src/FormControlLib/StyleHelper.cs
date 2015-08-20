using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FormControlLib
{
    class StyleHelper
    {
        static ResourceDictionary _styleDict;
        internal static ResourceDictionary StyleDict
        {
            get
            {
                if (_styleDict == null)
                {
                    _styleDict = Application.LoadComponent(
                     new Uri("/FormControlLib;component/StyleDictionary.xaml",
                     UriKind.RelativeOrAbsolute)) as ResourceDictionary;
                }
                return _styleDict;
            }
        }

		internal static void ApplyStyle(FrameworkElement ctl)
		{
			var styledict = StyleDict;
			foreach (string key in styledict.Keys)
			{
				var style = styledict[key] as Style;
				if (style != null && style.TargetType == ctl.GetType())
				{
					ctl.Style = style;
				}
			}
		}

		internal static void ApplyStyle(FrameworkElement ctl, string styleName)
		{
			var styledict = StyleDict;
			ctl.Style = styledict[styleName] as Style;
		}
    }
}
