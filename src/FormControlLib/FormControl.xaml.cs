using FormControlLib.ControlSinks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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

namespace FormControlLib
{
	/// <summary>
	/// Interaction logic for FormControl.xaml
	/// </summary>
	public partial class FormControl : UserControl
	{
		public Button ConfirmButton
		{
			get
			{
				return confirm_bn;
			}
		}

		ControlsBuilder _controlbuilder = new ControlsBuilder();
		public ControlsBuilder InputControlBuilder
		{
			set
			{
				_controlbuilder = value;
			}
			get
			{
				return _controlbuilder;
			}
		}

		private List<FrameworkElement> _errlabels = new List<FrameworkElement>();
		private List<FrameworkElement> _editctls = new List<FrameworkElement>();
		public void RenderForm(object data, bool isreadonly)
		{
			DataContext = data;
			var datatype = data.GetType();
			if (InputControlBuilder == null)
				InputControlBuilder = new ControlsBuilder();

			root.Children.Clear();
			_errlabels.Clear();
			_editctls.Clear();

			var props = datatype.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
			int rownum = 0;
			for (int i = 0; i < props.Count(); i++)
			{
				var fieldcontext = new FormItemContext(data, datatype, props[i], ControlType.None);

				// determine if create field
				bool suggestion = true;
				var attr0 = props[i].GetCustomAttribute(typeof(EditableAttribute)) as EditableAttribute;
				var attr1 = props[i].GetCustomAttribute(typeof(KeyAttribute)) as KeyAttribute;
				if (attr1 != null ||
					(attr0 != null && !attr0.AllowEdit))
				{
					suggestion = false;
				}
				if (DetermineFieldCreationCallback != null)
				{
					suggestion = DetermineFieldCreationCallback(fieldcontext, suggestion);
				}
				if (!suggestion)
				{
					continue;
				}

				// add row
				root.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(0, GridUnitType.Auto)
				});

				var rowcontainer = new StackPanel();
				rowcontainer.Orientation = Orientation.Horizontal;
				Grid.SetRow(rowcontainer,rownum);
				root.Children.Add(rowcontainer);

				// label
				var lbcontext = new FormItemContext(data, datatype, props[i], ControlType.Label);
				Label label = new Label();
				var attr = props[i].GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
				if (attr != null)
				{
					if (!string.IsNullOrEmpty(attr.Name))
					{
						label.Content = attr.Name;
					}
					if (!string.IsNullOrEmpty(attr.Description))
					{
						label.ToolTip = attr.Description;
					}
				}
				if (label.Content == null)
					label.Content = props[i].Name;
				StyleHelper.ApplyStyle(label);
				var labelctl = OnCreateControl(lbcontext, label);
				rowcontainer.Children.Add(labelctl);

				// editable
				var editcontext = new FormItemContext(data, datatype, props[i], ControlType.Editable);
				var editctl = InputControlBuilder.CreateControl(editcontext);
				editctl = OnCreateControl(editcontext, editctl);
				if (isreadonly)
					editctl.IsEnabled = false;
				rowcontainer.Children.Add(editctl);
				_editctls.Add(editctl);

				// error label
				var errtb = new TextBlock();
				if (!isreadonly)
				{
					errtb.Tag = props[i].Name;

					var errbinding = new Binding
					{
						Source = editctl,
						Path = CustomValidation.GetValidationMsgBindingPath(editctl)
					};
					errtb.SetBinding(TextBlock.TextProperty, errbinding);
					StyleHelper.ApplyStyle(errtb, FormControlConstrants.VALIDATION_ERROR_STYLE);
					rowcontainer.Children.Add(errtb);
					_errlabels.Add(errtb);
				}

				if (LayoutControlCallback != null)
					LayoutControlCallback(this.root, label, editctl, errtb);

				rownum++;
			}

			if (isreadonly)
			{
				ConfirmButton.Visibility = Visibility.Collapsed;
			}
		}

		public Action<object> SubmitCallback { set; private get; }

		public Func<FormItemContext, bool, bool> DetermineFieldCreationCallback
		{
			set;
			private get;
		}

		public Func<FormItemContext, FrameworkElement, FrameworkElement> CreateControlCallback
		{
			set;
			private get;
		}

		public Action<Grid, FrameworkElement, FrameworkElement,FrameworkElement> LayoutControlCallback
		{
			set;
			private get;
		}

		FrameworkElement OnCreateControl(FormItemContext context, FrameworkElement ctl)
		{
			if (CreateControlCallback != null)
			{
				return CreateControlCallback(context, ctl);
			}
			else
				return ctl;
		}

		public void SetErrorMsgManually(string property, string errMsg)
		{
			foreach (FrameworkElement ele in _errlabels)
			{
				if (ele!=null
					&& ele.Tag!=null
					&& string.Equals(property,ele.Tag.ToString()))
				{
					((TextBlock)ele).SetCurrentValue(TextBlock.TextProperty, errMsg);
					break;
				}
			}
		}

		public void ClearErrorMsg()
		{
			foreach (FrameworkElement ele in _errlabels)
			{
				((TextBlock)ele).SetCurrentValue(TextBlock.TextProperty, null);
			}
		}

		public string GetValidationErrors()
		{
			StringBuilder errorSummary = new StringBuilder();
			foreach (FrameworkElement ctl in _editctls)
			{
				if (ctl != null)
				{
					errorSummary.Append(CustomValidation.ForceValidation(ctl));
				}
			}
			return errorSummary.ToString();
		}

		void InitializeConfirmButton()
		{
			StyleHelper.ApplyStyle(ConfirmButton, "confirm_bn");
			ConfirmButton.Click += (s, e) =>
			{
				var errs = GetValidationErrors();
				if (string.IsNullOrEmpty(errs) && SubmitCallback != null)
				{
					SubmitCallback(this.DataContext);
				}
			};
		}

		public FormControl()
		{
			InitializeComponent();
			InitializeConfirmButton();
		}
	}
}
