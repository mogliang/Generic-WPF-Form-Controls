using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Warehouse.DataModel;
using Warehouse.DataModel.Entities;
using WarehouseMgr.Controls;
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr.ProductOperation
{
	/// <summary>
	///     Interaction logic for ManageTags.xaml
	/// </summary>
	public partial class ManageTags : UserControl
	{
		private TagEditMode editmode = TagEditMode.AddTagGroup;
		private bool isrendered;

		public ManageTags()
		{
			InitializeComponent();
			Loaded += ManageTags_Loaded;
		}

		public ManageTags(WarehouseContext dbcontext)
			: this()
		{
			DBContext = dbcontext;
		}

		public WarehouseContext DBContext { set; get; }

		private void ManageTags_Loaded(object sender, RoutedEventArgs e)
		{
			if (!isrendered)
			{
				isrendered = true;
				RenderTreeview();
				treeview_selectionchanged(null, null);
				ShowFormGrid(false);
			}
		}

		private void RenderTreeview()
		{
			tagtree.Items.Clear();

			var taggroups = DBContext.TagGroups.Include("Tags");
			foreach (var tg in taggroups)
			{
				if (tg.State == StateCode.Active)
				{
					var node = new TreeViewItem();
					node.Style = Application.Current.Resources["treelevel1_item"] as Style;
				node.DataContext = tg;
				foreach (var tag in tg.Tags)
				{
						if (tag.State == StateCode.Active)
						{
							var subnode = new TreeViewItem();
							subnode.Style = Application.Current.Resources["treelevel2_item"] as Style;
					subnode.DataContext = tag;
					node.Items.Add(subnode);
				}
					}
				tagtree.Items.Add(node);
			}
		}
		}

#region Tag opration
		private void AddTagNode(Tag tag)
		{
			foreach (TreeViewItem item in tagtree.Items)
			{
				var curtag = item.DataContext as TagGroup;
				if (curtag == tag.Group)
				{
					var subnode = new TreeViewItem();
					subnode.Style = Application.Current.Resources["treelevel2_item"] as Style;
					subnode.DataContext = tag;
					item.Items.Add(subnode);
				}
			}
		}

		private void CreateTag(Tag newtag)
		{
			var findtag = DBContext.Tags.FirstOrDefault((t) => t.Name == newtag.Name && t.State == StateCode.Active);
			if (findtag != null)
			{
				WindowMgr.SendNotification("标签不能重名", NotificationLevel.Warning);
			}
			else
			{
				DBContext.Tags.Add(newtag);
			DBContext.SaveChanges();
				WindowMgr.SendNotification("成功创建标签", NotificationLevel.Information);
				AddTagNode(newtag);
				ShowFormGrid(false);
		}
		}

		private void UpdateTag(Tag updatetag)
		{
			var findtag = DBContext.Tags.FirstOrDefault((t) => t.Name == updatetag.Name && t.State == StateCode.Active);
			if (findtag != null)
			{
				WindowMgr.SendNotification("标签不能重名", NotificationLevel.Warning);
			}
			else
			{
			DBContext.SaveChanges();
				WindowMgr.SendNotification("成功更改标签", NotificationLevel.Information);
				ShowFormGrid(false);
		}
		}
#endregion

#region TagGroup Operation
		private void AddTagGroupNode(TagGroup tg)
		{
			var node = new TreeViewItem();
			node.Style = Application.Current.Resources["treelevel1_item"] as Style;
			node.DataContext = tg;
			tagtree.Items.Add(node);
		}

		private void CreateTagGroup(TagGroup newtg)
		{
			var findtg = DBContext.TagGroups.FirstOrDefault((g) => g.Name == newtg.Name && g.State == StateCode.Active);
			if (findtg != null)
			{
				WindowMgr.SendNotification("标签组不能重名", NotificationLevel.Warning);
			}
			else
			{
				try
				{
					DBContext.TagGroups.Add(newtg);
					DBContext.SaveChanges();
					WindowMgr.SendNotification("成功创建标签组", NotificationLevel.Information);
					AddTagGroupNode(newtg);
					ShowFormGrid(false);
				}
				catch (Exception ex)
				{
					// TODO
					WindowMgr.SendNotification("操作失败",NotificationLevel.Error);
				}
			}
		}

		private void UpdateTagGroup(TagGroup updatetg)
		{
			var findtg = DBContext.TagGroups.FirstOrDefault((g) => g.Name == updatetg.Name && g.State == StateCode.Active);
			if (findtg != null)
			{
				WindowMgr.SendNotification("标签组不能重名", NotificationLevel.Warning);
			}
			else
			{
				try
				{
					DBContext.SaveChanges();
					WindowMgr.SendNotification("成功更改标签组", NotificationLevel.Information);
					ShowFormGrid(false);
				}
				catch (Exception ex)
		{
					// TODO
					WindowMgr.SendNotification("操作失败", NotificationLevel.Error);
				}
			}
		}
#endregion

		private void confirm_click(object sender, RoutedEventArgs e)
		{
			switch (editmode)
			{
				case TagEditMode.AddTagGroup:
					CreateTagGroup(formgrid.DataContext as TagGroup);
					break;
				case TagEditMode.AddTag:
					CreateTag(formgrid.DataContext as Tag);
					break;
				case TagEditMode.EditTagGroup:
					UpdateTagGroup(formgrid.DataContext as TagGroup);
					break;
				case TagEditMode.EditTag:
					UpdateTag(formgrid.DataContext as Tag);
					break;
			}
		}

		private void cancel_click(object sender, RoutedEventArgs e)
		{
			ShowFormGrid(false);
		}

		private void ShowFormGrid(bool showform)
		{
			if (showform)
			{
				commandpanel.Visibility = Visibility.Collapsed;
				formgrid.Visibility = Visibility.Visible;
				name_tb.Focus();
			}
			else
			{
				commandpanel.Visibility = Visibility.Visible;
				formgrid.Visibility = Visibility.Collapsed;
		}
		}

		private void treeview_selectionchanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			foreach (FrameworkElement fe in commandpanel.Children)
				fe.Visibility = Visibility.Collapsed;
			addtaggroup_bn.Visibility = Visibility.Visible;

			if (tagtree.SelectedItem != null)
			{
				var data = ((TreeViewItem) tagtree.SelectedItem).DataContext;
				if (data is TagGroup)
				{
					addtag_bn.Visibility = Visibility.Visible;
					deletetaggroup_bn.Visibility = Visibility.Visible;
					edittaggroup_bn.Visibility = Visibility.Visible;
				}
				else if (data is Tag)
				{
					deletetag_bn.Visibility = Visibility.Visible;
					edittag_bn.Visibility = Visibility.Visible;
				}
			}
				}

		private void addtaggroup_click(object sender, RoutedEventArgs e)
		{
			editmode = TagEditMode.AddTagGroup;
			var taggroup = new TagGroup();
			formgrid.DataContext = taggroup;
			ShowFormGrid(true);
			}

		private void addtag_click(object sender, RoutedEventArgs e)
		{
			editmode = TagEditMode.AddTag;
			var tag = new Tag();
			var selectedgroup = ((TreeViewItem)tagtree.SelectedItem).DataContext as TagGroup;
			tag.Group = selectedgroup;
			formgrid.DataContext = tag;
			ShowFormGrid(true);
		}

		private void deltag_click(object sender, RoutedEventArgs e)
		{
			var data = ((TreeViewItem) tagtree.SelectedItem).DataContext as Tag;
			data.State = StateCode.InActive;
			DBContext.SaveChanges();
			var tgnode = ((TreeViewItem) tagtree.SelectedItem).Parent as TreeViewItem;
			tgnode.Items.Remove(tagtree.SelectedItem);
			WindowMgr.SendNotification("标签已删除", NotificationLevel.Information);
		}

		private void deltaggroup_click(object sender, RoutedEventArgs e)
		{
			var data = ((TreeViewItem) tagtree.SelectedItem).DataContext as TagGroup;
			if (data.Tags.Count(t => t.State == StateCode.Active) > 0)
			{
				WindowMgr.SendNotification("标签组内还有标签，不能删除", NotificationLevel.Warning);
				return;
			}
			data.State = StateCode.InActive;
			DBContext.SaveChanges();
			tagtree.Items.Remove(tagtree.SelectedItem);
			WindowMgr.SendNotification("标签组已删除", NotificationLevel.Information);
		}

		private void edittag_click(object sender, RoutedEventArgs e)
		{
			editmode = TagEditMode.EditTag;
			var data = ((TreeViewItem) tagtree.SelectedItem).DataContext as Tag;
			formgrid.DataContext = data;
			ShowFormGrid(true);
		}

		private void editgroup_click(object sender, RoutedEventArgs e)
		{
			editmode = TagEditMode.EditTagGroup;
			var data = ((TreeViewItem) tagtree.SelectedItem).DataContext as TagGroup;
			formgrid.DataContext = data;
			ShowFormGrid(true);
		}

		internal enum TagEditMode
		{
			AddTagGroup,
			EditTagGroup,
			AddTag,
			EditTag
		}
	}
}