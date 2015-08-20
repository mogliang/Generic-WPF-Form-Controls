using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.DataModel.Entities
{
	public class User
	{
		[Key]
		public int UserId { set; get; }

		[StringLength(10)]
		[Display(Name = "登录名")]
		[Required(ErrorMessage="必填")]
		[Index("username_index",IsUnique=true)]
		public string Username { set; get; }

		[StringLength(100)]
		[Display(Name = "密码")]
		[UIHint("password")]
		[Required(ErrorMessage = "必填")]
		public string Password { set; get; }

		[StringLength(10)]
		[Display(Name = "姓名")]
		[Required(ErrorMessage = "必填")]
		public string Name { set; get; }

		[StringLength(50)]
		[Display(Name = "身份证")]
		[Required(ErrorMessage = "必填")]
		[RegularExpression(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$",ErrorMessage="格式不正确")]
		public string IdentificationNumber { set; get; }

		[StringLength(20)]
		[Display(Name = "电话")]
		[RegularExpression(@"^[0-9]*",ErrorMessage="格式不正确")]
		[Required(ErrorMessage = "必填")]
		public string PhoneNumber { set; get; }

		[StringLength(50)]
		[Display(Name = "公司")]
		public string Company { set; get; }

		[StringLength(50)]
		[Display(Name = "部门")]
		public string Department { set; get; }

		[Display(Name = "权限", Description = "用户的访问权限")]
		public PermissionGroup PermissionGroup { set; get; }

		[Display(Name = "备忘")]
		[StringLength(5000)]
		public string Memo { set; get; }
	}

	public enum PermissionGroup
	{
		[Display(Name = "冻结", Description = "没有任何权限")]
		Guest = 0,
		[Display(Name = "普通用户", Description = "只有查询权限")]
		User = 1,
		[Display(Name = "仓库管理员", Description = "管理入库出库，查询")]
		Operator = 2,
		[Display(Name = "系统管理员", Description = "拥有所有权限")]
		Admin = 3
	}
}
