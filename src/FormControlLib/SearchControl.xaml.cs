using FormControlLib.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Interaction logic for SearchControl.xaml
	/// </summary>
	public partial class SearchControl : UserControl
	{
		public SearchControl()
		{
			InitializeComponent();
			StyleHelper.ApplyStyle(addquery_bn, "addquery_bn");
		}

		Type _intputType;
		public void RenderSearchControl(Type type)
		{
			_intputType = type;
			foreach (var row in querypanel.Children)
			{
				if (row is SearchRow)
				{
					((SearchRow)row).InputControlBuilder = InputControlBuilder;
					((SearchRow)row).DetermineFieldCallback = this.DetermineFieldCallback;
					((SearchRow)row).CreateControlCallback = this.CreateControlCallback;
					((SearchRow)row).RenderRow(_intputType);
				}
			}
		}

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

		int _rowcount = 0;
		private void addexp_click(object sender, RoutedEventArgs e)
		{
			if (_rowcount >= 4)
			{
				return;
			}

			querypanel.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(0,GridUnitType.Auto)
			});
			var idx = querypanel.RowDefinitions.Count - 1;

			var searchrow = new SearchRow();
			searchrow.DetermineFieldCallback = this.DetermineFieldCallback;
			searchrow.CreateControlCallback = this.CreateControlCallback;
			searchrow.RenderRow(_intputType);

			Grid.SetRow(searchrow, idx);
			Grid.SetColumn(searchrow, 1);
			querypanel.Children.Add(searchrow);

			var removebn = new Button();
			StyleHelper.ApplyStyle(removebn, "remove_bn");
			removebn.Click += (s, e2) =>
			{
				querypanel.Children.Remove(removebn);
				querypanel.Children.Remove(searchrow);
				_rowcount--;
			};
			Grid.SetRow(removebn, idx);
			querypanel.Children.Add(removebn);
			_rowcount++;
		}

		public string BuildQueryString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var row in querypanel.Children)
			{
				if (row is SearchRow)
					sb.Append(((SearchRow)row).GetConditionString());
			}
			var filterstr = sb.ToString();

			if (filterstr.StartsWith(" and"))
				filterstr = filterstr.Substring(4);
			else if (filterstr.StartsWith(" or"))
				filterstr = filterstr.Substring(3);
			return filterstr;
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
}
