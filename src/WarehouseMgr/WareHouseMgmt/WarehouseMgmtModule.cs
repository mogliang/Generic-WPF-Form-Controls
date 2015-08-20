using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Warehouse.DataModel;
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr.WarehouseMgmt
{
	[Command("WAREHOUSE_MGMT", DisplayName = "仓库设置")]
	public class WarehouseMgmtModule
	{
		[Command("WAREHOUSE_MAP", DisplayName = "存放位置管理")]
		public void CallWarehouseMapEditPage()
		{
			WarehouseContext dbcontext;
			var warehouseMapEditPage = WindowMgr.CreateTabPageWithDBContext("存放位置管理", "存放位置管理", true, out dbcontext);
			if (warehouseMapEditPage != null)
				warehouseMapEditPage.Content = new WarehouseMap(dbcontext);
		}
	}
}
