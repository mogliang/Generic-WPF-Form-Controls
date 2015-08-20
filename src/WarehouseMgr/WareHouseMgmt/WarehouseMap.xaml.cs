using Newtonsoft.Json.Linq;
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
using Warehouse.DataModel;
using WarehouseMgr.Infrastructure;
using WarehouseMgr.Metadata;

namespace WarehouseMgr.WarehouseMgmt
{
	/// <summary>
	/// Interaction logic for WarehouseMap.xaml
	/// </summary>
	public partial class WarehouseMap : UserControl
	{
		public WarehouseMap()
		{
			InitializeComponent();
			Loaded += WarehouseMap_Loaded;
		}

		bool isrendered = false;
		void WarehouseMap_Loaded(object sender, RoutedEventArgs e)
		{
			if (!isrendered)
			{
				isrendered = true;
				RenderWarehouseMap();
				rootnode.IsSelected = true;
			}
		}

		WarehouseContext _dbcontext;
		public WarehouseMap(WarehouseContext dbcontext):this()
		{
			_dbcontext = dbcontext;
		}

		private void treeview_selectionchanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			ShowCommandPanel();
		}

		void ShowCommandPanel()
		{
			addsubnodegrid.Visibility = Visibility.Collapsed;
			editnodegrid.Visibility = Visibility.Collapsed;
			commandpanel.Visibility = Visibility.Visible;

			var selectednode = warehousetree.SelectedItem as TreeViewItem;
			foreach (FrameworkElement fe in commandpanel.Children)
				fe.Visibility = Visibility.Collapsed;

			if (selectednode == rootnode)
			{
				addsubnode_bn.Visibility = Visibility.Visible;
			}
			else if (selectednode != null)
			{
				editnode_bn.Visibility = System.Windows.Visibility.Visible;
				delnode_bn.Visibility = System.Windows.Visibility.Visible;

				if (((string)selectednode.Tag).Count(c => c == '.') < Constrants.WAREHOUSE_MAX_LEVEL-1)
				{
					addsubnode_bn.Visibility = System.Windows.Visibility.Visible;
				}
			}
		}

		void ShowEditPanel()
		{
			addsubnodegrid.Visibility = Visibility.Collapsed;
			editnodegrid.Visibility = Visibility.Visible;
			commandpanel.Visibility = Visibility.Collapsed;
		}

		void ShowAddPanel()
		{
			addsubnodegrid.Visibility = Visibility.Visible;
			editnodegrid.Visibility = Visibility.Collapsed;
			commandpanel.Visibility = Visibility.Collapsed;
		}

		int GetCurrentMaxNumber(TreeViewItem parent)
		{
			if (parent.Items.Count == 0)
				return 0;
			else
			{
				var sbnum = ((TreeViewItem)parent.Items[parent.Items.Count - 1]).Tag as string;
				var lastSubNum = sbnum.Split('.').Last();
				return int.Parse(lastSubNum.Substring(1));
			}
		}

		string CreateNewNumber(TreeViewItem parent)
		{
			var parentnum = parent.Tag as string;
			int level;
			if (parentnum == null)
			{
				parentnum = "";
				level = 0;
			}
			else
			{
				level = parentnum.Split('.').Length;
			}

			string newnum = parentnum + "." + Constrants.ADDRPFX_LIST[level] + (GetCurrentMaxNumber(parent) + 1);
			return newnum.Trim('.');
		}

		void RenderWarehouseMap()
		{
			rootnode.Items.Clear();
			dynamic jo = MetadataUtility.LoadMetadata(Constrants.WAREHOUSE_MAP_STORE_STR);
			if (jo != null)
			{
				rootnode.Header = jo.Name;
				RenderWarehouseSubNode(rootnode, jo);
			}
		}

		TreeViewItem RenderWarehouseSubNode(TreeViewItem currentNode, dynamic currentdata)
		{
			var subdatas=currentdata.Children as JArray;
			if (subdatas != null && subdatas.Count > 0)
			{
				foreach (dynamic subdata in subdatas)
				{
					var subitem = new TreeViewItem
					{
						Header = subdata.Name.ToString(),
						Tag = subdata.ID.ToString()
					};
					currentNode.Items.Add(subitem);
					RenderWarehouseSubNode(subitem, subdata);
				}
			}
			return currentNode;
		}

		void SaveWarehouseMap()
		{
			dynamic rootdata = new JObject();
			rootdata.Name = "我的仓库";
			rootdata.FormatVersion = "1.0";
			rootdata = CreateMapJson(rootnode, rootdata);
			MetadataUtility.SaveMetadata(Constrants.WAREHOUSE_MAP_STORE_STR, rootdata);
		}

		JObject CreateMapJson(TreeViewItem currentNode, JObject currentData)
		{
			if (currentNode.Items.Count > 0)
			{
				var subdatas = new JArray();
				foreach (TreeViewItem subnode in currentNode.Items)
				{
					dynamic subdata = new JObject();
					subdata.ID = subnode.Tag as string;
					subdata.Name = subnode.Header as string;
					subdatas.Add(subdata);
					CreateMapJson(subnode, subdata);
				}
				currentData["Children"] = subdatas;
			}
			return currentData;
		}

		private void confirm_click(object sender, RoutedEventArgs e)
		{
			var current = warehousetree.SelectedItem as TreeViewItem;
			current.Header = name_tb.Text;
			ShowCommandPanel();
		}

		private void cancel_click(object sender, RoutedEventArgs e)
		{
			ShowCommandPanel();
		}

		private void addsubnode_click(object sender, RoutedEventArgs e)
		{
			ShowAddPanel();
		}

		private void editnode_click(object sender, RoutedEventArgs e)
		{
			var current = warehousetree.SelectedItem as TreeViewItem;
			name_tb.Text = current.Header as string;
			ShowEditPanel();
		}

		private void delnode_click(object sender, RoutedEventArgs e)
		{
			var current = warehousetree.SelectedItem as TreeViewItem;
			var parent= current.Parent as TreeViewItem;
			if (parent != null)
				parent.Items.Remove(current);
			ShowCommandPanel();
		}

		private void addconfirm_click(object sender, RoutedEventArgs e)
		{
			var parent = warehousetree.SelectedItem as TreeViewItem;
			for (int i = 0; i < num_ud.Value.Value; i++)
			{
				var titem = new TreeViewItem
				{
					Header = namepfx_tb.Text + (GetCurrentMaxNumber(parent) + 1),
					Tag = CreateNewNumber(parent)
				};
				parent.Items.Add(titem);
			}

			SaveWarehouseMap();
			ShowCommandPanel();
		}
	}
}
