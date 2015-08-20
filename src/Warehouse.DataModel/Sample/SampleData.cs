using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.DataModel.Entities;

namespace Warehouse.DataModel.Sample
{
	public class SampleData
	{
		public static void CleanDB()
		{
			using (var dbcontext = new WarehouseContext())
			{
			}
		}

		public static void AddSampleData()
		{
			using (var dbcontext = new WarehouseContext())
			{
				try
				{
					var user1 = new User()
					{
						Name = "二娃",
						Username = "dawang",
						Password = "123456",
						IdentificationNumber = "310392198305114344",
						PhoneNumber = "13543776409",
						Company = "梁山",
						Department = "库房管理部",
						Memo = "(●'◡'●)"
					};
					dbcontext.Users.Add(user1);

					var tg = new TagGroup()
					{
						Name = "功能"
					};
					dbcontext.TagGroups.Add(tg);

					var tag1 = new Tag()
					{
						Name = "食品",
						Group = tg
					};
					dbcontext.Tags.Add(tag1);

					var tag2 = new Tag()
					{
						Name = "书籍",
						Group = tg
					};
					dbcontext.Tags.Add(tag2);

					var tag3 = new Tag()
					{
						Name = "酒水",
						Group = tg
					};
					dbcontext.Tags.Add(tag3);

					var product = new Product()
					{
						Name = "乐事薯片100g（清新黄瓜）",
						ScanCode = "111"
					};
					dbcontext.Products.Add(product);

					var good = new Goods()
					{
						GoodsCode = "111",
						InboundDate = DateTime.Now,
						Product = product,
						State = GoodsState.Inbounding
					};
					dbcontext.Goods.Add(good);

					dbcontext.SaveChanges();
				}
				catch
				{
					// do nothing
				}
			}
			using (var dbcontext = new WarehouseContext())
			{
				var item = dbcontext.Goods.First();
			}
		}
	}
}
