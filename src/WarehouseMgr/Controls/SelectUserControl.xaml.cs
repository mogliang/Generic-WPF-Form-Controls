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

namespace WarehouseMgr.Controls
{
	/// <summary>
	/// Interaction logic for SelectUserControl.xaml
	/// </summary>
	public partial class SelectUserControl : UserControl
	{
		public SelectUserControl()
		{
			InitializeComponent();
			Loaded += SelectUserControl_Loaded;
		}

		List<User> userlist = null;
		void SelectUserControl_Loaded(object sender, RoutedEventArgs e)
		{
			using (var dbcontext = new WarehouseContext())
			{
				userlist = dbcontext.Users.ToList();
			}
		}

		public User SelectedUser
		{
			get { return (User)GetValue(SelectedUserProperty); }
			set { SetValue(SelectedUserProperty, value); }
		}

		public static readonly DependencyProperty SelectedUserProperty =
			DependencyProperty.Register("SelectedUser", typeof(User), typeof(SelectUserControl), new PropertyMetadata(0));
	
	}
}
