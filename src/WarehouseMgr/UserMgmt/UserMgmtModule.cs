using FormControlLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;
using WarehouseMgr.Controls;
using WarehouseMgr.Infrastructure;
using WarehouseMgr.Utilites;

namespace WarehouseMgr.UserMgmt
{
	[Command("USER_MGMT",DisplayName="用户管理")]
	public class UserMgmtModule
	{
		[Command("USER_NEW", DisplayName = "创建用户")]
		public void CallCreateUserTabPage()
		{
			var tab = WindowMgr.CreateTabPage("创建新用户", "创建新用户",false);
			var form = CallCreateUserControl((u) =>
			{
				WindowMgr.SendNotification("用户创建成功",NotificationLevel.Information);
				WindowMgr.CloseTabPage(tab);
			});
			tab.Content = form;
		}

		[Command("USER_EDIT_DELETE", DisplayName = "管理用户")]
		public void OpenManageUsersTabPage()
		{
			var usermgmtctl = new ManageUsers();
			WindowMgr.CreateTabPage("管理用户", "管理用户", true, usermgmtctl);
		}

		public Control CallCreateUserControl(Action<User> createCallback=null)
		{
			var formControl = FormControlHelper.CreateFormControl();
			formControl.RenderForm(new User(),false);
			formControl.ConfirmButton.Content = "创建用户";
			formControl.SubmitCallback = (d) =>
			{
				using (var dbcontext = new WarehouseContext())
				{
					try
					{
						dbcontext.Users.Add((User)d);
						dbcontext.SaveChanges();
						if (createCallback != null)
							createCallback((User)d);
					}
					catch (Exception ex)
					{
						//TODO: handle exception
						formControl.SetErrorMsgManually("Username", "此用户名已存在，请更换");
					}
				}
			};

			return formControl;
		}

		bool CallResetPasswordPopup(User user)
		{
			var pwdwindow=new ResetPwdWindow(user);
			var result = pwdwindow.ShowDialog();
			if (result.HasValue && result.Value)
			{
				WindowMgr.SendNotification("密码已重置", NotificationLevel.Information);
				return true;
			}
			return false;
		}

		FrameworkElement CreateResetPwdButton(User user)
		{
			var container = new StackPanel
			{
				Orientation = Orientation.Horizontal
			};
			var resetpwdBn = new Button
			{
				Content = "重置密码",
				Style = Application.Current.Resources["confirm_bn"] as Style
			};
			container.Children.Add(resetpwdBn);
			var msgtb = new TextBlock
			{
				Style = Application.Current.Resources["succeed_TextBlock"] as Style
			};
			container.Children.Add(msgtb);
			resetpwdBn.Click += (s, e) =>
			{
				CallResetPasswordPopup(user);
			};
			CustomValidation.SetValidationCallback(container, () => null);
			return container;
		}

		public Control CallViewUserControl(User user)
		{
			var formControl = FormControlHelper.CreateFormControl();
			// remove password
			formControl.DetermineFieldCreationCallback = (cx, s) =>
			{
				switch (cx.PropertyInfo.Name)
				{
					case "Username":
						return true;
					case "Password":
						return false;
					default:
						return s;
				}
			};
			formControl.RenderForm(user, true);
			return formControl;
		}

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

			formControl.SubmitCallback = (d) =>
			{
				using (var dbcontext = new WarehouseContext())
				{
					try
					{
						var selectuser = d as User;
						var updateuser = dbcontext.Users.Find(selectuser.UserId);
						updateuser.Name = selectuser.Name;
						updateuser.PhoneNumber = selectuser.PhoneNumber;
						updateuser.Memo = selectuser.Memo;
						updateuser.IdentificationNumber = selectuser.IdentificationNumber;
						updateuser.PermissionGroup = selectuser.PermissionGroup;
						updateuser.Department = selectuser.Department;
						dbcontext.SaveChanges();
						WindowMgr.SendNotification("更新用户成功", NotificationLevel.Information);
						if (updateCallback != null)
							updateCallback(selectuser);
					}
					catch (Exception ex)
					{
						// TODO
						WindowMgr.SendNotification("更新用户失败",NotificationLevel.Error);
					}
				}
			};

			formControl.RenderForm(user, false);
			formControl.ConfirmButton.Content = "保存设置";

			return formControl;
		}
	}
}
