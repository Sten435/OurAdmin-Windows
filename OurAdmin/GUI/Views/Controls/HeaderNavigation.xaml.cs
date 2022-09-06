using GUI.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.Views.Controls
{
	/// <summary>
	/// Interaction logic for HeaderNavigation.xaml
	/// </summary>
	public partial class HeaderNavigation : UserControl
	{
		public HeaderNavigation()
		{
			InitializeComponent();
			this.DataContext = ViewModelBase.Instance;
		}

		private void CanUserNavigateThisTab(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null && !button.IsHitTestVisible)
			{
				button.Opacity = .4;
				button.Cursor = Cursors.No;
			} else
			{
				button.Opacity = 1;
				button.Cursor = Cursors.Hand;
			}
		}
	}
}