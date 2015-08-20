using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace WarehouseMgr.Infrastructure
{
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,Inherited=false)]
	public class CommandAttribute:Attribute
	{
		public Verb Verbs { set; get; }
		public string DisplayName { set; get; }
		public string Description { set; get; }
		public string Name { set; get; }
		public CommandAttribute(string Name)
		{
			this.Name = Name;
		}

		public static string GetCommandName(MemberInfo t)
		{
			var attr = t.GetCustomAttribute<CommandAttribute>();
			if (attr != null)
				return attr.Name;
			else return null;
		}

		public static string GetCommandDisplayName(MemberInfo t)
		{
			var attr = t.GetCustomAttribute<CommandAttribute>();
			if (attr != null)
				return attr.DisplayName;
			else return null;
		}
	}

	public enum Verb
	{
		Read=0,
		Write=2,
	}
}
