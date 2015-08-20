using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.Entities
{
	public class Product : INotifyPropertyChanged
	{
		int productid;
		[Key]
		public int ProductId
		{
			set
			{
				productid = value;
				OnPropertyChanged("ProductId");
			}
			get
			{
				return productid;
			}
		}

		[StringLength(100)]
		[Display(Name = "物品扫码")]
		[Index(IsUnique=true)]
		[Required(ErrorMessage = "必填")]
		public string ScanCode { set; get; }

		[StringLength(20)]
		[Display(Name = "物品名称")]
		[Required(ErrorMessage ="必填")]
		public string Name { set; get; }

		//[StringLength(10)]
		//[Display(Name = "计量单位")]
		//[Required(ErrorMessage = "必填")]
		//public string Unit { set; get; }

		[Display(Name = "类型标签")]
		public string Tags { set; get; }

		[Display(Name = "备忘")]
		[StringLength(5000)]
		public string Memo { set; get; }

		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this,
					new PropertyChangedEventArgs(propName));
		}
	}
}
