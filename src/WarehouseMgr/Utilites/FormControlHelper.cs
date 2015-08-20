using FormControlLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseMgr.Controls;

namespace WarehouseMgr.Utilites
{
	public class FormControlHelper
	{
		internal static FormControl CreateFormControl()
		{
			var formctl=new FormControl();
			formctl.InputControlBuilder.AddSink(new AddressSink());
			return formctl;
		}
	}
}
