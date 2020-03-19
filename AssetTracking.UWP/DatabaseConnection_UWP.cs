using SQLite;
using Xamarin.Forms;
using AssetTracking;
using AssetTracking.UWP;
using Windows.Storage;
using System.IO;
using DatabaseConnection.UWP;

[assembly: Dependency(typeof(DatabaseConnection_UWP))]
namespace DatabaseConnection.UWP
{
    public class DatabaseConnection_UWP : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            var dbName = "assetTracking.db3";
            var path = Path.Combine(ApplicationData.
              Current.LocalFolder.Path, dbName);
            return new SQLiteConnection(path);
        }
    }
}