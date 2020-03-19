using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using AssetTracking.ViewModels;
using Plugin.Geolocator;
using System.Collections.ObjectModel;

namespace AssetTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapDisplayPage : ContentPage
	{
		public MapDisplayPage (double latitude, double longitude, ObservableCollection<Pin> mapPins)
		{
			InitializeComponent ();
            BindingContext = new MapViewModel();
            var vm = (MapViewModel)BindingContext;
            vm.MapCenter = new Position(latitude,longitude);
            //   Map.MapPosition = new Position(latitude, longitude);
            vm.PinCollection = mapPins;
        //    Map.MapPins = mapPins;
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(Map.MapPosition, Distance.FromMiles(.3)));
        }

        async Task ShowCurrentPosition()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
      //     return Task.CompletedTask;
        }
    
    }
}