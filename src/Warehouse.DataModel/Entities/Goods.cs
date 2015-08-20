using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.Entities
{
	public class Goods
	{
		[Key]
		public int GoodsId { set; get; }

		//[StringLength(20)]
		//[Display(Name="货物名称")]
		//public string Name { set; get; }

		[StringLength(100)]
		[Display(Name = "货物扫码")]
		[Required(ErrorMessage = "必填")]
		public string GoodsCode { set; get; }

		[StringLength(100)]
		[UIHint("Address")]
		[Display(Name = "存放位置")]
		public string AddressCode { set; get; }

		[Display(Name = "物品名")]
		public string Name { set; get; }

		[Display(Name = "产品")]
		public virtual Product Product { set; get; }

		// default value = product.Tags, can be modified

		[Display(Name = "类型标签")]
		public string Tags { set; get; }

		[Display(Name = "状态")]
		[Editable(false)]
		public GoodsState State { set; get; }

		[Display(Name = "入库日期")]
		[Editable(false)]
		public DateTime? InboundDate { set; get; }

		[Display(Name = "出库日期")]
		[Editable(false)]
		public DateTime? OutboundDate { set; get; }

		[Display(Name = "更新日期")]
		[Editable(false)]
		public DateTime? LastUpdateDate { set; get; }

		[Display(Name = "备忘")]
		[StringLength(5000)]
		public string Memo { set; get; }
	}

	public enum GoodsState
	{
		[Display(Name = "未知", Description = "货物状态异常")]
		Unknown = 0,
		[Display(Name = "入库", Description = "目前在库内")]
		Inbound = 1,
		[Display(Name = "出库", Description = "已经出库")]
		Outbound = 2,
		[Display(Name = "借出", Description = "已经借出")]
		Borrow = 3,
		[Display(Name = "等待入库", Description = "等待入库")]
		Inbounding = 4,
		//[Display(Name = "返还", Description = "被返还")]
		//Return=3,
	}
}
