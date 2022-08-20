using GUI.Views;
using System;
using System.Windows;
using System.Windows.Interop;

namespace GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		object defaultDataContent;
		public MainWindow()
		{
			InitializeComponent();
			this.SourceInitialized += new EventHandler(myClass_SourceInitialized);
		}
		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (defaultDataContent == null)
				defaultDataContent = this.Content;

			if (this.WindowState != WindowState.Maximized)
				this.Content = "Full screen only.";
			else if (this.Content != defaultDataContent)
				this.Content = defaultDataContent;
		}

		void myClass_SourceInitialized(object sender, EventArgs e)
		{
			System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(new System.Windows.Interop.WindowInteropHelper(this).Handle);
			source.AddHook(new System.Windows.Interop.HwndSourceHook(WndProc));
		}

		int WM_NCLBUTTONDBLCLK { get { return 0x00A3; } }

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WM_NCLBUTTONDBLCLK)
			{
				handled = true;  //prevent double click from maximizing the window.
			}

			return IntPtr.Zero;
		}

	}
}
