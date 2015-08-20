using FormControlLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;
using WarehouseMgr.Controls;
using WarehouseMgr.Infrastructure;
using WarehouseMgr.Utilites;

namespace WarehouseMgr.ProductOperation
{
	[Command("GOODS_OPERATION",DisplayName="产品管理")]
	public class ProductMgmtModule
	{
		public Control CallAddProductControl(string productScanCode, WarehouseContext dbcontext, Action<Product> createCallback = null)
		{
			var formControl = FormControlHelper.CreateFormControl();
			var product = new Product();
			product.ScanCode = productScanCode;
			formControl.RenderForm(product, false);
			formControl.ConfirmButton.Content = "录入产品";
			formControl.SubmitCallback = (d) =>
			{
				try
				{
					dbcontext.Products.Add((Product)d);
					dbcontext.SaveChanges();
					if (createCallback != null)
						createCallback((Product)d);
				}
				catch (Exception ex)
				{
					//TODO: handle exception
					formControl.SetErrorMsgManually("ScanCode", "此产品扫码已存在");
				}
			};

			return formControl;
		}

		public Product CallAddProductPopup(string productScanCode, string notificationMsg,WarehouseContext dbcontext)
		{
			var window = new NotifyWindow();
			window.Width = 400;
			window.Height = 600;
			window.Title = "录入产品信息";

			Product newp = null;
			window.MyContent = CallAddProductControl(productScanCode, dbcontext, (p) =>
			{
				window.Close();
				newp = p;
			});

			if (!string.IsNullOrEmpty(notificationMsg))
				window.ShowNotificationMessage(notificationMsg);

			// block until dialog close
			window.ShowDialog();
			return newp;
		}

		[Command("PRODUCT_ADD", DisplayName = "添加产品信息")]
		public void CallAddProductPage()
		{
			var dbcontext = new WarehouseContext();
			var tab = WindowMgr.CreateTabPage("添加产品信息", "添加产品信息", false);
			var form = CallAddProductControl(null, dbcontext, (u) =>
			{
				WindowMgr.SendNotification("添加产品信息成功",NotificationLevel.Information);
				WindowMgr.CloseTabPage(tab);
				dbcontext.Dispose();
			});
			tab.Content = form;
		}

		[Command("PRODUCT_EDIT_DELETE", DisplayName = "管理产品信息")]
		public void CallManageProductPage()
		{

		}

		[Command("TAG_EDIT", DisplayName = "管理分类标签")]
		public void CallTagMgmtPage()
		{
			WarehouseContext dbcontext;
			var tagMgmtPage = WindowMgr.CreateTabPageWithDBContext("管理分类标签", "管理分类标签", true, out dbcontext);
			if (tagMgmtPage != null)
				tagMgmtPage.Content = new ManageTags(dbcontext);
		}
	}
}
