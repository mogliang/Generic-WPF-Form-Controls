using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormControlLib.Utilites
{
	public class PropertyNameValuePair:NameValuePair
	{
		static public List<PropertyNameValuePair> TypeToList(Type datatype)
		{
			var list =new List<PropertyNameValuePair>();
			var props = datatype.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
			foreach (var prop in props)
				list.Add(PropertyToNVPair(prop));
			return list;
		}

		static public PropertyNameValuePair PropertyToNVPair(PropertyInfo pinfo)
		{
			var nv = new PropertyNameValuePair();
			nv.Value = pinfo;
			nv.Name = pinfo.Name;

			var attr = pinfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
			if (attr != null)
			{
				if (!string.IsNullOrEmpty(attr.Name))
				{
					nv.Name = attr.Name;
				}
				if (!string.IsNullOrEmpty(attr.Description))
				{
					nv.Description = attr.Description;
				}
			}
			return nv;
		}
	}
}
