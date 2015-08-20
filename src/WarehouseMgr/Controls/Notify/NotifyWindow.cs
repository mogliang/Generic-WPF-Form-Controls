using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseMgr.Controls
{
	public class NotifyWindow:Window
	{
		NotifyContentControl _notifycontentControl = null;
		public NotifyWindow()
		{
			_notifycontentControl = new NotifyContentControl();
			this.Content = _notifycontentControl;
		}

		public FrameworkElement MyContent
		{
			get { return (FrameworkElement)GetValue(MyContentProperty); }
			set { SetValue(MyContentProperty, value); }
		}

		public static readonly DependencyProperty MyContentProperty =
			DependencyProperty.Register("MyContent", typeof(FrameworkElement), typeof(NotifyWindow),
			new PropertyMetadata(
				new PropertyChangedCallback((o, e) =>
				{
					var win = o as NotifyWindow;
					win._notifycontentControl.MyContent = e.NewValue as FrameworkElement;
				})));

		public void ShowNotificationMessage(string msg, NotificationLevel level=NotificationLevel.Information)
		{
			_notifycontentControl.ShowNotificationMessage(msg, level);
		}
	}
}
