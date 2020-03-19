using AssetTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AssetTracking.Helpers;

namespace AssetTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartupPage : ContentPage
	{
		public StartupPage ()
		{
			InitializeComponent ();
            BindingContext = new DeviceViewModel();
            var vm = (DeviceViewModel)BindingContext;
       //     vm.GetBackgroundFilesCommand.Execute(null);
         

        }
       
        private void StartButton_Clicked(object sender, EventArgs e)
        {
            var vm = (DeviceViewModel)BindingContext;

            if (vm.IsClockRunning)
            {
                vm.ClockInCommand.Execute(null);
            }
            else
            {
                vm.ClockOutCommand.Execute(null);
            }

            // Based on your Mode Selection, This will navigate you to the proper page
            switch (UserSettings.ModeId)
            {
                case "1":
                    Navigation.PushAsync(new ScanActionPage());
                    break;
                case "2":
                    Navigation.PushAsync(new InventoryPage());
                    break;
                case "3":
                    Navigation.PushAsync(new PickListPage());
                    break;
                case "4":
                    Navigation.PushAsync(new ItemLookupPage());
                    break;
                case "5":
                    Navigation.PushAsync(new StatusPage());
                    break;
                case "6":
                    Navigation.PushAsync(new StatusPage());
                    break;
                default:
                    break;
            }
        }

        private void SetupButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }
    }
}