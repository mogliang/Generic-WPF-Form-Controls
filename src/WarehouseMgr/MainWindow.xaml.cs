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
using WarehouseMgr.Controls;
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
	        InitializeNotificationBar();
			var page = new Dashboard();

			WindowMgr.CreateTabPage("欢迎页", "欢迎页",true, page);
			WindowMgr.RenderContextMenu();


        }

	    Storyboard _storyboard = null;
	    void InitializeNotificationBar()
	    {
			_storyboard = new Storyboard();
			_storyboard.Completed += _storyboard_Completed;

			var animation0 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.1)));
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
			animation3.BeginTime = TimeSpan.FromSeconds(3.5);
			Storyboard.SetTarget(animation3, notify_br);
			Storyboard.SetTargetProperty(animation3, new PropertyPath(Border.OpacityProperty));
			_storyboard.Children.Add(animation3);
	    }

	    private void _storyboard_Completed(object sender, EventArgs e)
	    {
		    if (_animationFinshCallback != null)
			    _animationFinshCallback();
	    }

	    private Action _animationFinshCallback = null;
		public void ShowNotificationMessage(string msg, NotificationLevel msgLevel, Action animationFinishCallback=null)
		{
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

			notify_tb.Text = msg;
			_storyboard.Begin();
			_animationFinshCallback = animationFinishCallback;
		}
    }
}
