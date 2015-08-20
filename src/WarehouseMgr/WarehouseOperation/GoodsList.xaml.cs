using FormControlLib;
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
using Warehouse.DataModel.Entities;
using Xceed.Wpf.Toolkit;

namespace WarehouseMgr.WarehouseOperation
{
	/// <summary>
	/// Interaction logic for GoodsList.xaml
	/// </summary>
	public partial class GoodsList : UserControl
	{
		public GoodsList()
		{
			InitializeComponent();
			CustomValidation.SetValidationCallback(this, () => null);
			this.DataContextChanged += GoodsList_DataContextChanged;
		}

		public WarehouseContext DBContext { set; get; }
		public GoodsList(WarehouseContext dbcontext):this()
		{
			this.DBContext = dbcontext;
		}

		void GoodsList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			LoadDataGrid();
		}

		private void tb_keydown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				CreateGoods();
			}
		}

		private void add_click(object sender, RoutedEventArgs e)
		{
			CreateGoods();
		}

		void CreateGoods()
		{
			var product = DBContext.Products.FirstOrDefault(p => p.ScanCode == scancode_tb.Text);
			if (product == null)
			{
				product = new ProductOperation.ProductMgmtModule()
					.CallAddProductPopup(scancode_tb.Text, "未发现此产品，请先录入产品信息",DBContext);
			}

			Goods newgoods = null;
			if (product != null)
			{
				newgoods = new WarehouseOperationModule()
					.CallRegisterGoodsPopup(product, DBContext);
			}

			if (newgoods != null)
			{
				var inbound = this.DataContext as WarehouseInbound;
				if (inbound != null)
				{
					if (inbound.InboundGoods == null)
						inbound.InboundGoods = new List<Goods>();
					inbound.InboundGoods.Add(newgoods);
				}
			}
			LoadDataGrid();
		}

		void LoadDataGrid()
		{
			var inbound = this.DataContext as WarehouseInbound;
			if (inbound != null && inbound.InboundGoods != null)
			{
				goodsgrid.ItemsSource = null;
				goodsgrid.ItemsSource = inbound.InboundGoods;
			}
		}

		private void remove_click(object sender, RoutedEventArgs e)
		{
			var goods = ((FrameworkElement)sender).DataContext as Goods;
			var inbound = this.DataContext as WarehouseInbound;
			inbound.InboundGoods.Remove(goods);
			goodsgrid.ItemsSource = null;
			goodsgrid.ItemsSource = inbound.InboundGoods;
		}
	}
}
