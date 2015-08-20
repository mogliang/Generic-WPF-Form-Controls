using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;

namespace WarehouseMgr.Sample
{
	public class SampleDataInitializer
	{
		static void Initialize()
		{
			using (var dbcontext = new WarehouseContext())
			{
				var product = new Product
				{
					Name = "水果机7S",
					ScanCode = "111",
					Memo = "我们要做地球人都买得起的水果机"
				};
			}
		}
	}
}
