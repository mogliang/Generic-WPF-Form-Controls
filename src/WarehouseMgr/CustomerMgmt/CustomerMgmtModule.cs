using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr.CustomerMgmt
{
		[Command("CUSTOMER_MGMT", DisplayName = "客户管理")]
	class CustomerMgmtModule
	{
			[Command("CUSTOMER_ADD", DisplayName = "添加客户")]
			public void CallAddCustomerPage()
			{

			}

			public void CallAddCustomerControl()
			{

			}

			[Command("CUSTOMER_EDIT_MGMT", DisplayName = "管理客户")]
			public void CallEditCustomerPage()
			{

			}
	}
}
