using Domein.Controllers;
using Domein.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class TablesViewModel : ViewModelBase
	{
		public TablesViewModel(DomeinController _domeinController)
		{
			domeinController = _domeinController;

			Server server = new("localhost", new(user: "root", password: ""));
			domeinController.AddServer(server);
			domeinController.OpenConnectionToServer(server);
		}
	}
}
