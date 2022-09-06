using Domein.Controllers;
using Domein.DataBase.DataTable;
using Domein.Validatie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class NewColumnViewModel
	{
		ViewModelBase ViewModelBase = ViewModelBase.Instance;
		public static bool OnlyChangeColumn = false;

		#region Propperties

		private static string _name;
		public static string Name {
			get {
				return _name;
			}
			set {
				if (value == _name)
					return;
				_name = value;
			}
		}
		private static string _type;
		public static string Type {
			get {
				return _type;
			}
			set {
				if (value == _type)
					return;
				_type = value;
			}
		}
		private static string _lengthValues;
		public static string LengthValues {
			get {
				return _lengthValues;
			}
			set {
				if (value == _lengthValues)
					return;
				_lengthValues = value;
			}
		}
		private static string _default;
		public static string Default {
			get {
				return _default;
			}
			set {
				if (value == _default)
					return;
				_default = value;
			}
		}
		private static string _collation;
		public static string Collation {
			get {
				return _collation;
			}
			set {
				if (value == _collation)
					return;
				_collation = value;
			}
		}
		private static string _attribute;
		public static string Attribute {
			get {
				return _attribute;
			}
			set {
				if (value == _attribute)
					return;
				_attribute = value;
			}
		}
		private static string _index;
		public static string Index {
			get {
				return _index;
			}
			set {
				if (value == _index)
					return;
				_index = value;
			}
		}
		private static string _comments;
		public static string Comments {
			get {
				return _comments;
			}
			set {
				if (value == _comments)
					return;
				_comments = value;
			}
		}
		private static string _virtuality;
		public static string Virtuality {
			get {
				return _virtuality;
			}
			set {
				if (value == _virtuality)
					return;
				_virtuality = value;
			}
		}
		private static string _moveColumn;
		public static string Move_Column {
			get {
				return _moveColumn;
			}
			set {
				if (value == _moveColumn)
					return;
				_moveColumn = value;
			}
		}
		private static bool _isNull;
		public static bool IsNull {
			get {
				return _isNull;
			}
			set {
				if (value == _isNull)
					return;
				_isNull = value;
			}
		}
		private static bool _autoIncrement;
		public static bool AutoIncrement {
			get {
				return _autoIncrement;
			}
			set {
				if (value == _autoIncrement)
					return;
				_autoIncrement = value;
			}
		}
		private static string _asDefined;
		public static string AsDefined {
			get {
				return _asDefined;
			}
			set {
				if (value == _asDefined)
					return;
				_asDefined = value;
			}
		}

		#endregion

		#region Combobox

		private List<string> _types = DomeinController.GetServerTypes();
		public List<string> Types {
			get {
				return _types;
			}
		}

		private List<string> _defaults = DomeinController.GetServerDefaults();
		public List<string> Defaults {
			get {
				return _defaults;
			}
		}

		private List<string> _attributes = DomeinController.GetServerAttributes();
		public List<string> Attributes {
			get {
				return _attributes;
			}
		}

		#endregion

		#region Commands

		public RelayCommand<object> AddColumnToTable => new(ColumnToTable);

		#endregion

		private void ColumnToTable(object window)
		{
			Column newColumn = FillColumnWithFields();
			if (Validate.NullOrWhiteSpace(newColumn.Name))
			{
				MessageBox.Show("Name can't be empty.");
				return;
			}

			if (Validate.NullOrWhiteSpace(newColumn.__Type))
			{
				MessageBox.Show("Type can't be empty.");
				return;
			}

			if (!OnlyChangeColumn)
				DomeinController.AddColumnToTable(newColumn, DomeinController.SelectedTable);
			else
				DomeinController.WriteChangeColumnToTable(newColumn, ViewModelBase.SelectedColumn.Name, DomeinController.SelectedTable);

			ViewModelBase.OnPropertyChanged(nameof(ViewModelBase.ColumnStructure));
			ViewModelBase.OnPropertyChanged(nameof(ViewModelBase.Tables));
			(window as Window).Close();
		}

		private Column FillColumnWithFields()
		{
			Column newColumn = new();
			newColumn.Name = Name ?? string.Empty;
			newColumn.__Type = Type ?? string.Empty;
			newColumn.__LengthValues = LengthValues ?? string.Empty;
			newColumn.__DefaultValue = Default ?? string.Empty;
			if (newColumn.__DefaultValue.ToString().ToUpper() == "AS DEFINED")
			{
				newColumn.__AsDefined = AsDefined;
			} else
				newColumn.__AsDefined = string.Empty;

			newColumn.__Attributes = Attribute ?? string.Empty;
			newColumn.__Comments = Comments ?? string.Empty;
			newColumn.IsNull = IsNull;
			newColumn.__AutoIncrement = AutoIncrement;

			return newColumn;
		}
	}
}
