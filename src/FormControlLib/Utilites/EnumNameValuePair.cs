using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace FormControlLib.Utilites
{
	public class EnumNameValuePair : NameValuePair
	{
		static public List<EnumNameValuePair> EnumToList(Type enumtype)
		{
			var valuelist = enumtype.GetEnumValues();
			var source = new List<EnumNameValuePair>();
			foreach (var v in valuelist)
			{
				var sitem = new EnumNameValuePair();
				sitem.Value = v;

				var member = enumtype.GetMember(v.ToString())[0];
				var attr = member.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
				if (attr != null)
				{
					sitem.Name = attr.Name;
					sitem.Description = attr.Description;
				}
				else
					sitem.Name = v.ToString();

				source.Add(sitem);
			}

			return source;
		}
	}

	

}
