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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarehouseMgr.Controls
{
	/// <summary>
	/// Interaction logic for NotifyWindow.xaml
	/// </summary>
	public partial class NotifyContentControl : UserControl
	{
		Storyboard _storyboard;
		public NotifyContentControl()
		{
			InitializeComponent();

			_storyboard = new Storyboard();

			var animation0 = new DoubleAnimation(0.9, new Duration(TimeSpan.FromSeconds(0.1)));
			Storyboard.SetTarget(animation0, notify_br);
			Storyboard.SetTargetProperty(animation0, new PropertyPath(Border.OpacityProperty));
			_storyboard.Children.Add(animation0);

			var animation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.5)));
			animation.EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut };
			Storyboard.SetTarget(animation, notify_br);
			Storyboard.SetTargetProperty(animation, new PropertyPath("(Border.RenderTransform).Y"));
			_storyboard.Children.Add(animation);

			var animation2 = new DoubleAnimation(40, new Duration(TimeSpan.FromSeconds(0.5)));
			animation2.BeginTime = TimeSpan.FromSeconds(3);
			animation2.EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn };
			Storyboard.SetTarget(animation2, notify_br);
			Storyboard.SetTargetProperty(animation2, new PropertyPath("(Border.RenderTransform).Y"));
			_storyboard.Children.Add(animation2);

			var animation3 = new DoubleAnimation(0, new Duration(TimeSpan.Zero));
			animation3.BeginTime = TimeSpan.FromSeconds(5.5);
			Storyboard.SetTarget(animation3, notify_br);
			Storyboard.SetTargetProperty(animation3, new PropertyPath(Border.OpacityProperty));
			_storyboard.Children.Add(animation3);
		}

		public FrameworkElement MyContent
		{
			get { return (FrameworkElement)GetValue(MyContentProperty); }
			set { SetValue(MyContentProperty, value); }
		}

		public static readonly DependencyProperty MyContentProperty =
			DependencyProperty.Register("MyContent", typeof(FrameworkElement), typeof(NotifyContentControl),
			new PropertyMetadata(
				new PropertyChangedCallback((o, e) =>
				{
					var win = o as NotifyContentControl;
					win.contentcontrol.Content = e.NewValue;
				})));

		public void ShowNotificationMessage(string msg, NotificationLevel msgLevel){
			Brush msgBackgorund = null;
			switch (msgLevel)
			{
				case NotificationLevel.Information:
					msgBackgorund = new SolidColorBrush(Colors.Green);
					break;
				case NotificationLevel.Warning:
					msgBackgorund = new SolidColorBrush(Colors.Orange);
					break;
				case NotificationLevel.Error:
					msgBackgorund = new SolidColorBrush(Colors.DarkRed);
					break;
				case NotificationLevel.Critical:
					msgBackgorund = new SolidColorBrush(Colors.Red);
					break;
			}
			notify_br.Background = msgBackgorund;

			notify_tb.Text=msg;
			_storyboard.Begin();
		}
	}

	public enum NotificationLevel
	{
		// green
		Information,
		// orange
		Warning,
		// dark red
		Error,
		// red
		Critical
	}
}
