using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;

namespace WarehouseMgr.UserMgmt
{
	/// <summary>
	/// Interaction logic for ResetPwdWindow.xaml
	/// </summary>
	public partial class ResetPwdWindow : Window
	{
		public ResetPwdWindow()
		{
			InitializeComponent();
		}

		User UpdateUser { set; get; }
		public ResetPwdWindow(User user)
			: this()
		{
			UpdateUser = user;
			this.DataContext = user;
		}

		private void pwd_changed(object sender, RoutedEventArgs e)
		{
			string errmsg="密码强度不够。\n";
			if (string.IsNullOrEmpty(pwdbox1.Password) ||
				pwdbox1.Password.Length < 6)
			{
				err_tb1.Text = errmsg;
			}
			else
				err_tb1.Text = null;
		}

		private void pwd2_changed(object sender, RoutedEventArgs e)
		{
			string errmsg = "两次密码输入不一致。\n"; ;
			if (pwdbox1.Password != pwdbox2.Password)
			{
				err_tb2.Text = errmsg;
			}
			else
				err_tb2.Text = null;
		}

		private void confirm_click(object sender, RoutedEventArgs e)
		{
			pwd_changed(null, null);
			pwd2_changed(null, null);
			if (string.IsNullOrEmpty(err_tb1.Text)
				&& string.IsNullOrEmpty(err_tb2.Text))
			{
				try
				{
					ResetPassword();
					this.DialogResult = true;
					this.Close();
				}
				catch (Exception ex)
				{
					err_tb1.Text = "数据库连接失败，请重试。";
				}
			}
		}

		void ResetPassword()
		{
			using (var dbcontext = new WarehouseContext())
			{
				var updateuser = dbcontext.Users.Find(UpdateUser.UserId);
				if (updateuser != null)
				{
					updateuser.Password = pwdbox1.Password;
				}
				dbcontext.SaveChanges();
			}
		}
	}
}
