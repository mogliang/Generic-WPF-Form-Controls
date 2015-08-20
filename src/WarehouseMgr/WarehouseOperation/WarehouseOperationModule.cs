using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FormControlLib;
using FormControlLib.ControlSinks;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;
using WarehouseMgr.Controls;
using WarehouseMgr.Infrastructure;
using FormControlLib.Utilites;
using WarehouseMgr.Utilites;

namespace WarehouseMgr.WarehouseOperation
{
	[Command("WAREHOUSE_OPERATION", DisplayName = "出入库管理")]
	public class WarehouseOperationModule
	{
		[Command("GOODS_QUERY", DisplayName = "查询货物")]
		public void CallQueryGoodsPage()
		{
			WarehouseContext context;
			WindowMgr.CreateTabPageWithDBContext("查询货物", "查询货物", true, out context, new QueryGoods());
		}

		public Goods CallRegisterGoodsPopup(Product product, WarehouseContext dbcontext)
		{
			Goods newGoods = null;
			var window = new NotifyWindow();
			window.Width = 400;
			window.Height = 600;
			window.Title = "添加货物";

			var formControl = FormControlHelper.CreateFormControl();
			var goods = new Goods();
			goods.Product = product;
			goods.Name = product.Name;
			goods.State = GoodsState.Inbound;
			goods.GoodsCode = DateTime.Now.ToString("yyyyMMddHHmmss")+"001";

			formControl.CreateControlCallback = (cx, ctl) =>
			{
				if (cx.ControlType == ControlType.Editable)
				{
					switch (cx.PropertyInfo.Name)
					{
						case "Product":
							var tb = new TextBox();
							tb.Text = product.Name;
							tb.Style = Application.Current.Resources["editctl_TextBox"] as Style;
							tb.IsEnabled = false;
							CustomValidation.SetValidationOptOut(tb);
							return tb;
						case "GoodsCode":
							ctl.IsEnabled = false;
							return ctl;
					}
				}
				return ctl;
			};

			formControl.RenderForm(goods, false);
			formControl.ConfirmButton.Content = "添加";
			formControl.SubmitCallback = d =>
			{
				try
				{
					newGoods = d as Goods;
					newGoods.InboundDate=DateTime.Now;
					dbcontext.Goods.Add(newGoods);
					window.Close();
				}
				catch (Exception ex)
				{
					window.ShowNotificationMessage("添加货物失败，请重试。");
				}
			};
			window.MyContent = formControl;
			window.ShowDialog();
			return newGoods;
		}

		internal FrameworkElement RenderSelectUserControl(FormItemContext context, WarehouseContext dbcontext,
			User selecteduser = null)
		{
			var options = (from u in dbcontext.Users
				select new NameValuePair
				{
					Name = u.Name,
					Description = u.IdentificationNumber,
					Value = u
				}).ToList();
			var ctl = new ComboBoxSink().CreateControlForLookup(context, options);

			if (selecteduser != null)
			{
				var selectedop = options.FirstOrDefault(o => ((User) o.Value).UserId == selecteduser.UserId);
				((ComboBox) ctl).SelectedValue = selectedop.Value;
			}
			return ctl;
		}

		[Command("WAREHOUSE_INBOUND", DisplayName = "入库")]
		public void CallWarehouseInboundtPage()
		{
			WarehouseContext dbcontext;
			var tab = WindowMgr.CreateTabPageWithDBContext("入库单", "入库单",false, out dbcontext);
			var formControl = FormControlHelper.CreateFormControl();
			formControl.CreateControlCallback = (cx, ctl) =>
			{
				if (cx.ControlType == ControlType.Label &&
					cx.PropertyInfo.Name == "InboundGoods")
				{
					ctl.VerticalAlignment = VerticalAlignment.Top;
					return ctl;
				}
				if (cx.ControlType == ControlType.Editable ||
					cx.ControlType == ControlType.Readonly)
				{
					switch (cx.PropertyInfo.Name)
					{
						case "Operator":
							return RenderSelectUserControl(cx, dbcontext);
						case "InboundGoods":
							return new GoodsList(dbcontext);
					}
				}
				return ctl;
			};

			//form submit callback
			formControl.SubmitCallback = d =>
			{
				try
				{
					dbcontext.WarehouseInbounds.Add((WarehouseInbound) d);
					dbcontext.SaveChanges();
					WindowMgr.SendNotification("入库成功", NotificationLevel.Information);
					WindowMgr.CloseTabPage(tab);
					dbcontext.Dispose();
				}
				catch (Exception ex)
				{
					//TODO: handle exception
					WindowMgr.SendNotification("入库操作失败", NotificationLevel.Error);
				}
			};

			var newinbound = new WarehouseInbound();
			newinbound.Name = string.Format("入库 - {0}", DateTime.Now);
			formControl.RenderForm(newinbound, false);
			formControl.ConfirmButton.Content = "入库";
			tab.Content = formControl;
		}

		[Command("WAREHOUSE_OUTBOUND", DisplayName = "出库")]
		public void CallWarehouseOutboundPage()
		{
		}

		[Command("WAREHOUSE_BORROW", DisplayName = "借出")]
		public void CallWarehouseBorrowPage()
		{
		}

		[Command("WAREHOUSE_OPERATION_QUERY", DisplayName = "出入库历史查询")]
		public void CallWarehouseOpHistoryPage()
		{
		}

		[Command("WAREHOUSE_RETURN_QUERY", DisplayName = "物品归还查询")]
		public void CallQueryReturnPage()
		{
		}
	}
}