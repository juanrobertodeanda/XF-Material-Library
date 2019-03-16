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
    }

    public abstract class BaseMainView : BaseView<MainViewModel> { }
}
