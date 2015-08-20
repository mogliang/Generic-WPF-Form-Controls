using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.DataModel.Entities
{
	public class WarehouseInbound
	{
		[Key]
		public int WarehouseInboundId { set; get; }

		[StringLength(100)]
		[Display(Name = "标题")]
		[Required(ErrorMessage = "必填")]
		public string Name { set; get; }

		[Required(ErrorMessage = "必填")]
		[Display(Name = "操作者")]
		public virtual User Operator { set; get; }

		[Display(Name = "备忘")]
		[StringLength(5000)]
		public string Memo { set; get; }

		[Display(Name = "货物")]
		public virtual List<Goods> InboundGoods { set; get; }
	}
}