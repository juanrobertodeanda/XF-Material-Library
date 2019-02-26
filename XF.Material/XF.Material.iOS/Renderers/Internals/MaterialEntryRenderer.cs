using Foundation;
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