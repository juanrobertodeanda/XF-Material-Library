using Android.Net;
using Android.Text;
using Android.Widget;
using Java.Lang;
using Java.Text;

namespace XF.Material.Droid.Utilities
{
    public class NumberTextWatcher : Object, ITextWatcher
    {
        private DecimalFormat df;
        private DecimalFormat dfnd;
        private bool hasFractionalPart;

        private EditText et;

        public NumberTextWatcher(EditText et)
        {
            df = new DecimalFormat("#,###.####");
            df.DecimalSeparatorAlwaysShown = true;
            dfnd = new DecimalFormat("#,###");
            this.et = et;
            hasFractionalPart = false;
        }

        public void AfterTextChanged(IEditable s)
        {
            et.RemoveTextChangedListener(this);

            try
            {
                int inilen, endlen;
                inilen = et.Text.Length;

                string v = s.ToString().Replace(df.DecimalFormatSymbols.GroupingSeparator.ToString(), "");
                if (!string.IsNullOrWhiteSpace(v))
                {
                    Number n = df.Parse(v);
                    int cp = et.SelectionStart;
                    if (hasFractionalPart)
                    {
                        et.Text = (df.Format(n));
                    }
                    else
                    {
                        et.Text = (dfnd.Format(n));
                    }
                    endlen = et.Text.Length;
                    int sel = (cp + (endlen - inilen));
                    if (sel > 0 && sel <= et.Text.Length)
                    {
                        et.SetSelection(sel);
                    }
                    else
                    {
                        et.SetSelection(et.Text.Length - 1);
                    }
                }
            }
            catch (NumberFormatException nfe)
            {
            }
            catch (Java.Text.ParseException e)
            {
            }

            et.AddTextChangedListener(this);
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (s.ToString().Contains(df.DecimalFormatSymbols.DecimalSeparator))
            {
                hasFractionalPart = true;
            }
            else
            {
                hasFractionalPart = false;
            }
        }
    }
}