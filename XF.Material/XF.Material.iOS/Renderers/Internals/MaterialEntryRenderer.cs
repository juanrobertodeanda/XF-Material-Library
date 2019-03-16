using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XF.Material.Forms.UI.Internals;
using XF.Material.iOS.Renderers.Internals;

[assembly: ExportRenderer(typeof(MaterialEntry), typeof(MaterialEntryRenderer))]
namespace XF.Material.iOS.Renderers.Internals
{
    internal class MaterialEntryRenderer : EntryRenderer
    {
        private string text;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement == null) return;
            this.Control.BorderStyle = UITextBorderStyle.None;
            this.Control.TintColor = (this.Element as MaterialEntry)?.TintColor.ToUIColor();

            if((Element as MaterialEntry).HasNumberFormat)
            {
                Control.EditingChanged += Control_EditingChanged;
            }

            if (!(Element as MaterialEntry).ShowKeyboard)
            {
                Control.InputView = new UIView();
            }

            if ((Element as MaterialEntry).AlwaysUppercase)
            {
                Control.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
            }

            if ((Element as MaterialEntry).InputType == Forms.UI.MaterialTextFieldInputType.Date)
            {
                var datePicker = new UIDatePicker();
                datePicker.Mode = UIDatePickerMode.Date;
                datePicker.AddTarget(DateTextField, UIControlEvent.ValueChanged);
                datePicker.SetDate((NSDate)DateTime.Now, true);

                if ((Element as MaterialEntry).Date.HasValue)
                {
                    datePicker.Date = (NSDate)(Element as MaterialEntry).Date.Value;
                }

                Control.InputView = datePicker;
                Control.EditingDidBegin += Control_EditingDidBegin;

                var toolBar = new UIToolbar();
                toolBar.BarStyle = UIBarStyle.Default;
                toolBar.Translucent = true;
                toolBar.TintColor = UIColor.Black;
                toolBar.SizeToFit();
                var doneButton = new UIBarButtonItem(title: "Aceptar", style: UIBarButtonItemStyle.Plain, handler: (sender, args) => Control.ResignFirstResponder());
                var spaceButton = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, target: null, action: null);

                toolBar.SetItems(new UIBarButtonItem[] { spaceButton, doneButton }, animated: false);
                toolBar.UserInteractionEnabled = true;

                Control.InputAccessoryView = toolBar;
            }
        }

        private void Control_EditingDidBegin(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Control.Text))
            {
                (Element as MaterialEntry).Date = DateTime.Now;
            }
        }

        private void DateTextField(object sender, EventArgs args)
        {
            var picker = Control.InputView as UIDatePicker;
            var dateFormat = new NSDateFormatter();
            dateFormat.DateFormat = "dd/MM/yyyy";

            var eventDate = DateTime.SpecifyKind((DateTime)picker.Date, DateTimeKind.Utc).ToLocalTime(); ;

            Control.Text = eventDate.ToString("dd/MM/yyyy");
            (Element as MaterialEntry).Date = (DateTime?)eventDate;
        }

        private void Control_EditingChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace((sender as UITextField).Text))
            {
                if ((sender as UITextField).Text.Count(x => x == '.') > 1)
                {
                    var selectedRange = Control.SelectedTextRange;
                    Control.Text = text;
                    Control.SelectedTextRange = selectedRange;
                }
                else if (Control.Text.Last() != '.')
                {
                    var textFieldText = Control.Text.Replace(",", "");
                    var formatter = new NSNumberFormatter();
                    formatter.NumberStyle = NSNumberFormatterStyle.Decimal;
                    formatter.MaximumFractionDigits = 10;
                    var a = double.Parse(textFieldText);
                    var number = new NSNumber(a);
                    var formateOutput = formatter.StringFromNumber(number);
                    Control.Text = formateOutput;
                    text = formateOutput;
                }
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e?.PropertyName == nameof(MaterialEntry.TintColor))
            {
                this.Control.TintColor = (this.Element as MaterialEntry)?.TintColor.ToUIColor();
            }
        }
    }
}