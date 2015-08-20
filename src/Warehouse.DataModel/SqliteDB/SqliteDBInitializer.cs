using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.SqliteDB
{
	public class SqliteDBInitializer
	{
		public static void CopyEmptyDBIfNotExist(string path)
		{
			if (!File.Exists(path))
			{
				var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Warehouse.DataModel.SqliteDB.warehouse.sqlite");
				using (var fs = File.Open(path, FileMode.CreateNew))
				{
					stream.CopyTo(fs);
				}
				stream.Close();
			}
		}
	}
}
