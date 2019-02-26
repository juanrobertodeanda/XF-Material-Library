using Xamarin.Forms;

namespace XF.Material.Forms.UI.Internals
{
    /// <summary>
    /// Used for rendering the <see cref="Entry"/> control in <see cref="MaterialTextField"/>.
    /// </summary>
    public class MaterialEntry : Entry
    {
        public static readonly BindableProperty ShowKeyboardProperty = BindableProperty.Create(nameof(ShowKeyboard), typeof(bool), typeof(MaterialEntry), true);
        public static readonly BindableProperty AlwaysUppercaseProperty = BindableProperty.Create(nameof(AlwaysUppercase), typeof(bool), typeof(MaterialEntry), false);
        public static readonly BindableProperty HasNumberFormatProperty = BindableProperty.Create(nameof(HasNumberFormat), typeof(bool), typeof(MaterialEntry), false);
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(MaterialEntry), Material.Color.Secondary);
        public static readonly BindableProperty IsDateTimeProperty = BindableProperty.Create(nameof(IsDateTime), typeof(bool), typeof(MaterialEntry), false);

        internal MaterialEntry() { }

        public bool ShowKeyboard
        {
            get { return (bool)GetValue(ShowKeyboardProperty); }
            set { SetValue(ShowKeyboardProperty, value); }
        }

        public bool AlwaysUppercase
        {
            get => (bool)this.GetValue(AlwaysUppercaseProperty);
            set => this.SetValue(AlwaysUppercaseProperty, value);
        }

        public bool HasNumberFormat
        {
            get => (bool)this.GetValue(HasNumberFormatProperty);
            set => this.SetValue(HasNumberFormatProperty, value);
        }

        public Color TintColor
        {
            get => (Color)this.GetValue(TintColorProperty);
            set => this.SetValue(TintColorProperty, value);
        }

        public bool IsDateTime
        {
            get => (bool)this.GetValue(IsDateTimeProperty);
            set => this.SetValue(IsDateTimeProperty, value);
        }
    }
}
