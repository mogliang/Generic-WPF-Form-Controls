using FormControlLib;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;
using WarehouseMgr.Controls;
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr.UserMgmt
{
	/// <summary>
	/// Interaction logic for ManageUsers.xaml
	/// </summary>
	public partial class ManageUsers : UserControl
	{
		public ManageUsers()
		{
			InitializeComponent();
			Loaded += ManageUsers_Loaded;
		}

		void ManageUsers_Loaded(object sender, RoutedEventArgs e)
		{
			ReloadUserGrid();
		}

		private void ReloadUserGrid()
		{
			using (var dbcontext = new WarehouseContext())
			{
				usersdg.ItemsSource = dbcontext.Users.ToList();
			}
		}

		private void DeleteUser_Clicked(object sender, RoutedEventArgs e)
		{
			var data = ((Button)sender).DataContext as User;
			if(data!=null){
				var result = MessageBox.Show(
					string.Format("你是否决定要删除用户'{0}'?", data.Name),
					"操作警告",
					MessageBoxButton.OKCancel,
					MessageBoxImage.Warning);
				if (result == MessageBoxResult.OK)
				{
					try
					{
						using (var dbcontext = new WarehouseContext())
						{
							var deluser = dbcontext.Users.Find(((User)data).UserId);
							dbcontext.Users.Remove(deluser);
							dbcontext.SaveChanges();
							edit_br.Child = null;
							ReloadUserGrid();
						}
					}
					catch (Exception ex)
					{
						WindowMgr.SendNotification("删除操作失败。", NotificationLevel.Error);
					}
				}
			}
		}

		private void UpdateUser_Clicked(object sender, RoutedEventArgs e)
		{
			var usermgmt = new UserMgmtModule();
			var data = ((Button)sender).DataContext;
			var form = usermgmt.CallUpdateUserControl((User)data, (u) =>
			{
				ShowUserView(u);
			});

			edit_br.Child = form;
		}

		private void usersdg_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				var user = e.AddedItems[0] as User;
				ShowUserView(user);
			}
		}

		private void ShowUserView(User user)
		{
			var usermgmt = new UserMgmtModule();
			var form = usermgmt.CallViewUserControl(user);
			edit_br.Child = form;
		}
	}
}
