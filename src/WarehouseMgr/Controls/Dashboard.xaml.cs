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
using Warehouse.DataModel.Sample;
using WarehouseMgr.Infrastructure;

namespace WarehouseMgr.Controls
{
	/// <summary>
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	public partial class Dashboard : UserControl
	{
		public Dashboard()
		{
			InitializeComponent();
			EnvironmentInitializer.CheckAndInitializeSQLiteDB();
			SampleData.AddSampleData();

			using (var dbcontext = new WarehouseContext())
			{
				var query = dbcontext.Database.SqlQuery<User>("select * from Users where Name like @p0","%二%");
				var list = query.ToList();
			}
		}
	}
}
