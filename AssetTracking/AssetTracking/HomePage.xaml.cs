using AssetTracking.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AssetTracking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
            BindingContext = new DeviceViewModel();
            var vm = (DeviceViewModel)BindingContext;
            vm.GetBackgroundFilesCommand.Execute(null);
        }

        private void ScannerTest_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScannerTestPage());
        }

        private void Config_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        private void ResumeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScanActionPage());
        }
        private void StartButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StartupPage());
        }
    }
}