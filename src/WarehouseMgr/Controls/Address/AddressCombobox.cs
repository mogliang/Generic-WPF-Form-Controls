using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Effects;
using WarehouseMgr.Utilites;
using Xceed.Wpf.Toolkit;

namespace WarehouseMgr.Controls
{
	public class AddressCombobox : WatermarkTextBox
	{
		public AddressCombobox()
			: base()
		{
			this.PreviewMouseDown += AddressCombobox_GotFocus;
			this.GotFocus+=AddressCombobox_GotFocus;
			this.IsReadOnly = true;
			this.Watermark = "点击这里选取地址";
		}

		Popup _dropdown = null;
		void AddressCombobox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
		{
			if (_dropdown == null)
			{
				_dropdown = new Popup();
				_dropdown.AllowsTransparency = true;
				_dropdown.MouseLeave += _dropdown_MouseLeave;
				_dropdown.MouseEnter += _dropdown_MouseEnter;
				var addrpop = new AddressPopup();
				addrpop.Effect = new DropShadowEffect
				{
					ShadowDepth = 3
				};
				addrpop.SelectedCallback = (v) =>
				{
					_dropdown.IsOpen = false;
					this.Text = v;
					this.ToolTip = AddressConverter.AddressCodeToAddress(v);
				};
				_dropdown.Child = addrpop;
			}

			var ele = sender as FrameworkElement;
			var pos = ele.PointToScreen(new Point(0, this.ActualHeight));
			_dropdown.VerticalOffset = pos.Y;
			_dropdown.HorizontalOffset = pos.X;
			_dropdown.IsOpen = true;
		}

		bool _willclose = false;
		void _dropdown_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			_willclose = false;
		}

		void _dropdown_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			_dropdown.IsOpen = false;
		}
	}
}
