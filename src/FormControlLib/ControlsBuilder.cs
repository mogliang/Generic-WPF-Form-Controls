using FormControlLib.ControlSinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FormControlLib
{
	public class ControlsBuilder
	{
		private object _lock = new object();
		protected List<ControlSinkBase> _sinklist;

		public ControlsBuilder()
		{
			InitializeSinks();
		}

		protected virtual void InitializeSinks()
		{
			lock (_lock)
			{
				_sinklist = new List<ControlSinkBase>();
				_sinklist.Add(new TextBoxSink());
				_sinklist.Add(new DatePickerSink());
				_sinklist.Add(new ComboBoxSink());
				_sinklist.Add(new PasswordSink());
				_sinklist.Add(new PlaceholderSink());
			}
		}

		public void AddSink(ControlSinkBase sink)
		{
			lock (_lock)
			{
				_sinklist.Add(sink);
			}
		}

		public FrameworkElement CreateControl(FormItemContext context)
		{
			MatchResult result = MatchResult.No;
			ControlSinkBase selectedSink = null;

			lock (_lock)
			{
				for (int i = 0; i < _sinklist.Count; i++)
				{
					var tempresult = _sinklist[i].MatchTest(context);
					if (tempresult > result)
					{
						result = tempresult;
						selectedSink = _sinklist[i];
					}
				}
			}

			if (result == MatchResult.No)
				throw new Exception(string.Format("No suitable Control Sink for {0}.{1}", context.DataType.Name, context.PropertyInfo.Name));
			else
			{
				return selectedSink.CreateControl(context);
			}
		}
	}
}
