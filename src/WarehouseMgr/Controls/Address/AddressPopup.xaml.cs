using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr.Controls
{
	/// <summary>
	/// Interaction logic for AddressPopup.xaml
	/// </summary>
	public partial class AddressPopup : UserControl
	{
		JObject addrmap;
		public AddressPopup()
		{
			InitializeComponent();
			addrmap = Metadata.MetadataUtility.LoadMetadata(Constrants.WAREHOUSE_MAP_STORE_STR);
			LoadList(1, addrmap);
		}

		void LoadList(int level, JObject parent)
		{
			IEnumerable list = null;
			if (parent != null && parent["Children"] != null)
			{
				list = parent["Children"];
			}

			switch (level)
			{
				case 1:
					addr_lb1.ItemsSource = list;
					break;
				case 2:
					addr_lb2.ItemsSource = list;
					break;
				case 3:
					addr_lb3.ItemsSource = list;
					break;
				case 4:
					addr_lb4.ItemsSource = list;
					break;
			}
		}

		string GetSelectedAddrCode()
		{
			if (addr_lb4.SelectedValue != null)
				return ((dynamic)addr_lb4.SelectedValue).ID;
			if (addr_lb3.SelectedValue != null)
				return ((dynamic)addr_lb3.SelectedValue).ID;
			if (addr_lb2.SelectedValue != null)
				return ((dynamic)addr_lb2.SelectedValue).ID;
			if (addr_lb1.SelectedValue != null)
				return ((dynamic)addr_lb1.SelectedValue).ID;
			return null;
		}

		public Action<string> SelectedCallback { set; private get; }

		private void level1_selected(object sender, RoutedEventArgs e)
		{
			var node = addr_lb1.SelectedItem as JObject;
			LoadList(2, node);
		}

		private void level2_selected(object sender, RoutedEventArgs e)
		{
			var node = addr_lb2.SelectedItem as JObject;
			LoadList(3, node);
		}

		private void level3_selected(object sender, RoutedEventArgs e)
		{
			var node = addr_lb3.SelectedItem as JObject;
			LoadList(4, node);
		}

		private void level4_selected(object sender, RoutedEventArgs e)
		{
			if (addr_lb4.SelectedValue != null && SelectedCallback != null)
				SelectedCallback(GetSelectedAddrCode());
		}

		private void ok_Click(object sender, RoutedEventArgs e)
		{
			if (SelectedCallback != null)
				SelectedCallback(GetSelectedAddrCode());
		}
	}
}
