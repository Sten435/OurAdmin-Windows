using Domein.Controllers;
using Domein.DataBase.DataTable;
using GUI.Views.ViewClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GUI.ViewModels.Pages
{
	public class TablesViewModel : ViewModelBase
	{
		public TablesViewModel() => Tables = new(DomeinController.GetTables().Select(table => new DatabaseTableData(new(table))).ToList());
		public ObservableCollection<DatabaseTableData> Tables { get; set; }

	}
}
