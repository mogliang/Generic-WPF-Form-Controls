using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.Entities
{
	public class Tag
	{
		[Key]
		public int TagId { set; get; }

		[StringLength(20)]
		[Required(ErrorMessage = "必填")]
		public string Name { set; get; }

		[Editable(false)]
		[Required(ErrorMessage = "必填")]
		public StateCode State { set; get; }

		public virtual TagGroup Group { set; get; }
		//public virtual IList<Product> Products { set; get; }
		//public virtual IList<Goods> Goods { set; get; }
	}

	public enum StateCode
	{
		Active = 0,
		InActive = 1
	}
}
