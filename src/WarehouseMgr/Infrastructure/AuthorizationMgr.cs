using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;

namespace WarehouseMgr.Infrastructure
{
	public class AuthorizationMgr
	{
		public static User CurrentUser { private set; get; }

		public static bool Login(string username, string password)
		{
			return true;
		}

		public static void Logout()
		{
		}

		static List<object> _modulelist = new List<object>();
		//static List<>
		public static void LoadCommandList()
		{
			_modulelist.Add(new UserMgmt.UserMgmtModule());
			_modulelist.Add(new WarehouseMgmt.WarehouseMgmtModule());
			_modulelist.Add(new WarehouseOperation.WarehouseOperationModule());
			_modulelist.Add(new ProductOperation.ProductMgmtModule());
			_modulelist.Add(new CustomerMgmt.CustomerMgmtModule());

			//// TODO, implement authorization
			//JObject permissionjo = Metadata.MetadataUtility.LoadMetadata("Permission");
			//foreach (JObject cmd in permissionjo["Commands"])
			//{
			//	foreach(JObject cmd2 in cmd["Commands"])
			//	{
					
			//	}
			//}

			_modulelist.FirstOrDefault(m =>
			{
				return CommandAttribute.GetCommandName(m.GetType()) == "";
			});
		}
	}
}
