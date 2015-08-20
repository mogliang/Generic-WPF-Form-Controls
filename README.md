#Generic WPF Form Controls Library
A WPF control library, provides a set of controls which can create edit form or query control based on target entity’s annotation attributes. It can work best with Entity framework code first design, see entity below:
	
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

Use Form control, it will create edit view below dynamically
![Entity Form view](/docimages/form.png) 

Use Query Control, it will create view below dynamically
![Entity search view](/docimages/query.png) 

##Entity annotation sheet

|Attribute	|Function	|
|-----------|-----------|
|[Key]|Control will not generate input row for field with this attribute|
|[Editable(AllowEdit=false)]|Control will not generate input row for field with this attribute|
|[Display(Name = "filed display name")]|Set input row’s display name|
|[UIHint("password")]|Use password input row for target field|
|All validation attribute|Validate the target field before submit|
|[UIHint("<control name>")]|Reserved for extend input controls|

##Create Edit Form
See sample code below:

	public Control CallUpdateUserControl(User user, Action<User> updateCallback=null)
	{
		var formControl = FormControlHelper.CreateFormControl();
		// remove password
		formControl.DetermineFieldCreationCallback = (cx, s) =>
		{
			switch (cx.PropertyInfo.Name)
			{
				case "Username":
					return true;
				default:
					return s;
			}
		};
		// replace password edit control
		formControl.CreateControlCallback = (cx, c) =>
		{
			if (cx.ControlType == ControlType.Editable)
			{
				if (cx.PropertyInfo.Name == "Password")
				{
					return CreateResetPwdButton(user);
				}
				else if (cx.PropertyInfo.Name == "Username")
				{
					c.IsEnabled = false;
				}
			}
			return c;
		};
		// submit callback
		formControl.SubmitCallback = (d) =>
		{
			using (var dbcontext = new WarehouseContext())
			{
				// call EF to save entity
			}
		};

		formControl.RenderForm(user, false);
		// customize submit button
		formControl.ConfirmButton.Content = "保存设置";

		return formControl;
	}

##Create Query Window
Check file [/src/WarehouseMgr/WarehouseOperation/QueryGoods.xaml](/src/WarehouseMgr/WarehouseOperation/QueryGoods.xaml)

##Extend Edit control
Default field Edit controls include textbox, combobox, datepicker and passwordbox, users can extend the control set to meet special needs, see example below:
![Address input control](/docimages/extend.png) 
Check source code under [/src/WarehouseMgr/Controls/Address](/src/WarehouseMgr/Controls/Address) folder
