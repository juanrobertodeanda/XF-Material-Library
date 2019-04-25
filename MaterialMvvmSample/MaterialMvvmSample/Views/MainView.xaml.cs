using MaterialMvvmSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;

namespace MaterialMvvmSample.Views
{
    public partial class MainView : BaseMainView
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        private void Entry2_Focused(object sender, FocusEventArgs e)
        {
            entry1.Text = "HOLA MUNDO";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            entry1.Text = string.Empty;
        }
    }

    public abstract class BaseMainView : BaseView<MainViewModel> { }
}
