using AssetTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;
using AssetTracking.Model;

namespace AssetTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanHistoryPage : ContentPage
	{
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "assetTracking.db3");
     //   private ListView _listView;

        public ScanHistoryPage ()
		{
			InitializeComponent ();
            BindingContext = new ScanViewModel();
            //var db = new SQLiteConnection(dbPath);
            //StackLayout stackLayout = new StackLayout();
            //_listView = new ListView();
            //_listView.ItemsSource = db.Table<Scan>().OrderBy(s => s.Id).ToList();
            //stackLayout.Children.Add(_listView);

            //Content = stackLayout;
            var vm = (ScanViewModel)BindingContext;
            vm.UpdateHistoryCommand.Execute(null);

        }
    }
}