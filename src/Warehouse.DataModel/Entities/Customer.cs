using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.Entities
{
	public class Customer
	{
		[Key]
		public int CustomerId { set; get; }
		[StringLength(10)]
		public string Name { set; get; }
		[StringLength(50)]
		public string IdentificationNumber { set; get; }
		[StringLength(20)]
		public string PhoneNumber { set; get; }
		[StringLength(50)]
		public string Company { set; get; }
		[StringLength(50)]
		public string Department { set; get; }
		public string Memo { set; get; }
	}
}
