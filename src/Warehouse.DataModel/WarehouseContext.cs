using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.DataModel.Entities;

namespace Warehouse.DataModel
{
	public class WarehouseContext:DbContext
	{
		public WarehouseContext() :
			base("azureDb")
		{
		}
		public DbSet<User> Users { set; get; }
		public DbSet<Customer> Customers { set; get; }
		public DbSet<Goods> Goods { set; get; }
		public DbSet<Product> Products { set; get; }
		public DbSet<WarehouseInbound> WarehouseInbounds { set; get; }
		public DbSet<Tag> Tags { set; get; }
		public DbSet<TagGroup> TagGroups { set; get; }
		public DbSet<Metadata> Metadatas { set; get; }
	}
}
