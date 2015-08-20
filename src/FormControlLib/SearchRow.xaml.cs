using FormControlLib.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormControlLib
{
	/// <summary>
	/// Interaction logic for SearchRow.xaml
	/// </summary>
	public partial class SearchRow : UserControl
	{
		public SearchRow()
		{
			InitializeComponent();
			StyleHelper.ApplyStyle(logic_cb, "searchbase_control");
			StyleHelper.ApplyStyle(field_cb, "searchbase_control");
			StyleHelper.ApplyStyle(operator_cb, "searchbase_control");
			//StyleHelper.ApplyStyle(remove_bn,"remove_bn");
		}

		Type _type;
		public void RenderRow(Type inputtype)
		{
			_type = inputtype;
			Initialize(_type);
		}

		public bool IsFirstRow
		{
			set;
			get;
		}

		void Initialize(Type datatype)
		{
			var logiclist = EnumNameValuePair.EnumToList(typeof(Logical));
			logic_cb.ItemsSource = logiclist;
			if (IsFirstRow)
				logic_cb.IsEnabled = false;
			else
			{
				logic_cb.IsEnabled = true;
				logic_cb.SelectedIndex = 0;
			}

			var proplist = PropertyNameValuePair.TypeToList(datatype);
			if (DetermineFieldCallback != null)
				proplist = DetermineFieldCallback(datatype, proplist);
			field_cb.ItemsSource = proplist;
			field_cb.SelectedIndex = 0;
		}

		Object _bindobject;
		ControlsBuilder _controlbuilder = new ControlsBuilder();
		public ControlsBuilder InputControlBuilder
		{
			set
			{
				_controlbuilder = value;
			}
			get
			{
				return _controlbuilder;
			}
		}
		private void filed_changed(object sender, SelectionChangedEventArgs e)
		{
			var controlop = new ControlAndOperator();
			var pinfo=field_cb.SelectedValue as PropertyInfo;
			var ctlcontext = new FormItemContext(_type, pinfo);
			controlop.InputControl= InputControlBuilder.CreateControl(ctlcontext);
			if (pinfo.PropertyType.Name.Equals("string", StringComparison.OrdinalIgnoreCase))
			{
				controlop.UseStringOperator = true;
			}
			else
				controlop.UseNumberOperator = true;

			if (CreateControlCallback != null)
				controlop = CreateControlCallback(ctlcontext, controlop);

			_bindobject = Activator.CreateInstance(_type);
			controlop.InputControl.DataContext = _bindobject;
			input_br.Child = controlop.InputControl;

			operator_cb.ItemsSource = null;
			if (controlop.UseStringOperator)
				operator_cb.ItemsSource = EnumNameValuePair.EnumToList(typeof(StringOperator));
			else if (controlop.UseNumberOperator)
				operator_cb.ItemsSource = EnumNameValuePair.EnumToList(typeof(NumberOperator));

			operator_cb.SelectedIndex = 0;

			if (pinfo.PropertyType.IsEnum)
			{
				operator_cb.IsEnabled = false;
			}else
			{
				operator_cb.IsEnabled = true;
			}
		}

		public string GetConditionString()
		{
			StringBuilder sb = new StringBuilder();

			if (!IsFirstRow)
			{
				if (((Logical)logic_cb.SelectedValue) == Logical.And)
					sb.Append(" and");
				else
					sb.Append(" or");
			}

			sb.Append(" " + ((PropertyInfo)field_cb.SelectedValue).Name);
			var opstr = ConvertOperator(operator_cb.SelectedValue);
			sb.Append(" " + opstr);

			var propinfo = field_cb.SelectedValue as PropertyInfo;
			var inputvalue = propinfo.GetValue(_bindobject, null);
			if (inputvalue == null)
			{
				return "";
			}
			var inputtype = propinfo.PropertyType;
			if (inputtype.IsEnum ||
				inputtype == typeof(int))
			{
				sb.Append((int)inputvalue);
			}
			else if (opstr == "like")
			{
				sb.Append("'%" + inputvalue + "%'");
			}
			else
			{
				sb.Append("'" + inputvalue + "'");
			}

			return sb.ToString();
		}

		string ConvertOperator(object enumoperator)
		{
			if (enumoperator is StringOperator)
			{
				var op = (StringOperator)enumoperator;
				switch (op)
				{
					case StringOperator.Contains:
						return "like";
					case StringOperator.Equal:
						return "=";
				}
			}
			else if (enumoperator is NumberOperator)
			{
				var op = (NumberOperator)enumoperator;
				switch (op)
				{
					case NumberOperator.Equal:
						return "=";
					case NumberOperator.GreatorOrEqual:
						return ">=";
					case NumberOperator.GreatorThan:
						return ">";
					case NumberOperator.LessOrEqual:
						return "<=";
					case NumberOperator.LessThan:
						return "<";
				}
			}
			throw new Exception("Unexpected result.");
		}

		public Func<FormItemContext, ControlAndOperator, ControlAndOperator> CreateControlCallback
		{
			set;
			private get;
		}

		public Func<Type, List<PropertyNameValuePair>, List<PropertyNameValuePair>> DetermineFieldCallback
		{
			set;
			private get;
		}
	}

	public enum Logical
	{
		[Display(Name = "而且")]
		And,
		[Display(Name = "或者")]
		Or
	}
	public enum NumberOperator
	{
		[Display(Name = "等于")]
		Equal,
		[Display(Name = "大于")]
		GreatorThan,
		[Display(Name = "大于等于")]
		GreatorOrEqual,
		[Display(Name = "小于")]
		LessThan,
		[Display(Name = "小于等于")]
		LessOrEqual
	}

	public enum StringOperator
	{
		[Display(Name = "等于")]
		Equal,
		[Display(Name = "包含")]
		Contains
	}

	public class ControlAndOperator
	{
		public FrameworkElement InputControl { set; get; }
		public bool UseStringOperator { set; get; }
		public bool UseNumberOperator { set; get; }
	}
}
