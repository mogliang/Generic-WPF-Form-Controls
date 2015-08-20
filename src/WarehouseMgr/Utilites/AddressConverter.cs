using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr.Utilites
{
	public class AddressConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return AddressCodeToAddress((string)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}


		public static string AddressCodeToAddress(string addrcode)
		{
			if (string.IsNullOrEmpty(addrcode))
				return null;

			var mapjo = Metadata.MetadataUtility.LoadMetadata(Constrants.WAREHOUSE_MAP_STORE_STR);
			var codeArr = addrcode.Split('.');
			string addr = "";

			var parentnode = mapjo;
			for (int i = 0; i < codeArr.Length; i++)
			{
				if (parentnode["Children"] != null)
				{
					bool found = false;
					foreach (JObject subnode in (JArray)parentnode["Children"])
					{
						if (addrcode.StartsWith(subnode["ID"].Value<string>()))
						{
							addr += " "+subnode["Name"].Value<string>();
							parentnode = subnode;
							found = true;
							break;
						}
					}

					if (!found)
					{
						addr += " 未知地址";
						return addr;
					}
				}
				else
				{
					addr += " 未知地址";
					return addr;
				}
			}

			return addr;
		}
	}
}
