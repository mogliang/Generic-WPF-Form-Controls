using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.Entities
{
	public class Metadata
	{
		[Key]
		public int MetadataId { set; get; }

		[Required]
		public string Name { set; get; }
		public string Content { set; get; }
		public string ContentType { set; get; }
	}

	public class OperationHistory
	{
		[Key]
		public int OperationHistoryId { set; get; }
		[StringLength(20)]
		public string Name { set; get; }

		public OperationType ObjectType { set; get; }
		public string Message { set; get; }
		public virtual User Operator { set; get; }
	}

	public enum OperationType
	{
		Other = 0,
		Customer = 1,
		Metadata = 2,
		Product = 3,
		Goods = 4,
		Tag = 5,
		TagGroup = 6,
		User = 7
	}
}
