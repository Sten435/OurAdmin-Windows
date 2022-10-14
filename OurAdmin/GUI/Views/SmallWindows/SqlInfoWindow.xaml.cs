using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using Cursors = System.Windows.Input.Cursors;
using Label = System.Windows.Controls.Label;
using Orientation = System.Windows.Controls.Orientation;

namespace GUI.Views.SmallWindows
{
	/// <summary>
	/// Interaction logic for SqlInfoWindow.xaml
	/// </summary>
	public partial class SqlInfoWindow : Window
	{
		public SqlInfoWindow(List<string> columns, List<string> rows)
		{
			InitializeComponent();
			GenerateLayout(columns, rows);
		}

		private void GenerateLayout(List<string> columns, List<string> rows)
		{
			for (int i = 0; i < rows.Count; i++)
			{
				grid.RowDefinitions.Add(new RowDefinition());

				StackPanel stackPanel = new()
				{
					Orientation = Orientation.Vertical,
					Margin = new Thickness(0, 20, 0, 0)
				};

				Label columnName = new()
				{
					Content = columns[i],
					FontSize = 20,
					FontWeight = FontWeights.Bold,
					Margin = new Thickness(5, 0, 0, 0)
				};

				Label data = new()
				{
					Content = rows[i],
					Margin = new Thickness(5, 0, 0, 0),
					Cursor = Cursors.Hand
				};

				data.MouseDown += Data_MouseDown;

				stackPanel.Children.Add(columnName);
				stackPanel.Children.Add(data);

				if (i == rows.Count - 1)
				{
					stackPanel.Margin = new Thickness(0, 20, 0, 20);
				}

				grid.Children.Add(stackPanel);

				Grid.SetRow(stackPanel, i);
			}

		}

		private void Data_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Notifier notifier = new Notifier(cfg =>
			{
				cfg.PositionProvider = new WindowPositionProvider(
					parentWindow: Application.Current.MainWindow,
					corner: Corner.BottomCenter,
					offsetX: 10,
					offsetY: 10);

				cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
					notificationLifetime: TimeSpan.FromSeconds(3),
					maximumNotificationCount: MaximumNotificationCount.FromCount(5));

				cfg.Dispatcher = Application.Current.Dispatcher;
			});

			string data = ((Label)sender).Content.ToString();
			try
			{
				Clipboard.SetText(data);
				string message = $"{data} copied to clipboard";
				notifier.ShowSuccess(message);
			} catch (Exception)
			{
				notifier.ShowError("Error while pasting result to clipboard");
			}

			Task.Delay(2000).ContinueWith((task) =>
			{
				this.Dispatcher.Invoke(() =>
				{
					notifier.Dispose();
});
			});
		}
	}
}
