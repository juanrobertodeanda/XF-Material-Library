using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Text;
using Android.Views.InputMethods;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XF.Material.Droid.Renderers.Internals;
using XF.Material.Droid.Utilities;
using XF.Material.Forms.UI.Internals;
using static Android.Views.View;

[assembly: ExportRenderer(typeof(MaterialEntry), typeof(MaterialEntryRenderer))]
namespace XF.Material.Droid.Renderers.Internals
{
    internal class MaterialEntryRenderer : EntryRenderer
    {
        DatePickerDialog dialog;

        public MaterialEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement == null) return;
            this.ChangeCursorColor();

            this.Control.Background = new ColorDrawable(Color.Transparent.ToAndroid());
            this.Control.SetPadding(0, 0, 0, 0);
            this.Control.SetIncludeFontPadding(false);

            if ((Element as MaterialEntry).HasNumberFormat)
            {
                Control.AddTextChangedListener(new NumberTextWatcher(Control));
            }

            if ((Element as MaterialEntry).AlwaysUppercase)
            {
                Control.SetFilters(new Android.Text.IInputFilter[] { new InputFilterAllCaps() });
            }

            if ((Element as MaterialEntry).InputType == Forms.UI.MaterialTextFieldInputType.Date)
            {
                this.SetOnClickListener(new OnClickListener());
                this.Control.Click += OnPickerClick;
                this.Control.KeyListener = null;
                this.Control.FocusChange += OnPickerFocusChange;
            }
        }

        void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
                this.Control.FocusChange -= OnPickerFocusChange;

                if (dialog != null)
                {
                    dialog.Hide();
                    dialog.Dispose();
                    dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        void SetDate(DateTime date)
        {
            this.Control.Text = date.ToString("dd/MM/yyyy");
            (Element as MaterialEntry).Date = date;
        }

        private void ShowDatePicker()
        {
            var date = (Element as MaterialEntry).Date;
            if(date is null)
            {
                CreateDatePickerDialog(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            }
            else
            {
                CreateDatePickerDialog(date.Value.Year, date.Value.Month - 1, date.Value.Day);
            }
            dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {
            var view = Element as MaterialEntry;
            dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                dialog = null;
            }, year, month, day);

            dialog.SetButton("Aceptar", (sender, e) =>
            {
                SetDate(dialog.DatePicker.DateTime);
                (Element as MaterialEntry).Date = dialog.DatePicker.DateTime;
            });

            dialog.SetButton2("Cancelar", (sender, e) =>
            {
            });
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e?.PropertyName == nameof(MaterialEntry.TintColor))
            {
                this.ChangeCursorColor();
            }
        }


        private void ChangeCursorColor()
        {
            try
            {
                var field = Java.Lang.Class.FromType(typeof(Android.Widget.TextView)).GetDeclaredField("mCursorDrawableRes");
                field.Accessible = true;
                var resId = field.GetInt(this.Control);

                field = Java.Lang.Class.FromType(typeof(Android.Widget.TextView)).GetDeclaredField("mEditor");
                field.Accessible = true;

                var cursorDrawable = ContextCompat.GetDrawable(this.Context, resId);
                cursorDrawable.SetColorFilter((this.Element as MaterialEntry).TintColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);

                var editor = field.Get(this.Control);
                field = editor.Class.GetDeclaredField("mCursorDrawable");
                field.Accessible = true;
                field.Set(editor, new Drawable[] { cursorDrawable, cursorDrawable });
            }
            catch (Java.Lang.NoSuchFieldException)
            {
                System.Diagnostics.Debug.WriteLine("Cannot change Textfield's cursor color.");
            }
        }
    }

    public class OnClickListener : Java.Lang.Object, IOnClickListener
    {
        public void OnClick(Android.Views.View v)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)v.Context.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(v.WindowToken, 0);
        }
    }
}