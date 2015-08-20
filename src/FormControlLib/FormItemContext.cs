using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormControlLib
{
    public class FormItemContext
    {
        public FormItemContext() { }
		public FormItemContext(object bindedData, Type datatype, PropertyInfo propinfo, ControlType ctype)
		{
			BindedData = bindedData;
			DataType = datatype;
			PropertyInfo = propinfo;
			ControlType = ctype;
		}

		public FormItemContext(Type datatype, PropertyInfo propinfo)
		{
			DataType = datatype;
			PropertyInfo = propinfo;
		}

        public Type DataType { private set; get; }
        public PropertyInfo PropertyInfo {private set; get; }
		public ControlType ControlType {private set; get; }
		public object BindedData {private set; get; }
		
		public void SetValueToBindedProperty(object value)
		{
			PropertyInfo.SetValue(BindedData, value);
		}
    }

	public enum ControlType
	{
		None,
		Label,
		Editable,
		Readonly
	}
}
