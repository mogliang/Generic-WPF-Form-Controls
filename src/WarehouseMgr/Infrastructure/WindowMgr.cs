using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Warehouse.DataModel;
using WarehouseMgr.Controls;

namespace WarehouseMgr.Infrastructure
{
	public class WindowMgr
	{
		static Dictionary<string, TabItem> _tabdict = new Dictionary<string, TabItem>();
		static Random _random = new Random(int.MaxValue);
		public static TabItem CreateTabPage(string name, object title, bool isUnique, FrameworkElement content = null)
		{
			var mainwindow = Application.Current.MainWindow as MainWindow;
			var tab = new TabItem();

			// unique check
			if (isUnique)
			{
				if (_tabdict.ContainsKey(name))
				{
					ActivateTabPage(_tabdict[name]);
					return null;
				}else
				{
					_tabdict[name] = tab;
				}
			}

			var headercontent = new ContentControl
			{
				Content = title
			};
			var tabclosebn = new Button
			{
				Style = Application.Current.Resources["closetab_bn"] as Style
			};
			tabclosebn.Click += (s, e) =>
			{
				CloseTabPage(tab);
			};
			var header = new StackPanel
			{
				Orientation = Orientation.Horizontal
			};
			header.Children.Add(headercontent);
			header.Children.Add(tabclosebn);
			tab.Header = header;

			tab.Content = content;
			tab.Style = Application.Current.Resources["maintab_tab"] as Style;

			mainwindow.tabHost.Items.Add(tab);
			mainwindow.tabHost.SelectedIndex = mainwindow.tabHost.Items.Count - 1;
			return tab;
		}

		public static TabItem CreateTabPageWithDBContext(string name, object title, bool isUnique, out WarehouseContext dbContext, FrameworkElement content = null)
		{
			var tab = CreateTabPage(name, title, isUnique, content);
			if (tab == null)
			{
				dbContext = null;
				return tab;
			}

			dbContext=new WarehouseContext();
			tab.Tag = dbContext;
			return tab;
		}

		public static void ActivateTabPage(TabItem tab)
		{
			var mainwindow = Application.Current.MainWindow as MainWindow;
			mainwindow.tabHost.SelectedItem = tab;
		}

		//public static void CloseTabPage(string id)
		//{
		//	var mainwindow = Application.Current.MainWindow as MainWindow;
		//	foreach (TabItem item in mainwindow.tabHost.Items)
		//	{
		//		if (item.Name == id)
		//		{
		//			mainwindow.tabHost.Items.Remove(item);
		//			break;
		//		}
		//	}
		//}

		public static void CloseTabPage(TabItem tab)
		{
			foreach (var kv in _tabdict)
			{
				if (kv.Value == tab)
				{
					_tabdict.Remove(kv.Key);
					break;
				}
			}

			var mainwindow = Application.Current.MainWindow as MainWindow;
			var dbcontext = tab.Tag as WarehouseContext;
			if (dbcontext != null)
			{
				try
				{
					dbcontext.Dispose();
				}
				catch (Exception ex)
				{
					//TODO
					WindowMgr.SendNotification("数据库连接关闭过程中出现错误，自动处理请忽略",NotificationLevel.Warning);
				}
			}
			mainwindow.tabHost.Items.Remove(tab);
		}

		public static void CreatePopup(string name, string title, FrameworkElement content)
		{
			Window window = content as Window;
			if (window == null)
			{
				window = new Window();
				window.Width = 400;
				window.Height = 600;
				window.Title = title;
				window.Content = content;
			}
			window.ShowDialog();
		}

		public static void SendNotification(string message, NotificationLevel level)
		{
			var mainwindow = Application.Current.MainWindow as MainWindow;
			mainwindow.ShowNotificationMessage(message, level, () =>
			{
				Uri imageuri = null;
				switch (level)
				{
					case NotificationLevel.Information:
						imageuri = new Uri("pack://application:,,,/WarehouseMgr;component/Images/success_16px.png");
						break;
					case NotificationLevel.Warning:
						imageuri = new Uri("pack://application:,,,/WarehouseMgr;component/Images/alert_16px.png");
						break;
					case NotificationLevel.Error:
						imageuri = new Uri("pack://application:,,,/WarehouseMgr;component/Images/error_16px.png");
						break;
					case NotificationLevel.Critical:
						imageuri = new Uri("pack://application:,,,/WarehouseMgr;component/Images/error_16px.png");
						break;
				}
				var img = new BitmapImage();
				img.BeginInit();
				img.UriSource = imageuri;
				img.EndInit();
				mainwindow.status_img.Source = img;
				mainwindow.status_tblock.Text = message;
			});
		}

		public static void RenderContextMenu()
		{
			var mainwindow = Application.Current.MainWindow as MainWindow;
			mainwindow.mainMenu.Items.Clear();
			mainwindow.mainMenu.Items.Add(CreateMenuItem(typeof(UserMgmt.UserMgmtModule)));
			mainwindow.mainMenu.Items.Add(CreateMenuItem(typeof(ProductOperation.ProductMgmtModule)));
			mainwindow.mainMenu.Items.Add(CreateMenuItem(typeof(WarehouseMgmt.WarehouseMgmtModule)));
			mainwindow.mainMenu.Items.Add(CreateMenuItem(typeof(WarehouseOperation.WarehouseOperationModule)));
			mainwindow.mainMenu.Items.Add(CreateMenuItem(typeof(CustomerMgmt.CustomerMgmtModule)));
		}


		static MenuItem CreateMenuItem(Type type)
		{
			var displayname = CommandAttribute.GetCommandDisplayName(type);
			var item = new MenuItem();
			item.Header = displayname;

			var methods = from m in type.GetMethods()
						  where CommandAttribute.GetCommandDisplayName(m) != null
						  select m;

			foreach (var m in methods)
			{
				item.Items.Add(CreateMenuItem2(m));
			}
			return item;
		}
		static MenuItem CreateMenuItem2(MethodInfo member)
		{
			var displayname = CommandAttribute.GetCommandDisplayName(member);

			var item = new MenuItem();
			item.Header = displayname;
			item.Click += (s, e) =>
			{
				var ins = Activator.CreateInstance(member.DeclaringType);
				member.Invoke(ins, new object[] { });
			};

			return item;
		}
	}
}
