using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
	public class ObservableProperty<T> : INotifyPropertyChanged
	{
		private T _value;

		public T Value {
			get => _value;
			set {
				if (!_value.Equals(value))
				{
					NotifyPropertyChanged(nameof(Value));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		internal void NotifyPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
