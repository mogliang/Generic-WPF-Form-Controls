using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.DataModel;

namespace WarehouseMgr.Metadata
{
	public class MetadataUtility
	{
		public static JObject LoadMetadata(string name)
		{
			using (var dbcontext = new WarehouseContext())
			{
				var data = dbcontext.Metadatas.FirstOrDefault(
					m => string.Equals(m.Name, name));
				if (data != null)
					return JObject.Parse(data.Content);
				else return null;
			}
		}

		public static void SaveMetadata(string name, JObject jo)
		{
			using (var dbcontext = new WarehouseContext())
			{
				var data = dbcontext.Metadatas.FirstOrDefault(
					m => string.Equals(m.Name, name));
				if (data != null)
				{
					data.Content = jo.ToString();
					dbcontext.SaveChanges();
				}
				else
				{
					var newmeta = new Warehouse.DataModel.Entities.Metadata
					{
						Name = name,
						ContentType = "text/json",
						Content = jo.ToString()
					};
					dbcontext.Metadatas.Add(newmeta);
					dbcontext.SaveChanges();
				}
			}
		}
	}
}
