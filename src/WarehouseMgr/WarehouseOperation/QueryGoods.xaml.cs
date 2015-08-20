using FormControlLib.Utilites;
using System;
using System.Collections.Generic;
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
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;
using WarehouseMgr.Controls;

namespace WarehouseMgr.WarehouseOperation
{
	/// <summary>
	/// Interaction logic for QueryGoods.xaml
	/// </summary>
	public partial class QueryGoods : UserControl
	{
		public QueryGoods()
		{
			InitializeComponent();

			searchctl.InputControlBuilder.AddSink(new AddressSink());
			searchctl.DetermineFieldCallback = (t, list) =>
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					var pinfo = list[i].Value as PropertyInfo;
					switch (pinfo.Name)
					{
						case "GoodsId":
						case "Product":
							list.RemoveAt(i);
							break;
					}
				}
				return list;
			};
			searchctl.RenderSearchControl(typeof(Goods));
			GenerateDataGridColumns(typeof(Goods));
		}

		private void query_click(object sender, RoutedEventArgs e)
		{
			var filter = searchctl.BuildQueryString();
			if (string.IsNullOrEmpty(filter))
				return;

			using (var dbcontext = new WarehouseContext())
			{
				var querystr = "select * from Goods where " + filter;
				var data = dbcontext.Database.SqlQuery<Goods>(querystr);
				goodsgrid.ItemsSource = null;
				goodsgrid.ItemsSource = data.ToList();
			}
		}

		void GenerateDataGridColumns(Type type)
		{
			var list = PropertyNameValuePair.TypeToList(type);
			foreach (var p in list)
			{
				var pinfo = p.Value as PropertyInfo;
				if (pinfo.Name == "GoodsId")
					continue;
				if (pinfo.Name == "AddressCode")
				{
					var datagridcolumn = new DataGridTemplateColumn();
					datagridcolumn.Header = p.Name;
					datagridcolumn.CellTemplate = Application.Current.Resources["addressTemplate"] as DataTemplate;
					goodsgrid.Columns.Add(datagridcolumn);
					continue;
				}

				// general
				var textcolumn = new DataGridTextColumn();
				textcolumn.Header = p.Name;
				var binding = new Binding(pinfo.Name);

				if (pinfo.PropertyType.IsEnum)
				{
					binding.Converter = new EnumConverter();
				}
				textcolumn.Binding = binding;
				
				goodsgrid.Columns.Add(textcolumn);
			}
		}
	}
}
