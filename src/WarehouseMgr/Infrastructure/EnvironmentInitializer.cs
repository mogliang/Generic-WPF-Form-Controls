using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.DataModel.SqliteDB;

namespace WarehouseMgr.Infrastructure
{
	internal class EnvironmentInitializer
	{
		internal static void CheckAndInitializeSQLiteDB()
		{
			var dbstring = ConfigurationManager.ConnectionStrings["azuredb"];
			if (dbstring.ProviderName.Contains("System.Data.SQLite"))
			{
				var para = dbstring.ConnectionString.Split(';').First(s => s.StartsWith("Data Source="));
				var path = para.Replace("Data Source=", "");

				SqliteDBInitializer.CopyEmptyDBIfNotExist(path);
			}
		}
	}
}
