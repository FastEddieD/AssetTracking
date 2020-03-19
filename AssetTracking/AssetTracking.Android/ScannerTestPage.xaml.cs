using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AssetTracking.ViewModels;

namespace AssetTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScannerTestPage : ContentPage
	{
		public ScannerTestPage ()
		{
			InitializeComponent ();
            BindingContext = new ScannerTestViewModel();

        //    var vm = (ScannerTestViewModel)BindingContext;
        //    vm.ConnectCommand.Execute(null);
        }
	}
}