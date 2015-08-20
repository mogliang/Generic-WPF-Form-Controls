using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.Entities
{
	public class TagGroup
	{
		[Key]
		public int TagGroupId { set; get; }

		[StringLength(20)]
		[Required(ErrorMessage = "必填")]
		public string Name { set; get; }

		[Editable(false)]
		[Required(ErrorMessage = "必填")]
		public StateCode State { set; get; }

		public virtual List<Tag> Tags { set; get; }
	}
}
